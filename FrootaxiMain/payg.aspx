<%@ Page Title="" Language="C#" MasterPageFile="~/Template.Master" AutoEventWireup="true" CodeBehind="payg.aspx.cs" Inherits="FrootaxiMain.payg" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
Payment Options | Frootaxi
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="script" runat="server">
<script src="https://maps.googleapis.com/maps/api/js?v=3.exp&sensor=true" type="text/javascript"></script>
    <!--Accordion-->
    <link rel="stylesheet" href="css/jquery-ui.css"/>
     <script type="text/javascript">
         jQ(function ($) {
             $("#accordion").accordion({
                 collapsible: true,
                 active: 'none'
             });
         });

         jQ(document).ready(function () {
             initializePAYGMap();
         });

         function unloadActivationRequestInfo() {
             document.getElementById("activate").className = "close";
         }
    </script>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="loginStatus1" runat="server">
    <asp:UpdatePanel ID="upErrorMessage" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblErrorMessage" runat="server" Text=""></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
    <li>
        <asp:LoginStatus ID="LoginStatus2" runat="server" LoginText="Sign in" 
            LogoutAction="Redirect" LogoutPageUrl="~/Accounts/Login.aspx" 
            LogoutText="Sign Out" onloggingout="LoginStatus1_LoggingOut" />
    </li>
	<li id="registerLinkTop"><a href="Accounts/Register.aspx">Register</a></li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="toRightContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
        <ContentTemplate>
            <input id="txtTripCoordinates" type="text" runat="server" class="hide" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="panel">
        <div id="tripSummary">
            <ul class="billing_dets">
                <li><label class="freeText" for="txtCost">Cost: </label>
                <asp:TextBox ID="txtCost" runat="server" Text="GHC 0.00" CssClass="trip_confirmation_text freeText"></asp:TextBox></li>
                <li><label class="freeText" for="tripDistance">Trip type: </label><asp:TextBox ID="txtTripType" runat="server" Text="RoundTrip" CssClass="trip_confirmation_text freeText"></asp:TextBox></li>
                <li><label class="freeText" for="tripDistance">Duration: </label><asp:TextBox ID="txtDuration" runat="server" Text="" CssClass="trip_confirmation_text freeText"></asp:TextBox></li>
                <li><label class="freeText" for="tripDistance">Pick-up time: </label><asp:TextBox ID="txtPickTime" runat="server" Text="7:30pm" CssClass="trip_confirmation_text freeText"></asp:TextBox></li>
                <!--<li><label for="tripDistance">Distance: </label><input id="txtTripDistance" type="text" runat="server" /></li>-->
            </ul>
        </div>
    </div>
    <div id="map-canvas-2"></div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="loginStatus2" runat="server">
    <li>
        <asp:LoginStatus ID="LoginStatus1" runat="server" LoginText="Sign in" 
            LogoutAction="Redirect" LogoutPageUrl="~/Accounts/Login.aspx" 
            LogoutText="Sign Out" onloggingout="LoginStatus1_LoggingOut" />
    </li>
	<li id="registerLinkBottom"><a href="Accounts/Register.aspx">Register</a></li>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="rightSideContent" runat="server">
	<fieldset>
        <legend></legend>
		<ul id="activate">
			<li class="title padding right">Create your account</li>
            <li class="style">Finally, we will need your mobile number and email address to send you feedback throughout your journey and provide a copy of your receipt.</li>
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
		        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
		            <ContentTemplate>
		                <asp:TextBox ID="txtPassword" placeholder="Password" 
                            CssClass="textBox" runat="server" TextMode="Password"></asp:TextBox>
		            </ContentTemplate>
		        </asp:UpdatePanel>
			</li>
            <li>
		        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
		            <ContentTemplate>
		                <asp:TextBox ID="txtConfirmPassword" placeholder="Confirm your password" 
                            CssClass="textBox" runat="server" TextMode="Password"></asp:TextBox>
		            </ContentTemplate>
		        </asp:UpdatePanel>
			</li>
        </ul>
    </fieldset>
    <fieldset>
        <legend></legend>
        <ul>
            <li class="title padding right">Billing Information</li>
            <li id="accordion">
                <h3>Pay with cash</h3>
                <div>
                    <br />
                    <p class="radioButton"><asp:RadioButton ID="rbtnCash" runat="server" GroupName="paymentOption" CssClass="radio" />Select this option</p>
                    <img src="../images/cash_story_board.png" id="Img1" alt="Instructions for paying with cash" />
                </div>
                <h3>Electronic payment</h3>
                <div>
                    <br />
                    <ul>
                        <li><p class="radioButton"><asp:RadioButton ID="rbtnMPower" runat="server" GroupName="paymentOption" CssClass="radio" />Select this option</p></li>
                        <li class="title padding right">Pay with MPower</li>
                        <li>
                            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
		                        <ContentTemplate>		                    
		                            <asp:TextBox CssClass="textBox account" placeholder="MPower Username/Mobile Number" ID="txtMPowerAccountNumber" 
                                        runat="server"></asp:TextBox>
                                </ContentTemplate>
		                    </asp:UpdatePanel>
                        </li>
                        <li class="style textLink adjustMargin" id="Li1"><a href="https://app.mpowerpayments.com/signup" target="_blank">I don't have an MPower account</a></li>
                        <li><p class="radioButton"><asp:RadioButton ID="rbtnExpressPay" runat="server" GroupName="paymentOption" CssClass="radio" />Select this option</p></li>
                        <li class="title padding right">Pay with Expresspay</li> 
                        <li><a href="http://sandbox.expresspaygh.com/api/submit.php" id="btnNow" class="button big" type="button">Expresspay</a></li>
                    </ul>
                </div>
            </li>
		</ul>
	</fieldset>
	<fieldset class="last-child">
        <legend></legend>
		<ul>
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
		<ul>
			<li>
                <asp:UpdatePanel ID="upButtonTrigger" runat="server">
                    <ContentTemplate>
                        <asp:Button CssClass="button" ID="btnSubmit" runat="server" Text="Confirm Payment" 
                            onclick="btnSubmit_Click" OnClientClick="this.disabled = true; this.value = 'Submitting...';"   UseSubmitBehavior="false" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </li>
			<li id="footer"><p>Copyright &copy; 2013  Frootaxi , All rights reserved.</p></li>
		</ul>
	</fieldset>
</asp:Content>