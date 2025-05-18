using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Remote_Control_Client
{
    public partial class FormMensaje : Form
    {
        TcpClient client;
        NetworkStream stream;
        public FormMensaje(TcpClient client, NetworkStream stream)
        {
            InitializeComponent();
            this.client = client;
            this.stream = stream;

            btnEnviarMensaje.Click += btnMensaje_Click;
        }

        private void btnMensaje_Click(object sender, EventArgs e)
        {
            EnviarMensajeAlServidor();

        }

        private void EnviarMensajeAlServidor()
        {
            if(stream != null && client.Connected){
                string mensaje = txtMensaje.Text.Trim();
                if(!string.IsNullOrEmpty(mensaje))
                {
                    try
                    {
                        byte[] datos = Encoding.UTF8.GetBytes(mensaje);
                        stream.Write(datos, 0, datos.Length);
                        txtMensaje.Clear();
                        txtChat.AppendText("Yo: " + mensaje + Environment.NewLine);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al enviar mensaje: " + ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("El mensaje no puede estar vacío.");
                }
            }
        }
    }
}
