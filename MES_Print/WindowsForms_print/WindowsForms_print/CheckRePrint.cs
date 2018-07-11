using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Print.Message.Bll;
using Print_Message;

namespace WindowsForms_print
{

    public partial class CheckRePrint : Form
    {
        PrintMessageBLL PMB = new PrintMessageBLL();
        List<PrintMessage> list = new List<PrintMessage>();

        public CheckRePrint()
        {
            InitializeComponent();
            this.InputToCheck.Text = "请输入制单号或IMEI号查询";
        }

        private void JST_RePrintCheck_Click(object sender, EventArgs e)
        {
            if (this.InputToCheck.Text != "请输入制单号或IMEI号查询")
            {
                this.InputToCheck.Clear();
            }
            this.dataGridView1.Rows.Clear();
            list = PMB.SelectAllReJSTBLL();
            if (list.Count == 0)
            {
                MessageBox.Show("没有重打记录");
            }
            else
            {
                foreach (PrintMessage a in list)
                {
                    this.dataGridView1.Rows.Add(a.Zhidan+"(机身)", a.IMEI, a.SN, a.JS_PrintTime, a.JS_RePrintNum, a.JS_ReFirstPrintTime, a.JS_ReEndPrintTime, a.SoftModel,a.JS_TemplatePath,"");
                }
            }
        }

        private void CHT_RePrintCheck_Click(object sender, EventArgs e)
        {
            if (this.InputToCheck.Text != "请输入制单号或IMEI号查询")
            {
                this.InputToCheck.Clear();
            }
            this.dataGridView1.Rows.Clear();
            list = PMB.SelectAllReCHTBLL();
            if (list.Count == 0)
            {
                MessageBox.Show("没有重打记录！");
            }
            else
            {
                foreach (PrintMessage a in list)
                {
                    this.dataGridView1.Rows.Add(a.Zhidan+"(彩盒)", a.IMEI, a.SN, a.CH_PrintTime, a.CH_RePrintNum, a.CH_ReFirstPrintTime, a.CH_ReEndPrintTime, a.SoftModel,a.CH_TemplatePath1,a.CH_TemplatePath2);
                }
            }
        }

        private void InputToCheck_Click(object sender, EventArgs e)
        {
            if (this.InputToCheck.Text == "请输入制单号或IMEI号查询")
            {
                this.InputToCheck.Text = string.Empty;
            }
        }

        private void CheckByInput_Click(object sender, EventArgs e)
        {
            if (this.InputToCheck.Text == "" || this.InputToCheck.Text == "请输入制单号或IMEI号查询")
            {
                MessageBox.Show("请输入制单号或IMEI号！");
                this.InputToCheck.Text = "请输入制单号或IMEI号查询";
            }
            else {
                this.dataGridView1.Rows.Clear();
                list = PMB.SelectReMesByZhiDanOrIMEIBLL(this.InputToCheck.Text);
                if (list.Count == 0)
                {
                    MessageBox.Show("没有重打记录");
                    this.InputToCheck.Clear();
                }
                else
                {
                    foreach (PrintMessage a in list)
                    {
                        if (a.JS_RePrintNum != 0)
                        {
                            this.dataGridView1.Rows.Add(a.Zhidan+"(机身)", a.IMEI, a.SN, a.JS_PrintTime, a.JS_RePrintNum, a.JS_ReFirstPrintTime, a.JS_ReEndPrintTime, a.SoftModel, a.JS_TemplatePath,"");
                        }
                        if (int.Parse(a.CH_RePrintNum) != 0) {
                            this.dataGridView1.Rows.Add(a.Zhidan + "(彩盒)", a.IMEI, a.SN, a.CH_PrintTime, a.CH_RePrintNum, a.CH_ReFirstPrintTime, a.CH_ReEndPrintTime, a.SoftModel, a.CH_TemplatePath1, a.CH_TemplatePath2);
                        }
                    }
                }
            }   
        }
        
    }
}
