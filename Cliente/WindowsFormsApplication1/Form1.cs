using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        Socket server;  // Defino variables para guardar los  threads y los formularios.
        Form2 login;
        int ERR = 0;
        List<Form2> formularios = new List<Form2>();
 
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("192.168.1.194");
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            //Creamos un IPEndPoint con el ip del servidor y puerto del servidor 
            //al que deseamos conectarnos
            IPAddress direc = IPAddress.Parse(comboBox1.Text);
            IPEndPoint ipep = new IPEndPoint(direc, 50001);

            //Creamos el socket 
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                server.Connect(ipep);//Intentamos conectar el socket
            }
            catch (SocketException)
            {
                //Si hay excepcion imprimimos error y salimos del programa con return 
                MessageBox.Show("No he podido conectar con el servidor");
                ERR = -1; 
                return;
                
            }
            if (ERR != -1)
            {
               
                ThreadStart ts1 = delegate { activar_formulario_login(); };
                Thread t1 = new Thread(ts1);
                t1.Start();

                //activar_formulario_login();
                //this.Close();
            }
       
            /*pongo en marcha el thread que atenderá los mensajes del servidor
            ThreadStart ts1 = delegate { atender_mensajes_servidor(); };
            Thread t1 = new Thread(ts1);
            t1.Start();*/
            
        }
      
        private void activar_formulario_login()
        {
            //Este thread lo único que hace es activar el formulario de login
            // Se pasa la información de conexión por parametro al nuevo formulario
            login = new Form2(server);
            login.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
              
    }
}
