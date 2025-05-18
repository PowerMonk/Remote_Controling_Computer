namespace Remote_Control_Client
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnControl = new System.Windows.Forms.Button();
            this.btnMensaje = new System.Windows.Forms.Button();
            this.btnArchivo = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnControl
            // 
            this.btnControl.Location = new System.Drawing.Point(21, 12);
            this.btnControl.Name = "btnControl";
            this.btnControl.Size = new System.Drawing.Size(146, 88);
            this.btnControl.TabIndex = 0;
            this.btnControl.Text = "Controlar la computadora del servidor";
            this.btnControl.UseVisualStyleBackColor = true;
            // 
            // btnMensaje
            // 
            this.btnMensaje.Location = new System.Drawing.Point(22, 106);
            this.btnMensaje.Name = "btnMensaje";
            this.btnMensaje.Size = new System.Drawing.Size(145, 82);
            this.btnMensaje.TabIndex = 1;
            this.btnMensaje.Text = "Enviar un mensaje al servidor";
            this.btnMensaje.UseVisualStyleBackColor = true;
            // 
            // btnArchivo
            // 
            this.btnArchivo.Location = new System.Drawing.Point(21, 194);
            this.btnArchivo.Name = "btnArchivo";
            this.btnArchivo.Size = new System.Drawing.Size(146, 78);
            this.btnArchivo.TabIndex = 2;
            this.btnArchivo.Text = "Enviar un archivo al servidor";
            this.btnArchivo.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(533, 292);
            this.Controls.Add(this.btnArchivo);
            this.Controls.Add(this.btnMensaje);
            this.Controls.Add(this.btnControl);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Remote Control Client";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnControl;
        private System.Windows.Forms.Button btnMensaje;
        private System.Windows.Forms.Button btnArchivo;
    }
}

