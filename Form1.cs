using System;
using System.Drawing;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace Remote_Control_Client
{
    public partial class Form1 : Form
    {
        TcpClient client;
        NetworkStream stream;

        public Form1()
        {
            InitializeComponent();
            ConectarServidor();
        }

        private void ConectarServidor()
        {
            try
            {
                client = new TcpClient("192.168.1.153", 5000); // Cambia la IP según sea necesario
                stream = client.GetStream();

                // Iniciar un thread para recibir capturas
                new System.Threading.Thread(RecibirCapturas) { IsBackground = true }.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar con el servidor: " + ex.Message);
            }
        }

        private void RecibirCapturas()
        {
            try
            {
                while (true)
                {
                    // Leer el tamaño de la imagen
                    byte[] tamañoBytes = new byte[4];
                    stream.Read(tamañoBytes, 0, tamañoBytes.Length);
                    int tamaño = BitConverter.ToInt32(tamañoBytes, 0);

                    // Leer la imagen
                    byte[] imagenBytes = new byte[tamaño];
                    int bytesLeidos = 0;
                    while (bytesLeidos < tamaño)
                    {
                        bytesLeidos += stream.Read(imagenBytes, bytesLeidos, tamaño - bytesLeidos);
                    }

                    // Mostrar la imagen en un PictureBox
                    using (MemoryStream ms = new MemoryStream(imagenBytes))
                    {
                        //Image imagenOriginal = Image.FromStream(ms);

                        pictureBox1.Image = Image.FromStream(ms);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al recibir capturas: " + ex.Message);
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            // Enviar movimiento del mouse al servidor
            EnviarMensaje($"MOVE|{e.X}|{e.Y}");
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            // Enviar clic del mouse al servidor
            string boton = e.Button == MouseButtons.Left ? "LEFT" : "RIGHT";
            EnviarMensaje($"CLICK|{boton}");
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
