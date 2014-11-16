using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;

namespace EngineClass
{
    public class ErrorMessageGenerator
    {
        DataProvider dp = DataProvider.GetInstance();
        string script = "";

        public void showErrorMessage(string errorMsg, TextBox t)
        {
            t.CssClass = "textBox add_calendar hasDatepicker errorStyle";
            t.ToolTip = errorMsg;
        }

        public void showErrorMessage(string errorMsg, UpdatePanel up, DropDownList ddl)
        {
            loadScript(ddl, up, " ddlErrorStyle");
            ddl.Attributes.Add("title", errorMsg);
        }

        public void showErrorMessage(string errorMsg, Label l, UpdatePanel up, bool b)
        {
            if(!b)
                l.CssClass = "error msg ui-corner-bottom";

            if (b)
                l.CssClass = "confirm msg ui-corner-bottom";

            l.Visible = true;
            setErrorMessage(errorMsg, l);
            loadScript(l, up);
        }

        public void hideErrorMessage(TextBox t)
        {
            t.CssClass = "textBox";
            t.ToolTip = "";
        }

        public void hideErrorMessage(DropDownList ddl, UpdatePanel up)
        {
            loadScript(ddl, up, "");
            ddl.Attributes.Add("title", " ");
        }

        public void loadScript(Label l, UpdatePanel up)
        {
            //document.getElementById('body_" + l.ID + "').style.display = '" + e + "'; 
            script = "setTimeout(function(){jQuery('#body_" + l.ID + "').fadeOut(); }, 15000);";
            ScriptManager.RegisterStartupScript(up, this.GetType(), "ErrorDivOpen", script, true);
        }

        public void loadScript(DropDownList d, UpdatePanel up, string e)
        {
            script = "document.getElementById('body_" + d.ID + "').className += '" + e + "';";
            ScriptManager.RegisterStartupScript(up, this.GetType(), "ErrorDivOpen", script, true);
        }

        public void setErrorMessage(string errorMsg, Label l)
        {
            l.Text = errorMsg;
        }

        /*public void showDialog(string modalDialog, UpdatePanel up)
        {
            //script = string.Format(@"openModalDiv('{0}')", modalDialog);
            ScriptManager.RegisterClientScriptBlock(up, this.GetType(), "ErrorDivShow", script, true);
        }*/

        public void showDialog(string script, UpdatePanel up)
        {
            ScriptManager.RegisterClientScriptBlock(up, this.GetType(), "ShowPaygPopup", script, true);
        }

        public void loadScript(string script, UpdatePanel up)
        {
            ScriptManager.RegisterClientScriptBlock(up, this.GetType(), "LoadScript", script, true);
        }
    }
}
