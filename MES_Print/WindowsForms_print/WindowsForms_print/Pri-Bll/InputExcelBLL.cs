using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using System.Data.SqlClient;

namespace Print.Message.Bll
{
    class InputExcelBLL
    {
        public DataSet GetExcelDatatable(string fileUrl) {
            const string cmdText = "Provider=Microsoft.Ace.OleDb.12.0;Data Source={0};Extended Properties='Excel 12.0; HDR=Yes; IMEX=1'";
            //DataTable dt = null;
            string strconn = string.Format(cmdText, fileUrl);
            OleDbConnection conn = new OleDbConnection(strconn);
            try
            {
                if (conn.State == ConnectionState.Broken || conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                DataTable schematable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                //获取Excel的第一个Sheet名称
                string sheetName = schematable.Rows[0]["TABLE_NAME"].ToString().Trim();
                //查询sheet中的数据
                string StrSql = "select * from [" + sheetName + "]";
                OleDbDataAdapter da = new OleDbDataAdapter(StrSql, conn);
                DataSet ds = new DataSet();
                da.Fill(ds);
                //dt = ds.Tables[0];
                return ds;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally {
                conn.Close();
                conn.Dispose();
            }
        }

        public int InsetData(DataTable dt)
        {
            int i = 0;
            string UUID = "";
            string IMEI = "";
            string UUID_211 = "";
            string UUID_211_IMEI = "";
            foreach (DataRow dr in dt.Rows)
            {
                UUID = dr["UUID"].ToString().Trim();
                IMEI = dr["IMEI"].ToString().Trim();
                UUID_211 = dr["UUID_211"].ToString().Trim();
                UUID_211_IMEI = dr["UUID_211_IMEI"].ToString().Trim();

                string strSql = string.Format("Insert into Gps_GetIMEIFromexcel (UUID,IMEI,UUID_211,UUID_211_IMEI) Values ('{0}','{1}','{2}','{3}')", UUID, IMEI, UUID_211, UUID_211_IMEI);
                string conStr = ConfigurationManager.ConnectionStrings["conn1"].ToString();
                SqlConnection conn1 = new SqlConnection(conStr);
                try
                {
                    conn1.Open();
                    SqlCommand command = conn1.CreateCommand();
                    command.CommandText = strSql;
                    command.Connection = conn1;
                    SqlDataReader sqlDataReader = command.ExecuteReader();
                    i++;
                    sqlDataReader.Close();
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    conn1.Close();
                }
            }
                 return i;
        }

    }
}
