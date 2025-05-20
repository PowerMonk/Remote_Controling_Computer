using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization; 
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Remote_Control_Client
{
    public partial class FormControl : Form
    {
        TcpClient client;
        NetworkStream stream;
        PictureBox PictureBoxPantalla = new PictureBox
        {
            Dock = DockStyle.Fill,
            SizeMode = PictureBoxSizeMode.StretchImage
        };

        // Constructor
        public FormControl(TcpClient client, NetworkStream stream)
        {
            InitializeComponent();
            this.client = client;
            this.stream = stream;
            Controls.Add(PictureBoxPantalla);

            // Suscribirse a eventos del PictureBox y del Formulario
            PictureBoxPantalla.MouseDown += PictureBoxPantalla_MouseDown;
            PictureBoxPantalla.MouseUp += PictureBoxPantalla_MouseUp; // Opcional, para clics completos

            this.KeyPreview = true; // Importante para que el Form capture teclas antes que los controles hijos
            this.KeyDown += FormControl_KeyDown;
            this.KeyUp += FormControl_KeyUp;


            try
            {
                new System.Threading.Thread(RecibirCapturas) { IsBackground = true }.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar con el servidor: " + ex.Message);
                this.Close(); // Cerrar si hay error al iniciar
            }
        }

        private void RecibirCapturas()
        {
            try
            {
                while (client.Connected) // si el cliente sigue conectado
                {
                    byte[] tamañoBytes = new byte[4];
                    if (stream.Read(tamañoBytes, 0, tamañoBytes.Length) == 0) break; // Conexión cerrada
                    int tamaño = BitConverter.ToInt32(tamañoBytes, 0);

                    if (tamaño == 0) continue; // Puede ser una señal o error

                    byte[] imagenBytes = new byte[tamaño];
                    int bytesLeidos = 0;
                    while (bytesLeidos < tamaño)
                    {
                        bytesLeidos += stream.Read(imagenBytes, bytesLeidos, tamaño - bytesLeidos);
                    }

                    using (MemoryStream ms = new MemoryStream(imagenBytes))
                    {
                        // Usar Invoke si se accede desde un hilo diferente al de la UI
                        if (PictureBoxPantalla.InvokeRequired)
                        {
                            PictureBoxPantalla.Invoke(new Action(() => PictureBoxPantalla.Image = Image.FromStream(ms)));
                        }
                        else
                        {
                            PictureBoxPantalla.Image = Image.FromStream(ms);
                        }
                    }
                }
            }
            catch (ObjectDisposedException)
            {
                // El stream o el cliente se cerraron, probablemente porque el formulario se está cerrando.
                // No mostrar mensaje de error en este caso.
            }
            catch (IOException ex)
            {
                // Conexión cerrada inesperadamente
                if (!this.IsDisposed && client.Connected) // Solo mostrar si no nos estamos desconectando intencionalmente
                {
                    MessageBox.Show("Error de conexión al recibir capturas: " + ex.Message + "\nEl servidor pudo haberse desconectado.");
                }
            }
            catch (Exception ex)
            {
                if (!this.IsDisposed) // Solo mostrar si el formulario no se está desechando
                {
                    MessageBox.Show("Error al recibir capturas: " + ex.Message);
                }
            }
            finally
            {
                // Asegurarse de que el formulario se cierre si la conexión se pierde y el formulario aún está visible
                if (!this.IsDisposed && this.Visible)
                {
                    this.BeginInvoke(new Action(() => this.Close()));
                }
            }
        }

        private void EnviarComando(string comando)
        {
            if (client.Connected && stream != null && stream.CanWrite)
            {
                try
                {
                    byte[] datosComando = Encoding.UTF8.GetBytes(comando);
                    stream.Write(datosComando, 0, datosComando.Length);
                    stream.Flush(); 
                }
                catch (IOException ex)
                {
                    MessageBox.Show("Error al enviar comando (IOException): " + ex.Message);
                    // Considerar cerrar la conexión o reintentar
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al enviar comando: " + ex.Message);
                }
            }
        }

        private void PictureBoxPantalla_MouseDown(object sender, MouseEventArgs e)
        {
            double ratioX = (double)e.X / PictureBoxPantalla.Width;
            double ratioY = (double)e.Y / PictureBoxPantalla.Height;

            string boton = "";
            if (e.Button == MouseButtons.Left) boton = "LEFT_DOWN";
            else if (e.Button == MouseButtons.Right) boton = "RIGHT_DOWN";
            else if (e.Button == MouseButtons.Middle) boton = "MIDDLE_DOWN";
            // Se puede agregar MouseUp de forma similar para enviar "LEFT_UP", "RIGHT_UP" etc...

            if (!string.IsNullOrEmpty(boton))
            {
                // Usar CultureInfo.InvariantCulture para asegurar que el punto decimal sea '.'
                string comando = $"MOUSE:{boton}:{ratioX.ToString(CultureInfo.InvariantCulture)}:{ratioY.ToString(CultureInfo.InvariantCulture)}";
                EnviarComando(comando);
            }
        }
        private void PictureBoxPantalla_MouseUp(object sender, MouseEventArgs e)
        {
            double ratioX = (double)e.X / PictureBoxPantalla.Width;
            double ratioY = (double)e.Y / PictureBoxPantalla.Height;

            string boton = string.Empty;
            if (e.Button == MouseButtons.Left) boton = "LEFT_UP";
            else if (e.Button == MouseButtons.Right) boton = "RIGHT_UP";
            else if (e.Button == MouseButtons.Middle) boton = "MIDDLE_UP";

            if (!string.IsNullOrEmpty(boton))
            {
                string comando = $"MOUSE:{boton}:{ratioX.ToString(CultureInfo.InvariantCulture)}:{ratioY.ToString(CultureInfo.InvariantCulture)}";
                EnviarComando(comando);
            }
        }
        private void FormControl_KeyDown(object sender, KeyEventArgs e)
        {
            // Convertimos KeyCode a su valor entero para enviarlo
            // El servidor necesitará mapear esto de nuevo si es necesario o usarlo directamente con keybd_event
            string comando = $"KEY:DOWN:{(int)e.KeyCode}";
            EnviarComando(comando);
            e.Handled = true; // Marcar como manejado para evitar que el sistema procese la tecla también
        }

        private void FormControl_KeyUp(object sender, KeyEventArgs e)
        {
            string comando = $"KEY:UP:{(int)e.KeyCode}";
            EnviarComando(comando);
            e.Handled = true;
        }

        // Asegurarse de limpiar recursos al cerrar el formulario
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            // No es necesario cerrar explícitamente client y stream aquí si se pasan desde Form1
            // y Form1 gestiona su ciclo de vida. Si FormControl es el dueño, entonces sí.
        }
    }
}