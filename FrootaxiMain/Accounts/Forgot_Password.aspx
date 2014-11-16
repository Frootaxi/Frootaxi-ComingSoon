<%@ Page Title="" Language="C#" MasterPageFile="~/Accounts/Verify.Master" AutoEventWireup="true" CodeBehind="Forgot_Password.aspx.cs" Inherits="FrootaxiMain.Accounts.Forgot_Password" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
Forgot your password? | Frootaxi
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="script" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="promptContent" runat="server">
    <asp:UpdatePanel ID="upErrorMessage" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblErrorMessage" runat="server" Text="" style="display:none"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="toRightContent" runat="server">
    <li class="title3 right">Forgot your password?</li>
    <li>
	    <p>Don't worry we've been there. Enter your email address to restore your access.</p>
    </li>
    <li class="loginInput">
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
		    <ContentTemplate>
                <asp:TextBox CssClass="textBox" ID="txtEmailAddress" runat="server" placeholder="email address" AutoPostBack="False"></asp:TextBox>
            </ContentTemplate>
	    </asp:UpdatePanel>
    </li>
    <li class="loginInput">
        <asp:Button CssClass="button" ID="btnRequest" runat="server" Text="Request" 
            onclick="btnRequest_Click" />
    </li>
</asp:Content>
