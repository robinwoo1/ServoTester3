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
      tbMaintCnt = new TextBox();
      label12 = new Label();
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
      cbGraph_ch7 = new CheckBox();
      cbGraph_ch6 = new CheckBox();
      cbGraph_ch5 = new CheckBox();
      cbGraph_ch4 = new CheckBox();
      cbGraph_ch3 = new CheckBox();
      cbGraph_ch2 = new CheckBox();
      cbGraph_ch1 = new CheckBox();
      formsPlot1 = new ScottPlot.WinForms.FormsPlot();
      tabPage3 = new TabPage();
      tbSpeedFFgain = new NumericUpDown();
      tbSpeedIgain = new NumericUpDown();
      label22 = new Label();
      label23 = new Label();
      tbSpeedPgain = new NumericUpDown();
      tbTorqueFFgain = new NumericUpDown();
      label20 = new Label();
      label21 = new Label();
      tbTorqueIgain = new NumericUpDown();
      tbTorquePgain = new NumericUpDown();
      label18 = new Label();
      label19 = new Label();
      groupBox1 = new GroupBox();
      btCommOpen = new Button();
      cbBaudrate = new ComboBox();
      label2 = new Label();
      btCommRefresh = new Button();
      cbCommPorts = new ComboBox();
      label1 = new Label();
      workTimer = new System.Windows.Forms.Timer(components);
      groupBox2 = new GroupBox();
      btServoOnOff = new Button();
      rbOn = new RadioButton();
      rbOff = new RadioButton();
      tbError = new TextBox();
      btAlarmReset = new Button();
      btResetMc = new Button();
      label4 = new Label();
      groupBox3 = new GroupBox();
      tbDriverType = new NumericUpDown();
      label5 = new Label();
      tbEnc = new TextBox();
      btMcInit = new Button();
      label3 = new Label();
      gbFastenLoosen = new GroupBox();
      tbLoosenAngle = new NumericUpDown();
      btStartStopFL = new Button();
      btFastenLoosen = new Button();
      gbServo = new GroupBox();
      nudSpeed = new NumericUpDown();
      nudTorque = new NumericUpDown();
      btSpeed = new Button();
      btTorque = new Button();
      groupBox13 = new GroupBox();
      rbNut = new RadioButton();
      rbMot = new RadioButton();
      btMotorTest = new Button();
      btNutRunner = new Button();
      label13 = new Label();
      tbTimeTickMessage = new TextBox();
      tbDataCount = new TextBox();
      tbGraphDataCount = new TextBox();
      gbGain = new GroupBox();
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
      ((System.ComponentModel.ISupportInitialize)tbSpeedFFgain).BeginInit();
      ((System.ComponentModel.ISupportInitialize)tbSpeedIgain).BeginInit();
      ((System.ComponentModel.ISupportInitialize)tbSpeedPgain).BeginInit();
      ((System.ComponentModel.ISupportInitialize)tbTorqueFFgain).BeginInit();
      ((System.ComponentModel.ISupportInitialize)tbTorqueIgain).BeginInit();
      ((System.ComponentModel.ISupportInitialize)tbTorquePgain).BeginInit();
      groupBox1.SuspendLayout();
      groupBox2.SuspendLayout();
      groupBox3.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)tbDriverType).BeginInit();
      gbFastenLoosen.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)tbLoosenAngle).BeginInit();
      gbServo.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)nudSpeed).BeginInit();
      ((System.ComponentModel.ISupportInitialize)nudTorque).BeginInit();
      groupBox13.SuspendLayout();
      gbGain.SuspendLayout();
      SuspendLayout();
      // 
      // tabControl1
      // 
      tabControl1.Controls.Add(tabPage1);
      tabControl1.Controls.Add(tabPage2);
      tabControl1.Controls.Add(tabPage3);
      tabControl1.Location = new Point(0, 150);
      tabControl1.Name = "tabControl1";
      tabControl1.SelectedIndex = 0;
      tabControl1.Size = new Size(878, 392);
      tabControl1.TabIndex = 0;
      // 
      // tabPage1
      // 
      tabPage1.Controls.Add(groupBox9);
      tabPage1.Controls.Add(tbMaintCnt);
      tabPage1.Controls.Add(label12);
      tabPage1.Controls.Add(groupBox8);
      tabPage1.Controls.Add(groupBox7);
      tabPage1.Controls.Add(groupBox6);
      tabPage1.Controls.Add(groupBox5);
      tabPage1.Controls.Add(groupBox4);
      tabPage1.Location = new Point(4, 24);
      tabPage1.Name = "tabPage1";
      tabPage1.Padding = new Padding(3);
      tabPage1.Size = new Size(870, 364);
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
      btSoftHardAutocustom.Click += btSoftHardAutocustom_Click;
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
      btStartStopAutocustom.Click += btStartStopAutocustom_Click;
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
      // tbMaintCnt
      // 
      tbMaintCnt.Location = new Point(650, 16);
      tbMaintCnt.Name = "tbMaintCnt";
      tbMaintCnt.ReadOnly = true;
      tbMaintCnt.Size = new Size(67, 23);
      tbMaintCnt.TabIndex = 9;
      tbMaintCnt.Text = "0";
      // 
      // label12
      // 
      label12.AutoSize = true;
      label12.Location = new Point(587, 20);
      label12.Name = "label12";
      label12.Size = new Size(57, 15);
      label12.TabIndex = 12;
      label12.Text = "Main Cnt";
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
      btStartOrigin.Click += btStartOrigin_Click;
      // 
      // btSaveOrigin
      // 
      btSaveOrigin.Location = new Point(15, 18);
      btSaveOrigin.Name = "btSaveOrigin";
      btSaveOrigin.Size = new Size(84, 23);
      btSaveOrigin.TabIndex = 0;
      btSaveOrigin.Text = "Save Origin";
      btSaveOrigin.UseVisualStyleBackColor = true;
      btSaveOrigin.Click += btSaveOrigin_Click;
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
      btTqOffsetCheck.Click += btTqOffset_Click;
      // 
      // btTqOffsetSave
      // 
      btTqOffsetSave.Location = new Point(101, 18);
      btTqOffsetSave.Name = "btTqOffsetSave";
      btTqOffsetSave.Size = new Size(92, 23);
      btTqOffsetSave.TabIndex = 2;
      btTqOffsetSave.Text = "Save Offset";
      btTqOffsetSave.UseVisualStyleBackColor = true;
      btTqOffsetSave.Click += btTqOffset_Click;
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
      groupBox6.Location = new Point(8, 100);
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
      rbCalibNone.ForeColor = SystemColors.ControlText;
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
      btCalibStop.Click += btCalibrationCommand_Click;
      // 
      // btCalibStart
      // 
      btCalibStart.Location = new Point(15, 18);
      btCalibStart.Name = "btCalibStart";
      btCalibStart.Size = new Size(75, 23);
      btCalibStart.TabIndex = 0;
      btCalibStart.Text = "Start";
      btCalibStart.UseVisualStyleBackColor = true;
      btCalibStart.Click += btCalibrationCommand_Click;
      // 
      // tabPage2
      // 
      tabPage2.Controls.Add(cbGraph_ch7);
      tabPage2.Controls.Add(cbGraph_ch6);
      tabPage2.Controls.Add(cbGraph_ch5);
      tabPage2.Controls.Add(cbGraph_ch4);
      tabPage2.Controls.Add(cbGraph_ch3);
      tabPage2.Controls.Add(cbGraph_ch2);
      tabPage2.Controls.Add(cbGraph_ch1);
      tabPage2.Controls.Add(formsPlot1);
      tabPage2.Location = new Point(4, 24);
      tabPage2.Name = "tabPage2";
      tabPage2.Padding = new Padding(3);
      tabPage2.Size = new Size(870, 364);
      tabPage2.TabIndex = 1;
      tabPage2.Text = "Graph";
      tabPage2.UseVisualStyleBackColor = true;
      // 
      // cbGraph_ch7
      // 
      cbGraph_ch7.AutoSize = true;
      cbGraph_ch7.Location = new Point(6, 227);
      cbGraph_ch7.Name = "cbGraph_ch7";
      cbGraph_ch7.Size = new Size(85, 19);
      cbGraph_ch7.TabIndex = 7;
      cbGraph_ch7.Text = "SnugAngle";
      cbGraph_ch7.UseVisualStyleBackColor = true;
      cbGraph_ch7.CheckedChanged += cbGraph_CheckedChanged;
      // 
      // cbGraph_ch6
      // 
      cbGraph_ch6.AutoSize = true;
      cbGraph_ch6.Location = new Point(6, 202);
      cbGraph_ch6.Name = "cbGraph_ch6";
      cbGraph_ch6.Size = new Size(92, 19);
      cbGraph_ch6.TabIndex = 6;
      cbGraph_ch6.Text = "CurrentCmd";
      cbGraph_ch6.UseVisualStyleBackColor = true;
      cbGraph_ch6.CheckedChanged += cbGraph_CheckedChanged;
      // 
      // cbGraph_ch5
      // 
      cbGraph_ch5.AutoSize = true;
      cbGraph_ch5.Location = new Point(6, 177);
      cbGraph_ch5.Name = "cbGraph_ch5";
      cbGraph_ch5.Size = new Size(77, 19);
      cbGraph_ch5.TabIndex = 5;
      cbGraph_ch5.Text = "RPMCmd";
      cbGraph_ch5.UseVisualStyleBackColor = true;
      cbGraph_ch5.CheckedChanged += cbGraph_CheckedChanged;
      // 
      // cbGraph_ch4
      // 
      cbGraph_ch4.AutoSize = true;
      cbGraph_ch4.Location = new Point(6, 152);
      cbGraph_ch4.Name = "cbGraph_ch4";
      cbGraph_ch4.Size = new Size(57, 19);
      cbGraph_ch4.TabIndex = 4;
      cbGraph_ch4.Text = "Angle";
      cbGraph_ch4.UseVisualStyleBackColor = true;
      cbGraph_ch4.CheckedChanged += cbGraph_CheckedChanged;
      // 
      // cbGraph_ch3
      // 
      cbGraph_ch3.AutoSize = true;
      cbGraph_ch3.Location = new Point(6, 127);
      cbGraph_ch3.Name = "cbGraph_ch3";
      cbGraph_ch3.Size = new Size(51, 19);
      cbGraph_ch3.TabIndex = 3;
      cbGraph_ch3.Text = "RPM";
      cbGraph_ch3.UseVisualStyleBackColor = true;
      cbGraph_ch3.CheckedChanged += cbGraph_CheckedChanged;
      // 
      // cbGraph_ch2
      // 
      cbGraph_ch2.AutoSize = true;
      cbGraph_ch2.Location = new Point(6, 102);
      cbGraph_ch2.Name = "cbGraph_ch2";
      cbGraph_ch2.Size = new Size(66, 19);
      cbGraph_ch2.TabIndex = 2;
      cbGraph_ch2.Text = "Current";
      cbGraph_ch2.UseVisualStyleBackColor = true;
      cbGraph_ch2.CheckedChanged += cbGraph_CheckedChanged;
      // 
      // cbGraph_ch1
      // 
      cbGraph_ch1.AutoSize = true;
      cbGraph_ch1.Location = new Point(6, 77);
      cbGraph_ch1.Name = "cbGraph_ch1";
      cbGraph_ch1.Size = new Size(63, 19);
      cbGraph_ch1.TabIndex = 1;
      cbGraph_ch1.Text = "Torque";
      cbGraph_ch1.UseVisualStyleBackColor = true;
      cbGraph_ch1.CheckedChanged += cbGraph_CheckedChanged;
      // 
      // formsPlot1
      // 
      formsPlot1.DisplayScale = 1F;
      formsPlot1.Location = new Point(99, 3);
      formsPlot1.Name = "formsPlot1";
      formsPlot1.Size = new Size(748, 338);
      formsPlot1.TabIndex = 0;
      // 
      // tabPage3
      // 
      tabPage3.Location = new Point(4, 24);
      tabPage3.Name = "tabPage3";
      tabPage3.Padding = new Padding(3);
      tabPage3.Size = new Size(870, 364);
      tabPage3.TabIndex = 2;
      tabPage3.Text = "Servo";
      tabPage3.UseVisualStyleBackColor = true;
      // 
      // tbSpeedFFgain
      // 
      tbSpeedFFgain.Location = new Point(219, 69);
      tbSpeedFFgain.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
      tbSpeedFFgain.Name = "tbSpeedFFgain";
      tbSpeedFFgain.Size = new Size(69, 23);
      tbSpeedFFgain.TabIndex = 20;
      tbSpeedFFgain.Tag = "8";
      tbSpeedFFgain.ValueChanged += Set_ValueChanged;
      // 
      // tbSpeedIgain
      // 
      tbSpeedIgain.Location = new Point(219, 43);
      tbSpeedIgain.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
      tbSpeedIgain.Name = "tbSpeedIgain";
      tbSpeedIgain.Size = new Size(69, 23);
      tbSpeedIgain.TabIndex = 19;
      tbSpeedIgain.Tag = "7";
      tbSpeedIgain.ValueChanged += Set_ValueChanged;
      // 
      // label22
      // 
      label22.AutoSize = true;
      label22.Location = new Point(157, 45);
      label22.Name = "label22";
      label22.Size = new Size(54, 15);
      label22.TabIndex = 18;
      label22.Text = "Speed Ki";
      // 
      // label23
      // 
      label23.AutoSize = true;
      label23.Location = new Point(157, 71);
      label23.Name = "label23";
      label23.Size = new Size(55, 15);
      label23.TabIndex = 17;
      label23.Text = "Speed Kf";
      // 
      // tbSpeedPgain
      // 
      tbSpeedPgain.Location = new Point(219, 17);
      tbSpeedPgain.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
      tbSpeedPgain.Name = "tbSpeedPgain";
      tbSpeedPgain.Size = new Size(69, 23);
      tbSpeedPgain.TabIndex = 16;
      tbSpeedPgain.Tag = "6";
      tbSpeedPgain.ValueChanged += Set_ValueChanged;
      // 
      // tbTorqueFFgain
      // 
      tbTorqueFFgain.Location = new Point(76, 69);
      tbTorqueFFgain.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
      tbTorqueFFgain.Name = "tbTorqueFFgain";
      tbTorqueFFgain.Size = new Size(69, 23);
      tbTorqueFFgain.TabIndex = 15;
      tbTorqueFFgain.Tag = "5";
      tbTorqueFFgain.ValueChanged += Set_ValueChanged;
      // 
      // label20
      // 
      label20.AutoSize = true;
      label20.Location = new Point(10, 71);
      label20.Name = "label20";
      label20.Size = new Size(59, 15);
      label20.TabIndex = 14;
      label20.Text = "Torque Kf";
      // 
      // label21
      // 
      label21.AutoSize = true;
      label21.Location = new Point(158, 19);
      label21.Name = "label21";
      label21.Size = new Size(58, 15);
      label21.TabIndex = 13;
      label21.Text = "Speed Kp";
      // 
      // tbTorqueIgain
      // 
      tbTorqueIgain.Location = new Point(76, 43);
      tbTorqueIgain.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
      tbTorqueIgain.Name = "tbTorqueIgain";
      tbTorqueIgain.Size = new Size(69, 23);
      tbTorqueIgain.TabIndex = 12;
      tbTorqueIgain.Tag = "4";
      tbTorqueIgain.ValueChanged += Set_ValueChanged;
      // 
      // tbTorquePgain
      // 
      tbTorquePgain.Location = new Point(76, 17);
      tbTorquePgain.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
      tbTorquePgain.Name = "tbTorquePgain";
      tbTorquePgain.Size = new Size(69, 23);
      tbTorquePgain.TabIndex = 11;
      tbTorquePgain.Tag = "3";
      tbTorquePgain.ValueChanged += Set_ValueChanged;
      // 
      // label18
      // 
      label18.AutoSize = true;
      label18.Location = new Point(11, 19);
      label18.Name = "label18";
      label18.Size = new Size(62, 15);
      label18.TabIndex = 10;
      label18.Text = "Torque Kp";
      // 
      // label19
      // 
      label19.AutoSize = true;
      label19.Location = new Point(10, 45);
      label19.Name = "label19";
      label19.Size = new Size(58, 15);
      label19.TabIndex = 9;
      label19.Text = "Torque Ki";
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
      groupBox1.Size = new Size(221, 69);
      groupBox1.TabIndex = 1;
      groupBox1.TabStop = false;
      groupBox1.Text = "CommPort";
      // 
      // btCommOpen
      // 
      btCommOpen.Location = new Point(140, 42);
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
      cbBaudrate.Items.AddRange(new object[] { "1152000", "921600", "576000", "460800", "230400", "115200", "57600", "38400", "19200", "9600" });
      cbBaudrate.Location = new Point(62, 42);
      cbBaudrate.Name = "cbBaudrate";
      cbBaudrate.Size = new Size(73, 23);
      cbBaudrate.TabIndex = 4;
      // 
      // label2
      // 
      label2.AutoSize = true;
      label2.Location = new Point(4, 45);
      label2.Name = "label2";
      label2.Size = new Size(54, 15);
      label2.TabIndex = 3;
      label2.Text = "Baudrate";
      // 
      // btCommRefresh
      // 
      btCommRefresh.Location = new Point(140, 15);
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
      cbCommPorts.Location = new Point(61, 15);
      cbCommPorts.Name = "cbCommPorts";
      cbCommPorts.Size = new Size(73, 23);
      cbCommPorts.TabIndex = 1;
      // 
      // label1
      // 
      label1.AutoSize = true;
      label1.Location = new Point(13, 17);
      label1.Name = "label1";
      label1.Size = new Size(29, 15);
      label1.TabIndex = 0;
      label1.Text = "Port";
      // 
      // workTimer
      // 
      workTimer.Interval = 50;
      workTimer.Tick += workTimer_Tick;
      // 
      // groupBox2
      // 
      groupBox2.Controls.Add(btServoOnOff);
      groupBox2.Controls.Add(rbOn);
      groupBox2.Controls.Add(rbOff);
      groupBox2.Controls.Add(tbError);
      groupBox2.Controls.Add(btAlarmReset);
      groupBox2.Controls.Add(btResetMc);
      groupBox2.Controls.Add(label4);
      groupBox2.Location = new Point(707, 0);
      groupBox2.Name = "groupBox2";
      groupBox2.Size = new Size(162, 89);
      groupBox2.TabIndex = 6;
      groupBox2.TabStop = false;
      groupBox2.Text = "Board Status";
      // 
      // btServoOnOff
      // 
      btServoOnOff.Enabled = false;
      btServoOnOff.Location = new Point(4, 36);
      btServoOnOff.Name = "btServoOnOff";
      btServoOnOff.Size = new Size(65, 23);
      btServoOnOff.TabIndex = 12;
      btServoOnOff.Tag = "1";
      btServoOnOff.Text = "Servo On";
      btServoOnOff.UseVisualStyleBackColor = true;
      btServoOnOff.Click += ServoOn_Click;
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
      btAlarmReset.Location = new Point(4, 61);
      btAlarmReset.Name = "btAlarmReset";
      btAlarmReset.Size = new Size(79, 23);
      btAlarmReset.TabIndex = 5;
      btAlarmReset.Text = "Clear Alarm";
      btAlarmReset.UseVisualStyleBackColor = true;
      btAlarmReset.Click += btAlarmReset_Click;
      // 
      // btResetMc
      // 
      btResetMc.Location = new Point(90, 61);
      btResetMc.Name = "btResetMc";
      btResetMc.Size = new Size(66, 23);
      btResetMc.TabIndex = 2;
      btResetMc.Text = "Reset MC";
      btResetMc.UseVisualStyleBackColor = true;
      btResetMc.Click += btResetMC_Click;
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
      groupBox3.Controls.Add(tbDriverType);
      groupBox3.Controls.Add(label5);
      groupBox3.Controls.Add(tbEnc);
      groupBox3.Controls.Add(btMcInit);
      groupBox3.Controls.Add(label3);
      groupBox3.Location = new Point(227, 0);
      groupBox3.Name = "groupBox3";
      groupBox3.Size = new Size(174, 69);
      groupBox3.TabIndex = 7;
      groupBox3.TabStop = false;
      groupBox3.Text = "Initialize";
      // 
      // tbDriverType
      // 
      tbDriverType.Location = new Point(136, 42);
      tbDriverType.Name = "tbDriverType";
      tbDriverType.Size = new Size(35, 23);
      tbDriverType.TabIndex = 9;
      tbDriverType.Value = new decimal(new int[] { 3, 0, 0, 0 });
      // 
      // label5
      // 
      label5.AutoSize = true;
      label5.Location = new Point(104, 45);
      label5.Name = "label5";
      label5.Size = new Size(32, 15);
      label5.TabIndex = 8;
      label5.Text = "Type";
      // 
      // tbEnc
      // 
      tbEnc.Location = new Point(93, 15);
      tbEnc.Name = "tbEnc";
      tbEnc.ReadOnly = true;
      tbEnc.Size = new Size(75, 23);
      tbEnc.TabIndex = 6;
      tbEnc.Text = "0";
      // 
      // btMcInit
      // 
      btMcInit.Location = new Point(6, 43);
      btMcInit.Name = "btMcInit";
      btMcInit.Size = new Size(94, 23);
      btMcInit.TabIndex = 5;
      btMcInit.Text = "Init MC - No";
      btMcInit.UseVisualStyleBackColor = true;
      btMcInit.Click += btMcInit_Click;
      // 
      // label3
      // 
      label3.AutoSize = true;
      label3.Location = new Point(38, 18);
      label3.Name = "label3";
      label3.Size = new Size(50, 15);
      label3.TabIndex = 0;
      label3.Text = "Encoder";
      // 
      // gbFastenLoosen
      // 
      gbFastenLoosen.Controls.Add(tbLoosenAngle);
      gbFastenLoosen.Controls.Add(btStartStopFL);
      gbFastenLoosen.Controls.Add(btFastenLoosen);
      gbFastenLoosen.Location = new Point(557, 0);
      gbFastenLoosen.Name = "gbFastenLoosen";
      gbFastenLoosen.Size = new Size(144, 71);
      gbFastenLoosen.TabIndex = 9;
      gbFastenLoosen.TabStop = false;
      gbFastenLoosen.Text = "Fasten/Loosen";
      // 
      // tbLoosenAngle
      // 
      tbLoosenAngle.Location = new Point(66, 41);
      tbLoosenAngle.Name = "tbLoosenAngle";
      tbLoosenAngle.Size = new Size(68, 23);
      tbLoosenAngle.TabIndex = 13;
      // 
      // btStartStopFL
      // 
      btStartStopFL.Location = new Point(7, 16);
      btStartStopFL.Name = "btStartStopFL";
      btStartStopFL.Size = new Size(63, 22);
      btStartStopFL.TabIndex = 8;
      btStartStopFL.Text = "Start F/L";
      btStartStopFL.UseVisualStyleBackColor = true;
      btStartStopFL.Click += btStartStopFL_Click;
      // 
      // btFastenLoosen
      // 
      btFastenLoosen.Location = new Point(6, 41);
      btFastenLoosen.Name = "btFastenLoosen";
      btFastenLoosen.Size = new Size(53, 23);
      btFastenLoosen.TabIndex = 5;
      btFastenLoosen.Text = "Loosen";
      btFastenLoosen.UseVisualStyleBackColor = true;
      btFastenLoosen.Click += btFastenLoosen_Click;
      // 
      // gbServo
      // 
      gbServo.Controls.Add(nudSpeed);
      gbServo.Controls.Add(nudTorque);
      gbServo.Controls.Add(btSpeed);
      gbServo.Controls.Add(btTorque);
      gbServo.Location = new Point(557, 1);
      gbServo.Name = "gbServo";
      gbServo.Size = new Size(144, 70);
      gbServo.TabIndex = 14;
      gbServo.TabStop = false;
      gbServo.Text = "Servo";
      // 
      // nudSpeed
      // 
      nudSpeed.Enabled = false;
      nudSpeed.Location = new Point(67, 19);
      nudSpeed.Maximum = new decimal(new int[] { 30000, 0, 0, 0 });
      nudSpeed.Minimum = new decimal(new int[] { 30000, 0, 0, int.MinValue });
      nudSpeed.Name = "nudSpeed";
      nudSpeed.Size = new Size(68, 23);
      nudSpeed.TabIndex = 14;
      nudSpeed.Tag = "1";
      nudSpeed.ValueChanged += Set_ValueChanged;
      // 
      // nudTorque
      // 
      nudTorque.Enabled = false;
      nudTorque.Location = new Point(66, 44);
      nudTorque.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
      nudTorque.Minimum = new decimal(new int[] { 10000, 0, 0, int.MinValue });
      nudTorque.Name = "nudTorque";
      nudTorque.Size = new Size(68, 23);
      nudTorque.TabIndex = 13;
      nudTorque.Tag = "2";
      nudTorque.ValueChanged += Set_ValueChanged;
      // 
      // btSpeed
      // 
      btSpeed.Location = new Point(7, 16);
      btSpeed.Name = "btSpeed";
      btSpeed.Size = new Size(52, 22);
      btSpeed.TabIndex = 8;
      btSpeed.Text = "Speed";
      btSpeed.UseVisualStyleBackColor = true;
      btSpeed.Click += SpeedTorque_Click;
      // 
      // btTorque
      // 
      btTorque.Location = new Point(6, 41);
      btTorque.Name = "btTorque";
      btTorque.Size = new Size(53, 23);
      btTorque.TabIndex = 5;
      btTorque.Text = "Torque";
      btTorque.UseVisualStyleBackColor = true;
      btTorque.Click += SpeedTorque_Click;
      // 
      // groupBox13
      // 
      groupBox13.Controls.Add(rbNut);
      groupBox13.Controls.Add(rbMot);
      groupBox13.Controls.Add(btMotorTest);
      groupBox13.Controls.Add(btNutRunner);
      groupBox13.Location = new Point(407, 0);
      groupBox13.Name = "groupBox13";
      groupBox13.Size = new Size(140, 69);
      groupBox13.TabIndex = 10;
      groupBox13.TabStop = false;
      groupBox13.Text = "Mode";
      // 
      // rbNut
      // 
      rbNut.AutoSize = true;
      rbNut.Checked = true;
      rbNut.Location = new Point(87, 42);
      rbNut.Name = "rbNut";
      rbNut.Size = new Size(45, 19);
      rbNut.TabIndex = 16;
      rbNut.TabStop = true;
      rbNut.Text = "Nut";
      rbNut.UseVisualStyleBackColor = true;
      // 
      // rbMot
      // 
      rbMot.AutoSize = true;
      rbMot.Location = new Point(87, 18);
      rbMot.Name = "rbMot";
      rbMot.Size = new Size(47, 19);
      rbMot.TabIndex = 15;
      rbMot.TabStop = true;
      rbMot.Text = "Mot";
      rbMot.UseVisualStyleBackColor = true;
      // 
      // btMotorTest
      // 
      btMotorTest.Location = new Point(5, 16);
      btMotorTest.Name = "btMotorTest";
      btMotorTest.Size = new Size(78, 23);
      btMotorTest.TabIndex = 8;
      btMotorTest.Text = "MotorTest";
      btMotorTest.UseVisualStyleBackColor = true;
      btMotorTest.Click += TestModeSelect_Click;
      // 
      // btNutRunner
      // 
      btNutRunner.Location = new Point(4, 40);
      btNutRunner.Name = "btNutRunner";
      btNutRunner.Size = new Size(79, 23);
      btNutRunner.TabIndex = 5;
      btNutRunner.Text = "Nut Runner";
      btNutRunner.UseVisualStyleBackColor = true;
      btNutRunner.Click += TestModeSelect_Click;
      // 
      // label13
      // 
      label13.AutoSize = true;
      label13.Location = new Point(12, 96);
      label13.Name = "label13";
      label13.Size = new Size(58, 15);
      label13.TabIndex = 14;
      label13.Text = "Time Tick";
      // 
      // tbTimeTickMessage
      // 
      tbTimeTickMessage.Location = new Point(72, 92);
      tbTimeTickMessage.Name = "tbTimeTickMessage";
      tbTimeTickMessage.ReadOnly = true;
      tbTimeTickMessage.Size = new Size(67, 23);
      tbTimeTickMessage.TabIndex = 13;
      tbTimeTickMessage.Text = "0";
      // 
      // tbDataCount
      // 
      tbDataCount.Location = new Point(151, 92);
      tbDataCount.Name = "tbDataCount";
      tbDataCount.ReadOnly = true;
      tbDataCount.Size = new Size(75, 23);
      tbDataCount.TabIndex = 11;
      tbDataCount.Text = "0";
      // 
      // tbGraphDataCount
      // 
      tbGraphDataCount.Location = new Point(230, 92);
      tbGraphDataCount.Name = "tbGraphDataCount";
      tbGraphDataCount.ReadOnly = true;
      tbGraphDataCount.Size = new Size(75, 23);
      tbGraphDataCount.TabIndex = 13;
      tbGraphDataCount.Text = "0";
      // 
      // gbGain
      // 
      gbGain.Controls.Add(tbSpeedFFgain);
      gbGain.Controls.Add(label18);
      gbGain.Controls.Add(tbSpeedIgain);
      gbGain.Controls.Add(label19);
      gbGain.Controls.Add(label22);
      gbGain.Controls.Add(tbTorquePgain);
      gbGain.Controls.Add(label23);
      gbGain.Controls.Add(tbTorqueIgain);
      gbGain.Controls.Add(tbSpeedPgain);
      gbGain.Controls.Add(label20);
      gbGain.Controls.Add(label21);
      gbGain.Controls.Add(tbTorqueFFgain);
      gbGain.Location = new Point(407, 70);
      gbGain.Name = "gbGain";
      gbGain.Size = new Size(295, 98);
      gbGain.TabIndex = 15;
      gbGain.TabStop = false;
      gbGain.Text = "Gain";
      // 
      // Form1
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(877, 544);
      Controls.Add(gbGain);
      Controls.Add(gbServo);
      Controls.Add(tbGraphDataCount);
      Controls.Add(tbDataCount);
      Controls.Add(label13);
      Controls.Add(tbTimeTickMessage);
      Controls.Add(groupBox13);
      Controls.Add(gbFastenLoosen);
      Controls.Add(groupBox3);
      Controls.Add(groupBox2);
      Controls.Add(groupBox1);
      Controls.Add(tabControl1);
      Name = "Form1";
      Text = "ServoTester3";
      FormClosed += ServoFormClosed;
      Load += Form1_Load;
      Click += Form1_Load;
      tabControl1.ResumeLayout(false);
      tabPage1.ResumeLayout(false);
      tabPage1.PerformLayout();
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
      tabPage2.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)tbSpeedFFgain).EndInit();
      ((System.ComponentModel.ISupportInitialize)tbSpeedIgain).EndInit();
      ((System.ComponentModel.ISupportInitialize)tbSpeedPgain).EndInit();
      ((System.ComponentModel.ISupportInitialize)tbTorqueFFgain).EndInit();
      ((System.ComponentModel.ISupportInitialize)tbTorqueIgain).EndInit();
      ((System.ComponentModel.ISupportInitialize)tbTorquePgain).EndInit();
      groupBox1.ResumeLayout(false);
      groupBox1.PerformLayout();
      groupBox2.ResumeLayout(false);
      groupBox2.PerformLayout();
      groupBox3.ResumeLayout(false);
      groupBox3.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)tbDriverType).EndInit();
      gbFastenLoosen.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)tbLoosenAngle).EndInit();
      gbServo.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)nudSpeed).EndInit();
      ((System.ComponentModel.ISupportInitialize)nudTorque).EndInit();
      groupBox13.ResumeLayout(false);
      groupBox13.PerformLayout();
      gbGain.ResumeLayout(false);
      gbGain.PerformLayout();
      ResumeLayout(false);
      PerformLayout();
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
        private GroupBox gbFastenLoosen;
        private Button btStartStopFL;
        private Button btFastenLoosen;
        private ScottPlot.WinForms.FormsPlot formsPlot1;
        private GroupBox groupBox13;
        private Button btMotorTest;
        private Button btNutRunner;
        private Label label12;
        private TextBox tbMaintCnt;
        private Label label13;
        private TextBox tbTimeTickMessage;
        private NumericUpDown tbDriverType;
        private NumericUpDown tbLoosenAngle;
        private NumericUpDown tbSpeedFFgain;
        private NumericUpDown tbSpeedIgain;
        private Label label22;
        private Label label23;
        private NumericUpDown tbSpeedPgain;
        private NumericUpDown tbTorqueFFgain;
        private Label label20;
        private Label label21;
        private NumericUpDown tbTorqueIgain;
        private NumericUpDown tbTorquePgain;
        private Label label18;
        private Label label19;
        private TextBox tbDataCount;
        private TextBox tbGraphDataCount;
        private CheckBox cbGraph_ch7;
        private CheckBox cbGraph_ch6;
        private CheckBox cbGraph_ch5;
        private CheckBox cbGraph_ch4;
        private CheckBox cbGraph_ch3;
        private CheckBox cbGraph_ch2;
        private CheckBox cbGraph_ch1;
        private RadioButton rbNut;
        private RadioButton rbMot;
        private Button btServoOnOff;
        private GroupBox gbServo;
        private NumericUpDown nudTorque;
        private Button btSpeed;
        private Button btTorque;
        private NumericUpDown nudSpeed;
        private GroupBox gbGain;
    }
}
