<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="administrator_panel._default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Trip Costing | Frootaxi</title>
    <link rel="Stylesheet" type="text/css" href="css/costing.css" />
    <link href="css/typography.css" rel="stylesheet" type="text/css" />
    <link href="http://code.jquery.com/ui/1.10.3/themes/vader/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script src="http://code.jquery.com/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="http://code.jquery.com/ui/1.11.1/jquery-ui.js" type="text/javascript"></script>
    <script src='https://cdn.firebase.com/js/client/1.0.15/firebase.js'></script>
    <script type="text/javascript">
        var x = 0;

        function hideErrorMsg() {
            setTimeout(unloadErrorMsg, 5000);
        }

        function unloadErrorMsg() {
            var x = document.getElementById('<%=lblErrorMessage.ClientID%>');
            var id = '#' + x;

            $(id).fadeOut('slow');
            //document.getElementById('lblErrorMessage').className = '';

            x.innerHTML = "";
        }

        function clearEntries() {
            var u = document.getElementById("txtUuid");
            var p = document.getElementById("txtTripPrice");
            var d = document.getElementById("txtDatetimestamp");
            var j = document.getElementById("txtJourneyDetails");

            u.value = "";
            p.value = "";
            d.value = "";
            j.value = "";
        }

        $(document).ready(function () {
            var x = document.getElementById("lblTotalRequestCount");
            $('#lblTotalRequestCount').change(function () {
                $('#lblTotalRequestCount').fadeIn("slow");
            });
        });
    </script>
</head>
<body>
    <form id="frmCosting" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
        <div class="background">
            <asp:UpdatePanel ID="upScriptLoader" runat="server"></asp:UpdatePanel>
            <asp:UpdatePanel ID="upErrorMessage" runat="server">
            <ContentTemplate>
                <asp:Label ID="lblErrorMessage" runat="server" Text=""></asp:Label>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="topcon">
			<div id="logo">
			</div><!-- 100% color all the way across padding:50px 55px; -->
            <div id="subscriptionPanel">
				<fieldset><legend></legend>
					<ol>
						<li>Please enter the cost of the trip below</li>
					</ol>
				</fieldset>
			</div>
		</div>
	</div>
	<div class="bottomcon text_color textShadow">
        <fieldset class="entry_form ui-corner">
            <legend></legend>
            <ul>
                <li>
                    <label id="lblTripId" >Trip Id:</label><input id="txtUuid" type="text" /><asp:TextBox ID="txtTransactionId"
            runat="server" CssClass="ui-corner hide"></asp:TextBox>
                </li>
                <li>
                    <label id="lblTripPrice" >Price of trip: <span class="currency">GHC</span></label><asp:TextBox ID="txtTripPrice"
            runat="server"></asp:TextBox>
                </li>
                <li>
                    <input id="txtDatetimestamp" type="text" class="hide" />
                    <input id="txtJourneyDetails" type="text" class="hide" />
                </li>
                <li>
                    <button id="btnAdd" class="btn display" >Add Price</button>
                </li>
            </ul>
        </fieldset>
        <br /><br />
        <asp:UpdatePanel ID="upLatestRequests" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div class="requestCount">Total requests: <asp:Label ID="lblTotalRequestCount" runat="server" Text="0" CssClass="count"></asp:Label></div>
                <br /><br />
                <div class="gridView">
                    <asp:Label ID="lblNoRequests" runat="server" Text="" CssClass="hide"></asp:Label>
                    <table id="gdvLatestRequests" cellspacing="5" cellpadding="5" border="1" rules="all">
                        <tbody>
                            <tr>
                                <th scope="col">Trid ID</th>
                                <th scope="col">Journey Details</th>
                                <th scope="col">Cost</th>
                                <th scope="col">Request Time</th>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <script>
                    var myDataRef = new Firebase('https://frootaxi.firebaseio.com/');
                    var table = document.getElementById("gdvLatestRequests");
                    var requestCount = 0;

                    $('#btnAdd').click(function () {
                        var cells = table.rows.cells;

                        var uuid = $('#txtUuid').val();
                        var cost = $('#txtTripPrice').val();
                        var dts = $('#txtDatetimestamp').val();
                        var text = $('#txtJourneyDetails').val();
                        myDataRef.child(uuid).set({ user_id: uuid, cost: cost, datetimestamp: dts, message: text });

                        clearEntries();
                    });
                    myDataRef.on('child_added', function (snapshot) {
                        var message = snapshot.val();

                        AddRequest(message.user_id, message.message, message.datetimestamp, message.cost);
                        
                    });
                    function AddRequest(u_id, msg, dts, price) {
                        var cost = parseFloat(price);
                        var x = document.getElementById("lblTotalRequestCount");

                        if (cost === 0) {
                            requestCount += 1;
                            x.innerHTML = requestCount;

                            var rowCount = table.rows.length;

                            var row = table.insertRow(rowCount);
                            var id = row.insertCell(0);
                            var jd = row.insertCell(1);
                            var ct = row.insertCell(2);
                            var rt = row.insertCell(3);

                            id.innerHTML = u_id;
                            jd.innerHTML = msg;
                            ct.innerHTML = cost;
                            rt.innerHTML = dts;

                            addStyles();
                        }
                    };

                    function addStyles() {
                        var rows = table.rows;
                        for (i = 1; i <= table.rows.length; i++) {
                            rows[i].onmouseover = function () {
                                this.className = "hovered";
                            }

                            rows[i].onmouseout = function () {
                                this.className = "";
                            }

                            rows[i].onclick = function () {
                                if (this.parentNode.nodeName == 'THEAD')
                                    return;

                                var cells = this.cells;

                                var u = document.getElementById("txtUuid");
                                var p = document.getElementById("txtTripPrice");
                                var d = document.getElementById("txtDatetimestamp");
                                var j = document.getElementById("txtJourneyDetails");

                                u.value = cells[0].innerHTML;
                                j.value = cells[1].innerHTML;
                                p.value = cells[2].innerHTML;
                                d.value = cells[0].innerHTML;

                                u.focus();
                            };
                        };
                    }
                </script>
            </ContentTemplate>
        </asp:UpdatePanel>
        <br /><br /><br /><br />
	</div>
    </form>
</body>
</html>
