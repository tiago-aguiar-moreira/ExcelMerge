namespace ExcelTools.Forms
{
    partial class FormConfig
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormConfig));
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtHeaderLength = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.lblHeader = new System.Windows.Forms.Label();
            this.cbxHeader = new System.Windows.Forms.ComboBox();
            this.btnBrowserFolder = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.lblSeparador = new System.Windows.Forms.Label();
            this.txtSeparatorCSV = new System.Windows.Forms.TextBox();
            this.txtDefaultDirectorySaveFiles = new System.Windows.Forms.TextBox();
            this.lblDefaultDirectorySaveFiles = new System.Windows.Forms.Label();
            this.lblAction = new System.Windows.Forms.Label();
            this.cbxAction = new System.Windows.Forms.ComboBox();
            this.gbxGeneral = new System.Windows.Forms.GroupBox();
            this.lblLanguage = new System.Windows.Forms.Label();
            this.cbxLanguage = new System.Windows.Forms.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnSalvar = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtHeaderLength)).BeginInit();
            this.gbxGeneral.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.gbxGeneral);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(738, 393);
            this.panel1.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.txtHeaderLength);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.lblHeader);
            this.groupBox2.Controls.Add(this.cbxHeader);
            this.groupBox2.Controls.Add(this.btnBrowserFolder);
            this.groupBox2.Controls.Add(this.lblSeparador);
            this.groupBox2.Controls.Add(this.txtSeparatorCSV);
            this.groupBox2.Controls.Add(this.txtDefaultDirectorySaveFiles);
            this.groupBox2.Controls.Add(this.lblDefaultDirectorySaveFiles);
            this.groupBox2.Controls.Add(this.lblAction);
            this.groupBox2.Controls.Add(this.cbxAction);
            this.groupBox2.Location = new System.Drawing.Point(12, 114);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(714, 209);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = " Arquivo ";
            // 
            // txtHeaderLength
            // 
            this.txtHeaderLength.Location = new System.Drawing.Point(10, 168);
            this.txtHeaderLength.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.txtHeaderLength.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtHeaderLength.Name = "txtHeaderLength";
            this.txtHeaderLength.Size = new System.Drawing.Size(254, 26);
            this.txtHeaderLength.TabIndex = 10;
            this.txtHeaderLength.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtHeaderLength.ValueChanged += new System.EventHandler(this.HeaderLength_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 145);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(189, 20);
            this.label1.TabIndex = 9;
            this.label1.Text = "Linha inicial do cabeçalho";
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Location = new System.Drawing.Point(271, 87);
            this.lblHeader.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(219, 20);
            this.lblHeader.TabIndex = 5;
            this.lblHeader.Text = "Como considerar o cabeçalho";
            // 
            // cbxHeader
            // 
            this.cbxHeader.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxHeader.FormattingEnabled = true;
            this.cbxHeader.Location = new System.Drawing.Point(272, 112);
            this.cbxHeader.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbxHeader.Name = "cbxHeader";
            this.cbxHeader.Size = new System.Drawing.Size(261, 28);
            this.cbxHeader.TabIndex = 6;
            this.cbxHeader.SelectedIndexChanged += new System.EventHandler(this.CbxHeader_SelectedIndexChanged);
            // 
            // btnBrowserFolder
            // 
            this.btnBrowserFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowserFolder.ImageIndex = 0;
            this.btnBrowserFolder.ImageList = this.imageList1;
            this.btnBrowserFolder.Location = new System.Drawing.Point(668, 44);
            this.btnBrowserFolder.Name = "btnBrowserFolder";
            this.btnBrowserFolder.Size = new System.Drawing.Size(40, 40);
            this.btnBrowserFolder.TabIndex = 2;
            this.btnBrowserFolder.UseVisualStyleBackColor = true;
            this.btnBrowserFolder.Click += new System.EventHandler(this.BtnBrowserFolder_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Folder.png");
            this.imageList1.Images.SetKeyName(1, "DoubleArrowDown.png");
            this.imageList1.Images.SetKeyName(2, "DoubleArrowUp.png");
            // 
            // lblSeparador
            // 
            this.lblSeparador.AutoSize = true;
            this.lblSeparador.Location = new System.Drawing.Point(540, 89);
            this.lblSeparador.Name = "lblSeparador";
            this.lblSeparador.Size = new System.Drawing.Size(121, 20);
            this.lblSeparador.TabIndex = 7;
            this.lblSeparador.Text = "Separador CSV";
            // 
            // txtSeparatorCSV
            // 
            this.txtSeparatorCSV.Location = new System.Drawing.Point(540, 112);
            this.txtSeparatorCSV.MaxLength = 1;
            this.txtSeparatorCSV.Name = "txtSeparatorCSV";
            this.txtSeparatorCSV.Size = new System.Drawing.Size(167, 26);
            this.txtSeparatorCSV.TabIndex = 8;
            this.txtSeparatorCSV.TextChanged += new System.EventHandler(this.TxtSeparatorCSV_TextChanged);
            // 
            // txtDefaultDirectorySaveFiles
            // 
            this.txtDefaultDirectorySaveFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDefaultDirectorySaveFiles.Location = new System.Drawing.Point(11, 58);
            this.txtDefaultDirectorySaveFiles.Name = "txtDefaultDirectorySaveFiles";
            this.txtDefaultDirectorySaveFiles.Size = new System.Drawing.Size(651, 26);
            this.txtDefaultDirectorySaveFiles.TabIndex = 1;
            this.txtDefaultDirectorySaveFiles.TextChanged += new System.EventHandler(this.TxtDefaultDirectorySaveFiles_TextChanged);
            this.txtDefaultDirectorySaveFiles.Validating += new System.ComponentModel.CancelEventHandler(this.TxtDefaultDirectorySaveFiles_Validating);
            // 
            // lblDefaultDirectorySaveFiles
            // 
            this.lblDefaultDirectorySaveFiles.AutoSize = true;
            this.lblDefaultDirectorySaveFiles.Location = new System.Drawing.Point(10, 35);
            this.lblDefaultDirectorySaveFiles.Name = "lblDefaultDirectorySaveFiles";
            this.lblDefaultDirectorySaveFiles.Size = new System.Drawing.Size(270, 20);
            this.lblDefaultDirectorySaveFiles.TabIndex = 0;
            this.lblDefaultDirectorySaveFiles.Text = "Caminho padrão para salvar arquivos";
            // 
            // lblAction
            // 
            this.lblAction.AutoSize = true;
            this.lblAction.Location = new System.Drawing.Point(10, 87);
            this.lblAction.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAction.Name = "lblAction";
            this.lblAction.Size = new System.Drawing.Size(190, 20);
            this.lblAction.TabIndex = 3;
            this.lblAction.Text = "Ao fim do processamento";
            // 
            // cbxAction
            // 
            this.cbxAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxAction.FormattingEnabled = true;
            this.cbxAction.Location = new System.Drawing.Point(11, 112);
            this.cbxAction.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbxAction.Name = "cbxAction";
            this.cbxAction.Size = new System.Drawing.Size(253, 28);
            this.cbxAction.TabIndex = 4;
            this.cbxAction.SelectedIndexChanged += new System.EventHandler(this.cbxAction_SelectedIndexChanged);
            // 
            // gbxGeneral
            // 
            this.gbxGeneral.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbxGeneral.Controls.Add(this.lblLanguage);
            this.gbxGeneral.Controls.Add(this.cbxLanguage);
            this.gbxGeneral.Location = new System.Drawing.Point(12, 12);
            this.gbxGeneral.Name = "gbxGeneral";
            this.gbxGeneral.Size = new System.Drawing.Size(714, 96);
            this.gbxGeneral.TabIndex = 0;
            this.gbxGeneral.TabStop = false;
            this.gbxGeneral.Text = " Geral ";
            // 
            // lblLanguage
            // 
            this.lblLanguage.AutoSize = true;
            this.lblLanguage.Location = new System.Drawing.Point(10, 34);
            this.lblLanguage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLanguage.Name = "lblLanguage";
            this.lblLanguage.Size = new System.Drawing.Size(57, 20);
            this.lblLanguage.TabIndex = 0;
            this.lblLanguage.Text = "Idioma";
            // 
            // cbxLanguage
            // 
            this.cbxLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxLanguage.FormattingEnabled = true;
            this.cbxLanguage.Location = new System.Drawing.Point(11, 56);
            this.cbxLanguage.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbxLanguage.Name = "cbxLanguage";
            this.cbxLanguage.Size = new System.Drawing.Size(257, 28);
            this.cbxLanguage.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnSalvar);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 334);
            this.panel2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(738, 59);
            this.panel2.TabIndex = 2;
            // 
            // btnSalvar
            // 
            this.btnSalvar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSalvar.Location = new System.Drawing.Point(605, 15);
            this.btnSalvar.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnSalvar.Name = "btnSalvar";
            this.btnSalvar.Size = new System.Drawing.Size(120, 30);
            this.btnSalvar.TabIndex = 0;
            this.btnSalvar.Text = "Salvar";
            this.btnSalvar.UseVisualStyleBackColor = true;
            this.btnSalvar.Click += new System.EventHandler(this.BtnSalvar_Click);
            // 
            // FormConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(738, 393);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormConfig";
            this.Text = "Configurações";
            this.panel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtHeaderLength)).EndInit();
            this.gbxGeneral.ResumeLayout(false);
            this.gbxGeneral.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnSalvar;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown txtHeaderLength;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.ComboBox cbxHeader;
        private System.Windows.Forms.Button btnBrowserFolder;
        private System.Windows.Forms.Label lblSeparador;
        private System.Windows.Forms.TextBox txtSeparatorCSV;
        private System.Windows.Forms.TextBox txtDefaultDirectorySaveFiles;
        private System.Windows.Forms.Label lblDefaultDirectorySaveFiles;
        private System.Windows.Forms.Label lblAction;
        private System.Windows.Forms.ComboBox cbxAction;
        private System.Windows.Forms.GroupBox gbxGeneral;
        private System.Windows.Forms.Label lblLanguage;
        private System.Windows.Forms.ComboBox cbxLanguage;
    }
}