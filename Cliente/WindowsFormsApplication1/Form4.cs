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

namespace WindowsFormsApplication1
{
    public partial class Form4 : Form
    {
        Socket server;

        public Form4(Socket s) 
        {
            InitializeComponent();
            
            server = s;
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void Form4_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
           // String usuarioContraseña = "5 " + usuario2.Text;
            //byte[] msg = System.Text.Encoding.ASCII.GetBytes(usuarioContraseña);
           // server.Send(msg);
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            StringBuilder constructor = new StringBuilder();
            constructor.Append("3 ");
            //constructor.Append(usuario2.Text);

            string verify = constructor.ToString();
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(verify);
            server.Send(msg);

          /*  byte[] msg2 = new byte[80];
            server.Receive(msg2);
            string mensaje = Encoding.ASCII.GetString(msg2);
            MessageBox.Show(mensaje); */
        }

        private void button3_Click(object sender, EventArgs e)
        {
            String logoff = "100 " + this.Text ;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(logoff);
            server.Send(msg);
            this.Close();
        }
    }
}
