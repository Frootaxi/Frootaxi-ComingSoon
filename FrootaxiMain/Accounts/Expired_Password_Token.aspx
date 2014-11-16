<%@ Page Title="" Language="C#" MasterPageFile="~/Accounts/Verify.Master" AutoEventWireup="true" CodeBehind="Expired_Password_Token.aspx.cs" Inherits="FrootaxiMain.Accounts.Expired_Password_Token" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
Expired password token | Frootaxi
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="script" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="promptContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="toRightContent" runat="server">
    <li class="title3 right">Expired Link</li>
    <li>
	    <p>The link in your email is no longer active. It is valid for only 24 hours.</p>
    </li>
    <li class="defaultButtons alt_settings"><a href="Forgot_Password.aspx">Request new password link</a></li>
</asp:Content>
