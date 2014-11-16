<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="driver_registration_form.aspx.cs" Inherits="FrootaxiMain.driver_registration_form" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="Stylesheet" type="text/css" href="css/costing.css" />
    <link href="css/typography.css" rel="stylesheet" type="text/css" />
    <link href="http://code.jquery.com/ui/1.10.3/themes/vader/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script src="http://code.jquery.com/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="http://code.jquery.com/ui/1.11.1/jquery-ui.js" type="text/javascript"></script>
</head>
<body>
    <form id="frmDriverRegistration" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
        <div class="background">
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
                    <label for="txtFName" >First Name: </label><asp:TextBox ID="txtFName"
            runat="server" CssClass="ui-corner"></asp:TextBox>
                </li>
                <li>
                    <label for="txtLName" >Last Name: </label><asp:TextBox ID="txtTripPrice"
            runat="server"></asp:TextBox>
                </li>
                <li>
                    <asp:Button ID="btnAddDriver" runat="server" Text="Add Driver" 
            onclick="btnAddDriver_Click" CssClass="btn display" />
                </li>
            </ul>
        </fieldset>
        <br /><br /><br /><br />
	</div>
    </form>
</body>
</html>
