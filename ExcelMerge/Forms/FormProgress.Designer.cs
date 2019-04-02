namespace ExcelMerge.Forms
{
    partial class FormProgress
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
            this.progBarFile = new System.Windows.Forms.ProgressBar();
            this.progBarSheet = new System.Windows.Forms.ProgressBar();
            this.progBarRow = new System.Windows.Forms.ProgressBar();
            this.lblFile = new System.Windows.Forms.Label();
            this.lblSheet = new System.Windows.Forms.Label();
            this.lblRow = new System.Windows.Forms.Label();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.backWorker = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // progBarFile
            // 
            this.progBarFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progBarFile.Location = new System.Drawing.Point(13, 34);
            this.progBarFile.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.progBarFile.Name = "progBarFile";
            this.progBarFile.Size = new System.Drawing.Size(495, 35);
            this.progBarFile.TabIndex = 1;
            // 
            // progBarSheet
            // 
            this.progBarSheet.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progBarSheet.Location = new System.Drawing.Point(13, 99);
            this.progBarSheet.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.progBarSheet.Name = "progBarSheet";
            this.progBarSheet.Size = new System.Drawing.Size(495, 35);
            this.progBarSheet.TabIndex = 3;
            // 
            // progBarRow
            // 
            this.progBarRow.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progBarRow.Location = new System.Drawing.Point(13, 164);
            this.progBarRow.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.progBarRow.Name = "progBarRow";
            this.progBarRow.Size = new System.Drawing.Size(495, 35);
            this.progBarRow.TabIndex = 5;
            // 
            // lblFile
            // 
            this.lblFile.AutoSize = true;
            this.lblFile.Location = new System.Drawing.Point(13, 9);
            this.lblFile.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFile.Name = "lblFile";
            this.lblFile.Size = new System.Drawing.Size(70, 20);
            this.lblFile.TabIndex = 0;
            this.lblFile.Text = "Arquivos";
            // 
            // lblSheet
            // 
            this.lblSheet.AutoSize = true;
            this.lblSheet.Location = new System.Drawing.Point(13, 74);
            this.lblSheet.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSheet.Name = "lblSheet";
            this.lblSheet.Size = new System.Drawing.Size(72, 20);
            this.lblSheet.TabIndex = 2;
            this.lblSheet.Text = "Planilhas";
            // 
            // lblRow
            // 
            this.lblRow.AutoSize = true;
            this.lblRow.Location = new System.Drawing.Point(13, 139);
            this.lblRow.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRow.Name = "lblRow";
            this.lblRow.Size = new System.Drawing.Size(56, 20);
            this.lblRow.TabIndex = 4;
            this.lblRow.Text = "Linhas";
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(192, 207);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(137, 45);
            this.btnCancelar.TabIndex = 6;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // backWorker
            // 
            this.backWorker.WorkerReportsProgress = true;
            this.backWorker.WorkerSupportsCancellation = true;
            this.backWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backWorker_DoWork);
            this.backWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backWorker_RunWorkerCompleted);
            // 
            // FormProgress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(521, 262);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.lblRow);
            this.Controls.Add(this.lblSheet);
            this.Controls.Add(this.lblFile);
            this.Controls.Add(this.progBarRow);
            this.Controls.Add(this.progBarSheet);
            this.Controls.Add(this.progBarFile);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "FormProgress";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Excel Merge";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormProgress_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progBarFile;
        private System.Windows.Forms.ProgressBar progBarSheet;
        private System.Windows.Forms.ProgressBar progBarRow;
        private System.Windows.Forms.Label lblFile;
        private System.Windows.Forms.Label lblSheet;
        private System.Windows.Forms.Label lblRow;
        private System.Windows.Forms.Button btnCancelar;
        private System.ComponentModel.BackgroundWorker backWorker;
    }
}