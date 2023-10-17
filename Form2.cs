using ARDUINO_Project;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Windows.Forms;




namespace ARDUINO_Project
{
    public partial class Form2 : Form
    {

        private const int WM_NCHITTEST = 0x0084;
        private const int HTCLIENT = 0x0001;
        private const int HTCAPTION = 0x0002;
        private SerialPort serialPort;

        private Dictionary<string, System.Drawing.Bitmap> imageResourceMap = new Dictionary<string, System.Drawing.Bitmap>();

        public Form2(SerialPort port)
        {
            InitializeComponent();

            imageResourceMap.Add("metal", Properties.Resource1.metal);
            imageResourceMap.Add("plastic", Properties.Resource1.plastic);
            imageResourceMap.Add("waste", Properties.Resource1.waste);

            this.StartPosition = FormStartPosition.CenterScreen;
            serialPort = port;

        }


        private void Form2_Load(object sender, EventArgs e)
        {
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.ResizeRedraw, true);
        }

        private void Form2_MouseDown(object sender, MouseEventArgs e)
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
                serialPort.ReadTimeout = 5000;

                // char wasteType = (char)serialPort.ReadByte();
                char wasteType = 'P';
                string material = "unknown"; 
                if (wasteType == 'P')
                {
                    material = "plastic";
                }
                else if (wasteType == 'M')
                {
                    material = "metal";
                }
                else if (wasteType == 'W')
                {
                    material = "waste";
                }

                if (imageResourceMap.ContainsKey(material))
                {
                    System.Drawing.Image imagePath = imageResourceMap[material];
                    Form3 form3 = new Form3(imagePath);
                    form3.Show();
                    serialPort.Close();               
                    this.Hide();
                }
                else
                {
                    Console.WriteLine("Unknown waste type: " + material);
                }
            }
            catch (TimeoutException ex)
            {
                MessageBox.Show("Timeout: No data received from Arduino.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

        }


    }
}
