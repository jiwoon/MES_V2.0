using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seagull.BarTender.Print;
using System.Diagnostics;
using System.Windows.Forms;
using Print_Message;
using Print.Message.Bll;
using ManuOrder.Param.BLL;
using System.Runtime.InteropServices;
using System.Drawing.Printing;
using System.Text.RegularExpressions;

namespace WindowsForms_print
{
    public partial class Form1 : Form
    {

        public static void Log(string msg,Exception e)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + "print.log";
                if (!File.Exists(path))
                {
                   File.Create(path).Close();
                }
                StreamWriter writer = new StreamWriter(path,true);
                writer.WriteLine("");
                writer.WriteLine(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + msg);
                writer.Flush();
                writer.Close();
            }
            catch
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + "print.log";
                if (!Directory.Exists(path))
                {
                    File.Create(path).Close();
                }
                StreamWriter writer = File.AppendText(path);
                writer.WriteLine("");
                writer.WriteLine(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + msg + "错误：" + e.Message);
                writer.Flush();
                writer.Close();
            }
        }

        string outString;

        ManuOrderParamBLL MOPB = new ManuOrderParamBLL();

        PrintMessageBLL PMB = new PrintMessageBLL();

        InputExcelBLL IEB = new InputExcelBLL();

        List<Gps_ManuOrderParam> G_MOP = new List<Gps_ManuOrderParam>();

        List<PrintMessage> list = new List<PrintMessage>();

        public Form1()
        {
            InitializeComponent();
        }
        [DllImport("kernel32.dll")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder returnvalue, int buffersize, string filepath);

        private string IniFilePath;
        private void GetValue(string section, string key, out string value)
        {
            IniFilePath = AppDomain.CurrentDomain.BaseDirectory + "PrintVariable.ini";
            StringBuilder stringBuilder = new StringBuilder();
            GetPrivateProfileString(section, key, "", stringBuilder, 1024, IniFilePath);
            value = stringBuilder.ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            PrintDocument print = new PrintDocument();
            string sDefault = print.PrinterSettings.PrinterName;//默认打印机名
            this.Printer1.Text = sDefault;
            foreach (string sPrint in PrinterSettings.InstalledPrinters)//获取所有打印机名称
            {
                Printer1.Items.Add(sPrint);
            }
            G_MOP = MOPB.SelectZhidanNumBLL();
            foreach (Gps_ManuOrderParam a in G_MOP)
            {
                this.CB_ZhiDan.Items.Add(a.ZhiDan);
            }
            string NowData = System.DateTime.Now.ToString("yyyy.MM.dd");
            this.ProductData.Text = NowData;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Engine btEngine = new Engine();
                btEngine.Start();
                string lj = "";
                if (this.Select_Template1.Text != "")
                {
                    lj = this.Select_Template1.Text;
                    LabelFormatDocument btFormat = btEngine.Documents.Open(lj);
                    //对模板相应字段进行赋值
                    GetValue("Information", "型号", out outString);
                    btFormat.SubStrings[outString].Value = this.SoftModel.Text;
                    GetValue("Information", "生产日期", out outString);
                    btFormat.SubStrings[outString].Value = this.ProductData.Text;
                    GetValue("Information", "产品编码", out outString);
                    btFormat.SubStrings[outString].Value = this.ProductNo.Text;
                    GetValue("Information", "软件版本", out outString);
                    btFormat.SubStrings[outString].Value = this.SoftwareVersion.Text;
                    GetValue("Information", "备注", out outString);
                    btFormat.SubStrings[outString].Value = this.Remake.Text;

                    //指定打印机名称
                    btFormat.PrintSetup.PrinterName = this.Printer1.Text;
                    //打印份数,同序列打印的份数
                    btFormat.PrintSetup.IdenticalCopiesOfLabel = 1;

                    Messages messages;
                    int waitout = 10000;
                    if (this.PrintOne.Checked == true)
                    {
                        if (this.IMEI_Start.Text != "")
                        {
                            btFormat.SubStrings["IMEI"].Value = this.IMEI_Start.Text;
                            if (!PMB.CheckIMEIBLL(this.IMEI_Start.Text))
                            {
                                //记录打印信息日志
                                string ProductTime = "";
                                string NowTime = System.DateTime.Now.ToString("HH:mm:ss:fff");
                                if (this.ProductData.Text == "")
                                {
                                    ProductTime = System.DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:fff");
                                }
                                else
                                {
                                    ProductTime = this.ProductData.Text.Trim() + " " + NowTime;
                                }
                                if (this.SN1_num.Text != "")
                                {
                                    btFormat.SubStrings["SN"].Value = this.SN1_num.Text;
                                    list.Add(new PrintMessage()
                                    {
                                        Zhidan = this.CB_ZhiDan.Text.Trim(),
                                        IMEI = this.IMEI_Start.Text.Trim(),
                                        IMEIStart = this.IMEI_num1.Text.Trim(),
                                        IMEIEnd = this.IMEI_num2.Text.Trim(),
                                        SN = this.SN1_num.Text,
                                        IMEIRel = this.IMEIRel.Text.Trim(),
                                        SIM = "",
                                        VIP = "",
                                        BAT = "",
                                        SoftModel = this.SoftModel.Text.Trim(),
                                        Version = this.SoftwareVersion.Text.Trim(),
                                        Remark = this.Remake.Text.Trim(),
                                        JS_PrintTime = ProductTime,
                                        JS_TemplatePath = this.Select_Template1.Text.Trim(),
                                        CH_PrintTime = "",
                                        CH_TemplatePath1 = null,
                                        CH_TemplatePath2 = null
                                    });
                                    if (PMB.InsertPrintMessageBLL(list))
                                    {
                                        string sn1_prefix = SN1_num.Text.Substring(0, this.SN1_num.Text.Length - 5);
                                        long sn1_suffix = long.Parse(SN1_num.Text.Remove(0, (this.SN1_num.Text.Length) - 5));
                                        string sn1 = sn1_prefix + (sn1_suffix + 1).ToString().PadLeft(5, '0');
                                        string sn2_suffix = SN2_num.Text.Remove(0, (this.SN2_num.Text.Length) - 5);
                                        if (MOPB.UpdateSNnumberBLL(this.CB_ZhiDan.Text, sn1_prefix, (sn1_suffix + 1).ToString().PadLeft(5, '0'), sn2_suffix, this.VIP_num1.Text, this.VIP_num2.Text, long.Parse(this.IMEI_Start.Text.Substring(0, 14)) + 1))
                                        {
                                            Result nResult1 = btFormat.Print("标签打印软件", waitout, out messages);
                                            Form1.Log("打印了IMEI号为" + this.IMEI_Start.Text + "的制单", null);
                                            MessageBox.Show("打印成功！");
                                            this.SN1_num.Text = sn1;
                                            long imei_star14 = long.Parse(this.IMEI_Start.Text.Substring(0, 14)) + 1;
                                            //string imei_star15 = getimei15(imei_star14.ToString());
                                            //string imei_star = imei_star14.ToString() + imei_star15;
                                            this.IMEI_Start.Clear();
                                            this.IMEI_Present.Text = imei_star14.ToString();
                                            this.SN1_num.ReadOnly = true;
                                            this.SN2_num.ReadOnly = true;
                                            this.VIP_num1.ReadOnly = true;
                                            this.VIP_num2.ReadOnly = true;
                                        }
                                    }
                                }
                                else
                                {
                                    btFormat.SubStrings["SN"].Value = "";
                                    list.Add(new PrintMessage()
                                    {
                                        Zhidan = this.CB_ZhiDan.Text.Trim(),
                                        IMEI = this.IMEI_Start.Text.Trim(),
                                        IMEIStart = this.IMEI_num1.Text.Trim(),
                                        IMEIEnd = this.IMEI_num2.Text.Trim(),
                                        SN = "",
                                        IMEIRel = this.IMEIRel.Text.Trim(),
                                        SIM = "",
                                        VIP = "",
                                        BAT = "",
                                        SoftModel = this.SoftModel.Text.Trim(),
                                        Version = this.SoftwareVersion.Text.Trim(),
                                        Remark = this.Remake.Text.Trim(),
                                        JS_PrintTime = ProductTime,
                                        JS_TemplatePath = this.Select_Template1.Text.Trim(),
                                        CH_PrintTime = "",
                                        CH_TemplatePath1 = null,
                                        CH_TemplatePath2 = null
                                    });
                                    if (PMB.InsertPrintMessageBLL(list))
                                    {
                                        string sn2_suffix;
                                        if (this.SN2_num.Text != "")
                                        {
                                            sn2_suffix = SN2_num.Text.Remove(0, (this.SN1_num.Text.Length) - 5);
                                        }
                                        else
                                        {
                                            sn2_suffix = "";
                                        }
                                        if (MOPB.UpdateSNnumberBLL(this.CB_ZhiDan.Text, "", "", sn2_suffix, this.VIP_num1.Text, this.VIP_num2.Text, long.Parse(this.IMEI_Start.Text.Substring(0, 14)) + 1))
                                        {
                                            Result nResult1 = btFormat.Print("标签打印软件", waitout, out messages);
                                            Form1.Log("打印了IMEI号为" + this.IMEI_Start.Text + "的制单", null);
                                            MessageBox.Show("打印成功！");
                                            long imei_star14 = long.Parse(this.IMEI_Start.Text.Substring(0, 14)) + 1;
                                            //string imei_star15 = getimei15(imei_star14.ToString());
                                            //string imei_star = imei_star14.ToString() + imei_star15;
                                            this.IMEI_Start.Clear();
                                            this.IMEI_Present.Text = imei_star14.ToString();
                                            this.SN1_num.ReadOnly = true;
                                            this.SN2_num.ReadOnly = true;
                                            this.VIP_num1.ReadOnly = true;
                                            this.VIP_num2.ReadOnly = true;
                                        }
                                    }
                                }
                            }
                            else if (PMB.CheckCHOrJSIMEIBLL(this.IMEI_Start.Text, 1))
                            {
                                string ProductTime = "";
                                string NowTime = System.DateTime.Now.ToString("HH:mm:ss:fff");
                                if (this.ProductData.Text == "")
                                {
                                    ProductTime = System.DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:fff");
                                }
                                else
                                {
                                    ProductTime = this.ProductData.Text.Trim() + " " + NowTime;
                                }
                                list = PMB.SelectSnByIMEIBLL(this.IMEI_Start.Text);
                                foreach (PrintMessage a in list)
                                {
                                    btFormat.SubStrings["SN"].Value = a.SN;
                                }
                                if (MOPB.UpdateJSmesBLL(this.IMEI_Start.Text, ProductTime, lj))
                                {
                                    Result nResult1 = btFormat.Print("标签打印软件", waitout, out messages);
                                    Form1.Log("打印了机身贴IMEI号为" + this.IMEI_Start.Text + "的制单", null);
                                    MessageBox.Show("打印成功");
                                    this.IMEI_Start.Clear();
                                }
                            }
                            else
                            {
                                MessageBox.Show("该IMEI号已被打印过！");
                            }
                        }
                        else
                        {
                            MessageBox.Show("IMEI打印号不能为空！");
                        }
                        btFormat.PrintSetup.Cache.FlushInterval = CacheFlushInterval.PerSession;
                        MOPB.UpdateStatusByZhiDanBLL(this.CB_ZhiDan.Text);
                    }
                    else if (this.PrintMore.Checked == true)
                    {
                        long imei_begin;
                        string imei15,sn_bef,sn_aft,sn_laf;
                        if (this.PrintNum.Text != "")
                        {
                            if (this.IMEI_Present.Text != "")
                            {
                                imei_begin = long.Parse(this.IMEI_Present.Text);
                            }
                            else
                            {
                                imei_begin = long.Parse(this.IMEI_num1.Text);
                            }
                            if (this.SN1_num.Text != "")
                            {
                                sn_bef = this.SN1_num.Text.Substring(0, this.SN1_num.Text.Length - 5);
                                sn_aft = this.SN1_num.Text.Remove(0, this.SN1_num.Text.Length - 5);
                                sn_laf = this.SN2_num.Text.Remove(0, this.SN2_num.Text.Length - 5);
                                for (int i = 0; i < int.Parse(this.PrintNum.Text); i++)
                                {
                                    if (int.Parse(sn_aft) < int.Parse(sn_laf))
                                    {
                                        imei15 = getimei15(imei_begin.ToString());
                                        btFormat.SubStrings["IMEI"].Value = imei_begin.ToString() + imei15;
                                        if (!PMB.CheckIMEIBLL(imei_begin.ToString() + imei15))
                                        {
                                            btFormat.SubStrings["SN"].Value = sn_bef + sn_aft;
                                            //记录打印信息日志
                                            string ProductTime = "";
                                            string NowTime = System.DateTime.Now.ToString("HH:mm:ss:fff");
                                            if (this.ProductData.Text == "")
                                            {
                                                ProductTime = System.DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:fff");
                                            }
                                            else
                                            {
                                                ProductTime = this.ProductData.Text.Trim() + " " + NowTime;
                                            }
                                            list.Add(new PrintMessage()
                                            {
                                                Zhidan = this.CB_ZhiDan.Text.Trim(),
                                                IMEI = imei_begin.ToString() + imei15,
                                                IMEIStart = this.IMEI_num1.Text.Trim(),
                                                IMEIEnd = this.IMEI_num2.Text.Trim(),
                                                SN = sn_bef + sn_aft,
                                                IMEIRel = this.IMEIRel.Text.Trim(),
                                                SIM = "",
                                                VIP = "",
                                                BAT = "",
                                                SoftModel = this.SoftModel.Text.Trim(),
                                                Version = this.SoftwareVersion.Text.Trim(),
                                                Remark = this.Remake.Text.Trim(),
                                                JS_PrintTime = ProductTime,
                                                JS_TemplatePath = this.Select_Template1.Text.Trim(),
                                                CH_PrintTime = "",
                                                CH_TemplatePath1 = null,
                                                CH_TemplatePath2 = null
                                            });
                                            if (PMB.InsertPrintMessageBLL(list))
                                            {
                                                Result nResult1 = btFormat.Print("标签打印软件", waitout, out messages);
                                                Form1.Log("批量打印了IMEI号为" + imei_begin + imei15 + "的制单", null);
                                                imei_begin++;
                                                sn_aft = (int.Parse(sn_aft) + 1).ToString().PadLeft(5, '0');
                                            }
                                        }
                                        else if (PMB.CheckCHOrJSIMEIBLL(imei_begin.ToString() + imei15, 1))
                                        {
                                            string ProductTime = "";
                                            string NowTime = System.DateTime.Now.ToString("HH:mm:ss:fff");
                                            if (this.ProductData.Text == "")
                                            {
                                                ProductTime = System.DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:fff");
                                            }
                                            else
                                            {
                                                ProductTime = this.ProductData.Text.Trim() + " " + NowTime;
                                            }
                                            list = PMB.SelectSnByIMEIBLL(imei_begin.ToString() + imei15);
                                            foreach (PrintMessage a in list)
                                            {
                                                btFormat.SubStrings["SN"].Value = a.SN;
                                            }
                                            if (MOPB.UpdateJSmesBLL(imei_begin.ToString()+ imei15, ProductTime, lj))
                                            {
                                                Result nResult1 = btFormat.Print("标签打印软件", waitout, out messages);
                                                Form1.Log("批量打印了机身贴IMEI号为" + imei_begin.ToString() + imei15 + "的制单", null);
                                                imei_begin++;
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("该" + imei_begin + imei15 + "IMEI号已被打印过！");
                                            imei_begin++;
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("SN号不足！");
                                    }
                                }
                                if (MOPB.UpdateSNnumberBLL(this.CB_ZhiDan.Text, sn_bef, sn_aft, sn_laf, this.VIP_num1.Text, this.VIP_num2.Text, imei_begin))
                                {
                                    MessageBox.Show("打印成功！");
                                    this.SN1_num.Text = sn_bef + sn_aft;
                                    this.IMEI_Present.Text = imei_begin.ToString();
                                    this.PrintNum.Clear();
                                }
                            }
                            else
                            {
                                for (int i = 0; i < int.Parse(this.PrintNum.Text); i++)
                                {
                                    imei15 = getimei15(imei_begin.ToString());
                                    btFormat.SubStrings["IMEI"].Value = imei_begin.ToString() + imei15;
                                    btFormat.SubStrings["SN"].Value = "";
                                    if (!PMB.CheckIMEIBLL(imei_begin.ToString() + imei15))
                                    {
                                        //记录打印信息日志
                                        string ProductTime = "";
                                        string NowTime = System.DateTime.Now.ToString("HH:mm:ss:fff");
                                        if (this.ProductData.Text == "")
                                        {
                                            ProductTime = System.DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:fff");
                                        }
                                        else
                                        {
                                            ProductTime = this.ProductData.Text.Trim() + " " + NowTime;
                                        }
                                        list.Add(new PrintMessage()
                                        {
                                            Zhidan = this.CB_ZhiDan.Text.Trim(),
                                            IMEI = imei_begin.ToString() + imei15,
                                            IMEIStart = this.IMEI_num1.Text.Trim(),
                                            IMEIEnd = this.IMEI_num2.Text.Trim(),
                                            SN = "",
                                            IMEIRel = this.IMEIRel.Text.Trim(),
                                            SIM = "",
                                            VIP = "",
                                            BAT = "",
                                            SoftModel = this.SoftModel.Text.Trim(),
                                            Version = this.SoftwareVersion.Text.Trim(),
                                            Remark = this.Remake.Text.Trim(),
                                            JS_PrintTime = ProductTime,
                                            JS_TemplatePath = this.Select_Template1.Text.Trim(),
                                            CH_PrintTime = "",
                                            CH_TemplatePath1 = null,
                                            CH_TemplatePath2 = null
                                        });
                                        if (PMB.InsertPrintMessageBLL(list))
                                        {
                                            Result nResult1 = btFormat.Print("标签打印软件", waitout, out messages);
                                            Form1.Log("批量打印了IMEI号为" + imei_begin + imei15 + "的制单", null);
                                            imei_begin++;
                                        }
                                    }
                                    else if (PMB.CheckCHOrJSIMEIBLL(imei_begin.ToString() + imei15, 1))
                                    {
                                        string ProductTime = "";
                                        string NowTime = System.DateTime.Now.ToString("HH:mm:ss:fff");
                                        if (this.ProductData.Text == "")
                                        {
                                            ProductTime = System.DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:fff");
                                        }
                                        else
                                        {
                                            ProductTime = this.ProductData.Text.Trim() + " " + NowTime;
                                        }
                                        list = PMB.SelectSnByIMEIBLL(this.IMEI_Start.Text);
                                        foreach (PrintMessage a in list)
                                        {
                                            btFormat.SubStrings["SN"].Value = a.SN;
                                        }
                                        if (MOPB.UpdateJSmesBLL(this.IMEI_Start.Text, ProductTime, lj))
                                        {
                                            Result nResult1 = btFormat.Print("标签打印软件", waitout, out messages);
                                            Form1.Log("批量打印了机身贴IMEI号为" + this.IMEI_Start.Text + "的制单", null);
                                            imei_begin++;
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("该" + imei_begin + imei15 + "IMEI号已被打印过！");
                                    }
                                }
                                if (MOPB.UpdateSNnumberBLL(this.CB_ZhiDan.Text, "", "", "", this.VIP_num1.Text, this.VIP_num2.Text, imei_begin))
                                {
                                    MessageBox.Show("打印成功！");
                                    this.IMEI_Present.Text = imei_begin.ToString();
                                    this.PrintNum.Clear();
                                }
                            } 
                        }
                        else {
                            MessageBox.Show("请输入批量打印份数");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("请先选择模板");
                }
                //结束打印引擎
                btEngine.Stop();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception:" + ex.Message);
            }
        }

        private void CB_ZhiDan_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.IMEI_Start.Clear();
            this.PrintNum.Clear();
            this.ProductData.Clear();
            this.Re_IMEINum.Clear();
            string NowData = System.DateTime.Now.ToString("yyyy.MM.dd");
            this.ProductData.Text = NowData;
            string ZhidanNum = this.CB_ZhiDan.Text;
            G_MOP = MOPB.selectManuOrderParamByzhidanBLL(ZhidanNum);
            foreach (Gps_ManuOrderParam b in G_MOP)
            {
                this.SoftModel.Text = b.SoftModel;
                this.SN1_num.Text = b.SN1 + b.SN2;
                this.SN2_num.Text = b.SN1 + b.SN3;
                this.IMEI_Present.Text = b.IMEIPrints;
                this.ProductNo.Text = b.ProductNo;
                this.SoftwareVersion.Text = b.Version;
                this.IMEI_num1.Text = b.IMEIStart;
                this.IMEI_num2.Text = b.IMEIEnd;
                this.SIM_num1.Text = b.SIMStart;
                this.SIM_num2.Text = b.SIMEnd;
                this.BAT_num1.Text = b.BATStart;
                this.BAT_num2.Text = b.BATEnd;
                this.VIP_num1.Text = b.VIPStart;
                this.VIP_num2.Text = b.VIPEnd;
                this.IMEI_Present.Text = b.IMEIPrints;
                if (b.Remark1 != "")
                {
                    string rem = b.Remark1;
                    string[] remark = rem.Split('：');
                    this.Remake.Text = remark[1];
                }
                else
                {
                    this.Remake.Text = b.Remark1;
                }
                if (int.Parse(b.IMEIRel) == 0)
                {
                    this.IMEIRel.Text = "无绑定";
                }
                else if (int.Parse(b.IMEIRel) == 1)
                {
                    this.IMEIRel.Text = "与SIM卡绑定";
                }
                else if (int.Parse(b.IMEIRel) == 2)
                {
                    this.IMEIRel.Text = "与SIM&BAT绑定";
                }
                else if (int.Parse(b.IMEIRel) == 3)
                {
                    this.IMEIRel.Text = "与SIM&VIP绑定";
                }
                else if (int.Parse(b.IMEIRel) == 4)
                {
                    this.IMEIRel.Text = "与BAT绑定";
                }
                if (b.status == 1)
                {
                    this.SN1_num.ReadOnly = true;
                    this.SN2_num.ReadOnly = true;
                    this.VIP_num1.ReadOnly = true;
                    this.VIP_num2.ReadOnly = true;
                }
                else
                {
                    this.SN1_num.ReadOnly = false;
                    this.SN2_num.ReadOnly = false;
                    this.VIP_num1.ReadOnly = false;
                    this.VIP_num2.ReadOnly = false;
                }
            }
        }
        
        static bool IsNumeric(string s)
        {
            double v;
            if (double.TryParse(s, out v))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string getimei15(string imei) {
            if (imei.Length == 14)
            {
                Char[] imeiChar = imei.ToCharArray();
                int resultInt = 0;
                for (int i = 0; i < imeiChar.Length; i++)
                {
                    int a = int.Parse(imeiChar[i].ToString());
                    i++;
                    int temp = int.Parse(imeiChar[i].ToString()) * 2;
                    int b = temp < 10 ? temp : temp - 9;
                    resultInt += a + b;
                }
                resultInt %= 10;
                resultInt = resultInt == 0 ? 0 : 10 - resultInt;
                return resultInt + "";
            }
            else { return ""; }
        }

        private void Re_IMEINum_Leave(object sender, EventArgs e)
        {
            if (this.Re_IMEINum.Text != "")
            {
                if (IsNumeric(this.Re_IMEINum.Text))
                {
                    if (this.Re_IMEINum.Text.Length != 15)
                    {
                        MessageBox.Show("IMEI号只能为15位纯数字，请重新输入！");
                        this.Re_IMEINum.Focus();
                        this.Re_IMEINum.Clear();
                    }
                    else {
                        string imeiRes;
                        string imei14 = this.Re_IMEINum.Text.Substring(0, 14);
                        string imei15 = getimei15(imei14);
                        imeiRes = imei14 + imei15;
                        if (imeiRes != this.Re_IMEINum.Text)
                        {
                            MessageBox.Show("校验出错,请重新输入");
                            this.Re_IMEINum.Clear();
                            this.Re_IMEINum.Focus();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("IMEI号格式不正确,请重新设置！");
                    this.Re_IMEINum.Clear();
                }
            }
        }

        public bool IsDate(string strDate)
        {
            try
            {
                DateTime.Parse(strDate);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void ProductData_MouseLeave(object sender, EventArgs e)
        {
            if (this.ProductData.Text != "")
            {
                if (!IsDate(this.ProductData.Text))
                {
                    MessageBox.Show("请输入正确的日期格式");
                    this.ProductData.Text = System.DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
        }

        private void ProductData_Leave(object sender, EventArgs e)
        {
            if (this.ProductData.Text != "")
            {
                if (!IsDate(this.ProductData.Text))
                {
                    MessageBox.Show("请输入正确的日期格式");
                    this.ProductData.Text = System.DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
        }

        //判断是否有中文字符
        public static bool HasChinese(string str)
        {
            return Regex.IsMatch(str, @"[\u4e00-\u9fa5]");
        }

        private void Re_print_button_Click(object sender, EventArgs e)
        {
            try
            {
                Engine btEngine = new Engine();
                btEngine.Start();
                string lj = "";
                if (this.Select_Template1.Text != "")
                {
                    lj = this.Select_Template1.Text;
                    LabelFormatDocument btFormat = btEngine.Documents.Open(lj);
                    //指定打印机名称
                    btFormat.PrintSetup.PrinterName = this.Printer1.Text;
                    //对模板相应字段进行赋值
                    GetValue("Information", "生产日期", out outString);
                    btFormat.SubStrings[outString].Value = this.ProductData.Text;
                    //打印份数,同序列打印的份数
                    btFormat.PrintSetup.IdenticalCopiesOfLabel = 1;

                    Messages messages;
                    int waitout = 10000;
                    if (this.Re_IMEINum.Text != "")
                    {
                        btFormat.SubStrings["IMEI"].Value = this.Re_IMEINum.Text;
                        if (PMB.CheckReCHOrJSIMEIBLL(this.Re_IMEINum.Text, 1))
                        {
                            list = PMB.SelectSnByIMEIBLL(this.Re_IMEINum.Text);
                            foreach (PrintMessage a in list)
                            {
                                GetValue("Information", "型号", out outString);
                                btFormat.SubStrings[outString].Value = a.SoftModel;
                                GetValue("Information", "SIM卡号", out outString);
                                btFormat.SubStrings[outString].Value = a.SIM;
                                GetValue("Information", "服务卡号", out outString);
                                btFormat.SubStrings[outString].Value = a.VIP;
                                btFormat.SubStrings["BAT"].Value = a.BAT;
                                GetValue("Information", "SN", out outString);
                                btFormat.SubStrings["SN"].Value = a.SN;
                                GetValue("Information", "备注", out outString);
                                btFormat.SubStrings[outString].Value = a.Remark;
                            }
                            //更新打印信息到数据表
                            string NowTime = System.DateTime.Now.ToString("HH:mm:ss:fff");
                            string NowData = System.DateTime.Now.ToString("yyyy-MM-dd");
                            string RE_PrintTime = NowData + " " + NowTime;
                            if (PMB.UpdateRePrintBLL(this.Re_IMEINum.Text, RE_PrintTime, 1, lj,lj))
                            {
                                Result nResult1 = btFormat.Print("标签打印软件", waitout, out messages);
                                Form1.Log("重打了IMEI号为" + this.Re_IMEINum.Text + "的制单", null);
                                btFormat.PrintSetup.Cache.FlushInterval = CacheFlushInterval.PerSession;
                                MessageBox.Show("打印成功！");
                            }
                        }
                        else
                        {
                            MessageBox.Show("该IMEI号还未被打印，不需重打！");
                        }
                    }
                    else
                    {
                        MessageBox.Show("重打IMEI号不能为空！");
                    }
                }
                else
                {
                    MessageBox.Show("请先选择模板");
                }
                //结束打印引擎
                btEngine.Stop();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception:" + ex.Message);
            }
        }

        private void Open_Template1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            string path = ofd.FileName;
            string strExtension = "";
            int nIndex = path.LastIndexOf('.');
            if (nIndex > 0) {
                strExtension = path.Substring(nIndex);
                if (strExtension != ".btw")
                {
                    MessageBox.Show("请选择正确的btw文件！");
                }
                else {
                    this.Select_Template1.Text = path;
                }
            }

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            tabControl1.SelectedTab.Refresh();
            if (tabControl1.SelectedTab == tabPage2)
            {
                Color_Box CB = new Color_Box();
                CB.TopLevel = false;
                tabPage2.Controls.Add(CB);
                CB.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                CB.Show();
            }
            else if (tabControl1.SelectedTab == tabPage3) {
                CheckRePrint CRP = new CheckRePrint();
                CRP.TopLevel = false;
                tabPage3.Controls.Add(CRP);
                CRP.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                CRP.Show();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("是否退出系统？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
            {
                e.Cancel = true;
            }
        }

        private void tabControl2_DrawItem(object sender, DrawItemEventArgs e)
        {
            Rectangle tabArea = tabControl2.GetTabRect(e.Index);//主要是做个转换来获得TAB项的RECTANGELF 
            RectangleF tabTextArea = (RectangleF)(tabControl2.GetTabRect(e.Index));
            Graphics g = e.Graphics;
            StringFormat sf = new StringFormat();//封装文本布局信息 
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;
            Font font = this.tabControl2.Font;
            SolidBrush brush = new SolidBrush(Color.Black);//绘制边框的画笔 
            g.DrawString(((TabControl)(sender)).TabPages[e.Index].Text, font, brush, tabTextArea, sf);
        }

        private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl2.SelectedTab == CheckAndDelete)
            {
                JST_CheckAndDelect JSTCAD = new JST_CheckAndDelect();
                JSTCAD.TopLevel = false;
                CheckAndDelete.Controls.Add(JSTCAD);
                JSTCAD.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                JSTCAD.Show();
            }
            else if (tabControl2.SelectedTab == ExcelToPrint)
            {
                PrintFromExcel PFE = new PrintFromExcel();
                PFE.TopLevel = false;
                ExcelToPrint.Controls.Add(PFE);
                PFE.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                PFE.Show();
                //MessageBox.Show("该功能暂建设中...");
            }
        }

        private void Debug_print_Click(object sender, EventArgs e)
        {
            try
            {
                Engine btEngine = new Engine();
                btEngine.Start();
                string lj = "";
                if (this.Select_Template1.Text != "")
                {
                    lj = this.Select_Template1.Text;
                    LabelFormatDocument btFormat = btEngine.Documents.Open(lj);
                    //指定打印机名称
                    btFormat.PrintSetup.PrinterName = this.Printer1.Text;
                    //对模板相应字段进行赋值
                    GetValue("Information", "IMEI", out outString);
                    btFormat.SubStrings[outString].Value = this.IMEI_num1.Text;
                    GetValue("Information", "SN", out outString);
                    btFormat.SubStrings[outString].Value = this.SN1_num.Text;
                    GetValue("Information", "型号", out outString);
                    btFormat.SubStrings[outString].Value = this.SoftModel.Text;
                    GetValue("Information", "产品编码", out outString);
                    btFormat.SubStrings[outString].Value = this.ProductNo.Text;
                    GetValue("Information", "软件版本", out outString);
                    btFormat.SubStrings[outString].Value = this.SoftwareVersion.Text;
                    GetValue("Information", "SIM卡号", out outString);
                    btFormat.SubStrings[outString].Value = this.SIM_num1.Text;
                    GetValue("Information", "服务卡号", out outString);
                    btFormat.SubStrings[outString].Value = this.VIP_num1.Text;
                    GetValue("Information", "备注", out outString);
                    btFormat.SubStrings[outString].Value = this.Remake.Text;
                    GetValue("Information", "生产日期", out outString);
                    btFormat.SubStrings[outString].Value = this.ProductData.Text;
                    //打印份数,同序列打印的份数
                    btFormat.PrintSetup.IdenticalCopiesOfLabel = 1;
                    Messages messages;
                    int waitout = 10000;
                    Result nResult1 = btFormat.Print("标签打印软件", waitout, out messages);
                    Form1.Log("调试打印了机身贴IMEI号为" + this.IMEI_num1.Text + "的制单", null);
                    btFormat.PrintSetup.Cache.FlushInterval = CacheFlushInterval.PerSession;
                    MessageBox.Show("调试打印成功！");
                    btEngine.Stop();
                }
                else
                {
                    MessageBox.Show("请先选择模板");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception:" + ex.Message);
            }
        }

        private void PrintOne_Click(object sender, EventArgs e)
        {
            if (this.PrintOne.Checked == true)
            {
                this.PrintMore.Checked = false;
                this.IMEI_Start.ReadOnly = false;
                this.PrintNum.ReadOnly = true;
                this.PrintNum.Clear();
            }
            else
            {
                this.PrintOne.Checked = true;
            }
        }

        private void PrintMore_Click(object sender, EventArgs e)
        {
            if (this.PrintMore.Checked == true)
            {
                this.PrintOne.Checked = false;
                this.PrintNum.ReadOnly = false;
                this.IMEI_Start.ReadOnly = true;
                this.IMEI_Start.Clear();
            }
            else
            {
                this.PrintMore.Checked = true;
            }
        }

        private void PrintNum_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) {
                if (this.CB_ZhiDan.Text != "")
                {
                    if (this.PrintNum.Text != "" && IsNumeric(this.PrintNum.Text))
                    {
                        long between = long.Parse(this.IMEI_num2.Text) - long.Parse(this.IMEI_num1.Text);
                        if (int.Parse(this.PrintNum.Text) < 0)
                        {
                            MessageBox.Show("请输入0--" + between + "之间的数！");
                            this.PrintNum.Clear();
                            this.PrintNum.Focus();
                            return;
                        }
                        else if (int.Parse(this.PrintNum.Text) > between)
                        {
                            MessageBox.Show("请输入0--" + between + "之间的数！");
                            this.PrintNum.Clear();
                            this.PrintNum.Focus();
                            return;
                        }
                    }
                    else if (this.PrintNum.Text == "")
                    {

                    }
                    else
                    {
                        MessageBox.Show("输入格式不正确，只允许输入数字！");
                        this.PrintNum.Clear();
                        this.PrintNum.Focus();
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("请先选择制单号");
                    this.PrintNum.Clear();
                    this.PrintNum.Focus();
                    return;
                }
                try
                {
                    Engine btEngine = new Engine();
                    btEngine.Start();
                    string lj = "";
                    if (this.Select_Template1.Text != "")
                    {
                        lj = this.Select_Template1.Text;
                        LabelFormatDocument btFormat = btEngine.Documents.Open(lj);
                        //对模板相应字段进行赋值
                        GetValue("Information", "型号", out outString);
                        btFormat.SubStrings[outString].Value = this.SoftModel.Text;
                        GetValue("Information", "生产日期", out outString);
                        btFormat.SubStrings[outString].Value = this.ProductData.Text;
                        GetValue("Information", "产品编码", out outString);
                        btFormat.SubStrings[outString].Value = this.ProductNo.Text;
                        GetValue("Information", "软件版本", out outString);
                        btFormat.SubStrings[outString].Value = this.SoftwareVersion.Text;
                        GetValue("Information", "备注", out outString);
                        btFormat.SubStrings[outString].Value = this.Remake.Text;

                        //指定打印机名称
                        btFormat.PrintSetup.PrinterName = this.Printer1.Text;
                        //打印份数,同序列打印的份数
                        btFormat.PrintSetup.IdenticalCopiesOfLabel = 1;

                        Messages messages;
                        int waitout = 10000;
                        if (this.PrintMore.Checked == true)
                        {
                            long imei_begin;
                            string imei15, sn_bef, sn_aft, sn_laf;
                            if (this.PrintNum.Text != "")
                            {
                                if (this.IMEI_Present.Text != "")
                                {
                                    imei_begin = long.Parse(this.IMEI_Present.Text);
                                }
                                else
                                {
                                    imei_begin = long.Parse(this.IMEI_num1.Text);
                                }
                                if (this.SN1_num.Text != "")
                                {
                                    sn_bef = this.SN1_num.Text.Substring(0, this.SN1_num.Text.Length - 5);
                                    sn_aft = this.SN1_num.Text.Remove(0, this.SN1_num.Text.Length - 5);
                                    sn_laf = this.SN2_num.Text.Remove(0, this.SN2_num.Text.Length - 5);
                                    for (int i = 0; i < int.Parse(this.PrintNum.Text); i++)
                                    {
                                        if (int.Parse(sn_aft) < int.Parse(sn_laf))
                                        {
                                            imei15 = getimei15(imei_begin.ToString());
                                            btFormat.SubStrings["IMEI"].Value = imei_begin.ToString() + imei15;
                                            if (!PMB.CheckIMEIBLL(imei_begin.ToString() + imei15))
                                            {
                                                btFormat.SubStrings["SN"].Value = sn_bef + sn_aft;
                                                //记录打印信息日志
                                                string ProductTime = "";
                                                string NowTime = System.DateTime.Now.ToString("HH:mm:ss:fff");
                                                if (this.ProductData.Text == "")
                                                {
                                                    ProductTime = System.DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:fff");
                                                }
                                                else
                                                {
                                                    ProductTime = this.ProductData.Text.Trim() + " " + NowTime;
                                                }
                                                list.Add(new PrintMessage()
                                                {
                                                    Zhidan = this.CB_ZhiDan.Text.Trim(),
                                                    IMEI = imei_begin.ToString() + imei15,
                                                    IMEIStart = this.IMEI_num1.Text.Trim(),
                                                    IMEIEnd = this.IMEI_num2.Text.Trim(),
                                                    SN = sn_bef + sn_aft,
                                                    IMEIRel = this.IMEIRel.Text.Trim(),
                                                    SIM = "",
                                                    VIP = "",
                                                    BAT = "",
                                                    SoftModel = this.SoftModel.Text.Trim(),
                                                    Version = this.SoftwareVersion.Text.Trim(),
                                                    Remark = this.Remake.Text.Trim(),
                                                    JS_PrintTime = ProductTime,
                                                    JS_TemplatePath = this.Select_Template1.Text.Trim(),
                                                    CH_PrintTime = "",
                                                    CH_TemplatePath1 = null,
                                                    CH_TemplatePath2 = null
                                                });
                                                if (PMB.InsertPrintMessageBLL(list))
                                                {
                                                    Result nResult1 = btFormat.Print("标签打印软件", waitout, out messages);
                                                    Form1.Log("批量打印了IMEI号为" + imei_begin + imei15 + "的制单", null);
                                                    imei_begin++;
                                                    sn_aft = (int.Parse(sn_aft) + 1).ToString().PadLeft(5, '0');
                                                }
                                            }
                                            else if (PMB.CheckCHOrJSIMEIBLL(imei_begin.ToString() + imei15, 1))
                                            {
                                                string ProductTime = "";
                                                string NowTime = System.DateTime.Now.ToString("HH:mm:ss:fff");
                                                if (this.ProductData.Text == "")
                                                {
                                                    ProductTime = System.DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:fff");
                                                }
                                                else
                                                {
                                                    ProductTime = this.ProductData.Text.Trim() + " " + NowTime;
                                                }
                                                list = PMB.SelectSnByIMEIBLL(imei_begin.ToString() + imei15);
                                                foreach (PrintMessage a in list)
                                                {
                                                    btFormat.SubStrings["SN"].Value = a.SN;
                                                }
                                                if (MOPB.UpdateJSmesBLL(imei_begin.ToString() + imei15, ProductTime, lj))
                                                {
                                                    Result nResult1 = btFormat.Print("标签打印软件", waitout, out messages);
                                                    Form1.Log("批量打印了机身贴IMEI号为" + imei_begin.ToString() + imei15 + "的制单", null);
                                                    imei_begin++;
                                                }
                                            }
                                            else
                                            {
                                                MessageBox.Show("该" + imei_begin + imei15 + "IMEI号已被打印过！");
                                                imei_begin++;
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("SN号不足！");
                                        }
                                    }
                                    if (MOPB.UpdateSNnumberBLL(this.CB_ZhiDan.Text, sn_bef, sn_aft, sn_laf, this.VIP_num1.Text, this.VIP_num2.Text, imei_begin))
                                    {
                                        this.SN1_num.Text = sn_bef + sn_aft;
                                        this.IMEI_Present.Text = imei_begin.ToString();
                                        this.PrintNum.Clear();
                                        this.PrintNum.Focus();
                                    }
                                }
                                else
                                {
                                    for (int i = 0; i < int.Parse(this.PrintNum.Text); i++)
                                    {
                                        imei15 = getimei15(imei_begin.ToString());
                                        btFormat.SubStrings["IMEI"].Value = imei_begin.ToString() + imei15;
                                        btFormat.SubStrings["SN"].Value = "";
                                        if (!PMB.CheckIMEIBLL(imei_begin.ToString() + imei15))
                                        {
                                            //记录打印信息日志
                                            string ProductTime = "";
                                            string NowTime = System.DateTime.Now.ToString("HH:mm:ss:fff");
                                            if (this.ProductData.Text == "")
                                            {
                                                ProductTime = System.DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:fff");
                                            }
                                            else
                                            {
                                                ProductTime = this.ProductData.Text.Trim() + " " + NowTime;
                                            }
                                            list.Add(new PrintMessage()
                                            {
                                                Zhidan = this.CB_ZhiDan.Text.Trim(),
                                                IMEI = imei_begin.ToString() + imei15,
                                                IMEIStart = this.IMEI_num1.Text.Trim(),
                                                IMEIEnd = this.IMEI_num2.Text.Trim(),
                                                SN = "",
                                                IMEIRel = this.IMEIRel.Text.Trim(),
                                                SIM = "",
                                                VIP = "",
                                                BAT = "",
                                                SoftModel = this.SoftModel.Text.Trim(),
                                                Version = this.SoftwareVersion.Text.Trim(),
                                                Remark = this.Remake.Text.Trim(),
                                                JS_PrintTime = ProductTime,
                                                JS_TemplatePath = this.Select_Template1.Text.Trim(),
                                                CH_PrintTime = "",
                                                CH_TemplatePath1 = null,
                                                CH_TemplatePath2 = null
                                            });
                                            if (PMB.InsertPrintMessageBLL(list))
                                            {
                                                Result nResult1 = btFormat.Print("标签打印软件", waitout, out messages);
                                                Form1.Log("批量打印了IMEI号为" + imei_begin + imei15 + "的制单", null);
                                                imei_begin++;
                                            }
                                        }
                                        else if (PMB.CheckCHOrJSIMEIBLL(imei_begin.ToString() + imei15, 1))
                                        {
                                            string ProductTime = "";
                                            string NowTime = System.DateTime.Now.ToString("HH:mm:ss:fff");
                                            if (this.ProductData.Text == "")
                                            {
                                                ProductTime = System.DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:fff");
                                            }
                                            else
                                            {
                                                ProductTime = this.ProductData.Text.Trim() + " " + NowTime;
                                            }
                                            list = PMB.SelectSnByIMEIBLL(this.IMEI_Start.Text);
                                            foreach (PrintMessage a in list)
                                            {
                                                btFormat.SubStrings["SN"].Value = a.SN;
                                            }
                                            if (MOPB.UpdateJSmesBLL(this.IMEI_Start.Text, ProductTime, lj))
                                            {
                                                Result nResult1 = btFormat.Print("标签打印软件", waitout, out messages);
                                                Form1.Log("批量打印了机身贴IMEI号为" + this.IMEI_Start.Text + "的制单", null);
                                                imei_begin++;
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("该" + imei_begin + imei15 + "IMEI号已被打印过！");
                                        }
                                    }
                                    if (MOPB.UpdateSNnumberBLL(this.CB_ZhiDan.Text, "", "", "", this.VIP_num1.Text, this.VIP_num2.Text, imei_begin))
                                    {
                                        this.IMEI_Present.Text = imei_begin.ToString();
                                        this.PrintNum.Clear();
                                        this.PrintNum.Focus();
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("请输入批量打印份数");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("请先选择模板");
                        this.PrintNum.Clear();
                        this.PrintNum.Focus();
                    }
                    //结束打印引擎
                    btEngine.Stop();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Exception:" + ex.Message);
                }
            }
        }

        private void IMEI_Start_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) {
                if (this.CB_ZhiDan.Text != "")
                {
                    string imei14;
                    String imeiRes = "";
                    if (this.IMEI_Start.Text != "" && IsNumeric(this.IMEI_Start.Text))
                    {
                        imei14 = this.IMEI_Start.Text.Substring(0, 14);
                        long IMEI_Start = long.Parse(imei14);
                        if (IMEI_Start < long.Parse(this.IMEI_num1.Text))
                        {
                            MessageBox.Show("IMEI打印起始位不能小于IMEI起始位,请重新设置！");
                            this.IMEI_Start.Clear();
                            this.IMEI_Start.Focus();
                            return;
                        }
                        else if (IMEI_Start > long.Parse(this.IMEI_num2.Text))
                        {
                            MessageBox.Show("IMEI打印起始位不能大于IMEI终止位,请重新设置！");
                            this.IMEI_Start.Clear();
                            this.IMEI_Start.Focus();
                            return;
                        }
                        else if (this.IMEI_Start.Text.Length != 15)
                        {
                            MessageBox.Show("IMEI号只能为15位纯数字，请重新输入！");
                            this.IMEI_Start.Clear();
                            this.IMEI_Start.Focus();
                            return;
                        }
                        else
                        {
                            string imei15 = getimei15(imei14);
                            imeiRes = imei14 + imei15;
                            if (imeiRes != this.IMEI_Start.Text)
                            {
                                MessageBox.Show("校验出错,请重新输入");
                                this.IMEI_Start.Clear();
                                this.IMEI_Start.Focus();
                                return;
                            }
                        }
                    }
                    else if (this.IMEI_Start.Text == "")
                    { }
                    else
                    {
                        MessageBox.Show("IMEI号格式不正确，请重新输入！");
                        this.IMEI_Start.Clear();
                        this.IMEI_Start.Focus();
                        return;
                    }
                }
                else {
                    MessageBox.Show("请先选择制单号");
                    this.IMEI_Start.Clear();
                    this.IMEI_Start.Focus();
                    return;
                }
                try
                {
                    Engine btEngine = new Engine();
                    btEngine.Start();
                    string lj = "";
                    if (this.Select_Template1.Text != "")
                    {
                        lj = this.Select_Template1.Text;
                        LabelFormatDocument btFormat = btEngine.Documents.Open(lj);
                        //对模板相应字段进行赋值
                        GetValue("Information", "型号", out outString);
                        btFormat.SubStrings[outString].Value = this.SoftModel.Text;
                        GetValue("Information", "生产日期", out outString);
                        btFormat.SubStrings[outString].Value = this.ProductData.Text;
                        GetValue("Information", "产品编码", out outString);
                        btFormat.SubStrings[outString].Value = this.ProductNo.Text;
                        GetValue("Information", "软件版本", out outString);
                        btFormat.SubStrings[outString].Value = this.SoftwareVersion.Text;
                        GetValue("Information", "备注", out outString);
                        btFormat.SubStrings[outString].Value = this.Remake.Text;

                        //指定打印机名称
                        btFormat.PrintSetup.PrinterName = this.Printer1.Text;
                        //打印份数,同序列打印的份数
                        btFormat.PrintSetup.IdenticalCopiesOfLabel = 1;

                        Messages messages;
                        int waitout = 10000;
                        if (this.PrintOne.Checked == true)
                        {
                            if (this.IMEI_Start.Text != "")
                            {
                                btFormat.SubStrings["IMEI"].Value = this.IMEI_Start.Text;
                                if (!PMB.CheckIMEIBLL(this.IMEI_Start.Text))
                                {
                                    //记录打印信息日志
                                    string ProductTime = "";
                                    string NowTime = System.DateTime.Now.ToString("HH:mm:ss:fff");
                                    if (this.ProductData.Text == "")
                                    {
                                        ProductTime = System.DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:fff");
                                    }
                                    else
                                    {
                                        ProductTime = this.ProductData.Text.Trim() + " " + NowTime;
                                    }
                                    if (this.SN1_num.Text != "")
                                    {
                                        btFormat.SubStrings["SN"].Value = this.SN1_num.Text;
                                        list.Add(new PrintMessage()
                                        {
                                            Zhidan = this.CB_ZhiDan.Text.Trim(),
                                            IMEI = this.IMEI_Start.Text.Trim(),
                                            IMEIStart = this.IMEI_num1.Text.Trim(),
                                            IMEIEnd = this.IMEI_num2.Text.Trim(),
                                            SN = this.SN1_num.Text,
                                            IMEIRel = this.IMEIRel.Text.Trim(),
                                            SIM = "",
                                            VIP = "",
                                            BAT = "",
                                            SoftModel = this.SoftModel.Text.Trim(),
                                            Version = this.SoftwareVersion.Text.Trim(),
                                            Remark = this.Remake.Text.Trim(),
                                            JS_PrintTime = ProductTime,
                                            JS_TemplatePath = this.Select_Template1.Text.Trim(),
                                            CH_PrintTime = "",
                                            CH_TemplatePath1 = null,
                                            CH_TemplatePath2 = null
                                        });
                                        if (PMB.InsertPrintMessageBLL(list))
                                        {
                                            string sn1_prefix = SN1_num.Text.Substring(0, this.SN1_num.Text.Length - 5);
                                            long sn1_suffix = long.Parse(SN1_num.Text.Remove(0, (this.SN1_num.Text.Length) - 5));
                                            string sn1 = sn1_prefix + (sn1_suffix + 1).ToString().PadLeft(5, '0');
                                            string sn2_suffix = SN2_num.Text.Remove(0, (this.SN2_num.Text.Length) - 5);
                                            if (MOPB.UpdateSNnumberBLL(this.CB_ZhiDan.Text, sn1_prefix, (sn1_suffix + 1).ToString().PadLeft(5, '0'), sn2_suffix, this.VIP_num1.Text, this.VIP_num2.Text, long.Parse(this.IMEI_Start.Text.Substring(0, 14)) + 1))
                                            {
                                                Result nResult1 = btFormat.Print("标签打印软件", waitout, out messages);
                                                Form1.Log("打印了IMEI号为" + this.IMEI_Start.Text + "的制单", null);
                                                this.SN1_num.Text = sn1;
                                                long imei_star14 = long.Parse(this.IMEI_Start.Text.Substring(0, 14)) + 1;
                                                //string imei_star15 = getimei15(imei_star14.ToString());
                                                //string imei_star = imei_star14.ToString() + imei_star15;
                                                this.IMEI_Start.Clear();
                                                this.IMEI_Start.Focus();
                                                this.IMEI_Present.Text = imei_star14.ToString();
                                                this.SN1_num.ReadOnly = true;
                                                this.SN2_num.ReadOnly = true;
                                                this.VIP_num1.ReadOnly = true;
                                                this.VIP_num2.ReadOnly = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        btFormat.SubStrings["SN"].Value = "";
                                        list.Add(new PrintMessage()
                                        {
                                            Zhidan = this.CB_ZhiDan.Text.Trim(),
                                            IMEI = this.IMEI_Start.Text.Trim(),
                                            IMEIStart = this.IMEI_num1.Text.Trim(),
                                            IMEIEnd = this.IMEI_num2.Text.Trim(),
                                            SN = "",
                                            IMEIRel = this.IMEIRel.Text.Trim(),
                                            SIM = "",
                                            VIP = "",
                                            BAT = "",
                                            SoftModel = this.SoftModel.Text.Trim(),
                                            Version = this.SoftwareVersion.Text.Trim(),
                                            Remark = this.Remake.Text.Trim(),
                                            JS_PrintTime = ProductTime,
                                            JS_TemplatePath = this.Select_Template1.Text.Trim(),
                                            CH_PrintTime = "",
                                            CH_TemplatePath1 = null,
                                            CH_TemplatePath2 = null
                                        });
                                        if (PMB.InsertPrintMessageBLL(list))
                                        {
                                            string sn2_suffix;
                                            if (this.SN2_num.Text != "")
                                            {
                                                sn2_suffix = SN2_num.Text.Remove(0, (this.SN1_num.Text.Length) - 5);
                                            }
                                            else
                                            {
                                                sn2_suffix = "";
                                            }
                                            if (MOPB.UpdateSNnumberBLL(this.CB_ZhiDan.Text, "", "", sn2_suffix, this.VIP_num1.Text, this.VIP_num2.Text, long.Parse(this.IMEI_Start.Text.Substring(0, 14)) + 1))
                                            {
                                                Result nResult1 = btFormat.Print("标签打印软件", waitout, out messages);
                                                Form1.Log("打印了IMEI号为" + this.IMEI_Start.Text + "的制单", null);
                                                long imei_star14 = long.Parse(this.IMEI_Start.Text.Substring(0, 14)) + 1;
                                                //string imei_star15 = getimei15(imei_star14.ToString());
                                                //string imei_star = imei_star14.ToString() + imei_star15;
                                                this.IMEI_Start.Clear();
                                                this.IMEI_Start.Focus();
                                                this.IMEI_Present.Text = imei_star14.ToString();
                                                this.SN1_num.ReadOnly = true;
                                                this.SN2_num.ReadOnly = true;
                                                this.VIP_num1.ReadOnly = true;
                                                this.VIP_num2.ReadOnly = true;
                                            }
                                        }
                                    }
                                }
                                else if (PMB.CheckCHOrJSIMEIBLL(this.IMEI_Start.Text, 1))
                                {
                                    string ProductTime = "";
                                    string NowTime = System.DateTime.Now.ToString("HH:mm:ss:fff");
                                    if (this.ProductData.Text == "")
                                    {
                                        ProductTime = System.DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:fff");
                                    }
                                    else
                                    {
                                        ProductTime = this.ProductData.Text.Trim() + " " + NowTime;
                                    }
                                    list = PMB.SelectSnByIMEIBLL(this.IMEI_Start.Text);
                                    foreach (PrintMessage a in list)
                                    {
                                        btFormat.SubStrings["SN"].Value = a.SN;
                                    }
                                    if (MOPB.UpdateJSmesBLL(this.IMEI_Start.Text, ProductTime, lj))
                                    {
                                        Result nResult1 = btFormat.Print("标签打印软件", waitout, out messages);
                                        Form1.Log("打印了机身贴IMEI号为" + this.IMEI_Start.Text + "的制单", null);
                                        this.IMEI_Start.Clear();
                                        this.IMEI_Start.Focus();
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("该IMEI号已被打印过！");
                                    this.IMEI_Start.Clear();
                                    this.IMEI_Start.Focus();
                                }
                            }
                            else
                            {
                                MessageBox.Show("IMEI打印号不能为空！");
                            }
                            btFormat.PrintSetup.Cache.FlushInterval = CacheFlushInterval.PerSession;
                            MOPB.UpdateStatusByZhiDanBLL(this.CB_ZhiDan.Text);
                        }
                    }
                    else
                    {
                        MessageBox.Show("请先选择模板");
                        this.IMEI_Start.Clear();
                        this.IMEI_Start.Focus();
                    }
                    //结束打印引擎
                    btEngine.Stop();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Exception:" + ex.Message);
                }
            }
        }

        private void Re_IMEINum_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) {
                if (this.Re_IMEINum.Text != "")
                {
                    if (IsNumeric(this.Re_IMEINum.Text))
                    {
                        if (this.Re_IMEINum.Text.Length != 15)
                        {
                            MessageBox.Show("IMEI号只能为15位纯数字，请重新输入！");
                            this.Re_IMEINum.Clear();
                            this.Re_IMEINum.Focus();
                            return;
                        }
                        else
                        {
                            string imeiRes;
                            string imei14 = this.Re_IMEINum.Text.Substring(0, 14);
                            string imei15 = getimei15(imei14);
                            imeiRes = imei14 + imei15;
                            if (imeiRes != this.Re_IMEINum.Text)
                            {
                                MessageBox.Show("校验出错,请重新输入");
                                this.Re_IMEINum.Clear();
                                this.Re_IMEINum.Focus();
                                return;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("IMEI号格式不正确,请重新设置！");
                        this.Re_IMEINum.Clear();
                        this.Re_IMEINum.Focus();
                        return;
                    }
                }
                try
                {
                    Engine btEngine = new Engine();
                    btEngine.Start();
                    string lj = "";
                    if (this.Select_Template1.Text != "")
                    {
                        lj = this.Select_Template1.Text;
                        LabelFormatDocument btFormat = btEngine.Documents.Open(lj);
                        //指定打印机名称
                        btFormat.PrintSetup.PrinterName = this.Printer1.Text;
                        //对模板相应字段进行赋值
                        GetValue("Information", "生产日期", out outString);
                        btFormat.SubStrings[outString].Value = this.ProductData.Text;
                        //打印份数,同序列打印的份数
                        btFormat.PrintSetup.IdenticalCopiesOfLabel = 1;

                        Messages messages;
                        int waitout = 10000;
                        if (this.Re_IMEINum.Text != "")
                        {
                            btFormat.SubStrings["IMEI"].Value = this.Re_IMEINum.Text;
                            if (PMB.CheckReCHOrJSIMEIBLL(this.Re_IMEINum.Text, 1))
                            {
                                list = PMB.SelectSnByIMEIBLL(this.Re_IMEINum.Text);
                                foreach (PrintMessage a in list)
                                {
                                    GetValue("Information", "型号", out outString);
                                    btFormat.SubStrings[outString].Value = a.SoftModel;
                                    GetValue("Information", "SIM卡号", out outString);
                                    btFormat.SubStrings[outString].Value = a.SIM;
                                    GetValue("Information", "服务卡号", out outString);
                                    btFormat.SubStrings[outString].Value = a.VIP;
                                    btFormat.SubStrings["BAT"].Value = a.BAT;
                                    GetValue("Information", "SN", out outString);
                                    btFormat.SubStrings["SN"].Value = a.SN;
                                    GetValue("Information", "备注", out outString);
                                    btFormat.SubStrings[outString].Value = a.Remark;
                                }
                                //更新打印信息到数据表
                                string NowTime = System.DateTime.Now.ToString("HH:mm:ss:fff");
                                string NowData = System.DateTime.Now.ToString("yyyy-MM-dd");
                                string RE_PrintTime = NowData + " " + NowTime;
                                if (PMB.UpdateRePrintBLL(this.Re_IMEINum.Text, RE_PrintTime, 1, lj, lj))
                                {
                                    Result nResult1 = btFormat.Print("标签打印软件", waitout, out messages);
                                    Form1.Log("重打了IMEI号为" + this.Re_IMEINum.Text + "的制单", null);
                                    btFormat.PrintSetup.Cache.FlushInterval = CacheFlushInterval.PerSession;
                                    this.Re_IMEINum.Clear();
                                    this.Re_IMEINum.Focus();
                                }
                            }
                            else
                            {
                                MessageBox.Show("该IMEI号还未被打印，不需重打！");
                                this.Re_IMEINum.Clear();
                                this.Re_IMEINum.Focus();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("请先选择模板");
                        this.Re_IMEINum.Clear();
                        this.Re_IMEINum.Focus();
                    }
                    //结束打印引擎
                    btEngine.Stop();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Exception:" + ex.Message);
                }
            }
        }


    }
}
    