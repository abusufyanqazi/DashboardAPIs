using System;
using System.Data;
using System.Data.OracleClient;
using System.Configuration;
using System.Web;
using System.Collections;

/// <summary>
/// Summary description for DB_Utility
/// </summary>

namespace DAL
{
    public class DB_Utility
    {
        private string _constr;
        public DB_Utility(string conStr)
        {
            //
            // TODO: Add constructor logic here
            //
            _constr = conStr;
        }

        //
        public DataTable getBillingStatus()
        {
            OracleConnection con = null;
            OracleCommand cmd;
            con = new OracleConnection(_constr);//
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
            cmd = new OracleCommand(@"SELECT BILLING_MONTH, DIV_NAME, BATCH_DUE, BATCH_RECEIVED, BATCH_BILLED, CC_CODE, RECEIVED_DATE, POSTED_DATE
                                FROM BILLING_STATUS_DIV", con);
            cmd.CommandType = CommandType.Text;
            DataSet ds = new DataSet();
            OracleDataAdapter ad = new OracleDataAdapter();
            ad.SelectCommand = cmd;
            try
            {
                ad.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                DataTable dtErr = new DataTable();
                dtErr.Columns.Add("Desc");
                    DataRow drErr= dtErr.NewRow();
                    drErr["Desc"] = ex.ToString();
                    dtErr.Rows.Add(drErr);
                    return dtErr;
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            return null;
        }

        public DataTable GetCollVsCompAssmnt(string code = "")
        {
            OracleConnection con = null;
            OracleCommand cmd;
            con = new OracleConnection(_constr);//
            string sortorder = "";

            switch (code.Length)
            {
                case 2:
                    {
                        sortorder = "IN(3,4)";
                        break;
                    }
                case 3:
                    {
                        sortorder = "IN(2,3)";
                        break;
                    }
                case 4:
                    {
                        sortorder = "IN('1','2')";
                        break;
                    }

                default:
                    {
                        sortorder = "IN(3,4)";
                        break;
                    }


            }
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            string sql =
                @"SELECT SRT_ORDER2, SRT_ORDER1, SDIVCODE, SDIVNAME, PVT_COMP_ASSES, GVT_COMP_ASSES, COMP_ASSES, PVT_COLL, GVT_COLL, TOT_COLL, B_PERIOD, CC_CODE, PVT_PERCENT, GVT_PERCENT, TOT_PERCENT
                                      FROM VW_COLL_VS_COMP_ASS";
            if (!string.IsNullOrEmpty(code))
            {
                sql += " WHERE SDIVCODE LIKE '" + code + "%' AND SRT_ORDER2 " + sortorder;
            }

            //sql += " ORDER BY SRT_ORDER2 DESC";

            cmd = new OracleCommand(sql, con);
            cmd.CommandType = CommandType.Text;
            DataSet ds = new DataSet();
            OracleDataAdapter ad = new OracleDataAdapter();
            ad.SelectCommand = cmd;
            try
            {
                ad.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                DataTable dtErr = new DataTable();
                dtErr.Columns.Add("Desc");
                DataRow drErr = dtErr.NewRow();
                drErr["Desc"] = ex.ToString();
                dtErr.Rows.Add(drErr);
                return dtErr;
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            return null;
        }


        public DataTable getReceiveables()
        {
            OracleConnection con = null;
            OracleCommand cmd;
            con = new OracleConnection(_constr);
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
            cmd = new OracleCommand(@"SELECT SRT_ORDER2, SRT_ORDER1, SDIV_CODE, SDIV_NAME, PVT_REC, GVT_REC, TOT_REC, PVT_SPILL, GVT_SPILL, TOT_SPILL, PVT_ARREAR, GVT_ARREAR, TOT_ARREAR, CC_CODE, B_PERIOD
                                     FROM VW_RECIEVABLES", con);
            cmd.CommandType = CommandType.Text;
            DataSet ds = new DataSet();
            OracleDataAdapter ad = new OracleDataAdapter();
            ad.SelectCommand = cmd;
            try
            {
                ad.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            return null;
        }

    }
}