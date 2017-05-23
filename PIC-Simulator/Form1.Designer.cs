namespace PIC_Simulator
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.textbox_path = new System.Windows.Forms.TextBox();
            this.btn_path = new System.Windows.Forms.Button();
            this.startbtn = new System.Windows.Forms.Button();
            this.stepbtn = new System.Windows.Forms.Button();
            this.bnt_help = new System.Windows.Forms.Button();
            this.opcodedata = new System.Windows.Forms.DataGridView();
            this.BP = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Index = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HexOpcode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Operand = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bank0 = new System.Windows.Forms.DataGridView();
            this.Register = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Location = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.readbtn = new System.Windows.Forms.Button();
            this.clearbtn = new System.Windows.Forms.Button();
            this.port1_0 = new System.Windows.Forms.Button();
            this.port1_1 = new System.Windows.Forms.Button();
            this.port1_2 = new System.Windows.Forms.Button();
            this.port1_3 = new System.Windows.Forms.Button();
            this.port1_4 = new System.Windows.Forms.Button();
            this.port1_5 = new System.Windows.Forms.Button();
            this.port1_7 = new System.Windows.Forms.Button();
            this.port1_6 = new System.Windows.Forms.Button();
            this.port2_6 = new System.Windows.Forms.Button();
            this.port2_7 = new System.Windows.Forms.Button();
            this.port2_5 = new System.Windows.Forms.Button();
            this.port2_4 = new System.Windows.Forms.Button();
            this.port2_3 = new System.Windows.Forms.Button();
            this.port2_2 = new System.Windows.Forms.Button();
            this.port2_1 = new System.Windows.Forms.Button();
            this.port2_0 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.bank1 = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.speicherzellen = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn_weiter = new System.Windows.Forms.Button();
            this.btn_interrupt = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label21 = new System.Windows.Forms.Label();
            this.port3_0 = new System.Windows.Forms.Button();
            this.port3_1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.port3_2 = new System.Windows.Forms.Button();
            this.port3_3 = new System.Windows.Forms.Button();
            this.port3_4 = new System.Windows.Forms.Button();
            this.port3_5 = new System.Windows.Forms.Button();
            this.port3_7 = new System.Windows.Forms.Button();
            this.port3_6 = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.port4_0 = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.port4_1 = new System.Windows.Forms.Button();
            this.label17 = new System.Windows.Forms.Label();
            this.port4_2 = new System.Windows.Forms.Button();
            this.label18 = new System.Windows.Forms.Label();
            this.port4_3 = new System.Windows.Forms.Button();
            this.label19 = new System.Windows.Forms.Label();
            this.port4_4 = new System.Windows.Forms.Button();
            this.label20 = new System.Windows.Forms.Label();
            this.port4_5 = new System.Windows.Forms.Button();
            this.port4_6 = new System.Windows.Forms.Button();
            this.port4_7 = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.timerinputporta = new System.Windows.Forms.Timer(this.components);
            this.timerinputportb = new System.Windows.Forms.Timer(this.components);
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.label26 = new System.Windows.Forms.Label();
            this.txt_pc = new System.Windows.Forms.TextBox();
            this.label25 = new System.Windows.Forms.Label();
            this.txt_stack = new System.Windows.Forms.TextBox();
            this.btn_conndisconn = new System.Windows.Forms.Button();
            this.btn_comconn = new System.Windows.Forms.Button();
            this.label24 = new System.Windows.Forms.Label();
            this.txt_runtime = new System.Windows.Forms.TextBox();
            this.txt_freq = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.timer_freq = new System.Windows.Forms.Timer(this.components);
            this.timer4 = new System.Windows.Forms.Timer(this.components);
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.timer5 = new System.Windows.Forms.Timer(this.components);
            this.portsettimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.opcodedata)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bank0)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bank1)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.speicherzellen)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.SuspendLayout();
            // 
            // textbox_path
            // 
            this.textbox_path.Location = new System.Drawing.Point(6, 16);
            this.textbox_path.Name = "textbox_path";
            this.textbox_path.Size = new System.Drawing.Size(569, 20);
            this.textbox_path.TabIndex = 0;
            this.textbox_path.Text = "Path...";
            // 
            // btn_path
            // 
            this.btn_path.Location = new System.Drawing.Point(581, 14);
            this.btn_path.Name = "btn_path";
            this.btn_path.Size = new System.Drawing.Size(59, 23);
            this.btn_path.TabIndex = 1;
            this.btn_path.Text = "...";
            this.btn_path.UseVisualStyleBackColor = true;
            this.btn_path.Click += new System.EventHandler(this.button1_Click);
            // 
            // startbtn
            // 
            this.startbtn.Enabled = false;
            this.startbtn.Location = new System.Drawing.Point(6, 14);
            this.startbtn.Name = "startbtn";
            this.startbtn.Size = new System.Drawing.Size(59, 23);
            this.startbtn.TabIndex = 4;
            this.startbtn.Text = "Start";
            this.startbtn.UseVisualStyleBackColor = true;
            this.startbtn.Click += new System.EventHandler(this.startbtn_Click);
            // 
            // stepbtn
            // 
            this.stepbtn.Enabled = false;
            this.stepbtn.Location = new System.Drawing.Point(71, 14);
            this.stepbtn.Name = "stepbtn";
            this.stepbtn.Size = new System.Drawing.Size(59, 23);
            this.stepbtn.TabIndex = 5;
            this.stepbtn.Text = "Step";
            this.stepbtn.UseVisualStyleBackColor = true;
            this.stepbtn.Click += new System.EventHandler(this.stepbtn_Click_1);
            // 
            // bnt_help
            // 
            this.bnt_help.Location = new System.Drawing.Point(581, 41);
            this.bnt_help.Name = "bnt_help";
            this.bnt_help.Size = new System.Drawing.Size(59, 23);
            this.bnt_help.TabIndex = 6;
            this.bnt_help.Text = "Help";
            this.bnt_help.UseVisualStyleBackColor = true;
            this.bnt_help.Click += new System.EventHandler(this.button3_Click);
            // 
            // opcodedata
            // 
            this.opcodedata.AllowUserToAddRows = false;
            this.opcodedata.AllowUserToDeleteRows = false;
            this.opcodedata.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.opcodedata.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.BP,
            this.Index,
            this.HexOpcode,
            this.Operand});
            this.opcodedata.Location = new System.Drawing.Point(7, 43);
            this.opcodedata.Name = "opcodedata";
            this.opcodedata.Size = new System.Drawing.Size(377, 543);
            this.opcodedata.TabIndex = 7;
            // 
            // BP
            // 
            this.BP.HeaderText = "BP";
            this.BP.Name = "BP";
            this.BP.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.BP.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.BP.Width = 31;
            // 
            // Index
            // 
            this.Index.HeaderText = "Index";
            this.Index.Name = "Index";
            this.Index.ReadOnly = true;
            this.Index.Width = 60;
            // 
            // HexOpcode
            // 
            this.HexOpcode.HeaderText = "14-Bit Opcode";
            this.HexOpcode.Name = "HexOpcode";
            this.HexOpcode.ReadOnly = true;
            this.HexOpcode.Width = 110;
            // 
            // Operand
            // 
            this.Operand.HeaderText = "Operand";
            this.Operand.Name = "Operand";
            this.Operand.ReadOnly = true;
            this.Operand.Width = 130;
            // 
            // bank0
            // 
            this.bank0.AllowUserToAddRows = false;
            this.bank0.AllowUserToDeleteRows = false;
            this.bank0.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.bank0.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Register,
            this.Location,
            this.Value});
            this.bank0.Location = new System.Drawing.Point(-3, -1);
            this.bank0.Name = "bank0";
            this.bank0.ReadOnly = true;
            this.bank0.RowHeadersVisible = false;
            this.bank0.Size = new System.Drawing.Size(310, 321);
            this.bank0.TabIndex = 8;
            // 
            // Register
            // 
            this.Register.HeaderText = "Register";
            this.Register.Name = "Register";
            this.Register.ReadOnly = true;
            this.Register.Width = 70;
            // 
            // Location
            // 
            this.Location.HeaderText = "Location";
            this.Location.Name = "Location";
            this.Location.ReadOnly = true;
            this.Location.Width = 60;
            // 
            // Value
            // 
            this.Value.HeaderText = "Value";
            this.Value.Name = "Value";
            this.Value.ReadOnly = true;
            // 
            // readbtn
            // 
            this.readbtn.Location = new System.Drawing.Point(6, 42);
            this.readbtn.Name = "readbtn";
            this.readbtn.Size = new System.Drawing.Size(59, 23);
            this.readbtn.TabIndex = 9;
            this.readbtn.Text = "Read";
            this.readbtn.UseVisualStyleBackColor = true;
            this.readbtn.Click += new System.EventHandler(this.readbtn_Click);
            // 
            // clearbtn
            // 
            this.clearbtn.Location = new System.Drawing.Point(71, 42);
            this.clearbtn.Name = "clearbtn";
            this.clearbtn.Size = new System.Drawing.Size(59, 23);
            this.clearbtn.TabIndex = 10;
            this.clearbtn.Text = "Clear";
            this.clearbtn.UseVisualStyleBackColor = true;
            this.clearbtn.Click += new System.EventHandler(this.clearbtn_Click);
            // 
            // port1_0
            // 
            this.port1_0.BackColor = System.Drawing.Color.Firebrick;
            this.port1_0.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.port1_0.Location = new System.Drawing.Point(194, 32);
            this.port1_0.Name = "port1_0";
            this.port1_0.Size = new System.Drawing.Size(20, 20);
            this.port1_0.TabIndex = 11;
            this.port1_0.UseVisualStyleBackColor = false;
            this.port1_0.Click += new System.EventHandler(this.port1_0_Click);
            // 
            // port1_1
            // 
            this.port1_1.BackColor = System.Drawing.Color.Firebrick;
            this.port1_1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.port1_1.Location = new System.Drawing.Point(173, 32);
            this.port1_1.Name = "port1_1";
            this.port1_1.Size = new System.Drawing.Size(20, 20);
            this.port1_1.TabIndex = 12;
            this.port1_1.UseVisualStyleBackColor = false;
            this.port1_1.Click += new System.EventHandler(this.port1_1_Click);
            // 
            // port1_2
            // 
            this.port1_2.BackColor = System.Drawing.Color.Firebrick;
            this.port1_2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.port1_2.Location = new System.Drawing.Point(152, 32);
            this.port1_2.Name = "port1_2";
            this.port1_2.Size = new System.Drawing.Size(20, 20);
            this.port1_2.TabIndex = 13;
            this.port1_2.UseVisualStyleBackColor = false;
            this.port1_2.Click += new System.EventHandler(this.port1_2_Click);
            // 
            // port1_3
            // 
            this.port1_3.BackColor = System.Drawing.Color.Firebrick;
            this.port1_3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.port1_3.Location = new System.Drawing.Point(131, 32);
            this.port1_3.Name = "port1_3";
            this.port1_3.Size = new System.Drawing.Size(20, 20);
            this.port1_3.TabIndex = 14;
            this.port1_3.UseVisualStyleBackColor = false;
            this.port1_3.Click += new System.EventHandler(this.port1_3_Click);
            // 
            // port1_4
            // 
            this.port1_4.BackColor = System.Drawing.Color.Firebrick;
            this.port1_4.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.port1_4.Location = new System.Drawing.Point(107, 32);
            this.port1_4.Name = "port1_4";
            this.port1_4.Size = new System.Drawing.Size(20, 20);
            this.port1_4.TabIndex = 15;
            this.port1_4.UseVisualStyleBackColor = false;
            this.port1_4.Click += new System.EventHandler(this.port1_4_Click);
            // 
            // port1_5
            // 
            this.port1_5.BackColor = System.Drawing.Color.Gray;
            this.port1_5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.port1_5.Enabled = false;
            this.port1_5.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.port1_5.Location = new System.Drawing.Point(86, 32);
            this.port1_5.Name = "port1_5";
            this.port1_5.Size = new System.Drawing.Size(20, 20);
            this.port1_5.TabIndex = 16;
            this.port1_5.UseVisualStyleBackColor = false;
            this.port1_5.Click += new System.EventHandler(this.port1_5_Click);
            // 
            // port1_7
            // 
            this.port1_7.BackColor = System.Drawing.Color.Gray;
            this.port1_7.Enabled = false;
            this.port1_7.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.port1_7.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.port1_7.Location = new System.Drawing.Point(44, 32);
            this.port1_7.Margin = new System.Windows.Forms.Padding(0);
            this.port1_7.Name = "port1_7";
            this.port1_7.Size = new System.Drawing.Size(20, 20);
            this.port1_7.TabIndex = 17;
            this.port1_7.UseVisualStyleBackColor = false;
            this.port1_7.Click += new System.EventHandler(this.port1_7_Click);
            // 
            // port1_6
            // 
            this.port1_6.BackColor = System.Drawing.Color.Gray;
            this.port1_6.Enabled = false;
            this.port1_6.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.port1_6.Location = new System.Drawing.Point(65, 32);
            this.port1_6.Name = "port1_6";
            this.port1_6.Size = new System.Drawing.Size(20, 20);
            this.port1_6.TabIndex = 18;
            this.port1_6.UseVisualStyleBackColor = false;
            this.port1_6.Click += new System.EventHandler(this.port1_6_Click);
            // 
            // port2_6
            // 
            this.port2_6.BackColor = System.Drawing.Color.Firebrick;
            this.port2_6.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.port2_6.Location = new System.Drawing.Point(65, 36);
            this.port2_6.Name = "port2_6";
            this.port2_6.Size = new System.Drawing.Size(20, 20);
            this.port2_6.TabIndex = 26;
            this.port2_6.UseVisualStyleBackColor = false;
            this.port2_6.Click += new System.EventHandler(this.port2_6_Click);
            // 
            // port2_7
            // 
            this.port2_7.BackColor = System.Drawing.Color.Firebrick;
            this.port2_7.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.port2_7.Location = new System.Drawing.Point(44, 36);
            this.port2_7.Name = "port2_7";
            this.port2_7.Size = new System.Drawing.Size(20, 20);
            this.port2_7.TabIndex = 25;
            this.port2_7.UseVisualStyleBackColor = false;
            this.port2_7.Click += new System.EventHandler(this.port2_7_Click);
            // 
            // port2_5
            // 
            this.port2_5.BackColor = System.Drawing.Color.Firebrick;
            this.port2_5.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.port2_5.Location = new System.Drawing.Point(86, 36);
            this.port2_5.Name = "port2_5";
            this.port2_5.Size = new System.Drawing.Size(20, 20);
            this.port2_5.TabIndex = 24;
            this.port2_5.UseVisualStyleBackColor = false;
            this.port2_5.Click += new System.EventHandler(this.port2_5_Click);
            // 
            // port2_4
            // 
            this.port2_4.BackColor = System.Drawing.Color.Firebrick;
            this.port2_4.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.port2_4.Location = new System.Drawing.Point(107, 36);
            this.port2_4.Name = "port2_4";
            this.port2_4.Size = new System.Drawing.Size(20, 20);
            this.port2_4.TabIndex = 23;
            this.port2_4.UseVisualStyleBackColor = false;
            this.port2_4.Click += new System.EventHandler(this.port2_4_Click);
            // 
            // port2_3
            // 
            this.port2_3.BackColor = System.Drawing.Color.Firebrick;
            this.port2_3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.port2_3.Location = new System.Drawing.Point(131, 36);
            this.port2_3.Name = "port2_3";
            this.port2_3.Size = new System.Drawing.Size(20, 20);
            this.port2_3.TabIndex = 22;
            this.port2_3.UseVisualStyleBackColor = false;
            this.port2_3.Click += new System.EventHandler(this.port2_3_Click);
            // 
            // port2_2
            // 
            this.port2_2.BackColor = System.Drawing.Color.Firebrick;
            this.port2_2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.port2_2.Location = new System.Drawing.Point(152, 36);
            this.port2_2.Name = "port2_2";
            this.port2_2.Size = new System.Drawing.Size(20, 20);
            this.port2_2.TabIndex = 21;
            this.port2_2.UseVisualStyleBackColor = false;
            this.port2_2.Click += new System.EventHandler(this.port2_2_Click);
            // 
            // port2_1
            // 
            this.port2_1.BackColor = System.Drawing.Color.Firebrick;
            this.port2_1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.port2_1.Location = new System.Drawing.Point(173, 36);
            this.port2_1.Name = "port2_1";
            this.port2_1.Size = new System.Drawing.Size(20, 20);
            this.port2_1.TabIndex = 20;
            this.port2_1.UseVisualStyleBackColor = false;
            this.port2_1.Click += new System.EventHandler(this.port2_1_Click);
            // 
            // port2_0
            // 
            this.port2_0.BackColor = System.Drawing.Color.Firebrick;
            this.port2_0.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.port2_0.Location = new System.Drawing.Point(194, 36);
            this.port2_0.Name = "port2_0";
            this.port2_0.Size = new System.Drawing.Size(20, 20);
            this.port2_0.TabIndex = 19;
            this.port2_0.UseVisualStyleBackColor = false;
            this.port2_0.Click += new System.EventHandler(this.port2_0_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 75;
            this.label1.Text = "Tris A";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 76;
            this.label2.Text = "Port A";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(199, 16);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(13, 13);
            this.label9.TabIndex = 83;
            this.label9.Text = "0";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(178, 16);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(13, 13);
            this.label10.TabIndex = 84;
            this.label10.Text = "1";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(157, 16);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(13, 13);
            this.label11.TabIndex = 85;
            this.label11.Text = "2";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(136, 16);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(13, 13);
            this.label12.TabIndex = 86;
            this.label12.Text = "3";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(112, 16);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(13, 13);
            this.label13.TabIndex = 87;
            this.label13.Text = "4";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(91, 16);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(13, 13);
            this.label14.TabIndex = 88;
            this.label14.Text = "5";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(69, 16);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(13, 13);
            this.label15.TabIndex = 89;
            this.label15.Text = "6";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(49, 16);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(13, 13);
            this.label16.TabIndex = 90;
            this.label16.Text = "7";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(6, 18);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(238, 342);
            this.tabControl1.TabIndex = 91;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.bank0);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(230, 316);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Bank 0";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.bank1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(230, 316);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Bank 1";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // bank1
            // 
            this.bank1.AllowUserToAddRows = false;
            this.bank1.AllowUserToDeleteRows = false;
            this.bank1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.bank1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3});
            this.bank1.Location = new System.Drawing.Point(-3, -1);
            this.bank1.Name = "bank1";
            this.bank1.ReadOnly = true;
            this.bank1.RowHeadersVisible = false;
            this.bank1.Size = new System.Drawing.Size(243, 321);
            this.bank1.TabIndex = 9;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Register";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 70;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Location";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 60;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Value";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.speicherzellen);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(230, 316);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Speicherzellen";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // speicherzellen
            // 
            this.speicherzellen.AllowUserToAddRows = false;
            this.speicherzellen.AllowUserToDeleteRows = false;
            this.speicherzellen.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.speicherzellen.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6});
            this.speicherzellen.Location = new System.Drawing.Point(-3, -1);
            this.speicherzellen.Name = "speicherzellen";
            this.speicherzellen.ReadOnly = true;
            this.speicherzellen.RowHeadersVisible = false;
            this.speicherzellen.Size = new System.Drawing.Size(243, 321);
            this.speicherzellen.TabIndex = 10;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "Name";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Width = 70;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "Location";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.Width = 60;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.HeaderText = "Value";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btn_weiter);
            this.groupBox1.Controls.Add(this.btn_interrupt);
            this.groupBox1.Controls.Add(this.opcodedata);
            this.groupBox1.Controls.Add(this.startbtn);
            this.groupBox1.Controls.Add(this.stepbtn);
            this.groupBox1.Location = new System.Drawing.Point(16, 141);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(390, 594);
            this.groupBox1.TabIndex = 92;
            this.groupBox1.TabStop = false;
            // 
            // btn_weiter
            // 
            this.btn_weiter.Enabled = false;
            this.btn_weiter.Location = new System.Drawing.Point(7, 13);
            this.btn_weiter.Name = "btn_weiter";
            this.btn_weiter.Size = new System.Drawing.Size(59, 23);
            this.btn_weiter.TabIndex = 9;
            this.btn_weiter.Text = "Weiter";
            this.btn_weiter.UseVisualStyleBackColor = true;
            this.btn_weiter.Visible = false;
            this.btn_weiter.Click += new System.EventHandler(this.btn_weiter_Click);
            // 
            // btn_interrupt
            // 
            this.btn_interrupt.Enabled = false;
            this.btn_interrupt.Location = new System.Drawing.Point(306, 13);
            this.btn_interrupt.Name = "btn_interrupt";
            this.btn_interrupt.Size = new System.Drawing.Size(78, 23);
            this.btn_interrupt.TabIndex = 8;
            this.btn_interrupt.Text = "INTERRUPT";
            this.btn_interrupt.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tabControl1);
            this.groupBox2.Location = new System.Drawing.Point(412, 141);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(253, 369);
            this.groupBox2.TabIndex = 93;
            this.groupBox2.TabStop = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label21);
            this.groupBox3.Controls.Add(this.port3_0);
            this.groupBox3.Controls.Add(this.port3_1);
            this.groupBox3.Controls.Add(this.textBox1);
            this.groupBox3.Controls.Add(this.port3_2);
            this.groupBox3.Controls.Add(this.label16);
            this.groupBox3.Controls.Add(this.port3_3);
            this.groupBox3.Controls.Add(this.port1_0);
            this.groupBox3.Controls.Add(this.port3_4);
            this.groupBox3.Controls.Add(this.port1_1);
            this.groupBox3.Controls.Add(this.port1_2);
            this.groupBox3.Controls.Add(this.port3_5);
            this.groupBox3.Controls.Add(this.port1_3);
            this.groupBox3.Controls.Add(this.port1_4);
            this.groupBox3.Controls.Add(this.port3_7);
            this.groupBox3.Controls.Add(this.label15);
            this.groupBox3.Controls.Add(this.port3_6);
            this.groupBox3.Controls.Add(this.port1_5);
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Controls.Add(this.port1_7);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.port1_6);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Location = new System.Drawing.Point(412, 529);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(253, 100);
            this.groupBox3.TabIndex = 94;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "A";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(218, 15);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(29, 13);
            this.label21.TabIndex = 93;
            this.label21.Text = "Takt";
            // 
            // port3_0
            // 
            this.port3_0.BackColor = System.Drawing.Color.Transparent;
            this.port3_0.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.port3_0.Location = new System.Drawing.Point(194, 59);
            this.port3_0.Name = "port3_0";
            this.port3_0.Size = new System.Drawing.Size(20, 20);
            this.port3_0.TabIndex = 11;
            this.port3_0.UseVisualStyleBackColor = false;
            this.port3_0.Click += new System.EventHandler(this.port3_0_Click);
            // 
            // port3_1
            // 
            this.port3_1.BackColor = System.Drawing.Color.Transparent;
            this.port3_1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.port3_1.Location = new System.Drawing.Point(173, 59);
            this.port3_1.Name = "port3_1";
            this.port3_1.Size = new System.Drawing.Size(20, 20);
            this.port3_1.TabIndex = 12;
            this.port3_1.UseVisualStyleBackColor = false;
            this.port3_1.Click += new System.EventHandler(this.port3_1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(217, 31);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(30, 20);
            this.textBox1.TabIndex = 91;
            this.textBox1.Text = "300";
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // port3_2
            // 
            this.port3_2.BackColor = System.Drawing.Color.Transparent;
            this.port3_2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.port3_2.Location = new System.Drawing.Point(152, 59);
            this.port3_2.Name = "port3_2";
            this.port3_2.Size = new System.Drawing.Size(20, 20);
            this.port3_2.TabIndex = 13;
            this.port3_2.UseVisualStyleBackColor = false;
            this.port3_2.Click += new System.EventHandler(this.port3_2_Click);
            // 
            // port3_3
            // 
            this.port3_3.BackColor = System.Drawing.Color.Transparent;
            this.port3_3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.port3_3.Location = new System.Drawing.Point(131, 59);
            this.port3_3.Name = "port3_3";
            this.port3_3.Size = new System.Drawing.Size(20, 20);
            this.port3_3.TabIndex = 14;
            this.port3_3.UseVisualStyleBackColor = false;
            this.port3_3.Click += new System.EventHandler(this.port3_3_Click);
            // 
            // port3_4
            // 
            this.port3_4.BackColor = System.Drawing.Color.Transparent;
            this.port3_4.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.port3_4.Location = new System.Drawing.Point(107, 59);
            this.port3_4.Name = "port3_4";
            this.port3_4.Size = new System.Drawing.Size(20, 20);
            this.port3_4.TabIndex = 15;
            this.port3_4.UseVisualStyleBackColor = false;
            this.port3_4.Click += new System.EventHandler(this.port3_4_Click);
            // 
            // port3_5
            // 
            this.port3_5.BackColor = System.Drawing.Color.Gray;
            this.port3_5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.port3_5.Enabled = false;
            this.port3_5.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.port3_5.Location = new System.Drawing.Point(86, 59);
            this.port3_5.Name = "port3_5";
            this.port3_5.Size = new System.Drawing.Size(20, 20);
            this.port3_5.TabIndex = 16;
            this.port3_5.UseVisualStyleBackColor = false;
            // 
            // port3_7
            // 
            this.port3_7.BackColor = System.Drawing.Color.Gray;
            this.port3_7.Enabled = false;
            this.port3_7.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.port3_7.Location = new System.Drawing.Point(44, 59);
            this.port3_7.Margin = new System.Windows.Forms.Padding(0);
            this.port3_7.Name = "port3_7";
            this.port3_7.Size = new System.Drawing.Size(20, 20);
            this.port3_7.TabIndex = 17;
            this.port3_7.UseVisualStyleBackColor = false;
            // 
            // port3_6
            // 
            this.port3_6.BackColor = System.Drawing.Color.Gray;
            this.port3_6.Enabled = false;
            this.port3_6.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.port3_6.Location = new System.Drawing.Point(65, 59);
            this.port3_6.Name = "port3_6";
            this.port3_6.Size = new System.Drawing.Size(20, 20);
            this.port3_6.TabIndex = 18;
            this.port3_6.UseVisualStyleBackColor = false;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(219, 36);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(30, 20);
            this.textBox2.TabIndex = 92;
            this.textBox2.Text = "300";
            this.textBox2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.textBox2);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.port4_0);
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Controls.Add(this.port4_1);
            this.groupBox4.Controls.Add(this.port2_0);
            this.groupBox4.Controls.Add(this.label17);
            this.groupBox4.Controls.Add(this.port4_2);
            this.groupBox4.Controls.Add(this.port2_1);
            this.groupBox4.Controls.Add(this.label18);
            this.groupBox4.Controls.Add(this.port4_3);
            this.groupBox4.Controls.Add(this.port2_2);
            this.groupBox4.Controls.Add(this.label19);
            this.groupBox4.Controls.Add(this.port4_4);
            this.groupBox4.Controls.Add(this.port2_3);
            this.groupBox4.Controls.Add(this.label20);
            this.groupBox4.Controls.Add(this.port4_5);
            this.groupBox4.Controls.Add(this.port2_4);
            this.groupBox4.Controls.Add(this.port4_6);
            this.groupBox4.Controls.Add(this.port4_7);
            this.groupBox4.Controls.Add(this.port2_5);
            this.groupBox4.Controls.Add(this.port2_7);
            this.groupBox4.Controls.Add(this.port2_6);
            this.groupBox4.Location = new System.Drawing.Point(412, 635);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(253, 100);
            this.groupBox4.TabIndex = 95;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "B";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(47, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(13, 13);
            this.label3.TabIndex = 90;
            this.label3.Text = "7";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(67, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(13, 13);
            this.label4.TabIndex = 89;
            this.label4.Text = "6";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(89, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(13, 13);
            this.label5.TabIndex = 88;
            this.label5.Text = "5";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(110, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(13, 13);
            this.label6.TabIndex = 87;
            this.label6.Text = "4";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(134, 16);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(13, 13);
            this.label7.TabIndex = 86;
            this.label7.Text = "3";
            // 
            // port4_0
            // 
            this.port4_0.BackColor = System.Drawing.Color.Transparent;
            this.port4_0.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.port4_0.Location = new System.Drawing.Point(193, 61);
            this.port4_0.Name = "port4_0";
            this.port4_0.Size = new System.Drawing.Size(20, 20);
            this.port4_0.TabIndex = 19;
            this.port4_0.UseVisualStyleBackColor = false;
            this.port4_0.Click += new System.EventHandler(this.port4_0_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(155, 16);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(13, 13);
            this.label8.TabIndex = 85;
            this.label8.Text = "2";
            // 
            // port4_1
            // 
            this.port4_1.BackColor = System.Drawing.Color.Transparent;
            this.port4_1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.port4_1.Location = new System.Drawing.Point(172, 61);
            this.port4_1.Name = "port4_1";
            this.port4_1.Size = new System.Drawing.Size(20, 20);
            this.port4_1.TabIndex = 20;
            this.port4_1.UseVisualStyleBackColor = false;
            this.port4_1.Click += new System.EventHandler(this.port4_1_Click);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(176, 16);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(13, 13);
            this.label17.TabIndex = 84;
            this.label17.Text = "1";
            // 
            // port4_2
            // 
            this.port4_2.BackColor = System.Drawing.Color.Transparent;
            this.port4_2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.port4_2.Location = new System.Drawing.Point(151, 61);
            this.port4_2.Name = "port4_2";
            this.port4_2.Size = new System.Drawing.Size(20, 20);
            this.port4_2.TabIndex = 21;
            this.port4_2.UseVisualStyleBackColor = false;
            this.port4_2.Click += new System.EventHandler(this.port4_2_Click);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(197, 16);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(13, 13);
            this.label18.TabIndex = 83;
            this.label18.Text = "0";
            // 
            // port4_3
            // 
            this.port4_3.BackColor = System.Drawing.Color.Transparent;
            this.port4_3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.port4_3.Location = new System.Drawing.Point(130, 61);
            this.port4_3.Name = "port4_3";
            this.port4_3.Size = new System.Drawing.Size(20, 20);
            this.port4_3.TabIndex = 22;
            this.port4_3.UseVisualStyleBackColor = false;
            this.port4_3.Click += new System.EventHandler(this.port4_3_Click);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(4, 64);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(36, 13);
            this.label19.TabIndex = 76;
            this.label19.Text = "Port B";
            // 
            // port4_4
            // 
            this.port4_4.BackColor = System.Drawing.Color.Transparent;
            this.port4_4.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.port4_4.Location = new System.Drawing.Point(106, 61);
            this.port4_4.Name = "port4_4";
            this.port4_4.Size = new System.Drawing.Size(20, 20);
            this.port4_4.TabIndex = 23;
            this.port4_4.UseVisualStyleBackColor = false;
            this.port4_4.Click += new System.EventHandler(this.port4_4_Click);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(4, 35);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(34, 13);
            this.label20.TabIndex = 75;
            this.label20.Text = "Tris B";
            // 
            // port4_5
            // 
            this.port4_5.BackColor = System.Drawing.Color.Transparent;
            this.port4_5.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.port4_5.Location = new System.Drawing.Point(85, 61);
            this.port4_5.Name = "port4_5";
            this.port4_5.Size = new System.Drawing.Size(20, 20);
            this.port4_5.TabIndex = 24;
            this.port4_5.UseVisualStyleBackColor = false;
            this.port4_5.Click += new System.EventHandler(this.port4_5_Click);
            // 
            // port4_6
            // 
            this.port4_6.BackColor = System.Drawing.Color.Transparent;
            this.port4_6.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.port4_6.Location = new System.Drawing.Point(64, 61);
            this.port4_6.Name = "port4_6";
            this.port4_6.Size = new System.Drawing.Size(20, 20);
            this.port4_6.TabIndex = 26;
            this.port4_6.UseVisualStyleBackColor = false;
            this.port4_6.Click += new System.EventHandler(this.port4_6_Click);
            // 
            // port4_7
            // 
            this.port4_7.BackColor = System.Drawing.Color.Transparent;
            this.port4_7.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.port4_7.Location = new System.Drawing.Point(43, 61);
            this.port4_7.Name = "port4_7";
            this.port4_7.Size = new System.Drawing.Size(20, 20);
            this.port4_7.TabIndex = 25;
            this.port4_7.UseVisualStyleBackColor = false;
            this.port4_7.Click += new System.EventHandler(this.port4_7_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.textbox_path);
            this.groupBox5.Controls.Add(this.readbtn);
            this.groupBox5.Controls.Add(this.clearbtn);
            this.groupBox5.Controls.Add(this.btn_path);
            this.groupBox5.Controls.Add(this.bnt_help);
            this.groupBox5.Location = new System.Drawing.Point(16, 4);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(649, 70);
            this.groupBox5.TabIndex = 96;
            this.groupBox5.TabStop = false;
            // 
            // timerinputporta
            // 
            this.timerinputporta.Tick += new System.EventHandler(this.timerinputporta_Tick);
            // 
            // timerinputportb
            // 
            this.timerinputportb.Tick += new System.EventHandler(this.timerinputportb_Tick);
            // 
            // groupBox7
            // 
            this.groupBox7.BackColor = System.Drawing.Color.Transparent;
            this.groupBox7.Controls.Add(this.label26);
            this.groupBox7.Controls.Add(this.txt_pc);
            this.groupBox7.Controls.Add(this.label25);
            this.groupBox7.Controls.Add(this.txt_stack);
            this.groupBox7.Controls.Add(this.btn_conndisconn);
            this.groupBox7.Controls.Add(this.btn_comconn);
            this.groupBox7.Controls.Add(this.label24);
            this.groupBox7.Controls.Add(this.txt_runtime);
            this.groupBox7.Controls.Add(this.txt_freq);
            this.groupBox7.Controls.Add(this.label23);
            this.groupBox7.Controls.Add(this.label22);
            this.groupBox7.Location = new System.Drawing.Point(16, 74);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(649, 68);
            this.groupBox7.TabIndex = 97;
            this.groupBox7.TabStop = false;
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(322, 22);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(21, 13);
            this.label26.TabIndex = 10;
            this.label26.Text = "PC";
            // 
            // txt_pc
            // 
            this.txt_pc.Location = new System.Drawing.Point(319, 41);
            this.txt_pc.Name = "txt_pc";
            this.txt_pc.Size = new System.Drawing.Size(100, 20);
            this.txt_pc.TabIndex = 9;
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(428, 22);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(35, 13);
            this.label25.TabIndex = 8;
            this.label25.Text = "Stack";
            // 
            // txt_stack
            // 
            this.txt_stack.Location = new System.Drawing.Point(428, 40);
            this.txt_stack.Name = "txt_stack";
            this.txt_stack.Size = new System.Drawing.Size(100, 20);
            this.txt_stack.TabIndex = 7;
            // 
            // btn_conndisconn
            // 
            this.btn_conndisconn.Location = new System.Drawing.Point(534, 40);
            this.btn_conndisconn.Name = "btn_conndisconn";
            this.btn_conndisconn.Size = new System.Drawing.Size(106, 23);
            this.btn_conndisconn.TabIndex = 6;
            this.btn_conndisconn.Text = "COM ausschließen";
            this.btn_conndisconn.UseVisualStyleBackColor = true;
            this.btn_conndisconn.Click += new System.EventHandler(this.button4_Click);
            // 
            // btn_comconn
            // 
            this.btn_comconn.Location = new System.Drawing.Point(534, 11);
            this.btn_comconn.Name = "btn_comconn";
            this.btn_comconn.Size = new System.Drawing.Size(106, 23);
            this.btn_comconn.TabIndex = 5;
            this.btn_comconn.Text = "COM anschließen";
            this.btn_comconn.UseVisualStyleBackColor = true;
            this.btn_comconn.Click += new System.EventHandler(this.button2_Click);
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(158, 44);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(18, 13);
            this.label24.TabIndex = 4;
            this.label24.Text = "µs";
            // 
            // txt_runtime
            // 
            this.txt_runtime.Location = new System.Drawing.Point(90, 41);
            this.txt_runtime.Name = "txt_runtime";
            this.txt_runtime.Size = new System.Drawing.Size(62, 20);
            this.txt_runtime.TabIndex = 3;
            this.txt_runtime.Text = "0";
            // 
            // txt_freq
            // 
            this.txt_freq.Location = new System.Drawing.Point(90, 15);
            this.txt_freq.Name = "txt_freq";
            this.txt_freq.Size = new System.Drawing.Size(62, 20);
            this.txt_freq.TabIndex = 2;
            this.txt_freq.Text = "4000";
            this.txt_freq.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(6, 44);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(46, 13);
            this.label23.TabIndex = 1;
            this.label23.Text = "Runtime";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(6, 22);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(76, 13);
            this.label22.TabIndex = 0;
            this.label22.Text = "Quarzfrequenz";
            // 
            // timer_freq
            // 
            this.timer_freq.Tick += new System.EventHandler(this.timer3_Tick);
            // 
            // timer4
            // 
            this.timer4.Tick += new System.EventHandler(this.timer4_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(675, 739);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.Text = "PIC - Simulator";
            ((System.ComponentModel.ISupportInitialize)(this.opcodedata)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bank0)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bank1)).EndInit();
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.speicherzellen)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textbox_path;
        private System.Windows.Forms.Button btn_path;
        private System.Windows.Forms.Button startbtn;
        private System.Windows.Forms.Button stepbtn;
        private System.Windows.Forms.Button bnt_help;
        private System.Windows.Forms.DataGridView opcodedata;
        private System.Windows.Forms.DataGridView bank0;
        private System.Windows.Forms.Button readbtn;
        private System.Windows.Forms.Button clearbtn;
        private System.Windows.Forms.Button port1_0;
        private System.Windows.Forms.Button port1_1;
        private System.Windows.Forms.Button port1_2;
        private System.Windows.Forms.Button port1_3;
        private System.Windows.Forms.Button port1_4;
        private System.Windows.Forms.Button port1_5;
        private System.Windows.Forms.Button port1_7;
        private System.Windows.Forms.Button port1_6;
        private System.Windows.Forms.Button port2_6;
        private System.Windows.Forms.Button port2_7;
        private System.Windows.Forms.Button port2_5;
        private System.Windows.Forms.Button port2_4;
        private System.Windows.Forms.Button port2_3;
        private System.Windows.Forms.Button port2_2;
        private System.Windows.Forms.Button port2_1;
        private System.Windows.Forms.Button port2_0;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView bank1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Register;
        private System.Windows.Forms.DataGridViewTextBoxColumn Location;
        private System.Windows.Forms.DataGridViewTextBoxColumn Value;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.DataGridView speicherzellen;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button port3_0;
        private System.Windows.Forms.Button port3_1;
        private System.Windows.Forms.Button port3_2;
        private System.Windows.Forms.Button port3_3;
        private System.Windows.Forms.Button port3_4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button port3_5;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button port3_7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button port3_6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button port4_0;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button port4_1;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Button port4_2;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Button port4_3;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Button port4_4;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Button port4_5;
        private System.Windows.Forms.Button port4_6;
        private System.Windows.Forms.Button port4_7;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button btn_interrupt;
        private System.Windows.Forms.Timer timerinputporta;
        private System.Windows.Forms.Timer timerinputportb;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn BP;
        private System.Windows.Forms.DataGridViewTextBoxColumn Index;
        private System.Windows.Forms.DataGridViewTextBoxColumn HexOpcode;
        private System.Windows.Forms.DataGridViewTextBoxColumn Operand;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.TextBox txt_freq;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.TextBox txt_runtime;
        private System.Windows.Forms.Timer timer_freq;
        private System.Windows.Forms.Timer timer4;
        public System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.Button btn_conndisconn;
        private System.Windows.Forms.Button btn_comconn;
        private System.Windows.Forms.Timer timer5;
        private System.Windows.Forms.TextBox txt_stack;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Button btn_weiter;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.TextBox txt_pc;
        private System.Windows.Forms.Timer portsettimer;
    }
}

