namespace WindowsForms_print
{
    partial class CheckRePrint
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.CheckByInput = new System.Windows.Forms.Button();
            this.InputToCheck = new System.Windows.Forms.TextBox();
            this.CHT_RePrintCheck = new System.Windows.Forms.Button();
            this.JST_RePrintCheck = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ZhiDan = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IMEI = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FirstPrintTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RePrintNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RePrintTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RePrintEndTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SoftModel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TemPath1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TemPath2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.Color.Linen;
            this.splitContainer1.Panel1.Controls.Add(this.CheckByInput);
            this.splitContainer1.Panel1.Controls.Add(this.InputToCheck);
            this.splitContainer1.Panel1.Controls.Add(this.CHT_RePrintCheck);
            this.splitContainer1.Panel1.Controls.Add(this.JST_RePrintCheck);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.Color.White;
            this.splitContainer1.Panel2.Controls.Add(this.dataGridView1);
            this.splitContainer1.Size = new System.Drawing.Size(1500, 692);
            this.splitContainer1.SplitterDistance = 83;
            this.splitContainer1.TabIndex = 0;
            // 
            // CheckByInput
            // 
            this.CheckByInput.Location = new System.Drawing.Point(647, 22);
            this.CheckByInput.Margin = new System.Windows.Forms.Padding(4);
            this.CheckByInput.Name = "CheckByInput";
            this.CheckByInput.Size = new System.Drawing.Size(139, 29);
            this.CheckByInput.TabIndex = 3;
            this.CheckByInput.Text = "查询";
            this.CheckByInput.UseVisualStyleBackColor = true;
            this.CheckByInput.Click += new System.EventHandler(this.CheckByInput_Click);
            // 
            // InputToCheck
            // 
            this.InputToCheck.ForeColor = System.Drawing.Color.Black;
            this.InputToCheck.Location = new System.Drawing.Point(399, 22);
            this.InputToCheck.Margin = new System.Windows.Forms.Padding(4);
            this.InputToCheck.Name = "InputToCheck";
            this.InputToCheck.Size = new System.Drawing.Size(228, 25);
            this.InputToCheck.TabIndex = 2;
            this.InputToCheck.Click += new System.EventHandler(this.InputToCheck_Click);
            // 
            // CHT_RePrintCheck
            // 
            this.CHT_RePrintCheck.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.CHT_RePrintCheck.Location = new System.Drawing.Point(209, 16);
            this.CHT_RePrintCheck.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.CHT_RePrintCheck.Name = "CHT_RePrintCheck";
            this.CHT_RePrintCheck.Size = new System.Drawing.Size(139, 32);
            this.CHT_RePrintCheck.TabIndex = 1;
            this.CHT_RePrintCheck.Text = "彩盒贴重打查询";
            this.CHT_RePrintCheck.UseVisualStyleBackColor = true;
            this.CHT_RePrintCheck.Click += new System.EventHandler(this.CHT_RePrintCheck_Click);
            // 
            // JST_RePrintCheck
            // 
            this.JST_RePrintCheck.FlatAppearance.BorderColor = System.Drawing.Color.Maroon;
            this.JST_RePrintCheck.Location = new System.Drawing.Point(36, 16);
            this.JST_RePrintCheck.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.JST_RePrintCheck.Name = "JST_RePrintCheck";
            this.JST_RePrintCheck.Size = new System.Drawing.Size(139, 32);
            this.JST_RePrintCheck.TabIndex = 0;
            this.JST_RePrintCheck.Text = "机身贴重打查询";
            this.JST_RePrintCheck.UseVisualStyleBackColor = true;
            this.JST_RePrintCheck.Click += new System.EventHandler(this.JST_RePrintCheck_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeight = 30;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ZhiDan,
            this.IMEI,
            this.SN,
            this.FirstPrintTime,
            this.RePrintNum,
            this.RePrintTime,
            this.RePrintEndTime,
            this.SoftModel,
            this.TemPath1,
            this.TemPath2});
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle9;
            this.dataGridView1.GridColor = System.Drawing.SystemColors.ActiveBorder;
            this.dataGridView1.Location = new System.Drawing.Point(0, 2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle10;
            this.dataGridView1.RowHeadersWidth = 20;
            this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridView1.RowTemplate.Height = 30;
            this.dataGridView1.Size = new System.Drawing.Size(1497, 601);
            this.dataGridView1.TabIndex = 0;
            // 
            // ZhiDan
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ZhiDan.DefaultCellStyle = dataGridViewCellStyle2;
            this.ZhiDan.HeaderText = "制单号";
            this.ZhiDan.MinimumWidth = 20;
            this.ZhiDan.Name = "ZhiDan";
            this.ZhiDan.Width = 144;
            // 
            // IMEI
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.IMEI.DefaultCellStyle = dataGridViewCellStyle3;
            this.IMEI.HeaderText = "IMEI";
            this.IMEI.MinimumWidth = 20;
            this.IMEI.Name = "IMEI";
            this.IMEI.Width = 119;
            // 
            // SN
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.SN.DefaultCellStyle = dataGridViewCellStyle4;
            this.SN.HeaderText = "SN";
            this.SN.MinimumWidth = 20;
            this.SN.Name = "SN";
            this.SN.Width = 119;
            // 
            // FirstPrintTime
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.FirstPrintTime.DefaultCellStyle = dataGridViewCellStyle5;
            this.FirstPrintTime.HeaderText = "首次打印时间";
            this.FirstPrintTime.MinimumWidth = 20;
            this.FirstPrintTime.Name = "FirstPrintTime";
            this.FirstPrintTime.Width = 119;
            // 
            // RePrintNum
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.RePrintNum.DefaultCellStyle = dataGridViewCellStyle6;
            this.RePrintNum.HeaderText = "重打次数";
            this.RePrintNum.MinimumWidth = 20;
            this.RePrintNum.Name = "RePrintNum";
            this.RePrintNum.Width = 94;
            // 
            // RePrintTime
            // 
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.RePrintTime.DefaultCellStyle = dataGridViewCellStyle7;
            this.RePrintTime.HeaderText = "首次重打时间";
            this.RePrintTime.MinimumWidth = 20;
            this.RePrintTime.Name = "RePrintTime";
            this.RePrintTime.Width = 119;
            // 
            // RePrintEndTime
            // 
            this.RePrintEndTime.HeaderText = "最后重打时间";
            this.RePrintEndTime.Name = "RePrintEndTime";
            this.RePrintEndTime.Width = 119;
            // 
            // SoftModel
            // 
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.SoftModel.DefaultCellStyle = dataGridViewCellStyle8;
            this.SoftModel.HeaderText = "软件版本";
            this.SoftModel.MinimumWidth = 20;
            this.SoftModel.Name = "SoftModel";
            this.SoftModel.Width = 119;
            // 
            // TemPath1
            // 
            this.TemPath1.HeaderText = "模板路径1";
            this.TemPath1.Name = "TemPath1";
            this.TemPath1.Width = 144;
            // 
            // TemPath2
            // 
            this.TemPath2.HeaderText = "模板路径2";
            this.TemPath2.Name = "TemPath2";
            this.TemPath2.Width = 144;
            // 
            // CheckRePrint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1500, 692);
            this.Controls.Add(this.splitContainer1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "CheckRePrint";
            this.Text = "重打查询";
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
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button CHT_RePrintCheck;
        private System.Windows.Forms.Button JST_RePrintCheck;
        private System.Windows.Forms.Button CheckByInput;
        private System.Windows.Forms.TextBox InputToCheck;
        private System.Windows.Forms.DataGridViewTextBoxColumn ZhiDan;
        private System.Windows.Forms.DataGridViewTextBoxColumn IMEI;
        private System.Windows.Forms.DataGridViewTextBoxColumn SN;
        private System.Windows.Forms.DataGridViewTextBoxColumn FirstPrintTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn RePrintNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn RePrintTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn RePrintEndTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn SoftModel;
        private System.Windows.Forms.DataGridViewTextBoxColumn TemPath1;
        private System.Windows.Forms.DataGridViewTextBoxColumn TemPath2;
    }
}