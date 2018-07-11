using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seagull.BarTender.Print;
using System.Windows.Forms;
using Print_Message;
using Print.Message.Bll;
using ManuOrder.Param.BLL;
using System.Runtime.InteropServices;
using System.Drawing.Printing;
using System.Text.RegularExpressions;

namespace WindowsForms_print
{
    public partial class Color_Box : Form
    {

        public static void Log(string msg, Exception e)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + "print.log";
                if (!File.Exists(path))
                {
                    File.Create(path).Close();
                }
                StreamWriter writer = new StreamWriter(path, true);
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

        public Color_Box()
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

        private void Color_Box_Load(object sender, EventArgs e)
        {
            PrintDocument print = new PrintDocument();
            string sDefault = print.PrinterSettings.PrinterName;//默认打印机名
            this.Printer2.Text = sDefault;
            foreach (string sPrint in PrinterSettings.InstalledPrinters)//获取所有打印机名称
            {
                Printer2.Items.Add(sPrint);
            }
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
            this.PrintDate.Text = NowData;
        }

        private void Re_print_button_Click(object sender, EventArgs e)
        {
            try
            {
                Engine btEngine = new Engine();
                btEngine.Start();
                string lj = "";
                if (this.Select_Template1.Text != "" && this.Select_Template2.Text == "")
                {
                    lj = this.Select_Template1.Text;
                    LabelFormatDocument btFormat = btEngine.Documents.Open(lj);
                    //指定打印机名称
                    btFormat.PrintSetup.PrinterName = this.Printer2.Text;
                    //对模板相应字段进行赋值
                    GetValue("Information", "生产日期", out outString);
                    btFormat.SubStrings[outString].Value = this.PrintDate.Text;

                    //打印份数,同序列打印的份数
                    btFormat.PrintSetup.IdenticalCopiesOfLabel = 1;

                    Messages messages;
                    int waitout = 10000;
                    if (this.Re_IMEINum.Text != "")
                    {
                        btFormat.SubStrings["IMEI"].Value = this.Re_IMEINum.Text;
                        if (PMB.CheckReCHOrJSIMEIBLL(this.Re_IMEINum.Text, 2))
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
                                btFormat.SubStrings["SN"].Value = a.SN;
                            }
                            //更新打印信息到数据表
                            string NowTime = System.DateTime.Now.ToString("HH:mm:ss:fff");
                            string NowData = System.DateTime.Now.ToString("yyyy-MM-dd");
                            string RE_PrintTime = NowData + " " + NowTime;
                            if (PMB.UpdateRePrintBLL(this.Re_IMEINum.Text, RE_PrintTime, 2, lj, ""))
                            {
                                Result nResult1 = btFormat.Print("标签打印软件", waitout, out messages);
                                btFormat.PrintSetup.Cache.FlushInterval = CacheFlushInterval.PerSession;
                                //结束打印引擎
                                btEngine.Stop();
                                Form1.Log("重打了彩盒贴IMEI号为" + this.Re_IMEINum.Text + "的制单", null);
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
                else if (this.Select_Template1.Text == "" && this.Select_Template2.Text != "")
                {
                    lj = this.Select_Template2.Text;
                    LabelFormatDocument btFormat = btEngine.Documents.Open(lj);
                    //指定打印机名称
                    btFormat.PrintSetup.PrinterName = this.Printer2.Text;
                    //对模板相应字段进行赋值
                    GetValue("Information", "生产日期", out outString);
                    btFormat.SubStrings[outString].Value = this.PrintDate.Text;
                    //打印份数,同序列打印的份数
                    btFormat.PrintSetup.IdenticalCopiesOfLabel = 1;

                    Messages messages;
                    int waitout = 10000;
                    if (this.Re_IMEINum.Text != "")
                    {
                        btFormat.SubStrings["IMEI"].Value = this.Re_IMEINum.Text;
                        if (PMB.CheckReCHOrJSIMEIBLL(this.Re_IMEINum.Text, 2))
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
                                btFormat.SubStrings["SN"].Value = a.SN;
                            }
                            //更新打印信息到数据表
                            string NowTime = System.DateTime.Now.ToString("HH:mm:ss:fff");
                            string NowData = System.DateTime.Now.ToString("yyyy-MM-dd");
                            string RE_PrintTime = NowData + " " + NowTime;
                            if (PMB.UpdateRePrintBLL(this.Re_IMEINum.Text, RE_PrintTime, 2, "", lj))
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
                else if (this.Select_Template1.Text != "" && this.Select_Template2.Text != "")
                {
                    Messages messages;
                    int waitout = 10000;
                    if (this.Re_IMEINum.Text != "")
                    {
                        if (PMB.CheckReCHOrJSIMEIBLL(this.Re_IMEINum.Text, 2))
                        {
                            for (int S = 1; S <= 2; S++)
                            {
                                if (S == 1)
                                {
                                    lj = this.Select_Template1.Text;
                                }
                                else
                                {
                                    lj = this.Select_Template2.Text;
                                }
                                LabelFormatDocument btFormat = btEngine.Documents.Open(lj);
                                //指定打印机名称
                                btFormat.PrintSetup.PrinterName = this.Printer2.Text;
                                //对模板相应字段进行赋值
                                GetValue("Information", "生产日期", out outString);
                                btFormat.SubStrings[outString].Value = this.PrintDate.Text;
                                btFormat.SubStrings["IMEI"].Value = this.Re_IMEINum.Text;
                                //打印份数,同序列打印的份数
                                btFormat.PrintSetup.IdenticalCopiesOfLabel = 1;
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
                                    btFormat.SubStrings["SN"].Value = a.SN;
                                }
                                Result nResult1 = btFormat.Print("标签打印软件", waitout, out messages);
                                btFormat.PrintSetup.Cache.FlushInterval = CacheFlushInterval.PerSession;
                            }
                            //更新打印信息到数据表
                            string NowTime = System.DateTime.Now.ToString("HH:mm:ss:fff");
                            string NowData = System.DateTime.Now.ToString("yyyy-MM-dd");
                            string RE_PrintTime = NowData + " " + NowTime;
                            if (PMB.UpdateRePrintBLL(this.Re_IMEINum.Text, RE_PrintTime, 2, this.Select_Template1.Text, this.Select_Template2.Text))
                            {
                                Form1.Log("重打了IMEI号为" + this.Re_IMEINum.Text + "的制单", null);
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
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception:" + ex.Message);
            }
        }

        private void open_template1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            string path = ofd.FileName;
            string strExtension = "";
            int nIndex = path.LastIndexOf('.');
            if (nIndex > 0)
            {
                strExtension = path.Substring(nIndex);
                if (strExtension != ".btw")
                {
                    MessageBox.Show("请选择正确的btw文件！");
                }
                else
                {
                    this.Select_Template1.Text = path;
                }
            }
        }

        private void open_template2_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            string path = ofd.FileName;
            string strExtension = "";
            int nIndex = path.LastIndexOf('.');
            if (nIndex > 0)
            {
                strExtension = path.Substring(nIndex);
                if (strExtension != ".btw")
                {
                    MessageBox.Show("请选择正确的btw文件！");
                }
                else
                {
                    this.Select_Template2.Text = path;
                }
            }
        }

        private void CB_ZhiDan_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.IMEI_Start.Clear();
            this.Re_IMEINum.Clear();
            string NowData = System.DateTime.Now.ToString("yyyy.MM.dd");
            this.PrintDate.Text = NowData;
            string ZhidanNum = this.CB_ZhiDan.Text;
            G_MOP = MOPB.selectManuOrderParamByzhidanBLL(ZhidanNum);
            foreach (Gps_ManuOrderParam b in G_MOP)
            { 
                this.SoftModel.Text = b.SoftModel;
                this.SN1_num.Text = b.SN1 + b.SN2;
                this.SN2_num.Text = b.SN1 + b.SN3;
                this.BN1.Text = b.Box_No1;
                this.BN2.Text = b.Box_No2;
                this.ProductDate.Text = b.ProductDate;
                this.Color.Text = b.Color;
                this.Weight.Text = b.Weight;
                this.Qty.Text = b.Qty;
                this.ProductNo.Text = b.ProductNo;
                this.SoftwareVersion.Text = b.Version;
                this.IMEI_num1.Text = b.IMEIStart;
                this.IMEI_num2.Text = b.IMEIEnd;
                this.SIM_num1.Text = b.SIMStart;
                this.SIM_num2.Text = b.SIMEnd;
                this.SIM_digits.Text = b.SIM_digits;
                this.SIM_prefix.Text = b.SIM_prefix;
                this.BAT_num1.Text = b.BATStart;
                this.BAT_num2.Text = b.BATEnd;
                this.BAT_digits.Text = b.BAT_digits;
                this.BAT_prefix.Text = b.BAT_prefix;
                this.VIP_num1.Text = b.VIPStart;
                this.VIP_num2.Text = b.VIPEnd;
                this.VIP_digits.Text = b.VIP_digits;
                this.VIP_prefix.Text = b.VIP_prefix;
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

        public string getimei15(string imei)
        {
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

        private void PrintDate_Leave(object sender, EventArgs e)
        {
            if (this.PrintDate.Text != "")
            {
                if (!IsDate(this.PrintDate.Text))
                {
                    MessageBox.Show("请输入正确的日期格式");
                    this.PrintDate.Text = System.DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
        }

        public static bool HasChinese(string str)
        {
            return Regex.IsMatch(str, @"[\u4e00-\u9fa5]");
        }

        private void VIP_num1_Leave(object sender, EventArgs e)
        {
            if (this.VIP_num1.Text != "")
            {
                if (HasChinese(this.VIP_num1.Text))
                {
                    MessageBox.Show("VIP号不能有汉字,请重新输入");
                    this.VIP_num1.Clear();
                }
            }
        }

        private void VIP_num2_Leave(object sender, EventArgs e)
        {
            if (this.VIP_num2.Text != "")
            {
                if (HasChinese(this.VIP_num2.Text))
                {
                    MessageBox.Show("VIP号不能有汉字,请重新输入");
                    this.VIP_num2.Clear();
                }
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
                    btFormat.SubStrings[outString].Value = this.PrintDate.Text;
                    //打印份数,同序列打印的份数
                    btFormat.PrintSetup.IdenticalCopiesOfLabel = 1;
                    Messages messages;
                    int waitout = 10000;
                    Result nResult1 = btFormat.Print("标签打印软件", waitout, out messages);
                    Form1.Log("调试打印了机身贴IMEI号为" + this.IMEI_num1.Text + "的制单", null);
                    btFormat.PrintSetup.Cache.FlushInterval = CacheFlushInterval.PerSession;
                    MessageBox.Show("调试打印成功！");
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

        private void choose_sim_Click(object sender, EventArgs e)
        {
            if (this.choose_sim.Checked == true)
            {
                this.choose_vip.Checked = false;
                this.choose_all.Checked = false;
                this.SIMStart.ReadOnly = false;
                this.VIPStart.ReadOnly = true;
            }
            else {
                this.choose_sim.Checked = false;
                this.SIMStart.ReadOnly = true;
                this.VIPStart.ReadOnly = true;
            }
        }

        private void choose_vip_Click(object sender, EventArgs e)
        {
            if (this.choose_vip.Checked == true)
            {
                this.choose_sim.Checked = false;
                this.choose_all.Checked = false;
                this.SIMStart.ReadOnly = true;
                this.VIPStart.ReadOnly = false;
            }
            else
            {
                this.choose_vip.Checked = false;
                this.SIMStart.ReadOnly = true;
                this.VIPStart.ReadOnly = true;
            }
        }

        private void choose_all_Click(object sender, EventArgs e)
        {
            if (this.choose_all.Checked == true)
            {
                this.choose_sim.Checked = false;
                this.choose_vip.Checked = false;
                this.SIMStart.ReadOnly = false;
                this.VIPStart.ReadOnly = false;
            }
            else
            {
                this.choose_all.Checked = false;
                this.SIMStart.ReadOnly = true;
                this.VIPStart.ReadOnly = true;
            }
        }

        private void Re_IMEINum_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
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
                    if (this.Select_Template1.Text != "" && this.Select_Template2.Text == "")
                    {
                        lj = this.Select_Template1.Text;
                        LabelFormatDocument btFormat = btEngine.Documents.Open(lj);
                        //指定打印机名称
                        btFormat.PrintSetup.PrinterName = this.Printer2.Text;
                        //对模板相应字段进行赋值
                        GetValue("Information", "生产日期", out outString);
                        btFormat.SubStrings[outString].Value = this.PrintDate.Text;

                        //打印份数,同序列打印的份数
                        btFormat.PrintSetup.IdenticalCopiesOfLabel = 1;

                        Messages messages;
                        int waitout = 10000;
                        if (this.Re_IMEINum.Text != "")
                        {
                            btFormat.SubStrings["IMEI"].Value = this.Re_IMEINum.Text;
                            if (PMB.CheckReCHOrJSIMEIBLL(this.Re_IMEINum.Text, 2))
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
                                    btFormat.SubStrings["SN"].Value = a.SN;
                                }
                                //更新打印信息到数据表
                                string NowTime = System.DateTime.Now.ToString("HH:mm:ss:fff");
                                string NowData = System.DateTime.Now.ToString("yyyy-MM-dd");
                                string RE_PrintTime = NowData + " " + NowTime;
                                if (PMB.UpdateRePrintBLL(this.Re_IMEINum.Text, RE_PrintTime, 2, lj, ""))
                                {
                                    Result nResult1 = btFormat.Print("标签打印软件", waitout, out messages);
                                    btFormat.PrintSetup.Cache.FlushInterval = CacheFlushInterval.PerSession;
                                    //结束打印引擎
                                    btEngine.Stop();
                                    Form1.Log("重打了彩盒贴IMEI号为" + this.Re_IMEINum.Text + "的制单", null);
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
                    else if (this.Select_Template1.Text == "" && this.Select_Template2.Text != "")
                    {
                        lj = this.Select_Template2.Text;
                        LabelFormatDocument btFormat = btEngine.Documents.Open(lj);
                        //指定打印机名称
                        btFormat.PrintSetup.PrinterName = this.Printer2.Text;
                        //对模板相应字段进行赋值
                        GetValue("Information", "生产日期", out outString);
                        btFormat.SubStrings[outString].Value = this.PrintDate.Text;
                        //打印份数,同序列打印的份数
                        btFormat.PrintSetup.IdenticalCopiesOfLabel = 1;

                        Messages messages;
                        int waitout = 10000;
                        if (this.Re_IMEINum.Text != "")
                        {
                            btFormat.SubStrings["IMEI"].Value = this.Re_IMEINum.Text;
                            if (PMB.CheckReCHOrJSIMEIBLL(this.Re_IMEINum.Text, 2))
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
                                    btFormat.SubStrings["SN"].Value = a.SN;
                                }
                                //更新打印信息到数据表
                                string NowTime = System.DateTime.Now.ToString("HH:mm:ss:fff");
                                string NowData = System.DateTime.Now.ToString("yyyy-MM-dd");
                                string RE_PrintTime = NowData + " " + NowTime;
                                if (PMB.UpdateRePrintBLL(this.Re_IMEINum.Text, RE_PrintTime, 2, "", lj))
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
                    else if (this.Select_Template1.Text != "" && this.Select_Template2.Text != "")
                    {
                        Messages messages;
                        int waitout = 10000;
                        if (this.Re_IMEINum.Text != "")
                        {
                            if (PMB.CheckReCHOrJSIMEIBLL(this.Re_IMEINum.Text, 2))
                            {
                                for (int S = 1; S <= 2; S++)
                                {
                                    if (S == 1)
                                    {
                                        lj = this.Select_Template1.Text;
                                    }
                                    else
                                    {
                                        lj = this.Select_Template2.Text;
                                    }
                                    LabelFormatDocument btFormat = btEngine.Documents.Open(lj);
                                    //指定打印机名称
                                    btFormat.PrintSetup.PrinterName = this.Printer2.Text;
                                    //对模板相应字段进行赋值
                                    GetValue("Information", "生产日期", out outString);
                                    btFormat.SubStrings[outString].Value = this.PrintDate.Text;
                                    btFormat.SubStrings["IMEI"].Value = this.Re_IMEINum.Text;
                                    //打印份数,同序列打印的份数
                                    btFormat.PrintSetup.IdenticalCopiesOfLabel = 1;
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
                                        btFormat.SubStrings["SN"].Value = a.SN;
                                    }
                                    Result nResult1 = btFormat.Print("标签打印软件", waitout, out messages);
                                    btFormat.PrintSetup.Cache.FlushInterval = CacheFlushInterval.PerSession;
                                }
                                //更新打印信息到数据表
                                string NowTime = System.DateTime.Now.ToString("HH:mm:ss:fff");
                                string NowData = System.DateTime.Now.ToString("yyyy-MM-dd");
                                string RE_PrintTime = NowData + " " + NowTime;
                                if (PMB.UpdateRePrintBLL(this.Re_IMEINum.Text, RE_PrintTime, 2, this.Select_Template1.Text, this.Select_Template2.Text))
                                {
                                    Form1.Log("重打了IMEI号为" + this.Re_IMEINum.Text + "的制单", null);
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
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Exception:" + ex.Message);
                }
            }
        }

        private void IMEI_Start_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
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
                }
                else
                {
                    MessageBox.Show("请先选择制单号");
                    this.IMEI_Start.Clear();
                    this.IMEI_Start.Focus();
                    return;
                }
                if (this.choose_sim.Checked == false && this.choose_vip.Checked == false && this.choose_all.Checked == false)
                {
                    try
                    {
                        Engine btEngine = new Engine();
                        btEngine.Start();
                        string lj = "";
                        if (this.Select_Template1.Text != "" && this.Select_Template2.Text == "")
                        {
                            lj = this.Select_Template1.Text;
                            LabelFormatDocument btFormat = btEngine.Documents.Open(lj);
                            //指定打印机名称
                            btFormat.PrintSetup.PrinterName = this.Printer1.Text;
                            //对模板相应字段进行赋值
                            GetValue("Information", "型号", out outString);
                            btFormat.SubStrings[outString].Value = this.SoftModel.Text;
                            GetValue("Information", "生产日期", out outString);
                            btFormat.SubStrings[outString].Value = this.PrintDate.Text;
                            GetValue("Information", "产品编码", out outString);
                            btFormat.SubStrings[outString].Value = this.ProductNo.Text;
                            GetValue("Information", "软件版本", out outString);
                            btFormat.SubStrings[outString].Value = this.SoftwareVersion.Text;
                            GetValue("Information", "备注", out outString);
                            btFormat.SubStrings[outString].Value = this.Remake.Text;
                            GetValue("Information", "箱号", out outString);
                            btFormat.SubStrings[outString].Value = this.BN1.Text + this.BN2.Text;
                            GetValue("Information", "颜色", out outString);
                            btFormat.SubStrings[outString].Value = this.Color.Text;
                            GetValue("Information", "重量", out outString);
                            btFormat.SubStrings[outString].Value = this.Weight.Text;

                            //打印份数,同序列打印的份数
                            btFormat.PrintSetup.IdenticalCopiesOfLabel = 1;

                            string ProductTime = "";
                            string NowTime = System.DateTime.Now.ToString("HH:mm:ss:fff");
                            if (this.PrintDate.Text == "")
                            {
                                ProductTime = System.DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:fff");
                            }
                            else
                            {
                                ProductTime = this.PrintDate.Text.Trim() + " " + NowTime;
                            }
                            Messages messages;
                            int waitout = 10000;
                            if (this.IMEI_Start.Text != "")
                            {
                                btFormat.SubStrings["IMEI"].Value = this.IMEI_Start.Text;
                                GetValue("Information", "SIM卡号", out outString);
                                btFormat.SubStrings[outString].Value = this.SIMStart.Text;
                                GetValue("Information", "服务卡号", out outString);
                                btFormat.SubStrings[outString].Value = this.VIPStart.Text;
                                if (!PMB.CheckIMEIBLL(this.IMEI_Start.Text))
                                {
                                    //记录打印信息日志
                                    btFormat.SubStrings["SN"].Value = this.SN1_num.Text;
                                    list.Add(new PrintMessage()
                                    {
                                        Zhidan = this.CB_ZhiDan.Text.Trim(),
                                        IMEI = this.IMEI_Start.Text.Trim(),
                                        IMEIStart = this.IMEI_num1.Text.Trim(),
                                        IMEIEnd = this.IMEI_num2.Text.Trim(),
                                        SN = this.SN1_num.Text,
                                        IMEIRel = this.IMEIRel.Text.Trim(),
                                        SIM = this.SIMStart.Text.Trim(),
                                        VIP = this.VIPStart.Text.Trim(),
                                        BAT = "",
                                        SoftModel = this.SoftModel.Text.Trim(),
                                        Version = this.SoftwareVersion.Text.Trim(),
                                        Remark = this.Remake.Text.Trim(),
                                        JS_PrintTime = "",
                                        JS_TemplatePath = "",
                                        CH_PrintTime = ProductTime,
                                        CH_TemplatePath1 = lj,
                                        CH_TemplatePath2 = ""
                                    });
                                    if (PMB.InsertPrintMessageBLL(list))
                                    {
                                        if (this.SN1_num.Text != "")
                                        {
                                            string sn1_prefix = SN1_num.Text.Substring(0, this.SN1_num.Text.Length - 5);
                                            long sn1_suffix = long.Parse(SN1_num.Text.Remove(0, (this.SN1_num.Text.Length) - 5));
                                            string sn1 = sn1_prefix + (sn1_suffix + 1).ToString().PadLeft(5, '0');
                                            string sn2_suffix = SN2_num.Text.Remove(0, (this.SN2_num.Text.Length) - 5);
                                            if (MOPB.UpdateSN3numberBLL(this.CB_ZhiDan.Text, sn1_prefix, (sn1_suffix + 1).ToString().PadLeft(5, '0'), sn2_suffix, this.VIP_num1.Text, this.VIP_num2.Text))
                                            {
                                                Result nResult1 = btFormat.Print("标签打印软件", waitout, out messages);
                                                Form1.Log("打印了彩盒贴IMEI号为" + this.IMEI_Start.Text + "的制单", null);
                                                this.SN1_num.Text = sn1;
                                                this.IMEI_Start.Clear();
                                                this.IMEI_Start.Focus();
                                                this.SN1_num.ReadOnly = true;
                                                this.SN2_num.ReadOnly = true;
                                                this.VIP_num1.ReadOnly = true;
                                                this.VIP_num2.ReadOnly = true;
                                            }
                                        }
                                        else
                                        {
                                            if (MOPB.UpdateSN3numberBLL(this.CB_ZhiDan.Text, "", "", "", this.VIP_num1.Text, this.VIP_num2.Text))
                                            {
                                                Result nResult1 = btFormat.Print("标签打印软件", waitout, out messages);
                                                Form1.Log("打印了彩盒贴IMEI号为" + this.IMEI_Start.Text + "的制单", null);
                                                this.IMEI_Start.Clear();
                                                this.IMEI_Start.Focus();
                                                this.SN1_num.ReadOnly = true;
                                                this.SN2_num.ReadOnly = true;
                                                this.VIP_num1.ReadOnly = true;
                                                this.VIP_num2.ReadOnly = true;
                                            }
                                        }
                                    }
                                }
                                else if (PMB.CheckCHOrJSIMEIBLL(this.IMEI_Start.Text, 2))
                                {
                                    list = PMB.SelectSnByIMEIBLL(this.IMEI_Start.Text);
                                    foreach (PrintMessage a in list)
                                    {
                                        btFormat.SubStrings["SN"].Value = a.SN;
                                    }
                                    if (MOPB.UpdateCHmesBLL(this.IMEI_Start.Text, ProductTime, lj, ""))
                                    {
                                        Result nResult1 = btFormat.Print("标签打印软件", waitout, out messages);
                                        Form1.Log("打印了彩盒贴IMEI号为" + this.IMEI_Start.Text + "的制单", null);
                                        this.IMEI_Start.Clear();
                                        this.IMEI_Start.Focus();
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("该IMEI号已被打印过！");
                                }
                            }
                            else
                            {
                                MessageBox.Show("IMEI打印起始号不能为空！");
                            }
                            btFormat.PrintSetup.Cache.FlushInterval = CacheFlushInterval.PerSession;
                            MOPB.UpdateStatusByZhiDanBLL(this.CB_ZhiDan.Text);
                        }
                        else if (this.Select_Template1.Text == "" && this.Select_Template2.Text != "")
                        {
                            lj = this.Select_Template2.Text;
                            LabelFormatDocument btFormat = btEngine.Documents.Open(lj);
                            //指定打印机名称
                            btFormat.PrintSetup.PrinterName = this.Printer2.Text;
                            //对模板相应字段进行赋值
                            GetValue("Information", "型号", out outString);
                            btFormat.SubStrings[outString].Value = this.SoftModel.Text;
                            GetValue("Information", "生产日期", out outString);
                            btFormat.SubStrings[outString].Value = this.PrintDate.Text;
                            GetValue("Information", "产品编码", out outString);
                            btFormat.SubStrings[outString].Value = this.ProductNo.Text;
                            GetValue("Information", "软件版本", out outString);
                            btFormat.SubStrings[outString].Value = this.SoftwareVersion.Text;
                            GetValue("Information", "备注", out outString);
                            btFormat.SubStrings[outString].Value = this.Remake.Text;

                            //打印份数,同序列打印的份数
                            btFormat.PrintSetup.IdenticalCopiesOfLabel = 1;
                            string ProductTime = "";
                            string NowTime = System.DateTime.Now.ToString("HH:mm:ss:fff");
                            if (this.PrintDate.Text == "")
                            {
                                ProductTime = System.DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:fff");
                            }
                            else
                            {
                                ProductTime = this.PrintDate.Text.Trim() + " " + NowTime;
                            }
                            Messages messages;
                            int waitout = 10000;
                            if (this.IMEI_Start.Text != "")
                            {
                                GetValue("Information", "SIM卡号", out outString);
                                btFormat.SubStrings[outString].Value = this.SIMStart.Text;
                                GetValue("Information", "服务卡号", out outString);
                                btFormat.SubStrings[outString].Value = this.VIPStart.Text;
                                btFormat.SubStrings["IMEI"].Value = this.IMEI_Start.Text;
                                if (!PMB.CheckIMEIBLL(this.IMEI_Start.Text))
                                {
                                    //记录打印信息日志
                                    btFormat.SubStrings["SN"].Value = this.SN1_num.Text;
                                    list.Add(new PrintMessage()
                                    {
                                        Zhidan = this.CB_ZhiDan.Text.Trim(),
                                        IMEI = this.IMEI_Start.Text.Trim(),
                                        IMEIStart = this.IMEI_num1.Text.Trim(),
                                        IMEIEnd = this.IMEI_num2.Text.Trim(),
                                        SN = this.SN1_num.Text,
                                        IMEIRel = this.IMEIRel.Text.Trim(),
                                        SIM = this.SIMStart.Text.Trim(),
                                        VIP = this.VIPStart.Text.Trim(),
                                        BAT = "",
                                        SoftModel = this.SoftModel.Text.Trim(),
                                        Version = this.SoftwareVersion.Text.Trim(),
                                        Remark = this.Remake.Text.Trim(),
                                        JS_PrintTime = "",
                                        JS_TemplatePath = "",
                                        CH_PrintTime = ProductTime,
                                        CH_TemplatePath1 = "",
                                        CH_TemplatePath2 = lj
                                    });
                                    if (PMB.InsertPrintMessageBLL(list))
                                    {
                                        if (this.SN1_num.Text != "")
                                        {
                                            string sn1_prefix = SN1_num.Text.Substring(0, this.SN1_num.Text.Length - 5);
                                            long sn1_suffix = long.Parse(SN1_num.Text.Remove(0, (this.SN1_num.Text.Length) - 5));
                                            string sn1 = sn1_prefix + (sn1_suffix + 1).ToString().PadLeft(5, '0');
                                            string sn2_suffix = SN2_num.Text.Remove(0, (this.SN2_num.Text.Length) - 5);
                                            if (MOPB.UpdateSN3numberBLL(this.CB_ZhiDan.Text, sn1_prefix, (sn1_suffix + 1).ToString().PadLeft(5, '0'), sn2_suffix, this.VIP_num1.Text, this.VIP_num2.Text))
                                            {
                                                Result nResult1 = btFormat.Print("标签打印软件", waitout, out messages);
                                                Form1.Log("打印了彩盒贴IMEI号为" + this.IMEI_Start.Text + "的制单", null);
                                                this.SN1_num.Text = sn1;
                                                this.IMEI_Start.Clear();
                                                this.IMEI_Start.Focus();
                                                this.SN1_num.ReadOnly = true;
                                                this.SN2_num.ReadOnly = true;
                                                this.VIP_num1.ReadOnly = true;
                                                this.VIP_num2.ReadOnly = true;
                                            }
                                        }
                                        else
                                        {
                                            if (MOPB.UpdateSN3numberBLL(this.CB_ZhiDan.Text, "", "", "", this.VIP_num1.Text, this.VIP_num2.Text))
                                            {
                                                Result nResult1 = btFormat.Print("标签打印软件", waitout, out messages);
                                                Form1.Log("打印了彩盒贴IMEI号为" + this.IMEI_Start.Text + "的制单", null);
                                                this.IMEI_Start.Clear();
                                                this.IMEI_Start.Focus();
                                                this.SN1_num.ReadOnly = true;
                                                this.SN2_num.ReadOnly = true;
                                                this.VIP_num1.ReadOnly = true;
                                                this.VIP_num2.ReadOnly = true;
                                            }
                                        }
                                    }
                                }
                                else if (PMB.CheckCHOrJSIMEIBLL(this.IMEI_Start.Text, 2))
                                {
                                    list = PMB.SelectSnByIMEIBLL(this.IMEI_Start.Text);
                                    foreach (PrintMessage a in list)
                                    {
                                        btFormat.SubStrings["SN"].Value = a.SN;
                                    }
                                    if (MOPB.UpdateCHmesBLL(this.IMEI_Start.Text, ProductTime, "", lj))
                                    {
                                        Result nResult1 = btFormat.Print("标签打印软件", waitout, out messages);
                                        Form1.Log("打印了彩盒贴IMEI号为" + this.IMEI_Start.Text + "的制单", null);
                                        this.IMEI_Start.Clear();
                                        this.IMEI_Start.Focus();
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("该IMEI号已被打印过！");
                                }
                            }
                            else
                            {
                                MessageBox.Show("IMEI打印起始号不能为空！");
                            }
                            btFormat.PrintSetup.Cache.FlushInterval = CacheFlushInterval.PerSession;
                            MOPB.UpdateStatusByZhiDanBLL(this.CB_ZhiDan.Text);
                        }
                        else if (this.Select_Template1.Text != "" && this.Select_Template2.Text != "")
                        {
                            LabelFormatDocument btFormat;
                            string ProductTime = "";
                            string NowTime = System.DateTime.Now.ToString("HH:mm:ss:fff");
                            if (this.PrintDate.Text == "")
                            {
                                ProductTime = System.DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:fff");
                            }
                            else
                            {
                                ProductTime = this.PrintDate.Text.Trim() + " " + NowTime;
                            }
                            Messages messages;
                            int waitout = 10000;
                            if (this.IMEI_Start.Text != "")
                            {
                                if (!PMB.CheckIMEIBLL(this.IMEI_Start.Text))
                                {
                                    for (int S = 1; S <= 2; S++)
                                    {
                                        if (S == 1)
                                        {
                                            lj = this.Select_Template1.Text;
                                            btFormat = btEngine.Documents.Open(lj);
                                            //指定打印机名称
                                            btFormat.PrintSetup.PrinterName = this.Printer1.Text;
                                        }
                                        else
                                        {
                                            lj = this.Select_Template2.Text;
                                            btFormat = btEngine.Documents.Open(lj);
                                            //指定打印机名称
                                            btFormat.PrintSetup.PrinterName = this.Printer2.Text;
                                        }
                                        //对模板相应字段进行赋值
                                        GetValue("Information", "SIM卡号", out outString);
                                        btFormat.SubStrings[outString].Value = this.SIMStart.Text;
                                        GetValue("Information", "服务卡号", out outString);
                                        btFormat.SubStrings[outString].Value = this.VIPStart.Text;
                                        btFormat.SubStrings["IMEI"].Value = this.IMEI_Start.Text;
                                        GetValue("Information", "型号", out outString);
                                        btFormat.SubStrings[outString].Value = this.SoftModel.Text;
                                        GetValue("Information", "生产日期", out outString);
                                        btFormat.SubStrings[outString].Value = this.PrintDate.Text;
                                        GetValue("Information", "产品编码", out outString);
                                        btFormat.SubStrings[outString].Value = this.ProductNo.Text;
                                        GetValue("Information", "软件版本", out outString);
                                        btFormat.SubStrings[outString].Value = this.SoftwareVersion.Text;
                                        GetValue("Information", "备注", out outString);
                                        btFormat.SubStrings[outString].Value = this.Remake.Text;
                                        btFormat.SubStrings["SN"].Value = this.SN1_num.Text;
                                        Result nResult1 = btFormat.Print("标签打印软件", waitout, out messages);
                                        btFormat.PrintSetup.Cache.FlushInterval = CacheFlushInterval.PerSession;
                                        Form1.Log("打印了IMEI号为" + this.IMEI_Start.Text + "的彩盒贴制单", null);
                                    }
                                    //记录打印信息日志
                                    list.Add(new PrintMessage()
                                    {
                                        Zhidan = this.CB_ZhiDan.Text.Trim(),
                                        IMEI = this.IMEI_Start.Text.Trim(),
                                        IMEIStart = this.IMEI_num1.Text.Trim(),
                                        IMEIEnd = this.IMEI_num2.Text.Trim(),
                                        SN = this.SN1_num.Text,
                                        IMEIRel = this.IMEIRel.Text.Trim(),
                                        SIM = this.SIMStart.Text.Trim(),
                                        VIP = this.VIPStart.Text.Trim(),
                                        BAT = "",
                                        SoftModel = this.SoftModel.Text.Trim(),
                                        Version = this.SoftwareVersion.Text.Trim(),
                                        Remark = this.Remake.Text.Trim(),
                                        JS_PrintTime = "",
                                        JS_TemplatePath = "",
                                        CH_PrintTime = ProductTime,
                                        CH_TemplatePath1 = this.Select_Template1.Text,
                                        CH_TemplatePath2 = this.Select_Template2.Text
                                    });
                                    if (PMB.InsertPrintMessageBLL(list))
                                    {
                                        if (SN1_num.Text != "")
                                        {
                                            string sn1_prefix = SN1_num.Text.Substring(0, this.SN1_num.Text.Length - 5);
                                            long sn1_suffix = long.Parse(SN1_num.Text.Remove(0, (this.SN1_num.Text.Length) - 5));
                                            string sn1 = sn1_prefix + (sn1_suffix + 1).ToString().PadLeft(5, '0');
                                            string sn2_suffix = SN2_num.Text.Remove(0, (this.SN2_num.Text.Length) - 5);
                                            if (MOPB.UpdateSN3numberBLL(this.CB_ZhiDan.Text, sn1_prefix, (sn1_suffix + 1).ToString().PadLeft(5, '0'), sn2_suffix, this.VIP_num1.Text, this.VIP_num2.Text))
                                            {
                                                this.SN1_num.Text = sn1;
                                                this.IMEI_Start.Clear();
                                                this.IMEI_Start.Focus();
                                                this.SN1_num.ReadOnly = true;
                                                this.SN2_num.ReadOnly = true;
                                                this.VIP_num1.ReadOnly = true;
                                                this.VIP_num2.ReadOnly = true;
                                            }
                                        }
                                        else
                                        {
                                            if (MOPB.UpdateSN3numberBLL(this.CB_ZhiDan.Text, "", "", "", this.VIP_num1.Text, this.VIP_num2.Text))
                                            {
                                                Form1.Log("打印了彩盒贴IMEI号为" + this.IMEI_Start.Text + "的制单", null);
                                                this.IMEI_Start.Clear();
                                                this.IMEI_Start.Focus();
                                                this.SN1_num.ReadOnly = true;
                                                this.SN2_num.ReadOnly = true;
                                                this.VIP_num1.ReadOnly = true;
                                                this.VIP_num2.ReadOnly = true;
                                            }
                                        }
                                    }
                                }
                                else if (PMB.CheckCHOrJSIMEIBLL(this.IMEI_Start.Text, 2))
                                {
                                    for (int S = 1; S <= 2; S++)
                                    {
                                        if (S == 1)
                                        {
                                            lj = this.Select_Template1.Text;
                                            btFormat = btEngine.Documents.Open(lj);
                                            //指定打印机名称
                                            btFormat.PrintSetup.PrinterName = this.Printer1.Text;
                                        }
                                        else
                                        {
                                            lj = this.Select_Template2.Text;
                                            btFormat = btEngine.Documents.Open(lj);
                                            //指定打印机名称
                                            btFormat.PrintSetup.PrinterName = this.Printer2.Text;
                                        }
                                        //对模板相应字段进行赋值
                                        GetValue("Information", "SIM卡号", out outString);
                                        btFormat.SubStrings[outString].Value = this.SIMStart.Text;
                                        GetValue("Information", "服务卡号", out outString);
                                        btFormat.SubStrings[outString].Value = this.VIPStart.Text;
                                        GetValue("Information", "型号", out outString);
                                        btFormat.SubStrings[outString].Value = this.SoftModel.Text;
                                        GetValue("Information", "生产日期", out outString);
                                        btFormat.SubStrings[outString].Value = this.PrintDate.Text;
                                        GetValue("Information", "产品编码", out outString);
                                        btFormat.SubStrings[outString].Value = this.ProductNo.Text;
                                        GetValue("Information", "软件版本", out outString);
                                        btFormat.SubStrings[outString].Value = this.SoftwareVersion.Text;
                                        GetValue("Information", "备注", out outString);
                                        btFormat.SubStrings[outString].Value = this.Remake.Text;
                                        btFormat.SubStrings["IMEI"].Value = this.IMEI_Start.Text;
                                        list = PMB.SelectSnByIMEIBLL(this.IMEI_Start.Text);
                                        foreach (PrintMessage a in list)
                                        {
                                            btFormat.SubStrings["SN"].Value = a.SN;
                                        }
                                        Result nResult1 = btFormat.Print("标签打印软件", waitout, out messages);
                                        btFormat.PrintSetup.Cache.FlushInterval = CacheFlushInterval.PerSession;
                                        Form1.Log("打印了IMEI号为" + this.IMEI_Start.Text + "的彩盒贴制单", null);
                                    }
                                    if (MOPB.UpdateCHmesBLL(this.IMEI_Start.Text, ProductTime, this.Select_Template1.Text, this.Select_Template2.Text))
                                    {
                                        this.IMEI_Start.Clear();
                                        this.IMEI_Start.Focus();
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("该IMEI号已被打印过！");
                                }
                            }
                            else
                            {
                                MessageBox.Show("IMEI打印起始号不能为空！");
                            }
                            MOPB.UpdateStatusByZhiDanBLL(this.CB_ZhiDan.Text);
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
                if (this.choose_sim.Checked == true)
                {
                    try
                    {
                        Engine btEngine = new Engine();
                        btEngine.Start();
                        string lj = "";
                        if (this.Select_Template1.Text != "" && this.Select_Template2.Text == "")
                        {
                            lj = this.Select_Template1.Text;
                            LabelFormatDocument btFormat = btEngine.Documents.Open(lj);
                            //指定打印机名称
                            btFormat.PrintSetup.PrinterName = this.Printer1.Text;
                            //对模板相应字段进行赋值
                            GetValue("Information", "型号", out outString);
                            btFormat.SubStrings[outString].Value = this.SoftModel.Text;
                            GetValue("Information", "生产日期", out outString);
                            btFormat.SubStrings[outString].Value = this.PrintDate.Text;
                            GetValue("Information", "产品编码", out outString);
                            btFormat.SubStrings[outString].Value = this.ProductNo.Text;
                            GetValue("Information", "软件版本", out outString);
                            btFormat.SubStrings[outString].Value = this.SoftwareVersion.Text;
                            GetValue("Information", "备注", out outString);
                            btFormat.SubStrings[outString].Value = this.Remake.Text;
                            //打印份数,同序列打印的份数
                            btFormat.PrintSetup.IdenticalCopiesOfLabel = 1;

                            Messages messages;
                            int waitout = 10000;
                            if (this.IMEI_Start.Text != "")
                            {
                                btFormat.SubStrings["IMEI"].Value = this.IMEI_Start.Text;
                                if (!PMB.CheckIMEIBLL(this.IMEI_Start.Text))
                                {
                                    this.SIMStart.Focus();
                                }
                                else if (PMB.CheckCHOrJSIMEIBLL(this.IMEI_Start.Text, 2))
                                {
                                    string ProductTime = "";
                                    string NowTime = System.DateTime.Now.ToString("HH:mm:ss:fff");
                                    if (this.PrintDate.Text == "")
                                    {
                                        ProductTime = System.DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:fff");
                                    }
                                    else
                                    {
                                        ProductTime = this.PrintDate.Text.Trim() + " " + NowTime;
                                    }
                                    list = PMB.SelectSnByIMEIBLL(this.IMEI_Start.Text);
                                    foreach (PrintMessage a in list)
                                    {
                                        GetValue("Information", "型号", out outString);
                                        btFormat.SubStrings[outString].Value = a.SoftModel;
                                        btFormat.SubStrings["BAT"].Value = a.BAT;
                                        btFormat.SubStrings["SN"].Value = a.SN;
                                        if (a.SIM != "")
                                        {
                                            btFormat.SubStrings["IMEI"].Value = this.IMEI_Start.Text; ;
                                            btFormat.SubStrings["SIM"].Value = a.SIM;
                                            this.SIMStart.Text = a.SIM;
                                            if (MOPB.UpdateCHmesBLL(this.IMEI_Start.Text, ProductTime, lj, ""))
                                            {
                                                Result nResult1 = btFormat.Print("标签打印软件", waitout, out messages);
                                                Form1.Log("关联SIM打印了彩盒贴IMEI号为" + this.IMEI_Start.Text + "的制单", null);
                                                this.IMEI_Start.Clear();
                                                this.SIMStart.Clear();
                                                this.IMEI_Start.Focus();
                                            }
                                        }
                                        else
                                        {
                                            this.SIMStart.Focus();
                                        }
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("该IMEI号已被打印过！");
                                }
                            }
                            else
                            {
                                MessageBox.Show("IMEI打印起始号不能为空！");
                            }
                            btFormat.PrintSetup.Cache.FlushInterval = CacheFlushInterval.PerSession;
                            MOPB.UpdateStatusByZhiDanBLL(this.CB_ZhiDan.Text);
                        }
                        else if (this.Select_Template1.Text == "" && this.Select_Template2.Text != "")
                        {
                            lj = this.Select_Template2.Text;
                            LabelFormatDocument btFormat = btEngine.Documents.Open(lj);
                            //指定打印机名称
                            btFormat.PrintSetup.PrinterName = this.Printer2.Text;
                            //对模板相应字段进行赋值
                            GetValue("Information", "型号", out outString);
                            btFormat.SubStrings[outString].Value = this.SoftModel.Text;
                            GetValue("Information", "生产日期", out outString);
                            btFormat.SubStrings[outString].Value = this.PrintDate.Text;
                            GetValue("Information", "产品编码", out outString);
                            btFormat.SubStrings[outString].Value = this.ProductNo.Text;
                            GetValue("Information", "软件版本", out outString);
                            btFormat.SubStrings[outString].Value = this.SoftwareVersion.Text;
                            GetValue("Information", "备注", out outString);
                            btFormat.SubStrings[outString].Value = this.Remake.Text;

                            //打印份数,同序列打印的份数
                            btFormat.PrintSetup.IdenticalCopiesOfLabel = 1;

                            Messages messages;
                            int waitout = 10000;
                            if (this.IMEI_Start.Text != "")
                            {
                                if (!PMB.CheckIMEIBLL(this.IMEI_Start.Text))
                                {
                                    this.SIMStart.Focus();
                                }
                                else if (PMB.CheckCHOrJSIMEIBLL(this.IMEI_Start.Text, 2))
                                {
                                    string ProductTime = "";
                                    string NowTime = System.DateTime.Now.ToString("HH:mm:ss:fff");
                                    if (this.PrintDate.Text == "")
                                    {
                                        ProductTime = System.DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:fff");
                                    }
                                    else
                                    {
                                        ProductTime = this.PrintDate.Text.Trim() + " " + NowTime;
                                    }
                                    list = PMB.SelectSnByIMEIBLL(this.IMEI_Start.Text);
                                    foreach (PrintMessage a in list)
                                    {
                                        GetValue("Information", "型号", out outString);
                                        btFormat.SubStrings[outString].Value = a.SoftModel;
                                        btFormat.SubStrings["BAT"].Value = a.BAT;
                                        btFormat.SubStrings["SN"].Value = a.SN;
                                        if (a.SIM != "")
                                        {
                                            btFormat.SubStrings["IMEI"].Value = this.IMEI_Start.Text; ;
                                            btFormat.SubStrings["SIM"].Value = a.SIM;
                                            this.SIMStart.Text = a.SIM;
                                            if (MOPB.UpdateCHmesBLL(this.IMEI_Start.Text, ProductTime, lj, ""))
                                            {
                                                Result nResult1 = btFormat.Print("标签打印软件", waitout, out messages);
                                                Form1.Log("关联SIM打印了彩盒贴IMEI号为" + this.IMEI_Start.Text + "的制单", null);
                                                this.IMEI_Start.Clear();
                                                this.SIMStart.Clear();
                                                this.IMEI_Start.Focus();
                                            }
                                        }
                                        else
                                        {
                                            this.SIMStart.Focus();
                                        }
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("该IMEI号已被打印过！");
                                }
                            }
                            else
                            {
                                MessageBox.Show("IMEI打印起始号不能为空！");
                            }
                            btFormat.PrintSetup.Cache.FlushInterval = CacheFlushInterval.PerSession;
                            MOPB.UpdateStatusByZhiDanBLL(this.CB_ZhiDan.Text);
                        }
                        else if (this.Select_Template1.Text != "" && this.Select_Template2.Text != "")
                        {
                            LabelFormatDocument btFormat;
                            Messages messages;
                            int waitout = 10000;
                            if (this.IMEI_Start.Text != "")
                            {
                                if (!PMB.CheckIMEIBLL(this.IMEI_Start.Text))
                                {
                                    this.SIMStart.Focus();
                                }
                                else if (PMB.CheckCHOrJSIMEIBLL(this.IMEI_Start.Text, 2))
                                {
                                    list = PMB.SelectSnByIMEIBLL(this.IMEI_Start.Text);
                                    foreach (PrintMessage a in list)
                                    {
                                        if (a.SIM != "")
                                        {
                                            for (int S = 1; S <= 2; S++)
                                            {
                                                if (S == 1)
                                                {
                                                    lj = this.Select_Template1.Text;
                                                    btFormat = btEngine.Documents.Open(lj);
                                                    //指定打印机名称
                                                    btFormat.PrintSetup.PrinterName = this.Printer1.Text;
                                                }
                                                else
                                                {
                                                    lj = this.Select_Template2.Text;
                                                    btFormat = btEngine.Documents.Open(lj);
                                                    //指定打印机名称
                                                    btFormat.PrintSetup.PrinterName = this.Printer2.Text;
                                                }
                                                //对模板相应字段进行赋值
                                                btFormat.SubStrings["IMEI"].Value = this.IMEI_Start.Text;
                                                GetValue("Information", "型号", out outString);
                                                btFormat.SubStrings[outString].Value = this.SoftModel.Text;
                                                GetValue("Information", "生产日期", out outString);
                                                btFormat.SubStrings[outString].Value = this.PrintDate.Text;
                                                GetValue("Information", "产品编码", out outString);
                                                btFormat.SubStrings[outString].Value = this.ProductNo.Text;
                                                GetValue("Information", "软件版本", out outString);
                                                btFormat.SubStrings[outString].Value = this.SoftwareVersion.Text;
                                                GetValue("Information", "备注", out outString);
                                                btFormat.SubStrings[outString].Value = this.Remake.Text;
                                                btFormat.SubStrings["SN"].Value = a.SN;
                                                btFormat.SubStrings["SIM"].Value = a.SIM;
                                                this.SIMStart.Text = a.SIM;
                                                Result nResult1 = btFormat.Print("标签打印软件", waitout, out messages);
                                                btFormat.PrintSetup.Cache.FlushInterval = CacheFlushInterval.PerSession;
                                                Form1.Log("关联SIM打印了IMEI号为" + this.IMEI_Start.Text + "的彩盒贴制单", null);
                                            }
                                            this.IMEI_Start.Clear();
                                            this.SIMStart.Clear();
                                            this.IMEI_Start.Focus();
                                            string ProductTime = "";
                                            string NowTime = System.DateTime.Now.ToString("HH:mm:ss:fff");
                                            if (this.PrintDate.Text == "")
                                            {
                                                ProductTime = System.DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:fff");
                                            }
                                            else
                                            {
                                                ProductTime = this.PrintDate.Text.Trim() + " " + NowTime;
                                            }
                                            MOPB.UpdateCHmesBLL(this.IMEI_Start.Text, ProductTime, this.Select_Template1.Text, this.Select_Template2.Text);
                                        }
                                        else
                                        {
                                            this.SIMStart.Focus();
                                        }
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("该IMEI号已被打印过！");
                                }
                            }
                            else
                            {
                                MessageBox.Show("IMEI打印起始号不能为空！");
                            }
                            MOPB.UpdateStatusByZhiDanBLL(this.CB_ZhiDan.Text);
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
                if (this.choose_vip.Checked == true)
                {
                    try
                    {
                        Engine btEngine = new Engine();
                        btEngine.Start();
                        string lj = "";
                        if (this.Select_Template1.Text != "" && this.Select_Template2.Text == "")
                        {
                            lj = this.Select_Template1.Text;
                            LabelFormatDocument btFormat = btEngine.Documents.Open(lj);
                            //指定打印机名称
                            btFormat.PrintSetup.PrinterName = this.Printer1.Text;
                            //对模板相应字段进行赋值
                            GetValue("Information", "型号", out outString);
                            btFormat.SubStrings[outString].Value = this.SoftModel.Text;
                            GetValue("Information", "生产日期", out outString);
                            btFormat.SubStrings[outString].Value = this.PrintDate.Text;
                            GetValue("Information", "产品编码", out outString);
                            btFormat.SubStrings[outString].Value = this.ProductNo.Text;
                            GetValue("Information", "软件版本", out outString);
                            btFormat.SubStrings[outString].Value = this.SoftwareVersion.Text;
                            GetValue("Information", "备注", out outString);
                            btFormat.SubStrings[outString].Value = this.Remake.Text;
                            //打印份数,同序列打印的份数
                            btFormat.PrintSetup.IdenticalCopiesOfLabel = 1;

                            Messages messages;
                            int waitout = 10000;
                            if (this.IMEI_Start.Text != "")
                            {
                                btFormat.SubStrings["IMEI"].Value = this.IMEI_Start.Text;
                                if (!PMB.CheckIMEIBLL(this.IMEI_Start.Text))
                                {
                                    this.VIPStart.Focus();
                                }
                                else if (PMB.CheckCHOrJSIMEIBLL(this.IMEI_Start.Text, 2))
                                {
                                    string ProductTime = "";
                                    string NowTime = System.DateTime.Now.ToString("HH:mm:ss:fff");
                                    if (this.PrintDate.Text == "")
                                    {
                                        ProductTime = System.DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:fff");
                                    }
                                    else
                                    {
                                        ProductTime = this.PrintDate.Text.Trim() + " " + NowTime;
                                    }
                                    list = PMB.SelectSnByIMEIBLL(this.IMEI_Start.Text);
                                    foreach (PrintMessage a in list)
                                    {
                                        GetValue("Information", "型号", out outString);
                                        btFormat.SubStrings[outString].Value = a.SoftModel;
                                        btFormat.SubStrings["SN"].Value = a.SN;
                                        if (a.VIP != "")
                                        {
                                            btFormat.SubStrings["IMEI"].Value = this.IMEI_Start.Text; ;
                                            btFormat.SubStrings["VIP"].Value = a.VIP;
                                            this.VIPStart.Text = a.VIP;
                                            if (MOPB.UpdateCHmesBLL(this.IMEI_Start.Text, ProductTime, lj, ""))
                                            {
                                                Result nResult1 = btFormat.Print("标签打印软件", waitout, out messages);
                                                Form1.Log("关联VIP打印了彩盒贴IMEI号为" + this.IMEI_Start.Text + "的制单", null);
                                                this.IMEI_Start.Clear();
                                                this.VIPStart.Clear();
                                                this.IMEI_Start.Focus();
                                            }
                                        }
                                        else
                                        {
                                            this.VIPStart.Focus();
                                        }
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("该IMEI号已被打印过！");
                                }
                            }
                            else
                            {
                                MessageBox.Show("IMEI打印起始号不能为空！");
                            }
                            btFormat.PrintSetup.Cache.FlushInterval = CacheFlushInterval.PerSession;
                            MOPB.UpdateStatusByZhiDanBLL(this.CB_ZhiDan.Text);
                        }
                        else if (this.Select_Template1.Text == "" && this.Select_Template2.Text != "")
                        {
                            lj = this.Select_Template2.Text;
                            LabelFormatDocument btFormat = btEngine.Documents.Open(lj);
                            //指定打印机名称
                            btFormat.PrintSetup.PrinterName = this.Printer2.Text;
                            //对模板相应字段进行赋值
                            GetValue("Information", "型号", out outString);
                            btFormat.SubStrings[outString].Value = this.SoftModel.Text;
                            GetValue("Information", "生产日期", out outString);
                            btFormat.SubStrings[outString].Value = this.PrintDate.Text;
                            GetValue("Information", "产品编码", out outString);
                            btFormat.SubStrings[outString].Value = this.ProductNo.Text;
                            GetValue("Information", "软件版本", out outString);
                            btFormat.SubStrings[outString].Value = this.SoftwareVersion.Text;
                            GetValue("Information", "备注", out outString);
                            btFormat.SubStrings[outString].Value = this.Remake.Text;

                            //打印份数,同序列打印的份数
                            btFormat.PrintSetup.IdenticalCopiesOfLabel = 1;

                            Messages messages;
                            int waitout = 10000;
                            if (this.IMEI_Start.Text != "")
                            {
                                if (!PMB.CheckIMEIBLL(this.IMEI_Start.Text))
                                {
                                    this.VIPStart.Focus();
                                }
                                else if (PMB.CheckCHOrJSIMEIBLL(this.IMEI_Start.Text, 2))
                                {
                                    string ProductTime = "";
                                    string NowTime = System.DateTime.Now.ToString("HH:mm:ss:fff");
                                    if (this.PrintDate.Text == "")
                                    {
                                        ProductTime = System.DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:fff");
                                    }
                                    else
                                    {
                                        ProductTime = this.PrintDate.Text.Trim() + " " + NowTime;
                                    }
                                    list = PMB.SelectSnByIMEIBLL(this.IMEI_Start.Text);
                                    foreach (PrintMessage a in list)
                                    {
                                        GetValue("Information", "型号", out outString);
                                        btFormat.SubStrings[outString].Value = a.SoftModel;
                                        btFormat.SubStrings["SN"].Value = a.SN;
                                        if (a.VIP != "")
                                        {
                                            btFormat.SubStrings["IMEI"].Value = this.IMEI_Start.Text; ;
                                            btFormat.SubStrings["VIP"].Value = a.VIP;
                                            this.VIPStart.Text = a.VIP;
                                            if (MOPB.UpdateCHmesBLL(this.IMEI_Start.Text, ProductTime, "", lj))
                                            {
                                                Result nResult1 = btFormat.Print("标签打印软件", waitout, out messages);
                                                Form1.Log("关联VIP打印了彩盒贴IMEI号为" + this.IMEI_Start.Text + "的制单", null);
                                                this.IMEI_Start.Clear();
                                                this.VIPStart.Clear();
                                                this.IMEI_Start.Focus();
                                            }
                                        }
                                        else
                                        {
                                            this.VIPStart.Focus();
                                        }
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("该IMEI号已被打印过！");
                                }
                            }
                            else
                            {
                                MessageBox.Show("IMEI打印起始号不能为空！");
                            }
                            btFormat.PrintSetup.Cache.FlushInterval = CacheFlushInterval.PerSession;
                            MOPB.UpdateStatusByZhiDanBLL(this.CB_ZhiDan.Text);
                        }
                        else if (this.Select_Template1.Text != "" && this.Select_Template2.Text != "")
                        {
                            LabelFormatDocument btFormat;
                            Messages messages;
                            int waitout = 10000;
                            if (this.IMEI_Start.Text != "")
                            {
                                if (!PMB.CheckIMEIBLL(this.IMEI_Start.Text))
                                {
                                    this.VIPStart.Focus();
                                }
                                else if (PMB.CheckCHOrJSIMEIBLL(this.IMEI_Start.Text, 2))
                                {
                                    list = PMB.SelectSnByIMEIBLL(this.IMEI_Start.Text);
                                    foreach (PrintMessage a in list)
                                    {
                                        if (a.VIP != "")
                                        {
                                            for (int S = 1; S <= 2; S++)
                                            {
                                                if (S == 1)
                                                {
                                                    lj = this.Select_Template1.Text;
                                                    btFormat = btEngine.Documents.Open(lj);
                                                    //指定打印机名称
                                                    btFormat.PrintSetup.PrinterName = this.Printer1.Text;
                                                }
                                                else
                                                {
                                                    lj = this.Select_Template2.Text;
                                                    btFormat = btEngine.Documents.Open(lj);
                                                    //指定打印机名称
                                                    btFormat.PrintSetup.PrinterName = this.Printer2.Text;
                                                }
                                                //对模板相应字段进行赋值
                                                btFormat.SubStrings["IMEI"].Value = this.IMEI_Start.Text;
                                                GetValue("Information", "型号", out outString);
                                                btFormat.SubStrings[outString].Value = this.SoftModel.Text;
                                                GetValue("Information", "生产日期", out outString);
                                                btFormat.SubStrings[outString].Value = this.PrintDate.Text;
                                                GetValue("Information", "产品编码", out outString);
                                                btFormat.SubStrings[outString].Value = this.ProductNo.Text;
                                                GetValue("Information", "软件版本", out outString);
                                                btFormat.SubStrings[outString].Value = this.SoftwareVersion.Text;
                                                GetValue("Information", "备注", out outString);
                                                btFormat.SubStrings[outString].Value = this.Remake.Text;
                                                GetValue("Information", "服务卡号", out outString);
                                                btFormat.SubStrings[outString].Value = a.VIP;
                                                btFormat.SubStrings["SN"].Value = a.SN;
                                                this.VIPStart.Text = a.VIP;
                                                Result nResult1 = btFormat.Print("标签打印软件", waitout, out messages);
                                                btFormat.PrintSetup.Cache.FlushInterval = CacheFlushInterval.PerSession;
                                                Form1.Log("关联VIP打印了IMEI号为" + this.IMEI_Start.Text + "的彩盒贴制单", null);
                                            }
                                            this.IMEI_Start.Clear();
                                            this.VIPStart.Clear();
                                            this.IMEI_Start.Focus();
                                            string ProductTime = "";
                                            string NowTime = System.DateTime.Now.ToString("HH:mm:ss:fff");
                                            if (this.PrintDate.Text == "")
                                            {
                                                ProductTime = System.DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:fff");
                                            }
                                            else
                                            {
                                                ProductTime = this.PrintDate.Text.Trim() + " " + NowTime;
                                            }
                                            MOPB.UpdateCHmesBLL(this.IMEI_Start.Text, ProductTime, this.Select_Template1.Text, this.Select_Template2.Text);
                                        }
                                        else
                                        {
                                            this.VIPStart.Focus();
                                        }
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("该IMEI号已被打印过！");
                                }
                            }
                            else
                            {
                                MessageBox.Show("IMEI打印起始号不能为空！");
                            }
                            MOPB.UpdateStatusByZhiDanBLL(this.CB_ZhiDan.Text);
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
                if (this.choose_all.Checked == true)
                {
                    try
                    {
                        Engine btEngine = new Engine();
                        btEngine.Start();
                        string lj = "";
                        if (this.Select_Template1.Text != "" && this.Select_Template2.Text == "")
                        {
                            lj = this.Select_Template1.Text;
                            LabelFormatDocument btFormat = btEngine.Documents.Open(lj);
                            //指定打印机名称
                            btFormat.PrintSetup.PrinterName = this.Printer1.Text;
                            //对模板相应字段进行赋值
                            GetValue("Information", "型号", out outString);
                            btFormat.SubStrings[outString].Value = this.SoftModel.Text;
                            GetValue("Information", "生产日期", out outString);
                            btFormat.SubStrings[outString].Value = this.PrintDate.Text;
                            GetValue("Information", "产品编码", out outString);
                            btFormat.SubStrings[outString].Value = this.ProductNo.Text;
                            GetValue("Information", "软件版本", out outString);
                            btFormat.SubStrings[outString].Value = this.SoftwareVersion.Text;
                            GetValue("Information", "备注", out outString);
                            btFormat.SubStrings[outString].Value = this.Remake.Text;
                            //打印份数,同序列打印的份数
                            btFormat.PrintSetup.IdenticalCopiesOfLabel = 1;

                            Messages messages;
                            int waitout = 10000;
                            if (this.IMEI_Start.Text != "")
                            {
                                btFormat.SubStrings["IMEI"].Value = this.IMEI_Start.Text;
                                if (!PMB.CheckIMEIBLL(this.IMEI_Start.Text))
                                {
                                    this.SIMStart.Focus();
                                }
                                else if (PMB.CheckCHOrJSIMEIBLL(this.IMEI_Start.Text, 2))
                                {
                                    string ProductTime = "";
                                    string NowTime = System.DateTime.Now.ToString("HH:mm:ss:fff");
                                    if (this.PrintDate.Text == "")
                                    {
                                        ProductTime = System.DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:fff");
                                    }
                                    else
                                    {
                                        ProductTime = this.PrintDate.Text.Trim() + " " + NowTime;
                                    }
                                    list = PMB.SelectSnByIMEIBLL(this.IMEI_Start.Text);
                                    foreach (PrintMessage a in list)
                                    {
                                        GetValue("Information", "型号", out outString);
                                        btFormat.SubStrings[outString].Value = a.SoftModel;
                                        btFormat.SubStrings["SN"].Value = a.SN;
                                        if (a.SIM != "" && a.VIP != "")
                                        {
                                            btFormat.SubStrings["SIM"].Value = a.SIM;
                                            btFormat.SubStrings["VIP"].Value = a.VIP;
                                            this.SIMStart.Text = a.SIM;
                                            this.VIPStart.Text = a.VIP;
                                            if (MOPB.UpdateCHmesBLL(this.IMEI_Start.Text, ProductTime, lj, ""))
                                            {
                                                Result nResult1 = btFormat.Print("标签打印软件", waitout, out messages);
                                                Form1.Log("关联VIP和SIM打印了彩盒贴IMEI号为" + this.IMEI_Start.Text + "的制单", null);
                                                this.IMEI_Start.Clear();
                                                this.VIPStart.Clear();
                                                this.SIMStart.Clear();
                                                this.IMEI_Start.Focus();
                                            }
                                        }
                                        else if (a.SIM != "" && a.VIP == "")
                                        {
                                            this.SIMStart.Text = a.SIM;
                                            this.VIPStart.Focus();
                                        }
                                        else if (a.SIM == "" && a.VIP != "")
                                        {
                                            this.VIPStart.Text = a.VIP;
                                            this.SIMStart.Focus();
                                        }
                                        else
                                        {
                                            this.SIMStart.Focus();
                                        }
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("该IMEI号已被打印过！");
                                }
                            }
                            else
                            {
                                MessageBox.Show("IMEI打印起始号不能为空！");
                            }
                            btFormat.PrintSetup.Cache.FlushInterval = CacheFlushInterval.PerSession;
                            MOPB.UpdateStatusByZhiDanBLL(this.CB_ZhiDan.Text);
                        }
                        else if (this.Select_Template1.Text == "" && this.Select_Template2.Text != "")
                        {
                            lj = this.Select_Template2.Text;
                            LabelFormatDocument btFormat = btEngine.Documents.Open(lj);
                            //指定打印机名称
                            btFormat.PrintSetup.PrinterName = this.Printer2.Text;
                            //对模板相应字段进行赋值
                            GetValue("Information", "型号", out outString);
                            btFormat.SubStrings[outString].Value = this.SoftModel.Text;
                            GetValue("Information", "生产日期", out outString);
                            btFormat.SubStrings[outString].Value = this.PrintDate.Text;
                            GetValue("Information", "产品编码", out outString);
                            btFormat.SubStrings[outString].Value = this.ProductNo.Text;
                            GetValue("Information", "软件版本", out outString);
                            btFormat.SubStrings[outString].Value = this.SoftwareVersion.Text;
                            GetValue("Information", "备注", out outString);
                            btFormat.SubStrings[outString].Value = this.Remake.Text;

                            //打印份数,同序列打印的份数
                            btFormat.PrintSetup.IdenticalCopiesOfLabel = 1;

                            Messages messages;
                            int waitout = 10000;
                            if (this.IMEI_Start.Text != "")
                            {
                                if (!PMB.CheckIMEIBLL(this.IMEI_Start.Text))
                                {
                                    this.SIMStart.Focus();
                                }
                                else if (PMB.CheckCHOrJSIMEIBLL(this.IMEI_Start.Text, 2))
                                {
                                    string ProductTime = "";
                                    string NowTime = System.DateTime.Now.ToString("HH:mm:ss:fff");
                                    if (this.PrintDate.Text == "")
                                    {
                                        ProductTime = System.DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:fff");
                                    }
                                    else
                                    {
                                        ProductTime = this.PrintDate.Text.Trim() + " " + NowTime;
                                    }
                                    list = PMB.SelectSnByIMEIBLL(this.IMEI_Start.Text);
                                    foreach (PrintMessage a in list)
                                    {
                                        GetValue("Information", "型号", out outString);
                                        btFormat.SubStrings[outString].Value = a.SoftModel;
                                        btFormat.SubStrings["BAT"].Value = a.BAT;
                                        btFormat.SubStrings["SN"].Value = a.SN;
                                        if (a.SIM != "" && a.VIP != "")
                                        {
                                            btFormat.SubStrings["IMEI"].Value = this.IMEI_Start.Text; ;
                                            btFormat.SubStrings["SIM"].Value = a.SIM;
                                            btFormat.SubStrings["VIP"].Value = a.VIP;
                                            this.SIMStart.Text = a.SIM;
                                            this.VIPStart.Text = a.VIP;
                                            if (MOPB.UpdateCHmesBLL(this.IMEI_Start.Text, ProductTime, "", lj))
                                            {
                                                Result nResult1 = btFormat.Print("标签打印软件", waitout, out messages);
                                                Form1.Log("关联VIP和SIM打印了彩盒贴IMEI号为" + this.IMEI_Start.Text + "的制单", null);
                                                this.IMEI_Start.Clear();
                                                this.VIPStart.Clear();
                                                this.SIMStart.Clear();
                                                this.IMEI_Start.Focus();
                                            }
                                        }
                                        else if (a.SIM != "" && a.VIP == "")
                                        {
                                            this.SIMStart.Text = a.SIM;
                                            this.VIPStart.Focus();
                                        }
                                        else if (a.SIM == "" && a.VIP != "")
                                        {
                                            this.VIPStart.Text = a.VIP;
                                            this.SIMStart.Focus();
                                        }
                                        else
                                        {
                                            this.SIMStart.Focus();
                                        }
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("该IMEI号已被打印过！");
                                }
                            }
                            else
                            {
                                MessageBox.Show("IMEI打印起始号不能为空！");
                            }
                            btFormat.PrintSetup.Cache.FlushInterval = CacheFlushInterval.PerSession;
                            MOPB.UpdateStatusByZhiDanBLL(this.CB_ZhiDan.Text);
                        }
                        else if (this.Select_Template1.Text != "" && this.Select_Template2.Text != "")
                        {
                            LabelFormatDocument btFormat;
                            Messages messages;
                            int waitout = 10000;
                            if (this.IMEI_Start.Text != "")
                            {
                                if (!PMB.CheckIMEIBLL(this.IMEI_Start.Text))
                                {
                                    this.SIMStart.Focus();
                                }
                                else if (PMB.CheckCHOrJSIMEIBLL(this.IMEI_Start.Text, 2))
                                {
                                    list = PMB.SelectSnByIMEIBLL(this.IMEI_Start.Text);
                                    foreach (PrintMessage a in list)
                                    {
                                        if (a.SIM != "" && a.VIP != "")
                                        {
                                            for (int S = 1; S <= 2; S++)
                                            {
                                                if (S == 1)
                                                {
                                                    lj = this.Select_Template1.Text;
                                                    btFormat = btEngine.Documents.Open(lj);
                                                    //指定打印机名称
                                                    btFormat.PrintSetup.PrinterName = this.Printer1.Text;
                                                }
                                                else
                                                {
                                                    lj = this.Select_Template2.Text;
                                                    btFormat = btEngine.Documents.Open(lj);
                                                    //指定打印机名称
                                                    btFormat.PrintSetup.PrinterName = this.Printer2.Text;
                                                }
                                                //对模板相应字段进行赋值
                                                btFormat.SubStrings["IMEI"].Value = this.IMEI_Start.Text;
                                                GetValue("Information", "型号", out outString);
                                                btFormat.SubStrings[outString].Value = this.SoftModel.Text;
                                                GetValue("Information", "生产日期", out outString);
                                                btFormat.SubStrings[outString].Value = this.PrintDate.Text;
                                                GetValue("Information", "产品编码", out outString);
                                                btFormat.SubStrings[outString].Value = this.ProductNo.Text;
                                                GetValue("Information", "软件版本", out outString);
                                                btFormat.SubStrings[outString].Value = this.SoftwareVersion.Text;
                                                GetValue("Information", "备注", out outString);
                                                btFormat.SubStrings[outString].Value = this.Remake.Text;
                                                GetValue("Information", "服务卡号", out outString);
                                                btFormat.SubStrings[outString].Value = a.VIP;
                                                btFormat.SubStrings["SN"].Value = a.SN;
                                                btFormat.SubStrings["SIM"].Value = a.SIM;
                                                this.SIMStart.Text = a.SIM;
                                                this.VIPStart.Text = a.VIP;
                                                Result nResult1 = btFormat.Print("标签打印软件", waitout, out messages);
                                                btFormat.PrintSetup.Cache.FlushInterval = CacheFlushInterval.PerSession;
                                                Form1.Log("关联SIM和VIP打印了IMEI号为" + this.IMEI_Start.Text + "的彩盒贴制单", null);
                                            }
                                            this.IMEI_Start.Clear();
                                            this.VIPStart.Clear();
                                            this.SIMStart.Clear();
                                            this.IMEI_Start.Focus();
                                            string ProductTime = "";
                                            string NowTime = System.DateTime.Now.ToString("HH:mm:ss:fff");
                                            if (this.PrintDate.Text == "")
                                            {
                                                ProductTime = System.DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:fff");
                                            }
                                            else
                                            {
                                                ProductTime = this.PrintDate.Text.Trim() + " " + NowTime;
                                            }
                                            MOPB.UpdateCHmesBLL(this.IMEI_Start.Text, ProductTime, this.Select_Template1.Text, this.Select_Template2.Text);
                                        }
                                        else if (a.SIM != "" && a.VIP == "")
                                        {
                                            this.SIMStart.Text = a.SIM;
                                            this.VIPStart.Focus();
                                        }
                                        else if (a.SIM == "" && a.VIP != "")
                                        {
                                            this.VIPStart.Text = a.VIP;
                                            this.SIMStart.Focus();
                                        }
                                        else
                                        {
                                            this.SIMStart.Focus();
                                        }
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("该IMEI号已被打印过！");
                                }
                            }
                            else
                            {
                                MessageBox.Show("IMEI打印起始号不能为空！");
                            }
                            MOPB.UpdateStatusByZhiDanBLL(this.CB_ZhiDan.Text);
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
                else if (this.IMEI_Start.Text == "") { }
                else
                {
                    MessageBox.Show("IMEI号格式不正确，请重新输入！");
                    this.IMEI_Start.Clear();
                    this.IMEI_Start.Focus();
                    return;
                }
            }
        }

        private void SIMStart_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) {
                if (this.SIMStart.Text != "" && IsNumeric(this.SIMStart.Text))
                {
                    if (this.SIM_num1.Text != "" && this.SIM_num2.Text != "")
                    {
                        if (long.Parse(this.SIMStart.Text) < long.Parse(this.SIM_num1.Text) || long.Parse(this.SIMStart.Text) > long.Parse(this.SIM_num2.Text))
                        {
                            MessageBox.Show("SIM起始位不在范围内，请重新输入");
                            this.SIMStart.Clear();
                            this.SIMStart.Focus();
                            return;
                        }
                        else if (this.SIM_digits.Text != "" && this.SIM_prefix.Text != "")
                        {
                            int sim_width = this.SIMStart.Text.Length;
                            int sim_prefix_width = this.SIM_prefix.Text.Length;
                            string sim_prefix = this.SIMStart.Text.Substring(0, sim_prefix_width);
                            if (sim_width != int.Parse(this.SIM_digits.Text) || sim_prefix != this.SIM_prefix.Text)
                            {
                                MessageBox.Show("SIM位数或前缀不正确");
                                this.SIMStart.Clear();
                                this.SIMStart.Focus();
                                return;
                            }
                        }
                    }
                    if (PMB.CheckSIMBLL(this.SIMStart.Text))
                    {
                        MessageBox.Show("SIM号已存在");
                        this.SIMStart.Clear();
                        this.SIMStart.Focus();
                        return;
                    }
                    if (this.choose_sim.Checked == true)
                    {
                        Engine btEngine = new Engine();
                        btEngine.Start();
                        string lj = "";
                        LabelFormatDocument btFormat;
                        if (this.Select_Template1.Text != "" && this.Select_Template2.Text == "")
                        {
                            lj = this.Select_Template1.Text;
                            btFormat = btEngine.Documents.Open(lj);
                            //指定打印机名称
                            btFormat.PrintSetup.PrinterName = this.Printer1.Text;
                            //对模板相应字段进行赋值
                            GetValue("Information", "型号", out outString);
                            btFormat.SubStrings[outString].Value = this.SoftModel.Text;
                            GetValue("Information", "生产日期", out outString);
                            btFormat.SubStrings[outString].Value = this.PrintDate.Text;
                            GetValue("Information", "产品编码", out outString);
                            btFormat.SubStrings[outString].Value = this.ProductNo.Text;
                            GetValue("Information", "软件版本", out outString);
                            btFormat.SubStrings[outString].Value = this.SoftwareVersion.Text;
                            GetValue("Information", "备注", out outString);
                            btFormat.SubStrings[outString].Value = this.Remake.Text;
                            //打印份数,同序列打印的份数
                            btFormat.PrintSetup.IdenticalCopiesOfLabel = 1;

                            string ProductTime = "";
                            string NowTime = System.DateTime.Now.ToString("HH:mm:ss:fff");
                            if (this.PrintDate.Text == "")
                            {
                                ProductTime = System.DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:fff");
                            }
                            else
                            {
                                ProductTime = this.PrintDate.Text.Trim() + " " + NowTime;
                            }
                            Messages messages;
                            int waitout = 10000;
                            if (!PMB.CheckIMEIBLL(this.IMEI_Start.Text))
                            {
                                btFormat.SubStrings["IMEI"].Value = this.IMEI_Start.Text;
                                btFormat.SubStrings["SN"].Value = this.SN1_num.Text;
                                GetValue("Information", "SIM卡号", out outString);
                                btFormat.SubStrings[outString].Value = this.SIMStart.Text;
                                //记录打印信息日志
                                list.Add(new PrintMessage()
                                {
                                    Zhidan = this.CB_ZhiDan.Text.Trim(),
                                    IMEI = this.IMEI_Start.Text.Trim(),
                                    IMEIStart = this.IMEI_num1.Text.Trim(),
                                    IMEIEnd = this.IMEI_num2.Text.Trim(),
                                    SN = this.SN1_num.Text,
                                    IMEIRel = this.IMEIRel.Text.Trim(),
                                    SIM = this.SIMStart.Text.Trim(),
                                    VIP = this.VIPStart.Text.Trim(),
                                    BAT = "",
                                    SoftModel = this.SoftModel.Text.Trim(),
                                    Version = this.SoftwareVersion.Text.Trim(),
                                    Remark = this.Remake.Text.Trim(),
                                    JS_PrintTime = "",
                                    JS_TemplatePath = "",
                                    CH_PrintTime = ProductTime,
                                    CH_TemplatePath1 = lj,
                                    CH_TemplatePath2 = ""
                                });
                                if (PMB.InsertPrintMessageBLL(list))
                                {
                                    if (this.SN1_num.Text != "")
                                    {
                                        string sn1_prefix = SN1_num.Text.Substring(0, this.SN1_num.Text.Length - 5);
                                        long sn1_suffix = long.Parse(SN1_num.Text.Remove(0, (this.SN1_num.Text.Length) - 5));
                                        string sn1 = sn1_prefix + (sn1_suffix + 1).ToString().PadLeft(5, '0');
                                        string sn2_suffix = SN2_num.Text.Remove(0, (this.SN2_num.Text.Length) - 5);
                                        if (MOPB.UpdateSN3numberBLL(this.CB_ZhiDan.Text, sn1_prefix, (sn1_suffix + 1).ToString().PadLeft(5, '0'), sn2_suffix, this.VIP_num1.Text, this.VIP_num2.Text))
                                        {
                                            Result nResult1 = btFormat.Print("标签打印软件", waitout, out messages);
                                            Form1.Log("关联SIM打印了彩盒贴IMEI号为" + this.IMEI_Start.Text + "的制单", null);
                                            this.SN1_num.Text = sn1;
                                            this.IMEI_Start.Clear();
                                            this.SIMStart.Clear();
                                            this.IMEI_Start.Focus();
                                            this.SN1_num.ReadOnly = true;
                                            this.SN2_num.ReadOnly = true;
                                            this.VIP_num1.ReadOnly = true;
                                            this.VIP_num2.ReadOnly = true;
                                        }
                                    }
                                    else
                                    {
                                        if (MOPB.UpdateSN3numberBLL(this.CB_ZhiDan.Text, "", "", "", this.VIP_num1.Text, this.VIP_num2.Text))
                                        {
                                            Result nResult1 = btFormat.Print("标签打印软件", waitout, out messages);
                                            Form1.Log("关联SIM打印了彩盒贴IMEI号为" + this.IMEI_Start.Text + "的制单", null);
                                            this.IMEI_Start.Clear();
                                            this.SIMStart.Clear();
                                            this.IMEI_Start.Focus();
                                            this.SN1_num.ReadOnly = true;
                                            this.SN2_num.ReadOnly = true;
                                            this.VIP_num1.ReadOnly = true;
                                            this.VIP_num2.ReadOnly = true;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                list = PMB.SelectSnByIMEIBLL(this.IMEI_Start.Text);
                                foreach (PrintMessage a in list)
                                {
                                    GetValue("Information", "型号", out outString);
                                    btFormat.SubStrings[outString].Value = a.SoftModel;
                                    btFormat.SubStrings["BAT"].Value = a.BAT;
                                    btFormat.SubStrings["SN"].Value = a.SN;
                                    btFormat.SubStrings["IMEI"].Value = this.IMEI_Start.Text; ;
                                    btFormat.SubStrings["SIM"].Value = this.SIMStart.Text; ;
                                    if (MOPB.UpdateCHSIMBLL(this.IMEI_Start.Text, ProductTime, lj, "", this.SIMStart.Text))
                                    {
                                        Result nResult1 = btFormat.Print("标签打印软件", waitout, out messages);
                                        Form1.Log("关联SIM打印了彩盒贴IMEI号为" + this.IMEI_Start.Text + "的制单", null);
                                        this.IMEI_Start.Clear();
                                        this.SIMStart.Clear();
                                        this.IMEI_Start.Focus();
                                    }
                                }
                            }
                        }
                        else if (this.Select_Template1.Text == "" && this.Select_Template2.Text != "")
                        {
                            lj = this.Select_Template2.Text;
                            btFormat = btEngine.Documents.Open(lj);
                            //指定打印机名称
                            btFormat.PrintSetup.PrinterName = this.Printer1.Text;
                            //对模板相应字段进行赋值
                            GetValue("Information", "型号", out outString);
                            btFormat.SubStrings[outString].Value = this.SoftModel.Text;
                            GetValue("Information", "生产日期", out outString);
                            btFormat.SubStrings[outString].Value = this.PrintDate.Text;
                            GetValue("Information", "产品编码", out outString);
                            btFormat.SubStrings[outString].Value = this.ProductNo.Text;
                            GetValue("Information", "软件版本", out outString);
                            btFormat.SubStrings[outString].Value = this.SoftwareVersion.Text;
                            GetValue("Information", "备注", out outString);
                            btFormat.SubStrings[outString].Value = this.Remake.Text;
                            //打印份数,同序列打印的份数
                            btFormat.PrintSetup.IdenticalCopiesOfLabel = 1;

                            string ProductTime = "";
                            string NowTime = System.DateTime.Now.ToString("HH:mm:ss:fff");
                            if (this.PrintDate.Text == "")
                            {
                                ProductTime = System.DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:fff");
                            }
                            else
                            {
                                ProductTime = this.PrintDate.Text.Trim() + " " + NowTime;
                            }
                            Messages messages;
                            int waitout = 10000;
                            if (!PMB.CheckIMEIBLL(this.IMEI_Start.Text))
                            {
                                btFormat.SubStrings["IMEI"].Value = this.IMEI_Start.Text;
                                btFormat.SubStrings["SN"].Value = this.SN1_num.Text;
                                GetValue("Information", "SIM卡号", out outString);
                                btFormat.SubStrings[outString].Value = this.SIMStart.Text;
                                //记录打印信息日志
                                list.Add(new PrintMessage()
                                {
                                    Zhidan = this.CB_ZhiDan.Text.Trim(),
                                    IMEI = this.IMEI_Start.Text.Trim(),
                                    IMEIStart = this.IMEI_num1.Text.Trim(),
                                    IMEIEnd = this.IMEI_num2.Text.Trim(),
                                    SN = this.SN1_num.Text,
                                    IMEIRel = this.IMEIRel.Text.Trim(),
                                    SIM = this.SIMStart.Text.Trim(),
                                    VIP = this.VIPStart.Text.Trim(),
                                    BAT = "",
                                    SoftModel = this.SoftModel.Text.Trim(),
                                    Version = this.SoftwareVersion.Text.Trim(),
                                    Remark = this.Remake.Text.Trim(),
                                    JS_PrintTime = "",
                                    JS_TemplatePath = "",
                                    CH_PrintTime = ProductTime,
                                    CH_TemplatePath1 = "",
                                    CH_TemplatePath2 = lj
                                });
                                if (PMB.InsertPrintMessageBLL(list))
                                {
                                    if (this.SN1_num.Text != "")
                                    {
                                        string sn1_prefix = SN1_num.Text.Substring(0, this.SN1_num.Text.Length - 5);
                                        long sn1_suffix = long.Parse(SN1_num.Text.Remove(0, (this.SN1_num.Text.Length) - 5));
                                        string sn1 = sn1_prefix + (sn1_suffix + 1).ToString().PadLeft(5, '0');
                                        string sn2_suffix = SN2_num.Text.Remove(0, (this.SN2_num.Text.Length) - 5);
                                        if (MOPB.UpdateSN3numberBLL(this.CB_ZhiDan.Text, sn1_prefix, (sn1_suffix + 1).ToString().PadLeft(5, '0'), sn2_suffix, this.VIP_num1.Text, this.VIP_num2.Text))
                                        {
                                            Result nResult1 = btFormat.Print("标签打印软件", waitout, out messages);
                                            Form1.Log("关联SIM打印了彩盒贴IMEI号为" + this.IMEI_Start.Text + "的制单", null);
                                            this.SN1_num.Text = sn1;
                                            this.IMEI_Start.Clear();
                                            this.SIMStart.Clear();
                                            this.IMEI_Start.Focus();
                                            this.SN1_num.ReadOnly = true;
                                            this.SN2_num.ReadOnly = true;
                                            this.VIP_num1.ReadOnly = true;
                                            this.VIP_num2.ReadOnly = true;
                                        }
                                    }
                                    else
                                    {
                                        if (MOPB.UpdateSN3numberBLL(this.CB_ZhiDan.Text, "", "", "", this.VIP_num1.Text, this.VIP_num2.Text))
                                        {
                                            Result nResult1 = btFormat.Print("标签打印软件", waitout, out messages);
                                            Form1.Log("关联SIM打印了彩盒贴IMEI号为" + this.IMEI_Start.Text + "的制单", null);
                                            this.IMEI_Start.Clear();
                                            this.SIMStart.Clear();
                                            this.IMEI_Start.Focus();
                                            this.SN1_num.ReadOnly = true;
                                            this.SN2_num.ReadOnly = true;
                                            this.VIP_num1.ReadOnly = true;
                                            this.VIP_num2.ReadOnly = true;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                list = PMB.SelectSnByIMEIBLL(this.IMEI_Start.Text);
                                foreach (PrintMessage a in list)
                                {
                                    GetValue("Information", "型号", out outString);
                                    btFormat.SubStrings[outString].Value = a.SoftModel;
                                    btFormat.SubStrings["BAT"].Value = a.BAT;
                                    btFormat.SubStrings["SN"].Value = a.SN;
                                    btFormat.SubStrings["IMEI"].Value = this.IMEI_Start.Text;
                                    btFormat.SubStrings["SIM"].Value = this.SIMStart.Text;
                                    if (MOPB.UpdateCHSIMBLL(this.IMEI_Start.Text, ProductTime, "", lj, this.SIMStart.Text))
                                    {
                                        Result nResult1 = btFormat.Print("标签打印软件", waitout, out messages);
                                        Form1.Log("关联SIM打印了彩盒贴IMEI号为" + this.IMEI_Start.Text + "的制单", null);
                                        this.IMEI_Start.Clear();
                                        this.SIMStart.Clear();
                                        this.IMEI_Start.Focus();
                                    }
                                }
                            }
                        }
                        else if (this.Select_Template1.Text != "" && this.Select_Template2.Text != "")
                        {
                            string ProductTime = "";
                            string NowTime = System.DateTime.Now.ToString("HH:mm:ss:fff");
                            if (this.PrintDate.Text == "")
                            {
                                ProductTime = System.DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:fff");
                            }
                            else
                            {
                                ProductTime = this.PrintDate.Text.Trim() + " " + NowTime;
                            }
                            Messages messages;
                            int waitout = 10000;
                            if (!PMB.CheckIMEIBLL(this.IMEI_Start.Text))
                            {
                                for (int S = 1; S <= 2; S++)
                                {
                                    if (S == 1)
                                    {
                                        lj = this.Select_Template1.Text;
                                        btFormat = btEngine.Documents.Open(lj);
                                        //指定打印机名称
                                        btFormat.PrintSetup.PrinterName = this.Printer1.Text;
                                    }
                                    else
                                    {
                                        lj = this.Select_Template2.Text;
                                        btFormat = btEngine.Documents.Open(lj);
                                        //指定打印机名称
                                        btFormat.PrintSetup.PrinterName = this.Printer2.Text;
                                    }
                                    //对模板相应字段进行赋值
                                    GetValue("Information", "SIM卡号", out outString);
                                    btFormat.SubStrings[outString].Value = this.SIMStart.Text;
                                    GetValue("Information", "服务卡号", out outString);
                                    btFormat.SubStrings[outString].Value = this.VIPStart.Text;
                                    btFormat.SubStrings["IMEI"].Value = this.IMEI_Start.Text;
                                    GetValue("Information", "型号", out outString);
                                    btFormat.SubStrings[outString].Value = this.SoftModel.Text;
                                    GetValue("Information", "生产日期", out outString);
                                    btFormat.SubStrings[outString].Value = this.PrintDate.Text;
                                    GetValue("Information", "产品编码", out outString);
                                    btFormat.SubStrings[outString].Value = this.ProductNo.Text;
                                    GetValue("Information", "软件版本", out outString);
                                    btFormat.SubStrings[outString].Value = this.SoftwareVersion.Text;
                                    GetValue("Information", "备注", out outString);
                                    btFormat.SubStrings[outString].Value = this.Remake.Text;
                                    btFormat.SubStrings["SN"].Value = this.SN1_num.Text;
                                    Result nResult1 = btFormat.Print("标签打印软件", waitout, out messages);
                                    btFormat.PrintSetup.Cache.FlushInterval = CacheFlushInterval.PerSession;
                                    Form1.Log("关联SIM打印了IMEI号为" + this.IMEI_Start.Text + "的彩盒贴制单", null);
                                }
                                //记录打印信息日志
                                list.Add(new PrintMessage()
                                {
                                    Zhidan = this.CB_ZhiDan.Text.Trim(),
                                    IMEI = this.IMEI_Start.Text.Trim(),
                                    IMEIStart = this.IMEI_num1.Text.Trim(),
                                    IMEIEnd = this.IMEI_num2.Text.Trim(),
                                    SN = this.SN1_num.Text,
                                    IMEIRel = this.IMEIRel.Text.Trim(),
                                    SIM = this.SIMStart.Text.Trim(),
                                    VIP = this.VIPStart.Text.Trim(),
                                    BAT = "",
                                    SoftModel = this.SoftModel.Text.Trim(),
                                    Version = this.SoftwareVersion.Text.Trim(),
                                    Remark = this.Remake.Text.Trim(),
                                    JS_PrintTime = "",
                                    JS_TemplatePath = "",
                                    CH_PrintTime = ProductTime,
                                    CH_TemplatePath1 = this.Select_Template1.Text,
                                    CH_TemplatePath2 = this.Select_Template2.Text
                                });
                                if (PMB.InsertPrintMessageBLL(list))
                                {
                                    if (SN1_num.Text != "")
                                    {
                                        string sn1_prefix = SN1_num.Text.Substring(0, this.SN1_num.Text.Length - 5);
                                        long sn1_suffix = long.Parse(SN1_num.Text.Remove(0, (this.SN1_num.Text.Length) - 5));
                                        string sn1 = sn1_prefix + (sn1_suffix + 1).ToString().PadLeft(5, '0');
                                        string sn2_suffix = SN2_num.Text.Remove(0, (this.SN2_num.Text.Length) - 5);
                                        if (MOPB.UpdateSN3numberBLL(this.CB_ZhiDan.Text, sn1_prefix, (sn1_suffix + 1).ToString().PadLeft(5, '0'), sn2_suffix, this.VIP_num1.Text, this.VIP_num2.Text))
                                        {
                                            this.SN1_num.Text = sn1;
                                            this.IMEI_Start.Clear();
                                            this.SIMStart.Clear();
                                            this.IMEI_Start.Focus();
                                            this.SN1_num.ReadOnly = true;
                                            this.SN2_num.ReadOnly = true;
                                            this.VIP_num1.ReadOnly = true;
                                            this.VIP_num2.ReadOnly = true;
                                        }
                                    }
                                    else
                                    {
                                        if (MOPB.UpdateSN3numberBLL(this.CB_ZhiDan.Text, "", "", "", this.VIP_num1.Text, this.VIP_num2.Text))
                                        {
                                            Form1.Log("关联SIM打印了彩盒贴IMEI号为" + this.IMEI_Start.Text + "的制单", null);
                                            this.IMEI_Start.Clear();
                                            this.SIMStart.Clear();
                                            this.IMEI_Start.Focus();
                                            this.SN1_num.ReadOnly = true;
                                            this.SN2_num.ReadOnly = true;
                                            this.VIP_num1.ReadOnly = true;
                                            this.VIP_num2.ReadOnly = true;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                for (int S = 1; S <= 2; S++)
                                {
                                    if (S == 1)
                                    {
                                        lj = this.Select_Template1.Text;
                                        btFormat = btEngine.Documents.Open(lj);
                                        //指定打印机名称
                                        btFormat.PrintSetup.PrinterName = this.Printer1.Text;
                                    }
                                    else
                                    {
                                        lj = this.Select_Template2.Text;
                                        btFormat = btEngine.Documents.Open(lj);
                                        //指定打印机名称
                                        btFormat.PrintSetup.PrinterName = this.Printer2.Text;
                                    }
                                    //对模板相应字段进行赋值
                                    GetValue("Information", "SIM卡号", out outString);
                                    btFormat.SubStrings[outString].Value = this.SIMStart.Text;
                                    GetValue("Information", "服务卡号", out outString);
                                    btFormat.SubStrings[outString].Value = this.VIPStart.Text;
                                    btFormat.SubStrings["IMEI"].Value = this.IMEI_Start.Text;
                                    GetValue("Information", "型号", out outString);
                                    btFormat.SubStrings[outString].Value = this.SoftModel.Text;
                                    GetValue("Information", "生产日期", out outString);
                                    btFormat.SubStrings[outString].Value = this.PrintDate.Text;
                                    GetValue("Information", "产品编码", out outString);
                                    btFormat.SubStrings[outString].Value = this.ProductNo.Text;
                                    GetValue("Information", "软件版本", out outString);
                                    btFormat.SubStrings[outString].Value = this.SoftwareVersion.Text;
                                    GetValue("Information", "备注", out outString);
                                    btFormat.SubStrings[outString].Value = this.Remake.Text;
                                    list = PMB.SelectSnByIMEIBLL(this.IMEI_Start.Text);
                                    foreach (PrintMessage a in list)
                                    {
                                        btFormat.SubStrings["SN"].Value = a.SN;
                                    }
                                    Result nResult1 = btFormat.Print("标签打印软件", waitout, out messages);
                                    btFormat.PrintSetup.Cache.FlushInterval = CacheFlushInterval.PerSession;
                                    Form1.Log("关联SIM打印了IMEI号为" + this.IMEI_Start.Text + "的彩盒贴制单", null);
                                }
                                if (MOPB.UpdateCHSIMBLL(this.IMEI_Start.Text, ProductTime, this.Select_Template1.Text, this.Select_Template2.Text, this.SIMStart.Text))
                                {
                                    this.IMEI_Start.Clear();
                                    this.SIMStart.Clear();
                                    this.IMEI_Start.Focus();
                                }
                            }
                        }
                        //结束打印引擎
                        btEngine.Stop();
                    }
                    if (this.choose_all.Checked == true)
                    {
                        Engine btEngine = new Engine();
                        btEngine.Start();
                        string lj = "";
                        LabelFormatDocument btFormat;
                        if (this.Select_Template1.Text != "" && this.Select_Template2.Text == "")
                        {
                            lj = this.Select_Template1.Text;
                            btFormat = btEngine.Documents.Open(lj);
                            //指定打印机名称
                            btFormat.PrintSetup.PrinterName = this.Printer1.Text;
                            //对模板相应字段进行赋值
                            GetValue("Information", "型号", out outString);
                            btFormat.SubStrings[outString].Value = this.SoftModel.Text;
                            GetValue("Information", "生产日期", out outString);
                            btFormat.SubStrings[outString].Value = this.PrintDate.Text;
                            GetValue("Information", "产品编码", out outString);
                            btFormat.SubStrings[outString].Value = this.ProductNo.Text;
                            GetValue("Information", "软件版本", out outString);
                            btFormat.SubStrings[outString].Value = this.SoftwareVersion.Text;
                            GetValue("Information", "备注", out outString);
                            btFormat.SubStrings[outString].Value = this.Remake.Text;
                            //打印份数,同序列打印的份数
                            btFormat.PrintSetup.IdenticalCopiesOfLabel = 1;

                            Messages messages;
                            int waitout = 10000;
                            if (PMB.CheckCHOrJSIMEIBLL(this.IMEI_Start.Text, 2))
                            {
                                if (this.VIPStart.Text != "")
                                {
                                    string ProductTime = "";
                                    string NowTime = System.DateTime.Now.ToString("HH:mm:ss:fff");
                                    if (this.PrintDate.Text == "")
                                    {
                                        ProductTime = System.DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:fff");
                                    }
                                    else
                                    {
                                        ProductTime = this.PrintDate.Text.Trim() + " " + NowTime;
                                    }
                                    list = PMB.SelectSnByIMEIBLL(this.IMEI_Start.Text);
                                    foreach (PrintMessage a in list)
                                    {
                                        GetValue("Information", "型号", out outString);
                                        btFormat.SubStrings[outString].Value = a.SoftModel;
                                        btFormat.SubStrings["BAT"].Value = a.BAT;
                                        btFormat.SubStrings["SN"].Value = a.SN;
                                        btFormat.SubStrings["IMEI"].Value = this.IMEI_Start.Text;
                                        btFormat.SubStrings["SIM"].Value = this.SIMStart.Text;
                                        btFormat.SubStrings["VIP"].Value = this.VIPStart.Text;
                                        if (MOPB.UpdateCHSIMBLL(this.IMEI_Start.Text, ProductTime, lj, "", this.SIMStart.Text))
                                        {
                                            Result nResult1 = btFormat.Print("标签打印软件", waitout, out messages);
                                            Form1.Log("关联SIM && VIP打印了彩盒贴IMEI号为" + this.IMEI_Start.Text + "的制单", null);
                                            this.IMEI_Start.Clear();
                                            this.SIMStart.Clear();
                                            this.VIPStart.Clear();
                                            this.IMEI_Start.Focus();
                                        }
                                    }
                                }
                            }
                        }
                        else if (this.Select_Template1.Text == "" && this.Select_Template2.Text != "")
                        {
                            lj = this.Select_Template2.Text;
                            btFormat = btEngine.Documents.Open(lj);
                            //指定打印机名称
                            btFormat.PrintSetup.PrinterName = this.Printer1.Text;
                            //对模板相应字段进行赋值
                            GetValue("Information", "型号", out outString);
                            btFormat.SubStrings[outString].Value = this.SoftModel.Text;
                            GetValue("Information", "生产日期", out outString);
                            btFormat.SubStrings[outString].Value = this.PrintDate.Text;
                            GetValue("Information", "产品编码", out outString);
                            btFormat.SubStrings[outString].Value = this.ProductNo.Text;
                            GetValue("Information", "软件版本", out outString);
                            btFormat.SubStrings[outString].Value = this.SoftwareVersion.Text;
                            GetValue("Information", "备注", out outString);
                            btFormat.SubStrings[outString].Value = this.Remake.Text;
                            //打印份数,同序列打印的份数
                            btFormat.PrintSetup.IdenticalCopiesOfLabel = 1;

                            Messages messages;
                            int waitout = 10000;
                            if (PMB.CheckCHOrJSIMEIBLL(this.IMEI_Start.Text, 2))
                            {
                                if (this.VIPStart.Text != "")
                                {
                                    string ProductTime = "";
                                    string NowTime = System.DateTime.Now.ToString("HH:mm:ss:fff");
                                    if (this.PrintDate.Text == "")
                                    {
                                        ProductTime = System.DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:fff");
                                    }
                                    else
                                    {
                                        ProductTime = this.PrintDate.Text.Trim() + " " + NowTime;
                                    }
                                    list = PMB.SelectSnByIMEIBLL(this.IMEI_Start.Text);
                                    foreach (PrintMessage a in list)
                                    {
                                        GetValue("Information", "型号", out outString);
                                        btFormat.SubStrings[outString].Value = a.SoftModel;
                                        btFormat.SubStrings["BAT"].Value = a.BAT;
                                        btFormat.SubStrings["SN"].Value = a.SN;
                                        btFormat.SubStrings["IMEI"].Value = this.IMEI_Start.Text;
                                        btFormat.SubStrings["SIM"].Value = this.SIMStart.Text;
                                        btFormat.SubStrings["VIP"].Value = this.VIPStart.Text;
                                        if (MOPB.UpdateCHSIMBLL(this.IMEI_Start.Text, ProductTime, "", lj, this.SIMStart.Text))
                                        {
                                            Result nResult1 = btFormat.Print("标签打印软件", waitout, out messages);
                                            Form1.Log("关联SIM && VIP打印了彩盒贴IMEI号为" + this.IMEI_Start.Text + "的制单", null);
                                            this.IMEI_Start.Clear();
                                            this.SIMStart.Clear();
                                            this.VIPStart.Clear();
                                            this.IMEI_Start.Focus();
                                        }
                                    }
                                }
                            }
                        }
                        else if (this.Select_Template1.Text != "" && this.Select_Template2.Text != "")
                        {
                            Messages messages;
                            int waitout = 10000;
                            if (PMB.CheckCHOrJSIMEIBLL(this.IMEI_Start.Text, 2))
                            {
                                if (this.VIPStart.Text != "")
                                {
                                    for (int S = 1; S <= 2; S++)
                                    {
                                        if (S == 1)
                                        {
                                            lj = this.Select_Template1.Text;
                                            btFormat = btEngine.Documents.Open(lj);
                                            //指定打印机名称
                                            btFormat.PrintSetup.PrinterName = this.Printer1.Text;
                                        }
                                        else
                                        {
                                            lj = this.Select_Template2.Text;
                                            btFormat = btEngine.Documents.Open(lj);
                                            //指定打印机名称
                                            btFormat.PrintSetup.PrinterName = this.Printer2.Text;
                                        }
                                        //对模板相应字段进行赋值
                                        GetValue("Information", "SIM卡号", out outString);
                                        btFormat.SubStrings[outString].Value = this.SIMStart.Text;
                                        GetValue("Information", "服务卡号", out outString);
                                        btFormat.SubStrings[outString].Value = this.VIPStart.Text;
                                        btFormat.SubStrings["IMEI"].Value = this.IMEI_Start.Text;
                                        GetValue("Information", "型号", out outString);
                                        btFormat.SubStrings[outString].Value = this.SoftModel.Text;
                                        GetValue("Information", "生产日期", out outString);
                                        btFormat.SubStrings[outString].Value = this.PrintDate.Text;
                                        GetValue("Information", "产品编码", out outString);
                                        btFormat.SubStrings[outString].Value = this.ProductNo.Text;
                                        GetValue("Information", "软件版本", out outString);
                                        btFormat.SubStrings[outString].Value = this.SoftwareVersion.Text;
                                        GetValue("Information", "备注", out outString);
                                        btFormat.SubStrings[outString].Value = this.Remake.Text;
                                        list = PMB.SelectSnByIMEIBLL(this.IMEI_Start.Text);
                                        foreach (PrintMessage a in list)
                                        {
                                            btFormat.SubStrings["SN"].Value = a.SN;
                                        }
                                        Result nResult1 = btFormat.Print("标签打印软件", waitout, out messages);
                                        btFormat.PrintSetup.Cache.FlushInterval = CacheFlushInterval.PerSession;
                                        Form1.Log("关联SIM && VIP打印了IMEI号为" + this.IMEI_Start.Text + "的彩盒贴制单", null);
                                    }
                                    string ProductTime = "";
                                    string NowTime = System.DateTime.Now.ToString("HH:mm:ss:fff");
                                    if (this.PrintDate.Text == "")
                                    {
                                        ProductTime = System.DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:fff");
                                    }
                                    else
                                    {
                                        ProductTime = this.PrintDate.Text.Trim() + " " + NowTime;
                                    }
                                    if (MOPB.UpdateCHSIMBLL(this.IMEI_Start.Text, ProductTime, this.Select_Template1.Text, this.Select_Template2.Text, this.SIMStart.Text))
                                    {
                                        this.IMEI_Start.Clear();
                                        this.SIMStart.Clear();
                                        this.VIPStart.Clear();
                                        this.IMEI_Start.Focus();
                                    }
                                }
                            }
                        }
                        //结束打印引擎
                        btEngine.Stop();
                    }
                }
                else if (this.SIMStart.Text == "") { }
                else
                {
                    MessageBox.Show("SIM起始位输入格式不正确");
                    this.SIMStart.Clear();
                    this.SIMStart.Focus();
                    return;
                }
            }
        }

        private void VIPStart_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) {
                if (this.VIPStart.Text != "" && IsNumeric(this.VIPStart.Text))
                {
                    if (this.VIP_num1.Text != "" && this.VIP_num2.Text != "")
                    {
                        if (long.Parse(this.VIPStart.Text) < long.Parse(this.VIP_num1.Text) || long.Parse(this.VIPStart.Text) > long.Parse(this.VIP_num2.Text))
                        {
                            MessageBox.Show("VIP起始位不在范围内，请重新输入");
                            this.VIPStart.Clear();
                            this.VIPStart.Focus();
                            return;
                        }
                        else if (this.VIP_digits.Text != "" && this.VIP_prefix.Text != "")
                        {
                            int vip_width = this.VIPStart.Text.Length;
                            int vip_prefix_width = this.VIP_prefix.Text.Length;
                            string vip_prefix = this.VIPStart.Text.Substring(0, vip_prefix_width);
                            if (vip_width != int.Parse(this.VIP_digits.Text) || vip_prefix != this.VIP_prefix.Text)
                            {
                                MessageBox.Show("VIP位数或前缀不正确");
                                this.VIPStart.Clear();
                                this.VIPStart.Focus();
                                return;
                            }
                        }
                    }
                    if (PMB.CheckVIPBLL(this.VIPStart.Text))
                    {
                        MessageBox.Show("该VIP已被打印过");
                        this.VIPStart.Clear();
                        this.VIPStart.Focus();
                        return;
                    }
                    if (this.choose_vip.Checked == true)
                    {
                        Engine btEngine = new Engine();
                        btEngine.Start();
                        string lj = "";
                        LabelFormatDocument btFormat;
                        if (this.Select_Template1.Text != "" && this.Select_Template2.Text == "")
                        {
                            lj = this.Select_Template1.Text;
                            btFormat = btEngine.Documents.Open(lj);
                            //指定打印机名称
                            btFormat.PrintSetup.PrinterName = this.Printer1.Text;
                            //对模板相应字段进行赋值
                            GetValue("Information", "型号", out outString);
                            btFormat.SubStrings[outString].Value = this.SoftModel.Text;
                            GetValue("Information", "生产日期", out outString);
                            btFormat.SubStrings[outString].Value = this.PrintDate.Text;
                            GetValue("Information", "产品编码", out outString);
                            btFormat.SubStrings[outString].Value = this.ProductNo.Text;
                            GetValue("Information", "软件版本", out outString);
                            btFormat.SubStrings[outString].Value = this.SoftwareVersion.Text;
                            GetValue("Information", "备注", out outString);
                            btFormat.SubStrings[outString].Value = this.Remake.Text;
                            //打印份数,同序列打印的份数
                            btFormat.PrintSetup.IdenticalCopiesOfLabel = 1;

                            Messages messages;
                            int waitout = 10000;
                            if (!PMB.CheckIMEIBLL(this.IMEI_Start.Text))
                            {
                                btFormat.SubStrings["IMEI"].Value = this.IMEI_Start.Text;
                                btFormat.SubStrings["SN"].Value = this.SN1_num.Text;
                                GetValue("Information", "服务卡号", out outString);
                                btFormat.SubStrings[outString].Value = this.VIPStart.Text;
                                //记录打印信息日志
                                string ProductTime = "";
                                string NowTime = System.DateTime.Now.ToString("HH:mm:ss:fff");
                                if (this.PrintDate.Text == "")
                                {
                                    ProductTime = System.DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:fff");
                                }
                                else
                                {
                                    ProductTime = this.PrintDate.Text.Trim() + " " + NowTime;
                                }
                                list.Add(new PrintMessage()
                                {
                                    Zhidan = this.CB_ZhiDan.Text.Trim(),
                                    IMEI = this.IMEI_Start.Text.Trim(),
                                    IMEIStart = this.IMEI_num1.Text.Trim(),
                                    IMEIEnd = this.IMEI_num2.Text.Trim(),
                                    SN = this.SN1_num.Text,
                                    IMEIRel = this.IMEIRel.Text.Trim(),
                                    SIM = this.SIMStart.Text.Trim(),
                                    VIP = this.VIPStart.Text.Trim(),
                                    BAT = "",
                                    SoftModel = this.SoftModel.Text.Trim(),
                                    Version = this.SoftwareVersion.Text.Trim(),
                                    Remark = this.Remake.Text.Trim(),
                                    JS_PrintTime = "",
                                    JS_TemplatePath = "",
                                    CH_PrintTime = ProductTime,
                                    CH_TemplatePath1 = lj,
                                    CH_TemplatePath2 = ""
                                });
                                if (PMB.InsertPrintMessageBLL(list))
                                {
                                    if (this.SN1_num.Text != "")
                                    {
                                        string sn1_prefix = SN1_num.Text.Substring(0, this.SN1_num.Text.Length - 5);
                                        long sn1_suffix = long.Parse(SN1_num.Text.Remove(0, (this.SN1_num.Text.Length) - 5));
                                        string sn1 = sn1_prefix + (sn1_suffix + 1).ToString().PadLeft(5, '0');
                                        string sn2_suffix = SN2_num.Text.Remove(0, (this.SN2_num.Text.Length) - 5);
                                        if (MOPB.UpdateSN3numberBLL(this.CB_ZhiDan.Text, sn1_prefix, (sn1_suffix + 1).ToString().PadLeft(5, '0'), sn2_suffix, this.VIP_num1.Text, this.VIP_num2.Text))
                                        {
                                            Result nResult1 = btFormat.Print("标签打印软件", waitout, out messages);
                                            Form1.Log("关联VIP打印了彩盒贴IMEI号为" + this.IMEI_Start.Text + "的制单", null);
                                            this.SN1_num.Text = sn1;
                                            this.IMEI_Start.Clear();
                                            this.VIPStart.Clear();
                                            this.IMEI_Start.Focus();
                                            this.SN1_num.ReadOnly = true;
                                            this.SN2_num.ReadOnly = true;
                                            this.VIP_num1.ReadOnly = true;
                                            this.VIP_num2.ReadOnly = true;
                                        }
                                    }
                                    else
                                    {
                                        if (MOPB.UpdateSN3numberBLL(this.CB_ZhiDan.Text, "", "", "", this.VIP_num1.Text, this.VIP_num2.Text))
                                        {
                                            Result nResult1 = btFormat.Print("标签打印软件", waitout, out messages);
                                            Form1.Log("关联VIP打印了彩盒贴IMEI号为" + this.IMEI_Start.Text + "的制单", null);
                                            this.IMEI_Start.Clear();
                                            this.VIPStart.Clear();
                                            this.IMEI_Start.Focus();
                                            this.SN1_num.ReadOnly = true;
                                            this.SN2_num.ReadOnly = true;
                                            this.VIP_num1.ReadOnly = true;
                                            this.VIP_num2.ReadOnly = true;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                string ProductTime = "";
                                string NowTime = System.DateTime.Now.ToString("HH:mm:ss:fff");
                                if (this.PrintDate.Text == "")
                                {
                                    ProductTime = System.DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:fff");
                                }
                                else
                                {
                                    ProductTime = this.PrintDate.Text.Trim() + " " + NowTime;
                                }
                                list = PMB.SelectSnByIMEIBLL(this.IMEI_Start.Text);
                                foreach (PrintMessage a in list)
                                {
                                    GetValue("Information", "型号", out outString);
                                    btFormat.SubStrings[outString].Value = a.SoftModel;
                                    btFormat.SubStrings["BAT"].Value = a.BAT;
                                    btFormat.SubStrings["SN"].Value = a.SN;
                                    btFormat.SubStrings["IMEI"].Value = this.IMEI_Start.Text; ;
                                    btFormat.SubStrings["VIP"].Value = this.VIPStart.Text; ;
                                    if (MOPB.UpdateCHVIPBLL(this.IMEI_Start.Text, ProductTime, lj, "", this.VIPStart.Text))
                                    {
                                        Result nResult1 = btFormat.Print("标签打印软件", waitout, out messages);
                                        Form1.Log("关联VIP打印了彩盒贴IMEI号为" + this.IMEI_Start.Text + "的制单", null);
                                        this.IMEI_Start.Clear();
                                        this.VIPStart.Clear();
                                        this.IMEI_Start.Focus();
                                    }
                                }
                            }
                        }
                        else if (this.Select_Template1.Text == "" && this.Select_Template2.Text != "")
                        {
                            lj = this.Select_Template2.Text;
                            btFormat = btEngine.Documents.Open(lj);
                            //指定打印机名称
                            btFormat.PrintSetup.PrinterName = this.Printer1.Text;
                            //对模板相应字段进行赋值
                            GetValue("Information", "型号", out outString);
                            btFormat.SubStrings[outString].Value = this.SoftModel.Text;
                            GetValue("Information", "生产日期", out outString);
                            btFormat.SubStrings[outString].Value = this.PrintDate.Text;
                            GetValue("Information", "产品编码", out outString);
                            btFormat.SubStrings[outString].Value = this.ProductNo.Text;
                            GetValue("Information", "软件版本", out outString);
                            btFormat.SubStrings[outString].Value = this.SoftwareVersion.Text;
                            GetValue("Information", "备注", out outString);
                            btFormat.SubStrings[outString].Value = this.Remake.Text;
                            //打印份数,同序列打印的份数
                            btFormat.PrintSetup.IdenticalCopiesOfLabel = 1;

                            Messages messages;
                            int waitout = 10000;
                            if (!PMB.CheckIMEIBLL(this.IMEI_Start.Text))
                            {
                                btFormat.SubStrings["IMEI"].Value = this.IMEI_Start.Text;
                                btFormat.SubStrings["SN"].Value = this.SN1_num.Text;
                                GetValue("Information", "服务卡号", out outString);
                                btFormat.SubStrings[outString].Value = this.VIPStart.Text;
                                //记录打印信息日志
                                string ProductTime = "";
                                string NowTime = System.DateTime.Now.ToString("HH:mm:ss:fff");
                                if (this.PrintDate.Text == "")
                                {
                                    ProductTime = System.DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:fff");
                                }
                                else
                                {
                                    ProductTime = this.PrintDate.Text.Trim() + " " + NowTime;
                                }
                                list.Add(new PrintMessage()
                                {
                                    Zhidan = this.CB_ZhiDan.Text.Trim(),
                                    IMEI = this.IMEI_Start.Text.Trim(),
                                    IMEIStart = this.IMEI_num1.Text.Trim(),
                                    IMEIEnd = this.IMEI_num2.Text.Trim(),
                                    SN = this.SN1_num.Text,
                                    IMEIRel = this.IMEIRel.Text.Trim(),
                                    SIM = this.SIMStart.Text.Trim(),
                                    VIP = this.VIPStart.Text.Trim(),
                                    BAT = "",
                                    SoftModel = this.SoftModel.Text.Trim(),
                                    Version = this.SoftwareVersion.Text.Trim(),
                                    Remark = this.Remake.Text.Trim(),
                                    JS_PrintTime = "",
                                    JS_TemplatePath = "",
                                    CH_PrintTime = ProductTime,
                                    CH_TemplatePath1 = "",
                                    CH_TemplatePath2 = lj
                                });
                                if (PMB.InsertPrintMessageBLL(list))
                                {
                                    if (this.SN1_num.Text != "")
                                    {
                                        string sn1_prefix = SN1_num.Text.Substring(0, this.SN1_num.Text.Length - 5);
                                        long sn1_suffix = long.Parse(SN1_num.Text.Remove(0, (this.SN1_num.Text.Length) - 5));
                                        string sn1 = sn1_prefix + (sn1_suffix + 1).ToString().PadLeft(5, '0');
                                        string sn2_suffix = SN2_num.Text.Remove(0, (this.SN2_num.Text.Length) - 5);
                                        if (MOPB.UpdateSN3numberBLL(this.CB_ZhiDan.Text, sn1_prefix, (sn1_suffix + 1).ToString().PadLeft(5, '0'), sn2_suffix, this.VIP_num1.Text, this.VIP_num2.Text))
                                        {
                                            Result nResult1 = btFormat.Print("标签打印软件", waitout, out messages);
                                            Form1.Log("关联VIP打印了彩盒贴IMEI号为" + this.IMEI_Start.Text + "的制单", null);
                                            this.SN1_num.Text = sn1;
                                            this.IMEI_Start.Clear();
                                            this.VIPStart.Clear();
                                            this.IMEI_Start.Focus();
                                            this.SN1_num.ReadOnly = true;
                                            this.SN2_num.ReadOnly = true;
                                            this.VIP_num1.ReadOnly = true;
                                            this.VIP_num2.ReadOnly = true;
                                        }
                                    }
                                    else
                                    {
                                        if (MOPB.UpdateSN3numberBLL(this.CB_ZhiDan.Text, "", "", "", this.VIP_num1.Text, this.VIP_num2.Text))
                                        {
                                            Result nResult1 = btFormat.Print("标签打印软件", waitout, out messages);
                                            Form1.Log("关联VIP打印了彩盒贴IMEI号为" + this.IMEI_Start.Text + "的制单", null);
                                            this.IMEI_Start.Clear();
                                            this.VIPStart.Clear();
                                            this.IMEI_Start.Focus();
                                            this.SN1_num.ReadOnly = true;
                                            this.SN2_num.ReadOnly = true;
                                            this.VIP_num1.ReadOnly = true;
                                            this.VIP_num2.ReadOnly = true;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                string ProductTime = "";
                                string NowTime = System.DateTime.Now.ToString("HH:mm:ss:fff");
                                if (this.PrintDate.Text == "")
                                {
                                    ProductTime = System.DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:fff");
                                }
                                else
                                {
                                    ProductTime = this.PrintDate.Text.Trim() + " " + NowTime;
                                }
                                list = PMB.SelectSnByIMEIBLL(this.IMEI_Start.Text);
                                foreach (PrintMessage a in list)
                                {
                                    GetValue("Information", "型号", out outString);
                                    btFormat.SubStrings[outString].Value = a.SoftModel;
                                    btFormat.SubStrings["BAT"].Value = a.BAT;
                                    btFormat.SubStrings["SN"].Value = a.SN;
                                    btFormat.SubStrings["IMEI"].Value = this.IMEI_Start.Text; ;
                                    btFormat.SubStrings["VIP"].Value = this.VIPStart.Text; ;
                                    if (MOPB.UpdateCHVIPBLL(this.IMEI_Start.Text, ProductTime, "", lj, this.VIPStart.Text))
                                    {
                                        Result nResult1 = btFormat.Print("标签打印软件", waitout, out messages);
                                        Form1.Log("关联VIP打印了彩盒贴IMEI号为" + this.IMEI_Start.Text + "的制单", null);
                                        this.IMEI_Start.Clear();
                                        this.VIPStart.Clear();
                                        this.IMEI_Start.Focus();
                                    }
                                }
                            }
                        }
                        else if (this.Select_Template1.Text != "" && this.Select_Template2.Text != "")
                        {
                            Messages messages;
                            int waitout = 10000;
                            if (!PMB.CheckIMEIBLL(this.IMEI_Start.Text))
                            {
                                for (int S = 1; S <= 2; S++)
                                {
                                    if (S == 1)
                                    {
                                        lj = this.Select_Template1.Text;
                                        btFormat = btEngine.Documents.Open(lj);
                                        //指定打印机名称
                                        btFormat.PrintSetup.PrinterName = this.Printer1.Text;
                                    }
                                    else
                                    {
                                        lj = this.Select_Template2.Text;
                                        btFormat = btEngine.Documents.Open(lj);
                                        //指定打印机名称
                                        btFormat.PrintSetup.PrinterName = this.Printer2.Text;
                                    }
                                    //对模板相应字段进行赋值
                                    GetValue("Information", "SIM卡号", out outString);
                                    btFormat.SubStrings[outString].Value = this.SIMStart.Text;
                                    GetValue("Information", "服务卡号", out outString);
                                    btFormat.SubStrings[outString].Value = this.VIPStart.Text;
                                    btFormat.SubStrings["IMEI"].Value = this.IMEI_Start.Text;
                                    GetValue("Information", "型号", out outString);
                                    btFormat.SubStrings[outString].Value = this.SoftModel.Text;
                                    GetValue("Information", "生产日期", out outString);
                                    btFormat.SubStrings[outString].Value = this.PrintDate.Text;
                                    GetValue("Information", "产品编码", out outString);
                                    btFormat.SubStrings[outString].Value = this.ProductNo.Text;
                                    GetValue("Information", "软件版本", out outString);
                                    btFormat.SubStrings[outString].Value = this.SoftwareVersion.Text;
                                    GetValue("Information", "备注", out outString);
                                    btFormat.SubStrings[outString].Value = this.Remake.Text;
                                    btFormat.SubStrings["SN"].Value = this.SN1_num.Text;
                                    Result nResult1 = btFormat.Print("标签打印软件", waitout, out messages);
                                    btFormat.PrintSetup.Cache.FlushInterval = CacheFlushInterval.PerSession;
                                    Form1.Log("关联VIP打印了IMEI号为" + this.IMEI_Start.Text + "的彩盒贴制单", null);
                                }
                                //记录打印信息日志
                                string ProductTime = "";
                                string NowTime = System.DateTime.Now.ToString("HH:mm:ss:fff");
                                if (this.PrintDate.Text == "")
                                {
                                    ProductTime = System.DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:fff");
                                }
                                else
                                {
                                    ProductTime = this.PrintDate.Text.Trim() + " " + NowTime;
                                }
                                list.Add(new PrintMessage()
                                {
                                    Zhidan = this.CB_ZhiDan.Text.Trim(),
                                    IMEI = this.IMEI_Start.Text.Trim(),
                                    IMEIStart = this.IMEI_num1.Text.Trim(),
                                    IMEIEnd = this.IMEI_num2.Text.Trim(),
                                    SN = this.SN1_num.Text,
                                    IMEIRel = this.IMEIRel.Text.Trim(),
                                    SIM = this.SIMStart.Text.Trim(),
                                    VIP = this.VIPStart.Text.Trim(),
                                    BAT = "",
                                    SoftModel = this.SoftModel.Text.Trim(),
                                    Version = this.SoftwareVersion.Text.Trim(),
                                    Remark = this.Remake.Text.Trim(),
                                    JS_PrintTime = "",
                                    JS_TemplatePath = "",
                                    CH_PrintTime = ProductTime,
                                    CH_TemplatePath1 = this.Select_Template1.Text,
                                    CH_TemplatePath2 = this.Select_Template2.Text
                                });
                                if (PMB.InsertPrintMessageBLL(list))
                                {
                                    if (SN1_num.Text != "")
                                    {
                                        string sn1_prefix = SN1_num.Text.Substring(0, this.SN1_num.Text.Length - 5);
                                        long sn1_suffix = long.Parse(SN1_num.Text.Remove(0, (this.SN1_num.Text.Length) - 5));
                                        string sn1 = sn1_prefix + (sn1_suffix + 1).ToString().PadLeft(5, '0');
                                        string sn2_suffix = SN2_num.Text.Remove(0, (this.SN2_num.Text.Length) - 5);
                                        if (MOPB.UpdateSN3numberBLL(this.CB_ZhiDan.Text, sn1_prefix, (sn1_suffix + 1).ToString().PadLeft(5, '0'), sn2_suffix, this.VIP_num1.Text, this.VIP_num2.Text))
                                        {
                                            this.SN1_num.Text = sn1;
                                            this.IMEI_Start.Clear();
                                            this.VIPStart.Clear();
                                            this.IMEI_Start.Focus();
                                            this.SN1_num.ReadOnly = true;
                                            this.SN2_num.ReadOnly = true;
                                            this.VIP_num1.ReadOnly = true;
                                            this.VIP_num2.ReadOnly = true;
                                        }
                                    }
                                    else
                                    {
                                        if (MOPB.UpdateSN3numberBLL(this.CB_ZhiDan.Text, "", "", "", this.VIP_num1.Text, this.VIP_num2.Text))
                                        {
                                            Form1.Log("关联VIP打印了彩盒贴IMEI号为" + this.IMEI_Start.Text + "的制单", null);
                                            this.IMEI_Start.Clear();
                                            this.VIPStart.Clear();
                                            this.IMEI_Start.Focus();
                                            this.SN1_num.ReadOnly = true;
                                            this.SN2_num.ReadOnly = true;
                                            this.VIP_num1.ReadOnly = true;
                                            this.VIP_num2.ReadOnly = true;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                for (int S = 1; S <= 2; S++)
                                {
                                    if (S == 1)
                                    {
                                        lj = this.Select_Template1.Text;
                                        btFormat = btEngine.Documents.Open(lj);
                                        //指定打印机名称
                                        btFormat.PrintSetup.PrinterName = this.Printer1.Text;
                                    }
                                    else
                                    {
                                        lj = this.Select_Template2.Text;
                                        btFormat = btEngine.Documents.Open(lj);
                                        //指定打印机名称
                                        btFormat.PrintSetup.PrinterName = this.Printer2.Text;
                                    }
                                    //对模板相应字段进行赋值
                                    GetValue("Information", "SIM卡号", out outString);
                                    btFormat.SubStrings[outString].Value = this.SIMStart.Text;
                                    GetValue("Information", "服务卡号", out outString);
                                    btFormat.SubStrings[outString].Value = this.VIPStart.Text;
                                    btFormat.SubStrings["IMEI"].Value = this.IMEI_Start.Text;
                                    GetValue("Information", "型号", out outString);
                                    btFormat.SubStrings[outString].Value = this.SoftModel.Text;
                                    GetValue("Information", "生产日期", out outString);
                                    btFormat.SubStrings[outString].Value = this.PrintDate.Text;
                                    GetValue("Information", "产品编码", out outString);
                                    btFormat.SubStrings[outString].Value = this.ProductNo.Text;
                                    GetValue("Information", "软件版本", out outString);
                                    btFormat.SubStrings[outString].Value = this.SoftwareVersion.Text;
                                    GetValue("Information", "备注", out outString);
                                    btFormat.SubStrings[outString].Value = this.Remake.Text;
                                    list = PMB.SelectSnByIMEIBLL(this.IMEI_Start.Text);
                                    foreach (PrintMessage a in list)
                                    {
                                        btFormat.SubStrings["SN"].Value = a.SN;
                                    }
                                    Result nResult1 = btFormat.Print("标签打印软件", waitout, out messages);
                                    btFormat.PrintSetup.Cache.FlushInterval = CacheFlushInterval.PerSession;
                                    Form1.Log("关联VIP打印了IMEI号为" + this.IMEI_Start.Text + "的彩盒贴制单", null);
                                }
                                string ProductTime = "";
                                string NowTime = System.DateTime.Now.ToString("HH:mm:ss:fff");
                                if (this.PrintDate.Text == "")
                                {
                                    ProductTime = System.DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:fff");
                                }
                                else
                                {
                                    ProductTime = this.PrintDate.Text.Trim() + " " + NowTime;
                                }
                                if (MOPB.UpdateCHVIPBLL(this.IMEI_Start.Text, ProductTime, this.Select_Template1.Text, this.Select_Template2.Text, this.VIPStart.Text))
                                {
                                    this.IMEI_Start.Clear();
                                    this.VIPStart.Clear();
                                    this.IMEI_Start.Focus();
                                }
                            }
                        }
                        //结束打印引擎
                        btEngine.Stop();
                    }
                    if (this.choose_all.Checked == true)
                    {
                        Engine btEngine = new Engine();
                        btEngine.Start();
                        string lj = "";
                        LabelFormatDocument btFormat;
                        if (this.Select_Template1.Text != "" && this.Select_Template2.Text == "")
                        {
                            lj = this.Select_Template1.Text;
                            btFormat = btEngine.Documents.Open(lj);
                            //指定打印机名称
                            btFormat.PrintSetup.PrinterName = this.Printer1.Text;
                            //对模板相应字段进行赋值
                            GetValue("Information", "型号", out outString);
                            btFormat.SubStrings[outString].Value = this.SoftModel.Text;
                            GetValue("Information", "生产日期", out outString);
                            btFormat.SubStrings[outString].Value = this.PrintDate.Text;
                            GetValue("Information", "产品编码", out outString);
                            btFormat.SubStrings[outString].Value = this.ProductNo.Text;
                            GetValue("Information", "软件版本", out outString);
                            btFormat.SubStrings[outString].Value = this.SoftwareVersion.Text;
                            GetValue("Information", "备注", out outString);
                            btFormat.SubStrings[outString].Value = this.Remake.Text;
                            //打印份数,同序列打印的份数
                            btFormat.PrintSetup.IdenticalCopiesOfLabel = 1;

                            Messages messages;
                            int waitout = 10000;
                            if (!PMB.CheckIMEIBLL(this.IMEI_Start.Text))
                            {
                                btFormat.SubStrings["IMEI"].Value = this.IMEI_Start.Text;
                                GetValue("Information", "服务卡号", out outString);
                                btFormat.SubStrings[outString].Value = this.VIPStart.Text;
                                GetValue("Information", "SIM卡号", out outString);
                                btFormat.SubStrings[outString].Value = this.SIMStart.Text;
                                //记录打印信息日志
                                string ProductTime = "";
                                string NowTime = System.DateTime.Now.ToString("HH:mm:ss:fff");
                                if (this.PrintDate.Text == "")
                                {
                                    ProductTime = System.DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:fff");
                                }
                                else
                                {
                                    ProductTime = this.PrintDate.Text.Trim() + " " + NowTime;
                                }
                                btFormat.SubStrings["SN"].Value = this.SN1_num.Text;
                                list.Add(new PrintMessage()
                                {
                                    Zhidan = this.CB_ZhiDan.Text.Trim(),
                                    IMEI = this.IMEI_Start.Text.Trim(),
                                    IMEIStart = this.IMEI_num1.Text.Trim(),
                                    IMEIEnd = this.IMEI_num2.Text.Trim(),
                                    SN = this.SN1_num.Text,
                                    IMEIRel = this.IMEIRel.Text.Trim(),
                                    SIM = this.SIMStart.Text.Trim(),
                                    VIP = this.VIPStart.Text.Trim(),
                                    BAT = "",
                                    SoftModel = this.SoftModel.Text.Trim(),
                                    Version = this.SoftwareVersion.Text.Trim(),
                                    Remark = this.Remake.Text.Trim(),
                                    JS_PrintTime = "",
                                    JS_TemplatePath = "",
                                    CH_PrintTime = ProductTime,
                                    CH_TemplatePath1 = lj,
                                    CH_TemplatePath2 = ""
                                });
                                if (PMB.InsertPrintMessageBLL(list))
                                {
                                    if (this.SN1_num.Text != "")
                                    {
                                        string sn1_prefix = SN1_num.Text.Substring(0, this.SN1_num.Text.Length - 5);
                                        long sn1_suffix = long.Parse(SN1_num.Text.Remove(0, (this.SN1_num.Text.Length) - 5));
                                        string sn1 = sn1_prefix + (sn1_suffix + 1).ToString().PadLeft(5, '0');
                                        string sn2_suffix = SN2_num.Text.Remove(0, (this.SN2_num.Text.Length) - 5);
                                        if (MOPB.UpdateSN3numberBLL(this.CB_ZhiDan.Text, sn1_prefix, (sn1_suffix + 1).ToString().PadLeft(5, '0'), sn2_suffix, this.VIP_num1.Text, this.VIP_num2.Text))
                                        {
                                            Result nResult1 = btFormat.Print("标签打印软件", waitout, out messages);
                                            Form1.Log("关联SIM和VIP打印了彩盒贴IMEI号为" + this.IMEI_Start.Text + "的制单", null);
                                            this.SN1_num.Text = sn1;
                                            this.IMEI_Start.Clear();
                                            this.SIMStart.Clear();
                                            this.VIPStart.Clear();
                                            this.IMEI_Start.Focus();
                                            this.SN1_num.ReadOnly = true;
                                            this.SN2_num.ReadOnly = true;
                                            this.VIP_num1.ReadOnly = true;
                                            this.VIP_num2.ReadOnly = true;
                                        }
                                    }
                                    else
                                    {
                                        if (MOPB.UpdateSN3numberBLL(this.CB_ZhiDan.Text, "", "", "", this.VIP_num1.Text, this.VIP_num2.Text))
                                        {
                                            Result nResult1 = btFormat.Print("标签打印软件", waitout, out messages);
                                            Form1.Log("关联SIM打印了彩盒贴IMEI号为" + this.IMEI_Start.Text + "的制单", null);
                                            this.IMEI_Start.Clear();
                                            this.SIMStart.Clear();
                                            this.VIPStart.Clear();
                                            this.IMEI_Start.Focus();
                                            this.SN1_num.ReadOnly = true;
                                            this.SN2_num.ReadOnly = true;
                                            this.VIP_num1.ReadOnly = true;
                                            this.VIP_num2.ReadOnly = true;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                string ProductTime = "";
                                string NowTime = System.DateTime.Now.ToString("HH:mm:ss:fff");
                                if (this.PrintDate.Text == "")
                                {
                                    ProductTime = System.DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:fff");
                                }
                                else
                                {
                                    ProductTime = this.PrintDate.Text.Trim() + " " + NowTime;
                                }
                                list = PMB.SelectSnByIMEIBLL(this.IMEI_Start.Text);
                                foreach (PrintMessage a in list)
                                {
                                    GetValue("Information", "型号", out outString);
                                    btFormat.SubStrings[outString].Value = a.SoftModel;
                                    btFormat.SubStrings["BAT"].Value = a.BAT;
                                    btFormat.SubStrings["SN"].Value = a.SN;
                                    btFormat.SubStrings["IMEI"].Value = this.IMEI_Start.Text;
                                    btFormat.SubStrings["VIP"].Value = this.VIPStart.Text;
                                    btFormat.SubStrings["SIM"].Value = this.SIMStart.Text;
                                    if (MOPB.UpdateCHVIPAndSimBLL(this.IMEI_Start.Text, ProductTime, lj, "", this.VIPStart.Text, this.SIMStart.Text))
                                    {
                                        Result nResult1 = btFormat.Print("标签打印软件", waitout, out messages);
                                        Form1.Log("关联SIM和VIP打印了彩盒贴IMEI号为" + this.IMEI_Start.Text + "的制单", null);
                                        this.IMEI_Start.Clear();
                                        this.SIMStart.Clear();
                                        this.VIPStart.Clear();
                                        this.IMEI_Start.Focus();
                                    }
                                }
                            }
                        }
                        else if (this.Select_Template1.Text == "" && this.Select_Template2.Text != "")
                        {
                            lj = this.Select_Template2.Text;
                            btFormat = btEngine.Documents.Open(lj);
                            //指定打印机名称
                            btFormat.PrintSetup.PrinterName = this.Printer1.Text;
                            //对模板相应字段进行赋值
                            GetValue("Information", "型号", out outString);
                            btFormat.SubStrings[outString].Value = this.SoftModel.Text;
                            GetValue("Information", "生产日期", out outString);
                            btFormat.SubStrings[outString].Value = this.PrintDate.Text;
                            GetValue("Information", "产品编码", out outString);
                            btFormat.SubStrings[outString].Value = this.ProductNo.Text;
                            GetValue("Information", "软件版本", out outString);
                            btFormat.SubStrings[outString].Value = this.SoftwareVersion.Text;
                            GetValue("Information", "备注", out outString);
                            btFormat.SubStrings[outString].Value = this.Remake.Text;
                            //打印份数,同序列打印的份数
                            btFormat.PrintSetup.IdenticalCopiesOfLabel = 1;

                            Messages messages;
                            int waitout = 10000;
                            if (!PMB.CheckIMEIBLL(this.IMEI_Start.Text))
                            {
                                btFormat.SubStrings["IMEI"].Value = this.IMEI_Start.Text;
                                GetValue("Information", "服务卡号", out outString);
                                btFormat.SubStrings[outString].Value = this.VIPStart.Text;
                                GetValue("Information", "SIM卡号", out outString);
                                btFormat.SubStrings[outString].Value = this.SIMStart.Text;
                                //记录打印信息日志
                                string ProductTime = "";
                                string NowTime = System.DateTime.Now.ToString("HH:mm:ss:fff");
                                if (this.PrintDate.Text == "")
                                {
                                    ProductTime = System.DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:fff");
                                }
                                else
                                {
                                    ProductTime = this.PrintDate.Text.Trim() + " " + NowTime;
                                }
                                btFormat.SubStrings["SN"].Value = this.SN1_num.Text;
                                list.Add(new PrintMessage()
                                {
                                    Zhidan = this.CB_ZhiDan.Text.Trim(),
                                    IMEI = this.IMEI_Start.Text.Trim(),
                                    IMEIStart = this.IMEI_num1.Text.Trim(),
                                    IMEIEnd = this.IMEI_num2.Text.Trim(),
                                    SN = this.SN1_num.Text,
                                    IMEIRel = this.IMEIRel.Text.Trim(),
                                    SIM = this.SIMStart.Text.Trim(),
                                    VIP = this.VIPStart.Text.Trim(),
                                    BAT = "",
                                    SoftModel = this.SoftModel.Text.Trim(),
                                    Version = this.SoftwareVersion.Text.Trim(),
                                    Remark = this.Remake.Text.Trim(),
                                    JS_PrintTime = "",
                                    JS_TemplatePath = "",
                                    CH_PrintTime = ProductTime,
                                    CH_TemplatePath1 = "",
                                    CH_TemplatePath2 = lj
                                });
                                if (PMB.InsertPrintMessageBLL(list))
                                {
                                    if (this.SN1_num.Text != "")
                                    {
                                        string sn1_prefix = SN1_num.Text.Substring(0, this.SN1_num.Text.Length - 5);
                                        long sn1_suffix = long.Parse(SN1_num.Text.Remove(0, (this.SN1_num.Text.Length) - 5));
                                        string sn1 = sn1_prefix + (sn1_suffix + 1).ToString().PadLeft(5, '0');
                                        string sn2_suffix = SN2_num.Text.Remove(0, (this.SN2_num.Text.Length) - 5);
                                        if (MOPB.UpdateSN3numberBLL(this.CB_ZhiDan.Text, sn1_prefix, (sn1_suffix + 1).ToString().PadLeft(5, '0'), sn2_suffix, this.VIP_num1.Text, this.VIP_num2.Text))
                                        {
                                            Result nResult1 = btFormat.Print("标签打印软件", waitout, out messages);
                                            Form1.Log("关联SIM和VIP打印了彩盒贴IMEI号为" + this.IMEI_Start.Text + "的制单", null);
                                            this.SN1_num.Text = sn1;
                                            this.IMEI_Start.Clear();
                                            this.SIMStart.Clear();
                                            this.VIPStart.Clear();
                                            this.IMEI_Start.Focus();
                                            this.SN1_num.ReadOnly = true;
                                            this.SN2_num.ReadOnly = true;
                                            this.VIP_num1.ReadOnly = true;
                                            this.VIP_num2.ReadOnly = true;
                                        }
                                    }
                                    else
                                    {
                                        if (MOPB.UpdateSN3numberBLL(this.CB_ZhiDan.Text, "", "", "", this.VIP_num1.Text, this.VIP_num2.Text))
                                        {
                                            Result nResult1 = btFormat.Print("标签打印软件", waitout, out messages);
                                            Form1.Log("关联SIM和VIP打印了彩盒贴IMEI号为" + this.IMEI_Start.Text + "的制单", null);
                                            this.IMEI_Start.Clear();
                                            this.SIMStart.Clear();
                                            this.VIPStart.Clear();
                                            this.IMEI_Start.Focus();
                                            this.SN1_num.ReadOnly = true;
                                            this.SN2_num.ReadOnly = true;
                                            this.VIP_num1.ReadOnly = true;
                                            this.VIP_num2.ReadOnly = true;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                string ProductTime = "";
                                string NowTime = System.DateTime.Now.ToString("HH:mm:ss:fff");
                                if (this.PrintDate.Text == "")
                                {
                                    ProductTime = System.DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:fff");
                                }
                                else
                                {
                                    ProductTime = this.PrintDate.Text.Trim() + " " + NowTime;
                                }
                                list = PMB.SelectSnByIMEIBLL(this.IMEI_Start.Text);
                                foreach (PrintMessage a in list)
                                {
                                    GetValue("Information", "型号", out outString);
                                    btFormat.SubStrings[outString].Value = a.SoftModel;
                                    btFormat.SubStrings["BAT"].Value = a.BAT;
                                    btFormat.SubStrings["SN"].Value = a.SN;
                                    btFormat.SubStrings["IMEI"].Value = this.IMEI_Start.Text;
                                    btFormat.SubStrings["VIP"].Value = this.VIPStart.Text;
                                    btFormat.SubStrings["SIM"].Value = this.SIMStart.Text;
                                    if (MOPB.UpdateCHVIPAndSimBLL(this.IMEI_Start.Text, ProductTime, "", lj, this.VIPStart.Text, this.SIMStart.Text))
                                    {
                                        Result nResult1 = btFormat.Print("标签打印软件", waitout, out messages);
                                        Form1.Log("关联VIP和VIP打印了彩盒贴IMEI号为" + this.IMEI_Start.Text + "的制单", null);
                                        this.IMEI_Start.Clear();
                                        this.SIMStart.Clear();
                                        this.VIPStart.Clear();
                                        this.IMEI_Start.Focus();
                                    }
                                }
                            }
                        }
                        else if (this.Select_Template1.Text != "" && this.Select_Template2.Text != "")
                        {
                            Messages messages;
                            int waitout = 10000;
                            if (!PMB.CheckIMEIBLL(this.IMEI_Start.Text))
                            {
                                for (int S = 1; S <= 2; S++)
                                {
                                    if (S == 1)
                                    {
                                        lj = this.Select_Template1.Text;
                                        btFormat = btEngine.Documents.Open(lj);
                                        //指定打印机名称
                                        btFormat.PrintSetup.PrinterName = this.Printer1.Text;
                                    }
                                    else
                                    {
                                        lj = this.Select_Template2.Text;
                                        btFormat = btEngine.Documents.Open(lj);
                                        //指定打印机名称
                                        btFormat.PrintSetup.PrinterName = this.Printer2.Text;
                                    }
                                    //对模板相应字段进行赋值
                                    GetValue("Information", "SIM卡号", out outString);
                                    btFormat.SubStrings[outString].Value = this.SIMStart.Text;
                                    GetValue("Information", "服务卡号", out outString);
                                    btFormat.SubStrings[outString].Value = this.VIPStart.Text;
                                    btFormat.SubStrings["IMEI"].Value = this.IMEI_Start.Text;
                                    GetValue("Information", "型号", out outString);
                                    btFormat.SubStrings[outString].Value = this.SoftModel.Text;
                                    GetValue("Information", "生产日期", out outString);
                                    btFormat.SubStrings[outString].Value = this.PrintDate.Text;
                                    GetValue("Information", "产品编码", out outString);
                                    btFormat.SubStrings[outString].Value = this.ProductNo.Text;
                                    GetValue("Information", "软件版本", out outString);
                                    btFormat.SubStrings[outString].Value = this.SoftwareVersion.Text;
                                    GetValue("Information", "备注", out outString);
                                    btFormat.SubStrings[outString].Value = this.Remake.Text;
                                    btFormat.SubStrings["SN"].Value = this.SN1_num.Text;
                                    Result nResult1 = btFormat.Print("标签打印软件", waitout, out messages);
                                    btFormat.PrintSetup.Cache.FlushInterval = CacheFlushInterval.PerSession;
                                    Form1.Log("关联SIM和VIP打印了IMEI号为" + this.IMEI_Start.Text + "的彩盒贴制单", null);
                                }
                                //记录打印信息日志
                                string ProductTime = "";
                                string NowTime = System.DateTime.Now.ToString("HH:mm:ss:fff");
                                if (this.PrintDate.Text == "")
                                {
                                    ProductTime = System.DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:fff");
                                }
                                else
                                {
                                    ProductTime = this.PrintDate.Text.Trim() + " " + NowTime;
                                }
                                list.Add(new PrintMessage()
                                {
                                    Zhidan = this.CB_ZhiDan.Text.Trim(),
                                    IMEI = this.IMEI_Start.Text.Trim(),
                                    IMEIStart = this.IMEI_num1.Text.Trim(),
                                    IMEIEnd = this.IMEI_num2.Text.Trim(),
                                    SN = this.SN1_num.Text,
                                    IMEIRel = this.IMEIRel.Text.Trim(),
                                    SIM = this.SIMStart.Text.Trim(),
                                    VIP = this.VIPStart.Text.Trim(),
                                    BAT = "",
                                    SoftModel = this.SoftModel.Text.Trim(),
                                    Version = this.SoftwareVersion.Text.Trim(),
                                    Remark = this.Remake.Text.Trim(),
                                    JS_PrintTime = "",
                                    JS_TemplatePath = "",
                                    CH_PrintTime = ProductTime,
                                    CH_TemplatePath1 = this.Select_Template1.Text,
                                    CH_TemplatePath2 = this.Select_Template2.Text
                                });
                                if (PMB.InsertPrintMessageBLL(list))
                                {
                                    if (SN1_num.Text != "")
                                    {
                                        string sn1_prefix = SN1_num.Text.Substring(0, this.SN1_num.Text.Length - 5);
                                        long sn1_suffix = long.Parse(SN1_num.Text.Remove(0, (this.SN1_num.Text.Length) - 5));
                                        string sn1 = sn1_prefix + (sn1_suffix + 1).ToString().PadLeft(5, '0');
                                        string sn2_suffix = SN2_num.Text.Remove(0, (this.SN2_num.Text.Length) - 5);
                                        if (MOPB.UpdateSN3numberBLL(this.CB_ZhiDan.Text, sn1_prefix, (sn1_suffix + 1).ToString().PadLeft(5, '0'), sn2_suffix, this.VIP_num1.Text, this.VIP_num2.Text))
                                        {
                                            this.SN1_num.Text = sn1;
                                            this.IMEI_Start.Clear();
                                            this.SIMStart.Clear();
                                            this.VIPStart.Clear();
                                            this.IMEI_Start.Focus();
                                            this.SN1_num.ReadOnly = true;
                                            this.SN2_num.ReadOnly = true;
                                            this.VIP_num1.ReadOnly = true;
                                            this.VIP_num2.ReadOnly = true;
                                        }
                                    }
                                    else
                                    {
                                        if (MOPB.UpdateSN3numberBLL(this.CB_ZhiDan.Text, "", "", "", this.VIP_num1.Text, this.VIP_num2.Text))
                                        {
                                            this.IMEI_Start.Clear();
                                            this.SIMStart.Clear();
                                            this.VIPStart.Clear();
                                            this.IMEI_Start.Focus();
                                            this.SN1_num.ReadOnly = true;
                                            this.SN2_num.ReadOnly = true;
                                            this.VIP_num1.ReadOnly = true;
                                            this.VIP_num2.ReadOnly = true;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                for (int S = 1; S <= 2; S++)
                                {
                                    if (S == 1)
                                    {
                                        lj = this.Select_Template1.Text;
                                        btFormat = btEngine.Documents.Open(lj);
                                        //指定打印机名称
                                        btFormat.PrintSetup.PrinterName = this.Printer1.Text;
                                    }
                                    else
                                    {
                                        lj = this.Select_Template2.Text;
                                        btFormat = btEngine.Documents.Open(lj);
                                        //指定打印机名称
                                        btFormat.PrintSetup.PrinterName = this.Printer2.Text;
                                    }
                                    //对模板相应字段进行赋值
                                    GetValue("Information", "SIM卡号", out outString);
                                    btFormat.SubStrings[outString].Value = this.SIMStart.Text;
                                    GetValue("Information", "服务卡号", out outString);
                                    btFormat.SubStrings[outString].Value = this.VIPStart.Text;
                                    btFormat.SubStrings["IMEI"].Value = this.IMEI_Start.Text;
                                    GetValue("Information", "型号", out outString);
                                    btFormat.SubStrings[outString].Value = this.SoftModel.Text;
                                    GetValue("Information", "生产日期", out outString);
                                    btFormat.SubStrings[outString].Value = this.PrintDate.Text;
                                    GetValue("Information", "产品编码", out outString);
                                    btFormat.SubStrings[outString].Value = this.ProductNo.Text;
                                    GetValue("Information", "软件版本", out outString);
                                    btFormat.SubStrings[outString].Value = this.SoftwareVersion.Text;
                                    GetValue("Information", "备注", out outString);
                                    btFormat.SubStrings[outString].Value = this.Remake.Text;
                                    list = PMB.SelectSnByIMEIBLL(this.IMEI_Start.Text);
                                    foreach (PrintMessage a in list)
                                    {
                                        btFormat.SubStrings["SN"].Value = a.SN;
                                    }
                                    Result nResult1 = btFormat.Print("标签打印软件", waitout, out messages);
                                    btFormat.PrintSetup.Cache.FlushInterval = CacheFlushInterval.PerSession;
                                    Form1.Log("关联SIM和VIP打印了IMEI号为" + this.IMEI_Start.Text + "的彩盒贴制单", null);
                                }
                                string ProductTime = "";
                                string NowTime = System.DateTime.Now.ToString("HH:mm:ss:fff");
                                if (this.PrintDate.Text == "")
                                {
                                    ProductTime = System.DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:fff");
                                }
                                else
                                {
                                    ProductTime = this.PrintDate.Text.Trim() + " " + NowTime;
                                }
                                if (MOPB.UpdateCHVIPAndSimBLL(this.IMEI_Start.Text, ProductTime, this.Select_Template1.Text, this.Select_Template2.Text, this.VIPStart.Text, this.SIMStart.Text))
                                {
                                    this.IMEI_Start.Clear();
                                    this.SIMStart.Clear();
                                    this.VIPStart.Clear();
                                    this.IMEI_Start.Focus();
                                }
                            }
                        }
                        //结束打印引擎
                        btEngine.Stop();
                    }
                }
                else if (this.SIMStart.Text == "") { }
                else
                {
                    MessageBox.Show("VIP起始位输入格式不正确");
                    this.VIPStart.Clear();
                    this.VIPStart.Focus();
                    return;
                }
            }
        }

    }
}
