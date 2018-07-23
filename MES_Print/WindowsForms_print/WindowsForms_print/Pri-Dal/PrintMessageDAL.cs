using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Print_Message;

namespace Print.Message.Dal
{
    class PrintMessageDAL
    {
        private static readonly string conStr = ConfigurationManager.ConnectionStrings["conn1"].ConnectionString;

        //插入打印数据到ManuPrintParam表
        public int InsertPrintMessageDAL(List<PrintMessage> list) {
            SqlConnection conn1 = new SqlConnection(conStr);
            conn1.Open();
            SqlCommand command = conn1.CreateCommand();
            string sim, vip, bat;
            int i = list.Count;
            if (i > 0)
            {
                if (list[i - 1].SIM == "0") { sim = ""; } else { sim = list[i - 1].SIM; }
                if (list[i - 1].VIP == "0") { vip = ""; } else { vip = list[i - 1].VIP; }
                if (list[i - 1].BAT == "0") { bat = ""; } else { bat = list[i - 1].BAT; }
                string CH_PrintTime = list[i-1].CH_PrintTime==""? "NULL": "'"+list[i - 1].CH_PrintTime+"'";
                string JS_PrintTime = list[i - 1].JS_PrintTime == "" ? "NULL" : "'" + list[i - 1].JS_PrintTime + "'";
                command.CommandText = "INSERT INTO dbo.Gps_ManuPrintParam(ZhiDan,IMEI,IMEIStart,IMEIEnd,SN,IMEIRel,SIM,VIP,BAT,SoftModel,Version,Remark,JS_PrintTime,JS_TemplatePath,JS_ReprintNum,JS_ReFirstPrintTime,JS_ReEndPrintTime,UserName,CH_PrintTime,CH_TemplatePath1,CH_TemplatePath2,CH_ReprintNum,CH_ReFirstPrintTime,CH_ReEndPrintTime) VALUES('" + list[i-1].Zhidan + "','" + list[i - 1].IMEI + "','" + list[i - 1].IMEIStart + "','" + list[i - 1].IMEIEnd + "','" + list[i - 1].SN + "','"+list[i-1].IMEIRel+"','"+sim+"','"+vip+"','"+bat+"','"+list[i-1].SoftModel+"','"+list[i-1].Version+"','"+list[i-1].Remark+"',"+JS_PrintTime + ",'"+list[i-1].JS_TemplatePath + "','0',NULL,NULL,'',"+CH_PrintTime+",'"+list[i-1].CH_TemplatePath1+"','"+list[i-1].CH_TemplatePath2+"','0',NULL,NULL)";
            }
            int httpstr = command.ExecuteNonQuery();
            conn1.Close();
            return httpstr;
        }

        //检查IMEI号是否存在，存在返回1，否则返回0
        public int CheckCHOrJSIMEIDAL(string IMEInumber,int PrintType) {
            SqlConnection conn1 = new SqlConnection(conStr);
            conn1.Open();
            SqlCommand command = conn1.CreateCommand();
            if (PrintType == 1)
            {
                command.CommandText = "SELECT * FROM dbo.Gps_ManuPrintParam WHERE (IMEI='" + IMEInumber + "' AND JS_PrintTime is NULL)";
            }
            else {
                command.CommandText = "SELECT * FROM dbo.Gps_ManuPrintParam WHERE (IMEI='" + IMEInumber + "' AND CH_PrintTime is NULL)";
            }
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                    return 1;
            }
            return 0;
        }

        //检查IMEI号是否存在，存在返回1，否则返回0
        public int CheckReCHOrJSIMEIDAL(string IMEInumber, int PrintType)
        {
            SqlConnection conn1 = new SqlConnection(conStr);
            conn1.Open();
            SqlCommand command = conn1.CreateCommand();
            if (PrintType == 1)
            {
                command.CommandText = "SELECT * FROM dbo.Gps_ManuPrintParam WHERE (IMEI='" + IMEInumber + "' AND JS_PrintTime != '')";
            }
            else
            {
                command.CommandText = "SELECT * FROM dbo.Gps_ManuPrintParam WHERE (IMEI='" + IMEInumber + "' AND CH_PrintTime != '')";
            }
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                return 1;
            }
            return 0;
        }

        //检查IMEI号是否存在，存在返回1，否则返回0
        public int CheckIMEIDAL(string IMEInumber)
        {
            SqlConnection conn1 = new SqlConnection(conStr);
            conn1.Open();
            SqlCommand command = conn1.CreateCommand();
            command.CommandText = "SELECT * FROM dbo.Gps_ManuPrintParam WHERE IMEI='" + IMEInumber + "'";
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                return 1;
            }
            return 0;
        }

        //检查SIM号是否存在，存在返回1，否则返回0
        public int CheckSIMDAL(string SIM)
        {
            SqlConnection conn1 = new SqlConnection(conStr);
            conn1.Open();
            SqlCommand command = conn1.CreateCommand();
            command.CommandText = "SELECT * FROM dbo.Gps_ManuPrintParam WHERE SIM='" + SIM + "'";
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                return 1;
            }
            return 0;
        }

        //检查VIP号是否存在，存在返回1，否则返回0
        public int CheckVIPDAL(string VIP)
        {
            SqlConnection conn1 = new SqlConnection(conStr);
            conn1.Open();
            SqlCommand command = conn1.CreateCommand();
            command.CommandText = "SELECT * FROM dbo.Gps_ManuPrintParam WHERE VIP='" + VIP + "'";
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                return 1;
            }
            return 0;
        }

        //根据IMEI号获取sn号进行重打
        public List<PrintMessage> SelectSnByIMEIDAL(string IMEInumber)
        {
            List<PrintMessage> pm = new List<PrintMessage>();
            SqlConnection conn1 = new SqlConnection(conStr);
            conn1.Open();
            SqlCommand command = conn1.CreateCommand();
            command.CommandText = "SELECT * FROM dbo.Gps_ManuPrintParam WHERE IMEI='" + IMEInumber + "'";
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                pm.Add(new PrintMessage()
                {
                    SN = dr.IsDBNull(5) ? "" : dr.GetString(5),
                    SIM = dr.IsDBNull(7) ? "" : dr.GetString(7),
                    VIP = dr.IsDBNull(8) ? "" : dr.GetString(8),
                    BAT = dr.IsDBNull(9) ? "" : dr.GetString(9),
                    SoftModel = dr.GetString(10),
                    Remark = dr.IsDBNull(12) ? "" : dr.GetString(12)
                });
            }
            return pm;
            
        }

        //根据IMEI号获取机身贴重打次数
        public int SelectJS_RePrintNumByIMEIDAL(string IMEInumber)
        {
            SqlConnection conn1 = new SqlConnection(conStr);
            conn1.Open();
            SqlCommand command = conn1.CreateCommand();
            command.CommandText = "select  * FROM dbo.Gps_ManuPrintParam WHERE IMEI='" + IMEInumber + "'";
            SqlDataReader dr = command.ExecuteReader();
            int RePrintNum = 0;
            while (dr.Read()) {
                 RePrintNum = dr.GetInt32(15);
            }
            return RePrintNum;
        }

        //根据IMEI号获取彩盒贴重打次数
        public int SelectCH_RePrintNumByIMEIDAL(string IMEInumber)
        {
            SqlConnection conn1 = new SqlConnection(conStr);
            conn1.Open();
            SqlCommand command = conn1.CreateCommand();
            command.CommandText = "select  * FROM dbo.Gps_ManuPrintParam WHERE IMEI='" + IMEInumber + "'";
            SqlDataReader dr = command.ExecuteReader();
            int RePrintNum = 0;
            while (dr.Read())
            {
                RePrintNum = dr.GetInt32(22);
            }
            return RePrintNum;
        }

        //根据IMEI号和路径获取重打次数
        public int SelectRePrintNumByIMEIAndPathDAL(string IMEInumber,string path)
        {
            SqlConnection conn1 = new SqlConnection(conStr);
            conn1.Open();
            SqlCommand command = conn1.CreateCommand();
            command.CommandText = "select  * FROM dbo.Gps_ManuPrintParam WHERE (IMEI='" + IMEInumber + "' AND TemplatePath = '"+path+"')";
            SqlDataReader dr = command.ExecuteReader();
            int RePrintNum = 0;
            while (dr.Read())
            {
                RePrintNum = dr.GetInt32(15);
            }
            return RePrintNum;
        }

        //更新机身首次重打数据
        public int UpdateRePrintDAL(string IMEInumber, string RePrintTime,string lj)
        {
            SqlConnection conn1 = new SqlConnection(conStr);
            conn1.Open();
            SqlCommand command = conn1.CreateCommand();
            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET JS_ReFirstPrintTime ='" + RePrintTime + "',JS_TemplatePath = '" + lj+ "',JS_RePrintNum = JS_RePrintNum+1 WHERE IMEI='" + IMEInumber + "'";
            return command.ExecuteNonQuery();
        }

        //更新机身末次重打数据
        public int UpdateReEndPrintDAL(string IMEInumber, string RePrintTime, string lj)
        {
            SqlConnection conn1 = new SqlConnection(conStr);
            conn1.Open();
            SqlCommand command = conn1.CreateCommand();
            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET JS_ReEndPrintTime ='" + RePrintTime + "', JS_TemplatePath = '" + lj + "',JS_RePrintNum = JS_RePrintNum+1 WHERE IMEI='" + IMEInumber + "' ";
            return command.ExecuteNonQuery();
        }

        //更新彩盒首次重打数据
        public int UpdateCHRePrintDAL(string IMEInumber, string RePrintTime, string lj,string lj1)
        {
            SqlConnection conn1 = new SqlConnection(conStr);
            conn1.Open();
            SqlCommand command = conn1.CreateCommand();
            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET CH_ReFirstPrintTime ='" + RePrintTime + "',CH_TemplatePath1 = '" + lj + "',CH_TemplatePath2 = '"+lj1+ "', CH_RePrintNum = CH_RePrintNum+1 WHERE IMEI='" + IMEInumber + "'";
            return command.ExecuteNonQuery();
        }

        //更新彩盒末次重打数据
        public int UpdateCHReEndPrintDAL(string IMEInumber, string RePrintTime, string lj,string lj1)
        {
            SqlConnection conn1 = new SqlConnection(conStr);
            conn1.Open();
            SqlCommand command = conn1.CreateCommand();
            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET CH_ReEndPrintTime ='" + RePrintTime + "', CH_TemplatePath1 = '" + lj + "',CH_TemplatePath2 = '" + lj1 + "',CH_RePrintNum = CH_RePrintNum+1 WHERE IMEI='" + IMEInumber + "' ";
            return command.ExecuteNonQuery();
        }

        //获取机身贴重打记录
        public List<PrintMessage> SelectAllReJSTDAL()
        {
            List<PrintMessage> pm = new List<PrintMessage>();
            SqlConnection conn1 = new SqlConnection(conStr);
            conn1.Open();
            SqlCommand command = conn1.CreateCommand();
            command.CommandText = "SELECT * FROM dbo.Gps_ManuPrintParam WHERE JS_RePrintNum !=0";
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                pm.Add(new PrintMessage()
                {
                    Zhidan = dr.GetString(1),
                    IMEI = dr.GetString(2),
                    SN = dr.GetString(5),
                    SoftModel = dr.IsDBNull(10) ? "" : dr.GetString(10),
                    JS_PrintTime = dr.GetDateTime(13).ToString(),
                    JS_TemplatePath = dr.GetString(14),
                    JS_RePrintNum = dr.GetInt32(15),
                    JS_ReFirstPrintTime = dr.IsDBNull(16) ? "" : dr.GetDateTime(16).ToString(),
                    JS_ReEndPrintTime = dr.IsDBNull(17) ? "" : dr.GetDateTime(17).ToString()
                });
            }
            return pm;
        }

        //获取机身贴重打记录
        public List<PrintMessage> SelectAllReCHTDAL()
        {
            List<PrintMessage> pm = new List<PrintMessage>();
            SqlConnection conn1 = new SqlConnection(conStr);
            conn1.Open();
            SqlCommand command = conn1.CreateCommand();
            command.CommandText = "SELECT * FROM dbo.Gps_ManuPrintParam WHERE CH_RePrintNum !=0";
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                pm.Add(new PrintMessage()
                {
                    Zhidan = dr.GetString(1),
                    IMEI = dr.GetString(2),
                    SN = dr.GetString(5),
                    SoftModel = dr.IsDBNull(10) ? "" : dr.GetString(10),
                    CH_PrintTime = dr.IsDBNull(19) ? "" : dr.GetDateTime(19).ToString(),
                    CH_TemplatePath1 = dr.IsDBNull(20) ? "" : dr.GetString(20),
                    CH_TemplatePath2 = dr.IsDBNull(21) ? "" : dr.GetString(21),
                    CH_RePrintNum = dr.GetInt32(22).ToString(),
                    CH_ReFirstPrintTime = dr.IsDBNull(23) ? "" : dr.GetDateTime(23).ToString(),
                    CH_ReEndPrintTime = dr.IsDBNull(24) ? "" : dr.GetDateTime(24).ToString()
                });
            }
            return pm;
        }

        //根据制单号或IMEI号获取重打记录
        public List<PrintMessage> SelectReMesByZhiDanOrIMEIDAL(string ToCheck)
        {
            List<PrintMessage> pm = new List<PrintMessage>();
            SqlConnection conn1 = new SqlConnection(conStr);
            conn1.Open();
            SqlCommand command = conn1.CreateCommand();
            command.CommandText = "SELECT * FROM dbo.Gps_ManuPrintParam WHERE ((ZhiDan='" + ToCheck + "' OR IMEI='"+ToCheck+ "') AND (CH_RePrintNum!=0 OR JS_RePrintNum!=0))";
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                pm.Add(new PrintMessage()
                {
                    Zhidan = dr.GetString(1),
                    IMEI = dr.GetString(2),
                    SN = dr.IsDBNull(5) ? "" : dr.GetString(5),
                    SoftModel = dr.IsDBNull(10) ? "" : dr.GetString(10),
                    JS_PrintTime = dr.IsDBNull(13) ? "" : dr.GetDateTime(13).ToString(),
                    JS_TemplatePath = dr.IsDBNull(14) ? "" : dr.GetString(14),
                    JS_RePrintNum = dr.GetInt32(15),
                    JS_ReFirstPrintTime = dr.IsDBNull(16) ? "" : dr.GetDateTime(16).ToString(),
                    JS_ReEndPrintTime = dr.IsDBNull(17) ? "" : dr.GetDateTime(17).ToString(),
                    CH_PrintTime = dr.IsDBNull(19) ? "" : dr.GetDateTime(19).ToString(),
                    CH_TemplatePath1 = dr.IsDBNull(20) ? "" : dr.GetString(20),
                    CH_TemplatePath2 = dr.IsDBNull(21) ? "" : dr.GetString(21),
                    CH_RePrintNum = dr.GetInt32(22).ToString(),
                    CH_ReFirstPrintTime = dr.IsDBNull(23) ? "" : dr.GetDateTime(23).ToString(),
                    CH_ReEndPrintTime = dr.IsDBNull(24) ? "" : dr.GetDateTime(24).ToString()
                });
            }
            return pm;
        }

        //根据SN号或IMEI号获取打印记录
        public List<PrintMessage> SelectPrintMesBySNOrIMEIDAL(string conditions)
        {
            List<PrintMessage> pm = new List<PrintMessage>();
            SqlConnection conn1 = new SqlConnection(conStr);
            conn1.Open();
            SqlCommand command = conn1.CreateCommand();
            command.CommandText = "SELECT * FROM dbo.Gps_ManuPrintParam WHERE ((IMEI='" + conditions + "' OR SN='" + conditions + "')AND JS_PrintTime != '')";
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                pm.Add(new PrintMessage()
                {
                    ID = dr.GetInt32(0),
                    Zhidan = dr.IsDBNull(1) ? "" :dr.GetString(1),
                    IMEI = dr.IsDBNull(2) ? "" : dr.GetString(2),
                    SN = dr.IsDBNull(5) ? "" :dr.GetString(5),
                    IMEIRel = dr.IsDBNull(6) ? "" : dr.GetString(6),
                    SIM = dr.IsDBNull(7) ? "" : dr.GetString(7),
                    VIP = dr.IsDBNull(8) ? "" : dr.GetString(8),
                    BAT = dr.IsDBNull(9) ? "" : dr.GetString(9),
                    SoftModel = dr.IsDBNull(10) ? "" : dr.GetString(10),
                    JS_PrintTime = dr.GetDateTime(13).ToString(),
                    JS_TemplatePath = dr.GetString(14)
                });
            }
            return pm;
        }

        //根据ID删除打印记录
        public int DeletePrintMessageDAL(int id,string field)
        {
            SqlConnection conn1 = new SqlConnection(conStr);
            conn1.Open();
            SqlCommand command = conn1.CreateCommand();
            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET "+field+" ='' WHERE ID='" + id + "'";
            int httpstr = command.ExecuteNonQuery();
            conn1.Close();
            return httpstr;
        }

        //public List<PrintMessage> SelectPrintMesByIdDAL(string id)
        //{
        //    List<PrintMessage> pm = new List<PrintMessage>();
        //    SqlConnection conn1 = new SqlConnection(conStr);
        //    conn1.Open();
        //    SqlCommand command = conn1.CreateCommand();
        //    command.CommandText = "SELECT * FROM dbo.Gps_ManuPrintParam WHERE ((IMEI='"+id+"')AND PrintType=1)";
        //    SqlDataReader dr = command.ExecuteReader();
        //    while (dr.Read())
        //    {
        //        pm.Add(new PrintMessage()
        //        {
        //            ID = dr.GetInt32(0),
        //            Zhidan = dr.IsDBNull(1) ? "" : dr.GetString(1),
        //            IMEI = dr.GetString(2),
        //            SN1 = dr.IsDBNull(3) ? "" : dr.GetString(3),
        //            SN2 = dr.GetString(4),
        //            FirstPrintTime = dr.GetDateTime(5).ToString(),
        //            RePrintNum = dr.GetInt32(6),
        //            RePrintTime = dr.GetDateTime(7).ToString(),
        //            PrintType = dr.GetInt32(8),
        //            SoftModel = dr.IsDBNull(9) ? "" : dr.GetString(9),
        //            SNFromCustomer = dr.IsDBNull(10) ? "" : dr.GetString(10),
        //            RePrintEndTime = dr.IsDBNull(11) ? "" : dr.GetDateTime(11).ToString()
        //        });
        //    }
        //    return pm;
        //}

    }
}
