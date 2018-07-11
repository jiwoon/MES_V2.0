using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Print_Message;

namespace ManuOrder.Param.DAL
{
    class ManuOrderParamDAL
    {
        private static readonly string conStr = ConfigurationManager.ConnectionStrings["conn1"].ConnectionString;

        //返回制单号
        public List<Gps_ManuOrderParam> SelectZhidanNumDAL()
        {
            SqlConnection conn1 = new SqlConnection(conStr);
            conn1.Open();
            List<Gps_ManuOrderParam> list = new List<Gps_ManuOrderParam>();
            SqlCommand command = conn1.CreateCommand();
            command.CommandText = "SELECT * FROM dbo.Gps_ManuOrderParam WHERE Status='1' OR Status='0' ORDER BY Status";
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new Gps_ManuOrderParam() {
                    ZhiDan = dr.GetString(1)
                });
            }
            return list;
        }

        //模糊查询返回制单号
        public List<Gps_ManuOrderParam> SelectZhidanNumBylikeDAL(string likeword)
        {
            SqlConnection conn1 = new SqlConnection(conStr);
            conn1.Open();
            List<Gps_ManuOrderParam> list = new List<Gps_ManuOrderParam>();
            SqlCommand command = conn1.CreateCommand();
            command.CommandText = "SELECT * FROM dbo.Gps_ManuOrderParam WHERE ZhiDan LIKE '%"+likeword+"'%";
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new Gps_ManuOrderParam()
                {
                    ZhiDan = dr.GetString(1)
                });
            }
            return list;
        }

        //根据制单号返回该制单相关信息
        public List<Gps_ManuOrderParam> selectManuOrderParamByzhidanDAL(string ZhidanNum)
        {
            SqlConnection conn1 = new SqlConnection(conStr);
            conn1.Open();
            List<Gps_ManuOrderParam> list = new List<Gps_ManuOrderParam>();
            SqlCommand command = conn1.CreateCommand();
            command.CommandText = "SELECT * FROM dbo.Gps_ManuOrderParam WHERE ZhiDan='" + ZhidanNum + "'";
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new Gps_ManuOrderParam()
                {
                    SoftModel = dr.GetString(2),
                    SN1 = dr.GetString(3),
                    SN2 = dr.GetString(4),
                    SN3 = dr.IsDBNull(5)? "":dr.GetString(5),
                    Box_No1 = dr.GetString(6),
                    Box_No2 = dr.GetString(7),
                    ProductDate = dr.GetString(8),
                    Color = dr.GetString(9),
                    Weight = dr.GetString(10),
                    Qty = dr.GetString(11),
                    ProductNo = dr.GetString(12),
                    Version = dr.GetString(13),
                    IMEIStart = dr.GetString(14),
                    IMEIEnd = dr.GetString(15),
                    SIMStart = dr.IsDBNull(16)?"":dr.GetString(16),
                    SIMEnd = dr.IsDBNull(17) ? "":dr.GetString(17),
                    BATStart = dr.IsDBNull(18) ? "" : dr.GetString(18),
                    BATEnd = dr.IsDBNull(19) ? "" : dr.GetString(19),
                    VIPStart = dr.IsDBNull(20) ? "" : dr.GetString(20),
                    VIPEnd = dr.IsDBNull(21) ? "" : dr.GetString(21),
                    IMEIRel = dr.GetInt32(22).ToString(),
                    Remark1 = dr.IsDBNull(25) ? "" : dr.GetString(25),
                    status = dr.GetInt32(30),
                    JST_template = dr.IsDBNull(32) ? "" : dr.GetString(32),
                    CHT_template1 = dr.IsDBNull(33) ? "" : dr.GetString(33),
                    CHT_template2 = dr.IsDBNull(34) ? "" : dr.GetString(34),
                    BAT_prefix = dr.IsDBNull(35) ? "" : dr.GetString(35),
                    BAT_digits = dr.IsDBNull(36) ? "" : dr.GetString(36),
                    SIM_prefix = dr.IsDBNull(37) ? "" : dr.GetString(37),
                    SIM_digits = dr.IsDBNull(38) ? "" : dr.GetString(38),
                    VIP_prefix = dr.IsDBNull(39) ? "" : dr.GetString(39),
                    VIP_digits = dr.IsDBNull(40) ? "" : dr.GetString(40),
                    ICCID_prefix = dr.IsDBNull(41) ? "" : dr.GetString(41),
                    ICCID_digits = dr.IsDBNull(42) ? "" : dr.GetString(42),
                    IMEIPrints = dr.IsDBNull(43) ? "" : dr.GetString(43)
                });
            }
            return list;
        }

        //根据制单号更新SN2号
        public int UpdateSNnumberDAL(string ZhiDanNum, string SN1, string SN2, string SN3, string VIP1, string VIP2,long ImeiPrints)
        {
            SqlConnection conn1 = new SqlConnection(conStr);
            conn1.Open();
            SqlCommand command = conn1.CreateCommand();
            command.CommandText = "UPDATE Gps_ManuOrderParam SET SN1='"+SN1+"',SN2 ='"+SN2+ "',SN3='"+SN3+"',IMEIPrints = '"+ImeiPrints.ToString()+"',VIPStart = '" + VIP1+ "',VIPEnd = '" + VIP2+"' WHERE ZhiDan='"+ZhiDanNum+"'";
            return command.ExecuteNonQuery();
        }

        //根据制单号更新SN3号
        public int UpdateSN3numberDAL(string ZhiDanNum, string SN1, string SN2, string SN3, string VIP1, string VIP2)
        {
            SqlConnection conn1 = new SqlConnection(conStr);
            conn1.Open();
            SqlCommand command = conn1.CreateCommand();
            command.CommandText = "UPDATE Gps_ManuOrderParam SET SN1='" + SN1 + "', SN2 ='" + SN2 + "', SN3 ='" + SN3 + "',VIPStart = '" + VIP1 + "',VIPEnd = '" + VIP2 + "' WHERE ZhiDan='" + ZhiDanNum + "'";
            return command.ExecuteNonQuery();
        }

        //更新彩盒打印信息
        public int UpdateCHmesDAL(string IMEI, string CHPrintTime, string lj1, string lj2)
        {
            SqlConnection conn1 = new SqlConnection(conStr);
            conn1.Open();
            SqlCommand command = conn1.CreateCommand();
            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET CH_PrintTime='" + CHPrintTime + "', CH_TemplatePath1 ='" + lj1 + "', CH_TemplatePath2 ='" + lj2 + "' WHERE IMEI='" + IMEI + "'";
            return command.ExecuteNonQuery();
        }

        //更新彩盒sim打印信息
        public int UpdateCHSIMDAL(string IMEI, string CHPrintTime, string lj1, string lj2,string SIM)
        {
            SqlConnection conn1 = new SqlConnection(conStr);
            conn1.Open();
            SqlCommand command = conn1.CreateCommand();
            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET CH_PrintTime='" + CHPrintTime + "', CH_TemplatePath1 ='" + lj1 + "', CH_TemplatePath2 ='" + lj2 + "',SIM='"+SIM+"' WHERE IMEI='" + IMEI + "'";
            return command.ExecuteNonQuery();
        }

        //更新彩盒VIP打印信息
        public int UpdateCHVIPDAL(string IMEI, string CHPrintTime, string lj1, string lj2, string VIP)
        {
            SqlConnection conn1 = new SqlConnection(conStr);
            conn1.Open();
            SqlCommand command = conn1.CreateCommand();
            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET CH_PrintTime='" + CHPrintTime + "', CH_TemplatePath1 ='" + lj1 + "', CH_TemplatePath2 ='" + lj2 + "',VIP='" + VIP + "' WHERE IMEI='" + IMEI + "'";
            return command.ExecuteNonQuery();
        }

        //更新彩盒VIP 和 SIM打印信息
        public int UpdateCHVIPAndSimDAL(string IMEI, string CHPrintTime, string lj1, string lj2, string VIP, string SIM)
        {
            SqlConnection conn1 = new SqlConnection(conStr);
            conn1.Open();
            SqlCommand command = conn1.CreateCommand();
            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET CH_PrintTime='" + CHPrintTime + "', CH_TemplatePath1 ='" + lj1 + "', CH_TemplatePath2 ='" + lj2 + "',VIP='" + VIP + "',SIM='"+SIM+"' WHERE IMEI='" + IMEI + "'";
            return command.ExecuteNonQuery();
        }

        //更新机身打印信息
        public int UpdateJSmesDAL(string IMEI, string JSPrintTime, string lj1)
        {
            SqlConnection conn1 = new SqlConnection(conStr);
            conn1.Open();
            SqlCommand command = conn1.CreateCommand();
            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET JS_PrintTime='" + JSPrintTime + "', JS_TemplatePath ='" + lj1 + "' WHERE IMEI='" + IMEI + "'";
            return command.ExecuteNonQuery();
        }

        //根据制单号更新状态，打印后0改成1
        public int UpdateStatusByZhiDanDAL(string ZhiDanNum)
        {
            SqlConnection conn1 = new SqlConnection(conStr);
            conn1.Open();
            SqlCommand command = conn1.CreateCommand();
            command.CommandText = "UPDATE Gps_ManuOrderParam SET Status = 1 WHERE (ZhiDan='" + ZhiDanNum + "' AND Status=0)";
            return command.ExecuteNonQuery();
        }
    }
}
