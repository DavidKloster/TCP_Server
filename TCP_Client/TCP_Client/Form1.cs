using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace TCP_Client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ipEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9050);
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            data = new byte[1024];
        }

        private Socket server;
        private string stringData;
        private IPEndPoint ipEndPoint;
        private byte[] data;

        /// <summary>
        /// Handles the Click event of the btnSend control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (server.Connected)
                    server.Send(textBox1.Text.ToByteArray());
                else 
                    WriteLine("Not connected to Server!");

            }
            catch (SocketException exception)
            {
                Console.WriteLine("Unable to connect to server.");
                Console.WriteLine(exception.ToString());
                return;
            }
        }

        /// <summary>
        /// Handles the Click event of the btnOpen control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (!server.Connected)
                try
                {
                    server.Connect(ipEndPoint);
                    WriteLine("Connected to Server!");

                    var listener = new Thread(Listen);
                    listener.Priority = ThreadPriority.Lowest;
                    listener.Start();
                }
                catch (Exception exception)
                {
                    WriteLine(exception.Message);
                }
        }

        /// <summary>
        /// Handles the FormClosing event of the Form1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.FormClosingEventArgs"/> instance containing the event data.</param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Disconnect();
        }

        /// <summary>
        /// Handles the Click event of the btnClose control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            Disconnect();
        }

        delegate void Invoker(string parameter);

        /// <summary>
        /// Writes to Textbox
        ///  </summary>
        /// <param name="output">The output.</param>
        private void WriteLine(string output)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Invoker(WriteLine), output);
                return;
            }
            tbxTrace.Text += output + Environment.NewLine;
        }
        
        private void Listen()
        {
            var recv = server.Receive(data);
            stringData = Encoding.ASCII.GetString(data, 0, recv);
            WriteLine(stringData);

            while (server.Connected)
            {
                data = new byte[1024];
                recv = server.Receive(data);
                stringData = Encoding.ASCII.GetString(data, 0, recv);
                WriteLine("Received from Server: " + stringData);
            }
            WriteLine("Disconnecting from server...");
            server.Close();
        }

        private void Disconnect()
        {
            if (server.Connected)
                server.Disconnect(true);

            if (!server.Connected)
                WriteLine("Connection closed!");
        }
    }
}