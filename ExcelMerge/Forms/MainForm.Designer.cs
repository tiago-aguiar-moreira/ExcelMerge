namespace ExcelMerge
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnConfigs = new System.Windows.Forms.Button();
            this.btnDeleteAll = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnRun = new System.Windows.Forms.Button();
            this.pnlFiles = new System.Windows.Forms.Panel();
            this.gridVwFiles = new System.Windows.Forms.DataGridView();
            this.pnlSettings = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblSeparador = new System.Windows.Forms.Label();
            this.txtSeparatorCSV = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtHeaderLength = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.lblHeader = new System.Windows.Forms.Label();
            this.cbxHeader = new System.Windows.Forms.ComboBox();
            this.btnBrowserFolder = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.txtDefaultDirectorySaveFiles = new System.Windows.Forms.TextBox();
            this.lblDefaultDirectorySaveFiles = new System.Windows.Forms.Label();
            this.lblAction = new System.Windows.Forms.Label();
            this.cbxAction = new System.Windows.Forms.ComboBox();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.pnlFiles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridVwFiles)).BeginInit();
            this.pnlSettings.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtHeaderLength)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnConfigs);
            this.panel1.Controls.Add(this.btnDeleteAll);
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Controls.Add(this.btnDelete);
            this.panel1.Controls.Add(this.btnRun);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(949, 56);
            this.panel1.TabIndex = 0;
            // 
            // btnConfigs
            // 
            this.btnConfigs.Location = new System.Drawing.Point(575, 5);
            this.btnConfigs.Name = "btnConfigs";
            this.btnConfigs.Size = new System.Drawing.Size(135, 46);
            this.btnConfigs.TabIndex = 4;
            this.btnConfigs.Tag = "1";
            this.btnConfigs.Text = "Configurações";
            this.btnConfigs.UseVisualStyleBackColor = true;
            this.btnConfigs.Click += new System.EventHandler(this.btnConfig_Click);
            // 
            // btnDeleteAll
            // 
            this.btnDeleteAll.Enabled = false;
            this.btnDeleteAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteAll.Location = new System.Drawing.Point(290, 5);
            this.btnDeleteAll.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnDeleteAll.Name = "btnDeleteAll";
            this.btnDeleteAll.Size = new System.Drawing.Size(135, 46);
            this.btnDeleteAll.TabIndex = 2;
            this.btnDeleteAll.Text = "Remover Todos";
            this.btnDeleteAll.UseVisualStyleBackColor = true;
            this.btnDeleteAll.Click += new System.EventHandler(this.btnDeleteAll_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.Location = new System.Drawing.Point(4, 5);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(135, 46);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "Adicionar";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Enabled = false;
            this.btnDelete.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.Location = new System.Drawing.Point(147, 5);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(135, 46);
            this.btnDelete.TabIndex = 1;
            this.btnDelete.Text = "Remover";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnRun
            // 
            this.btnRun.Enabled = false;
            this.btnRun.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRun.Location = new System.Drawing.Point(433, 5);
            this.btnRun.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(135, 46);
            this.btnRun.TabIndex = 3;
            this.btnRun.Text = "Executar";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // pnlFiles
            // 
            this.pnlFiles.Controls.Add(this.gridVwFiles);
            this.pnlFiles.Controls.Add(this.panel1);
            this.pnlFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFiles.Location = new System.Drawing.Point(0, 157);
            this.pnlFiles.Name = "pnlFiles";
            this.pnlFiles.Size = new System.Drawing.Size(949, 422);
            this.pnlFiles.TabIndex = 4;
            // 
            // gridVwFiles
            // 
            this.gridVwFiles.AllowUserToAddRows = false;
            this.gridVwFiles.AllowUserToDeleteRows = false;
            this.gridVwFiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridVwFiles.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4});
            this.gridVwFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridVwFiles.Location = new System.Drawing.Point(0, 56);
            this.gridVwFiles.Name = "gridVwFiles";
            this.gridVwFiles.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridVwFiles.Size = new System.Drawing.Size(949, 366);
            this.gridVwFiles.TabIndex = 1;
            // 
            // pnlSettings
            // 
            this.pnlSettings.BackColor = System.Drawing.SystemColors.Control;
            this.pnlSettings.Controls.Add(this.groupBox2);
            this.pnlSettings.Controls.Add(this.groupBox1);
            this.pnlSettings.Controls.Add(this.btnBrowserFolder);
            this.pnlSettings.Controls.Add(this.txtDefaultDirectorySaveFiles);
            this.pnlSettings.Controls.Add(this.lblDefaultDirectorySaveFiles);
            this.pnlSettings.Controls.Add(this.lblAction);
            this.pnlSettings.Controls.Add(this.cbxAction);
            this.pnlSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSettings.Location = new System.Drawing.Point(0, 0);
            this.pnlSettings.Name = "pnlSettings";
            this.pnlSettings.Size = new System.Drawing.Size(949, 157);
            this.pnlSettings.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblSeparador);
            this.groupBox2.Controls.Add(this.txtSeparatorCSV);
            this.groupBox2.Location = new System.Drawing.Point(425, 64);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(124, 89);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = " CSV ";
            // 
            // lblSeparador
            // 
            this.lblSeparador.AutoSize = true;
            this.lblSeparador.Location = new System.Drawing.Point(6, 22);
            this.lblSeparador.Name = "lblSeparador";
            this.lblSeparador.Size = new System.Drawing.Size(84, 20);
            this.lblSeparador.TabIndex = 0;
            this.lblSeparador.Text = "Separador";
            // 
            // txtSeparatorCSV
            // 
            this.txtSeparatorCSV.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSeparatorCSV.Location = new System.Drawing.Point(6, 48);
            this.txtSeparatorCSV.Name = "txtSeparatorCSV";
            this.txtSeparatorCSV.Size = new System.Drawing.Size(112, 26);
            this.txtSeparatorCSV.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtHeaderLength);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.lblHeader);
            this.groupBox1.Controls.Add(this.cbxHeader);
            this.groupBox1.Location = new System.Drawing.Point(4, 64);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(415, 89);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = " Cabeçalho ";
            // 
            // txtHeaderLength
            // 
            this.txtHeaderLength.Location = new System.Drawing.Point(276, 49);
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
            this.txtHeaderLength.Size = new System.Drawing.Size(131, 26);
            this.txtHeaderLength.TabIndex = 3;
            this.txtHeaderLength.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtHeaderLength.ValueChanged += new System.EventHandler(this.HeaderLength_ValueChanged);
            this.txtHeaderLength.Validating += new System.ComponentModel.CancelEventHandler(this.SaveApp_Validating);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(276, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Linha Inicial";
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Location = new System.Drawing.Point(7, 22);
            this.lblHeader.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(86, 20);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "Considerar";
            // 
            // cbxHeader
            // 
            this.cbxHeader.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxHeader.FormattingEnabled = true;
            this.cbxHeader.Location = new System.Drawing.Point(8, 47);
            this.cbxHeader.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbxHeader.Name = "cbxHeader";
            this.cbxHeader.Size = new System.Drawing.Size(261, 28);
            this.cbxHeader.TabIndex = 1;
            this.cbxHeader.SelectedIndexChanged += new System.EventHandler(this.CbxHeader_SelectedIndexChanged);
            this.cbxHeader.Validating += new System.ComponentModel.CancelEventHandler(this.SaveApp_Validating);
            // 
            // btnBrowserFolder
            // 
            this.btnBrowserFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowserFolder.ImageIndex = 0;
            this.btnBrowserFolder.ImageList = this.imageList1;
            this.btnBrowserFolder.Location = new System.Drawing.Point(899, 18);
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
            // txtDefaultDirectorySaveFiles
            // 
            this.txtDefaultDirectorySaveFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDefaultDirectorySaveFiles.Location = new System.Drawing.Point(4, 32);
            this.txtDefaultDirectorySaveFiles.Name = "txtDefaultDirectorySaveFiles";
            this.txtDefaultDirectorySaveFiles.Size = new System.Drawing.Size(889, 26);
            this.txtDefaultDirectorySaveFiles.TabIndex = 1;
            this.txtDefaultDirectorySaveFiles.TextChanged += new System.EventHandler(this.TxtDefaultDirectorySaveFiles_TextChanged);
            this.txtDefaultDirectorySaveFiles.Validating += new System.ComponentModel.CancelEventHandler(this.TxtDefaultDirectorySaveFiles_Validating);
            // 
            // lblDefaultDirectorySaveFiles
            // 
            this.lblDefaultDirectorySaveFiles.AutoSize = true;
            this.lblDefaultDirectorySaveFiles.Location = new System.Drawing.Point(3, 9);
            this.lblDefaultDirectorySaveFiles.Name = "lblDefaultDirectorySaveFiles";
            this.lblDefaultDirectorySaveFiles.Size = new System.Drawing.Size(270, 20);
            this.lblDefaultDirectorySaveFiles.TabIndex = 0;
            this.lblDefaultDirectorySaveFiles.Text = "Caminho padrão para salvar arquivos";
            // 
            // lblAction
            // 
            this.lblAction.AutoSize = true;
            this.lblAction.Location = new System.Drawing.Point(556, 86);
            this.lblAction.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAction.Name = "lblAction";
            this.lblAction.Size = new System.Drawing.Size(190, 20);
            this.lblAction.TabIndex = 5;
            this.lblAction.Text = "Ao fim do processamento";
            // 
            // cbxAction
            // 
            this.cbxAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxAction.FormattingEnabled = true;
            this.cbxAction.Location = new System.Drawing.Point(556, 111);
            this.cbxAction.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbxAction.Name = "cbxAction";
            this.cbxAction.Size = new System.Drawing.Size(294, 28);
            this.cbxAction.TabIndex = 6;
            this.cbxAction.SelectedIndexChanged += new System.EventHandler(this.cbxAction_SelectedIndexChanged);
            this.cbxAction.Validating += new System.ComponentModel.CancelEventHandler(this.SaveApp_Validating);
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "HeaderLength";
            this.Column1.HeaderText = "Linha Inicial do Cabeçalho";
            this.Column1.Name = "Column1";
            this.Column1.Width = 130;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "SeparatorCSV";
            this.Column2.HeaderText = "Separador CSV";
            this.Column2.Name = "Column2";
            this.Column2.Width = 150;
            // 
            // Column3
            // 
            this.Column3.DataPropertyName = "FileName";
            this.Column3.HeaderText = "Arquivo";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 250;
            // 
            // Column4
            // 
            this.Column4.DataPropertyName = "Directory";
            this.Column4.HeaderText = "Diretório";
            this.Column4.Name = "Column4";
            this.Column4.Width = 580;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(949, 579);
            this.Controls.Add(this.pnlFiles);
            this.Controls.Add(this.pnlSettings);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(670, 470);
            this.Name = "MainForm";
            this.Text = "Excel Merge";
            this.panel1.ResumeLayout(false);
            this.pnlFiles.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridVwFiles)).EndInit();
            this.pnlSettings.ResumeLayout(false);
            this.pnlSettings.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtHeaderLength)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnDeleteAll;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Panel pnlFiles;
        private System.Windows.Forms.Panel pnlSettings;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown txtHeaderLength;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.ComboBox cbxHeader;
        private System.Windows.Forms.Button btnBrowserFolder;
        private System.Windows.Forms.TextBox txtDefaultDirectorySaveFiles;
        private System.Windows.Forms.Label lblDefaultDirectorySaveFiles;
        private System.Windows.Forms.Label lblAction;
        private System.Windows.Forms.ComboBox cbxAction;
        private System.Windows.Forms.DataGridView gridVwFiles;
        private System.Windows.Forms.TextBox txtSeparatorCSV;
        private System.Windows.Forms.Label lblSeparador;
        private System.Windows.Forms.Button btnConfigs;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
    }
}

