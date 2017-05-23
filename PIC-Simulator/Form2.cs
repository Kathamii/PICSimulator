using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace PIC_Simulator
{
    public partial class Form2 : Form
    {
        SerialPort serialPort1;
        public Form2(SerialPort serialport)
        {
            
            InitializeComponent();
            serialPort1 = serialport;
            getserialports();
        }

        //get all available serial ports
        private void getserialports()
        {
            String[] ports = SerialPort.GetPortNames();
            combobox_port.Items.AddRange(ports);
        }

        //disconnect serial port
        private void button1_Click(object sender, EventArgs e)
        {
            serialPort1.Close();
            label3.Text = "No Connection";
            
        }
       
        //connect serial port
        public void button2_Click(object sender, EventArgs e)
        {
            
            try
            {

                if (combobox_port.Text == "" || combox_bausrate.Text == "")
                {
                    MessageBox.Show("Please select port settings");
                }
                else
                {
                    serialPort1.PortName = combobox_port.Text;
                    serialPort1.BaudRate = Convert.ToInt32(combox_bausrate.Text);
                    serialPort1.DataBits = 8;
                    serialPort1.Parity = Parity.None;
                    serialPort1.StopBits = StopBits.One;
                    serialPort1.Open();
                    label3.Text = "Connection established";
                
                }
            }
            catch (UnauthorizedAccessException)
            {
                label3.Text = "Connection Failed";
            }
        }

        //set status
        public string status()
        {
            return label3.Text.ToString();
        }
        
    }
}
