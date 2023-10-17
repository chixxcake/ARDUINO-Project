using ARDUINO_Project;
using System;
using System.IO.Ports;
using System.Windows.Forms;

namespace ARDUINO_Project
{
    public partial class Form4 : Form
    {

        private const int WM_NCHITTEST = 0x0084;
        private const int HTCLIENT = 0x0001;
        private const int HTCAPTION = 0x0002;
        private SerialPort serialPort;


        public Form4()
        {
            InitializeComponent();

            this.StartPosition = FormStartPosition.CenterScreen;
            serialPort = new SerialPort("COM4", 9600); // Adjust port and baud rate.    
            StartProgressBar();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.ResizeRedraw, true);
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Capture = false;
                Message message = Message.Create(Handle, 0xa1, new IntPtr(2), IntPtr.Zero);
                WndProc(ref message);
            }
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WM_NCHITTEST && (int)m.Result == HTCLIENT)
            {
                m.Result = (IntPtr)HTCAPTION;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort.Open();
                Form2 form2 = new Form2(serialPort);
                form2.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void StartProgressBar()
        {
            int progress = 0;
            int maxProgress = 5; 

            Timer timer = new Timer();
            timer.Interval = 1000; 
            timer.Tick += (s, e) =>
            {
                progress++;
                progressBar1.Value = progress;

                if (progress >= maxProgress)
                {
                    timer.Stop();
                    Close();
                    Form5 form5 = new Form5();
                    form5.Show();
                }
            };

            timer.Start();
        }

     
    }
}
