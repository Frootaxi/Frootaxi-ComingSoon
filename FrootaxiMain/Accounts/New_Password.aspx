<%@ Page Title="" Language="C#" MasterPageFile="~/Accounts/Verify.Master" AutoEventWireup="true" CodeBehind="New_Password.aspx.cs" Inherits="FrootaxiMain.Accounts.New_Password" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
Reset your password | Frootaxi
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="script" runat="server">
<link href="../css/login.css" rel="stylesheet" type="text/css" />
<link href="../css/altLayout.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="promptContent" runat="server">
    <asp:UpdatePanel ID="upError" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblErrorMessage" runat="server" Text="" style="display:none"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="toRightContent" runat="server">
    <li class="title3 right">Reset Password</li>
    <li class="loginInput">
        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
		    <ContentTemplate>
			    <asp:TextBox ID="txtPassword" runat="server" CssClass="textBox" TextMode="Password"
                    placeholder="New Password" title="Please enter your new password" 
                    AutoPostBack="False" ontextchanged="txtPassword_TextChanged"></asp:TextBox>
            </ContentTemplate>
	    </asp:UpdatePanel>
    </li>
    <li class="loginInput last-child">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
		    <ContentTemplate>
			    <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" 
                    CssClass="textBox" placeholder="Confirm Password" AutoPostBack="False" 
                    ontextchanged="txtConfirmPassword_TextChanged"></asp:TextBox>
            </ContentTemplate>
	    </asp:UpdatePanel>
    </li>
    <li>
	    <asp:Button CssClass="button" ID="btnNewPassword" runat="server" Text="Submit" 
            onclick="btnNewPassword_Click" />
    </li>
</asp:Content>
