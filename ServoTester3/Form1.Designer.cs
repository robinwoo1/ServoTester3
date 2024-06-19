namespace ServoTester3
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            groupBox9 = new GroupBox();
            groupBox11 = new GroupBox();
            rbHardAutocustom = new RadioButton();
            rbSoftAutocustom = new RadioButton();
            btSoftHardAutocustom = new Button();
            groupBox10 = new GroupBox();
            rbStartAutocustom = new RadioButton();
            rbStopAutocustom = new RadioButton();
            btStartStopAutocustom = new Button();
            tbTargetSpeed = new TextBox();
            label10 = new Label();
            tbSeatingPoint = new TextBox();
            label11 = new Label();
            tbFreeSpeed = new TextBox();
            label8 = new Label();
            tbFreeAngle = new TextBox();
            label9 = new Label();
            groupBox8 = new GroupBox();
            btStartOrigin = new Button();
            btSaveOrigin = new Button();
            groupBox7 = new GroupBox();
            tbTqSensorValue = new TextBox();
            label7 = new Label();
            tbTqOffsetValue = new TextBox();
            btTqOffsetCheck = new Button();
            btTqOffsetSave = new Button();
            label6 = new Label();
            groupBox6 = new GroupBox();
            rbCalibUserStop = new RadioButton();
            rbCalibFail = new RadioButton();
            rbCalibSuccess = new RadioButton();
            groupBox5 = new GroupBox();
            rbCalibFinish = new RadioButton();
            rbCalibBackward = new RadioButton();
            rbCalibForward = new RadioButton();
            rbCalibHold = new RadioButton();
            rbCalibNone = new RadioButton();
            groupBox4 = new GroupBox();
            btCalibStop = new Button();
            btCalibStart = new Button();
            tabPage2 = new TabPage();
            formsPlot1 = new ScottPlot.WinForms.FormsPlot();
            tabPage3 = new TabPage();
            groupBox1 = new GroupBox();
            btCommOpen = new Button();
            cbBaudrate = new ComboBox();
            label2 = new Label();
            btCommRefresh = new Button();
            cbCommPorts = new ComboBox();
            label1 = new Label();
            workTimer = new System.Windows.Forms.Timer(components);
            groupBox2 = new GroupBox();
            button1 = new Button();
            rbOn = new RadioButton();
            rbOff = new RadioButton();
            tbError = new TextBox();
            btAlarmReset = new Button();
            btResetMc = new Button();
            label4 = new Label();
            groupBox3 = new GroupBox();
            label5 = new Label();
            tbDriverType = new TextBox();
            tbEnc = new TextBox();
            btMcInit = new Button();
            label3 = new Label();
            groupBox12 = new GroupBox();
            btStartStopFL = new Button();
            tbLoosenAngle = new TextBox();
            btFastenLoosen = new Button();
            groupBox13 = new GroupBox();
            button2 = new Button();
            button3 = new Button();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            groupBox9.SuspendLayout();
            groupBox11.SuspendLayout();
            groupBox10.SuspendLayout();
            groupBox8.SuspendLayout();
            groupBox7.SuspendLayout();
            groupBox6.SuspendLayout();
            groupBox5.SuspendLayout();
            groupBox4.SuspendLayout();
            tabPage2.SuspendLayout();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox12.SuspendLayout();
            groupBox13.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Location = new Point(0, 90);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(829, 372);
            tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(groupBox9);
            tabPage1.Controls.Add(groupBox8);
            tabPage1.Controls.Add(groupBox7);
            tabPage1.Controls.Add(groupBox6);
            tabPage1.Controls.Add(groupBox5);
            tabPage1.Controls.Add(groupBox4);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(821, 344);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Function";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox9
            // 
            groupBox9.Controls.Add(groupBox11);
            groupBox9.Controls.Add(groupBox10);
            groupBox9.Controls.Add(tbTargetSpeed);
            groupBox9.Controls.Add(label10);
            groupBox9.Controls.Add(tbSeatingPoint);
            groupBox9.Controls.Add(label11);
            groupBox9.Controls.Add(tbFreeSpeed);
            groupBox9.Controls.Add(label8);
            groupBox9.Controls.Add(tbFreeAngle);
            groupBox9.Controls.Add(label9);
            groupBox9.Location = new Point(364, 118);
            groupBox9.Name = "groupBox9";
            groupBox9.Size = new Size(201, 223);
            groupBox9.TabIndex = 9;
            groupBox9.TabStop = false;
            groupBox9.Text = "auto-customize";
            // 
            // groupBox11
            // 
            groupBox11.Controls.Add(rbHardAutocustom);
            groupBox11.Controls.Add(rbSoftAutocustom);
            groupBox11.Controls.Add(btSoftHardAutocustom);
            groupBox11.Location = new Point(6, 14);
            groupBox11.Name = "groupBox11";
            groupBox11.Size = new Size(187, 45);
            groupBox11.TabIndex = 14;
            groupBox11.TabStop = false;
            groupBox11.Text = "Soft/Hard";
            // 
            // rbHardAutocustom
            // 
            rbHardAutocustom.AutoSize = true;
            rbHardAutocustom.Location = new Point(123, 19);
            rbHardAutocustom.Name = "rbHardAutocustom";
            rbHardAutocustom.Size = new Size(51, 19);
            rbHardAutocustom.TabIndex = 7;
            rbHardAutocustom.Text = "Hard";
            rbHardAutocustom.UseVisualStyleBackColor = true;
            // 
            // rbSoftAutocustom
            // 
            rbSoftAutocustom.AutoSize = true;
            rbSoftAutocustom.Checked = true;
            rbSoftAutocustom.Location = new Point(74, 19);
            rbSoftAutocustom.Name = "rbSoftAutocustom";
            rbSoftAutocustom.Size = new Size(47, 19);
            rbSoftAutocustom.TabIndex = 6;
            rbSoftAutocustom.TabStop = true;
            rbSoftAutocustom.Text = "Soft";
            rbSoftAutocustom.UseVisualStyleBackColor = true;
            // 
            // btSoftHardAutocustom
            // 
            btSoftHardAutocustom.Location = new Point(8, 17);
            btSoftHardAutocustom.Name = "btSoftHardAutocustom";
            btSoftHardAutocustom.Size = new Size(59, 21);
            btSoftHardAutocustom.TabIndex = 5;
            btSoftHardAutocustom.Text = "Hard";
            btSoftHardAutocustom.UseVisualStyleBackColor = true;
            // 
            // groupBox10
            // 
            groupBox10.Controls.Add(rbStartAutocustom);
            groupBox10.Controls.Add(rbStopAutocustom);
            groupBox10.Controls.Add(btStartStopAutocustom);
            groupBox10.Location = new Point(6, 58);
            groupBox10.Name = "groupBox10";
            groupBox10.Size = new Size(187, 45);
            groupBox10.TabIndex = 13;
            groupBox10.TabStop = false;
            groupBox10.Text = "Start/Stop";
            // 
            // rbStartAutocustom
            // 
            rbStartAutocustom.AutoSize = true;
            rbStartAutocustom.Location = new Point(123, 19);
            rbStartAutocustom.Name = "rbStartAutocustom";
            rbStartAutocustom.Size = new Size(50, 19);
            rbStartAutocustom.TabIndex = 7;
            rbStartAutocustom.Text = "Start";
            rbStartAutocustom.UseVisualStyleBackColor = true;
            // 
            // rbStopAutocustom
            // 
            rbStopAutocustom.AutoSize = true;
            rbStopAutocustom.Checked = true;
            rbStopAutocustom.Location = new Point(74, 19);
            rbStopAutocustom.Name = "rbStopAutocustom";
            rbStopAutocustom.Size = new Size(50, 19);
            rbStopAutocustom.TabIndex = 6;
            rbStopAutocustom.TabStop = true;
            rbStopAutocustom.Text = "Stop";
            rbStopAutocustom.UseVisualStyleBackColor = true;
            // 
            // btStartStopAutocustom
            // 
            btStartStopAutocustom.Location = new Point(8, 18);
            btStartStopAutocustom.Name = "btStartStopAutocustom";
            btStartStopAutocustom.Size = new Size(59, 20);
            btStartStopAutocustom.TabIndex = 5;
            btStartStopAutocustom.Text = "Start";
            btStartStopAutocustom.UseVisualStyleBackColor = true;
            // 
            // tbTargetSpeed
            // 
            tbTargetSpeed.Location = new Point(104, 108);
            tbTargetSpeed.Name = "tbTargetSpeed";
            tbTargetSpeed.ReadOnly = true;
            tbTargetSpeed.Size = new Size(75, 23);
            tbTargetSpeed.TabIndex = 12;
            tbTargetSpeed.Text = "0";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(23, 111);
            label10.Name = "label10";
            label10.Size = new Size(77, 15);
            label10.TabIndex = 11;
            label10.Text = "Target Speed";
            // 
            // tbSeatingPoint
            // 
            tbSeatingPoint.Location = new Point(104, 135);
            tbSeatingPoint.Name = "tbSeatingPoint";
            tbSeatingPoint.ReadOnly = true;
            tbSeatingPoint.Size = new Size(75, 23);
            tbSeatingPoint.TabIndex = 10;
            tbSeatingPoint.Text = "0";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(22, 138);
            label11.Name = "label11";
            label11.Size = new Size(79, 15);
            label11.TabIndex = 9;
            label11.Text = "Seating Point";
            // 
            // tbFreeSpeed
            // 
            tbFreeSpeed.Location = new Point(104, 163);
            tbFreeSpeed.Name = "tbFreeSpeed";
            tbFreeSpeed.ReadOnly = true;
            tbFreeSpeed.Size = new Size(75, 23);
            tbFreeSpeed.TabIndex = 8;
            tbFreeSpeed.Text = "0";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(23, 166);
            label8.Name = "label8";
            label8.Size = new Size(66, 15);
            label8.TabIndex = 7;
            label8.Text = "Free Speed";
            // 
            // tbFreeAngle
            // 
            tbFreeAngle.Location = new Point(104, 190);
            tbFreeAngle.Name = "tbFreeAngle";
            tbFreeAngle.ReadOnly = true;
            tbFreeAngle.Size = new Size(75, 23);
            tbFreeAngle.TabIndex = 6;
            tbFreeAngle.Text = "0";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(22, 193);
            label9.Name = "label9";
            label9.Size = new Size(64, 15);
            label9.TabIndex = 0;
            label9.Text = "Free Angle";
            // 
            // groupBox8
            // 
            groupBox8.Controls.Add(btStartOrigin);
            groupBox8.Controls.Add(btSaveOrigin);
            groupBox8.Location = new Point(8, 164);
            groupBox8.Name = "groupBox8";
            groupBox8.Size = new Size(224, 45);
            groupBox8.TabIndex = 2;
            groupBox8.TabStop = false;
            groupBox8.Text = "Move Origin";
            // 
            // btStartOrigin
            // 
            btStartOrigin.Location = new Point(122, 18);
            btStartOrigin.Name = "btStartOrigin";
            btStartOrigin.Size = new Size(87, 23);
            btStartOrigin.TabIndex = 1;
            btStartOrigin.Text = "Start Origin";
            btStartOrigin.UseVisualStyleBackColor = true;
            // 
            // btSaveOrigin
            // 
            btSaveOrigin.Location = new Point(15, 18);
            btSaveOrigin.Name = "btSaveOrigin";
            btSaveOrigin.Size = new Size(84, 23);
            btSaveOrigin.TabIndex = 0;
            btSaveOrigin.Text = "Save Origin";
            btSaveOrigin.UseVisualStyleBackColor = true;
            // 
            // groupBox7
            // 
            groupBox7.Controls.Add(tbTqSensorValue);
            groupBox7.Controls.Add(label7);
            groupBox7.Controls.Add(tbTqOffsetValue);
            groupBox7.Controls.Add(btTqOffsetCheck);
            groupBox7.Controls.Add(btTqOffsetSave);
            groupBox7.Controls.Add(label6);
            groupBox7.Location = new Point(364, 8);
            groupBox7.Name = "groupBox7";
            groupBox7.Size = new Size(201, 107);
            groupBox7.TabIndex = 7;
            groupBox7.TabStop = false;
            groupBox7.Text = "Torque Offset Setting";
            // 
            // tbTqSensorValue
            // 
            tbTqSensorValue.Location = new Point(93, 47);
            tbTqSensorValue.Name = "tbTqSensorValue";
            tbTqSensorValue.ReadOnly = true;
            tbTqSensorValue.Size = new Size(75, 23);
            tbTqSensorValue.TabIndex = 8;
            tbTqSensorValue.Text = "0";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(12, 50);
            label7.Name = "label7";
            label7.Size = new Size(77, 15);
            label7.TabIndex = 7;
            label7.Text = "Sensor Value";
            // 
            // tbTqOffsetValue
            // 
            tbTqOffsetValue.Location = new Point(93, 74);
            tbTqOffsetValue.Name = "tbTqOffsetValue";
            tbTqOffsetValue.ReadOnly = true;
            tbTqOffsetValue.Size = new Size(75, 23);
            tbTqOffsetValue.TabIndex = 6;
            tbTqOffsetValue.Text = "0";
            // 
            // btTqOffsetCheck
            // 
            btTqOffsetCheck.Location = new Point(6, 18);
            btTqOffsetCheck.Name = "btTqOffsetCheck";
            btTqOffsetCheck.Size = new Size(89, 23);
            btTqOffsetCheck.TabIndex = 5;
            btTqOffsetCheck.Text = "Check Offset";
            btTqOffsetCheck.UseVisualStyleBackColor = true;
            // 
            // btTqOffsetSave
            // 
            btTqOffsetSave.Location = new Point(101, 18);
            btTqOffsetSave.Name = "btTqOffsetSave";
            btTqOffsetSave.Size = new Size(92, 23);
            btTqOffsetSave.TabIndex = 2;
            btTqOffsetSave.Text = "Save Offset";
            btTqOffsetSave.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(11, 77);
            label6.Name = "label6";
            label6.Size = new Size(73, 15);
            label6.TabIndex = 0;
            label6.Text = "Offset Value";
            // 
            // groupBox6
            // 
            groupBox6.Controls.Add(rbCalibUserStop);
            groupBox6.Controls.Add(rbCalibFail);
            groupBox6.Controls.Add(rbCalibSuccess);
            groupBox6.Location = new Point(5, 100);
            groupBox6.Name = "groupBox6";
            groupBox6.Size = new Size(335, 41);
            groupBox6.TabIndex = 5;
            groupBox6.TabStop = false;
            groupBox6.Text = "Initial Angle Result";
            // 
            // rbCalibUserStop
            // 
            rbCalibUserStop.AutoSize = true;
            rbCalibUserStop.Location = new Point(125, 18);
            rbCalibUserStop.Name = "rbCalibUserStop";
            rbCalibUserStop.Size = new Size(73, 19);
            rbCalibUserStop.TabIndex = 4;
            rbCalibUserStop.Text = "UserStop";
            rbCalibUserStop.UseVisualStyleBackColor = true;
            // 
            // rbCalibFail
            // 
            rbCalibFail.AutoSize = true;
            rbCalibFail.Location = new Point(76, 18);
            rbCalibFail.Name = "rbCalibFail";
            rbCalibFail.Size = new Size(43, 19);
            rbCalibFail.TabIndex = 2;
            rbCalibFail.Text = "Fail";
            rbCalibFail.UseVisualStyleBackColor = true;
            // 
            // rbCalibSuccess
            // 
            rbCalibSuccess.AutoSize = true;
            rbCalibSuccess.Checked = true;
            rbCalibSuccess.Location = new Point(3, 18);
            rbCalibSuccess.Name = "rbCalibSuccess";
            rbCalibSuccess.Size = new Size(67, 19);
            rbCalibSuccess.TabIndex = 0;
            rbCalibSuccess.TabStop = true;
            rbCalibSuccess.Text = "Success";
            rbCalibSuccess.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(rbCalibFinish);
            groupBox5.Controls.Add(rbCalibBackward);
            groupBox5.Controls.Add(rbCalibForward);
            groupBox5.Controls.Add(rbCalibHold);
            groupBox5.Controls.Add(rbCalibNone);
            groupBox5.Location = new Point(8, 57);
            groupBox5.Name = "groupBox5";
            groupBox5.Size = new Size(335, 41);
            groupBox5.TabIndex = 1;
            groupBox5.TabStop = false;
            groupBox5.Text = "Initial Angle Step";
            // 
            // rbCalibFinish
            // 
            rbCalibFinish.AutoSize = true;
            rbCalibFinish.Location = new Point(276, 18);
            rbCalibFinish.Name = "rbCalibFinish";
            rbCalibFinish.Size = new Size(56, 19);
            rbCalibFinish.TabIndex = 4;
            rbCalibFinish.Text = "Finish";
            rbCalibFinish.UseVisualStyleBackColor = true;
            // 
            // rbCalibBackward
            // 
            rbCalibBackward.AutoSize = true;
            rbCalibBackward.Location = new Point(194, 18);
            rbCalibBackward.Name = "rbCalibBackward";
            rbCalibBackward.Size = new Size(76, 19);
            rbCalibBackward.TabIndex = 3;
            rbCalibBackward.Text = "Backward";
            rbCalibBackward.UseVisualStyleBackColor = true;
            // 
            // rbCalibForward
            // 
            rbCalibForward.AutoSize = true;
            rbCalibForward.Location = new Point(120, 18);
            rbCalibForward.Name = "rbCalibForward";
            rbCalibForward.Size = new Size(68, 19);
            rbCalibForward.TabIndex = 2;
            rbCalibForward.Text = "Forward";
            rbCalibForward.UseVisualStyleBackColor = true;
            // 
            // rbCalibHold
            // 
            rbCalibHold.AutoSize = true;
            rbCalibHold.Location = new Point(63, 18);
            rbCalibHold.Name = "rbCalibHold";
            rbCalibHold.Size = new Size(51, 19);
            rbCalibHold.TabIndex = 1;
            rbCalibHold.Text = "Hold";
            rbCalibHold.UseVisualStyleBackColor = true;
            // 
            // rbCalibNone
            // 
            rbCalibNone.AutoSize = true;
            rbCalibNone.Checked = true;
            rbCalibNone.Location = new Point(3, 18);
            rbCalibNone.Name = "rbCalibNone";
            rbCalibNone.Size = new Size(54, 19);
            rbCalibNone.TabIndex = 0;
            rbCalibNone.TabStop = true;
            rbCalibNone.Text = "None";
            rbCalibNone.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(btCalibStop);
            groupBox4.Controls.Add(btCalibStart);
            groupBox4.Location = new Point(8, 10);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(200, 45);
            groupBox4.TabIndex = 0;
            groupBox4.TabStop = false;
            groupBox4.Text = "Initial Angle Start/Stop";
            // 
            // btCalibStop
            // 
            btCalibStop.Location = new Point(112, 18);
            btCalibStop.Name = "btCalibStop";
            btCalibStop.Size = new Size(75, 23);
            btCalibStop.TabIndex = 1;
            btCalibStop.Text = "Stop";
            btCalibStop.UseVisualStyleBackColor = true;
            // 
            // btCalibStart
            // 
            btCalibStart.Location = new Point(15, 18);
            btCalibStart.Name = "btCalibStart";
            btCalibStart.Size = new Size(75, 23);
            btCalibStart.TabIndex = 0;
            btCalibStart.Text = "Start";
            btCalibStart.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(formsPlot1);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(821, 334);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Graph";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // formsPlot1
            // 
            formsPlot1.DisplayScale = 1F;
            formsPlot1.Location = new Point(93, 42);
            formsPlot1.Name = "formsPlot1";
            formsPlot1.Size = new Size(725, 289);
            formsPlot1.TabIndex = 0;
            // 
            // tabPage3
            // 
            tabPage3.Location = new Point(4, 24);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(3);
            tabPage3.Size = new Size(821, 334);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "tabPage3";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btCommOpen);
            groupBox1.Controls.Add(cbBaudrate);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(btCommRefresh);
            groupBox1.Controls.Add(cbCommPorts);
            groupBox1.Controls.Add(label1);
            groupBox1.Location = new Point(0, 0);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(221, 74);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            groupBox1.Text = "CommPort";
            // 
            // btCommOpen
            // 
            btCommOpen.Location = new Point(140, 45);
            btCommOpen.Name = "btCommOpen";
            btCommOpen.Size = new Size(75, 23);
            btCommOpen.TabIndex = 5;
            btCommOpen.Text = "Open";
            btCommOpen.UseVisualStyleBackColor = true;
            btCommOpen.Click += btCommOpen_Click;
            // 
            // cbBaudrate
            // 
            cbBaudrate.DropDownStyle = ComboBoxStyle.DropDownList;
            cbBaudrate.FormattingEnabled = true;
            cbBaudrate.Items.AddRange(new object[] { "921600", "460800", "230400", "115200", "57600", "38400", "19200", "9600" });
            cbBaudrate.Location = new Point(62, 46);
            cbBaudrate.Name = "cbBaudrate";
            cbBaudrate.Size = new Size(73, 23);
            cbBaudrate.TabIndex = 4;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(4, 49);
            label2.Name = "label2";
            label2.Size = new Size(54, 15);
            label2.TabIndex = 3;
            label2.Text = "Baudrate";
            // 
            // btCommRefresh
            // 
            btCommRefresh.Location = new Point(140, 17);
            btCommRefresh.Name = "btCommRefresh";
            btCommRefresh.Size = new Size(75, 23);
            btCommRefresh.TabIndex = 2;
            btCommRefresh.Text = "Refresh";
            btCommRefresh.UseVisualStyleBackColor = true;
            btCommRefresh.Click += btCommRefresh_Click;
            // 
            // cbCommPorts
            // 
            cbCommPorts.DropDownStyle = ComboBoxStyle.DropDownList;
            cbCommPorts.FormattingEnabled = true;
            cbCommPorts.Location = new Point(61, 18);
            cbCommPorts.Name = "cbCommPorts";
            cbCommPorts.Size = new Size(73, 23);
            cbCommPorts.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(13, 21);
            label1.Name = "label1";
            label1.Size = new Size(29, 15);
            label1.TabIndex = 0;
            label1.Text = "Port";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(button1);
            groupBox2.Controls.Add(rbOn);
            groupBox2.Controls.Add(rbOff);
            groupBox2.Controls.Add(tbError);
            groupBox2.Controls.Add(btAlarmReset);
            groupBox2.Controls.Add(btResetMc);
            groupBox2.Controls.Add(label4);
            groupBox2.Location = new Point(657, 0);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(162, 89);
            groupBox2.TabIndex = 6;
            groupBox2.TabStop = false;
            groupBox2.Text = "Board Status";
            // 
            // button1
            // 
            button1.Location = new Point(5, 35);
            button1.Name = "button1";
            button1.Size = new Size(63, 23);
            button1.TabIndex = 11;
            button1.Text = "ServoOn";
            button1.UseVisualStyleBackColor = true;
            // 
            // rbOn
            // 
            rbOn.AutoSize = true;
            rbOn.Location = new Point(117, 38);
            rbOn.Name = "rbOn";
            rbOn.Size = new Size(41, 19);
            rbOn.TabIndex = 10;
            rbOn.Text = "On";
            rbOn.UseVisualStyleBackColor = true;
            // 
            // rbOff
            // 
            rbOff.AutoSize = true;
            rbOff.Checked = true;
            rbOff.Location = new Point(71, 38);
            rbOff.Name = "rbOff";
            rbOff.Size = new Size(42, 19);
            rbOff.TabIndex = 9;
            rbOff.TabStop = true;
            rbOff.Text = "Off";
            rbOff.UseVisualStyleBackColor = true;
            // 
            // tbError
            // 
            tbError.Location = new Point(87, 14);
            tbError.Name = "tbError";
            tbError.ReadOnly = true;
            tbError.Size = new Size(31, 23);
            tbError.TabIndex = 6;
            tbError.Text = "0";
            // 
            // btAlarmReset
            // 
            btAlarmReset.Location = new Point(5, 61);
            btAlarmReset.Name = "btAlarmReset";
            btAlarmReset.Size = new Size(79, 23);
            btAlarmReset.TabIndex = 5;
            btAlarmReset.Text = "Clear Alarm";
            btAlarmReset.UseVisualStyleBackColor = true;
            // 
            // btResetMc
            // 
            btResetMc.Location = new Point(90, 61);
            btResetMc.Name = "btResetMc";
            btResetMc.Size = new Size(66, 23);
            btResetMc.TabIndex = 2;
            btResetMc.Text = "Reset MC";
            btResetMc.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(17, 17);
            label4.Name = "label4";
            label4.Size = new Size(64, 15);
            label4.TabIndex = 0;
            label4.Text = "Error Code";
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(label5);
            groupBox3.Controls.Add(tbDriverType);
            groupBox3.Controls.Add(tbEnc);
            groupBox3.Controls.Add(btMcInit);
            groupBox3.Controls.Add(label3);
            groupBox3.Location = new Point(227, 0);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(174, 74);
            groupBox3.TabIndex = 7;
            groupBox3.TabStop = false;
            groupBox3.Text = "Initialize";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(106, 51);
            label5.Name = "label5";
            label5.Size = new Size(32, 15);
            label5.TabIndex = 8;
            label5.Text = "Type";
            // 
            // tbDriverType
            // 
            tbDriverType.Location = new Point(141, 47);
            tbDriverType.Name = "tbDriverType";
            tbDriverType.Size = new Size(25, 23);
            tbDriverType.TabIndex = 7;
            tbDriverType.Text = "3";
            // 
            // tbEnc
            // 
            tbEnc.Location = new Point(93, 18);
            tbEnc.Name = "tbEnc";
            tbEnc.ReadOnly = true;
            tbEnc.Size = new Size(75, 23);
            tbEnc.TabIndex = 6;
            tbEnc.Text = "0";
            // 
            // btMcInit
            // 
            btMcInit.Location = new Point(6, 47);
            btMcInit.Name = "btMcInit";
            btMcInit.Size = new Size(94, 23);
            btMcInit.TabIndex = 5;
            btMcInit.Text = "Init MC - No";
            btMcInit.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(38, 21);
            label3.Name = "label3";
            label3.Size = new Size(50, 15);
            label3.TabIndex = 0;
            label3.Text = "Encoder";
            // 
            // groupBox12
            // 
            groupBox12.Controls.Add(btStartStopFL);
            groupBox12.Controls.Add(tbLoosenAngle);
            groupBox12.Controls.Add(btFastenLoosen);
            groupBox12.Location = new Point(408, 0);
            groupBox12.Name = "groupBox12";
            groupBox12.Size = new Size(119, 74);
            groupBox12.TabIndex = 9;
            groupBox12.TabStop = false;
            groupBox12.Text = "Fasten/Loosen";
            // 
            // btStartStopFL
            // 
            btStartStopFL.Location = new Point(7, 19);
            btStartStopFL.Name = "btStartStopFL";
            btStartStopFL.Size = new Size(63, 23);
            btStartStopFL.TabIndex = 8;
            btStartStopFL.Text = "Start F/L";
            btStartStopFL.UseVisualStyleBackColor = true;
            // 
            // tbLoosenAngle
            // 
            tbLoosenAngle.Location = new Point(74, 47);
            tbLoosenAngle.Name = "tbLoosenAngle";
            tbLoosenAngle.Size = new Size(36, 23);
            tbLoosenAngle.TabIndex = 7;
            tbLoosenAngle.Text = "0";
            // 
            // btFastenLoosen
            // 
            btFastenLoosen.Location = new Point(6, 47);
            btFastenLoosen.Name = "btFastenLoosen";
            btFastenLoosen.Size = new Size(64, 23);
            btFastenLoosen.TabIndex = 5;
            btFastenLoosen.Text = "Loosen";
            btFastenLoosen.UseVisualStyleBackColor = true;
            // 
            // groupBox13
            // 
            groupBox13.Controls.Add(button2);
            groupBox13.Controls.Add(button3);
            groupBox13.Location = new Point(533, 0);
            groupBox13.Name = "groupBox13";
            groupBox13.Size = new Size(117, 74);
            groupBox13.TabIndex = 10;
            groupBox13.TabStop = false;
            groupBox13.Text = "Mode";
            // 
            // button2
            // 
            button2.Location = new Point(13, 19);
            button2.Name = "button2";
            button2.Size = new Size(93, 23);
            button2.TabIndex = 8;
            button2.Text = "MotorTest";
            button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            button3.Location = new Point(12, 47);
            button3.Name = "button3";
            button3.Size = new Size(94, 23);
            button3.TabIndex = 5;
            button3.Text = "Torque Runner";
            button3.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(826, 460);
            Controls.Add(groupBox13);
            Controls.Add(groupBox12);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(tabControl1);
            Name = "Form1";
            Text = "ServoTester3";
            Load += Form1_Load;
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            groupBox9.ResumeLayout(false);
            groupBox9.PerformLayout();
            groupBox11.ResumeLayout(false);
            groupBox11.PerformLayout();
            groupBox10.ResumeLayout(false);
            groupBox10.PerformLayout();
            groupBox8.ResumeLayout(false);
            groupBox7.ResumeLayout(false);
            groupBox7.PerformLayout();
            groupBox6.ResumeLayout(false);
            groupBox6.PerformLayout();
            groupBox5.ResumeLayout(false);
            groupBox5.PerformLayout();
            groupBox4.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox12.ResumeLayout(false);
            groupBox12.PerformLayout();
            groupBox13.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private GroupBox groupBox1;
        private Button btCommOpen;
        private ComboBox cbBaudrate;
        private Label label2;
        private Button btCommRefresh;
        private Label label1;
        private System.Windows.Forms.Timer workTimer;
        private GroupBox groupBox2;
        private TextBox tbError;
        private Button btAlarmReset;
        private Button btResetMc;
        private Label label4;
        private GroupBox groupBox3;
        private Label label5;
        private TextBox tbDriverType;
        private TextBox tbEnc;
        private Button btMcInit;
        private Label label3;
        private GroupBox groupBox5;
        private RadioButton rbCalibBackward;
        private RadioButton rbCalibForward;
        private RadioButton rbCalibHold;
        private RadioButton rbCalibNone;
        private GroupBox groupBox4;
        private Button btCalibStart;
        private GroupBox groupBox7;
        private TextBox tbTqOffsetValue;
        private Button btTqOffsetCheck;
        private Button btTqOffsetSave;
        private Label label6;
        private GroupBox groupBox6;
        private RadioButton rbCalibUserStop;
        private RadioButton rbCalibFail;
        private RadioButton rbCalibSuccess;
        private RadioButton rbCalibFinish;
        private TextBox tbTqSensorValue;
        private Label label7;
        private Button btCalibStop;
        private GroupBox groupBox8;
        private Button btStartOrigin;
        private Button btSaveOrigin;
        private GroupBox groupBox9;
        private GroupBox groupBox11;
        private RadioButton rbHardAutocustom;
        private RadioButton rbSoftAutocustom;
        private Button btSoftHardAutocustom;
        private GroupBox groupBox10;
        private RadioButton rbStartAutocustom;
        private RadioButton rbStopAutocustom;
        private Button btStartStopAutocustom;
        private TextBox tbTargetSpeed;
        private Label label10;
        private TextBox tbSeatingPoint;
        private Label label11;
        private TextBox tbFreeSpeed;
        private Label label8;
        private TextBox tbFreeAngle;
        private Label label9;
        private TabPage tabPage3;
        private RadioButton rbOn;
        private RadioButton rbOff;
        private ComboBox cbCommPorts;
        private GroupBox groupBox12;
        private Button btStartStopFL;
        private TextBox tbLoosenAngle;
        private Button btFastenLoosen;
        private ScottPlot.WinForms.FormsPlot formsPlot1;
        private Button button1;
        private GroupBox groupBox13;
        private Button button2;
        private Button button3;
    }
}
