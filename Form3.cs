using System;
using System.Drawing;
using System.Windows.Forms;

namespace ARDUINO_Project
{
    public partial class Form3 : Form
    {

        private const int WM_NCHITTEST = 0x0084;
        private const int HTCLIENT = 0x0001;
        private const int HTCAPTION = 0x0002;
        private string imagePath;

        public Form3(System.Drawing.Image imagePath)
        {
            InitializeComponent();

            if (imagePath != null)
            {
                pictureBox1.Image = imagePath;
            }
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            if (!string.IsNullOrEmpty(imagePath) && System.IO.File.Exists(imagePath))
            {
                pictureBox1.Image = Image.FromFile(imagePath);
            }
            else
            {

            }
        }

        private void Form3_MouseDown(object sender, MouseEventArgs e)
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
            Form4 form4 = new Form4();
            form4.Show();
            this.Hide();
        }



    }
}
