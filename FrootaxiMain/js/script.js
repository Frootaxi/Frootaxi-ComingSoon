// Enable the visual refresh
//google.maps.visualRefresh = true;

var infoWindowLocation;
var icon_image;
var geocoder;
var map2, journeymap;
var poly, journeypoly;
var path, journeypath;
var journeymarker;
var len;
var styles;
var styledMap;

var allLatLng = [];
var coords;

var pickMeHere;
var directionsDisplay;
var directionsService = new google.maps.DirectionsService();

function MVCArrayBinder(mvcArray) {
    this.array_ = mvcArray;
}
MVCArrayBinder.prototype = new google.maps.MVCObject();
MVCArrayBinder.prototype.get = function (key) {
    if (!isNaN(parseInt(key))) {
        return this.array_.getAt(parseInt(key));
    } else {
        this.array_.get(key);
    }
};
MVCArrayBinder.prototype.set = function (key, val) {
    if (!isNaN(parseInt(key))) {
        this.array_.setAt(parseInt(key), val);
    } else {
        this.array_.set(key, val);
    }
};

function dothis(_map) {
    var pickMeHere = new google.maps.LatLng(5.620043, -0.168228);

    if (!styledMap)
        loadStyle();

    var mapOptions = {
        zoom: 11,
        center: pickMeHere,
        zoomControl: true,
        zoomControlOptions: {
            style: google.maps.ZoomControlStyle.SMALL,
            position: google.maps.ControlPosition.RIGHT_CENTER
        },
        disableDefaultUI: true,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };

    map = new google.maps.Map(document.getElementById(_map), mapOptions);
    map.mapTypes.set('map_style', styledMap);
    map.setMapTypeId('map_style');
}

function initialize(_map, _addListener) {
    pickMeHere = new google.maps.LatLng(5.620043, -0.168228);

    if (!styledMap)
        loadStyle();

    var mapOptions = {
        zoom: 11,
        center: pickMeHere,
        zoomControl: true,
        zoomControlOptions: {
            style: google.maps.ZoomControlStyle.SMALL,
            position: google.maps.ControlPosition.RIGHT_CENTER
        },
        disableDefaultUI: true,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };

    map = new google.maps.Map(document.getElementById(_map), mapOptions);
    map.mapTypes.set('map_style', styledMap);
    map.setMapTypeId('map_style');

    initializeUserJourney();

    geocoder = new google.maps.Geocoder();

    if (_addListener) {
        google.maps.event.addListener(map, 'click', addLatLng);
        google.maps.event.addListener(marker, 'position_changed', function () {
            infoWindow.close();
        });
    }

    map.controls[google.maps.ControlPosition.TOP_LEFT].push(input);
}

function locate_address() {
    var address = document.getElementById('txtSearchAddress');
    
    if (address.value != "") {
        geocoder = new google.maps.Geocoder();
        geocoder.geocode({ 'address': address.value }, function (results, status) {
            if (status == google.maps.GeocoderStatus.OK) {
                var loc = results[0].geometry.location;
                addLatLng2(loc);
            } else {
                alert("Make sure your internet connection is functioning properly.");
                return;
            }
        });
    }
    else {
        alert("Please enter a location.");
        address.focus();
        return;
    }

}

function loadStyle() {
    // Create an array of styles.
    var styles = [{
        stylers: [{
            hue: "#c8880c"
        }, {
            saturation: -20
        }]
    }, {
        featureType: "road",
        elementType: "geometry",
        stylers: [{
            lightness: 0
        }, {
            visibility: "simplified"
        }]
    }, {
        featureType: "water",
        elementType: "geometry.fill",
        stylers: [{
            hue: "#00b2ff"
        }]
    }, {
        featureType: "road",
        elementType: "labels",
        stylers: [{
            visibility: "on"
        }]
    }];

    styledMap = new google.maps.StyledMapType(styles, {
        name: "Styled Map"
    });
}

function initializeUserJourney() {
    if (!pickMeHere)
        pickMeHere = new google.maps.LatLng(5.620043, -0.168228);

    if (!styledMap)
        loadStyle();

    var journeyMapOptions = {
        zoom: 11,
        center: pickMeHere,
        disableDefaultUI: true,
        draggable: false
    };
    journeymap = new google.maps.Map(document.getElementById('map-canvas-snapshot'), journeyMapOptions);
    journeymap.mapTypes.set('map_style', styledMap);
    journeymap.setMapTypeId('map_style');

    journeypoly = new google.maps.Polyline({
        strokeColor: '#cd8c0e',
        strokeOpacity: 1.0,
        strokeWeight: 3
    });
    journeypoly.setMap(journeymap);

    directionsDisplay = new google.maps.DirectionsRenderer({ polylineOptions: journeypoly, suppressMarkers: true });
    directionsDisplay.setMap(journeymap);

    //google.maps.event.trigger(journeymap, 'resize');
}

function calcRoute(_markers, _map) {
    var waypts = [];
    var _start = _markers[0];
    var _stop = _markers[_markers.length - 1];
    
    for (var i = 0; i < _markers.length; i++) {
        if (i > 0 && i < _markers.length) {
            waypts.push({
                location: _markers[i],
                stopover: true
            });
        }
    }

    icon_image = 'images/taxi.png';
    var startMarker = new google.maps.Marker({
        position: _start,
        map: _map,
        draggable: false,
        icon: icon_image
    });

    var stopMarker = new google.maps.Marker({
        position: _stop,
        map: _map,
        draggable: false,
        icon: icon_image
    });

    var request = {
        origin: _start,
        destination: _stop,
        waypoints: waypts,
        optimizeWaypoints: true,
        travelMode: google.maps.TravelMode.DRIVING
    };
    directionsService.route(request, function (response, status) {
        if (status == google.maps.DirectionsStatus.OK) {
            directionsDisplay.setDirections(response);
            var route = response.routes[0];
            for (var i = 0; i < _markers.length; i++) {
                if (_markers[i] === _start) {
                    addInfoWindow('Pick up', _map, setMarker(_markers[i], _map));
                }
                else if (_markers[i] === _stop) {
                    addInfoWindow('Drop off', _map, setMarker(_markers[i], _map));
                }
            }

            /*

            var summaryPanel = document.getElementById('directions_panel');
            summaryPanel.innerHTML = '';
            // For each route, display summary information.
            for (var i = 0; i < route.legs.length; i++) {
                var routeSegment = i + 1;
                summaryPanel.innerHTML += '<b>Route Segment: ' + routeSegment + '</b><br>';
                summaryPanel.innerHTML += route.legs[i].start_address + ' to ';
                summaryPanel.innerHTML += route.legs[i].end_address + '<br>';
                summaryPanel.innerHTML += route.legs[i].distance.text + '<br><br>';
            }*/
        }
    });
}

function mapTrip() {
    pickMeHere = new google.maps.LatLng(5.620043, -0.168228);
    icon_image = 'images/taxi.gif';

    setMarker(pickMeHere, map).setOptions({ optimized: false });
}

function setMarker(_latlng, _map) {
    var marker = new google.maps.Marker({
        //animation: google.maps.Animation.DROP,
        position: _latlng,
        draggable: false,
        icon: icon_image,
        map: _map
    });

    return marker;
}

function initializePAYGMap() {
    directionsService = new google.maps.DirectionsService();

    pickMeHere = new google.maps.LatLng(5.620043, -0.168228);
    loadStyle();

    var mapOptions = {
        zoom: 11,
        center: pickMeHere,
        draggable: false,
        disableDefaultUI: true
    };
    map2 = new google.maps.Map(document.getElementById('map-canvas-2'), mapOptions);
    map2.mapTypes.set('map_style', styledMap);
    map2.setMapTypeId('map_style');

    journeypath = new google.maps.Polyline({
        strokeColor: '#cd8c0e',
        strokeOpacity: 1.0,
        strokeWeight: 3
    });
    journeypath.setMap(map2);

    directionsDisplay = new google.maps.DirectionsRenderer({ polylineOptions: journeypath, map:map2, suppressMarkers: true });

    loadCoords();
    calcRoute(allLatLng, map2);

    for (var i = 0; i < markers.length; i++) {
        if (i > 0 && i < markers.length - 1) {
            var location = markers[i];

            journeymarker = new google.maps.Marker({
                position: location,
                draggable: false,
                icon: icon_image,
                map: journeymap
            });
        }
    }
}

function addLatLng2(latlng) {
    if (!polyOptions) {
        polyOptions = {
            strokeColor: '#cd8c0e',
            strokeOpacity: 1.0,
            strokeWeight: 3,
            editable: false
        };
        poly = new google.maps.Polyline(polyOptions);
        poly.binder = new MVCArrayBinder(poly.getPath());
        poly.setMap(map);

        icon_image = 'images/taxi.png';
        addPolyline(latlng);

        addMarker(latlng);
        addInfoWindow('Pick me here!', map, marker);
    } else {
        addPolyline(latlng);

        addMarker(latlng);
        map.fitBounds(getBoundsForPoly(poly));
    }

    markers.push(latlng);
    document.getElementById('toRightContent_txtTripCoordinates').value = markers;
}

function addLatLng(event) {
    if (!polyOptions) {
        polyOptions = {
            strokeColor: '#cd8c0e',
            strokeOpacity: 1.0,
            strokeWeight: 3,
            editable: false
        };
        poly = new google.maps.Polyline(polyOptions);
        poly.binder = new MVCArrayBinder(poly.getPath());
        poly.setMap(map);

        icon_image = 'images/taxi.png';
        addPolyline(event.latLng);

        addMarker(event.latLng);
        addInfoWindow('Pick me here!', map, marker);
    } else {
        addPolyline(event.latLng);

        addMarker(event.latLng);
        map.fitBounds(getBoundsForPoly(poly));
    }

    markers.push(event.latLng);
    document.getElementById('toRightContent_txtTripCoordinates').value = markers;
}

function addPolyline(event) {
    path = poly.getPath();
    path.push(event);
}

function addMarker(event) {
    len = path.getLength();

    marker = new google.maps.Marker({
        animation: google.maps.Animation.DROP,
        position: event,
        draggable: false,
        icon: icon_image,
        map: map
    });
    marker.bindTo('position', poly.binder, (len - 1).toString());
}

function getBoundsForPoly(poly) {
    var bounds = new google.maps.LatLngBounds();
    poly.getPath().forEach(function (latLng) {
        bounds.extend(latLng);
    });
    return bounds;
}

function addInfoWindow(text, _map, _marker) {
    _marker.infowindow = new google.maps.InfoWindow({
        content: text,
        maxWidth: 200
    });
    _marker.infowindow.open(_map, _marker);
    infoWindowLocation = _marker.getPosition();
}

function loadCoords() {
    coords = document.getElementById('toRightContent_txtTripCoordinates').value;
    var array = coords.split('),(');

    var arr2;
    var arr;

    for (var i = 0; i < array.length; i++) {
        if (i === 0) {
            arr = array[i].split('(');
            arr2 = arr[1].split(',');

            allLatLng.push(new google.maps.LatLng(arr2[0], arr2[1]));
        } else if (i === array.length - 1) {
            arr = array[i].split(')');
            arr2 = arr[0].split(',');

            allLatLng.push(new google.maps.LatLng(arr2[0], arr2[1]));
        } else {
            arr2 = array[i].split(',');

            allLatLng.push(new google.maps.LatLng(arr2[0], arr2[1]));
        }
    }
}

function addUserJourney() {
    calcRoute(markers, journeymap);
    google.maps.event.trigger(journeymap, 'resize');

    journeymap.setCenter(getBoundsForPoly(journeypoly).getCenter());
    journeymap.fitBounds(getBoundsForPoly(journeypoly));

    for (var i = 0; i < markers.length; i++) {
        if (i > 0 && i < markers.length - 1) {
            var location = markers[i];

            journeymarker = new google.maps.Marker({
                position: location,
                draggable: false,
                icon: icon_image,
                map: journeymap
            });
        }
    }
}

/*function addPaygJourney() {
    google.maps.event.trigger(map2, 'resize');

    map2.setCenter(getBoundsForPoly(journeypath).getCenter());
    map2.fitBounds(getBoundsForPoly(journeypath));

    for (i = 0; i < allLatLng.length; i++) {
        if (i > 0 && i < allLatLng.length - 1) {
            //setMarker(allLatLng[i], map2);
            var location = allLatLng[i];

            var marker = new google.maps.Marker({
                position: location,
                draggable: false,
                icon: icon_image,
                map: map2
            });
        }
    }
}*/

function getJourneyBounds() {
    bounds = new google.maps.LatLngBounds();
    for (var i = 0; i < markers.length; i++) {
        bounds.extend();
    }
    return bounds;
}

function unloadPopupBox() { // To Unload the Popupbox
    document.getElementById('payg_details').className = 'hide';
    document.getElementById('divCover').className = '';
    document.getElementById('txtCost').className = "cu_input cu_alt msg";
}

function fadeEffect() {
    $('#divCover').fadeOut('slow');
    $('#divErrorTop').fadeOut('slow');
    $('#lblErrorMsg').fadeOut('slow');
}

function fadeEffectTimeOut() {
    setTimeout(fadeEffect, 6000);
}

// Deletes all markers in the array by removing references to them.
function clearMaps() {
    clearMarkers();
    markers = [];

    removeLine();
}

function clearMarkers() {
    for (var i = 0; i < markers.length; i++) {
        markers[i].setMap(null);
    }
    markers = [];
}

function removeLine() {
    poly.setMap(null);
    poly = null;

    polyOptions = {
        strokeColor: '#cd8c0e',
        strokeOpacity: 1.0,
        strokeWeight: 3,
        editable: false
    };
    poly = new google.maps.Polyline(polyOptions);
    poly.binder = new MVCArrayBinder(poly.getPath());
    poly.setMap(map);
    poly.setMap(journeymap);
}