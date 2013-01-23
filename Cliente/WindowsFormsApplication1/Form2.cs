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
    public partial class Form2 : Form
    {
        Socket server;
        //Form3 users;
        Form4 consultas;
        string us;
        string USER;
        int LOGIN = -1;
        Thread t1;

        public Form2(Socket s)
        {
            InitializeComponent();
            //se asigna de manera global la información del socket que se recibe por paramentro
            server = s;
        }

        private void Form2_Load(object sender, EventArgs e)
        {

            //atender_mensajes_servidor();
            
            ThreadStart ts1 = delegate {atender_mensajes_servidor();};
            t1 = new Thread(ts1);
            t1.Start();
        }
        
        private void activar_formulario_usuarios(string us)
        {
            //Este thread lo único que hace es activar el formulario de jugar o consultar
            //users = new Form3(server, us);
            //users.ShowDialog();
        }

        private void activar_formulario_consultas(string us)
        {
            //Activo el thread del formulario de las diferentes consultas
           
            consultas = new Form4(server);
            consultas.Text = USER;
            consultas.ShowDialog();
        }
 
        private void atender_mensajes_servidor()
        {
            //este thread atiende los mensajes del servidor
            Boolean continuar = true;

            while (continuar)
            {
                int op;
                byte[] msg = new byte[180];
                // recibo mensaje del servidor

                try
                {
                    server.Receive(msg);



                    string mensaje = Encoding.ASCII.GetString(msg);
                    string[] trozos = mensaje.Split(' ');
                    op = Convert.ToInt32(trozos[0]);
                    // Averiguo el tipo de mensaje

                    switch (op)
                    {
                        case 0: // mensaje confirmación de conexión establecida
                            MessageBox.Show("CONEXION CORRECTA CON EL SERVIDOR");

                            //pongo en marcha el thread que activa el formulario de login
                            /*ThreadStart ts2 = delegate { activar_formulario_login(); };
                            Thread t2 = new Thread(ts2);
                            t2.Start();*/
                            break;
                        case 1: // mensaje confirmación de autentificación que el usuario se ha añadido correctamente
                            MessageBox.Show("Usuario dado de alta correctamente");
                            this.us = trozos[1];
                            // pongo en marcha el thread que activa el formulario de usuarios conectados y consultas
                            // ThreadStart ts4 = delegate { activar_formulario_consultas(this.us); };
                            // Thread t4 = new Thread(ts4);
                            //t4.Start();

                            break;
                        case 2:// Recido del servidor el mensaje con un valor de op=11 que querrá decir, caso 1, error 1.
                            LOGIN = -2;
                            MessageBox.Show("Ya te digo yo que tienes un problema con el usuario o el password!, si no lo pones bien vas a suspender...");
                            break;

                        case 3: 
                            
                        // Activo formulario y thread en el que llamo a las consultas
                            // pongo en marcha el thread que activa el formulario de usuarios conectados y consultas
                            
                            LOGIN = 0;
                            ThreadStart ts4 = delegate { activar_formulario_consultas(this.us); };
                            Thread t4 = new Thread(ts4);
                            t4.Start();
                            break;

                        case 4: 
                            // Aqui debería de recibir el listado de conectados
                            //PRIMEIRO ENVIA MENSAGEM COM A OP E DEPOIS COM A INFO BIZARRO 
                            byte[] msg2 = new byte[80];
                            server.Receive(msg2);
                            string consulta_ganadores = Encoding.ASCII.GetString(msg2);
                            MessageBox.Show(consulta_ganadores);
                            break;
                        
                        case 100:

                            continuar = false;
                            break;

                    }
                }
                catch (SocketException e)
                {
                    MessageBox.Show("Socket error " + e.ErrorCode + ": " + e.Message);
                    continuar = false;
                }
                catch (Exception e) // Si se finaliza el servidor inesperadamente
                {
                    MessageBox.Show("Source error " + e.Source + ": " + e.Message);
                    continuar = false;
                }
            }
            MessageBox.Show("Finaliza atender a servidor");
            t1.Abort();
        }
        // Enviamos mensaje para logearnos.
        // Previamente nos tenemos que haber registrado
        // Debemos introducir el mismo user y contraseña que al registrarte
        private void button1_Click(object sender, EventArgs e)
        {
            String usuarioContraseña = "1 " + usuario.Text + " " + contraseña.Text;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(usuarioContraseña);
            server.Send(msg);
            
        }
        // Enviamos mensaje para registrarnos.
        
        private void button2_Click(object sender, EventArgs e)
        {
            Boolean control = true;
            USER = usuario.Text;
            String usuarioContraseña = "2 " + usuario.Text + " " + contraseña.Text;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(usuarioContraseña);
            server.Send(msg);
            
            while (control)
            {
                if (LOGIN == -2)
                {
                    control = false;
                    Thread.Sleep(100);
                }
                if (LOGIN == 0)
                {
                    control = false;
                    this.Close();
                }
            }
        }

        private void usuario_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
