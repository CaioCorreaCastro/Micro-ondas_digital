namespace Interface
{
    partial class ConfiguracaoAPI
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
            this.textBoxUrlApi = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxUsuarioApi = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxSenhaApi = new System.Windows.Forms.TextBox();
            this.botaoAutenticar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBoxUrlApi
            // 
            this.textBoxUrlApi.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.textBoxUrlApi.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxUrlApi.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxUrlApi.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.textBoxUrlApi.Location = new System.Drawing.Point(24, 41);
            this.textBoxUrlApi.Name = "textBoxUrlApi";
            this.textBoxUrlApi.Size = new System.Drawing.Size(301, 29);
            this.textBoxUrlApi.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label1.Location = new System.Drawing.Point(21, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 24);
            this.label1.TabIndex = 3;
            this.label1.Text = "Url API";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label2.Location = new System.Drawing.Point(22, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 24);
            this.label2.TabIndex = 5;
            this.label2.Text = "Usuario";
            // 
            // textBoxUsuarioApi
            // 
            this.textBoxUsuarioApi.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.textBoxUsuarioApi.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxUsuarioApi.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxUsuarioApi.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.textBoxUsuarioApi.Location = new System.Drawing.Point(25, 95);
            this.textBoxUsuarioApi.Name = "textBoxUsuarioApi";
            this.textBoxUsuarioApi.Size = new System.Drawing.Size(301, 29);
            this.textBoxUsuarioApi.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label3.Location = new System.Drawing.Point(23, 126);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 24);
            this.label3.TabIndex = 7;
            this.label3.Text = "Senha";
            // 
            // textBoxSenhaApi
            // 
            this.textBoxSenhaApi.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.textBoxSenhaApi.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxSenhaApi.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxSenhaApi.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.textBoxSenhaApi.Location = new System.Drawing.Point(26, 150);
            this.textBoxSenhaApi.Name = "textBoxSenhaApi";
            this.textBoxSenhaApi.PasswordChar = '*';
            this.textBoxSenhaApi.Size = new System.Drawing.Size(301, 29);
            this.textBoxSenhaApi.TabIndex = 6;
            // 
            // botaoAutenticar
            // 
            this.botaoAutenticar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.botaoAutenticar.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.botaoAutenticar.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.botaoAutenticar.Location = new System.Drawing.Point(96, 199);
            this.botaoAutenticar.Name = "botaoAutenticar";
            this.botaoAutenticar.Size = new System.Drawing.Size(163, 42);
            this.botaoAutenticar.TabIndex = 8;
            this.botaoAutenticar.Text = "Autenticar";
            this.botaoAutenticar.UseVisualStyleBackColor = true;
            this.botaoAutenticar.Click += new System.EventHandler(this.botaoAutenticar_Click);
            // 
            // ConfiguracaoAPI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(350, 253);
            this.Controls.Add(this.botaoAutenticar);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxSenhaApi);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxUsuarioApi);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxUrlApi);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximumSize = new System.Drawing.Size(366, 292);
            this.MinimumSize = new System.Drawing.Size(366, 292);
            this.Name = "ConfiguracaoAPI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ConfiguracaoAPI";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxUrlApi;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxUsuarioApi;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxSenhaApi;
        private System.Windows.Forms.Button botaoAutenticar;
    }
}