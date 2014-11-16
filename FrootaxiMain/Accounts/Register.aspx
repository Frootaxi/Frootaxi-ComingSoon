<%@ Page Title="" Language="C#" MasterPageFile="~/Accounts/Account.master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="FrootaxiMain.Accounts.Register" %>
<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="server">
Register | Frootaxi
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="script" runat="server">
    <script type="text/javascript">
        var x;

        function displayHelpTip() {
            jQ("#packages").fadeIn("fast");
        }

        function hideHelpTip(_div) {
            jQ("#packages").fadeOut("fast");
        }

        function unloadAllPaymentTypes() {
            jQ('#divMPower').hide();
            jQ('#divExpressPay').hide();
        }

        function loadMPowerDiv() {
            unloadAllPaymentTypes();
            jQ('#divMPower').fadeIn(600);
        }

        function loadExpressPayDiv() {
            unloadAllPaymentTypes();
            jQ('#divExpressPay').fadeIn(600);
        }

        function getUserLocation() {
            x = document.getElementById('<%=txtUserLocation.ClientID%>');

            if (navigator.geolocation) {
                navigator.geolocation.getCurrentPosition(showPosition);
            } else {
                x.innerHTML = "Geolocation is not supported by this browser.";
            }
        }

        function showPosition(position) {
            x.innerHTML = "Latitude: " + position.coords.latitude +
            "&nbsp;Longitude: " + position.coords.longitude;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="promptContent" runat="server">
    <asp:UpdatePanel ID="upErrorMessage" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblErrorMessage" runat="server" Text=""></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="upModalDialog" style="display:none" runat="server">
	    <ContentTemplate>
	        <div id='errorDialog' title = "Confirm Registration?">
	            <p class="modalDialogText">
	                <span class="ui-icon ui-icon-alert" style="float:left; margin:0 7px 50px 0;"></span>
	                <asp:Label ID="lblErrorMessage1" runat="server" Text=""></asp:Label>
	            </p>
	        </div>
	        <asp:Button ID="btnSave" runat="server" Text="" style="display:none" 
	            onclick="btnSave_Click" />
	    </ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="loginStatus1" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="toRightContent" runat="server">
    <ul class="middleContent">
        <li class="defaultButtons"><a href="http://beta.frootaxi.com/HailACab.aspx">Hail a Frootaxi Cab</a></li>
	    <li class="defaultButtons"><a href="#">Become a Frootaxi Powered Fleet Company</a></li>
    </ul>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="loginStatus2" runat="server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="rightSideContent" runat="server">
    <fieldset>
        <legend></legend>
		<ul>
			<li class="title padding right">Register</li>
			<li>
                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
		            <ContentTemplate>
		                <asp:TextBox ID="txtFirstName" placeholder="First Name" CssClass="textBox" 
		                        runat="server"></asp:TextBox>
                    </ContentTemplate>
		        </asp:UpdatePanel>
			</li>
			<li>
		        <asp:TextBox ID="txtLastName" CssClass="textBox" placeholder="Surname" 
                    runat="server"></asp:TextBox>
			</li>
			<li>
		        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
		            <ContentTemplate>
		                <asp:TextBox ID="txtEmailAddress" runat="server" CssClass="textBox" 
                            placeholder="Email"></asp:TextBox>
		            </ContentTemplate>
		        </asp:UpdatePanel>
			</li>
			<li>
		        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
		            <ContentTemplate>
		                <asp:TextBox ID="txtMobileNumber" placeholder="Mobile Number" 
                            CssClass="textBox" runat="server"></asp:TextBox>
		            </ContentTemplate>
		        </asp:UpdatePanel>
			</li>
			<li>
		        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
		            <ContentTemplate>
		                <asp:TextBox ID="txtPassword" CssClass="textBox" placeholder="Password" 
                            runat="server" TextMode="Password"></asp:TextBox>
		            </ContentTemplate>
		        </asp:UpdatePanel>
			</li>
			<li>	                
		        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
		            <ContentTemplate>
		                <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="textBox" 
                            placeholder="Confirm Password" TextMode="Password"></asp:TextBox>
		            </ContentTemplate>
		        </asp:UpdatePanel>
			</li>
			<!--<li>
                <asp:UpdatePanel ID="upRegion" runat="server">
		            <ContentTemplate>
                        <asp:DropDownList CssClass="dropdownmenu" ID="ddlRegion" runat="server" 
		                    DataTextField="name_capitalized" DataValueField="id">
		                </asp:DropDownList>
                    </ContentTemplate>
		        </asp:UpdatePanel>
			</li>-->
            <li class="title space">My Location</li>
            <li class="style no-space">
                Please provide your current location by clicking the button below.
            </li>
            <li>
                <asp:UpdatePanel ID="upLocation" runat="server">
		            <ContentTemplate>
                        <asp:TextBox ID="txtUserLocation" runat="server" CssClass="textBox" placeholder="My location..."></asp:TextBox>
                    </ContentTemplate>
		        </asp:UpdatePanel>
            </li>
            <li>
                <button type="button" onclick=" getUserLocation();" class="button">Show my location</button>
            </li>
		</ul>
	</fieldset>
	<fieldset>
        <legend></legend>
		<ul id="packages" class="ui-corner-all">
            <li>
            	<p class="title">BRONZE</p><span class="bubbleTip"></span>
            	<p class="price">GHC50.00</p>
            	<p class="description">The Bronze package includes <span class="freeText">1 Free </span>trip costing GHC10.00</p>
            </li>
            <li>
            	<p class="title">GOLD</p>
            	<p class="price">GHC80.00</p>
            	<p class="description">You get <span class="freeText">2 Free </span>trips costing GHC10.00</p>
            </li>
			<li>
            	<p class="title">DIAMOND</p>
            	<p class="price">GHC110.00</p>
            	<p class="description">The Diamond package gives you <span class="freeText">3 Free </span>trips costing GHC10.00</p><span class="container_close" onclick="hideHelpTip();">x</span>
            </li>
        </ul>
	</fieldset>
	<fieldset>
        <legend></legend>
		<ul id="packages2">
            <li>
            	<p class="title">BRONZE</p>
            	<p class="price">GHC50.00</p>
            	<p class="description">The Bronze package includes <span class="freeText">1 Free </span>trip costing GHC10.00</p>
            </li>
            <li>
            	<p class="title">GOLD</p>
            	<p class="price">GH80.00</p>
            	<p class="description">You get <span class="freeText">2 Free </span>trips costing GHC10.00</p>
            </li>
			<li>
            	<p class="title">DIAMOND</p>
            	<p class="price">GHC110.00</p>
            	<p class="description">The Diamond package gives you <span class="freeText">3 Free </span>trips costing GHC10.00</p>
            </li>
        </ul>
	</fieldset>
	<fieldset class="last-child">
        <legend></legend>
		<ul>
			<li class="title padding">Billing Information</li>
			<li>
                <asp:UpdatePanel ID="upPaymentPackage" runat="server">
		            <ContentTemplate>
	                    <asp:DropDownList CssClass="dropdownmenu" ID="ddlPaymentPackage" runat="server" DataTextField="name_capitalized" 
		                    DataValueField="id">
		                </asp:DropDownList><span class="help" onclick="displayHelpTip();">[?]</span>
                    </ContentTemplate>
		        </asp:UpdatePanel>
            </li>
            <li>
                <asp:UpdatePanel ID="upPaymentType" runat="server" UpdateMode="Always">
		            <ContentTemplate>
	                    <asp:DropDownList CssClass="dropdownmenu" ID="ddlPaymentType" runat="server" DataTextField="type_capitalized" 
		                    DataValueField="id" AutoPostBack="True" OnSelectedIndexChanged="ddlPaymentType_SelectedIndexChanged">
		                </asp:DropDownList>
                    </ContentTemplate>
		        </asp:UpdatePanel>
            </li>
            <li id="divMPower">
                <ul>
                    <li class="title padding">Pay with MPower</li>
                    <li>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
		                    <ContentTemplate>		                    
		                        <asp:TextBox CssClass="textBox account" placeholder="MPower Username/Mobile Number" ID="txtAccountNumber" 
                                    runat="server"></asp:TextBox>
                            </ContentTemplate>
		                </asp:UpdatePanel>
			        </li>
			        <li class="style textLink" id="mpowerLink"><a href="https://app.mpowerpayments.com/signup" target="_blank">I don't have an MPower account</a></li>
                </ul>
            </li>
            <li id="divExpressPay">
                <ul>
                    <li class="title padding">Pay with ExpressPay</li>
                    <li><a href="http://sandbox.expresspaygh.com/api/submit.php" id="btnNow" class="button big" type="button">Expresspay</a></li>
                </ul>
            </li>
			<li class="style">If you have a promotion code enter it below to claim it</li>
			<li>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
		            <ContentTemplate>
                        <asp:TextBox CssClass="textBox" ID="txtPromotionCode" runat="server"
                                placeholder="I have a promotion code"></asp:TextBox>
                    </ContentTemplate>
		        </asp:UpdatePanel>
            </li>
		</ul>
	</fieldset>
	<fieldset id="submitButtonStyle">
        <legend></legend>
		<ul>
			<li><p>By clicking register, I accept the <a href="../Information/Terms.aspx" id="terms">terms</a> of the Frootaxi Service.</p></li>
			<li>
                <asp:UpdatePanel ID="upButtonTrigger" runat="server">
                    <ContentTemplate>
                        <asp:Button CssClass="button" ID="btnSubmit" runat="server" Text="Register" 
                            onclick="btnSubmit_Click" OnClientClick="this.disabled = true; this.value = 'Submitting...';"   UseSubmitBehavior="false" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </li>
			<li id="footer"><p>Copyright &copy; 2013  Frootaxi , All rights reserved.</p></li>
		</ul>
	</fieldset>
</asp:Content>