using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using InTheHand.Net.Bluetooth;
using InTheHand.Windows.Forms;
using InTheHand.Net.Ports;
using InTheHand.Net.Sockets;
using System.Data.Odbc;
using System.Diagnostics;


namespace at_Bluetooth
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }



       public OdbcConnection connection = new OdbcConnection("Driver={Microsoft Access Driver (*.mdb)};DBQ=" + Application.StartupPath.ToString() + "\\database.mdb");
        
            
        public int second = 90;
        private void Form1_Load(object sender, EventArgs e)
        {
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            
            sayac.Enabled = true;
            sayac.Interval = 1000;

            
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
 

          

            

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }


        void killer()
        {

            

        }
        void starter()
        {
            
        }

        void shutdown()
        {

            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = "shutdown.exe";
            psi.Arguments = "-s -f -t 0";
            psi.CreateNoWindow = true;
            Process p = Process.Start(psi);
            starter();

        }
        private void button2_Click(object sender, EventArgs e)
        {
           
        }


        public string databasePhoneAdress;
        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {

                if (BluetoothRadio.IsSupported)
                {
                    SelectBluetoothDeviceDialog dialog = new SelectBluetoothDeviceDialog();
                    dialog.ShowDialog();

                    string phoneAdress;
                    phoneAdress = dialog.SelectedDevice.DeviceAddress.ToString();

                    OdbcCommand cmd = new OdbcCommand("select ogretmenAdi from kayitlar where imeiNumarasi='" + phoneAdress + "'", connection);
                    OdbcCommand cmd2 = new OdbcCommand("select imeiNumarasi from kayitlar where imeiNumarasi='" + phoneAdress + "'", connection);
                    connection.Open();
                    OdbcDataReader read, read2;
                    read = cmd.ExecuteReader();
                    read2 = cmd2.ExecuteReader();

                    while (read2.Read())
                    {
                        databasePhoneAdress = read2["imeiNumarasi"].ToString();
                    }

                    if (databasePhoneAdress == phoneAdress)
                    {
                        while (read.Read())
                        {
                            sayac.Stop();
                            if (MessageBox.Show("Hoşgeldiniz Sayın " + read["ogretmenAdi"].ToString() + ".Akıllı tahtayı kullanmaya Bluetooth cihazını sökerek başlayabilirsiniz.İyi dersler.", "AKILLI TAHTA AKTİF", MessageBoxButtons.OK) == DialogResult.OK)
                            {
                                DateTime today;
                                today = DateTime.Now;
                                today.ToShortDateString();
                                OdbcCommand savee = new OdbcCommand("insert into ogrkayit(ogrAdi,cihazNo,tarih) values('" + read["ogretmenAdi"].ToString() + "','" + phoneAdress + "','" + today + "')", connection);
                                savee.ExecuteNonQuery();

                                if (BluetoothRadio.IsSupported)
                                {
                                    do
                                    {
                                        MessageBox.Show("Bluetooth cihazı bağlı durumda ! Cihazı çıkarınız.");
                                    }
                                    while (BluetoothRadio.IsSupported);
                                }
                                starter();
                                Application.Exit();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Bu telefon sistemde kayıtlı değil ! ");

                        string a, b;
                        a = dialog.SelectedDevice.DeviceName.ToString();
                        b = dialog.SelectedDevice.DeviceAddress.ToString();
                        DateTime today2;
                        today2 = DateTime.Now;
                        today2.ToShortDateString();
                        OdbcCommand saveNew = new OdbcCommand("insert into eslesmeyen(bluetoothAdi,cihazNo,tarih) values('" + a + "','" + b + "','" + today2 + "')", connection);

                        saveNew.ExecuteNonQuery();

                    }
                    connection.Close();
                }
                else
                {
                    MessageBox.Show("Lütfen Bluetooth Cihazını Tahtaya Bağlayınız.");
                    Refresh();
                }
            }
            catch
            {
                MessageBox.Show("EŞLEŞME SAĞLANAMADI");
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            shutdown();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void sayac_Tick(object sender, EventArgs e)
        {
            second--;
            label2.Text = second + " SN.";

            if (second == 0)
            {
                
                sayac.Stop(); 
                shutdown();
            }
        }
    }
}
