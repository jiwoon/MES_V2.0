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

namespace WindowsForms_print
{
    public partial class PrintFromExcel : Form
    {
        InputExcelBLL IEB = new InputExcelBLL();
        public PrintFromExcel()
        {
            InitializeComponent();
        }

        private void Open_Template_Click(object sender, EventArgs e)
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
                    this.Select_Template.Text = path;
                }
            }
        }

        private void Import_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.ShowDialog();
            string path = dialog.FileName;
            string strExtension = path.Substring(path.LastIndexOf('.'));
            if (strExtension != ".xls" && strExtension != ".xlsx")
            {
                MessageBox.Show("请选择xls文件！");
            }
            else
            {
                this.ImportPath.Text = path;
                //DataTable dt = IEB.GetExcelDatatable(path).Tables[0];
                //this.dataGridView1.DataSource = dt;
                //this.dataGridView1.Rows.Add(dt);
                //DataRow dr2 = dt.Rows[1];
                //MessageBox.Show(dt.Rows[0].ToString());
            }
        }

        private void RowNumber_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = IEB.GetExcelDatatable(this.ImportPath.Text).Tables[0];
            DataRow dr2 = dt.Rows[int.Parse(this.RowNumber.Text)-2];
            this.dataGridView1.Rows.Add(dr2[0], dr2[1], dr2[2], dr2[3]);
        }

    }
}
