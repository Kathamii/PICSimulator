using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Threading;
using System.IO.Ports;
using AxAcroPDFLib;

namespace PIC_Simulator
{
    public partial class Form1 : Form
    {
        int j = 0;
        int returnrow = 1234567;
        double runtime;
        double quarzfrequenz = 4000;
        int befehlnr = 0;

        int carry = 2;
        int zero = 2;
        int digitcarry = 2;
        int to = 1;
        int pd = 1;
        int rbie;
        int TimerWert;
        int TimerWertbefehl = 0;
        int Ausgabewert;

        int takta = 300;
        int taktb = 300;

        Color colorset;
        Form2 conn;



        public Form1()
        {

            InitializeComponent();
            

            timer4.Interval = 200;
         
            

            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Unterstützte Dateien (*.lst)|*.lst";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                textbox_path.Text = ofd.FileName; //Dateiname der ausgewählten Datei
            }
        }

        private void readbtn_Click(object sender, EventArgs e)
        {
            string path = textbox_path.Text;
            string line;
            int start = 0;
            try {
                System.IO.StreamReader file =
                    new System.IO.StreamReader(path);


                dataGridView2.Rows.Add("w", "", "");
                dataGridView2.Rows.Add("INDF", "00h", "--------");
                dataGridView2.Rows.Add("TMR0", "01h", "xxxxxxxx");
                dataGridView2.Rows.Add("PCL", "02h", "00000000");
                dataGridView2.Rows.Add("STATUS", "03h", "00011xxx");
                dataGridView2.Rows.Add("FSR", "04h", "xxxxxxxx");
                dataGridView2.Rows.Add("PORTA", "05h", "---xxxxx");
                dataGridView2.Rows.Add("PORTB", "06h", "xxxxxxxx");
                dataGridView2.Rows.Add("EEDATA", "08h", "xxxxxxxx");
                dataGridView2.Rows.Add("EEADR", "09h", "xxxxxxxx");
                dataGridView2.Rows.Add("PCLATH", "0Ah", "---00000");
                dataGridView2.Rows.Add("INTCON", "0Bh", "0000000x");

                dataGridView3.Rows.Add("w", "", "");
                dataGridView3.Rows.Add("INDF", "80h", "--------");
                dataGridView3.Rows.Add("OPTION_REG", "81h", "11111000");
                dataGridView3.Rows.Add("PCL", "82h", "00000000");
                dataGridView3.Rows.Add("STATUS", "83h", "00011xxx");
                dataGridView3.Rows.Add("FSR", "84h", "xxxxxxxx");
                dataGridView3.Rows.Add("TRISA", "85h", "---11111");
                dataGridView3.Rows.Add("TRISB", "86h", "11111111");
                dataGridView3.Rows.Add("EECON1", "88h", "---0x000");
                dataGridView3.Rows.Add("EECON2", "89h", "--------");
                dataGridView3.Rows.Add("PCLATH", "0Ah", "---00000");
                dataGridView3.Rows.Add("INTCON", "0Bh", "0000000x");
                int button;

                for (int i = 0; i < 8; i++)
                {
                    if (dataGridView3[2, 6].Value.ToString().Substring(i, 1) == "1")
                    {
                        button = i + 1;
                        inputoutput(button, true);
                    }
                    if (dataGridView3[2, 6].Value.ToString().Substring(i, 1) == "0")
                    {
                        button = i + 1;
                        inputoutput(button, false);
                    }
                    if (dataGridView3[2, 7].Value.ToString().Substring(i, 1) == "1")
                    {
                        button = i + 9;
                        inputoutput(button, true);
                    }
                    if (dataGridView3[2, 7].Value.ToString().Substring(i, 1) == "0")
                    {
                        button = i + 9;
                        inputoutput(button, false);
                    }
                }
                while ((line = file.ReadLine()) != null)
                {
                    /////Anlegen register, data sheet s.9
                    List<String> registername = new List<String>();
                    List<String> varvalue = new List<String>();

                    if (start == 0)
                    {
                        Regex equ = new Regex("equ");
                        if (equ.Match(line).Success)
                        {
                            string match = line.Remove(0, 36);
                            Regex regname = new Regex("[a-zA-Z]{1}[a-zA-Z0-9]*[ ]*equ");
                            Regex regvalue = new Regex("equ[ ]*[0-9a-fA-F]*[h]?");
                            string register = regname.Match(line).Value;
                            register = register.Substring(0, register.Length - 3);
                            register = register.TrimEnd(' ');
                            string value = regvalue.Match(line).Value;
                            value = value.Remove(0, 4);
                            value = value.TrimStart(' ');
                            registername.Add(register);
                            varvalue.Add(value);
                        }
                    }
                    for (int i = 0; registername.Count > i; i++)
                    {
                        dataGridView4.Rows.Add(registername[i], varvalue[i], "");
                    }

                    if (start == 1)
                    {
                        Regex regloop = new Regex("[0-9]{5}[ ]{2}[A-Za-z0-9]*");
                        if (regloop.Match(line).Success)
                        {
                            string match = regloop.Match(line).Value;
                            match = match.Remove(0, 7);
                            if (match != "")
                            {
                                dataGridView1.Rows.Add(false, "", "", "" + match + "");
                                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.LightGray;
                            }
                        }
                        Regex regop = new Regex("[0-9A-F]{4}[ ]{1}[0-9a-fA-F]{4}[ ]{1}");
                        if (regop.Match(line).Success)
                        {
                            string match = line;
                            string idx = match.Remove(4);
                            string bitop = match.Remove(9).Remove(0, 4).TrimStart(' ');

                            Regex regops = new Regex("[ a-zA-Z0-9,]*");
                            string op = match.Remove(0, 36);
                            op = regops.Match(op).Value;
                            op = op.TrimEnd(' ');
                            dataGridView1.Rows.Add(false, idx, bitop, op);
                        }
                    }
                    Regex r = new Regex("org[ ]*0");
                    if (r.Match(line).Success)
                    {
                        start = 1;
                    }
                }
                for (int i = 0; i < dataGridView4.RowCount; i++)
                {
                    string locs = dataGridView4[1, i].Value.ToString();
                    for (int j = 0; j < dataGridView2.RowCount; j++)
                    {
                        if (dataGridView2[1, j].Value.ToString() == locs)
                        {
                            dataGridView4[2, i].Value = dataGridView2[2, j].Value.ToString();
                        }
                    }
                }
                

            }
            catch
            { MessageBox.Show("Error!"); }
            startbtn.Enabled = true;
            stepbtn.Enabled = true;
            
        }

        private void clearbtn_Click(object sender, EventArgs e)
        {
            serialPort1.Close();
            Form1 NewForm = new Form1();
            NewForm.Show();
            this.Dispose(false);
        }

        private async void startbtn_Click(object sender, EventArgs e)
        {
            timer2.Interval = taktb;
            timer1.Interval = takta;
            timer3.Interval = 200;
            timer3.Start();
            timer2.Start();
            timer1.Start();

            if(dataGridView1.CurrentRow.DefaultCellStyle.BackColor != Color.Salmon) colorset = dataGridView1.CurrentRow.DefaultCellStyle.BackColor;
            while (dataGridView1.CurrentRow.Index < dataGridView1.RowCount && dataGridView1.CurrentRow.Cells[0].Value.ToString() == "False")

            {

                //try
                //{
                //    dataGridView1.Rows[dataGridView1.CurrentRow.Index - 1].DefaultCellStyle.BackColor = Color.White;
                //    if (dataGridView1.Rows[dataGridView1.CurrentRow.Index - 1].Cells[2].Value == "")
                //    { dataGridView1.Rows[dataGridView1.CurrentRow.Index - 1].DefaultCellStyle.BackColor = Color.LightGray; }

                //}
                //catch { }
                
                dataGridView1.CurrentRow.DefaultCellStyle.BackColor = Color.LightSalmon;
                string opcode = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                string opstring = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                int row = dataGridView1.CurrentRow.Index;
                int retvalue = dooperator(opcode, opstring, row);
                //Zeitdurchlauf für einen Befehl
                await Task.Delay(20);


                if (retvalue != 1234567)
                {
                    dataGridView1.Rows[dataGridView1.CurrentRow.Index].DefaultCellStyle.BackColor = colorset;
                    dataGridView1.CurrentCell = dataGridView1[1, retvalue];
                    colorset = dataGridView1.CurrentRow.DefaultCellStyle.BackColor;
                    dataGridView1.CurrentRow.DefaultCellStyle.BackColor = Color.LightSalmon;
                }
                else
                {
                    dataGridView1.Rows[dataGridView1.CurrentRow.Index].DefaultCellStyle.BackColor = colorset;
                    dataGridView1.CurrentCell = dataGridView1[1, dataGridView1.CurrentRow.Index + 1];
                    colorset = dataGridView1.CurrentRow.DefaultCellStyle.BackColor;
                    dataGridView1.CurrentRow.DefaultCellStyle.BackColor = Color.LightSalmon;
                }


            }
            if (dataGridView1.CurrentRow.Cells[0].Value.ToString() == "True") dataGridView1.CurrentRow.Cells[0].Value = "False";
        }

        private int dooperator(string opcode, string opstring, int row)
        {
            
            int rownr = 0;

            string binarystring = "";
            try
            {
                binarystring = String.Join(String.Empty,
                opcode.Select(
                c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')
                )
                ).Substring(2);
                
            }
            catch { }

            
            befehlnr++;
            TimerWertbefehl++;
            switch (readopcode(binarystring))
            {

                case 0:
                    //ADDWF
                    string d = binarystring.Substring(6, 1);
                    binarystring = binarystring.Substring(7);
                    string loc = BinaryStringToHexString(binarystring);

                    if (loc == "00h")
                    {
                        loc = BinaryStringToHexString(dataGridView2[2, 5].Value.ToString());
                    }
                    if (dataGridView2[2, 4].Value.ToString().Substring(2, 1) == "1" && loc.StartsWith("0")) loc = "8" + loc.Substring(1);

                    int locrow = 1234567;
                    for (int i = 0; dataGridView4.Rows.Count > i; i++)
                    {
                        if (dataGridView4[1, i].Value.ToString() == loc)
                        {
                            locrow = i;
                        }
                    }

                    string sf = dataGridView4[2, locrow].Value.ToString();
                    string w = dataGridView2[2, 0].Value.ToString();
                    int addwf = bin2dec(sf) + bin2dec(w);
                    if (addwf > 255)
                    {
                        carry = 1;
                        addwf = addwf - 256;
                    }
                    else carry = 0;

                    for (int i = 128; bin2dec(w) > 15 || bin2dec(sf) > 15; i = i / 2)
                    {
                        if (bin2dec(w) >= i) w = dec2bin(bin2dec(w) - i);
                        if (bin2dec(sf) >= i) sf = dec2bin(bin2dec(sf) - i);
                    }
                    int dc = bin2dec(sf) + bin2dec(w);
                    if (dc > 15) digitcarry = 1;
                    else digitcarry = 0;

                    if (addwf == 0) zero = 1;
                    else zero = 0;

                    if (d == "1") dataGridView4[2, locrow].Value = dec2bin(addwf);
                    if (d == "0")
                    {
                        dataGridView2[2, 0].Value = dec2bin(addwf);
                        dataGridView3[2, 0].Value = dec2bin(addwf);
                    }
                    

                    break;
                case 1:
                    //ANDWF

                    d = binarystring.Substring(6, 1);
                    binarystring = binarystring.Substring(7);
                    loc = BinaryStringToHexString(binarystring);


                    if (loc == "00h")
                    {
                        loc = BinaryStringToHexString(dataGridView2[2, 5].Value.ToString());
                    }
                    if (dataGridView2[2, 4].Value.ToString().Substring(2, 1) == "1" && loc.StartsWith("0")) loc = "8" + loc.Substring(1);
                    locrow = 1234567;
                    bool e = false;
                    for (int i = 0; dataGridView4.Rows.Count > i; i++)
                    {
                        if (dataGridView4[1, i].Value.ToString() == loc)
                        {
                            locrow = i;
                            e = true;
                        }
                    }

                    sf = dataGridView4[2, locrow].Value.ToString();
                    w = dataGridView2[2, 0].Value.ToString();


                    string equ = "";
                    for (int i = 7; i >= 0; i--)
                    {
                        if (sf.Substring(i, 1) == "1" && w.Substring(i, 1) == "1")
                        {
                            equ = "1" + equ;
                        }
                        else
                        {
                            equ = "0" + equ;
                        }
                    }
                    if (equ == "00000000") zero = 1;
                    else zero = 0;

                    if (d == "1") dataGridView4[2, locrow].Value = equ;
                    if (d == "0")
                    {
                        dataGridView2[2, 0].Value = equ;
                        dataGridView3[2, 0].Value = equ;
                    }


                    break;
                case 2:
                    //CLRF
                    binarystring = binarystring.Substring(7);
                    loc = BinaryStringToHexString(binarystring);

                    if (loc == "00h")
                    {
                        loc = BinaryStringToHexString(dataGridView2[2, 5].Value.ToString());
                    }
                    if (dataGridView2[2, 4].Value.ToString().Substring(2, 1) == "1" && loc.StartsWith("0")) loc = "8" + loc.Substring(1);
                    locrow = 1234567;
                    bool ex = false;
                    for (int i = 0; dataGridView4.Rows.Count > i; i++)
                    {
                        if (dataGridView4[1, i].Value.ToString() == loc)
                        {
                            locrow = i;
                            ex = true;
                        }
                    }
                    if (ex)
                    {
                        dataGridView4[2, locrow].Value = "00000000";
                    }
                    else
                    {
                        dataGridView4.Rows.Add("", loc, "00000000");
                    }

                    zero = 1;

                    break;
                case 3:
                    //CLRW

                    dataGridView2[2, 0].Value = "00000000";
                    dataGridView3[2, 0].Value = "00000000";
                    zero = 1;

                    break;
                case 4:
                    //COMF
                    d = binarystring.Substring(6, 1);
                    binarystring = binarystring.Substring(7);
                    loc = BinaryStringToHexString(binarystring);

                    if (loc == "00h")
                    {
                        loc = BinaryStringToHexString(dataGridView2[2, 5].Value.ToString());
                    }
                    if (dataGridView2[2, 4].Value.ToString().Substring(2, 1) == "1" && loc.StartsWith("0")) loc = "8" + loc.Substring(1);
                    locrow = 1234567;
                    for (int i = 0; dataGridView4.Rows.Count > i; i++)
                    {
                        if (dataGridView4[1, i].Value.ToString() == loc)
                        {
                            locrow = i;
                        }
                    }
                    string v;
                    string f = dataGridView4[2, locrow].Value.ToString();
                    string comp = "";
                    for (int i = 0; i < 8; i++)
                    {
                        if (f.Substring(i, 1) == "1") v = "0";
                        else v = "1";
                        comp = comp + v;
                    }
                    if (comp == "00000000") zero = 1;
                    else zero = 0;

                    if (d == "1") dataGridView4[2, locrow].Value = comp;
                    else
                    {
                        dataGridView2[2, 0].Value = comp;
                        dataGridView3[2, 0].Value = comp;
                    }




                    break;
                case 5:
                    //DECF
                    d = binarystring.Substring(6, 1);
                    binarystring = binarystring.Substring(7);
                    loc = BinaryStringToHexString(binarystring);

                    if (loc == "00h")
                    {
                        loc = BinaryStringToHexString(dataGridView2[2, 5].Value.ToString());
                    }
                    if (dataGridView2[2, 4].Value.ToString().Substring(2, 1) == "1" && loc.StartsWith("0")) loc = "8" + loc.Substring(1);
                    locrow = 1234567;
                    for (int i = 0; dataGridView4.Rows.Count > i; i++)
                    {
                        if (dataGridView4[1, i].Value.ToString() == loc)
                        {
                            locrow = i;
                        }
                    }
                    int incf = bin2dec(dataGridView4[2, locrow].Value.ToString()) - 1;
                    if (incf < 0) incf = 255;
                    if (incf == 0) zero = 1;
                    else zero = 0;
                    if (d == "1") dataGridView4[2, locrow].Value = dec2bin(incf);
                    if (d == "0")
                    {
                        dataGridView2[2, 0].Value = dec2bin(incf);
                        dataGridView3[2, 0].Value = dec2bin(incf);
                    }

                    break;
                case 6:
                    //DECFSZ

                    d = binarystring.Substring(6, 1);
                    binarystring = binarystring.Substring(7);
                    loc = BinaryStringToHexString(binarystring);
                    if (loc == "00h")
                    {
                        loc = BinaryStringToHexString(dataGridView2[2, 5].Value.ToString());
                    }
                    if (dataGridView2[2, 4].Value.ToString().Substring(2, 1) == "1" && loc.StartsWith("0")) loc = "8" + loc.Substring(1);
                    locrow = 1234567;
                    for (int i = 0; dataGridView4.Rows.Count > i; i++)
                    {
                        if (dataGridView4[1, i].Value.ToString() == loc)
                        {
                            locrow = i;
                        }
                    }
                    incf = bin2dec(dataGridView4[2, locrow].Value.ToString()) - 1;
                    if (incf < 0) incf = 255;
                    if (d == "1") dataGridView4[2, locrow].Value = dec2bin(incf);
                    if (d == "0")
                    {
                        dataGridView2[2, 0].Value = dec2bin(incf);
                        dataGridView3[2, 0].Value = dec2bin(incf);
                    }
                    if (incf == 0)
                    {
                        befehlnr++;
                        TimerWertbefehl++;
                        zero = 1;
                        //rownr = rownr + 1;
                        dataGridView1.CurrentRow.DefaultCellStyle.BackColor = Color.White;
                        dataGridView1.CurrentCell = dataGridView1[0, dataGridView1.CurrentRow.Index + 1];
                        //dataGridView1.Rows[rownr - 1].DefaultCellStyle.BackColor = Color.White;
                    }
                    else zero = 0;


                    break;
                case 7:
                    //INCF

                    d = binarystring.Substring(6, 1);
                    binarystring = binarystring.Substring(7);
                    loc = BinaryStringToHexString(binarystring);

                    if (loc == "00h")
                    {
                        loc = BinaryStringToHexString(dataGridView2[2, 5].Value.ToString());
                    }
                    if (dataGridView2[2, 4].Value.ToString().Substring(2, 1) == "1" && loc.StartsWith("0")) loc = "8" + loc.Substring(1);
                    locrow = 1234567;
                    for (int i = 0; dataGridView4.Rows.Count > i; i++)
                    {
                        if (dataGridView4[1, i].Value.ToString() == loc)
                        {
                            locrow = i;
                        }
                    }

                    incf = bin2dec(dataGridView4[2, locrow].Value.ToString()) + 1;
                    if (incf > 255) incf = 0; 
                    if (incf == 0) zero = 1;
                    else zero = 0;
                    if (d == "1") dataGridView4[2, locrow].Value = dec2bin(incf);
                    if (d == "0")
                    {
                        dataGridView2[2, 0].Value = dec2bin(incf);
                        dataGridView3[2, 0].Value = dec2bin(incf);
                    }

                    break;
                case 8:
                    //INCFSZ

                    d = binarystring.Substring(6, 1);
                    binarystring = binarystring.Substring(7);
                    loc = BinaryStringToHexString(binarystring);
                    if (loc == "00h")
                    {
                        loc = BinaryStringToHexString(dataGridView2[2, 5].Value.ToString());
                    }
                    if (dataGridView2[2, 4].Value.ToString().Substring(2, 1) == "1" && loc.StartsWith("0")) loc = "8" + loc.Substring(1);
                    locrow = 1234567;
                    for (int i = 0; dataGridView4.Rows.Count > i; i++)
                    {
                        if (dataGridView4[1, i].Value.ToString() == loc)
                        {
                            locrow = i;
                        }
                    }
                    incf = bin2dec(dataGridView4[2, locrow].Value.ToString()) + 1;
                    if (incf > 255) incf = 0;
                    if (d == "1") dataGridView4[2, locrow].Value = dec2bin(incf);
                    if (d == "0")
                    {
                        dataGridView2[2, 0].Value = dec2bin(incf);
                        dataGridView3[2, 0].Value = dec2bin(incf);
                    }
                    if (incf == 0)
                    {
                        befehlnr++;
                        TimerWertbefehl++;
                        zero = 1;
                        dataGridView1.CurrentRow.DefaultCellStyle.BackColor = Color.White;
                        dataGridView1.CurrentCell = dataGridView1[0, dataGridView1.CurrentRow.Index + 1];

                    }
                    else zero = 0;

                    break;
                case 9:
                    //IORWF

                    d = binarystring.Substring(6, 1);
                    binarystring = binarystring.Substring(7);
                    loc = BinaryStringToHexString(binarystring);
                    if (loc == "00h")
                    {
                        loc = BinaryStringToHexString(dataGridView2[2, 5].Value.ToString());
                    }
                    if (dataGridView2[2, 4].Value.ToString().Substring(2, 1) == "1" && loc.StartsWith("0")) loc = "8" + loc.Substring(1);
                    locrow = 1234567;
                    for (int i = 0; dataGridView4.Rows.Count > i; i++)
                    {
                        if (dataGridView4[1, i].Value.ToString() == loc)
                        {
                            locrow = i;
                        }
                    }

                    binarystring = binarystring.Substring(6);
                    string w9 = dataGridView2[2, 0].Value.ToString();
                    string equ9 = "";
                    for (int i = 7; i >= 0; i--)
                    {
                        if (dataGridView4[2, locrow].Value.ToString().Substring(i, 1) == "1" || w9.Substring(i, 1) == "1")
                        {
                            equ9 = "1" + equ9;
                        }
                        else
                        {
                            equ9 = "0" + equ9;
                        }
                    }
                    if (equ9 == "00000000") zero = 1;
                    else zero = 0;
                    if (d == "0")
                    {
                        dataGridView2[2, 0].Value = equ9;
                        dataGridView3[2, 0].Value = equ9;
                    }
                    else dataGridView4[2, locrow].Value = equ9;

                    break;
                case 10:
                    //MOVF
                    d = binarystring.Substring(6, 1);
                    binarystring = binarystring.Substring(7);
                    loc = BinaryStringToHexString(binarystring);
                    if (loc == "00h")
                    {
                        loc = BinaryStringToHexString(dataGridView2[2, 5].Value.ToString());
                    }
                    if (dataGridView2[2, 4].Value.ToString().Substring(2, 1) == "1" && loc.StartsWith("0")) loc = "8" + loc.Substring(1);
                    locrow = 1234567;
                    for (int i = 0; dataGridView4.Rows.Count > i; i++)
                    {
                        if (dataGridView4[1, i].Value.ToString() == loc)
                        {
                            locrow = i;
                        }
                    }
                    if (locrow != 1234567)
                    {
                        if (dataGridView4[2, locrow].Value.ToString() == "00000000")
                            zero = 1;
                        else zero = 0;
                        if (d == "0")
                        {
                            dataGridView2[2, 0].Value = dataGridView4[2, locrow].Value.ToString();
                            dataGridView3[2, 0].Value = dataGridView4[2, locrow].Value.ToString();
                        }
                    }
                    else
                    {
                        for (int i = 0; dataGridView2.Rows.Count > i; i++)
                        {
                            if (dataGridView2[1, i].Value.ToString() == loc)
                            {
                                locrow = i;
                            }
                        }
                        if (locrow != 1234567)
                        {
                            if (dataGridView2[2, locrow].Value.ToString() == "00000000")
                                zero = 1;
                            else zero = 0;
                            if (d == "0")
                            {
                                dataGridView2[2, 0].Value = dataGridView2[2, locrow].Value.ToString();
                                dataGridView3[2, 0].Value = dataGridView2[2, locrow].Value.ToString();
                            }
                        }
                        else
                        {
                            for (int i = 0; dataGridView3.Rows.Count > i; i++)
                            {
                                if (dataGridView2[1, i].Value.ToString() == loc)
                                {
                                    locrow = i;
                                }
                            }
                            if (locrow != 1234567)
                            {
                                if (dataGridView3[2, locrow].Value.ToString() == "00000000")
                                    zero = 1;
                                else zero = 0;
                                if (d == "0")
                                {
                                    dataGridView2[2, 0].Value = dataGridView3[2, locrow].Value.ToString();
                                    dataGridView3[2, 0].Value = dataGridView3[2, locrow].Value.ToString();
                                }
                            }
                        }
                    }


                    break;
                case 11:
                    //MOVWF


                    binarystring = binarystring.Substring(7);
                    loc = BinaryStringToHexString(binarystring);
                    if (loc == "00h")
                    {
                        loc = BinaryStringToHexString(dataGridView2[2, 5].Value.ToString());
                    }
                    if (dataGridView2[2, 4].Value.ToString().Substring(2, 1) == "1" && loc.StartsWith("0")) loc = "8" + loc.Substring(1);
                    bool exist = false;
                    int locrow1 = 1234567;
                    for (int i = 0; dataGridView4.Rows.Count > i; i++)
                    {
                        if (dataGridView4[1, i].Value.ToString() == loc)
                        {
                            locrow1 = i;
                            exist = true;
                        }
                    }
                    if (exist)
                    {
                        dataGridView4[2, locrow1].Value = dataGridView2[2, 0].Value.ToString();
                    }
                    else
                    {
                        dataGridView4.Rows.Add("", loc, dataGridView2[2, 0].Value.ToString());
                    }
                    for (int i = 0; i < dataGridView3.RowCount; i++)
                    {
                        string a = dataGridView4[1, dataGridView4.RowCount - 1].Value.ToString();
                        string c = dataGridView3[1, i].Value.ToString(); ;
                        if (dataGridView4[1, dataGridView4.RowCount - 1].Value.ToString() == dataGridView3[1, i].Value.ToString())
                        {
                            dataGridView3[2, i].Value = dataGridView4[2, dataGridView4.RowCount - 1].Value;
                        }
                    }

                    break;
                case 12:
                    //NOP
                    break;
                case 13:
                    //RLF

                    d = binarystring.Substring(6, 1);
                    binarystring = binarystring.Substring(7);
                    loc = BinaryStringToHexString(binarystring);
                    if (loc == "00h")
                    {
                        loc = BinaryStringToHexString(dataGridView2[2, 5].Value.ToString());
                    }
                    if (dataGridView2[2, 4].Value.ToString().Substring(2, 1) == "1" && loc.StartsWith("0")) loc = "8" + loc.Substring(1);
                    locrow = 123456;
                    for (int i = 0; dataGridView4.Rows.Count > i; i++)
                    {
                        if (dataGridView4[1, i].Value.ToString() == loc)
                        {
                            locrow = i;
                        }
                    }
                    string shift = dataGridView4[2, locrow].Value.ToString() + carry.ToString();
                    carry = Convert.ToInt32(shift.Substring(0, 1));
                    shift = shift.Substring(1);

                    if (shift == "00000000") zero = 1;
                    else zero = 0;


                    if (d == "1") dataGridView4[2, locrow].Value = shift;
                    if (d == "0")
                    {
                        dataGridView2[2, 0].Value = shift;
                        dataGridView3[2, 0].Value = shift;
                    }

                    break;
                case 14:
                    //RRF     
                    d = binarystring.Substring(6, 1);
                    binarystring = binarystring.Substring(7);
                    loc = BinaryStringToHexString(binarystring);
                    if (loc == "00h")
                    {
                        loc = BinaryStringToHexString(dataGridView2[2, 5].Value.ToString());
                    }
                    if (dataGridView2[2, 4].Value.ToString().Substring(2, 1) == "1" && loc.StartsWith("0")) loc = "8" + loc.Substring(1);
                    locrow = 123456;
                    for (int i = 0; dataGridView4.Rows.Count > i; i++)
                    {
                        if (dataGridView4[1, i].Value.ToString() == loc)
                        {
                            locrow = i;
                        }
                    }
                    shift = carry.ToString() + dataGridView4[2, locrow].Value.ToString();
                    carry = Convert.ToInt32(shift.Substring(8, 1));
                    shift = shift.Substring(0, 8);
                    if (shift == "00000000") zero = 1;
                    else zero = 0;


                    if (d == "1") dataGridView4[2, locrow].Value = shift;
                    if (d == "0")
                    {
                        dataGridView2[2, 0].Value = shift;
                        dataGridView3[2, 0].Value = shift;
                    }



                    break;
                case 15:
                    //SUBWF

                    d = binarystring.Substring(6, 1);
                    binarystring = binarystring.Substring(7);
                    loc = BinaryStringToHexString(binarystring);
                    if (loc == "00h")
                    {
                        loc = BinaryStringToHexString(dataGridView2[2, 5].Value.ToString());
                    }
                    if (dataGridView2[2, 4].Value.ToString().Substring(2, 1) == "1" && loc.StartsWith("0")) loc = "8" + loc.Substring(1);
                    locrow = 123456;
                    for (int i = 0; dataGridView4.Rows.Count > i; i++)
                    {
                        if (dataGridView4[1, i].Value.ToString() == loc)
                        {
                            locrow = i;
                        }
                    }
                    int sublw = bin2dec(dataGridView4[2, locrow].Value.ToString()) - bin2dec(dataGridView2[2, 0].Value.ToString());

                    if (sublw < 0)
                    {
                        carry = 0;
                        sublw = 256 + sublw;
                    }
                    else carry = 1;

                    w = dataGridView2[2, 0].Value.ToString();
                    sf = dataGridView4[2, locrow].Value.ToString();
                    for (int i = 128; bin2dec(w) > 15 || bin2dec(sf) > 15; i = i / 2)
                    {
                        if (bin2dec(w) >= i) w = dec2bin(bin2dec(w) - i);
                        if (bin2dec(sf) >= i) sf = dec2bin(bin2dec(sf) - i);
                    }
                    dc = bin2dec(sf) - bin2dec(w);
                    if (dc < 0) digitcarry = 0;
                    else digitcarry = 1;

                    if (sublw == 0) zero = 1;
                    else zero = 0;
                    if (d == "1") dataGridView4[2, locrow].Value = dec2bin(sublw);
                    if (d == "0")
                    {
                        dataGridView2[2, 0].Value = dec2bin(sublw);
                        dataGridView3[2, 0].Value = dec2bin(sublw);
                    }


                    break;
                case 16:
                    //SWAPF
                    d = binarystring.Substring(6, 1);
                    binarystring = binarystring.Substring(7);
                    loc = BinaryStringToHexString(binarystring);
                    if (loc == "00h")
                    {
                        loc = BinaryStringToHexString(dataGridView2[2, 5].Value.ToString());
                    }
                    if (dataGridView2[2, 4].Value.ToString().Substring(2, 1) == "1" && loc.StartsWith("0")) loc = "8" + loc.Substring(1);
                    locrow = 123456;
                    for (int i = 0; dataGridView4.Rows.Count > i; i++)
                    {
                        if (dataGridView4[1, i].Value.ToString() == loc)
                        {
                            locrow = i;
                        }
                    }
                    string swap = dataGridView4[2, locrow].Value.ToString().Substring(4) + dataGridView4[2, locrow].Value.ToString().Substring(0, 4);



                    if (d == "1") dataGridView4[2, locrow].Value = swap;
                    if (d == "0")
                    {
                        dataGridView2[2, 0].Value = swap;
                        dataGridView3[2, 0].Value = swap;
                    }

                    break;
                case 17:
                    //XORWF

                    d = binarystring.Substring(6, 1);
                    binarystring = binarystring.Substring(7);
                    loc = BinaryStringToHexString(binarystring);
                    if (loc == "00h")
                    {
                        loc = BinaryStringToHexString(dataGridView2[2, 5].Value.ToString());
                    }
                    if (dataGridView2[2, 4].Value.ToString().Substring(2, 1) == "1" && loc.StartsWith("0")) loc = "8" + loc.Substring(1);
                    locrow = 123456;
                    for (int i = 0; dataGridView4.Rows.Count > i; i++)
                    {
                        if (dataGridView4[1, i].Value.ToString() == loc)
                        {
                            locrow = i;
                        }
                    }
                    string w41 = dataGridView2[2, 0].Value.ToString();
                    string equ31 = "";
                    for (int i = 7; i >= 0; i--)
                    {
                        if ((dataGridView4[2, locrow].Value.ToString().Substring(i, 1) == "1" && w41.Substring(i, 1) == "0") || (w41.Substring(i, 1) == "1" && dataGridView4[2, locrow].Value.ToString().Substring(i, 1) == "0"))
                        {
                            equ31 = "1" + equ31;
                        }
                        else
                        {
                            equ31 = "0" + equ31;
                        }
                    }
                    if (equ31 == "00000000") zero = 1;
                    else zero = 0;

                    if (d == "1") dataGridView4[2, locrow].Value = equ31;
                    if (d == "0")
                    {
                        dataGridView2[2, 0].Value = equ31;
                        dataGridView3[2, 0].Value = equ31;
                    }

                    break;
                case 18:
                    //BCF


                    string bcf;
                    int b = bin2dec(binarystring.Substring(4, 3));
                    binarystring = binarystring.Substring(7);
                    loc = BinaryStringToHexString(binarystring);

                    if (loc == "00h")
                    {
                        loc = BinaryStringToHexString(dataGridView2[2, 5].Value.ToString());
                    }
                    if (dataGridView2[2, 4].Value.ToString().Substring(2, 1) == "1" && loc.StartsWith("0")) loc = "8" + loc.Substring(1);
                    locrow = 1234567;
                    for (int i = 0; dataGridView4.Rows.Count > i; i++)
                    {
                        if (dataGridView4[1, i].Value.ToString() == loc)
                        {
                            locrow = i;
                        }

                    }
                    if (locrow != 1234567)
                    {
                        b = 7 - b;
                        bcf = "";
                        if (b > 0 && b < 7)
                        {
                            bcf = dataGridView4[2, locrow].Value.ToString().Substring(0, b) + "0" + dataGridView4[2, locrow].Value.ToString().Substring(b + 1);
                        }
                        else
                        {
                            if (b == 0)
                                bcf = "0" + dataGridView4[2, locrow].Value.ToString().Substring(b + 1);
                            else bcf = bcf = dataGridView4[2, locrow].Value.ToString().Substring(0, b) + "0";
                        }
                        if (bcf == "00000000") zero = 1;
                        else zero = 0;
                        dataGridView4[2, locrow].Value = bcf;
                    }
                    else
                    {
                        for (int i = 0; dataGridView3.Rows.Count > i; i++)
                        {
                            if (dataGridView3[1, i].Value.ToString() == loc)
                            {
                                locrow = i;
                            }

                        }
                        if (locrow != 1234567)
                        {
                            b = 7 - b;
                            bcf = "";
                            string s = dataGridView3[2, locrow].Value.ToString();
                            if (b > 0 && b < 7)
                            {
                                bcf = dataGridView3[2, locrow].Value.ToString().Substring(0, b) + "0" + dataGridView3[2, locrow].Value.ToString().Substring(b + 1);
                            }
                            else
                            {
                                if (b == 0)
                                    bcf = "0" + dataGridView3[2, locrow].Value.ToString().Substring(b + 1);
                                else bcf = bcf = dataGridView3[2, locrow].Value.ToString().Substring(0, b) + "0";
                            }
                            if (bcf == "00000000") zero = 1;
                            else zero = 0;
                            dataGridView3[2, locrow].Value = bcf;
                            if (dataGridView3[2,4].Value.ToString() == bcf)
                            {
                                dataGridView2[2, 4].Value = dataGridView3[2, 4].Value.ToString();
                                for (int i = 0; i < dataGridView4.RowCount; i++)
                                {
                                    int testrow = i;
                                    for (j = 0; j < dataGridView3.RowCount; j++)
                                    {
                                        if (dataGridView3[1, j].Value.ToString() == dataGridView4[1, i].Value.ToString())
                                        {
                                            dataGridView4[2,i].Value = dataGridView3[2, j].Value.ToString();
                                        }
                                        if (dataGridView2[1, j].Value.ToString() == dataGridView4[1, i].Value.ToString())
                                        {
                                            dataGridView4[2, i].Value = dataGridView2[2, j].Value.ToString();
                                        }
                                    }
                                }

                            }
                        }
                        else
                        {
                            for (int i = 0; dataGridView3.Rows.Count > i; i++)
                            {
                                if (dataGridView2[1, i].Value.ToString() == loc)
                                {
                                    locrow = i;
                                }

                            }
                            if (locrow != 1234567)
                            {
                                b = 7 - b;
                                bcf = "";
                                string s = dataGridView2[2, locrow].Value.ToString();
                                if (b > 0 && b < 7)
                                {
                                    bcf = dataGridView2[2, locrow].Value.ToString().Substring(0, b) + "0" + dataGridView2[2, locrow].Value.ToString().Substring(b + 1);
                                }
                                else
                                {
                                    if (b == 0)
                                        bcf = "0" + dataGridView2[2, locrow].Value.ToString().Substring(b + 1);
                                    else bcf = bcf = dataGridView2[2, locrow].Value.ToString().Substring(0, b) + "0";
                                }
                                if (bcf == "00000000") zero = 1;
                                else zero = 0;
                                dataGridView2[2, locrow].Value = bcf;
                                for (int i = 0; i < dataGridView4.RowCount; i++)
                                {
                                    int testrow = i;
                                    for (j = 0; j < dataGridView3.RowCount; j++)
                                    {
                                        if (dataGridView3[1, j].Value.ToString() == dataGridView4[1, i].Value.ToString())
                                        {
                                            dataGridView4[2, j].Value = dataGridView3[2, i].Value.ToString();
                                        }
                                    }
                                }
                            }
                        }
                    }
                         
                       
                   

                    break;
                case 19:
                    //BSF

                    string bsf;
                    b = bin2dec(binarystring.Substring(4, 3));
                    binarystring = binarystring.Substring(7);
                    loc = BinaryStringToHexString(binarystring);

                    if (loc == "00h")
                    {
                        loc = BinaryStringToHexString(dataGridView2[2, 5].Value.ToString());
                    }
                    if (dataGridView2[2, 4].Value.ToString().Substring(2, 1) == "1" && loc.StartsWith("0")) loc = "8" + loc.Substring(1);
                    locrow = 1234567;
                    for (int i = 0; dataGridView4.Rows.Count > i; i++)
                    {
                        if (dataGridView4[1, i].Value.ToString() == loc)
                        {
                            locrow = i;
                        }

                    }
                    if (locrow != 1234567)
                    {
                        b = 7 - b;
                        bsf = "";
                        string s = dataGridView4[2, locrow].Value.ToString();
                        if (b > 0 && b < 7)
                        {
                            bsf = dataGridView4[2, locrow].Value.ToString().Substring(0, b) + "1" + dataGridView4[2, locrow].Value.ToString().Substring(b + 1);
                        }
                        else
                        {
                            if (b == 0)
                                bsf = "1" + dataGridView4[2, locrow].Value.ToString().Substring(b + 1);
                            else bsf = bsf = dataGridView4[2, locrow].Value.ToString().Substring(0, b) + "1";
                        }
                        if (bsf == "00000000") zero = 1;
                        else zero = 0;
                        dataGridView4[2, locrow].Value = bsf;
                    }
                    else
                    {
                        for (int i = 0; dataGridView3.Rows.Count > i; i++)
                        {
                            if (dataGridView3[1, i].Value.ToString() == loc)
                            {
                                locrow = i;
                            }

                        }
                        if (locrow != 1234567)
                        {
                            b = 7 - b;
                            bsf = "";
                            string s = dataGridView3[2, locrow].Value.ToString();
                            if (b > 0 && b < 7)
                            {
                                bsf = dataGridView3[2, locrow].Value.ToString().Substring(0, b) + "1" + dataGridView3[2, locrow].Value.ToString().Substring(b + 1);
                            }
                            else
                            {
                                if (b == 0)
                                    bsf = "1" + dataGridView3[2, locrow].Value.ToString().Substring(b + 1);
                                else bsf = bsf = dataGridView3[2, locrow].Value.ToString().Substring(0, b) + "1";
                            }
                            if (bsf == "00000000") zero = 1;
                            else zero = 0;
                            dataGridView3[2, locrow].Value = bsf;
                            if (dataGridView3[2, 4].Value.ToString() == bsf)
                            {
                                dataGridView2[2, 4].Value = dataGridView3[2, 4].Value.ToString();
                                for (int i = 0; i < dataGridView4.RowCount; i++)
                                {
                                    int testrow = i;
                                    for (j = 0; j < dataGridView3.RowCount; j++)
                                    {
                                        if (dataGridView3[1, j].Value.ToString() == dataGridView4[1, i].Value.ToString())
                                        {
                                            dataGridView4[2, i].Value = dataGridView3[2, j].Value.ToString();
                                        }
                                        if (dataGridView2[1, j].Value.ToString() == dataGridView4[1, i].Value.ToString())
                                        {
                                            dataGridView4[2, i].Value = dataGridView2[2, j].Value.ToString();
                                        }
                                    }
                                }

                            }
                        }
                        else
                        {
                            for (int i = 0; dataGridView3.Rows.Count > i; i++)
                            {
                                if (dataGridView2[1, i].Value.ToString() == loc)
                                {
                                    locrow = i;
                                }

                            }
                            if (locrow != 1234567)
                            {
                                b = 7 - b;
                                bsf = "";
                                string s = dataGridView2[2, locrow].Value.ToString();
                                if (b > 0 && b < 7)
                                {
                                    bsf = dataGridView2[2, locrow].Value.ToString().Substring(0, b) + "1" + dataGridView2[2, locrow].Value.ToString().Substring(b + 1);
                                }
                                else
                                {
                                    if (b == 0)
                                        bsf = "1" + dataGridView2[2, locrow].Value.ToString().Substring(b + 1);
                                    else bsf = bsf = dataGridView2[2, locrow].Value.ToString().Substring(0, b) + "1";
                                }
                                if (bsf == "00000000") zero = 1;
                                else zero = 0;
                                dataGridView2[2, locrow].Value = bsf;
                                for (int i = 0; i < dataGridView4.RowCount; i++)
                                {
                                    int testrow = i;
                                    for (j=0;j<dataGridView2.RowCount;j++)
                                    {
                                        if (dataGridView2[1,j].Value.ToString() == dataGridView4[1,i].Value.ToString())
                                        {
                                            dataGridView4[2, j].Value = dataGridView2[2, i].Value.ToString();
                                        }
                                    }
                                }

                            }
                        }
                    }

                    break;
                case 20:
                    //BTFSC


                    b = bin2dec(binarystring.Substring(4, 3));
                    binarystring = binarystring.Substring(7);
                    loc = BinaryStringToHexString(binarystring);
                    if (loc == "00h")
                    {
                        loc = BinaryStringToHexString(dataGridView2[2, 5].Value.ToString());
                    }
                    if (dataGridView2[2, 4].Value.ToString().Substring(2, 1) == "1" && loc.StartsWith("0")) loc = "8" + loc.Substring(1);
                    locrow = 1234567;
                    for (int i = 0; dataGridView4.Rows.Count > i; i++)
                    {
                        if (dataGridView4[1, i].Value.ToString() == loc)
                        {
                            locrow = i;
                        }
                    }
                    b = 7 - b;
                    if (dataGridView4[2, locrow].Value.ToString().Substring(b, 1) == "0")
                    {
                        befehlnr++;
                        TimerWertbefehl++;
                        dataGridView1.CurrentRow.DefaultCellStyle.BackColor = Color.White;
                        dataGridView1.CurrentCell = dataGridView1[0, dataGridView1.CurrentRow.Index + 1];
                        //dataGridView1.Rows[j - 1].DefaultCellStyle.BackColor = Color.White;
                    }
                    break;
                case 22:
                    //ADDLW

                    binarystring = binarystring.Substring(6);
                    string w3 = dataGridView2[2, 0].Value.ToString();
                    int addlw = bin2dec(binarystring) + bin2dec(w3);
                    if (addlw > 255)
                    {
                        addlw = addlw - 256;
                        carry = 1;
                    }
                    else carry = 0;

                    w = dataGridView2[2, 0].Value.ToString();
                    sf = binarystring;
                    for (int i = 128; bin2dec(w) > 15 || bin2dec(sf) > 15; i = i / 2)
                    {
                        if (bin2dec(w) >= i) w = dec2bin(bin2dec(w) - i);
                        if (bin2dec(sf) >= i) sf = dec2bin(bin2dec(sf) - i);
                    }
                    dc = bin2dec(sf) + bin2dec(w);
                    if (dc > 15) digitcarry = 1;
                    else digitcarry = 0;

                    if (addlw == 0) zero = 1;
                    else zero = 0;
                    dataGridView2[2, 0].Value = dec2bin(addlw);
                    dataGridView3[2, 0].Value = dec2bin(addlw);

                    break;
                case 23:
                    //ANDLW

                    binarystring = binarystring.Substring(6);
                    string w1 = dataGridView2[2, 0].Value.ToString();
                    equ = "";
                    for (int i = 7; i >= 0; i--)
                    {
                        if (binarystring.Substring(i, 1) == "1" && w1.Substring(i, 1) == "1")
                        {
                            equ = "1" + equ;
                        }
                        else
                        {
                            equ = "0" + equ;
                        }
                    }
                    if (equ == "00000000") zero = 1;
                    else zero = 0;
                    dataGridView2[2, 0].Value = equ;
                    dataGridView3[2, 0].Value = equ;
                    break;

                case 24:
                    //CALL
                    befehlnr++;
                    TimerWertbefehl++;
                    binarystring = binarystring.Substring(3);

                    string addressc = BinaryStringToHexString(binarystring);
                    int backrow = row;
                    string call = dataGridView1.Rows[row].Cells[3].Value.ToString().Substring(5);

                    for (int m = 0; m < dataGridView1.RowCount; m++)
                    {
                        string inxsdufjkm = dataGridView1.Rows[m].Cells[1].Value.ToString();
                        if (!inxsdufjkm.EndsWith("h")) inxsdufjkm = inxsdufjkm + "h";
                        if (inxsdufjkm == addressc)
                        {
                            rownr = m;
                        }
                    }

                    //  dataGridView1.Rows[row].DefaultCellStyle.BackColor = Color.White;
                    returnrow = row;
                    textBox5.Text = dataGridView1[1, row].Value.ToString();
                    return rownr -1;

                    break;

                case 25:
                    //CLRWDT

                    to = 1;
                    pd = 1;

                    break;
                case 26:
                    //GOTO
                    befehlnr++; TimerWertbefehl++;
                    binarystring = binarystring.Substring(3);

                    string address = BinaryStringToHexString(binarystring);


                    for (int m = 0; m < dataGridView1.RowCount; m++)
                    {
                        string inxsdufjkm = dataGridView1.Rows[m].Cells[1].Value.ToString();
                        if (!inxsdufjkm.EndsWith("h")) inxsdufjkm = inxsdufjkm + "h";
                        if (inxsdufjkm == address)
                        {
                            rownr = m;
                        }

                    }

                   // dataGridView1.Rows[row].DefaultCellStyle.BackColor = Color.White;
                    return rownr - 1;

                    break;
                case 27:
                    //IORLW

                    binarystring = binarystring.Substring(6);
                    string w2 = dataGridView2[2, 0].Value.ToString();
                    string equ2 = "";
                    for (int i = 7; i >= 0; i--)
                    {
                        if (binarystring.Substring(i, 1) == "1" || w2.Substring(i, 1) == "1")
                        {
                            equ2 = "1" + equ2;
                        }
                        else
                        {
                            equ2 = "0" + equ2;
                        }
                    }
                    if (equ2 == "00000000") zero = 1;
                    else zero = 0;
                    dataGridView2[2, 0].Value = equ2;
                    dataGridView3[2, 0].Value = equ2;

                    break;
                case 28:
                    //MOVLW

                    binarystring = binarystring.Substring(6);
                    dataGridView2[2, 0].Value = binarystring;
                    dataGridView3[2, 0].Value = binarystring;

                    break;
                case 29:
                    //RETFIE
                    befehlnr++;
                    TimerWertbefehl++;
                    dataGridView2[2, 11].Value = "1" + dataGridView2[2, 11].Value.ToString().Substring(1);

                    break;
                case 30:
                    //RETLW
                    befehlnr++;
                    TimerWertbefehl++;
                    binarystring = binarystring.Substring(6);
                    dataGridView2[2, 0].Value = binarystring;
                    dataGridView3[2, 0].Value = binarystring;
                    int sendreturnrow = returnrow + 1;
                    returnrow = 1234567;
                    textBox5.Text = "";
                    return sendreturnrow;

                    break;
                case 31:
                    //RETURN
                    befehlnr++;
                    TimerWertbefehl++;
                    sendreturnrow = returnrow + 1;
                    returnrow = 1234567;
                    textBox5.Text = "";
                    return sendreturnrow;
                    break;
                case 32:
                    //SLEEP

                    to = 1;
                    pd = 0;

                    break;
                case 33:
                    //SUBLW

                    binarystring = binarystring.Substring(6);
                    string w5 = dataGridView2[2, 0].Value.ToString();
                    sublw = bin2dec(binarystring) - bin2dec(w5);

                    if (sublw < 0)
                    {
                        carry = 0;
                        sublw = 256 + sublw;
                    }
                    else carry = 1;

                    w = dataGridView2[2, 0].Value.ToString();
                    sf = binarystring;
                    for (int i = 128; bin2dec(w) > 15 || bin2dec(sf) > 15; i = i / 2)
                    {
                        if (bin2dec(w) >= i) w = dec2bin(bin2dec(w) - i);
                        if (bin2dec(sf) >= i) sf = dec2bin(bin2dec(sf) - i);
                    }
                    dc = bin2dec(sf) - bin2dec(w);
                    if (dc < 0) digitcarry = 0;
                    else digitcarry = 1;

                    if (sublw == 0) zero = 1;
                    else zero = 0;

                    dataGridView2[2, 0].Value = dec2bin(sublw);
                    dataGridView3[2, 0].Value = dec2bin(sublw);
                    //l-w=w


                    break;
                case 34:
                    //XORLW

                    binarystring = binarystring.Substring(6);
                    string w4 = dataGridView2[2, 0].Value.ToString();
                    string equ3 = "";
                    for (int i = 7; i >= 0; i--)
                    {
                        if ((binarystring.Substring(i, 1) == "1" && w4.Substring(i, 1) == "0") || (w4.Substring(i, 1) == "1" && binarystring.Substring(i, 1) == "0"))
                        {
                            equ3 = "1" + equ3;
                        }
                        else
                        {
                            equ3 = "0" + equ3;
                        }
                    }
                    if (equ3 == "00000000") zero = 1;
                    else zero = 0;
                    dataGridView2[2, 0].Value = equ3;
                    dataGridView3[2, 0].Value = equ3;

                    break;

                case 21:
                    //BTFSS

                    b = bin2dec(binarystring.Substring(4, 3));
                    binarystring = binarystring.Substring(7);
                    loc = BinaryStringToHexString(binarystring);
                    if (loc == "00h")
                    {
                        loc = BinaryStringToHexString(dataGridView2[2, 5].Value.ToString());
                    }
                    if (dataGridView2[2, 4].Value.ToString().Substring(2, 1) == "1" && loc.StartsWith("0")) loc = "8" + loc.Substring(1);
                    locrow = 1234567;
                    for (int i = 0; dataGridView4.Rows.Count > i; i++)
                    {
                        if (dataGridView4[1, i].Value.ToString() == loc)
                        {
                            locrow = i;
                        }
                    }
                    b = 7 - b;
                    if (locrow != 1234567)
                    {
                        if (dataGridView4[2, locrow].Value.ToString().Substring(b, 1) == "1")
                        {
                            befehlnr++;
                            TimerWertbefehl++;
                            dataGridView1.CurrentRow.DefaultCellStyle.BackColor = Color.White;
                            dataGridView1.CurrentCell = dataGridView1[0, dataGridView1.CurrentRow.Index + 1];
                        }
                    }
                    else
                    {
                        for (int i = 0; dataGridView2.Rows.Count > i; i++)
                        {
                            if (dataGridView2[1, i].Value.ToString() == loc)
                            {
                                locrow = i;
                            }
                        }
                        if (locrow != 1234567)
                        {
                            if (dataGridView2[2, locrow].Value.ToString().Substring(b, 1) == "1")
                            {
                                befehlnr++;
                                TimerWertbefehl++;
                                dataGridView1.CurrentRow.DefaultCellStyle.BackColor = Color.White;
                                dataGridView1.CurrentCell = dataGridView1[0, dataGridView1.CurrentRow.Index + 1];
                            }
                        }
                        else
                        {
                            for (int i = 0; dataGridView3.Rows.Count > i; i++)
                            {
                                if (dataGridView3[1, i].Value.ToString() == loc)
                                {
                                    locrow = i;
                                }
                            }
                            if (locrow != 1234567)
                            {
                                if (dataGridView3[2, locrow].Value.ToString().Substring(b, 1) == "1")
                                {
                                    befehlnr++;
                                    TimerWertbefehl++;
                                    dataGridView1.CurrentRow.DefaultCellStyle.BackColor = Color.White;
                                    dataGridView1.CurrentCell = dataGridView1[0, dataGridView1.CurrentRow.Index + 1];
                                }
                            }
                        }
                    }
                    
                    break;

                default:
                    break;
            }
            clcktimer();
            for(int i = 0;i<dataGridView4.RowCount;i++)
            {
                if (dataGridView4[1,i].Value.ToString() == dataGridView2[1,2].Value.ToString())
                {
                    dataGridView4[2, i].Value = dataGridView2[2, 2].Value.ToString();
                }
            }

            for (int i = 0; i < dataGridView4.RowCount; i++)
            {
                string locs = dataGridView4[1, i].Value.ToString();
                for (int j = 0; j < dataGridView2.RowCount; j++)
                {
                    if (dataGridView2[1, j].Value.ToString() == locs)
                    {
                        dataGridView2[2, j].Value = dataGridView4[2, i].Value.ToString();
                    }
                }
            }
            //set input/output
            setinputoutput();

            
            if (rbie == 1)
            {
                dataGridView2[2, 11].Value = dataGridView2[2, 11].Value.ToString().Substring(0, 7) + "1";
                dataGridView3[2, 11].Value = dataGridView2[2, 11].Value.ToString();
            }
            else
            {
                dataGridView2[2, 11].Value = dataGridView2[2, 11].Value.ToString().Substring(0, 7) + "0";
                dataGridView3[2, 11].Value = dataGridView2[2, 11].Value.ToString();
            }
            if (dataGridView2[2,7].Value.ToString().Substring(0,1) == "1")
            {
                dataGridView2[2, 11].Value = dataGridView2[2, 11].Value.ToString().Substring(0, 6) + "1" + dataGridView2[2,11].Value.ToString().Substring(7);
                dataGridView3[2, 11].Value = dataGridView2[2, 11].Value.ToString();
            }
            else
            {
                dataGridView2[2, 11].Value = dataGridView2[2, 11].Value.ToString().Substring(0, 6) + "0" + dataGridView2[2, 11].Value.ToString().Substring(7);
                dataGridView3[2, 11].Value = dataGridView2[2, 11].Value.ToString();
            }

            //setze Status register
            setstatusreg();

            for (int i = 0; i<dataGridView4.RowCount;i++)
            {
                if(dataGridView4[1,i].Value.ToString() == dataGridView2[1,4].Value.ToString())
                {
                    dataGridView4[2, i].Value = dataGridView2[2, 4].Value;
                }
                if (dataGridView4[1, i].Value.ToString() == dataGridView3[1, 4].Value.ToString())
                {
                    dataGridView4[2, i].Value = dataGridView3[2, 4].Value;
                }
            }
            //Frage Bits im Interrupt Register ab
            testifinterrupt();

            return 1234567;
        }

        private void clcktimer()
        {
            String Prescaler_bits = dataGridView3[2, 2].Value.ToString().Substring(5, 1) + dataGridView3[2, 2].Value.ToString().Substring(6, 1) + dataGridView3[2, 2].Value.ToString().Substring(7, 1);

            if (dataGridView3[2, 2].Value.ToString().Substring(2, 1) == "0")
            {
                if (dataGridView3[2, 2].Value.ToString().Substring(4, 1) == "1")
                {

                    switch (Prescaler_bits)
                    {
                        case "000":
                            //1:1
                            if (TimerWertbefehl > 255)
                            {
                                TimerWertbefehl = TimerWertbefehl - 256;
                                dataGridView2[2, 11].Value = dataGridView2[2, 11].Value.ToString().Substring(0, 5) + "1" + dataGridView2[2, 11].Value.ToString().Substring(6);
                            }
                            dataGridView2[2, 2].Value = dec2bin(TimerWertbefehl);


                            break;

                        case "001":
                            //1:2
                        
                            if (TimerWertbefehl % 2 == 0)
                            {
                                Ausgabewert = Ausgabewert + 1;
                                if (Ausgabewert > 255)
                                {
                                    Ausgabewert = 0;
                                    dataGridView2[2, 11].Value = dataGridView2[2, 11].Value.ToString().Substring(0, 5) + "1" + dataGridView2[2, 11].Value.ToString().Substring(6);
                                }
                                dataGridView2[2, 2].Value = dec2bin(Ausgabewert);
                            }
                            break;

                        case "010":
                            //1:4
                            if (TimerWertbefehl % 4 == 0)
                            {
                                Ausgabewert = Ausgabewert + 1;
                                if (Ausgabewert > 255)
                                {
                                    Ausgabewert = 0;
                                    dataGridView2[2, 11].Value = dataGridView2[2, 11].Value.ToString().Substring(0, 5) + "1" + dataGridView2[2, 11].Value.ToString().Substring(6);
                                }
                                dataGridView2[2, 2].Value = dec2bin(Ausgabewert);
                            }
                            break;

                        case "011":
                            //1:8
                            if (TimerWertbefehl % 8 == 0)
                            {
                                Ausgabewert = Ausgabewert + 1;
                                if (Ausgabewert > 255)
                                {
                                    Ausgabewert = 0;
                                    dataGridView2[2, 11].Value = dataGridView2[2, 11].Value.ToString().Substring(0, 5) + "1" + dataGridView2[2, 11].Value.ToString().Substring(6);
                                }
                                dataGridView2[2, 2].Value = dec2bin(Ausgabewert);
                            }
                            break;

                        case "100":
                            //1:16
                            if (TimerWertbefehl % 16 == 0)
                            {
                                Ausgabewert = Ausgabewert + 1;
                                if (Ausgabewert > 255)
                                {
                                    Ausgabewert = 0;
                                    dataGridView2[2, 11].Value = dataGridView2[2, 11].Value.ToString().Substring(0, 5) + "1" + dataGridView2[2, 11].Value.ToString().Substring(6);
                                }
                                dataGridView2[2, 2].Value = dec2bin(Ausgabewert);
                            }
                            break;

                        case "101":
                            //1:32
                            if (TimerWertbefehl % 32 == 0)
                            {
                                Ausgabewert = Ausgabewert + 1;
                                if (Ausgabewert > 255)
                                {
                                    Ausgabewert = 0;
                                    dataGridView2[2, 11].Value = dataGridView2[2, 11].Value.ToString().Substring(0, 5) + "1" + dataGridView2[2, 11].Value.ToString().Substring(6);
                                }
                                dataGridView2[2, 2].Value = dec2bin(Ausgabewert);
                            }
                            break;

                        case "110":
                            //1:64
                            if (TimerWertbefehl % 64 == 0)
                            {
                                Ausgabewert = Ausgabewert + 1;
                                if (Ausgabewert > 255)
                                {
                                    Ausgabewert = 0;
                                    dataGridView2[2, 11].Value = dataGridView2[2, 11].Value.ToString().Substring(0, 5) + "1" + dataGridView2[2, 11].Value.ToString().Substring(6);
                                }
                                dataGridView2[2, 2].Value = dec2bin(Ausgabewert);
                            }
                            break;

                        case "111":
                            //1:128
                            if (TimerWertbefehl % 128 == 0)
                            {
                                Ausgabewert = Ausgabewert + 1;
                                if (Ausgabewert > 255)
                                {
                                    Ausgabewert = 0;
                                    dataGridView2[2, 11].Value = dataGridView2[2, 11].Value.ToString().Substring(0, 5) + "1" + dataGridView2[2, 11].Value.ToString().Substring(6);
                                }
                                dataGridView2[2, 2].Value = dec2bin(Ausgabewert);
                            }
                            break;
                    }
                }//Wenn PSA BIT 0= Timer
                else if (dataGridView3[2, 2].Value.ToString().Substring(4, 1) == "0")
                {
                    switch (Prescaler_bits)
                    {
                        case "000":
                            //1:2
                            if (TimerWertbefehl % 2 == 0)
                            {
                                Ausgabewert = Ausgabewert + 1;
                                if (Ausgabewert > 255)
                                {
                                    Ausgabewert = 0;
                                    dataGridView2[2, 11].Value = dataGridView2[2, 11].Value.ToString().Substring(0, 5) + "1" + dataGridView2[2, 11].Value.ToString().Substring(6);
                                }
                                dataGridView2[2, 2].Value = dec2bin(Ausgabewert);
                            }
                            break;

                        case "001":
                            //1:4
                            if (TimerWertbefehl % 4 == 0)
                            {
                                Ausgabewert = Ausgabewert + 1;
                                if (Ausgabewert > 255)
                                {
                                    Ausgabewert = 0;
                                    dataGridView2[2, 11].Value = dataGridView2[2, 11].Value.ToString().Substring(0, 5) + "1" + dataGridView2[2, 11].Value.ToString().Substring(6);
                                }
                                dataGridView2[2, 2].Value = dec2bin(Ausgabewert);
                            }
                            break;

                        case "010":
                            //1:8
                            if (TimerWertbefehl % 8 == 0)
                            {
                                Ausgabewert = Ausgabewert + 1;
                                if (Ausgabewert > 255)
                                {
                                    Ausgabewert = 0;
                                    dataGridView2[2, 11].Value = dataGridView2[2, 11].Value.ToString().Substring(0, 5) + "1" + dataGridView2[2, 11].Value.ToString().Substring(6);
                                }
                                dataGridView2[2, 2].Value = dec2bin(Ausgabewert);
                            }
                            break;

                        case "011":
                            //1:16
                            if (TimerWertbefehl % 16 == 0)
                            {
                                Ausgabewert = Ausgabewert + 1;
                                if (Ausgabewert > 255)
                                {
                                    Ausgabewert = 0;
                                    dataGridView2[2, 11].Value = dataGridView2[2, 11].Value.ToString().Substring(0, 5) + "1" + dataGridView2[2, 11].Value.ToString().Substring(6);
                                }
                                dataGridView2[2, 2].Value = dec2bin(Ausgabewert);
                            }
                            break;

                        case "100":
                            //1:32
                            if (TimerWertbefehl % 32 == 0)
                            {
                                Ausgabewert = Ausgabewert + 1;
                                if (Ausgabewert > 255)
                                {
                                    Ausgabewert = 0;
                                    dataGridView2[2, 11].Value = dataGridView2[2, 11].Value.ToString().Substring(0, 5) + "1" + dataGridView2[2, 11].Value.ToString().Substring(6);
                                }
                                dataGridView2[2, 2].Value = dec2bin(Ausgabewert);
                            }
                            break;

                        case "101":
                            //1:64
                            if (TimerWertbefehl % 64 == 0)
                            {
                                Ausgabewert = Ausgabewert + 1;
                                if (Ausgabewert > 255)
                                {
                                    Ausgabewert = 0;
                                    dataGridView2[2, 11].Value = dataGridView2[2, 11].Value.ToString().Substring(0, 5) + "1" + dataGridView2[2, 11].Value.ToString().Substring(6);
                                }
                                dataGridView2[2, 2].Value = dec2bin(Ausgabewert);
                            }
                            break;

                        case "110":
                            //1:128
                            if (TimerWertbefehl % 128 == 0)
                            {
                                Ausgabewert = Ausgabewert + 1;
                                if (Ausgabewert > 255)
                                {
                                    Ausgabewert = 0;
                                    dataGridView2[2, 11].Value = dataGridView2[2, 11].Value.ToString().Substring(0, 5) + "1" + dataGridView2[2, 11].Value.ToString().Substring(6);
                                }
                                dataGridView2[2, 2].Value = dec2bin(Ausgabewert);
                            }
                            break;

                        case "111":
                            //1:256
                            if (TimerWertbefehl % 256 == 0)
                            {
                                Ausgabewert = Ausgabewert + 1;
                                if (Ausgabewert > 255)
                                {
                                    Ausgabewert = 0;
                                    dataGridView2[2, 11].Value = dataGridView2[2, 11].Value.ToString().Substring(0, 5) + "1" + dataGridView2[2, 11].Value.ToString().Substring(6);
                                }
                                dataGridView2[2, 2].Value = dec2bin(Ausgabewert);
                            }
                            break;
                    }
                }
            }
        }

        private void setinputoutput()
        {
            int button;

            for (int i = 0; i < 8; i++)
            {
                if (dataGridView3[2, 6].Value.ToString().Substring(i, 1) == "1")
                {
                    button = i + 1;
                    inputoutput(button, true);
                }
                if (dataGridView3[2, 6].Value.ToString().Substring(i, 1) == "0")
                {
                    button = i + 1;
                    inputoutput(button, false);
                }
                if (dataGridView3[2, 7].Value.ToString().Substring(i, 1) == "1")
                {
                    button = i + 9;
                    inputoutput(button, true);
                }
                if (dataGridView3[2, 7].Value.ToString().Substring(i, 1) == "0")
                {
                    button = i + 9;
                    inputoutput(button, false);
                }
            }
        }

        private void testifinterrupt()
        {
            if (dataGridView2[2, 11].Value.ToString().Substring(0, 1) == "1")
            {
                if (dataGridView2[2, 11].Value.ToString().Substring(1, 1) == "1")
                { interrupt(); }
                if (dataGridView2[2, 11].Value.ToString().Substring(2, 1) == "1" && dataGridView2[2, 11].Value.ToString().Substring(5, 1) == "1")
                { interrupt(); }
                if (dataGridView2[2, 11].Value.ToString().Substring(3, 1) == "1" && dataGridView2[2, 11].Value.ToString().Substring(6, 1) == "1")
                { interrupt(); }
                if (dataGridView2[2, 11].Value.ToString().Substring(4, 1) == "1" && dataGridView2[2, 11].Value.ToString().Substring(7, 1) == "1")
                { interrupt(); }
            }
        }

        private void setstatusreg()
        {
            if (digitcarry == 2 && zero != 2 && carry != 2) dataGridView2[2, 4].Value = dataGridView2[2, 4].Value.ToString().Substring(0, 3) + to + pd + zero + "x" + carry;
            if (digitcarry != 2 && zero == 2 && carry != 2) dataGridView2[2, 4].Value = dataGridView2[2, 4].Value.ToString().Substring(0, 3) + to + pd + "x" + digitcarry + carry;
            if (digitcarry != 2 && zero != 2 && carry == 2) dataGridView2[2, 4].Value = dataGridView2[2, 4].Value.ToString().Substring(0, 3) + to + pd + zero + digitcarry + "x";
            if (digitcarry == 2 && zero == 2 && carry != 2) dataGridView2[2, 4].Value = dataGridView2[2, 4].Value.ToString().Substring(0, 3) + to + pd + "x" + "x" + carry;
            if (digitcarry == 2 && zero != 2 && carry == 2) dataGridView2[2, 4].Value = dataGridView2[2, 4].Value.ToString().Substring(0, 3) + to + pd + zero + "x" + "x";
            if (digitcarry != 2 && zero == 2 && carry == 2) dataGridView2[2, 4].Value = dataGridView2[2, 4].Value.ToString().Substring(0, 3) + to + pd + "x" + digitcarry + "x";
            if (digitcarry == 2 && zero == 2 && carry == 2) dataGridView2[2, 4].Value = dataGridView2[2, 4].Value.ToString().Substring(0, 3) + to + pd + "x" + "x" + "x";
            if (digitcarry != 2 && zero != 2 && carry != 2) dataGridView2[2, 4].Value = dataGridView2[2, 4].Value.ToString().Substring(0, 3) + to + pd + zero + digitcarry + carry;
            dataGridView3[2, 4].Value = dataGridView2[2, 4].Value.ToString();
        }

        private void inputporta()
        {
            if (port1_7.Visible == true)
            {
                if (port1_7.BackColor == Color.Firebrick) dataGridView2[2, 6].Value = "1" + dataGridView2[2, 6].Value.ToString().Substring(1);
                if (port1_7.BackColor == Color.Transparent) dataGridView2[2, 6].Value = "0" + dataGridView2[2, 6].Value.ToString().Substring(1);
                if (port1_7.BackColor == Color.SteelBlue)
                {
                    if (dataGridView2[2, 6].Value.ToString().Substring(0, 1) == "0") dataGridView2[2, 6].Value = "1" + dataGridView2[2, 6].Value.ToString().Substring(1);
                    else dataGridView2[2, 6].Value = "0" + dataGridView2[2, 6].Value.ToString().Substring(1);
                }
            }
            if (port1_6.Visible == true)
            {
                if (port1_6.BackColor == Color.Firebrick) dataGridView2[2, 6].Value = dataGridView2[2, 6].Value.ToString().Substring(0, 1) + "1" + dataGridView2[2, 6].Value.ToString().Substring(2);
                if (port1_6.BackColor == Color.Transparent) dataGridView2[2, 6].Value = dataGridView2[2, 6].Value.ToString().Substring(0, 1) + "0" + dataGridView2[2, 6].Value.ToString().Substring(2);
                if (port1_6.BackColor == Color.SteelBlue)
                {
                    if (dataGridView2[2, 6].Value.ToString().Substring(1, 1) == "0") dataGridView2[2, 6].Value = dataGridView2[2, 6].Value.ToString().Substring(0, 1) + "1" + dataGridView2[2, 6].Value.ToString().Substring(2);
                    else dataGridView2[2, 6].Value = dataGridView2[2, 6].Value.ToString().Substring(0, 1) + "0" + dataGridView2[2, 6].Value.ToString().Substring(2);
                }
            }
            if (port1_5.Visible == true)
            {
                if (port1_5.BackColor == Color.Firebrick) dataGridView2[2, 6].Value = dataGridView2[2, 6].Value.ToString().Substring(0, 2) + "1" + dataGridView2[2, 6].Value.ToString().Substring(3);
                if (port1_5.BackColor == Color.Transparent) dataGridView2[2, 6].Value = dataGridView2[2, 6].Value.ToString().Substring(0, 2) + "0" + dataGridView2[2, 6].Value.ToString().Substring(3);
                if (port1_5.BackColor == Color.SteelBlue)
                {
                    if (dataGridView2[2, 6].Value.ToString().Substring(2, 1) == "0") dataGridView2[2, 6].Value = dataGridView2[2, 6].Value.ToString().Substring(0, 2) + "1" + dataGridView2[2, 6].Value.ToString().Substring(3);
                    else dataGridView2[2, 6].Value = dataGridView2[2, 6].Value.ToString().Substring(0, 2) + "0" + dataGridView2[2, 6].Value.ToString().Substring(3);
                }
            }
            if (port1_4.Visible == true)
            {
                if (port1_4.BackColor == Color.Firebrick) dataGridView2[2, 6].Value = dataGridView2[2, 6].Value.ToString().Substring(0, 3) + "1" + dataGridView2[2, 6].Value.ToString().Substring(4);
                if (port1_4.BackColor == Color.Transparent) dataGridView2[2, 6].Value = dataGridView2[2, 6].Value.ToString().Substring(0, 3) + "0" + dataGridView2[2, 6].Value.ToString().Substring(4);
                if (port1_4.BackColor == Color.SteelBlue)
                {
                    //Taktgenerator PortA
                    if (dataGridView2[2, 6].Value.ToString().Substring(3, 1) == "0")
                        dataGridView2[2, 6].Value = dataGridView2[2, 6].Value.ToString().Substring(0, 3) + "1" + dataGridView2[2, 6].Value.ToString().Substring(4);
                    else
                        dataGridView2[2, 6].Value = dataGridView2[2, 6].Value.ToString().Substring(0, 3) + "0" + dataGridView2[2, 6].Value.ToString().Substring(4);

                    //PS2:PS0 Bits werden als String zusammengefasst und in Switch Case geprüft
                    String Prescaler_bits = dataGridView3[2, 2].Value.ToString().Substring(5, 1) + dataGridView3[2, 2].Value.ToString().Substring(6, 1) + dataGridView3[2, 2].Value.ToString().Substring(7, 1);
                    int WDT_rate;
                    int TMR0_rate;

                    //Prüfen ob PSA Bit 1--> WDT oder 0--> TMR0 ist
                    if (dataGridView3[2, 2].Value.ToString().Substring(2, 1) == "1")
                    {
                        if (dataGridView3[2, 2].Value.ToString().Substring(4, 1) == "1")
                        {

                            switch (Prescaler_bits)
                            {
                                case "000":
                                    //1:1
                                    TimerWert = TimerWert + 1;
                                    if (TimerWert > 255)
                                    {
                                        TimerWert = 0;
                                        dataGridView2[2, 11].Value = dataGridView2[2, 11].Value.ToString().Substring(0, 5) + "1" + dataGridView2[2, 11].Value.ToString().Substring(6);
                                    }
                                    dataGridView2[2, 2].Value = dec2bin(TimerWert);


                                    break;

                                case "001":
                                    //1:2
                                    TimerWert = TimerWert + 1;

                                    if (TimerWert % 2 == 0)
                                    {
                                        Ausgabewert = Ausgabewert + 1;
                                        if (Ausgabewert > 255)
                                        {
                                            Ausgabewert = 0;
                                            dataGridView2[2, 11].Value = dataGridView2[2, 11].Value.ToString().Substring(0, 5) + "1" + dataGridView2[2, 11].Value.ToString().Substring(6);
                                        }
                                        dataGridView2[2, 2].Value = dec2bin(Ausgabewert);
                                    }
                                    break;

                                case "010":
                                    //1:4
                                    TimerWert = TimerWert + 1;
                                    if (TimerWert % 4 == 0)
                                    {
                                        Ausgabewert = Ausgabewert + 1;
                                        if (Ausgabewert > 255)
                                        {
                                            Ausgabewert = 0;
                                            dataGridView2[2, 11].Value = dataGridView2[2, 11].Value.ToString().Substring(0, 5) + "1" + dataGridView2[2, 11].Value.ToString().Substring(6);
                                        }
                                        dataGridView2[2, 2].Value = dec2bin(Ausgabewert);
                                    }
                                    break;

                                case "011":
                                    //1:8
                                    TimerWert = TimerWert + 1;
                                    if (TimerWert % 8 == 0)
                                    {
                                        Ausgabewert = Ausgabewert + 1;
                                        if (Ausgabewert > 255)
                                        {
                                            Ausgabewert = 0;
                                            dataGridView2[2, 11].Value = dataGridView2[2, 11].Value.ToString().Substring(0, 5) + "1" + dataGridView2[2, 11].Value.ToString().Substring(6);
                                        }
                                        dataGridView2[2, 2].Value = dec2bin(Ausgabewert);
                                    }
                                    break;

                                case "100":
                                    //1:16
                                    TimerWert = TimerWert + 1;
                                    if (TimerWert % 16 == 0)
                                    {
                                        Ausgabewert = Ausgabewert + 1;
                                        if (Ausgabewert > 255)
                                        {
                                            Ausgabewert = 0;
                                            dataGridView2[2, 11].Value = dataGridView2[2, 11].Value.ToString().Substring(0, 5) + "1" + dataGridView2[2, 11].Value.ToString().Substring(6);
                                        }
                                        dataGridView2[2, 2].Value = dec2bin(Ausgabewert);
                                    }
                                    break;

                                case "101":
                                    //1:32
                                    TimerWert = TimerWert + 1;
                                    if (TimerWert % 32 == 0)
                                    {
                                        Ausgabewert = Ausgabewert + 1;
                                        if (Ausgabewert > 255)
                                        {
                                            Ausgabewert = 0;
                                            dataGridView2[2, 11].Value = dataGridView2[2, 11].Value.ToString().Substring(0, 5) + "1" + dataGridView2[2, 11].Value.ToString().Substring(6);
                                        }
                                        dataGridView2[2, 2].Value = dec2bin(Ausgabewert);
                                    }
                                    break;

                                case "110":
                                    //1:64
                                    TimerWert = TimerWert + 1;
                                    if (TimerWert % 64 == 0)
                                    {
                                        Ausgabewert = Ausgabewert + 1;
                                        if (Ausgabewert > 255)
                                        {
                                            Ausgabewert = 0;
                                            dataGridView2[2, 11].Value = dataGridView2[2, 11].Value.ToString().Substring(0, 5) + "1" + dataGridView2[2, 11].Value.ToString().Substring(6);
                                        }
                                        dataGridView2[2, 2].Value = dec2bin(Ausgabewert);
                                    }
                                    break;

                                case "111":
                                    //1:128
                                    TimerWert = TimerWert + 1;
                                    if (TimerWert % 128 == 0)
                                    {
                                        Ausgabewert = Ausgabewert + 1;
                                        if (Ausgabewert > 255)
                                        {
                                            Ausgabewert = 0;
                                            dataGridView2[2, 11].Value = dataGridView2[2, 11].Value.ToString().Substring(0, 5) + "1" + dataGridView2[2, 11].Value.ToString().Substring(6);
                                        }
                                        dataGridView2[2, 2].Value = dec2bin(Ausgabewert);
                                    }
                                    break;
                            }
                        }//Wenn PSA BIT 0= Timer
                        else if (dataGridView3[2, 2].Value.ToString().Substring(4, 1) == "0")
                        {
                            switch (Prescaler_bits)
                            {
                                case "000":
                                    //1:2
                                    TimerWert = TimerWert + 1;
                                    if (TimerWert % 2 == 0)
                                    {
                                        Ausgabewert = Ausgabewert + 1;
                                        if (Ausgabewert > 255)
                                        {
                                            Ausgabewert = 0;
                                            dataGridView2[2, 11].Value = dataGridView2[2, 11].Value.ToString().Substring(0, 5) + "1" + dataGridView2[2, 11].Value.ToString().Substring(6);
                                        }
                                        dataGridView2[2, 2].Value = dec2bin(Ausgabewert);
                                    }
                                    break;

                                case "001":
                                    //1:4
                                    TimerWert = TimerWert + 1;
                                    if (TimerWert % 4 == 0)
                                    {
                                        Ausgabewert = Ausgabewert + 1;
                                        if (Ausgabewert > 255)
                                        {
                                            Ausgabewert = 0;
                                            dataGridView2[2, 11].Value = dataGridView2[2, 11].Value.ToString().Substring(0, 5) + "1" + dataGridView2[2, 11].Value.ToString().Substring(6);
                                        }
                                        dataGridView2[2, 2].Value = dec2bin(Ausgabewert);
                                    }
                                    break;

                                case "010":
                                    //1:8
                                    TimerWert = TimerWert + 1;
                                    if (TimerWert % 8 == 0)
                                    {
                                        Ausgabewert = Ausgabewert + 1;
                                        if (Ausgabewert > 255)
                                        {
                                            Ausgabewert = 0;
                                            dataGridView2[2, 11].Value = dataGridView2[2, 11].Value.ToString().Substring(0, 5) + "1" + dataGridView2[2, 11].Value.ToString().Substring(6);
                                        }
                                        dataGridView2[2, 2].Value = dec2bin(Ausgabewert);
                                    }
                                    break;

                                case "011":
                                    //1:16
                                    TimerWert = TimerWert + 1;
                                    if (TimerWert % 16 == 0)
                                    {
                                        Ausgabewert = Ausgabewert + 1;
                                        if (Ausgabewert > 255)
                                        {
                                            Ausgabewert = 0;
                                            dataGridView2[2, 11].Value = dataGridView2[2, 11].Value.ToString().Substring(0, 5) + "1" + dataGridView2[2, 11].Value.ToString().Substring(6);
                                        }
                                        dataGridView2[2, 2].Value = dec2bin(Ausgabewert);
                                    }
                                    break;

                                case "100":
                                    //1:32
                                    TimerWert = TimerWert + 1;
                                    if (TimerWert % 32 == 0)
                                    {
                                        Ausgabewert = Ausgabewert + 1;
                                        if (Ausgabewert > 255)
                                        {
                                            Ausgabewert = 0;
                                            dataGridView2[2, 11].Value = dataGridView2[2, 11].Value.ToString().Substring(0, 5) + "1" + dataGridView2[2, 11].Value.ToString().Substring(6);
                                        }
                                        dataGridView2[2, 2].Value = dec2bin(Ausgabewert);
                                    }
                                    break;

                                case "101":
                                    //1:64
                                    TimerWert = TimerWert + 1;
                                    if (TimerWert % 64 == 0)
                                    {
                                        Ausgabewert = Ausgabewert + 1;
                                        if (Ausgabewert > 255)
                                        {
                                            Ausgabewert = 0;
                                            dataGridView2[2, 11].Value = dataGridView2[2, 11].Value.ToString().Substring(0, 5) + "1" + dataGridView2[2, 11].Value.ToString().Substring(6);
                                        }
                                        dataGridView2[2, 2].Value = dec2bin(Ausgabewert);
                                    }
                                    break;

                                case "110":
                                    //1:128
                                    TimerWert = TimerWert + 1;
                                    if (TimerWert % 128 == 0)
                                    {
                                        Ausgabewert = Ausgabewert + 1;
                                        if (Ausgabewert > 255)
                                        {
                                            Ausgabewert = 0;
                                            dataGridView2[2, 11].Value = dataGridView2[2, 11].Value.ToString().Substring(0, 5) + "1" + dataGridView2[2, 11].Value.ToString().Substring(6);
                                        }
                                        dataGridView2[2, 2].Value = dec2bin(Ausgabewert);
                                    }
                                    break;

                                case "111":
                                    //1:256
                                    TimerWert = TimerWert + 1;
                                    if (TimerWert % 256 == 0)
                                    {
                                        Ausgabewert = Ausgabewert + 1;
                                        if (Ausgabewert > 255)
                                        {
                                            Ausgabewert = 0;
                                            dataGridView2[2, 11].Value = dataGridView2[2, 11].Value.ToString().Substring(0, 5) + "1" + dataGridView2[2, 11].Value.ToString().Substring(6);
                                        }
                                        dataGridView2[2, 2].Value = dec2bin(Ausgabewert);
                                    }
                                    break;
                            }
                        }
                    }
                }

              

                if (port1_3.Visible == true)
                {
                    if (port1_3.BackColor == Color.Firebrick) dataGridView2[2, 6].Value = dataGridView2[2, 6].Value.ToString().Substring(0, 4) + "1" + dataGridView2[2, 6].Value.ToString().Substring(5);
                    if (port1_3.BackColor == Color.Transparent) dataGridView2[2, 6].Value = dataGridView2[2, 6].Value.ToString().Substring(0, 4) + "0" + dataGridView2[2, 6].Value.ToString().Substring(5);
                    if (port1_3.BackColor == Color.SteelBlue)
                    {
                        if (dataGridView2[2, 6].Value.ToString().Substring(4, 1) == "0") dataGridView2[2, 6].Value = dataGridView2[2, 6].Value.ToString().Substring(0, 4) + "1" + dataGridView2[2, 6].Value.ToString().Substring(5);
                        else dataGridView2[2, 6].Value = dataGridView2[2, 6].Value.ToString().Substring(0, 4) + "0" + dataGridView2[2, 6].Value.ToString().Substring(5);
                    }
                }
                if (port1_2.Visible == true)
                {
                    if (port1_2.BackColor == Color.Firebrick) dataGridView2[2, 6].Value = dataGridView2[2, 6].Value.ToString().Substring(0, 5) + "1" + dataGridView2[2, 6].Value.ToString().Substring(6);
                    if (port1_2.BackColor == Color.Transparent) dataGridView2[2, 6].Value = dataGridView2[2, 6].Value.ToString().Substring(0, 5) + "0" + dataGridView2[2, 6].Value.ToString().Substring(6);
                    if (port1_2.BackColor == Color.SteelBlue)
                    {
                        if (dataGridView2[2, 6].Value.ToString().Substring(5, 1) == "0") dataGridView2[2, 6].Value = dataGridView2[2, 6].Value.ToString().Substring(0, 5) + "1" + dataGridView2[2, 6].Value.ToString().Substring(6);
                        else dataGridView2[2, 6].Value = dataGridView2[2, 6].Value.ToString().Substring(0, 5) + "0" + dataGridView2[2, 6].Value.ToString().Substring(6);
                    }
                }
                if (port1_1.Visible == true)
                {
                    if (port1_1.BackColor == Color.Firebrick) dataGridView2[2, 6].Value = dataGridView2[2, 6].Value.ToString().Substring(0, 6) + "1" + dataGridView2[2, 6].Value.ToString().Substring(7);
                    if (port1_1.BackColor == Color.Transparent) dataGridView2[2, 6].Value = dataGridView2[2, 6].Value.ToString().Substring(0, 6) + "0" + dataGridView2[2, 6].Value.ToString().Substring(7);
                    if (port1_1.BackColor == Color.SteelBlue)
                    {
                        if (dataGridView2[2, 6].Value.ToString().Substring(6, 1) == "0") dataGridView2[2, 6].Value = dataGridView2[2, 6].Value.ToString().Substring(0, 6) + "1" + dataGridView2[2, 6].Value.ToString().Substring(7);
                        else dataGridView2[2, 6].Value = dataGridView2[2, 6].Value.ToString().Substring(0, 6) + "0" + dataGridView2[2, 6].Value.ToString().Substring(7);
                    }
                }
                if (port1_0.Visible == true)
                {
                    if (port1_0.BackColor == Color.Firebrick) dataGridView2[2, 6].Value = dataGridView2[2, 6].Value.ToString().Substring(0, 7) + "1";
                    if (port1_0.BackColor == Color.Transparent) dataGridView2[2, 6].Value = dataGridView2[2, 6].Value.ToString().Substring(0, 7) + "0";
                    if (port1_0.BackColor == Color.SteelBlue)
                    {
                        if (dataGridView2[2, 6].Value.ToString().Substring(7, 1) == "0") dataGridView2[2, 6].Value = dataGridView2[2, 6].Value.ToString().Substring(0, 7) + "1";
                        else dataGridView2[2, 6].Value = dataGridView2[2, 6].Value.ToString().Substring(0, 7) + "0";
                    }
                }

                for (int j = 0; j < dataGridView4.RowCount; j++)
                {
                    if (dataGridView4[1, j].Value.ToString() == dataGridView2[1, 6].Value.ToString())
                    {
                        dataGridView4[2, j].Value = dataGridView2[2, 6].Value.ToString();
                    }
                }

            }
        }

        private void inputportb()
        {
            if (port2_7.Visible == true)
            {
                if (port2_7.BackColor == Color.Firebrick) dataGridView2[2, 7].Value = "1" + dataGridView2[2, 7].Value.ToString().Substring(1);
                if (port2_7.BackColor == Color.Transparent) dataGridView2[2, 7].Value = "0" + dataGridView2[2, 7].Value.ToString().Substring(1);
                if (port2_7.BackColor == Color.SteelBlue)
                {
                    if (dataGridView2[2, 7].Value.ToString().Substring(0, 1) == "0") dataGridView2[2, 7].Value = "1" + dataGridView2[2, 7].Value.ToString().Substring(1);
                    else dataGridView2[2, 7].Value = "0" + dataGridView2[2, 7].Value.ToString().Substring(1);
                }
            }
            if (port2_6.Visible == true)
            {
                if (port2_6.BackColor == Color.Firebrick) dataGridView2[2, 7].Value = dataGridView2[2, 7].Value.ToString().Substring(0, 1) + "1" + dataGridView2[2, 7].Value.ToString().Substring(2);
                if (port2_6.BackColor == Color.Transparent) dataGridView2[2, 7].Value = dataGridView2[2, 7].Value.ToString().Substring(0, 1) + "0" + dataGridView2[2, 7].Value.ToString().Substring(2);
                if (port2_6.BackColor == Color.SteelBlue)
                {
                    if (dataGridView2[2, 7].Value.ToString().Substring(1, 1) == "0") dataGridView2[2, 7].Value = dataGridView2[2, 7].Value.ToString().Substring(0, 1) + "1" + dataGridView2[2, 7].Value.ToString().Substring(2);
                    else dataGridView2[2, 7].Value = dataGridView2[2, 7].Value.ToString().Substring(0, 1) + "0" + dataGridView2[2, 7].Value.ToString().Substring(2);
                }
            }
            if (port2_5.Visible == true)
            {
                if (port2_5.BackColor == Color.Firebrick) dataGridView2[2, 7].Value = dataGridView2[2, 7].Value.ToString().Substring(0, 2) + "1" + dataGridView2[2, 7].Value.ToString().Substring(3);
                if (port2_5.BackColor == Color.Transparent) dataGridView2[2, 7].Value = dataGridView2[2, 7].Value.ToString().Substring(0, 2) + "0" + dataGridView2[2, 7].Value.ToString().Substring(3);
                if (port2_5.BackColor == Color.SteelBlue)
                {
                    if (dataGridView2[2, 7].Value.ToString().Substring(2, 1) == "0") dataGridView2[2, 7].Value = dataGridView2[2, 7].Value.ToString().Substring(0, 2) + "1" + dataGridView2[2, 7].Value.ToString().Substring(3);
                    else dataGridView2[2, 7].Value = dataGridView2[2, 7].Value.ToString().Substring(0, 2) + "0" + dataGridView2[2, 7].Value.ToString().Substring(3);
                }
            }
            if (port2_4.Visible == true)
            {
                if (port2_4.BackColor == Color.Firebrick) dataGridView2[2, 7].Value = dataGridView2[2, 7].Value.ToString().Substring(0, 3) + "1" + dataGridView2[2, 7].Value.ToString().Substring(4);
                if (port2_4.BackColor == Color.Transparent) dataGridView2[2, 7].Value = dataGridView2[2, 7].Value.ToString().Substring(0, 3) + "0" + dataGridView2[2, 7].Value.ToString().Substring(4);
                if (port2_4.BackColor == Color.SteelBlue)
                {
                    if (dataGridView2[2, 7].Value.ToString().Substring(3, 1) == "0") dataGridView2[2, 7].Value = dataGridView2[2, 7].Value.ToString().Substring(0, 3) + "1" + dataGridView2[2, 7].Value.ToString().Substring(4);
                    else dataGridView2[2, 7].Value = dataGridView2[2, 7].Value.ToString().Substring(0, 3) + "0" + dataGridView2[2, 7].Value.ToString().Substring(4);
                }
            }
            if (port2_3.Visible == true)
            {
                if (port2_3.BackColor == Color.Firebrick) dataGridView2[2, 7].Value = dataGridView2[2, 7].Value.ToString().Substring(0, 4) + "1" + dataGridView2[2, 7].Value.ToString().Substring(5);
                if (port2_3.BackColor == Color.Transparent) dataGridView2[2, 7].Value = dataGridView2[2, 7].Value.ToString().Substring(0, 4) + "0" + dataGridView2[2, 7].Value.ToString().Substring(5);
                if (port2_3.BackColor == Color.SteelBlue)
                {
                    if (dataGridView2[2, 7].Value.ToString().Substring(4, 1) == "0") dataGridView2[2, 7].Value = dataGridView2[2, 7].Value.ToString().Substring(0, 4) + "1" + dataGridView2[2, 7].Value.ToString().Substring(5);
                    else dataGridView2[2, 7].Value = dataGridView2[2, 7].Value.ToString().Substring(0, 4) + "0" + dataGridView2[2, 7].Value.ToString().Substring(5);
                }
            }
            if (port2_2.Visible == true)
            {
                if (port2_2.BackColor == Color.Firebrick) dataGridView2[2, 7].Value = dataGridView2[2, 7].Value.ToString().Substring(0, 5) + "1" + dataGridView2[2, 7].Value.ToString().Substring(6);
                if (port2_2.BackColor == Color.Transparent) dataGridView2[2, 7].Value = dataGridView2[2, 7].Value.ToString().Substring(0, 5) + "0" + dataGridView2[2, 7].Value.ToString().Substring(6);
                if (port2_2.BackColor == Color.SteelBlue)
                {
                    if (dataGridView2[2, 7].Value.ToString().Substring(5, 1) == "0") dataGridView2[2, 7].Value = dataGridView2[2, 7].Value.ToString().Substring(0, 5) + "1" + dataGridView2[2, 7].Value.ToString().Substring(6);
                    else dataGridView2[2, 7].Value = dataGridView2[2, 7].Value.ToString().Substring(0, 5) + "0" + dataGridView2[2, 7].Value.ToString().Substring(6);
                }
            }
            if (port2_1.Visible == true)
            {
                if (port2_1.BackColor == Color.Firebrick) dataGridView2[2, 7].Value = dataGridView2[2, 7].Value.ToString().Substring(0, 6) + "1" + dataGridView2[2, 7].Value.ToString().Substring(7);
                if (port2_1.BackColor == Color.Transparent) dataGridView2[2, 7].Value = dataGridView2[2, 7].Value.ToString().Substring(0, 6) + "0" + dataGridView2[2, 7].Value.ToString().Substring(7);
                if (port2_1.BackColor == Color.SteelBlue)
                {
                    if (dataGridView2[2, 7].Value.ToString().Substring(6, 1) == "0") dataGridView2[2, 7].Value = dataGridView2[2, 7].Value.ToString().Substring(0, 6) + "1" + dataGridView2[2, 7].Value.ToString().Substring(7);
                    else dataGridView2[2, 7].Value = dataGridView2[2, 7].Value.ToString().Substring(0, 6) + "0" + dataGridView2[2, 7].Value.ToString().Substring(7);
                }
            }
            if (port2_0.Visible == true)
            {
                if (port2_0.BackColor == Color.Firebrick) dataGridView2[2, 7].Value = dataGridView2[2, 7].Value.ToString().Substring(0, 7) + "1";
                if (port2_0.BackColor == Color.Transparent) dataGridView2[2, 7].Value = dataGridView2[2, 7].Value.ToString().Substring(0, 7) + "0";
                if (port2_0.BackColor == Color.SteelBlue)
                {
                    if (dataGridView2[2, 7].Value.ToString().Substring(7, 1) == "0") dataGridView2[2, 7].Value = dataGridView2[2, 7].Value.ToString().Substring(0, 7) + "1";
                    else dataGridView2[2, 7].Value = dataGridView2[2, 7].Value.ToString().Substring(0, 7) + "0";
                }
            }
            for (int j = 0; j < dataGridView4.RowCount; j++)
            {
                if (dataGridView4[1, j].Value.ToString() == dataGridView2[1, 7].Value.ToString())
                {
                    dataGridView4[2, j].Value = dataGridView2[2, 7].Value.ToString();
                }
            }

        }

        private void inputoutput(int button, bool inout)
        {
            switch (button)
            {
                case 1:
                    if (inout == true)
                    {
                        port1_7.Visible = true;
                        port3_7.Visible = false;
                    }
                    else
                    {
                        port1_7.Visible = false;
                        port3_7.Visible = true;

                    }
                    break;

                case 2:
                    if (inout == true)
                    {
                        port1_6.Visible = true;
                        port3_6.Visible = false;
                    }
                    else
                    {
                        port1_6.Visible = false;
                        port3_6.Visible = true;

                    }
                    break;

                case 3:
                    if (inout == true)
                    {
                        port1_5.Visible = true;
                        port3_5.Visible = false;
                    }
                    else
                    {
                        port1_5.Visible = false;
                        port3_5.Visible = true;

                    }
                    break;

                case 4:
                    if (inout == true)
                    {
                        port1_4.Visible = true;
                        port3_4.Visible = false;
                    }
                    else
                    {
                        port1_4.Visible = false;
                        port3_4.Visible = true;

                    }
                    break;

                case 5:
                    if (inout == true)
                    {
                        port1_3.Visible = true;
                        port3_3.Visible = false;
                    }
                    else
                    {
                        port1_3.Visible = false;
                        port3_3.Visible = true;

                    }
                    break;

                case 6:
                    if (inout == true)
                    {
                        port1_2.Visible = true;
                        port3_2.Visible = false;
                    }
                    else
                    {
                        port1_2.Visible = false;
                        port3_2.Visible = true;

                    }
                    break;

                case 7:
                    if (inout == true)
                    {
                        port1_1.Visible = true;
                        port3_1.Visible = false;
                    }
                    else
                    {
                        port1_1.Visible = false;
                        port3_1.Visible = true;

                    }
                    break;

                case 8:
                    if (inout == true)
                    {
                        port1_0.Visible = true;
                        port3_0.Visible = false;
                    }
                    else
                    {
                        port1_0.Visible = false;
                        port3_0.Visible = true;

                    }
                    break;

                case 9:
                    if (inout == true)
                    {
                        port2_7.Visible = true;
                        port4_7.Visible = false;
                    }
                    else
                    {
                        port2_7.Visible = false;
                        port4_7.Visible = true;

                    }
                    break;

                case 10:
                    if (inout == true)
                    {
                        port2_6.Visible = true;
                        port4_6.Visible = false;
                    }
                    else
                    {
                        port2_6.Visible = false;
                        port4_6.Visible = true;

                    }
                    break;

                case 11:
                    if (inout == true)
                    {
                        port2_5.Visible = true;
                        port4_5.Visible = false;
                    }
                    else
                    {
                        port2_5.Visible = false;
                        port4_5.Visible = true;

                    }
                    break;

                case 12:
                    if (inout == true)
                    {
                        port2_4.Visible = true;
                        port4_4.Visible = false;
                    }
                    else
                    {
                        port2_4.Visible = false;
                        port4_4.Visible = true;

                    }
                    break;

                case 13:
                    if (inout == true)
                    {
                        port2_3.Visible = true;
                        port4_3.Visible = false;
                    }
                    else
                    {
                        port2_3.Visible = false;
                        port4_3.Visible = true;

                    }
                    break;

                case 14:
                    if (inout == true)
                    {
                        port2_2.Visible = true;
                        port4_2.Visible = false;
                    }
                    else
                    {
                        port2_2.Visible = false;
                        port4_2.Visible = true;

                    }
                    break;

                case 15:
                    if (inout == true)
                    {
                        port2_1.Visible = true;
                        port4_1.Visible = false;
                    }
                    else
                    {
                        port2_1.Visible = false;
                        port4_1.Visible = true;

                    }
                    break;

                case 16:
                    if (inout == true)
                    {
                        port2_0.Visible = true;
                        port4_0.Visible = false;
                    }
                    else
                    {
                        port2_0.Visible = false;
                        port4_0.Visible = true;

                    }
                    break;

            }

        }

        public async void interrupt()
        {
            btn_interrupt.Enabled = true;
            
            
            await Task.Delay(5000);
            for (int i = 0; dataGridView1.Rows.Count> i;i++)
            {
                if (dataGridView1[1, i].Value.ToString() == "0004")
                    dataGridView1.CurrentCell = dataGridView1[1, i];
            }
            btn_interrupt.Enabled = false;  
        }
        
        private int readopcode(string binarystring)
        {
            
            if (binarystring.StartsWith("000111"))
            {
                return 0;
            }
            if (binarystring.StartsWith("000101"))
            {
                return 1;
            }
            if (binarystring.StartsWith("0000011"))
            {
                return 2;
            }
            if (binarystring.StartsWith("0000010"))
            {
                return 3;
            }
            if (binarystring.StartsWith("001001"))
            {
                return 4;
            }
            if (binarystring.StartsWith("000011"))
            {
                return 5;
            }
            if (binarystring.StartsWith("001011"))
            {
                return 6;
            }
            if (binarystring.StartsWith("001010"))
            {
                return 7;
            }
            if (binarystring.StartsWith("001111"))
            {
                return 8;
            }
            if (binarystring.StartsWith("000100"))
            {
                return 9;
            }
            if (binarystring.StartsWith("001000"))
            {
                return 10;
            }
            if (binarystring.StartsWith("0000001"))
            {
                return 11;
            }
            if ((binarystring.StartsWith("000000") && binarystring.EndsWith("00000")))
            {
                return 12;
            }
            if (binarystring.StartsWith("001101"))
            {
                return 13;
            }
            if (binarystring.StartsWith("001100"))
            {
                return 14;
            }
            if (binarystring.StartsWith("000010"))
            {
                return 15;
            }
            if (binarystring.StartsWith("001110"))
            {
                return 16;
            }
            if (binarystring.StartsWith("000110"))
            {
                return 17;
            }
            if (binarystring.StartsWith("0100"))
            {
                return 18;
            }
            if (binarystring.StartsWith("0101"))
            {
                return 19;
            }
            if (binarystring.StartsWith("0110"))
            {
                return 20;
            }
            if (binarystring.StartsWith("0111"))
            {
                return 21;
            }
            if (binarystring.StartsWith("11111"))
            {
                return 22;
            }
            if (binarystring.StartsWith("111001"))
            {
                return 23;
            }
            if (binarystring.StartsWith("100"))
            {
                return 24;
            }
            if (binarystring.StartsWith("00000001100100"))
            {
                return 25;
            }
            if (binarystring.StartsWith("101"))
            {
                return 26;
            }
            if (binarystring.StartsWith("111000"))
            {
                return 27;
            }
            if (binarystring.StartsWith("1100"))
            {
                return 28;
            }
            if (binarystring.StartsWith("00000000001001"))
            {
                return 29;
            }
            if (binarystring.StartsWith("1101"))
            {
                return 30;
            }
            if (binarystring == "00000000001000")
            {
                return 31;
            }
            if (binarystring.StartsWith("00000001100011"))
            {
                return 32;
            }
            if (binarystring.StartsWith("11110"))
            {
                return 33;
            }
            if (binarystring.StartsWith("111010"))
            {
                return 34;
            }
           if (binarystring.StartsWith("0111"))
            {
                return 35;
            }

            return 1234567;
        }

        private void stepbtn_Click_1(object sender, EventArgs e)
        {
            timer2.Interval = taktb;
            timer1.Interval = takta;
            timer2.Start();
            timer1.Start();
            timer3.Interval = 200;
            timer3.Start();

            if (dataGridView1.CurrentRow.DefaultCellStyle.BackColor == Color.White || dataGridView1.CurrentRow.DefaultCellStyle.BackColor == Color.LightGray) colorset = dataGridView1.CurrentRow.DefaultCellStyle.BackColor;
            string opcode = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            string opstring = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            int row = dataGridView1.CurrentRow.Index;
            int retvalue = dooperator(opcode, opstring, row);
            


            if (retvalue != 1234567)
            {
                dataGridView1.Rows[dataGridView1.CurrentRow.Index].DefaultCellStyle.BackColor = colorset;
                dataGridView1.CurrentCell = dataGridView1[1, retvalue];

                if (dataGridView1.CurrentRow.DefaultCellStyle.BackColor != Color.Salmon) colorset = dataGridView1.CurrentRow.DefaultCellStyle.BackColor;
                dataGridView1.CurrentRow.DefaultCellStyle.BackColor = Color.LightSalmon;
            }
            else
            {
                dataGridView1.Rows[dataGridView1.CurrentRow.Index].DefaultCellStyle.BackColor = colorset;
                dataGridView1.CurrentCell = dataGridView1[1, dataGridView1.CurrentRow.Index + 1];
                
            if (dataGridView1.CurrentRow.DefaultCellStyle.BackColor != Color.Salmon) colorset = dataGridView1.CurrentRow.DefaultCellStyle.BackColor;
                dataGridView1.CurrentRow.DefaultCellStyle.BackColor = Color.LightSalmon;
            }
            
        }

        public static string BinaryStringToHexString(string binary)
        {
            StringBuilder result = new StringBuilder(binary.Length / 8 + 1);

            // TODO: check all 1's or 0's... Will throw otherwise

            int mod4Len = binary.Length % 8;
            if (mod4Len != 0)
            {
                // pad to length multiple of 8
                binary = binary.PadLeft(((binary.Length / 8) + 1) * 8, '0');
            }

            for (int i = 0; i < binary.Length; i += 8)
            {
                string eightBits = binary.Substring(i, 8);
                result.AppendFormat("{0:X2}", Convert.ToByte(eightBits, 2));
            }
            string resultstring = result.ToString();
            for (int i = 0; resultstring.Length < 2; i++)
            {
                resultstring = "0" + resultstring;
            }
            if (resultstring.EndsWith("A"))
            { resultstring = resultstring.Substring(0, 1) + "ah"; }
            if (resultstring.EndsWith("B"))
            { resultstring = resultstring.Substring(0, 1) + "bh"; }
            if (resultstring.EndsWith("C"))
            { resultstring = resultstring.Substring(0, 1) + "ch"; }
            if (resultstring.EndsWith("D"))
            { resultstring = resultstring.Substring(0, 1) + "dh"; }
            if (resultstring.EndsWith("E"))
            { resultstring = resultstring.Substring(0, 1) + "eh"; }
            if (resultstring.EndsWith("F"))
            { resultstring = resultstring.Substring(0, 1) + "fh"; }
            if (!resultstring.EndsWith("h")) resultstring = resultstring + "h";

            return resultstring;
        }

        private string dec2bin(int decint)
        {

            string binaryval = "";
            for (int i = 128; i > 0; i = i / 2)
            {
                if (decint >= i)
                {
                    binaryval = binaryval + "1";
                    decint = decint - i;
                }
                else
                {
                    binaryval = binaryval + "0";
                }
            }
            return binaryval;

        }

        private int bin2dec(string binarystring)
        {
            int val = 1;
            int decint = 0;
            for (int i = binarystring.Length - 1; i >= 0; i--)
            {
                if (binarystring.Substring(i, 1) == "1")
                {
                    decint = decint + val;
                }
                val = val * 2;
            }
            return decint;
        }

        private void port1_0_Click(object sender, EventArgs e)
        {
            if (port1_0.BackColor == Color.Transparent) port1_0.BackColor = Color.Firebrick;
            else
            {
                if (port1_0.BackColor == Color.Firebrick) port1_0.BackColor = Color.SteelBlue;
                else
                {
                    if (port1_0.BackColor == Color.SteelBlue) port1_0.BackColor = Color.Transparent;
                }
            }
            
        }

        private void port1_1_Click(object sender, EventArgs e)
        {
            if (port1_1.BackColor == Color.Transparent) port1_1.BackColor = Color.Firebrick;
            else {
                if (port1_1.BackColor == Color.Firebrick) port1_1.BackColor = Color.SteelBlue;
                else {
                    if (port1_1.BackColor == Color.SteelBlue) port1_1.BackColor = Color.Transparent;
                }
            }
        }

        private void port1_2_Click(object sender, EventArgs e)
        {
            if (port1_2.BackColor == Color.Transparent) port1_2.BackColor = Color.Firebrick;
            else {
                if (port1_2.BackColor == Color.Firebrick) port1_2.BackColor = Color.SteelBlue;
                else {
                    if (port1_2.BackColor == Color.SteelBlue) port1_2.BackColor = Color.Transparent;
                }
            }
        }

        private void port1_3_Click(object sender, EventArgs e)
        {
            if (port1_3.BackColor == Color.Transparent) port1_3.BackColor = Color.Firebrick;
            else {
                if (port1_3.BackColor == Color.Firebrick) port1_3.BackColor = Color.SteelBlue;
                else {
                    if (port1_3.BackColor == Color.SteelBlue) port1_3.BackColor = Color.Transparent;
                }
            }
        }

        private void port1_4_Click(object sender, EventArgs e)
        {
            if (port1_4.BackColor == Color.Transparent) port1_4.BackColor = Color.Firebrick;
            else {
                if (port1_4.BackColor == Color.Firebrick) port1_4.BackColor = Color.SteelBlue;
                else {
                    if (port1_4.BackColor == Color.SteelBlue) port1_4.BackColor = Color.Transparent;
                }
            }
        }

        private void port1_5_Click(object sender, EventArgs e)
        {
            if (port1_5.BackColor == Color.Transparent) port1_5.BackColor = Color.Firebrick;
            else {
                if (port1_5.BackColor == Color.Firebrick) port1_5.BackColor = Color.SteelBlue;
                else {
                    if (port1_5.BackColor == Color.SteelBlue) port1_5.BackColor = Color.Transparent;
                }
            }
        }

        private void port1_6_Click(object sender, EventArgs e)
        {
            if (port1_6.BackColor == Color.Transparent) port1_6.BackColor = Color.Firebrick;
            else {
                if (port1_6.BackColor == Color.Firebrick) port1_6.BackColor = Color.SteelBlue;
                else {
                    if (port1_6.BackColor == Color.SteelBlue) port1_6.BackColor = Color.Transparent;
                }
            }
        }

        private void port1_7_Click(object sender, EventArgs e)
        {
            if (port1_7.BackColor == Color.Transparent) port1_7.BackColor = Color.Firebrick;
            else {
                if (port1_7.BackColor == Color.Firebrick) port1_7.BackColor = Color.SteelBlue;
                else {
                    if (port1_7.BackColor == Color.SteelBlue) port1_7.BackColor = Color.Transparent;
                }
            }
        }

        private void port2_0_Click(object sender, EventArgs e)
        {
            if (port2_0.BackColor == Color.Transparent) port2_0.BackColor = Color.Firebrick;
            else {
                if (port2_0.BackColor == Color.Firebrick) port2_0.BackColor = Color.SteelBlue;
                else {
                    if (port2_0.BackColor == Color.SteelBlue) port2_0.BackColor = Color.Transparent;
                }
            }
        }

        private void port2_7_Click(object sender, EventArgs e)
        {
            if (port2_7.BackColor == Color.Transparent) port2_7.BackColor = Color.Firebrick;
            else {
                if (port2_7.BackColor == Color.Firebrick) port2_7.BackColor = Color.SteelBlue;
                else {
                    if (port2_7.BackColor == Color.SteelBlue) port2_7.BackColor = Color.Transparent;
                }
            }
        }

        private void port2_6_Click(object sender, EventArgs e)
        {
            if (port2_6.BackColor == Color.Transparent) port2_6.BackColor = Color.Firebrick;
            else {
                if (port2_6.BackColor == Color.Firebrick) port2_6.BackColor = Color.SteelBlue;
                else {
                    if (port2_6.BackColor == Color.SteelBlue) port2_6.BackColor = Color.Transparent;
                }
            }
        }

        private void port2_5_Click(object sender, EventArgs e)
        {
            if (port2_5.BackColor == Color.Transparent) port2_5.BackColor = Color.Firebrick;
            else {
                if (port2_5.BackColor == Color.Firebrick) port2_5.BackColor = Color.SteelBlue;
                else {
                    if (port2_5.BackColor == Color.SteelBlue) port2_5.BackColor = Color.Transparent;
                }
            }
        }

        private void port2_4_Click(object sender, EventArgs e)
        {
            if (port2_4.BackColor == Color.Transparent) port2_4.BackColor = Color.Firebrick;
            else {
                if (port2_4.BackColor == Color.Firebrick) port2_4.BackColor = Color.SteelBlue;
                else {
                    if (port2_4.BackColor == Color.SteelBlue) port2_4.BackColor = Color.Transparent;
                }
            }
        }

        private void port2_3_Click(object sender, EventArgs e)
        {
            if (port2_3.BackColor == Color.Transparent) port2_3.BackColor = Color.Firebrick;
            else {
                if (port2_3.BackColor == Color.Firebrick) port2_3.BackColor = Color.SteelBlue;
                else {
                    if (port2_3.BackColor == Color.SteelBlue) port2_3.BackColor = Color.Transparent;
                }
            }
        }

        private void port2_2_Click(object sender, EventArgs e)
        {
            if (port2_2.BackColor == Color.Transparent) port2_2.BackColor = Color.Firebrick;
            else {
                if (port2_2.BackColor == Color.Firebrick) port2_2.BackColor = Color.SteelBlue;
                else {
                    if (port2_2.BackColor == Color.SteelBlue) port2_2.BackColor = Color.Transparent;
                }
            }
        }

        private void port2_1_Click(object sender, EventArgs e)
        {
            if (port2_1.BackColor == Color.Transparent) port2_1.BackColor = Color.Firebrick;
            else {
                if (port2_1.BackColor == Color.Firebrick) port2_1.BackColor = Color.SteelBlue;
                else {
                    if (port2_1.BackColor == Color.SteelBlue) port2_1.BackColor = Color.Transparent;
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            inputporta();
           
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
           inputportb();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                takta = Convert.ToInt32(textBox1.Text.ToString());
                if (takta < 0) { takta = 0; textBox1.Text = "0"; }
                if (takta > 10000) { takta = 10000; textBox1.Text = "10000"; }

            }
            catch {
                takta = 300;
                textBox1.Text = "300";
            }

            timer1.Interval = takta;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                taktb = Convert.ToInt32(textBox2.Text.ToString());
                if (taktb < 0) { taktb = 0; textBox2.Text = "0"; }
                if (taktb > 10000) { taktb = 10000; textBox2.Text = "10000"; }

            }
            catch
            {
                taktb = 300;
                textBox2.Text = "300";
            }
            timer2.Interval = taktb;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            try {
                while (textBox3.Text.ToString() == "") { goto Warte; }
                quarzfrequenz = Convert.ToInt32(textBox3.Text);
                if(quarzfrequenz < 0) { quarzfrequenz = 4000; textBox3.Text = quarzfrequenz.ToString(); }
            }
            catch { quarzfrequenz = 4000; textBox3.Text = quarzfrequenz.ToString(); }

            Warte:
            ;
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            
            if (quarzfrequenz != 0)
            {
                runtime = (4000 / quarzfrequenz) * befehlnr;
                textBox4.Text = runtime.ToString();
            }
        }
        
        private void timer4_Tick(object sender, EventArgs e)
        {
            if (conn.status() == "Connection established" && startbtn.Enabled == true)
            {
                try
                {
                    serialPort1.Write(getcominformation() + '\r');
                    string returnval = ReadDataSegment();
                    string retporta = returnval.Substring(0, 2);
                    string retportb = returnval.Substring(2, 2);
                    retporta = picview2hex(retporta);
                    retportb = picview2hex(retportb);
                    retporta = String.Join(String.Empty, retporta.Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));
                    retportb = String.Join(String.Empty, retportb.Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));
                    retporta = "---" + retporta.Substring(3);
                    dataGridView2[2, 6].Value = retporta;
                    dataGridView2[2, 7].Value = retportb;
                }
                catch { }
            }
            else
            {
                //timer4.Stop();
                //timer2.Start();
                //timer1.Start();
                //button5.Visible = false;
            }
        }
        
        private string picview2hex(string picview)
        {
            string hex = "";
            for (int i = 0; i < 2; i++)
            {
                if (picview.Substring(i, 1) == ":") hex = hex + "a";
                if (picview.Substring(i, 1) == ";") hex = hex + "b";
                if (picview.Substring(i, 1) == "<") hex = hex + "c";
                if (picview.Substring(i, 1) == "=") hex = hex + "d";
                if (picview.Substring(i, 1) == ">") hex = hex + "e";
                if (picview.Substring(i, 1) == "?") hex = hex + "f";
                if (picview.Substring(i, 1) == "0") hex = hex + "0";
                if (picview.Substring(i, 1) == "1") hex = hex + "1";
                if (picview.Substring(i, 1) == "2") hex = hex + "2";
                if (picview.Substring(i, 1) == "3") hex = hex + "3";
                if (picview.Substring(i, 1) == "4") hex = hex + "4";
                if (picview.Substring(i, 1) == "5") hex = hex + "5";
                if (picview.Substring(i, 1) == "6") hex = hex + "6";
                if (picview.Substring(i, 1) == "7") hex = hex + "7";
                if (picview.Substring(i, 1) == "8") hex = hex + "8";
                if (picview.Substring(i, 1) == "9") hex = hex + "9";
            }
            return hex;

        }

        private void button2_Click(object sender, EventArgs e)
        {

            conn = new Form2(serialPort1);
            conn.Show();
            
            timer4.Start();
            button2.Enabled = false;
            timer1.Stop();
            timer2.Stop();
            button5.Visible = true;
        }

        private string makeitconvertable(string binary)
        {
            string convertedbinary = "";
            for(int i = 0;i<8;i++)
            {
                if (binary.Substring(i, 1) == "1") convertedbinary = convertedbinary + "1";
                else convertedbinary = convertedbinary + "0";
            }
            return convertedbinary;
        }

        private string getcominformation()
        {
            string trisa = makeitconvertable(dataGridView3[2, 6].Value.ToString());
            string trisb = makeitconvertable(dataGridView3[2, 7].Value.ToString());
            string porta = makeitconvertable(dataGridView2[2, 6].Value.ToString());
            string portb = makeitconvertable(dataGridView2[2, 7].Value.ToString());
            trisa = BinaryStringToHexString(trisa).Substring(0, 2);
            trisb = BinaryStringToHexString(trisb).Substring(0, 2);
            porta = BinaryStringToHexString(porta).Substring(0, 2);
            portb = BinaryStringToHexString(portb).Substring(0, 2);
            trisa = converttopicview(trisa);
            trisb = converttopicview(trisb);
            porta = converttopicview(porta);
            portb = converttopicview(portb);
            return trisa+porta+trisb+portb;
        }

        private string converttopicview(string hex)
        {
            string picviewstring = "";
            for (int i = 0;i<2;i++)
            {
                if (hex.Substring(i, 1) == "a" || hex.Substring(i, 1) == "A") picviewstring = picviewstring + ":";
                if (hex.Substring(i, 1) == "b" || hex.Substring(i, 1) == "B") picviewstring = picviewstring + ";";
                if (hex.Substring(i, 1) == "c" || hex.Substring(i, 1) == "C") picviewstring = picviewstring + "<";
                if (hex.Substring(i, 1) == "d" || hex.Substring(i, 1) == "D") picviewstring = picviewstring + "=";
                if (hex.Substring(i, 1) == "e" || hex.Substring(i, 1) == "E") picviewstring = picviewstring + ">";
                if (hex.Substring(i, 1) == "f" || hex.Substring(i, 1) == "F") picviewstring = picviewstring + "?";
                if (hex.Substring(i, 1) == "0") picviewstring = picviewstring + "0";
                if (hex.Substring(i, 1) == "1") picviewstring = picviewstring + "1";
                if (hex.Substring(i, 1) == "2") picviewstring = picviewstring + "2";
                if (hex.Substring(i, 1) == "3") picviewstring = picviewstring + "3";
                if (hex.Substring(i, 1) == "4") picviewstring = picviewstring + "4";
                if (hex.Substring(i, 1) == "5") picviewstring = picviewstring + "5";
                if (hex.Substring(i, 1) == "6") picviewstring = picviewstring + "6";
                if (hex.Substring(i, 1) == "7") picviewstring = picviewstring + "7";
                if (hex.Substring(i, 1) == "8") picviewstring = picviewstring + "8";
                if (hex.Substring(i, 1) == "9") picviewstring = picviewstring + "9";
            }
            return picviewstring;
        }

        private string ReadDataSegment()
        {
            string s = "";

            if (serialPort1.BytesToRead >= 5)
            {
                char? c = null;

                int idx = 5;
                while (c != '\r' && idx > 0)
                {
                    c = (char)serialPort1.ReadByte();

                    s += c;

                    idx--;
                }

                if (idx <= 0 && c != '\r')
                {
                    return null;
                }
            }
            else
            {
                return string.Empty;
            }

            return s.Trim('\r');
        }

        private void button5_Click(object sender, EventArgs e)
        {
            serialPort1.Close();
            timer4.Stop();
            timer1.Start();
            timer2.Start();
            button5.Visible = false;
            conn.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            serialPort1.Close();
            timer4.Stop();
            timer1.Start();
            timer2.Start();
            button5.Visible = false;
            conn.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Help help = new Help();
            help.Show();
        }

        
    }
}
