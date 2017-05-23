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
            timer4.Interval = 300;
        }

        //neues lst-File auswählen
        private void button1_Click(object sender, EventArgs e)
        {
            
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Unterstützte Dateien (*.lst)|*.lst";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                textbox_path.Text = ofd.FileName; //Dateiname der ausgewählten Datei
            }
        }

        //auslesen des LST-Files und setzen der Register (Standartwerte)
        private void readbtn_Click(object sender, EventArgs e)
        {
            

            string path = textbox_path.Text;
            string line;
            int start = 0;
            try {
                System.IO.StreamReader file =
                    new System.IO.StreamReader(path);

                //Register mit Standartwerten belegen
                bank0.Rows.Add("w", "", "");
                bank0.Rows.Add("INDF", "00h", "--------");
                bank0.Rows.Add("TMR0", "01h", "xxxxxxxx");
                bank0.Rows.Add("PCL", "02h", "00000000");
                bank0.Rows.Add("STATUS", "03h", "00011xxx");
                bank0.Rows.Add("FSR", "04h", "xxxxxxxx");
                bank0.Rows.Add("PORTA", "05h", "---xxxxx");
                bank0.Rows.Add("PORTB", "06h", "xxxxxxxx");
                bank0.Rows.Add("EEDATA", "08h", "xxxxxxxx");
                bank0.Rows.Add("EEADR", "09h", "xxxxxxxx");
                bank0.Rows.Add("PCLATH", "0Ah", "---00000");
                bank0.Rows.Add("INTCON", "0Bh", "0000000x");

                bank1.Rows.Add("w", "", "");
                bank1.Rows.Add("INDF", "80h", "--------");
                bank1.Rows.Add("OPTION_REG", "81h", "11111111");
                bank1.Rows.Add("PCL", "82h", "00000000");
                bank1.Rows.Add("STATUS", "83h", "00011xxx");
                bank1.Rows.Add("FSR", "84h", "xxxxxxxx");
                bank1.Rows.Add("TRISA", "85h", "---11111");
                bank1.Rows.Add("TRISB", "86h", "11111111");
                bank1.Rows.Add("EECON1", "88h", "---0x000");
                bank1.Rows.Add("EECON2", "89h", "--------");
                bank1.Rows.Add("PCLATH", "0Ah", "---00000");
                bank1.Rows.Add("INTCON", "0Bh", "0000000x");
                int button;

                //timer für input/output setzen
                timerinputporta.Interval = takta;
                timerinputportb.Interval = taktb;
                timerinputportb.Start();
                timerinputporta.Start();
               

               
                while ((line = file.ReadLine()) != null)
                {
                    
                    List<String> registername = new List<String>();
                    List<String> varvalue = new List<String>();

                    if (start == 0)
                    {
                        //auslesen der weite für Variablen
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

                    //Speicherzellen ins datagridview schreiben
                    for (int i = 0; registername.Count > i; i++)
                    {
                        speicherzellen.Rows.Add(registername[i], varvalue[i], "");
                    }

                    //Ab hier wird das eig Programm (mit befehlen) ausgelesen
                    if (start == 1)
                    {
                        //auslesen der Loops (Loop1, Loop2, etc)
                        Regex regloop = new Regex("[0-9]{5}[ ]{2}[A-Za-z0-9]*");
                        if (regloop.Match(line).Success)
                        {
                            string match = regloop.Match(line).Value;
                            match = match.Remove(0, 7);
                            if (match != "")
                            {
                                opcodedata.Rows.Add(false, "", "", "" + match + "");
                                opcodedata.Rows[opcodedata.Rows.Count - 1].DefaultCellStyle.BackColor = Color.LightGray;
                            }
                        }
                        //auslesen der operators
                        Regex regop = new Regex("[0-9A-F]{4}[ ]{1}[0-9a-fA-F]{4}[ ]{1}");
                        if (regop.Match(line).Success)
                        {
                            string match = line;
                            string idx = match.Remove(4); //Index
                            string bitop = match.Remove(9).Remove(0, 4).TrimStart(' '); //opcode
                            Regex regops = new Regex("[ a-zA-Z0-9,]*");
                            string op = match.Remove(0, 36);
                            op = regops.Match(op).Value;
                            op = op.TrimEnd(' '); //opstring
                            opcodedata.Rows.Add(false, idx, bitop, op);
                        }
                    }
                    //start auf 1, also signal, dass befehle nun kommen
                    Regex r = new Regex("org[ ]*0");
                    if (r.Match(line).Success)
                    {
                        start = 1;
                    }
                }
                //gleichsetzen von speicherzellen mir den Banken
                for (int i = 0; i < speicherzellen.RowCount; i++)
                {
                    string locs = speicherzellen[1, i].Value.ToString();
                    for (int j = 0; j < bank0.RowCount; j++)
                    {
                        if (bank0[1, j].Value.ToString() == locs)
                        {
                            speicherzellen[2, i].Value = bank0[2, j].Value.ToString();
                        }
                        if (bank1[1, j].Value.ToString() == locs)
                        {
                            speicherzellen[2, i].Value = bank1[2, j].Value.ToString();
                        }
                    }
                }
                

            }
            catch
            { MessageBox.Show("Error!"); }

            //befehle können nun ausgeführt werden
            startbtn.Enabled = true;
            stepbtn.Enabled = true;
            
        }

        //Neues Form wird geöffnet
        private void clearbtn_Click(object sender, EventArgs e)
        {
            serialPort1.Close();
            Form1 NewForm = new Form1();
            NewForm.Show();
            this.Dispose(false);
        }

        //start button wird geklickt und operator im loop ausgeführt
        private void startbtn_Click(object sender, EventArgs e)
        {
            opcodedata.CurrentCell = opcodedata[0, 0];
            timer_freq.Interval = 200;
            timer_freq.Start();
            startbtn.Visible = false;
            btn_weiter.Visible = true;
            btn_weiter.Enabled = true;
            dooperatorloop();
            
        }

        //ausführen der befehle in der loop
        private async void dooperatorloop()
        {
            if (opcodedata.CurrentRow.DefaultCellStyle.BackColor != Color.Salmon) colorset = opcodedata.CurrentRow.DefaultCellStyle.BackColor;
            while (opcodedata.CurrentRow.Index < opcodedata.RowCount && opcodedata.CurrentRow.Cells[0].Value.ToString() == "False")

            {

                //try
                //{
                //    dataGridView1.Rows[dataGridView1.CurrentRow.Index - 1].DefaultCellStyle.BackColor = Color.White;
                //    if (dataGridView1.Rows[dataGridView1.CurrentRow.Index - 1].Cells[2].Value == "")
                //    { dataGridView1.Rows[dataGridView1.CurrentRow.Index - 1].DefaultCellStyle.BackColor = Color.LightGray; }

                //}
                //catch { }

                opcodedata.CurrentRow.DefaultCellStyle.BackColor = Color.LightSalmon;
                string opcode = opcodedata.CurrentRow.Cells[2].Value.ToString();
                string opstring = opcodedata.CurrentRow.Cells[3].Value.ToString();
                int row = opcodedata.CurrentRow.Index;
                int retvalue = dooperator(opcode, opstring, row);
                //Zeitdurchlauf für einen Befehl
                await Task.Delay(60);


                if (retvalue != 1234567)
                {
                    opcodedata.Rows[opcodedata.CurrentRow.Index].DefaultCellStyle.BackColor = colorset;
                    opcodedata.CurrentCell = opcodedata[1, retvalue];
                    colorset = opcodedata.CurrentRow.DefaultCellStyle.BackColor;
                    opcodedata.CurrentRow.DefaultCellStyle.BackColor = Color.LightSalmon;
                }
                else
                {
                    opcodedata.Rows[opcodedata.CurrentRow.Index].DefaultCellStyle.BackColor = colorset;
                    opcodedata.CurrentCell = opcodedata[1, opcodedata.CurrentRow.Index + 1];
                    colorset = opcodedata.CurrentRow.DefaultCellStyle.BackColor;
                    opcodedata.CurrentRow.DefaultCellStyle.BackColor = Color.LightSalmon;
                }


            }
            if (opcodedata.CurrentRow.Cells[0].Value.ToString() == "True") opcodedata.CurrentRow.Cells[0].Value = "False";
        }

        //ausführen der befehle
        private int dooperator(string opcode, string opstring, int row)
        {
            //Program counter
            if (opcodedata[1, opcodedata.CurrentRow.Index].Value.ToString() != "")
            txt_pc.Text = opcodedata[1, opcodedata.CurrentRow.Index].Value.ToString();
            
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
                        loc = BinaryStringToHexString(bank0[2, 5].Value.ToString());
                    }
                    if (bank0[2, 4].Value.ToString().Substring(2, 1) == "1" && loc.StartsWith("0")) loc = "8" + loc.Substring(1);

                    int locrow = 1234567;
                    for (int i = 0; speicherzellen.Rows.Count > i; i++)
                    {
                        if (speicherzellen[1, i].Value.ToString() == loc)
                        {
                            locrow = i;
                        }
                    }

                    string speicherzellenval= speicherzellen[2, locrow].Value.ToString();
                    string w = bank0[2, 0].Value.ToString();
                    int addwf = bin2dec(speicherzellenval) + bin2dec(w);
                    if (addwf > 255)
                    {
                        carry = 1;
                        addwf = addwf - 256;
                    }
                    else carry = 0;

                    for (int i = 128; bin2dec(w) > 15 || bin2dec(speicherzellenval) > 15; i = i / 2)
                    {
                        if (bin2dec(w) >= i) w = dec2bin(bin2dec(w) - i);
                        if (bin2dec(speicherzellenval) >= i) speicherzellenval= dec2bin(bin2dec(speicherzellenval) - i);
                    }
                    int dc = bin2dec(speicherzellenval) + bin2dec(w);
                    if (dc > 15) digitcarry = 1;
                    else digitcarry = 0;

                    if (addwf == 0) zero = 1;
                    else zero = 0;

                    if (d == "1") speicherzellen[2, locrow].Value = dec2bin(addwf);
                    if (d == "0")
                    {
                        bank0[2, 0].Value = dec2bin(addwf);
                        bank1[2, 0].Value = dec2bin(addwf);
                    }
                    

                    break;
                case 1:
                    //ANDWF

                    d = binarystring.Substring(6, 1);
                    binarystring = binarystring.Substring(7);
                    loc = BinaryStringToHexString(binarystring);


                    if (loc == "00h")
                    {
                        loc = BinaryStringToHexString(bank0[2, 5].Value.ToString());
                    }
                    if (bank0[2, 4].Value.ToString().Substring(2, 1) == "1" && loc.StartsWith("0")) loc = "8" + loc.Substring(1);
                    locrow = 1234567;
                    for (int i = 0; speicherzellen.Rows.Count > i; i++)
                    {
                        if (speicherzellen[1, i].Value.ToString() == loc)
                        {
                            locrow = i;
                        }
                    }

                    speicherzellenval= speicherzellen[2, locrow].Value.ToString();
                    w = bank0[2, 0].Value.ToString();


                    string andwf = "";
                    for (int i = 7; i >= 0; i--)
                    {
                        if (speicherzellenval.Substring(i, 1) == "1" && w.Substring(i, 1) == "1")
                        {
                            andwf = "1" + andwf;
                        }
                        else
                        {
                            andwf = "0" + andwf;
                        }
                    }
                    if (andwf == "00000000") zero = 1;
                    else zero = 0;

                    if (d == "1") speicherzellen[2, locrow].Value = andwf;
                    if (d == "0")
                    {
                        bank0[2, 0].Value = andwf;
                        bank1[2, 0].Value = andwf;
                    }


                    break;
                case 2:
                    //CLRF
                    binarystring = binarystring.Substring(7);
                    loc = BinaryStringToHexString(binarystring);

                    if (loc == "00h")
                    {
                        loc = BinaryStringToHexString(bank0[2, 5].Value.ToString());
                    }
                    if (bank0[2, 4].Value.ToString().Substring(2, 1) == "1" && loc.StartsWith("0")) loc = "8" + loc.Substring(1);
                    locrow = 1234567;
                    bool ex = false;
                    for (int i = 0; speicherzellen.Rows.Count > i; i++)
                    {
                        if (speicherzellen[1, i].Value.ToString() == loc)
                        {
                            locrow = i;
                            ex = true;
                        }
                    }
                    if (ex)
                    {
                        speicherzellen[2, locrow].Value = "00000000";
                    }
                    else
                    {
                        speicherzellen.Rows.Add("", loc, "00000000");
                    }

                    zero = 1;

                    break;
                case 3:
                    //CLRW

                    bank0[2, 0].Value = "00000000";
                    bank1[2, 0].Value = "00000000";
                    zero = 1;

                    break;
                case 4:
                    //COMF
                    d = binarystring.Substring(6, 1);
                    binarystring = binarystring.Substring(7);
                    loc = BinaryStringToHexString(binarystring);

                    if (loc == "00h")
                    {
                        loc = BinaryStringToHexString(bank0[2, 5].Value.ToString());
                    }
                    if (bank0[2, 4].Value.ToString().Substring(2, 1) == "1" && loc.StartsWith("0")) loc = "8" + loc.Substring(1);
                    locrow = 1234567;
                    for (int i = 0; speicherzellen.Rows.Count > i; i++)
                    {
                        if (speicherzellen[1, i].Value.ToString() == loc)
                        {
                            locrow = i;
                        }
                    }
                    string v;
                    string f = speicherzellen[2, locrow].Value.ToString();
                    string comf = "";
                    for (int i = 0; i < 8; i++)
                    {
                        if (f.Substring(i, 1) == "1") v = "0";
                        else v = "1";
                        comf = comf + v;
                    }
                    if (comf == "00000000") zero = 1;
                    else zero = 0;

                    if (d == "1") speicherzellen[2, locrow].Value = comf;
                    else
                    {
                        bank0[2, 0].Value = comf;
                        bank1[2, 0].Value = comf;
                    }




                    break;
                case 5:
                    //DECF
                    d = binarystring.Substring(6, 1);
                    binarystring = binarystring.Substring(7);
                    loc = BinaryStringToHexString(binarystring);

                    if (loc == "00h")
                    {
                        loc = BinaryStringToHexString(bank0[2, 5].Value.ToString());
                    }
                    if (bank0[2, 4].Value.ToString().Substring(2, 1) == "1" && loc.StartsWith("0")) loc = "8" + loc.Substring(1);
                    locrow = 1234567;
                    for (int i = 0; speicherzellen.Rows.Count > i; i++)
                    {
                        if (speicherzellen[1, i].Value.ToString() == loc)
                        {
                            locrow = i;
                        }
                    }
                    int decf = bin2dec(speicherzellen[2, locrow].Value.ToString()) - 1;
                    if (decf < 0) decf = 255;
                    if (decf == 0) zero = 1;
                    else zero = 0;
                    if (d == "1") speicherzellen[2, locrow].Value = dec2bin(decf);
                    if (d == "0")
                    {
                        bank0[2, 0].Value = dec2bin(decf);
                        bank1[2, 0].Value = dec2bin(decf);
                    }

                    break;
                case 6:
                    //DECFSZ

                    d = binarystring.Substring(6, 1);
                    binarystring = binarystring.Substring(7);
                    loc = BinaryStringToHexString(binarystring);
                    if (loc == "00h")
                    {
                        loc = BinaryStringToHexString(bank0[2, 5].Value.ToString());
                    }
                    if (bank0[2, 4].Value.ToString().Substring(2, 1) == "1" && loc.StartsWith("0")) loc = "8" + loc.Substring(1);
                    locrow = 1234567;
                    for (int i = 0; speicherzellen.Rows.Count > i; i++)
                    {
                        if (speicherzellen[1, i].Value.ToString() == loc)
                        {
                            locrow = i;
                        }
                    }
                    int decfsz = bin2dec(speicherzellen[2, locrow].Value.ToString()) - 1;
                    if (decfsz < 0) decfsz = 255;
                    if (d == "1") speicherzellen[2, locrow].Value = dec2bin(decfsz);
                    if (d == "0")
                    {
                        bank0[2, 0].Value = dec2bin(decfsz);
                        bank1[2, 0].Value = dec2bin(decfsz);
                    }
                    if (decfsz == 0)
                    {
                        befehlnr++;
                        TimerWertbefehl++;
                        zero = 1;
                        //rownr = rownr + 1;
                        opcodedata.CurrentRow.DefaultCellStyle.BackColor = Color.White;
                        opcodedata.CurrentCell = opcodedata[0, opcodedata.CurrentRow.Index + 1];
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
                        loc = BinaryStringToHexString(bank0[2, 5].Value.ToString());
                    }
                    if (bank0[2, 4].Value.ToString().Substring(2, 1) == "1" && loc.StartsWith("0")) loc = "8" + loc.Substring(1);
                    locrow = 1234567;
                    for (int i = 0; speicherzellen.Rows.Count > i; i++)
                    {
                        if (speicherzellen[1, i].Value.ToString() == loc)
                        {
                            locrow = i;
                        }
                    }

                    int incf = bin2dec(speicherzellen[2, locrow].Value.ToString()) + 1;
                    if (incf > 255) incf = 0; 
                    if (incf == 0) zero = 1;
                    else zero = 0;
                    if (d == "1") speicherzellen[2, locrow].Value = dec2bin(incf);
                    if (d == "0")
                    {
                        bank0[2, 0].Value = dec2bin(incf);
                        bank1[2, 0].Value = dec2bin(incf);
                    }

                    break;
                case 8:
                    //INCFSZ

                    d = binarystring.Substring(6, 1);
                    binarystring = binarystring.Substring(7);
                    loc = BinaryStringToHexString(binarystring);
                    if (loc == "00h")
                    {
                        loc = BinaryStringToHexString(bank0[2, 5].Value.ToString());
                    }
                    if (bank0[2, 4].Value.ToString().Substring(2, 1) == "1" && loc.StartsWith("0")) loc = "8" + loc.Substring(1);
                    locrow = 1234567;
                    for (int i = 0; speicherzellen.Rows.Count > i; i++)
                    {
                        if (speicherzellen[1, i].Value.ToString() == loc)
                        {
                            locrow = i;
                        }
                    }
                    int incfsz = bin2dec(speicherzellen[2, locrow].Value.ToString()) + 1;
                    if (incfsz > 255) incfsz = 0;
                    if (d == "1") speicherzellen[2, locrow].Value = dec2bin(incfsz);
                    if (d == "0")
                    {
                        bank0[2, 0].Value = dec2bin(incfsz);
                        bank1[2, 0].Value = dec2bin(incfsz);
                    }
                    if (incfsz == 0)
                    {
                        befehlnr++;
                        TimerWertbefehl++;
                        zero = 1;
                        opcodedata.CurrentRow.DefaultCellStyle.BackColor = Color.White;
                        opcodedata.CurrentCell = opcodedata[0, opcodedata.CurrentRow.Index + 1];

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
                        loc = BinaryStringToHexString(bank0[2, 5].Value.ToString());
                    }
                    if (bank0[2, 4].Value.ToString().Substring(2, 1) == "1" && loc.StartsWith("0")) loc = "8" + loc.Substring(1);
                    locrow = 1234567;
                    for (int i = 0; speicherzellen.Rows.Count > i; i++)
                    {
                        if (speicherzellen[1, i].Value.ToString() == loc)
                        {
                            locrow = i;
                        }
                    }

                    binarystring = binarystring.Substring(6);
                    string iorwf = "";
                    for (int i = 7; i >= 0; i--)
                    {
                        if (speicherzellen[2, locrow].Value.ToString().Substring(i, 1) == "1" || bank0[2, 0].Value.ToString().Substring(i, 1) == "1")
                        {
                            iorwf = "1" + iorwf;
                        }
                        else
                        {
                            iorwf = "0" + iorwf;
                        }
                    }
                    if (iorwf == "00000000") zero = 1;
                    else zero = 0;
                    if (d == "0")
                    {
                        bank0[2, 0].Value = iorwf;
                        bank1[2, 0].Value = iorwf;
                    }
                    else speicherzellen[2, locrow].Value = iorwf;

                    break;
                case 10:
                    //MOVF
                    d = binarystring.Substring(6, 1);
                    binarystring = binarystring.Substring(7);
                    loc = BinaryStringToHexString(binarystring);
                    if (loc == "00h")
                    {
                        loc = BinaryStringToHexString(bank0[2, 5].Value.ToString());
                    }
                    if (bank0[2, 4].Value.ToString().Substring(2, 1) == "1" && loc.StartsWith("0")) loc = "8" + loc.Substring(1);
                    locrow = 1234567;
                    for (int i = 0; speicherzellen.Rows.Count > i; i++)
                    {
                        if (speicherzellen[1, i].Value.ToString() == loc)
                        {
                            locrow = i;
                        }
                    }
                    if (locrow != 1234567)
                    {
                        if (speicherzellen[2, locrow].Value.ToString() == "00000000")
                            zero = 1;
                        else zero = 0;
                        if (d == "0")
                        {
                            bank0[2, 0].Value = speicherzellen[2, locrow].Value.ToString();
                            bank1[2, 0].Value = speicherzellen[2, locrow].Value.ToString();
                        }
                    }
                    else
                    {
                        for (int i = 0; bank0.Rows.Count > i; i++)
                        {
                            if (bank0[1, i].Value.ToString() == loc)
                            {
                                locrow = i;
                            }
                        }
                        if (locrow != 1234567)
                        {
                            if (bank0[2, locrow].Value.ToString() == "00000000")
                                zero = 1;
                            else zero = 0;
                            if (d == "0")
                            {
                                bank0[2, 0].Value = bank0[2, locrow].Value.ToString();
                                bank1[2, 0].Value = bank0[2, locrow].Value.ToString();
                            }
                        }
                        else
                        {
                            for (int i = 0; bank1.Rows.Count > i; i++)
                            {
                                if (bank0[1, i].Value.ToString() == loc)
                                {
                                    locrow = i;
                                }
                            }
                            if (locrow != 1234567)
                            {
                                if (bank1[2, locrow].Value.ToString() == "00000000")
                                    zero = 1;
                                else zero = 0;
                                if (d == "0")
                                {
                                    bank0[2, 0].Value = bank1[2, locrow].Value.ToString();
                                    bank1[2, 0].Value = bank1[2, locrow].Value.ToString();
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
                        loc = BinaryStringToHexString(bank0[2, 5].Value.ToString());
                    }
                    if (bank0[2, 4].Value.ToString().Substring(2, 1) == "1" && loc.StartsWith("0")) loc = "8" + loc.Substring(1);
                    bool exist = false;
                    int locrow1 = 1234567;
                    for (int i = 0; speicherzellen.Rows.Count > i; i++)
                    {
                        if (speicherzellen[1, i].Value.ToString() == loc)
                        {
                            locrow1 = i;
                            exist = true;
                        }
                    }
                    if (exist)
                    {
                        speicherzellen[2, locrow1].Value = bank0[2, 0].Value.ToString();
                    }
                    else
                    {
                        speicherzellen.Rows.Add("", loc, bank0[2, 0].Value.ToString());
                    }
                    for (int i = 0; i < bank1.RowCount; i++)
                    {
                        string a = speicherzellen[1, speicherzellen.RowCount - 1].Value.ToString();
                        string c = bank1[1, i].Value.ToString(); ;
                        if (speicherzellen[1, speicherzellen.RowCount - 1].Value.ToString() == bank1[1, i].Value.ToString())
                        {
                            bank1[2, i].Value = speicherzellen[2, speicherzellen.RowCount - 1].Value;
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
                        loc = BinaryStringToHexString(bank0[2, 5].Value.ToString());
                    }
                    if (bank0[2, 4].Value.ToString().Substring(2, 1) == "1" && loc.StartsWith("0")) loc = "8" + loc.Substring(1);
                    locrow = 123456;
                    for (int i = 0; speicherzellen.Rows.Count > i; i++)
                    {
                        if (speicherzellen[1, i].Value.ToString() == loc)
                        {
                            locrow = i;
                        }
                    }
                    string shift = speicherzellen[2, locrow].Value.ToString() + carry.ToString();
                    carry = Convert.ToInt32(shift.Substring(0, 1));
                    shift = shift.Substring(1);

                    if (shift == "00000000") zero = 1;
                    else zero = 0;


                    if (d == "1") speicherzellen[2, locrow].Value = shift;
                    if (d == "0")
                    {
                        bank0[2, 0].Value = shift;
                        bank1[2, 0].Value = shift;
                    }

                    break;
                case 14:
                    //RRF     
                    d = binarystring.Substring(6, 1);
                    binarystring = binarystring.Substring(7);
                    loc = BinaryStringToHexString(binarystring);
                    if (loc == "00h")
                    {
                        loc = BinaryStringToHexString(bank0[2, 5].Value.ToString());
                    }
                    if (bank0[2, 4].Value.ToString().Substring(2, 1) == "1" && loc.StartsWith("0")) loc = "8" + loc.Substring(1);
                    locrow = 123456;
                    for (int i = 0; speicherzellen.Rows.Count > i; i++)
                    {
                        if (speicherzellen[1, i].Value.ToString() == loc)
                        {
                            locrow = i;
                        }
                    }
                    shift = carry.ToString() + speicherzellen[2, locrow].Value.ToString();
                    carry = Convert.ToInt32(shift.Substring(8, 1));
                    shift = shift.Substring(0, 8);
                    if (shift == "00000000") zero = 1;
                    else zero = 0;


                    if (d == "1") speicherzellen[2, locrow].Value = shift;
                    if (d == "0")
                    {
                        bank0[2, 0].Value = shift;
                        bank1[2, 0].Value = shift;
                    }



                    break;
                case 15:
                    //SUBWF

                    d = binarystring.Substring(6, 1);
                    binarystring = binarystring.Substring(7);
                    loc = BinaryStringToHexString(binarystring);
                    if (loc == "00h")
                    {
                        loc = BinaryStringToHexString(bank0[2, 5].Value.ToString());
                    }
                    if (bank0[2, 4].Value.ToString().Substring(2, 1) == "1" && loc.StartsWith("0")) loc = "8" + loc.Substring(1);
                    locrow = 123456;
                    for (int i = 0; speicherzellen.Rows.Count > i; i++)
                    {
                        if (speicherzellen[1, i].Value.ToString() == loc)
                        {
                            locrow = i;
                        }
                    }
                    int sublw = bin2dec(speicherzellen[2, locrow].Value.ToString()) - bin2dec(bank0[2, 0].Value.ToString());

                    if (sublw < 0)
                    {
                        carry = 0;
                        sublw = 256 + sublw;
                    }
                    else carry = 1;

                    w = bank0[2, 0].Value.ToString();
                    speicherzellenval= speicherzellen[2, locrow].Value.ToString();
                    for (int i = 128; bin2dec(w) > 15 || bin2dec(speicherzellenval) > 15; i = i / 2)
                    {
                        if (bin2dec(w) >= i) w = dec2bin(bin2dec(w) - i);
                        if (bin2dec(speicherzellenval) >= i) speicherzellenval= dec2bin(bin2dec(speicherzellenval) - i);
                    }
                    dc = bin2dec(speicherzellenval) - bin2dec(w);
                    if (dc < 0) digitcarry = 0;
                    else digitcarry = 1;

                    if (sublw == 0) zero = 1;
                    else zero = 0;
                    if (d == "1") speicherzellen[2, locrow].Value = dec2bin(sublw);
                    if (d == "0")
                    {
                        bank0[2, 0].Value = dec2bin(sublw);
                        bank1[2, 0].Value = dec2bin(sublw);
                    }


                    break;
                case 16:
                    //SWAPF
                    d = binarystring.Substring(6, 1);
                    binarystring = binarystring.Substring(7);
                    loc = BinaryStringToHexString(binarystring);
                    if (loc == "00h")
                    {
                        loc = BinaryStringToHexString(bank0[2, 5].Value.ToString());
                    }
                    if (bank0[2, 4].Value.ToString().Substring(2, 1) == "1" && loc.StartsWith("0")) loc = "8" + loc.Substring(1);
                    locrow = 123456;
                    for (int i = 0; speicherzellen.Rows.Count > i; i++)
                    {
                        if (speicherzellen[1, i].Value.ToString() == loc)
                        {
                            locrow = i;
                        }
                    }
                    string swap = speicherzellen[2, locrow].Value.ToString().Substring(4) + speicherzellen[2, locrow].Value.ToString().Substring(0, 4);



                    if (d == "1") speicherzellen[2, locrow].Value = swap;
                    if (d == "0")
                    {
                        bank0[2, 0].Value = swap;
                        bank1[2, 0].Value = swap;
                    }

                    break;
                case 17:
                    //XORWF

                    d = binarystring.Substring(6, 1);
                    binarystring = binarystring.Substring(7);
                    loc = BinaryStringToHexString(binarystring);
                    if (loc == "00h")
                    {
                        loc = BinaryStringToHexString(bank0[2, 5].Value.ToString());
                    }
                    if (bank0[2, 4].Value.ToString().Substring(2, 1) == "1" && loc.StartsWith("0")) loc = "8" + loc.Substring(1);
                    locrow = 123456;
                    for (int i = 0; speicherzellen.Rows.Count > i; i++)
                    {
                        if (speicherzellen[1, i].Value.ToString() == loc)
                        {
                            locrow = i;
                        }
                    }
                    string xorwf = "";
                    for (int i = 7; i >= 0; i--)
                    {
                        if ((speicherzellen[2, locrow].Value.ToString().Substring(i, 1) == "1" && bank0[2, 0].Value.ToString().Substring(i, 1) == "0") || (bank0[2, 0].Value.ToString().Substring(i, 1) == "1" && speicherzellen[2, locrow].Value.ToString().Substring(i, 1) == "0"))
                        {
                            xorwf = "1" + xorwf;
                        }
                        else
                        {
                            xorwf = "0" + xorwf;
                        }
                    }
                    if (xorwf == "00000000") zero = 1;
                    else zero = 0;

                    if (d == "1") speicherzellen[2, locrow].Value = xorwf;
                    if (d == "0")
                    {
                        bank0[2, 0].Value = xorwf;
                        bank1[2, 0].Value = xorwf;
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
                        loc = BinaryStringToHexString(bank0[2, 5].Value.ToString());
                    }
                    if (bank0[2, 4].Value.ToString().Substring(2, 1) == "1" && loc.StartsWith("0")) loc = "8" + loc.Substring(1);
                    locrow = 1234567;
                    for (int i = 0; speicherzellen.Rows.Count > i; i++)
                    {
                        if (speicherzellen[1, i].Value.ToString() == loc)
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
                            bcf = speicherzellen[2, locrow].Value.ToString().Substring(0, b) + "0" + speicherzellen[2, locrow].Value.ToString().Substring(b + 1);
                        }
                        else
                        {
                            if (b == 0)
                                bcf = "0" + speicherzellen[2, locrow].Value.ToString().Substring(b + 1);
                            else bcf = bcf = speicherzellen[2, locrow].Value.ToString().Substring(0, b) + "0";
                        }
                        if (bcf == "00000000") zero = 1;
                        else zero = 0;
                        speicherzellen[2, locrow].Value = bcf;
                    }
                    else
                    {
                        for (int i = 0; bank1.Rows.Count > i; i++)
                        {
                            if (bank1[1, i].Value.ToString() == loc)
                            {
                                locrow = i;
                            }

                        }
                        if (locrow != 1234567)
                        {
                            b = 7 - b;
                            bcf = "";
                            string s = bank1[2, locrow].Value.ToString();
                            if (b > 0 && b < 7)
                            {
                                bcf = bank1[2, locrow].Value.ToString().Substring(0, b) + "0" + bank1[2, locrow].Value.ToString().Substring(b + 1);
                            }
                            else
                            {
                                if (b == 0)
                                    bcf = "0" + bank1[2, locrow].Value.ToString().Substring(b + 1);
                                else bcf = bcf = bank1[2, locrow].Value.ToString().Substring(0, b) + "0";
                            }
                            if (bcf == "00000000") zero = 1;
                            else zero = 0;
                            bank1[2, locrow].Value = bcf;
                            if (bank1[2,4].Value.ToString() == bcf)
                            {
                                bank0[2, 4].Value = bank1[2, 4].Value.ToString();
                                for (int i = 0; i < speicherzellen.RowCount; i++)
                                {
                                    int testrow = i;
                                    for (j = 0; j < bank1.RowCount; j++)
                                    {
                                        if (bank1[1, j].Value.ToString() == speicherzellen[1, i].Value.ToString())
                                        {
                                            speicherzellen[2,i].Value = bank1[2, j].Value.ToString();
                                        }
                                        if (bank0[1, j].Value.ToString() == speicherzellen[1, i].Value.ToString())
                                        {
                                            speicherzellen[2, i].Value = bank0[2, j].Value.ToString();
                                        }
                                    }
                                }

                            }
                        }
                        else
                        {
                            for (int i = 0; bank1.Rows.Count > i; i++)
                            {
                                if (bank0[1, i].Value.ToString() == loc)
                                {
                                    locrow = i;
                                }

                            }
                            if (locrow != 1234567)
                            {
                                b = 7 - b;
                                bcf = "";
                                string s = bank0[2, locrow].Value.ToString();
                                if (b > 0 && b < 7)
                                {
                                    bcf = bank0[2, locrow].Value.ToString().Substring(0, b) + "0" + bank0[2, locrow].Value.ToString().Substring(b + 1);
                                }
                                else
                                {
                                    if (b == 0)
                                        bcf = "0" + bank0[2, locrow].Value.ToString().Substring(b + 1);
                                    else bcf = bcf = bank0[2, locrow].Value.ToString().Substring(0, b) + "0";
                                }
                                if (bcf == "00000000") zero = 1;
                                else zero = 0;
                                bank0[2, locrow].Value = bcf;
                                for (int i = 0; i < speicherzellen.RowCount; i++)
                                {
                                    int testrow = i;
                                    for (j = 0; j < bank1.RowCount; j++)
                                    {
                                        if (bank1[1, j].Value.ToString() == speicherzellen[1, i].Value.ToString())
                                        {
                                            speicherzellen[2, j].Value = bank1[2, i].Value.ToString();
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
                        loc = BinaryStringToHexString(bank0[2, 5].Value.ToString());
                    }
                    if (bank0[2, 4].Value.ToString().Substring(2, 1) == "1" && loc.StartsWith("0")) loc = "8" + loc.Substring(1);
                    locrow = 1234567;
                    for (int i = 0; speicherzellen.Rows.Count > i; i++)
                    {
                        if (speicherzellen[1, i].Value.ToString() == loc)
                        {
                            locrow = i;
                        }

                    }
                    if (locrow != 1234567)
                    {
                        b = 7 - b;
                        bsf= "";
                        string s = speicherzellen[2, locrow].Value.ToString();
                        if (b > 0 && b < 7)
                        {
                            bsf= speicherzellen[2, locrow].Value.ToString().Substring(0, b) + "1" + speicherzellen[2, locrow].Value.ToString().Substring(b + 1);
                        }
                        else
                        {
                            if (b == 0)
                                bsf= "1" + speicherzellen[2, locrow].Value.ToString().Substring(b + 1);
                            else bsf= bsf= speicherzellen[2, locrow].Value.ToString().Substring(0, b) + "1";
                        }
                        if (bsf== "00000000") zero = 1;
                        else zero = 0;
                        speicherzellen[2, locrow].Value = bsf;
                    }
                    else
                    {
                        for (int i = 0; bank1.Rows.Count > i; i++)
                        {
                            if (bank1[1, i].Value.ToString() == loc)
                            {
                                locrow = i;
                            }

                        }
                        if (locrow != 1234567)
                        {
                            b = 7 - b;
                            bsf= "";
                            string s = bank1[2, locrow].Value.ToString();
                            if (b > 0 && b < 7)
                            {
                                bsf= bank1[2, locrow].Value.ToString().Substring(0, b) + "1" + bank1[2, locrow].Value.ToString().Substring(b + 1);
                            }
                            else
                            {
                                if (b == 0)
                                    bsf= "1" + bank1[2, locrow].Value.ToString().Substring(b + 1);
                                else bsf= bsf= bank1[2, locrow].Value.ToString().Substring(0, b) + "1";
                            }
                            if (bsf== "00000000") zero = 1;
                            else zero = 0;
                            bank1[2, locrow].Value = bsf;
                            if (bank1[2, 4].Value.ToString() == bsf)
                            {
                                bank0[2, 4].Value = bank1[2, 4].Value.ToString();
                                for (int i = 0; i < speicherzellen.RowCount; i++)
                                {
                                    int testrow = i;
                                    for (j = 0; j < bank1.RowCount; j++)
                                    {
                                        if (bank1[1, j].Value.ToString() == speicherzellen[1, i].Value.ToString())
                                        {
                                            speicherzellen[2, i].Value = bank1[2, j].Value.ToString();
                                        }
                                        if (bank0[1, j].Value.ToString() == speicherzellen[1, i].Value.ToString())
                                        {
                                            speicherzellen[2, i].Value = bank0[2, j].Value.ToString();
                                        }
                                    }
                                }

                            }
                        }
                        else
                        {
                            for (int i = 0; bank1.Rows.Count > i; i++)
                            {
                                if (bank0[1, i].Value.ToString() == loc)
                                {
                                    locrow = i;
                                }

                            }
                            if (locrow != 1234567)
                            {
                                b = 7 - b;
                                bsf= "";
                                string s = bank0[2, locrow].Value.ToString();
                                if (b > 0 && b < 7)
                                {
                                    bsf= bank0[2, locrow].Value.ToString().Substring(0, b) + "1" + bank0[2, locrow].Value.ToString().Substring(b + 1);
                                }
                                else
                                {
                                    if (b == 0)
                                        bsf= "1" + bank0[2, locrow].Value.ToString().Substring(b + 1);
                                    else bsf= bsf= bank0[2, locrow].Value.ToString().Substring(0, b) + "1";
                                }
                                if (bsf== "00000000") zero = 1;
                                else zero = 0;
                                bank0[2, locrow].Value = bsf;
                                for (int i = 0; i < speicherzellen.RowCount; i++)
                                {
                                    int testrow = i;
                                    for (j=0;j<bank0.RowCount;j++)
                                    {
                                        if (bank0[1,j].Value.ToString() == speicherzellen[1,i].Value.ToString())
                                        {
                                            speicherzellen[2, j].Value = bank0[2, i].Value.ToString();
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
                        loc = BinaryStringToHexString(bank0[2, 5].Value.ToString());
                    }
                    if (bank0[2, 4].Value.ToString().Substring(2, 1) == "1" && loc.StartsWith("0")) loc = "8" + loc.Substring(1);
                    locrow = 1234567;
                    for (int i = 0; speicherzellen.Rows.Count > i; i++)
                    {
                        if (speicherzellen[1, i].Value.ToString() == loc)
                        {
                            locrow = i;
                        }
                    }
                    b = 7 - b;
                    if (speicherzellen[2, locrow].Value.ToString().Substring(b, 1) == "0")
                    {
                        befehlnr++;
                        TimerWertbefehl++;
                        opcodedata.CurrentRow.DefaultCellStyle.BackColor = Color.White;
                        opcodedata.CurrentCell = opcodedata[0, opcodedata.CurrentRow.Index + 1];
                        //dataGridView1.Rows[j - 1].DefaultCellStyle.BackColor = Color.White;
                    }
                    break;
                case 22:
                    //ADDLW

                    binarystring = binarystring.Substring(6);
                    string w3 = bank0[2, 0].Value.ToString();
                    int addlw = bin2dec(binarystring) + bin2dec(w3);
                    if (addlw > 255)
                    {
                        addlw = addlw - 256;
                        carry = 1;
                    }
                    else carry = 0;

                    w = bank0[2, 0].Value.ToString();
                    speicherzellenval= binarystring;
                    for (int i = 128; bin2dec(w) > 15 || bin2dec(speicherzellenval) > 15; i = i / 2)
                    {
                        if (bin2dec(w) >= i) w = dec2bin(bin2dec(w) - i);
                        if (bin2dec(speicherzellenval) >= i) speicherzellenval= dec2bin(bin2dec(speicherzellenval) - i);
                    }
                    dc = bin2dec(speicherzellenval) + bin2dec(w);
                    if (dc > 15) digitcarry = 1;
                    else digitcarry = 0;

                    if (addlw == 0) zero = 1;
                    else zero = 0;
                    bank0[2, 0].Value = dec2bin(addlw);
                    bank1[2, 0].Value = dec2bin(addlw);

                    break;
                case 23:
                    //ANDLW

                    binarystring = binarystring.Substring(6);
                    string w1 = bank0[2, 0].Value.ToString();
                    string andlw = "";
                    for (int i = 7; i >= 0; i--)
                    {
                        if (binarystring.Substring(i, 1) == "1" && w1.Substring(i, 1) == "1")
                        {
                            andlw = "1" + andlw;
                        }
                        else
                        {
                            andlw = "0" + andlw;
                        }
                    }
                    if (andlw == "00000000") zero = 1;
                    else zero = 0;
                    bank0[2, 0].Value = andlw;
                    bank1[2, 0].Value = andlw;
                    break;

                case 24:
                    //CALL
                    befehlnr++;
                    TimerWertbefehl++;
                    binarystring = binarystring.Substring(3);

                    string addressc = BinaryStringToHexString(binarystring);
                    int backrow = row;
                    string call = opcodedata.Rows[row].Cells[3].Value.ToString().Substring(5);

                    for (int m = 0; m < opcodedata.RowCount; m++)
                    {
                        string opaddress = opcodedata.Rows[m].Cells[1].Value.ToString();
                        if (!opaddress.EndsWith("h")) opaddress = opaddress + "h";
                        while (opaddress.Length > 3) opaddress = opaddress.Substring(1);
                        if (opaddress == addressc)
                        {
                            rownr = m;
                        }
                    }

                    //  dataGridView1.Rows[row].DefaultCellStyle.BackColor = Color.White;
                    returnrow = row;
                    txt_stack.Text = opcodedata[1, row].Value.ToString();
                    return rownr -1;

                    break;

                case 25:
                    //CLRWDT

                    to = 1;
                    pd = 1;

                    break;
                case 26:
                    //GOTO
                    befehlnr++;
                    TimerWertbefehl++;
                    binarystring = binarystring.Substring(3);

                    string address = BinaryStringToHexString(binarystring);


                    for (int m = 0; m < opcodedata.RowCount; m++)
                    {
                        string opaddress = opcodedata.Rows[m].Cells[1].Value.ToString();
                        if (!opaddress.EndsWith("h")) opaddress = opaddress + "h";
                        while (opaddress.Length > 3) opaddress = opaddress.Substring(1);
                        if (opaddress == address)
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
                    string w2 = bank0[2, 0].Value.ToString();
                    string iorlw = "";
                    for (int i = 7; i >= 0; i--)
                    {
                        if (binarystring.Substring(i, 1) == "1" || w2.Substring(i, 1) == "1")
                        {
                            iorlw = "1" + iorlw;
                        }
                        else
                        {
                            iorlw = "0" + iorlw;
                        }
                    }
                    if (iorlw == "00000000") zero = 1;
                    else zero = 0;
                    bank0[2, 0].Value = iorlw;
                    bank1[2, 0].Value = iorlw;

                    break;
                case 28:
                    //MOVLW

                    binarystring = binarystring.Substring(6);
                    bank0[2, 0].Value = binarystring;
                    bank1[2, 0].Value = binarystring;

                    break;
                case 29:
                    //RETFIE
                    befehlnr++;
                    TimerWertbefehl++;
                    bank0[2, 11].Value = "1" + bank0[2, 11].Value.ToString().Substring(1);

                    break;
                case 30:
                    //RETLW
                    befehlnr++;
                    TimerWertbefehl++;
                    binarystring = binarystring.Substring(6);
                    bank0[2, 0].Value = binarystring;
                    bank1[2, 0].Value = binarystring;
                    int sendreturnrow = returnrow + 1;
                    returnrow = 1234567;
                    txt_stack.Text = "";
                    return sendreturnrow;

                    break;
                case 31:
                    //RETURN
                    befehlnr++;
                    TimerWertbefehl++;
                    sendreturnrow = returnrow + 1;
                    returnrow = 1234567;
                    txt_stack.Text = "";
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
                    string w5 = bank0[2, 0].Value.ToString();
                    sublw = bin2dec(binarystring) - bin2dec(w5);

                    if (sublw < 0)
                    {
                        carry = 0;
                        sublw = 256 + sublw;
                    }
                    else carry = 1;

                    w = bank0[2, 0].Value.ToString();
                    speicherzellenval= binarystring;
                    for (int i = 128; bin2dec(w) > 15 || bin2dec(speicherzellenval) > 15; i = i / 2)
                    {
                        if (bin2dec(w) >= i) w = dec2bin(bin2dec(w) - i);
                        if (bin2dec(speicherzellenval) >= i) speicherzellenval= dec2bin(bin2dec(speicherzellenval) - i);
                    }
                    dc = bin2dec(speicherzellenval) - bin2dec(w);
                    if (dc < 0) digitcarry = 0;
                    else digitcarry = 1;

                    if (sublw == 0) zero = 1;
                    else zero = 0;

                    bank0[2, 0].Value = dec2bin(sublw);
                    bank1[2, 0].Value = dec2bin(sublw);
                    //l-w=w


                    break;
                case 34:
                    //XORLW

                    binarystring = binarystring.Substring(6);
                    string w4 = bank0[2, 0].Value.ToString();
                    string xorlw = "";
                    for (int i = 7; i >= 0; i--)
                    {
                        if ((binarystring.Substring(i, 1) == "1" && w4.Substring(i, 1) == "0") || (w4.Substring(i, 1) == "1" && binarystring.Substring(i, 1) == "0"))
                        {
                            xorlw = "1" + xorlw;
                        }
                        else
                        {
                            xorlw = "0" + xorlw;
                        }
                    }
                    if (xorlw == "00000000") zero = 1;
                    else zero = 0;
                    bank0[2, 0].Value = xorlw;
                    bank1[2, 0].Value = xorlw;

                    break;

                case 21:
                    //BTFSS

                    b = bin2dec(binarystring.Substring(4, 3));
                    binarystring = binarystring.Substring(7);
                    loc = BinaryStringToHexString(binarystring);
                    if (loc == "00h")
                    {
                        loc = BinaryStringToHexString(bank0[2, 5].Value.ToString());
                    }
                    if (bank0[2, 4].Value.ToString().Substring(2, 1) == "1" && loc.StartsWith("0")) loc = "8" + loc.Substring(1);
                    locrow = 1234567;
                    for (int i = 0; speicherzellen.Rows.Count > i; i++)
                    {
                        if (speicherzellen[1, i].Value.ToString() == loc)
                        {
                            locrow = i;
                        }
                    }
                    b = 7 - b;
                    if (locrow != 1234567)
                    {
                        if (speicherzellen[2, locrow].Value.ToString().Substring(b, 1) == "1")
                        {
                            befehlnr++;
                            TimerWertbefehl++;
                            opcodedata.CurrentRow.DefaultCellStyle.BackColor = Color.White;
                            opcodedata.CurrentCell = opcodedata[0, opcodedata.CurrentRow.Index + 1];
                        }
                    }
                    else
                    {
                        for (int i = 0; bank0.Rows.Count > i; i++)
                        {
                            if (bank0[1, i].Value.ToString() == loc)
                            {
                                locrow = i;
                            }
                        }
                        if (locrow != 1234567)
                        {
                            if (bank0[2, locrow].Value.ToString().Substring(b, 1) == "1")
                            {
                                befehlnr++;
                                TimerWertbefehl++;
                                opcodedata.CurrentRow.DefaultCellStyle.BackColor = Color.White;
                                opcodedata.CurrentCell = opcodedata[0, opcodedata.CurrentRow.Index + 1];
                            }
                        }
                        else
                        {
                            for (int i = 0; bank1.Rows.Count > i; i++)
                            {
                                if (bank1[1, i].Value.ToString() == loc)
                                {
                                    locrow = i;
                                }
                            }
                            if (locrow != 1234567)
                            {
                                if (bank1[2, locrow].Value.ToString().Substring(b, 1) == "1")
                                {
                                    befehlnr++;
                                    TimerWertbefehl++;
                                    opcodedata.CurrentRow.DefaultCellStyle.BackColor = Color.White;
                                    opcodedata.CurrentCell = opcodedata[0, opcodedata.CurrentRow.Index + 1];
                                }
                            }
                        }
                    }
                    
                    break;

                default:
                    break;
            }

            clcktimer();
            for(int i = 0; i<speicherzellen.RowCount;i++)
            {
                if (speicherzellen[1,i].Value.ToString() == bank0[1,11].Value.ToString())
                {
                    speicherzellen[2, i].Value = bank0[2, 11].Value.ToString();
                }
            }
            
            //überschrieben der banken mit den werten der speicherzellen
            for (int i = 0; i < speicherzellen.RowCount; i++)
            {
                string locs = speicherzellen[1, i].Value.ToString();
                for (int j = 0; j < bank0.RowCount; j++)
                {
                    if (bank0[1, j].Value.ToString() == locs)
                    {
                        bank0[2, j].Value = speicherzellen[2, i].Value.ToString();
                    }
                    if (bank1[1, j].Value.ToString() == locs)
                    {
                        bank1[2, j].Value = speicherzellen[2, i].Value.ToString();
                    }
                }
            }

            setinputoutput();

            
            //if (rbie == 1)
            //{
            //    bank0[2, 11].Value = bank0[2, 11].Value.ToString().Substring(0, 7) + "1";
            //    bank1[2, 11].Value = bank0[2, 11].Value.ToString();
            //}
            //else
            //{
            //    bank0[2, 11].Value = bank0[2, 11].Value.ToString().Substring(0, 7) + "0";
            //    bank1[2, 11].Value = bank0[2, 11].Value.ToString();
            //}

            //interruptbit abhängig von portb
            if (bank0[2,7].Value.ToString().Substring(0,1) == "1")
            {
                bank0[2, 11].Value = bank0[2, 11].Value.ToString().Substring(0, 6) + "1" + bank0[2,11].Value.ToString().Substring(7);
                bank1[2, 11].Value = bank0[2, 11].Value.ToString();
            }
            else
            {
                bank0[2, 11].Value = bank0[2, 11].Value.ToString().Substring(0, 6) + "0" + bank0[2, 11].Value.ToString().Substring(7);
                bank1[2, 11].Value = bank0[2, 11].Value.ToString();
            }

            //setze Status register
            setstatusreg();

            for (int i = 0; i<speicherzellen.RowCount;i++)
            {
                if(speicherzellen[1,i].Value.ToString() == bank0[1,4].Value.ToString())
                {
                    speicherzellen[2, i].Value = bank0[2, 4].Value;
                }
                if (speicherzellen[1, i].Value.ToString() == bank1[1, 4].Value.ToString())
                {
                    speicherzellen[2, i].Value = bank1[2, 4].Value;
                }
            }
            //Frage Bits im Interrupt Register ab
            testifinterrupt();

            return 1234567;
        }

        //timer, der bei befehlsdurchlauf läuft
        private void clcktimer()
        {
            String Prescaler_bits = bank1[2, 2].Value.ToString().Substring(5, 1) + bank1[2, 2].Value.ToString().Substring(6, 1) + bank1[2, 2].Value.ToString().Substring(7, 1);

            if (bank1[2, 2].Value.ToString().Substring(2, 1) == "0")
            {
                if (bank1[2, 2].Value.ToString().Substring(4, 1) == "1")
                {

                    switch (Prescaler_bits)
                    {
                        case "000":
                            //1:1
                            if (TimerWertbefehl > 255)
                            {

                                TimerWertbefehl = TimerWertbefehl - 256;
                                bank0[2, 11].Value = bank0[2, 11].Value.ToString().Substring(0, 5) + "1" + bank0[2, 11].Value.ToString().Substring(6);
                            }
                            bank0[2, 2].Value = dec2bin(TimerWertbefehl);


                            break;

                        case "001":
                            //1:2
                        
                            if (TimerWertbefehl % 2 == 0)
                            {
                                Ausgabewert = Ausgabewert + 1;
                                if (Ausgabewert > 255)
                                {
                                    Ausgabewert = 0;
                                    bank0[2, 11].Value = bank0[2, 11].Value.ToString().Substring(0, 5) + "1" + bank0[2, 11].Value.ToString().Substring(6);
                                }
                                bank0[2, 2].Value = dec2bin(Ausgabewert);
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
                                    bank0[2, 11].Value = bank0[2, 11].Value.ToString().Substring(0, 5) + "1" + bank0[2, 11].Value.ToString().Substring(6);
                                }
                                bank0[2, 2].Value = dec2bin(Ausgabewert);
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
                                    bank0[2, 11].Value = bank0[2, 11].Value.ToString().Substring(0, 5) + "1" + bank0[2, 11].Value.ToString().Substring(6);
                                }
                                bank0[2, 2].Value = dec2bin(Ausgabewert);
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
                                    bank0[2, 11].Value = bank0[2, 11].Value.ToString().Substring(0, 5) + "1" + bank0[2, 11].Value.ToString().Substring(6);
                                }
                                bank0[2, 2].Value = dec2bin(Ausgabewert);
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
                                    bank0[2, 11].Value = bank0[2, 11].Value.ToString().Substring(0, 5) + "1" + bank0[2, 11].Value.ToString().Substring(6);
                                }
                                bank0[2, 2].Value = dec2bin(Ausgabewert);
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
                                    bank0[2, 11].Value = bank0[2, 11].Value.ToString().Substring(0, 5) + "1" + bank0[2, 11].Value.ToString().Substring(6);
                                }
                                bank0[2, 2].Value = dec2bin(Ausgabewert);
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
                                    bank0[2, 11].Value = bank0[2, 11].Value.ToString().Substring(0, 5) + "1" + bank0[2, 11].Value.ToString().Substring(6);
                                }
                                bank0[2, 2].Value = dec2bin(Ausgabewert);
                            }
                            break;
                    }
                }//Wenn PSA BIT 0= Timer
                else if (bank1[2, 2].Value.ToString().Substring(4, 1) == "0")
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
                                    bank0[2, 11].Value = bank0[2, 11].Value.ToString().Substring(0, 5) + "1" + bank0[2, 11].Value.ToString().Substring(6);
                                }
                                bank0[2, 2].Value = dec2bin(Ausgabewert);
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
                                    bank0[2, 11].Value = bank0[2, 11].Value.ToString().Substring(0, 5) + "1" + bank0[2, 11].Value.ToString().Substring(6);
                                }
                                bank0[2, 2].Value = dec2bin(Ausgabewert);
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
                                    bank0[2, 11].Value = bank0[2, 11].Value.ToString().Substring(0, 5) + "1" + bank0[2, 11].Value.ToString().Substring(6);
                                }
                                bank0[2, 2].Value = dec2bin(Ausgabewert);
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
                                    bank0[2, 11].Value = bank0[2, 11].Value.ToString().Substring(0, 5) + "1" + bank0[2, 11].Value.ToString().Substring(6);
                                }
                                bank0[2, 2].Value = dec2bin(Ausgabewert);
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
                                    bank0[2, 11].Value = bank0[2, 11].Value.ToString().Substring(0, 5) + "1" + bank0[2, 11].Value.ToString().Substring(6);
                                }
                                bank0[2, 2].Value = dec2bin(Ausgabewert);
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
                                    bank0[2, 11].Value = bank0[2, 11].Value.ToString().Substring(0, 5) + "1" + bank0[2, 11].Value.ToString().Substring(6);
                                }
                                bank0[2, 2].Value = dec2bin(Ausgabewert);
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
                                    bank0[2, 11].Value = bank0[2, 11].Value.ToString().Substring(0, 5) + "1" + bank0[2, 11].Value.ToString().Substring(6);
                                }
                                bank0[2, 2].Value = dec2bin(Ausgabewert);
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
                                    bank0[2, 11].Value = bank0[2, 11].Value.ToString().Substring(0, 5) + "1" + bank0[2, 11].Value.ToString().Substring(6);
                                }
                                bank0[2, 2].Value = dec2bin(Ausgabewert);
                            }
                            break;
                    }
                }
            }
            //wenn timer in speicherzellen vorhanden ist, wird der wert überschrieben
            for (int i = 0; i < speicherzellen.RowCount; i++)
            {
                if (speicherzellen[1, i].Value.ToString() == bank0[1, 2].Value.ToString())
                {
                    speicherzellen[2, i].Value = bank0[2, 2].Value.ToString();
                }
            }
        }

        //setzen der in/outputs
        private void setinputoutput()
        {
            int button;

            for (int i = 0; i < 8; i++)
            {
                if (bank1[2, 6].Value.ToString().Substring(i, 1) == "1")
                {
                    button = i + 1;
                    inputoutput(button, true);
                }
                if (bank1[2, 6].Value.ToString().Substring(i, 1) == "0")
                {
                    button = i + 1;
                    inputoutput(button, false);
                }
                if (bank1[2, 7].Value.ToString().Substring(i, 1) == "1")
                {
                    button = i + 9;
                    inputoutput(button, true);
                }
                if (bank1[2, 7].Value.ToString().Substring(i, 1) == "0")
                {
                    button = i + 9;
                    inputoutput(button, false);
                }
            }
        }

        //interruptregister auf kombination testen, die einen interrupt auslöst
        private void testifinterrupt()
        {
            if (bank0[2, 11].Value.ToString().Substring(0, 1) == "1")
            {
                if (bank0[2, 11].Value.ToString().Substring(1, 1) == "1")
                { interrupt(); }
                if (bank0[2, 11].Value.ToString().Substring(2, 1) == "1" && bank0[2, 11].Value.ToString().Substring(5, 1) == "1")
                { interrupt(); }
                if (bank0[2, 11].Value.ToString().Substring(3, 1) == "1" && bank0[2, 11].Value.ToString().Substring(6, 1) == "1")
                { interrupt(); }
                if (bank0[2, 11].Value.ToString().Substring(4, 1) == "1" && bank0[2, 11].Value.ToString().Substring(7, 1) == "1")
                { interrupt(); }
            }
        }

        //statusregister setzen
        private void setstatusreg()
        {
            if (digitcarry == 2 && zero != 2 && carry != 2) bank0[2, 4].Value = bank0[2, 4].Value.ToString().Substring(0, 3) + to + pd + zero + "x" + carry;
            if (digitcarry != 2 && zero == 2 && carry != 2) bank0[2, 4].Value = bank0[2, 4].Value.ToString().Substring(0, 3) + to + pd + "x" + digitcarry + carry;
            if (digitcarry != 2 && zero != 2 && carry == 2) bank0[2, 4].Value = bank0[2, 4].Value.ToString().Substring(0, 3) + to + pd + zero + digitcarry + "x";
            if (digitcarry == 2 && zero == 2 && carry != 2) bank0[2, 4].Value = bank0[2, 4].Value.ToString().Substring(0, 3) + to + pd + "x" + "x" + carry;
            if (digitcarry == 2 && zero != 2 && carry == 2) bank0[2, 4].Value = bank0[2, 4].Value.ToString().Substring(0, 3) + to + pd + zero + "x" + "x";
            if (digitcarry != 2 && zero == 2 && carry == 2) bank0[2, 4].Value = bank0[2, 4].Value.ToString().Substring(0, 3) + to + pd + "x" + digitcarry + "x";
            if (digitcarry == 2 && zero == 2 && carry == 2) bank0[2, 4].Value = bank0[2, 4].Value.ToString().Substring(0, 3) + to + pd + "x" + "x" + "x";
            if (digitcarry != 2 && zero != 2 && carry != 2) bank0[2, 4].Value = bank0[2, 4].Value.ToString().Substring(0, 3) + to + pd + zero + digitcarry + carry;
            bank1[2, 4].Value = bank0[2, 4].Value.ToString();
        }

        //input aus ports in register schreiben
        private void inputporta()
        {
            if (port3_7.BackColor == Color.SteelBlue)
            {
                if (bank0[2, 6].Value.ToString().Substring(0, 1) == "0") bank0[2, 6].Value = "1" + bank0[2, 6].Value.ToString().Substring(1);
                else bank0[2, 6].Value = "0" + bank0[2, 6].Value.ToString().Substring(1);
            }
            else
            {
                if (bank0[2, 6].Value.ToString() == "1" + bank0[2, 6].Value.ToString().Substring(1)) port3_7.BackColor = Color.Firebrick;
                if (bank0[2, 6].Value.ToString() == "0" + bank0[2, 6].Value.ToString().Substring(1)) port3_7.BackColor = Color.Transparent;
            }

            if (port3_6.BackColor == Color.SteelBlue)
            {
                if (bank0[2, 6].Value.ToString().Substring(1, 1) == "0") bank0[2, 6].Value = bank0[2, 6].Value.ToString().Substring(0, 1) + "1" + bank0[2, 6].Value.ToString().Substring(2);
                else bank0[2, 6].Value = bank0[2, 6].Value.ToString().Substring(0, 1) + "0" + bank0[2, 6].Value.ToString().Substring(2);
            }
            else
            {
                if (bank0[2, 6].Value.ToString() == bank0[2, 6].Value.ToString().Substring(0, 1) + "1" + bank0[2, 6].Value.ToString().Substring(2)) port3_6.BackColor = Color.Firebrick;
                if (bank0[2, 6].Value.ToString() == bank0[2, 6].Value.ToString().Substring(0, 1) + "0" + bank0[2, 6].Value.ToString().Substring(2)) port3_6.BackColor = Color.Transparent ;
            }
            if (port3_5.BackColor == Color.SteelBlue)
            {
                if (bank0[2, 6].Value.ToString().Substring(2, 1) == "0") bank0[2, 6].Value = bank0[2, 6].Value.ToString().Substring(0, 2) + "1" + bank0[2, 6].Value.ToString().Substring(3);
                else bank0[2, 6].Value = bank0[2, 6].Value.ToString().Substring(0, 2) + "0" + bank0[2, 6].Value.ToString().Substring(3);
            }
            else
            {
                if (bank0[2, 6].Value.ToString() == bank0[2, 6].Value.ToString().Substring(0, 2) + "1" + bank0[2, 6].Value.ToString().Substring(3)) port3_5.BackColor = Color.Firebrick;
                if (bank0[2, 6].Value.ToString() == bank0[2, 6].Value.ToString().Substring(0, 2) + "0" + bank0[2, 6].Value.ToString().Substring(3)) port3_5.BackColor = Color.Transparent;
            }
            if (port3_4.BackColor == Color.SteelBlue)
            {
                //Taktgenerator PortA
                if (bank0[2, 6].Value.ToString().Substring(3, 1) == "0") bank0[2, 6].Value = bank0[2, 6].Value.ToString().Substring(0, 3) + "1" + bank0[2, 6].Value.ToString().Substring(4);
                else bank0[2, 6].Value = bank0[2, 6].Value.ToString().Substring(0, 3) + "0" + bank0[2, 6].Value.ToString().Substring(4);

                //PS2:PS0 Bits werden als String zusammengefasst und in Switch Case geprüft
                String Prescaler_bits = bank1[2, 2].Value.ToString().Substring(5, 1) + bank1[2, 2].Value.ToString().Substring(6, 1) + bank1[2, 2].Value.ToString().Substring(7, 1);

                //Prüfen ob PSA Bit 1--> WDT oder 0--> TMR0 ist
                if (bank1[2, 2].Value.ToString().Substring(2, 1) == "1")
                {
                    if (bank1[2, 2].Value.ToString().Substring(4, 1) == "1")
                    {

                        switch (Prescaler_bits)
                        {
                            case "000":
                                //1:1
                                TimerWert = TimerWert + 1;
                                if (TimerWert > 255)
                                {
                                    TimerWert = 0;
                                    bank0[2, 11].Value = bank0[2, 11].Value.ToString().Substring(0, 5) + "1" + bank0[2, 11].Value.ToString().Substring(6);
                                }
                                bank0[2, 2].Value = dec2bin(TimerWert);


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
                                        bank0[2, 11].Value = bank0[2, 11].Value.ToString().Substring(0, 5) + "1" + bank0[2, 11].Value.ToString().Substring(6);
                                    }
                                    bank0[2, 2].Value = dec2bin(Ausgabewert);
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
                                        bank0[2, 11].Value = bank0[2, 11].Value.ToString().Substring(0, 5) + "1" + bank0[2, 11].Value.ToString().Substring(6);
                                    }
                                    bank0[2, 2].Value = dec2bin(Ausgabewert);
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
                                        bank0[2, 11].Value = bank0[2, 11].Value.ToString().Substring(0, 5) + "1" + bank0[2, 11].Value.ToString().Substring(6);
                                    }
                                    bank0[2, 2].Value = dec2bin(Ausgabewert);
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
                                        bank0[2, 11].Value = bank0[2, 11].Value.ToString().Substring(0, 5) + "1" + bank0[2, 11].Value.ToString().Substring(6);
                                    }
                                    bank0[2, 2].Value = dec2bin(Ausgabewert);
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
                                        bank0[2, 11].Value = bank0[2, 11].Value.ToString().Substring(0, 5) + "1" + bank0[2, 11].Value.ToString().Substring(6);
                                    }
                                    bank0[2, 2].Value = dec2bin(Ausgabewert);
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
                                        bank0[2, 11].Value = bank0[2, 11].Value.ToString().Substring(0, 5) + "1" + bank0[2, 11].Value.ToString().Substring(6);
                                    }
                                    bank0[2, 2].Value = dec2bin(Ausgabewert);
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
                                        bank0[2, 11].Value = bank0[2, 11].Value.ToString().Substring(0, 5) + "1" + bank0[2, 11].Value.ToString().Substring(6);
                                    }
                                    bank0[2, 2].Value = dec2bin(Ausgabewert);
                                }
                                break;
                        }
                    }//Wenn PSA BIT 0= Timer
                    else if (bank1[2, 2].Value.ToString().Substring(4, 1) == "0")
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
                                        bank0[2, 11].Value = bank0[2, 11].Value.ToString().Substring(0, 5) + "1" + bank0[2, 11].Value.ToString().Substring(6);
                                    }
                                    bank0[2, 2].Value = dec2bin(Ausgabewert);
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
                                        bank0[2, 11].Value = bank0[2, 11].Value.ToString().Substring(0, 5) + "1" + bank0[2, 11].Value.ToString().Substring(6);
                                    }
                                    bank0[2, 2].Value = dec2bin(Ausgabewert);
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
                                        bank0[2, 11].Value = bank0[2, 11].Value.ToString().Substring(0, 5) + "1" + bank0[2, 11].Value.ToString().Substring(6);
                                    }
                                    bank0[2, 2].Value = dec2bin(Ausgabewert);
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
                                        bank0[2, 11].Value = bank0[2, 11].Value.ToString().Substring(0, 5) + "1" + bank0[2, 11].Value.ToString().Substring(6);
                                    }
                                    bank0[2, 2].Value = dec2bin(Ausgabewert);
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
                                        bank0[2, 11].Value = bank0[2, 11].Value.ToString().Substring(0, 5) + "1" + bank0[2, 11].Value.ToString().Substring(6);
                                    }
                                    bank0[2, 2].Value = dec2bin(Ausgabewert);
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
                                        bank0[2, 11].Value = bank0[2, 11].Value.ToString().Substring(0, 5) + "1" + bank0[2, 11].Value.ToString().Substring(6);
                                    }
                                    bank0[2, 2].Value = dec2bin(Ausgabewert);
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
                                        bank0[2, 11].Value = bank0[2, 11].Value.ToString().Substring(0, 5) + "1" + bank0[2, 11].Value.ToString().Substring(6);
                                    }
                                    bank0[2, 2].Value = dec2bin(Ausgabewert);
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
                                        bank0[2, 11].Value = bank0[2, 11].Value.ToString().Substring(0, 5) + "1" + bank0[2, 11].Value.ToString().Substring(6);
                                    }
                                    bank0[2, 2].Value = dec2bin(Ausgabewert);
                                }
                                break;
                        }
                    }
                }
            }

            else {
                if (bank0[2, 6].Value.ToString() == bank0[2, 6].Value.ToString().Substring(0, 3) + "1" + bank0[2, 6].Value.ToString().Substring(4)) port3_4.BackColor = Color.Firebrick;
                if (bank0[2, 6].Value.ToString() == bank0[2, 6].Value.ToString().Substring(0, 3) + "0" + bank0[2, 6].Value.ToString().Substring(4)) port3_4.BackColor = Color.Transparent;
            }
                if (port3_3.BackColor == Color.SteelBlue)
                {
                    if (bank0[2, 6].Value.ToString().Substring(4, 1) == "0") bank0[2, 6].Value = bank0[2, 6].Value.ToString().Substring(0, 4) + "1" + bank0[2, 6].Value.ToString().Substring(5);
                    else bank0[2, 6].Value = bank0[2, 6].Value.ToString().Substring(0, 4) + "0" + bank0[2, 6].Value.ToString().Substring(5);
                }
                else {
                    if (bank0[2, 6].Value.ToString() == bank0[2, 6].Value.ToString().Substring(0, 4) + "1" + bank0[2, 6].Value.ToString().Substring(5)) port3_3.BackColor = Color.Firebrick;
                    if (bank0[2, 6].Value.ToString() == bank0[2, 6].Value.ToString().Substring(0, 4) + "0" + bank0[2, 6].Value.ToString().Substring(5)) port3_3.BackColor = Color.Transparent;
                }
                if (port3_2.BackColor == Color.SteelBlue)
                {
                    if (bank0[2, 6].Value.ToString().Substring(5, 1) == "0") bank0[2, 6].Value = bank0[2, 6].Value.ToString().Substring(0, 5) + "1" + bank0[2, 6].Value.ToString().Substring(6);
                    else bank0[2, 6].Value = bank0[2, 6].Value.ToString().Substring(0, 5) + "0" + bank0[2, 6].Value.ToString().Substring(6);
                }
                else {
                    if (bank0[2, 6].Value.ToString() == bank0[2, 6].Value.ToString().Substring(0, 5) + "1" + bank0[2, 6].Value.ToString().Substring(6)) port3_2.BackColor = Color.Firebrick;
                    if (bank0[2, 6].Value.ToString() == bank0[2, 6].Value.ToString().Substring(0, 5) + "0" + bank0[2, 6].Value.ToString().Substring(6)) port3_2.BackColor = Color.Transparent ;
                }
                if (port3_1.BackColor == Color.SteelBlue)
                {
                    if (bank0[2, 6].Value.ToString().Substring(6, 1) == "0") bank0[2, 6].Value = bank0[2, 6].Value.ToString().Substring(0, 6) + "1" + bank0[2, 6].Value.ToString().Substring(7);
                    else bank0[2, 6].Value = bank0[2, 6].Value.ToString().Substring(0, 6) + "0" + bank0[2, 6].Value.ToString().Substring(7);
                }
                else {
                    if (bank0[2, 6].Value.ToString() == bank0[2, 6].Value.ToString().Substring(0, 6) + "1" + bank0[2, 6].Value.ToString().Substring(7)) port3_1.BackColor = Color.Firebrick ;
                    if (bank0[2, 6].Value.ToString() == bank0[2, 6].Value.ToString().Substring(0, 6) + "0" + bank0[2, 6].Value.ToString().Substring(7)) port3_1.BackColor = Color.Transparent ;
                }
                if (port3_0.BackColor == Color.SteelBlue)
                {
                    if (bank0[2, 6].Value.ToString().Substring(7, 1) == "0") bank0[2, 6].Value = bank0[2, 6].Value.ToString().Substring(0, 7) + "1";
                    else bank0[2, 6].Value = bank0[2, 6].Value.ToString().Substring(0, 7) + "0";
                }
                else { 
                if (bank0[2, 6].Value.ToString() == bank0[2, 6].Value.ToString().Substring(0, 7) + "1") port3_0.BackColor = Color.Firebrick ;
                if (bank0[2, 6].Value.ToString() == bank0[2, 6].Value.ToString().Substring(0, 7) + "0") port3_0.BackColor = Color.Transparent;
                }
                


                for (int j = 0; j < speicherzellen.RowCount; j++)
                {
                    if (speicherzellen[1, j].Value.ToString() == bank0[1, 6].Value.ToString())
                    {
                        speicherzellen[2, j].Value = bank0[2, 6].Value.ToString();
                    }
                }

            }
        

        //input aus ports in register schreiben
        private void inputportb()
        {
            if (port4_7.BackColor == Color.SteelBlue)
            {
                if (bank0[2, 7].Value.ToString().Substring(0, 1) == "0") bank0[2, 7].Value = "1" + bank0[2, 7].Value.ToString().Substring(1);
                else bank0[2, 7].Value = "0" + bank0[2, 7].Value.ToString().Substring(1);
            }
            else {
                if (bank0[2, 7].Value.ToString() == "1" + bank0[2, 7].Value.ToString().Substring(1)) port4_7.BackColor = Color.Firebrick;
                if (bank0[2, 7].Value.ToString() == "0" + bank0[2, 7].Value.ToString().Substring(1)) port4_7.BackColor = Color.Transparent;
            }
           if (port4_6.BackColor == Color.SteelBlue)
                {
                    if (bank0[2, 7].Value.ToString().Substring(1, 1) == "0") bank0[2, 7].Value = bank0[2, 7].Value.ToString().Substring(0, 1) + "1" + bank0[2, 7].Value.ToString().Substring(2);
                    else bank0[2, 7].Value = bank0[2, 7].Value.ToString().Substring(0, 1) + "0" + bank0[2, 7].Value.ToString().Substring(2);
                }
            else
            {

            
                if (bank0[2, 7].Value.ToString() == bank0[2, 7].Value.ToString().Substring(0, 1) + "1" + bank0[2, 7].Value.ToString().Substring(2)) port4_6.BackColor = Color.Firebrick ;
                if (bank0[2, 7].Value.ToString() == bank0[2, 7].Value.ToString().Substring(0, 1) + "0" + bank0[2, 7].Value.ToString().Substring(2)) port4_6.BackColor = Color.Transparent;
             }
            if (port4_5.BackColor == Color.SteelBlue)
            {
                if (bank0[2, 7].Value.ToString().Substring(2, 1) == "0") bank0[2, 7].Value = bank0[2, 7].Value.ToString().Substring(0, 2) + "1" + bank0[2, 7].Value.ToString().Substring(3);
                else bank0[2, 7].Value = bank0[2, 7].Value.ToString().Substring(0, 2) + "0" + bank0[2, 7].Value.ToString().Substring(3);
                
            }
            else {
                if (bank0[2, 7].Value.ToString() == bank0[2, 7].Value.ToString().Substring(0, 2) + "1" + bank0[2, 7].Value.ToString().Substring(3)) port4_5.BackColor = Color.Firebrick;
                if (bank0[2, 7].Value.ToString() == bank0[2, 7].Value.ToString().Substring(0, 2) + "0" + bank0[2, 7].Value.ToString().Substring(3)) port4_5.BackColor = Color.Transparent;
            }
            if (port4_4.BackColor == Color.SteelBlue)
            {
                if (bank0[2, 7].Value.ToString().Substring(3, 1) == "0") bank0[2, 7].Value = bank0[2, 7].Value.ToString().Substring(0, 3) + "1" + bank0[2, 7].Value.ToString().Substring(4);
                else bank0[2, 7].Value = bank0[2, 7].Value.ToString().Substring(0, 3) + "0" + bank0[2, 7].Value.ToString().Substring(4);
            }
            else {
                if (bank0[2, 7].ToString() == bank0[2, 7].Value.ToString().Substring(0, 3) + "1" + bank0[2, 7].Value.ToString().Substring(4)) port4_4.BackColor = Color.Firebrick ;
                if (bank0[2, 7].Value.ToString() == bank0[2, 7].Value.ToString().Substring(0, 3) + "0" + bank0[2, 7].Value.ToString().Substring(4)) port4_4.BackColor = Color.Transparent;
            }
            if (port4_3.BackColor == Color.SteelBlue)
            {
                if (bank0[2, 7].Value.ToString().Substring(4, 1) == "0") bank0[2, 7].Value = bank0[2, 7].Value.ToString().Substring(0, 4) + "1" + bank0[2, 7].Value.ToString().Substring(5);
                else bank0[2, 7].Value = bank0[2, 7].Value.ToString().Substring(0, 4) + "0" + bank0[2, 7].Value.ToString().Substring(5);
            }
            else {
                if (bank0[2, 7].Value.ToString() == bank0[2, 7].Value.ToString().Substring(0, 4) + "1" + bank0[2, 7].Value.ToString().Substring(5)) port4_3.BackColor = Color.Firebrick;
                if (bank0[2, 7].Value.ToString() == bank0[2, 7].Value.ToString().Substring(0, 4) + "0" + bank0[2, 7].Value.ToString().Substring(5)) port4_3.BackColor = Color.Transparent;
            }
            if (port4_2.BackColor == Color.SteelBlue)
            {
                if (bank0[2, 7].Value.ToString().Substring(5, 1) == "0") bank0[2, 7].Value = bank0[2, 7].Value.ToString().Substring(0, 5) + "1" + bank0[2, 7].Value.ToString().Substring(6);
                else bank0[2, 7].Value = bank0[2, 7].Value.ToString().Substring(0, 5) + "0" + bank0[2, 7].Value.ToString().Substring(6);
            }
            else {
                if (bank0[2, 7].Value.ToString() == bank0[2, 7].Value.ToString().Substring(0, 5) + "1" + bank0[2, 7].Value.ToString().Substring(6)) port4_2.BackColor = Color.Firebrick;
                if (bank0[2, 7].Value.ToString() == bank0[2, 7].Value.ToString().Substring(0, 5) + "0" + bank0[2, 7].Value.ToString().Substring(6)) port4_2.BackColor = Color.Transparent ;
            }
            if (port4_1.BackColor == Color.SteelBlue)
            {
                if (bank0[2, 7].Value.ToString().Substring(6, 1) == "0") bank0[2, 7].Value = bank0[2, 7].Value.ToString().Substring(0, 6) + "1" + bank0[2, 7].Value.ToString().Substring(7);
                else bank0[2, 7].Value = bank0[2, 7].Value.ToString().Substring(0, 6) + "0" + bank0[2, 7].Value.ToString().Substring(7);
            }
            else {
                if (bank0[2, 7].Value.ToString()== bank0[2, 7].Value.ToString().Substring(0, 6) + "1" + bank0[2, 7].Value.ToString().Substring(7)) port4_1.BackColor = Color.Firebrick;
                if (bank0[2, 7].Value.ToString()== bank0[2, 7].Value.ToString().Substring(0, 6) + "0" + bank0[2, 7].Value.ToString().Substring(7)) port4_1.BackColor = Color.Transparent;
            }
            if (port4_0.BackColor == Color.SteelBlue)
            {
                if (bank0[2, 7].Value.ToString().Substring(7, 1) == "0") bank0[2, 7].Value = bank0[2, 7].Value.ToString().Substring(0, 7) + "1";
                else bank0[2, 7].Value = bank0[2, 7].Value.ToString().Substring(0, 7) + "0";
            }
            else {
                if (bank0[2, 7].Value.ToString() == bank0[2, 7].Value.ToString().Substring(0, 7) + "1") port4_0.BackColor = Color.Firebrick ;
                if (bank0[2, 7].Value.ToString() == bank0[2, 7].Value.ToString().Substring(0, 7) + "0") port4_0.BackColor = Color.Transparent;
            }
            
                if (speicherzellen[1, j].Value.ToString() == bank0[1, 7].Value.ToString())
                {
                    speicherzellen[2, j].Value = bank0[2, 7].Value.ToString();
                }
            

        }

        //setzen der input/outputs
        private void inputoutput(int button, bool inout)
        {
          

        }

        //inetrrupt-methode
        public async void interrupt()
        {
            btn_interrupt.Enabled = true;
            
            
            await Task.Delay(5000);
            for (int i = 0; opcodedata.Rows.Count> i;i++)
            {
                if (opcodedata[1, i].Value.ToString() == "0004")
                    opcodedata.CurrentCell = opcodedata[1, i];
            }
            btn_interrupt.Enabled = false;  
        }
        
        //aus dem opcode den befehl ausleden, welcher durchgeführt werden muss
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

        //ausführen eines befehls nach klick des step buttons
        private void stepbtn_Click_1(object sender, EventArgs e)
        {

            timer_freq.Interval = 200;
            timer_freq.Start();

            if (opcodedata.CurrentRow.DefaultCellStyle.BackColor == Color.White || opcodedata.CurrentRow.DefaultCellStyle.BackColor == Color.LightGray) colorset = opcodedata.CurrentRow.DefaultCellStyle.BackColor;
            string opcode = opcodedata.CurrentRow.Cells[2].Value.ToString();
            string opstring = opcodedata.CurrentRow.Cells[3].Value.ToString();
            int row = opcodedata.CurrentRow.Index;
            int retvalue = dooperator(opcode, opstring, row);
            


            if (retvalue != 1234567)
            {
                opcodedata.Rows[opcodedata.CurrentRow.Index].DefaultCellStyle.BackColor = colorset;
                opcodedata.CurrentCell = opcodedata[1, retvalue];

                if (opcodedata.CurrentRow.DefaultCellStyle.BackColor != Color.Salmon) colorset = opcodedata.CurrentRow.DefaultCellStyle.BackColor;
                opcodedata.CurrentRow.DefaultCellStyle.BackColor = Color.LightSalmon;
            }
            else
            {
                opcodedata.Rows[opcodedata.CurrentRow.Index].DefaultCellStyle.BackColor = colorset;
                opcodedata.CurrentCell = opcodedata[1, opcodedata.CurrentRow.Index + 1];
                
            if (opcodedata.CurrentRow.DefaultCellStyle.BackColor != Color.Salmon) colorset = opcodedata.CurrentRow.DefaultCellStyle.BackColor;
                opcodedata.CurrentRow.DefaultCellStyle.BackColor = Color.LightSalmon;
            }
            
        }

        //umrechner von binär zu hexadezimal
        public static string BinaryStringToHexString(string binary)
        {
            while (binary.Length > 8) binary = binary.Substring(1);
            while (binary.Length < 8) binary = "0" + binary;
            string resultstring = "";
            string binlow = binary.Substring(4);
            string binhigh = binary.Substring(0, 4);
            switch (binlow)
            {
                case "0000":
                    resultstring = "0";
                    break;
                case "0001":
                    resultstring = "1";
                    break;
                case "0010":
                    resultstring = "2";
                    break;
                case "0011":
                    resultstring = "3";
                    break;
                case "0100":
                    resultstring = "4";
                    break;
                case "0101":
                    resultstring = "5";
                    break;
                case "0110":
                    resultstring = "6";
                    break;
                case "0111":
                    resultstring = "7";
                    break;
                case "1000":
                    resultstring = "8";
                    break;
                case "1001":
                    resultstring = "9";
                    break;
                case "1010":
                    resultstring = "A";
                    break;
                case "1011":
                    resultstring = "B";
                    break;
                case "1100":
                    resultstring = "C";
                    break;
                case "1101":
                    resultstring = "D";
                    break;
                case "1110":
                    resultstring = "E";
                    break;
                case "1111":
                    resultstring = "F";
                    break;
                default:
                    break;
            }
            switch (binhigh)
            {
                case "0000":
                    resultstring = "0" + resultstring;
                    break;
                case "0001":
                    resultstring = "1" + resultstring;
                    break;
                case "0010":
                    resultstring = "2" + resultstring;
                    break;
                case "0011":
                    resultstring = "3" + resultstring;
                    break;
                case "0100":
                    resultstring = "4" +resultstring;
                    break;
                case "0101":
                    resultstring = "5" + resultstring;
                    break;
                case "0110":
                    resultstring = "6" + resultstring;
                    break;
                case "0111":
                    resultstring = "7" + resultstring;
                    break;
                case "1000":
                    resultstring = "8" + resultstring;
                    break;
                case "1001":
                    resultstring = "9" + resultstring;
                    break;
                case "1010":
                    resultstring = "A" + resultstring;
                    break;
                case "1011":
                    resultstring = "B" + resultstring;
                    break;
                case "1100":
                    resultstring = "C" + resultstring;
                    break;
                case "1101":
                    resultstring = "D" + resultstring;
                    break;
                case "1110":
                    resultstring = "E" + resultstring;
                    break;
                case "1111":
                    resultstring = "F" + resultstring;
                    break;
                default:
                    break;
            }
            return resultstring + "h";
            
        }
        
        //umrechner von dezimal zu binär
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

        //umrechner von binär zu dezimal
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

        //Ports zum setzen der Bits__________________________________________________________________________

        //PortA input
        private void port1_0_Click(object sender, EventArgs e)
        {
            if (port1_0.BackColor == Color.Transparent)
            {
                port1_0.BackColor = Color.Firebrick;
                bank1[2, 6].Value = bank1[2, 6].Value.ToString().Substring(0, 7) + "1";
            }
            else
            {
                port1_0.BackColor = Color.Transparent;
                bank1[2, 6].Value = bank1[2, 6].Value.ToString().Substring(0, 7) + "0";
            }

        }

        private void port1_1_Click(object sender, EventArgs e)
        {
            if (port1_1.BackColor == Color.Transparent)
            {
                port1_1.BackColor = Color.Firebrick;
                bank1[2, 6].Value = bank1[2, 6].Value.ToString().Substring(0, 6) + "1" + bank1[2, 6].Value.ToString().Substring(7);
            }
            else
            {
                port1_1.BackColor = Color.Transparent;
                bank1[2, 6].Value = bank1[2, 6].Value.ToString().Substring(0, 6) + "0" + bank1[2, 6].Value.ToString().Substring(7);
            }
        }

        private void port1_2_Click(object sender, EventArgs e)
        {
            if (port1_2.BackColor == Color.Transparent)
            {
                port1_2.BackColor = Color.Firebrick;
                bank1[2, 6].Value = bank1[2, 6].Value.ToString().Substring(0, 5) + "1" + bank1[2, 6].Value.ToString().Substring(6);
            }
            else
            {
                port1_2.BackColor = Color.Transparent;
                bank1[2, 6].Value = bank1[2, 6].Value.ToString().Substring(0, 5) + "0" + bank1[2, 6].Value.ToString().Substring(6);
            }
        }

        private void port1_3_Click(object sender, EventArgs e)
        {
            if (port1_3.BackColor == Color.Transparent)
            {
                port1_3.BackColor = Color.Firebrick;
                bank1[2, 6].Value = bank1[2, 6].Value.ToString().Substring(0, 4) + "1" + bank1[2, 6].Value.ToString().Substring(5);
            }
            else
            {
                port1_3.BackColor = Color.Transparent;
                bank1[2, 6].Value = bank1[2, 6].Value.ToString().Substring(0, 4) + "0" + bank1[2, 6].Value.ToString().Substring(5);
            }
        }

        private void port1_4_Click(object sender, EventArgs e)
        {
            if (port1_4.BackColor == Color.Transparent)
            {
                port1_4.BackColor = Color.Firebrick;
                bank1[2, 6].Value = bank1[2, 6].Value.ToString().Substring(0, 3) + "1" + bank1[2, 6].Value.ToString().Substring(4);
            }
            else
            {
                port1_4.BackColor = Color.Transparent;
                bank1[2, 6].Value = bank1[2, 6].Value.ToString().Substring(0, 3) + "0" + bank1[2, 6].Value.ToString().Substring(4);
            }
        }

        private void port1_5_Click(object sender, EventArgs e)
        {
            if (port1_5.BackColor == Color.Transparent) port1_5.BackColor = Color.Firebrick;
            else {
                if (port1_5.BackColor == Color.Firebrick) port1_5.BackColor = Color.SteelBlue;
                else {
                    if (port1_5.BackColor == Color.SteelBlue)
                    {
                        port1_5.BackColor = Color.Transparent;
                        port1_5.Visible = false;
                        port3_5.Visible = true;
                        bank1[2, 6].Value = bank1[2, 6].Value.ToString().Substring(0, 2) + "0" + bank1[2, 6].Value.ToString().Substring(3);
                        
                    }
                }
            }
        }

        private void port1_6_Click(object sender, EventArgs e)
        {
            if (port1_6.BackColor == Color.Transparent) port1_6.BackColor = Color.Firebrick;
            else {
                if (port1_6.BackColor == Color.Firebrick) port1_6.BackColor = Color.SteelBlue;
                else {
                    if (port1_6.BackColor == Color.SteelBlue)
                    {
                        port1_6.BackColor = Color.Transparent;
                        port1_6.Visible = false;
                        port3_6.Visible = true;
                        bank1[2, 6].Value = bank1[2, 6].Value.ToString().Substring(0, 1) + "0" + bank1[2, 6].Value.ToString().Substring(2);
                        
                    }
                }
            }
        }

        private void port1_7_Click(object sender, EventArgs e)
        {
            if (port1_7.BackColor == Color.Transparent) port1_7.BackColor = Color.Firebrick;
            else {
                if (port1_7.BackColor == Color.Firebrick) port1_7.BackColor = Color.SteelBlue;
                else {
                    if (port1_7.BackColor == Color.SteelBlue)
                    {
                        port1_7.BackColor = Color.Transparent;
                        port1_7.Visible = false;
                        port3_7.Visible = true;
                        bank1[2, 6].Value = "0" + bank1[2, 6].Value.ToString().Substring(1);
                        
                    }
                }
            }
        }


        //PortB input
        private void port2_7_Click(object sender, EventArgs e)
        {
            if (port2_7.BackColor == Color.Transparent)
            {
                port2_7.BackColor = Color.Firebrick;
                bank1[2, 7].Value =  "1" + bank1[2, 7].Value.ToString().Substring(1);
            }
            else
            {
                port2_7.BackColor = Color.Transparent;
                bank1[2, 7].Value =  "0" + bank1[2, 7].Value.ToString().Substring(1);
            }
        }

        private void port2_6_Click(object sender, EventArgs e)
        {
            if (port2_6.BackColor == Color.Transparent)
            {
                port2_6.BackColor = Color.Firebrick;
                bank1[2, 7].Value = bank1[2, 6].Value.ToString().Substring(0, 1) + "1" + bank1[2, 7].Value.ToString().Substring(2);
            }
            else
            {
                port2_6.BackColor = Color.Transparent;
                bank1[2, 7].Value = bank1[2, 6].Value.ToString().Substring(0, 1) + "0" + bank1[2, 7].Value.ToString().Substring(2);
            }
        }

        private void port2_5_Click(object sender, EventArgs e)
        {
            if (port2_5.BackColor == Color.Transparent)
            {
                port2_5.BackColor = Color.Firebrick;
                bank1[2, 7].Value = bank1[2, 6].Value.ToString().Substring(0, 2) + "1" + bank1[2, 7].Value.ToString().Substring(3);
            }
            else
            {
                port2_5.BackColor = Color.Transparent;
                bank1[2, 7].Value = bank1[2, 6].Value.ToString().Substring(0, 2) + "0" + bank1[2, 7].Value.ToString().Substring(3);
            }
        }

        private void port2_4_Click(object sender, EventArgs e)
        {
            if (port2_4.BackColor == Color.Transparent)
            {
                port2_4.BackColor = Color.Firebrick;
                bank1[2, 7].Value = bank1[2, 6].Value.ToString().Substring(0, 3) + "1" + bank1[2, 7].Value.ToString().Substring(4);
            }
            else
            {
                port2_4.BackColor = Color.Transparent;
                bank1[2, 7].Value = bank1[2, 6].Value.ToString().Substring(0, 3) + "0" + bank1[2, 7].Value.ToString().Substring(4);
            }
        }

        private void port2_3_Click(object sender, EventArgs e)
        {
            if (port2_3.BackColor == Color.Transparent)
            {
                port2_3.BackColor = Color.Firebrick;
                bank1[2, 7].Value = bank1[2, 6].Value.ToString().Substring(0, 4) + "1" + bank1[2, 7].Value.ToString().Substring(5);
            }
            else
            {
                port2_3.BackColor = Color.Transparent;
                bank1[2, 7].Value = bank1[2, 6].Value.ToString().Substring(0, 4) + "0" + bank1[2, 7].Value.ToString().Substring(5);
            }
        }

        private void port2_2_Click(object sender, EventArgs e)
        {
            if (port2_2.BackColor == Color.Transparent)
            {
                port2_2.BackColor = Color.Firebrick;
                bank1[2, 7].Value = bank1[2, 6].Value.ToString().Substring(0, 5) + "1" + bank1[2, 7].Value.ToString().Substring(6);
            }
            else
            {
                port2_2.BackColor = Color.Transparent;
                bank1[2, 7].Value = bank1[2, 6].Value.ToString().Substring(0, 5) + "0" + bank1[2, 7].Value.ToString().Substring(6);
            }
        }

        private void port2_1_Click(object sender, EventArgs e)
        {
            if (port2_1.BackColor == Color.Transparent)
            {
                port2_1.BackColor = Color.Firebrick;
                bank1[2, 7].Value = bank1[2, 6].Value.ToString().Substring(0, 6) + "1" + bank1[2, 7].Value.ToString().Substring(7);
            }
            else
            {
                port2_1.BackColor = Color.Transparent;
                bank1[2, 7].Value = bank1[2, 6].Value.ToString().Substring(0, 6) + "0" + bank1[2, 7].Value.ToString().Substring(7);
            }
        }

        private void port2_0_Click(object sender, EventArgs e)
        {
            if (port2_1.BackColor == Color.Transparent)
            {
                port2_1.BackColor = Color.Firebrick;
                bank1[2, 7].Value = bank1[2, 6].Value.ToString().Substring(0, 7) + "1";
            }
            else
            {
                port2_1.BackColor = Color.Transparent;
                bank1[2, 7].Value = bank1[2, 6].Value.ToString().Substring(0, 7) + "0";
            }
        }
        //____________________________________________________________________________________________________

        //timer öffnet methode um port a zu setzen
        private void timerinputporta_Tick(object sender, EventArgs e)
        {
           inputporta();
           
        }

        //timer öffnet methode um port b zu setzen
        private void timerinputportb_Tick(object sender, EventArgs e)
        {
           inputportb();
        }

        //bestimmen von takta
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

            timerinputporta.Interval = takta;
            if (takta > taktb) timer4.Interval = takta;
            else { timer4.Interval = taktb; }
        }

        //bestimmen von taktb
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
            timerinputportb.Interval = taktb;
            if (takta > taktb) timer4.Interval = takta;
            else { timer4.Interval = taktb; }
        }

        //berechnung quarzfrequenz
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            try {
                while (txt_freq.Text.ToString() == "") { goto Warte; }
                quarzfrequenz = Convert.ToInt32(txt_freq.Text);
                if(quarzfrequenz < 0) { quarzfrequenz = 4000; txt_freq.Text = quarzfrequenz.ToString(); }
            }
            catch { quarzfrequenz = 4000; txt_freq.Text = quarzfrequenz.ToString(); }

            Warte:
            ;
        }

        //timer zum berechnen der quarzfrequenz
        private void timer3_Tick(object sender, EventArgs e)
        {
            
            if (quarzfrequenz != 0)
            {
                runtime = runtime + (4000 / quarzfrequenz) * befehlnr;
                txt_runtime.Text = runtime.ToString();
                befehlnr = 0;
            }
        }
       
        
        //timer zur hardwareansteuerung 
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
                    bank0[2, 6].Value = retporta;
                    bank0[2, 7].Value = retportb;

                }
                catch { }
            }
            else
            {
                //timer4.Stop();
                //timerinputportb.Start();
                //timerinputporta.Start();
                //button5.Visible = false;
            }
        }
        
        //Hardwareansteuerung______________________________________________________________________________
                                                                                                         
        //konvertiert string der vom picview geschicht wird in hex um
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
                                                                                                         
        //neues Form zur Hardwareansteuerung geöffnet                                                    
        private void button2_Click(object sender, EventArgs e) 
        {
            conn = new Form2(serialPort1);
            conn.Show();
            timer4.Start();                                                                                      
        }
                                                                                                   
        //konvertierbaren binärstring erstellen (---00000 -> 00000000)                                   
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
                                                                                          
        //tris a/b porta/b ausgelesen um diese an den picview senden zu können                           
        private string getcominformation()
        {
            string trisa = makeitconvertable(bank1[2, 6].Value.ToString());
            string trisb = makeitconvertable(bank1[2, 7].Value.ToString());
            string porta = makeitconvertable(bank0[2, 6].Value.ToString());
            string portb = makeitconvertable(bank0[2, 7].Value.ToString());
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
                                   
        //von hex in picviewstring konvertiert                                                           
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
                                                                                                         
        //auslesen des inputs des Picviews                                                               
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
                                                                                                         
        //Hardwareansteuerung beendet                                                                    
        private void button4_Click(object sender, EventArgs e)
        {                                                                                                
            serialPort1.Close();                                                                         
            timer4.Stop();                                                                               
            timerinputporta.Start();                                                                     
            timerinputportb.Start();                                                                     
            conn.Close();                                                                                
                                                                                                         
        }                                                                                                
                                                                                                         
        //_________________________________________________________________________________________________

        //helpbtn öffnet pdf
        private void button3_Click(object sender, EventArgs e)
        {
            Help help = new Help();
            help.Show();
        }

        //setzt loop zum ausführen der opcodes fort
        private void btn_weiter_Click(object sender, EventArgs e)
        {
            dooperatorloop();
        }

        private void port3_4_Click(object sender, EventArgs e)
        {
            if (port1_4.BackColor == Color.Firebrick)
            {
                if (port3_4.BackColor == Color.Firebrick)
                {
                    port3_4.BackColor = Color.Transparent;
                    bank0[2, 6].Value = bank0[2, 6].Value.ToString().Substring(0, 3) + "0" + bank0[2, 6].Value.ToString().Substring(4);
                }
                else if (port3_4.BackColor == Color.SteelBlue)
                {
                    port3_4.BackColor = Color.Firebrick;
                    bank0[2, 6].Value = bank0[2, 6].Value.ToString().Substring(0, 3) + "1" + bank0[2, 6].Value.ToString().Substring(4);
                }
                 else if (port3_4.BackColor == Color.Transparent)
                {
                    port3_4.BackColor = Color.SteelBlue;

                }
            }
        }

        private void port3_3_Click(object sender, EventArgs e)
        {
            if (port1_3.BackColor == Color.Firebrick)
            {
                if (port3_3.BackColor == Color.Firebrick)
                {
                    port3_3.BackColor = Color.Transparent;
                    bank0[2, 6].Value = bank0[2, 6].Value.ToString().Substring(0, 4) + "0" + bank0[2, 6].Value.ToString().Substring(5);
                }
                else if (port3_3.BackColor == Color.SteelBlue)
                {
                    port3_3.BackColor = Color.Firebrick;
                    bank0[2, 6].Value = bank0[2, 6].Value.ToString().Substring(0, 4) + "1" + bank0[2, 6].Value.ToString().Substring(5);
                }
                else if (port3_3.BackColor == Color.Transparent)
                {
                    port3_3.BackColor = Color.SteelBlue;
                   
                }
            }
        }

        private void port3_2_Click(object sender, EventArgs e)
        {
            if (port1_2.BackColor == Color.Firebrick)
            {
                if (port3_2.BackColor == Color.Firebrick)
                {
                    port3_2.BackColor = Color.Transparent;
                    bank0[2, 6].Value = bank0[2, 6].Value.ToString().Substring(0, 5) + "0" + bank0[2, 6].Value.ToString().Substring(6);
                }
                else if (port3_2.BackColor == Color.SteelBlue)
                {
                    port3_2.BackColor = Color.Firebrick;
                    bank0[2, 6].Value = bank0[2, 6].Value.ToString().Substring(0, 5) + "1" + bank0[2, 6].Value.ToString().Substring(6);
                }
                else if (port3_2.BackColor == Color.Transparent)
                {
                    port3_2.BackColor = Color.SteelBlue;

                }
            }
        }

        private void port3_1_Click(object sender, EventArgs e)
        {
            if (port1_1.BackColor == Color.Firebrick)
            {
                if (port3_1.BackColor == Color.Firebrick)
                {
                    port3_1.BackColor = Color.Transparent;
                    bank0[2, 6].Value = bank0[2, 6].Value.ToString().Substring(0, 6) + "0" + bank0[2, 6].Value.ToString().Substring(7);
                }
                else if (port3_1.BackColor == Color.SteelBlue)
                {
                    port3_1.BackColor = Color.Firebrick;
                    bank0[2, 6].Value = bank0[2, 6].Value.ToString().Substring(0, 6) + "1" + bank0[2, 6].Value.ToString().Substring(7);
                }
                else if (port3_1.BackColor == Color.Transparent)
                {
                    port3_1.BackColor = Color.SteelBlue;

                }
            }
        }

        private void port3_0_Click(object sender, EventArgs e)
        {
            if (port1_0.BackColor == Color.Firebrick)
            {
                if (port3_0.BackColor == Color.Firebrick)
                {
                    port3_0.BackColor = Color.Transparent;
                    bank0[2, 6].Value = bank0[2, 6].Value.ToString().Substring(0, 7) + "0";
                }
                else if (port3_0.BackColor == Color.SteelBlue)
                {
                    port3_0.BackColor = Color.Firebrick;
                    bank0[2, 6].Value = bank0[2, 6].Value.ToString().Substring(0, 7) + "1";
                }
                else if (port3_0.BackColor == Color.Transparent)
                {
                    port3_0.BackColor = Color.SteelBlue;

                }
            }

        }

        private void port4_7_Click(object sender, EventArgs e)
        {
            if (port2_7.BackColor == Color.Firebrick)
            {
                if (port4_7.BackColor == Color.Firebrick)
                {
                    port4_7.BackColor = Color.Transparent;
                    bank0[2, 7].Value =  "0" + bank0[2, 7].Value.ToString().Substring(1);
                }
                else if (port4_7.BackColor == Color.SteelBlue)
                {
                    port4_7.BackColor = Color.Firebrick;
                    bank0[2, 7].Value =  "1" + bank0[2, 7].Value.ToString().Substring(1);
                }
                else if (port4_7.BackColor == Color.Transparent)
                {
                    port4_7.BackColor = Color.SteelBlue;

                }
            }
        }

        private void port4_6_Click(object sender, EventArgs e)
        {
            if (port2_6.BackColor == Color.Firebrick)
            {
                if (port4_6.BackColor == Color.Firebrick)
                {
                    port4_6.BackColor = Color.Transparent;
                    bank0[2, 7].Value = bank0[2, 7].Value.ToString().Substring(0, 1) + "0" + bank0[2, 7].Value.ToString().Substring(2);
                }
                else if (port4_6.BackColor == Color.SteelBlue)
                {
                    port4_6.BackColor = Color.Firebrick;
                    bank0[2, 7].Value = bank0[2, 7].Value.ToString().Substring(0, 1) + "1" + bank0[2, 7].Value.ToString().Substring(2);
                }
                else if (port4_6.BackColor == Color.Transparent)
                {
                    port4_6.BackColor = Color.SteelBlue;

                }
            }
        }

        private void port4_5_Click(object sender, EventArgs e)
        {
            if (port2_5.BackColor == Color.Firebrick)
            {
                if (port4_5.BackColor == Color.Firebrick)
                {
                    port4_5.BackColor = Color.Transparent;
                    bank0[2, 7].Value = bank0[2, 7].Value.ToString().Substring(0, 2) + "0" + bank0[2, 7].Value.ToString().Substring(3);
                }
                else if (port4_5.BackColor == Color.SteelBlue)
                {
                    port4_5.BackColor = Color.Firebrick;
                    bank0[2, 7].Value = bank0[2, 7].Value.ToString().Substring(0, 2) + "1" + bank0[2, 7].Value.ToString().Substring(3);
                }
                else if (port4_5.BackColor == Color.Transparent)
                {
                    port4_5.BackColor = Color.SteelBlue;

                }
            }
        }

        private void port4_4_Click(object sender, EventArgs e)
        {
            if (port2_4.BackColor == Color.Firebrick)
            {
                if (port4_4.BackColor == Color.Firebrick)
                {
                    port4_4.BackColor = Color.Transparent;
                    bank0[2, 7].Value = bank0[2, 7].Value.ToString().Substring(0, 3) + "0" + bank0[2, 7].Value.ToString().Substring(4);
                }
                else if (port4_4.BackColor == Color.SteelBlue)
                {
                    port4_4.BackColor = Color.Firebrick;
                    bank0[2, 7].Value = bank0[2, 7].Value.ToString().Substring(0, 3) + "1" + bank0[2, 7].Value.ToString().Substring(4);
                }
                else if (port4_4.BackColor == Color.Transparent)
                {
                    port4_4.BackColor = Color.SteelBlue;

                }
            }
        }

        private void port4_3_Click(object sender, EventArgs e)
        {
            if (port2_3.BackColor == Color.Firebrick)
            {
                if (port4_3.BackColor == Color.Firebrick)
                {
                    port4_3.BackColor = Color.Transparent;
                    bank0[2, 7].Value = bank0[2, 7].Value.ToString().Substring(0, 4) + "0" + bank0[2, 7].Value.ToString().Substring(5);
                }
                else if (port4_3.BackColor == Color.SteelBlue)
                {
                    port4_3.BackColor = Color.Firebrick;
                    bank0[2, 7].Value = bank0[2, 7].Value.ToString().Substring(0, 4) + "1" + bank0[2, 7].Value.ToString().Substring(5);
                }
                else if (port4_3.BackColor == Color.Transparent)
                {
                    port4_3.BackColor = Color.SteelBlue;

                }
            }
        }

        private void port4_2_Click(object sender, EventArgs e)
        {
            if (port2_2.BackColor == Color.Firebrick)
            {
                if (port4_2.BackColor == Color.Firebrick)
                {
                    port4_2.BackColor = Color.Transparent;
                    bank0[2, 7].Value = bank0[2, 7].Value.ToString().Substring(0, 5) + "0" + bank0[2, 7].Value.ToString().Substring(6);
                }
                else if (port4_2.BackColor == Color.SteelBlue)
                {
                    port4_2.BackColor = Color.Firebrick;
                    bank0[2, 7].Value = bank0[2, 7].Value.ToString().Substring(0, 5) + "1"+ bank0[2, 7].Value.ToString().Substring(6);
                }
                else if (port4_2.BackColor == Color.Transparent)
                {
                    port4_2.BackColor = Color.SteelBlue;

                }
            }
        }

        private void port4_1_Click(object sender, EventArgs e)
        {
            if (port2_1.BackColor == Color.Firebrick)
            {
                if (port4_1.BackColor == Color.Firebrick)
                {
                    port4_1.BackColor = Color.Transparent;
                    bank0[2, 7].Value = bank0[2, 7].Value.ToString().Substring(0, 6) + "0" + bank0[2, 7].Value.ToString().Substring(7);
                }
                else if (port4_1.BackColor == Color.SteelBlue)
                {
                    port4_1.BackColor = Color.Firebrick;
                    bank0[2, 7].Value = bank0[2, 7].Value.ToString().Substring(0, 6) + "1" + bank0[2, 7].Value.ToString().Substring(7);
                }
                else if (port4_1.BackColor == Color.Transparent)
                {
                    port4_1.BackColor = Color.SteelBlue;

                }
            }
        }

        private void port4_0_Click(object sender, EventArgs e)
        {
            if (port2_0.BackColor == Color.Firebrick)
            {
                if (port4_0.BackColor == Color.Firebrick)
                {
                    port4_0.BackColor = Color.Transparent;
                    bank0[2, 7].Value = bank0[2, 7].Value.ToString().Substring(0, 7) + "0";
                }
                else if (port4_0.BackColor == Color.SteelBlue)
                {
                    port4_0.BackColor = Color.Firebrick;
                    bank0[2, 7].Value = bank0[2, 7].Value.ToString().Substring(0, 7) + "1";
                }
                else if (port4_0.BackColor == Color.Transparent)
                {
                    port4_0.BackColor = Color.SteelBlue;

                }
            }
        }

        
    }
}
