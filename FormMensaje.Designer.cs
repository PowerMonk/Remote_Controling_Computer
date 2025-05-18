namespace Remote_Control_Client
{
    partial class FormMensaje
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnEnviarMensaje = new System.Windows.Forms.Button();
            this.txtMensaje = new System.Windows.Forms.TextBox();
            this.txtChat = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnEnviarMensaje);
            this.panel1.Controls.Add(this.txtMensaje);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 421);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 29);
            this.panel1.TabIndex = 0;
            // 
            // btnEnviarMensaje
            // 
            this.btnEnviarMensaje.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnEnviarMensaje.Location = new System.Drawing.Point(702, 0);
            this.btnEnviarMensaje.Name = "btnEnviarMensaje";
            this.btnEnviarMensaje.Size = new System.Drawing.Size(98, 29);
            this.btnEnviarMensaje.TabIndex = 1;
            this.btnEnviarMensaje.Text = "Enviar";
            this.btnEnviarMensaje.UseVisualStyleBackColor = true;
            // 
            // txtMensaje
            // 
            this.txtMensaje.Dock = System.Windows.Forms.DockStyle.Left;
            this.txtMensaje.Location = new System.Drawing.Point(0, 0);
            this.txtMensaje.Multiline = true;
            this.txtMensaje.Name = "txtMensaje";
            this.txtMensaje.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMensaje.Size = new System.Drawing.Size(703, 29);
            this.txtMensaje.TabIndex = 0;
            // 
            // txtChat
            // 
            this.txtChat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtChat.Location = new System.Drawing.Point(0, 0);
            this.txtChat.Multiline = true;
            this.txtChat.Name = "txtChat";
            this.txtChat.ReadOnly = true;
            this.txtChat.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtChat.Size = new System.Drawing.Size(800, 421);
            this.txtChat.TabIndex = 1;
            // 
            // FormMensaje
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.txtChat);
            this.Controls.Add(this.panel1);
            this.Name = "FormMensaje";
            this.Text = "Asusta al servidor con un mensaje!";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtMensaje;
        private System.Windows.Forms.Button btnEnviarMensaje;
        private System.Windows.Forms.TextBox txtChat;
    }
}