namespace WindowsForms_print
{
    partial class JST_CheckAndDelect
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.JST_DY = new System.Windows.Forms.ComboBox();
            this.JST_ID = new System.Windows.Forms.ComboBox();
            this.JST_delete = new System.Windows.Forms.Button();
            this.JST_check = new System.Windows.Forms.Button();
            this.SnOrImei = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ZhiDan = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IMEI = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SIM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VIP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BAT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IEMIRel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PrintTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SoftModel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TemplatePath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.Color.Linen;
            this.splitContainer1.Panel1.Controls.Add(this.JST_DY);
            this.splitContainer1.Panel1.Controls.Add(this.JST_ID);
            this.splitContainer1.Panel1.Controls.Add(this.JST_delete);
            this.splitContainer1.Panel1.Controls.Add(this.JST_check);
            this.splitContainer1.Panel1.Controls.Add(this.SnOrImei);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.Color.White;
            this.splitContainer1.Panel2.Controls.Add(this.dataGridView1);
            this.splitContainer1.Size = new System.Drawing.Size(1372, 705);
            this.splitContainer1.SplitterDistance = 99;
            this.splitContainer1.TabIndex = 0;
            // 
            // JST_DY
            // 
            this.JST_DY.FormattingEnabled = true;
            this.JST_DY.Location = new System.Drawing.Point(132, 59);
            this.JST_DY.Name = "JST_DY";
            this.JST_DY.Size = new System.Drawing.Size(121, 23);
            this.JST_DY.TabIndex = 5;
            // 
            // JST_ID
            // 
            this.JST_ID.FormattingEnabled = true;
            this.JST_ID.Location = new System.Drawing.Point(36, 59);
            this.JST_ID.Name = "JST_ID";
            this.JST_ID.Size = new System.Drawing.Size(90, 23);
            this.JST_ID.TabIndex = 4;
            // 
            // JST_delete
            // 
            this.JST_delete.Location = new System.Drawing.Point(259, 59);
            this.JST_delete.Name = "JST_delete";
            this.JST_delete.Size = new System.Drawing.Size(126, 25);
            this.JST_delete.TabIndex = 2;
            this.JST_delete.Text = "删除";
            this.JST_delete.UseVisualStyleBackColor = true;
            this.JST_delete.Click += new System.EventHandler(this.JST_delete_Click);
            // 
            // JST_check
            // 
            this.JST_check.Location = new System.Drawing.Point(243, 12);
            this.JST_check.Name = "JST_check";
            this.JST_check.Size = new System.Drawing.Size(126, 25);
            this.JST_check.TabIndex = 1;
            this.JST_check.Text = "查询";
            this.JST_check.UseVisualStyleBackColor = true;
            this.JST_check.Click += new System.EventHandler(this.JST_check_Click);
            // 
            // SnOrImei
            // 
            this.SnOrImei.Location = new System.Drawing.Point(36, 12);
            this.SnOrImei.Name = "SnOrImei";
            this.SnOrImei.Size = new System.Drawing.Size(204, 25);
            this.SnOrImei.TabIndex = 0;
            this.SnOrImei.Click += new System.EventHandler(this.SnOrImei_Click);
            this.SnOrImei.Leave += new System.EventHandler(this.SnOrImei_Leave);
            // 
            // dataGridView1
            // 
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.ZhiDan,
            this.IMEI,
            this.SN,
            this.SIM,
            this.VIP,
            this.BAT,
            this.IEMIRel,
            this.PrintTime,
            this.SoftModel,
            this.TemplatePath});
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle8;
            this.dataGridView1.Location = new System.Drawing.Point(3, 3);
            this.dataGridView1.Name = "dataGridView1";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.dataGridView1.RowHeadersWidth = 20;
            this.dataGridView1.RowTemplate.Height = 27;
            this.dataGridView1.Size = new System.Drawing.Size(1366, 599);
            this.dataGridView1.TabIndex = 0;
            // 
            // ID
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ID.DefaultCellStyle = dataGridViewCellStyle2;
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            this.ID.Width = 45;
            // 
            // ZhiDan
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ZhiDan.DefaultCellStyle = dataGridViewCellStyle3;
            this.ZhiDan.HeaderText = "制单";
            this.ZhiDan.Name = "ZhiDan";
            this.ZhiDan.Width = 120;
            // 
            // IMEI
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.IMEI.DefaultCellStyle = dataGridViewCellStyle4;
            this.IMEI.HeaderText = "IMEI";
            this.IMEI.Name = "IMEI";
            this.IMEI.Width = 110;
            // 
            // SN
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.SN.DefaultCellStyle = dataGridViewCellStyle5;
            this.SN.HeaderText = "SN";
            this.SN.Name = "SN";
            this.SN.Width = 110;
            // 
            // SIM
            // 
            this.SIM.HeaderText = "SIM";
            this.SIM.Name = "SIM";
            // 
            // VIP
            // 
            this.VIP.HeaderText = "VIP";
            this.VIP.Name = "VIP";
            // 
            // BAT
            // 
            this.BAT.HeaderText = "BAT";
            this.BAT.Name = "BAT";
            // 
            // IEMIRel
            // 
            this.IEMIRel.HeaderText = "绑定关系";
            this.IEMIRel.Name = "IEMIRel";
            // 
            // PrintTime
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.PrintTime.DefaultCellStyle = dataGridViewCellStyle6;
            this.PrintTime.HeaderText = "打印时间";
            this.PrintTime.Name = "PrintTime";
            this.PrintTime.Width = 110;
            // 
            // SoftModel
            // 
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.SoftModel.DefaultCellStyle = dataGridViewCellStyle7;
            this.SoftModel.HeaderText = "软件型号";
            this.SoftModel.Name = "SoftModel";
            this.SoftModel.Width = 80;
            // 
            // TemplatePath
            // 
            this.TemplatePath.HeaderText = "模板路径";
            this.TemplatePath.Name = "TemplatePath";
            this.TemplatePath.Width = 160;
            // 
            // JST_CheckAndDelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1372, 705);
            this.Controls.Add(this.splitContainer1);
            this.Name = "JST_CheckAndDelect";
            this.Text = "机身贴查询与删除";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button JST_delete;
        private System.Windows.Forms.Button JST_check;
        private System.Windows.Forms.TextBox SnOrImei;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ZhiDan;
        private System.Windows.Forms.DataGridViewTextBoxColumn IMEI;
        private System.Windows.Forms.DataGridViewTextBoxColumn SN;
        private System.Windows.Forms.DataGridViewTextBoxColumn SIM;
        private System.Windows.Forms.DataGridViewTextBoxColumn VIP;
        private System.Windows.Forms.DataGridViewTextBoxColumn BAT;
        private System.Windows.Forms.DataGridViewTextBoxColumn IEMIRel;
        private System.Windows.Forms.DataGridViewTextBoxColumn PrintTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn SoftModel;
        private System.Windows.Forms.DataGridViewTextBoxColumn TemplatePath;
        private System.Windows.Forms.ComboBox JST_ID;
        private System.Windows.Forms.ComboBox JST_DY;
    }
}