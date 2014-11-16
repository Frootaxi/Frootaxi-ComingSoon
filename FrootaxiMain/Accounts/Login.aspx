<%@ Page Title="" Language="C#" MasterPageFile="~/Accounts/Account.master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="FrootaxiMain.Accounts.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
Sign in | Frootaxi
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="script" runat="server">
<link href="../css/login.css" rel="stylesheet" type="text/css" />
<link href="../css/landingPage.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="promptContent" runat="server">
    <asp:UpdatePanel ID="upLogin" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblLoginError" runat="server" Text=""></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="loginStatus1" runat="server">
    <li><p>Don't have an account? Click here to <a href="Register.aspx">register</a></p></li>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="toRightContent" runat="server">
    <ul class="middleContent">
        <li class="title3 right">Sign in</li>
	    <li class="loginInput">
		    <asp:TextBox ID="txtUsername" runat="server" CssClass="textBox" placeholder="username" title="Enter an email address or mobile number"></asp:TextBox>
	    </li>
	    <li class="loginInput last-child">
		    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="textBox" placeholder="password"></asp:TextBox>
	    </li>
	    <li class="loginInput">
		    <p><a href="Forgot_Password.aspx" data-rel="dialog">forgot your password?</a></p>
            <asp:Button CssClass="button" ID="btnLogin" runat="server" Text="Sign in" 
                onclick="btnLogin_Click" />
	    </li>
    </ul>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="loginStatus2" runat="server">
    <li><p>Don't have an account? Click here to <a href="Register.aspx">register</a></p></li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="rightSideContent" runat="server">
    <div id="serviceDetails">
	    <fieldset class="padding2">
            <legend></legend>
		    <ul>
			    <li class="title padding"><span class="imgContainer lpi1"></span>Our quality promise</li>
			    <li>
				    <p class="landingPageText">
					    We provide clean, fairly new to brand new taxi cabs. All Frootaxi drivers hold valid licenses and are vetted as added measure for your safety. You are always assured a comfortable and safe ride to your destination.
				    </p>
			    </li>
		    </ul>
	    </fieldset>
	    <fieldset class="padding2">
            <legend></legend>
		    <ul>
                <li class="title padding"><span class="imgContainer  lpi2"></span>Hail a taxi conveniently</li>
                <li>
            	    <p class="landingPageText">
            		    Request for a taxi from the comfort of your home, office or anywhere in town. At a click or tap of a button we will deliver a taxi to you within minutes. 
            	    </p>
                </li>
            </ul>
	    </fieldset>
	    <fieldset class="last-child">
            <legend></legend>
		    <ul>
                <li class="title padding"><span class="imgContainer  lpi3"></span>You pay the right price</li>
                <li>
            	    <p class="landingPageText">
            		    We have daily rates as well as metered charging for journeys without a specified destination. We accept cash as well as all forms of electronic payment including credit/debit cards and mobile money.
                    </p>
                </li>
            </ul>
	    </fieldset>
    </div>
    <fieldset id="submitButtonStyle">
        <legend></legend>
	    <ul>
		    <li id="footer"><p>Copyright &copy; 2013  Frootaxi , All rights reserved.</p></li>
	    </ul>
    </fieldset>
</asp:Content>

