using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Login : System.Web.UI.Page
{
    #region Delartion
    string _strLoginUserName = string.Empty;
    string _strTermName = string.Empty;
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        _strTermName = System.Environment.MachineName.ToString();
        if (!IsPostBack)
        {
            txtusername.Focus();
            if (Request.QueryString["Status"] != null)
            {
                //display msg password change successfuly
                Session.Abandon();

            }
            if (Session["UserName"] != null)
            {
                Response.Redirect("DashBoard.aspx");
            }
        }
    }

    private void logIn()
    {
        LoginManagementDataContext _objDataContext = new LoginManagementDataContext();
        List<loginProcessResult> _objList;


        try
        {
            string _strUserName = txtusername.Text.Trim().Replace("'", "''");
            string _strPassword = txtpassword.Text.Trim().Replace("'", "''");
            string _strWhereCondition = "stUserName = '" + _strUserName + "' And stPassword ='" + _strPassword + "'";
            _objList = _objDataContext.loginProcess(_strUserName,_strPassword).ToList();
            if (_objList != null)
            {
                if (_objList.Count > 0)
                {
                    Session["UserName"] = _objList[0].stUserName;
                    Session["inEntityId"] = _objList[0].inEntityId;
                    Response.Redirect("DashBoard.aspx");
                }
                else
                {
                   // Common.showAsyncMsg("Incorrect user Id or password!", this.Page);
                    txtpassword.Text = string.Empty;
                    txtusername.Focus();
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void reset()
    {
        txtusername.Text = string.Empty;
        txtpassword.Text = string.Empty;
        txtusername.Focus();
    }
    protected void btnlogin_Click(object sender, EventArgs e)
    {
        try
        {
            logIn();
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }
}