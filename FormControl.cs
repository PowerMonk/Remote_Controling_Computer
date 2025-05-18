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
    public partial class FormControl : Form
    {
        TcpClient client;
        NetworkStream stream;
        PictureBox PictureBoxPantalla = new PictureBox
        {
            Dock = DockStyle.Fill,
            SizeMode = PictureBoxSizeMode.StretchImage
        };
        public FormControl(TcpClient client, NetworkStream stream)
        {
            InitializeComponent();
            this.client = client;
            this.stream = stream;
            Controls.Add(PictureBoxPantalla);

            try
            {
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

                        PictureBoxPantalla.Image = Image.FromStream(ms);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al recibir capturas: " + ex.Message);
            }
        }
    }
}
