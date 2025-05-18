using System;
using System.Drawing;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Remote_Control_Client
{
    public partial class Form1 : Form
    {
        TcpClient client;
        NetworkStream stream;
        System.Windows.Forms.ProgressBar progressBar;
        string archivoSeleccionado = string.Empty;
        string ServerIp = "192.168.1.153";
        const int ServerPort = 5000;
        const int FileServerPort = 12345;

        public Form1()
        {
            InitializeComponent();
            ConectarServidor();
            btnControl.Click += btnControl_Click;
            btnMensaje.Click += btnMensaje_Click;
            btnArchivo.Click += btnArchivo_Click;

            // Inicializar la ProgressBar
            progressBar = new System.Windows.Forms.ProgressBar
            {
                Dock = DockStyle.Bottom,
                Minimum = 0,
                Maximum = 100,
                Visible = false // Oculta inicialmente
            };
            Controls.Add(progressBar);

        }

        private void ConectarServidor()
        {
            try
            {
                client = new TcpClient(ServerIp, ServerPort); // Cambiar la IP/Puerto según sea necesario
                stream = client.GetStream();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar con el servidor: " + ex.Message);
            }
        }

        private void btnControl_Click(object sender, EventArgs e)
        {
            // Por si queremos ocultar el Form1
            //this.Hide();

            // Mostrar el formulario de control remoto
            FormControl control = new FormControl(client, stream);
            control.FormClosed += (s, args) => this.Show(); // Mostrar Form1 cuando se cierre el otro
            control.Show();
        }

        private void btnMensaje_Click(object sender, EventArgs e)
        {
            // Por si lo queremos ocultar
            //this.Hide();

            // Mostrar el formulario de control remoto
            FormMensaje control = new FormMensaje(client, stream);
            control.FormClosed += (s, args) => this.Show(); // Mostrar Form1 cuando se cierre el otro
            control.Show();
        }
        private void btnArchivo_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                archivoSeleccionado = ofd.FileName;
                EnviarArchivo();
            }
        }
        private void EnviarArchivo()
        {
            if (string.IsNullOrEmpty(archivoSeleccionado))
            {
                MessageBox.Show("No se ha seleccionado ningún archivo.");
                return;
            }

            try
            {
                using (TcpClient clienteArchivos = new TcpClient(ServerIp, FileServerPort)) // Usamos un nombre diferente para el cliente de archivos
                using (NetworkStream fileStream = clienteArchivos.GetStream()) // Obtenemos el flujo del nuevo cliente
                using (BinaryWriter writer = new BinaryWriter(fileStream))
                {
                    FileInfo fileInfo = new FileInfo(archivoSeleccionado);
                    writer.Write(fileInfo.Name);
                    writer.Write(fileInfo.Length);
                    byte[] buffer = new byte[4096];
                    using (FileStream fs = new FileStream(archivoSeleccionado, FileMode.Open))
                    {
                        int bytesSent = 0;
                        int bytesRead;
                        while ((bytesRead = fs.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            fileStream.Write(buffer, 0, bytesRead);
                            bytesSent += bytesRead;
                            ActualizarProgress((int)(bytesSent * 100 / fileInfo.Length));
                        }
                    }
                    MessageBox.Show("Archivo enviado correctamente.", "El archivo se envió correctamente", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al enviar archivo: " + ex.Message);
            }
        }


        private void ActualizarProgress(int porcentaje)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<int>(ActualizarProgress), porcentaje);
                return;
            }

            if (!progressBar.Visible)
            {
                progressBar.Visible = true; // Mostrar la ProgressBar al iniciar
            }

            progressBar.Value = porcentaje;

            if (porcentaje >= 100)
            {
                progressBar.Visible = false; // Ocultar la ProgressBar al finalizar
            }
        }


        private void EnviarMensaje(string mensaje)
        {
            try
            {
                byte[] datos = Encoding.UTF8.GetBytes(mensaje);
                stream.Write(datos, 0, datos.Length);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al enviar mensaje: " + ex.Message);
            }
        }
    }
}
