namespace ExcelMerge
{
    partial class FormConfiguration
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormConfiguration));
            this.cbxAction = new System.Windows.Forms.ComboBox();
            this.lblAction = new System.Windows.Forms.Label();
            this.lblDefaultDirectorySaveFiles = new System.Windows.Forms.Label();
            this.txtDefaultDirectorySaveFiles = new System.Windows.Forms.TextBox();
            this.btnBrowserFolder = new System.Windows.Forms.Button();
            this.imgList = new System.Windows.Forms.ImageList(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.headerLength = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.lblHeader = new System.Windows.Forms.Label();
            this.cbxHeader = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.headerLength)).BeginInit();
            this.SuspendLayout();
            // 
            // cbxAction
            // 
            this.cbxAction.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxAction.FormattingEnabled = true;
            this.cbxAction.Location = new System.Drawing.Point(13, 34);
            this.cbxAction.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbxAction.Name = "cbxAction";
            this.cbxAction.Size = new System.Drawing.Size(527, 28);
            this.cbxAction.TabIndex = 1;
            this.cbxAction.SelectedIndexChanged += new System.EventHandler(this.cbxAction_SelectedIndexChanged);
            // 
            // lblAction
            // 
            this.lblAction.AutoSize = true;
            this.lblAction.Location = new System.Drawing.Point(13, 9);
            this.lblAction.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAction.Name = "lblAction";
            this.lblAction.Size = new System.Drawing.Size(229, 20);
            this.lblAction.TabIndex = 0;
            this.lblAction.Text = "Ação ao fim do processamento";
            // 
            // lblDefaultDirectorySaveFiles
            // 
            this.lblDefaultDirectorySaveFiles.AutoSize = true;
            this.lblDefaultDirectorySaveFiles.Location = new System.Drawing.Point(12, 67);
            this.lblDefaultDirectorySaveFiles.Name = "lblDefaultDirectorySaveFiles";
            this.lblDefaultDirectorySaveFiles.Size = new System.Drawing.Size(270, 20);
            this.lblDefaultDirectorySaveFiles.TabIndex = 2;
            this.lblDefaultDirectorySaveFiles.Text = "Caminho padrão para salvar arquivos";
            // 
            // txtDefaultDirectorySaveFiles
            // 
            this.txtDefaultDirectorySaveFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDefaultDirectorySaveFiles.Location = new System.Drawing.Point(13, 90);
            this.txtDefaultDirectorySaveFiles.Name = "txtDefaultDirectorySaveFiles";
            this.txtDefaultDirectorySaveFiles.Size = new System.Drawing.Size(481, 26);
            this.txtDefaultDirectorySaveFiles.TabIndex = 3;
            this.txtDefaultDirectorySaveFiles.TextChanged += new System.EventHandler(this.txtDefaultDirectorySaveFiles_TextChanged);
            this.txtDefaultDirectorySaveFiles.Validating += new System.ComponentModel.CancelEventHandler(this.txtDefaultDirectorySaveFiles_Validating);
            // 
            // btnBrowserFolder
            // 
            this.btnBrowserFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowserFolder.ImageIndex = 0;
            this.btnBrowserFolder.ImageList = this.imgList;
            this.btnBrowserFolder.Location = new System.Drawing.Point(500, 76);
            this.btnBrowserFolder.Name = "btnBrowserFolder";
            this.btnBrowserFolder.Size = new System.Drawing.Size(40, 40);
            this.btnBrowserFolder.TabIndex = 4;
            this.btnBrowserFolder.UseVisualStyleBackColor = true;
            this.btnBrowserFolder.Click += new System.EventHandler(this.btnBrowserFolder_Click);
            // 
            // imgList
            // 
            this.imgList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgList.ImageStream")));
            this.imgList.TransparentColor = System.Drawing.Color.Transparent;
            this.imgList.Images.SetKeyName(0, "Folder.png");
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.headerLength);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.lblHeader);
            this.groupBox1.Controls.Add(this.cbxHeader);
            this.groupBox1.Location = new System.Drawing.Point(13, 122);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(527, 89);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = " Cabeçalho ";
            // 
            // headerLength
            // 
            this.headerLength.Location = new System.Drawing.Point(276, 49);
            this.headerLength.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.headerLength.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.headerLength.Name = "headerLength";
            this.headerLength.Size = new System.Drawing.Size(131, 26);
            this.headerLength.TabIndex = 3;
            this.headerLength.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.headerLength.ValueChanged += new System.EventHandler(this.headerLength_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(276, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(131, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Tamanho (linhas)";
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Location = new System.Drawing.Point(7, 22);
            this.lblHeader.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(129, 20);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "Como considerar";
            // 
            // cbxHeader
            // 
            this.cbxHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxHeader.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxHeader.FormattingEnabled = true;
            this.cbxHeader.Location = new System.Drawing.Point(8, 47);
            this.cbxHeader.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbxHeader.Name = "cbxHeader";
            this.cbxHeader.Size = new System.Drawing.Size(261, 28);
            this.cbxHeader.TabIndex = 1;
            this.cbxHeader.SelectedIndexChanged += new System.EventHandler(this.cbxHeader_SelectedIndexChanged);
            // 
            // FormConfiguration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(553, 227);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnBrowserFolder);
            this.Controls.Add(this.txtDefaultDirectorySaveFiles);
            this.Controls.Add(this.lblDefaultDirectorySaveFiles);
            this.Controls.Add(this.lblAction);
            this.Controls.Add(this.cbxAction);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormConfiguration";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Configurações";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormConfiguration_FormClosing);
            this.Load += new System.EventHandler(this.FormConfiguration_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.headerLength)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbxAction;
        private System.Windows.Forms.Label lblAction;
        private System.Windows.Forms.Label lblDefaultDirectorySaveFiles;
        private System.Windows.Forms.TextBox txtDefaultDirectorySaveFiles;
        private System.Windows.Forms.Button btnBrowserFolder;
        private System.Windows.Forms.ImageList imgList;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown headerLength;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.ComboBox cbxHeader;
    }
}