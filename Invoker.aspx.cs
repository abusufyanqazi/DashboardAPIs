using System;
using System.Collections;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using util;

public partial class Invoker : System.Web.UI.Page
{
    private string GetWCFResponse()
    {

        string type = "";
        string token = "";
        string resp = "Error";
        if (!String.IsNullOrEmpty(Request.QueryString["type"]))
        {
            type = Request.QueryString["type"];
        }
        if (!String.IsNullOrEmpty(Request.QueryString["token"]))
        {
            token = Request.QueryString["token"];
        }

        return resp;
    }

    private string GetResponse()
    {

        string type = "";
        string token = "";
        string code = "";
        string resp = "BlankRsp";
        if (!String.IsNullOrEmpty(Request.QueryString["type"]))
        {
            type = Request.QueryString["type"];
        }
        if (!String.IsNullOrEmpty(Request.QueryString["token"]))
        {
            token = Request.QueryString["token"];
        }
        if (!String.IsNullOrEmpty(Request.QueryString["code"]))
        {
            code = Request.QueryString["code"];
        }
        else
        {
            code = "15";
        }

        if (type.ToUpper().Equals("BILLING"))
            resp = GetBillStatus(token);
        else if (type.ToUpper().Equals("COLLVSCOMPASSMNT"))
            resp = GetCollVsCompAssmnt(token, code);
        else if (type.ToUpper().Equals("RECEIVABLE"))
            resp = GetReceivable(token);
        else
            resp = "Invalid Req.";
        return resp;
    }
    public string GetBillStatus(string token)
    {
        string secKey = System.Configuration.ConfigurationManager.AppSettings["SECKEY"].ToString();
        string conStr = System.Configuration.ConfigurationManager.ConnectionStrings["CONSTR"].ToString();
        string ret = "Error";

        if (token != secKey)
            return "Ivalid Token.";
        try
        {
            DB_Utility objDBUTil = new DB_Utility(conStr);
            DataTable dt = objDBUTil.getBillingStatus();
            utility util = new utility();

            ret = util.DataTableToJSONWithStringBuilder(dt);
        }
        catch (Exception ex)
        {
            ret = ex.ToString();
        }
        return ret;
    }

    public string GetCollVsCompAssmnt(string token, string code="1")
    {
        string secKey = System.Configuration.ConfigurationManager.AppSettings["SECKEY"].ToString();
        string conStr = System.Configuration.ConfigurationManager.ConnectionStrings["CONSTR"].ToString();
        DataTable dt;
        utility util = new utility();
        DB_Utility objDbuTil = new DB_Utility(conStr);
        StringBuilder jsonString = new StringBuilder();
        jsonString.Append("{");
        jsonString.Append(@"""CollVsCompAssmnt"":{");
        StringBuilder filterExp = new StringBuilder();
        string ret = "error";
        
        if (token != secKey)
            return "Ivalid Token.";
        try
        {
            
            if (!string.IsNullOrEmpty(code))
            {
                filterExp.AppendFormat("LEN(SDIVCODE) >= {0} and LEN(SDIVCODE) <= {1}", (code.Length).ToString(), (code.Length+1).ToString());
            }

            dt = objDbuTil.GetCollVsCompAssmnt(code);

            if (dt != null)
            {
                int i = 0;
                DataView dv = dt.DefaultView;
                dv.RowFilter = filterExp.ToString();
                dv.Sort = "SRT_ORDER2 ASC";
                foreach (DataRowView dr in dv)
                {
                    util.GetCollVsCompAssmntPJSON1(dv.ToTable(), jsonString, dr["SDIVCODE"].ToString());
                    if (i == dv.ToTable().Rows.Count - 1)
                    {
                        jsonString.Append("}");
                    }
                    else
                    {
                        jsonString.Append("},");
                    }

                    i++;
                }
            }

            jsonString.Append("}");
                jsonString.Append("}");
                ret = jsonString.ToString();
           
        }
        catch (Exception ex)
        {
            ret = ex.ToString();
        }

        return ret;
    }

    public string GetReceivable(string token)
    {
        string secKey = System.Configuration.ConfigurationManager.AppSettings["SECKEY"].ToString();
        string conStr = System.Configuration.ConfigurationManager.ConnectionStrings["CONSTR2_6"].ToString();
        string ret = "Error";

        if (token != secKey)
            return "Ivalid Token.";
        try
        {
            DB_Utility objDBUTil = new DB_Utility(conStr);
            DataTable dt = objDBUTil.getReceiveables();
            utility util = new utility();

            ret = util.DataTableToJSONWithStringBuilder(dt);
        }
        catch (Exception ex)
        {
            ret = ex.ToString();
        }
        return ret;
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        string resp = "Blank";
        try
        {
            resp = GetResponse();
        }
        catch (Exception ex)
        {
            resp = ex.ToString();
        }
        Response.Clear();
        Response.ContentType = "application/json; charset=utf-8";
        Response.Write(resp);
        Response.End();
        Response.Write(resp);
    }

}