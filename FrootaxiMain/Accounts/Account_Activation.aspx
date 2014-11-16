<%@ Page Title="" Language="C#" MasterPageFile="~/Accounts/Account.master" AutoEventWireup="true" CodeBehind="Account_Activation.aspx.cs" Inherits="FrootaxiMain.Accounts.Account_Activation" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
Activate your account | Frootaxi
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="script" runat="server">
    <link href="../css/rightSide.OneColumn.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="promptContent" runat="server">
    <asp:UpdatePanel ID="upActivateAccount" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblActivateAccount" runat="server" Text=""></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="loginStatus1" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="toRightContent" runat="server">
    <ul class="middleContent">
        <li class="defaultButtons"><a href="http://beta.frootaxi.com/HailACab.aspx">Hail a Frootaxi Cab</a></li>
	    <li class="defaultButtons"><a href="#">Become a Frootaxi Powered Fleet Company</a></li>
    </ul>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="loginStatus2" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="rightSideContent" runat="server">
    <fieldset id="content">
	    <ul>
            <li class="title padding right">Activate your Account</li>
            <li class="style">To fully activate your account you need to verify your email address through the email sent to you and your mobile number through the SMS activation code here.</li>
		    <li class="style">It is important to complete the activation process as we will communicate with you through both these channels.</li>
            <li>&nbsp;</li>
            <li class="style">Enter the activation code sent to you below.</li>
		    <li>
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
		            <ContentTemplate>
                        <asp:TextBox CssClass="textBox" ID="txtActivationCode" runat="server" placeholder="SMS Activation Code" AutoPostBack="true"></asp:TextBox>
                    </ContentTemplate>
		        </asp:UpdatePanel>
            </li>
            <li>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:Button ID="btnActivateAccount" runat="server" CssClass="button" Text="Activate your account" 
                            onclick="btnActivateAccount_Click" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </li>
            <li class="style">If you have not received your activation code, click the button below to resend.</li>
            <li>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:Button ID="btnResendActivationCode" runat="server" CssClass="button"
                                    Text="Send activation code again" onclick="btnResendActivationCode_Click" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </li>
        </ul>
    </fieldset>
    <fieldset id="submitButtonStyle">
	    <ul>
		    <li id="footer"><p>Copyright &copy; 2013  Frootaxi , All rights reserved.</p></li>
	    </ul>
    </fieldset>
</asp:Content>