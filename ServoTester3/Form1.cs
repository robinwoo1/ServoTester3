using ScottPlot;
using ScottPlot.Plottables;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
//tex:
//Formula 1: $$(a+b)^2 = a^2 + 2ab + b^2$$
//Formula 2: $$a^2-b^2 = (a+b)(a-b)$$

namespace ServoTester3
{
  public partial class Form1 : Form
  {
    public const ushort SERIAL_BUF_SIZE = 128 * 16;//128*8;
    
    public const byte ON = 1;
    public const byte OFF = 0;
    public const int _LengthLow = 2;
    public const int _LengthHigh = 3;
    private List<byte> _requestPacket;
    public byte DriverRun = 0;
    public byte CommandRun = 0;
    public byte DriverFL = 0;
    public byte CommandFL = 0;
    public bool closing_flag = false;
    Thread myThread;// = new Thread(myFunc);
    public bool myThread_flag = false;
    _Parameter Mc = new _Parameter();
    _Packet Packet = new _Packet();
    public Form1()
    {
      InitializeComponent();

    }
    private bool clear_graph_flag = false;
    // private int MotorState;
    private int time_tick;
    private bool timer_working = false;
    private bool port_working = false;
    
    // public ConcurrentQueue<byte> graph_cq = new ConcurrentQueue<byte>();
    
    public byte[] graph_ComReadBuffer = new byte[1024];
    
    
    
    public List<byte> SendByte { get; set; } = new List<byte>();
    [StructLayout(LayoutKind.Explicit)]
    struct TestUnion
    {
      [FieldOffset(0)] public float f;
      [FieldOffset(0)] public int i;
      [FieldOffset(0)] public uint u;
      [FieldOffset(0)] public ushort us0;
      [FieldOffset(2)] public ushort us1;
      [FieldOffset(0)] public short s0;
      [FieldOffset(2)] public short s1;
      [FieldOffset(0)] public byte b0;
      [FieldOffset(1)] public byte b1;
      [FieldOffset(2)] public byte b2;
      [FieldOffset(3)] public byte b3;
    }

    private void Form1_Load(object sender, EventArgs e)
    {
      // refresh port
      PortRefresh();
      // select baudrate
      cbBaudrate.SelectedIndex = 0;

      // InitAutoSetting();
      // InitMcFlag();
      // InitMcInfo();
      // InitSyncStruct();
      // InitInfo_DrvModel_para(4);//1);
      // InitDriverInfo(4);
      // InitParameter(4);
      // set event
      // Port.DataReceived += PortOnDataReceived;
      // Port.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
    }
    private void SpeedTorque_Click(object sender, EventArgs e)
    {
      if (sender == btSpeed)
      {
        nudSpeed.Enabled = true;
        nudTorque.Enabled = false;
        Packet.MakeAndSendData(8, 2, 0, ref Mc);
      }
      else// btTorque
      {
        nudSpeed.Enabled = false;
        nudTorque.Enabled = true;
        Packet.MakeAndSendData(8, 2, 1, ref Mc);
      }
    }
    private void ServoOn_Click(object sender, EventArgs e)
    {
      if (!Packet.Port.IsOpen)
        return;
      if (btServoOnOff.Text == "Servo On")
      {
        Packet.MakeAndSendData(8, 3, 1, ref Mc);
        Mc.Flag.b1Run = 1;
        btServoOnOff.Text = "Servo Off";
      }
      else
      {
        Packet.MakeAndSendData(8, 3, 0, ref Mc);
        Mc.Flag.b1Run = 0;
        btServoOnOff.Text = "Servo On";
      }
    }
    private void TestModeSelect_Click(object sender, EventArgs e)
    {
      if (!Packet.Port.IsOpen)
        return;

      if (sender == btMotorTest)
      {
        Packet.MakeAndSendData(8, 1, 1, ref Mc);
        btServoOnOff.Enabled = true;
        // gbServo.Visible = true;
        // gbFastenLoosen.Visible = false;
        rbMot.Checked = true;
      }
      else// (sender == btNutRunner)
      {
        Packet.MakeAndSendData(8, 1, 0, ref Mc);
        btServoOnOff.Enabled = false;
        // gbFastenLoosen.Visible = true;
        // gbServo.Visible = false;
        rbNut.Checked = true;
      }
    }
    private void btSaveOrigin_Click(object sender, EventArgs e)
    {
      Packet.MakeAndSendData(2, 3, 0, ref Mc);
    }
    private void btStartOrigin_Click(object sender, EventArgs e)
    {
      Packet.MakeAndSendData(2, 4, 0, ref Mc);
    }
    private void btResetMC_Click(object sender, EventArgs e)
    {
      Packet.MakeAndSendData(2, 5, 0, ref Mc);
    }
    private void btSoftHardAutocustom_Click(object sender, EventArgs e)
    {
      if (btSoftHardAutocustom.Text == "Soft")
      {
        Packet.MakeAndSendData(2, 8, 0, ref Mc);
        // btSoftHardAutocustom.Text = "Hard";
        // rbSoftAutocustom.Checked = true;
      }
      else//Hard
      {
        Packet.MakeAndSendData(2, 8, 1, ref Mc);
        // btSoftHardAutocustom.Text = "Soft";
        // rbHardAutocustom.Checked = true;
      }
    }
    private void btStartStopAutocustom_Click(object sender, EventArgs e)
    {
      if (btStartStopAutocustom.Text == "Start")
      {
        Packet.MakeAndSendData(2, 9, 1, ref Mc);
        // btStartStopAutocustom.Text = "Stop";
        // rbStartAutocustom.Checked = true;
      }
      else//Stop
      {
        Packet.MakeAndSendData(2, 9, 0, ref Mc);
        // btStartStopAutocustom.Text = "Start";
        // rbStopAutocustom.Checked = true;
      }
    }
    private void btCommRefresh_Click(object sender, EventArgs e)
    {
      //Refresh
      PortRefresh();
    }
    private void PortRefresh()
    {
      // clear
      cbCommPorts.Items.Clear();
      // get port list
      var ports = SerialPort.GetPortNames().OrderBy(x => x);
      // check ports
      foreach (var port in ports)
        // add port
        cbCommPorts.Items.Add(port);
      // check item count
      if (cbCommPorts.Items.Count > 0)
        // select first
        cbCommPorts.SelectedIndex = 0;
    }

    private void btCommOpen_Click(object sender, EventArgs e)
    {

      // check port
      switch (Packet.Port.IsOpen)
      {
        case false when btCommOpen.Text == @"Open":
          // get port and baudrate
          var port = cbCommPorts.Text;
          var baudrate = Convert.ToInt32(cbBaudrate.Text);
          // check port
          if (string.IsNullOrWhiteSpace(port))
            break;
          // try catch
          try
          {
            //clear Port
            //Port.DiscardOutBuffer();
            //Port.DiscardInBuffer();

            Packet.ComReadIndex = 0;
            RecvBuf.tail = 0;
            RecvBuf.head = 0;
            // set port
            Packet.Port.PortName = port;
            Packet.Port.BaudRate = baudrate;
            Packet.Port.Encoding = Encoding.GetEncoding(28591);
            // open
            Packet.Port.Open();
            // InitAutoSetting();
            // InitMcFlag();
            // InitMcInfo();
            // InitSyncStruct();
            // InitInfo_DrvModel_para(4);//1);
            // InitDriverInfo(4);
            // InitParameter(4);
            // set event
            // Packet.Port.DataReceived += PortOnDataReceived;
            Packet.Port.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);

            // start timer
            workTimer.Start();
            // change button text
            btCommOpen.Text = @"Close";

            myThread_flag = true;
            myThread = new Thread(myFunc);
            myThread.Start();
          }
          catch (Exception ex)
          {
            // debug
            Debug.WriteLine(ex.Message);
            // error
            MessageBox.Show($@"{port} isn't enable to open.");
          }
          break;
        case true when btCommOpen.Text == @"Close":
          // try catch
          try
          {
            // close
            while (port_working) { }
            //clear Port
            Packet.Port.DiscardOutBuffer();
            Packet.Port.DiscardInBuffer();
            Packet.Port.Close();
            // stop timer
            while (timer_working) { }
            workTimer.Stop();
            // change button text
            btCommOpen.Text = @"Open";
            Packet.Port.DataReceived -= new SerialDataReceivedEventHandler(DataReceivedHandler);

            myThread_flag = false;
          }
          catch (Exception ex)
          {
            // debug
            Debug.WriteLine(ex.Message);
            // error
            MessageBox.Show(@"Port closing error.");
          }

          break;
      }
    }
    private void myFunc()
    {
      byte data;

      while (myThread_flag)
      {
        // if (graph_cq.Count>0)
        // {
        //   // for (int i=0;i<800;i++)
        //   // {
        //   //   graph_cq.TryDequeue(out data);
        //   //   graph_ComReadBuffer[i]=data;
        //   // }
        //   // this.Invoke(new Action(delegate() // this == Form 이다. Form이 아닌 컨트롤의 Invoke를 직접호출해도 무방하다.
        //   //           {
        //   //               //Invoke를 통해 lbl_Result 컨트롤에 결과값을 업데이트한다.
        //   //               // lbl_Result.Text = result.ToString();
        //   //               Packet.fresh_graph_data(ref Mc);
        //   //           }));
        //   // Packet.fresh_graph_data(ref Mc);
        //   // graph_count++;
        // }
        Packet.ProcessPcMcReceivedCommData(ref Mc);
        Thread.Sleep(50);
      }
    }
    // private void btMotor_Click(object sender, EventArgs e)
    // {
    //     // var list = new List<byte>();
    //     // check sender
    //     if (btRunStop.Text == @"Servo On")
    //     {
    //         Packet.MakeAndSendData(106, 1, 1, ref Mc);
    //         btRunStop.Text = @"Servo Off";
    //     }
    //     else
    //     {
    //         Packet.MakeAndSendData(106, 1, 0, ref Mc);
    //         btRunStop.Text = @"Servo On";
    //     }

    //     // // check port is open
    //     // if (Packet.Port.IsOpen && list.Count > 0)
    //     //   // write packet
    //     //   Packet.Port.Write(list.ToArray(), 0, list.Count);
    // }
    private void btStartStopFL_Click(object sender, EventArgs e)
    {
      if (btStartStopFL.Text == "StartFL")
      {
        if (tbLoosenAngle.Text == "" || tbLoosenAngle.Text == "0")
          Mc.Flag.LoosenAngle = 0;
        else
        {
          try
          {
            Mc.Flag.LoosenAngle = Int16.Parse(tbLoosenAngle.Text);
          }
          catch (FormatException)
          {
            Mc.Flag.LoosenAngle = 0;
          }
        }
        Packet.MakeAndSendData(2, 2, 1, ref Mc);
      }
      else
      {
        
        Packet.MakeAndSendData(2, 2, 0, ref Mc);
      }
    }
    private void btFastenLoosen_Click(object sender, EventArgs e)
    {
      clear_graph_flag = true;
      if (btFastenLoosen.Text == "Fasten")
      {
        Mc.Flag.b1ControlFL = 0;
        Packet.MakeAndSendData(2, 1, 0, ref Mc);
      }
      else//Loosen
      {
        Mc.Flag.b1ControlFL = 1;
        Packet.MakeAndSendData(2, 1, 1, ref Mc);
      }
    }
    private void btMcInit_Click(object sender, EventArgs e)
    {
      if (!Packet.Port.IsOpen)
        return;
      ushort DriverType = (ushort)Int16.Parse(tbDriverType.Text);
      Mc.InitInfo_DrvModel_para(DriverType);//1);
      Mc.InitDriverInfo(DriverType);
      Mc.InitParameter(DriverType);
      Packet.MakeAndSendData(2, 10, 0, ref Mc);
      Mc.Var.IniStep = 0;
    }
    private void btTqOffset_Click(object sender, EventArgs e)
    {

    }
    private void btSetTqOffset_Click(object sender, EventArgs e)
    {
      if (!Packet.Port.IsOpen)
        return;

      if (sender == btSetTqOffset)
      {
        Mc.outDriverInfo.f32TorqueOffset = (float)Double.Parse(tbTqOffsetValue.Text);
        Packet.MakeAndSendData(7, 6, 0, ref Mc);
        btSetTqOffset.Enabled = true;
      }
    }
    private void btGetTqOffset_Click(object sender, EventArgs e)
    {
      if (!Packet.Port.IsOpen)
        return;

      if (sender == btGetTqOffset)
      {
        Packet.MakeAndSendData(7, 7, 0, ref Mc);
        btGetTqOffset.Enabled = true;
      }
    }
    private void btAlarmReset_Click(object sender, EventArgs e)
    {
      if (!Packet.Port.IsOpen)
        return;
      Packet.MakeAndSendData(2, 6, 0, ref Mc);
    }
    private void btCalibrationCommand_Click(object sender, EventArgs e)
    {
      if (!Packet.Port.IsOpen)
        return;
      if (sender == btCalibStart)
      {
        Packet.MakeAndSendData(7, 12, 1, ref Mc);
      }
      else
      {
        Packet.MakeAndSendData(7, 12, 0, ref Mc);
      }
    }

    // private void Set_ValueChanged(object sender, EventArgs e)
    // {

    // }
    private void Set_ValueChanged(object sender, EventArgs e)
    {
      Control control = null;
      // check sender
      if (sender is ComboBox box)
        control = box;
      // set control
      else if (sender is NumericUpDown down)
        control = down;

      // check control
      if (control == null)
        return;
      // packet
      var packet = new List<byte>();
      // get addr
      var addr = Convert.ToUInt16(control.Tag);
      // check tag
      switch (addr)
      {
        case 1:
          Mc.Gain.Speed = (short)(Convert.ToInt32(((NumericUpDown)control).Value) / 10);
          Packet.MakeAndSendData(9, addr, Mc.Gain.Speed, ref Mc);
          break;
        case 2:
          Mc.Gain.Torque = Convert.ToInt16(((NumericUpDown)control).Value);
          Packet.MakeAndSendData(9, addr, Mc.Gain.Torque, ref Mc);
          break;
        case 3:
          Mc.Gain.Tq_Kp = Convert.ToUInt16(((NumericUpDown)control).Value);//(ushort)UInt16.Parse(tbTorquePgain.Text);
          Packet.MakeAndSendData(9, addr, (short)Mc.Gain.Tq_Kp, ref Mc);
          break;
        case 4:
          Mc.Gain.Tq_Ki = Convert.ToUInt16(((NumericUpDown)control).Value);//(ushort)UInt16.Parse(tbTorqueIgain.Text);
          Packet.MakeAndSendData(9, addr, (short)Mc.Gain.Tq_Ki, ref Mc);
          break;
        case 5:
          Mc.Gain.Tq_Kf = Convert.ToUInt16(((NumericUpDown)control).Value);//(ushort)UInt16.Parse(tbTorqueFFgain.Text);
          Packet.MakeAndSendData(9, addr, (short)Mc.Gain.Tq_Kf, ref Mc);
          break;
        case 6:
          Mc.Gain.Sp_Kp = Convert.ToUInt16(((NumericUpDown)control).Value);//(ushort)UInt16.Parse(tbSpeedPgain.Text);
          Packet.MakeAndSendData(9, addr, (short)Mc.Gain.Sp_Kp, ref Mc);
          break;
        case 7:
          Mc.Gain.Sp_Ki = Convert.ToUInt16(((NumericUpDown)control).Value);//(ushort)UInt16.Parse(tbSpeedIgain.Text);
          Packet.MakeAndSendData(9, addr, (short)Mc.Gain.Sp_Ki, ref Mc);
          break;
        case 8:
          Mc.Gain.Sp_Kf = Convert.ToUInt16(((NumericUpDown)control).Value);//(ushort)UInt16.Parse(tbSpeedFFgain.Text);
          Packet.MakeAndSendData(9, addr, (short)Mc.Gain.Sp_Kf, ref Mc);
          break;
          // add range
          // packet.AddRange(GetPacket(addr, Convert.ToInt32(((ComboBox)control).SelectedIndex)));
          // Packet.MakeAndSendData(9, addr, Convert.ToInt16(((ComboBox)control).SelectedIndex), ref Mc);
          // Packet.MakeAndSendData(9, addr, Convert.ToInt16(((NumericUpDown)control).Value), ref Mc);
          // break;
          // case 9:
          // case 10:
          // case 11:
          // case 12:
          // case 13:
          // case 14:
          // case 15:
          // case 16:
          // case 17:
          //     // add range
          //     // packet.AddRange(GetPacket(addr, Convert.ToInt32(((NumericUpDown)control).Value)));
          //     Packet.MakeAndSendData(106, addr, Convert.ToInt16(((NumericUpDown)control).Value), ref Mc);
          //     break;
      }
      // // check port is open
      // if (Packet.Port.IsOpen && packet.Count > 0)
      //     // write packet
      //     Packet.Port.Write(packet.ToArray(), 0, packet.Count);

      // // debug
      // foreach (var b in packet)
      // {
      //     Debug.Write($@"{b:X2} ");
      // }
      // Debug.WriteLine(string.Empty);
    }
    private void btnSetAllGain_Click(object sender, EventArgs e)
    {
      Packet.MakeAndSendData(9, 9, 0, ref Mc);
    }
    private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
    {
      try
      {
        if (Packet.Port.IsOpen)
        {
          port_working = true;
          // this.Invoke(new EventHandler(MySerialReceived));//
          byte[] data = Packet.Port.Encoding.GetBytes(Packet.Port.ReadExisting());
          // rbuf_put(data, (ushort)(data.Count()));
          // Packet.cq.CopyTo(data, data.Count());
          for (int i = 0; i < data.Count(); i++)
          {
            Packet.cq.Enqueue(data[i]);
          }
          port_working = false;
        }
      }
      finally
      {
        //Packet.Port.Close();
      }

    }

    // private void MySerialReceived(object s, EventArgs e)  //
    // {
    //   try
    //   {
    //     byte[] data = Packet.Port.Encoding.GetBytes(Packet.Port.ReadExisting());
    //     // rbuf_put(data, (ushort)(data.Count()));
    //     // cq.CopyTo(data, data.Count());
    //     for (int i=0;i<data.Count();i++)
    //     {
    //       cq.Enqueue(data[i]);
    //     }
    //     // Packet.ProcessPcMcReceivedCommData(ref Mc);
    //   }
    //   finally
    //   {

    //   }
    // }
    private void workTimer_Tick(object sender, EventArgs e)
    {
      timer_working = true;
      time_tick++;
      tbTimeTickMessage.Text = time_tick.ToString();

      tbTargetSpeed.Text = Mc.AutoSetting.CurrentSpeed.ToString();
      tbSeatingPoint.Text = Mc.AutoSetting.CurrentSeatingPoint.ToString();
      tbFreeSpeed.Text = Mc.AutoSetting.CurrentFSpeed.ToString();
      tbFreeAngle.Text = Mc.AutoSetting.CurrentFAngle.ToString();

      tbTqSensorValue.Text = Mc.Var.TqSensorValue.ToString();
      tbTqSensorOffsetValue.Text = Mc.Var.TqSensorOffsetValue.ToString();
      tbError.Text = Mc.Var.Error.ToString();
      tbMaintCnt.Text = Mc.Var.MaintCnt.ToString();
      tbEnc.Text = Mc.Var.Enc.ToString();

      // tbDataCount.Text = Data_ch1.Count.ToString();
      tbDataCount.Text = Mc.Var.graph_count.ToString();
      tbGraphDataCount.Text = Packet.Graph_ch1.Count.ToString();
      // tbGraphDataCount.Text = Mc.Var.graph_count.ToString();//Packet.Graph_ch1.Count.ToString();

      if (Mc.Var.refresh_graph_flag)
      {
        Mc.Var.refresh_graph_flag = false;
        Refresh_graph();
      }

      if (Mc.Var.DriverInfoIsReady)
      {
        ShowDriverInfo();
      }

      if (Mc.Var.DriverInfo_TorqueOffsetIsReady)
      {
        ShowDriverInfo_TorqueOffset();
      }

      switch (Mc.AutoSetting.FlagSetting)
      {
        case true when !rbSoftAutocustom.Checked:
          btSoftHardAutocustom.Text = "Hard";
          rbSoftAutocustom.Checked = true;
          break;
        case false when !rbHardAutocustom.Checked:
          btSoftHardAutocustom.Text = "Soft";
          rbHardAutocustom.Checked = true;
          break;
      }
      switch (Mc.AutoSetting.FlagStart)
      {
        case true when !rbStartAutocustom.Checked:
          btStartStopAutocustom.Text = "Stop";
          rbStartAutocustom.Checked = true;
          break;
        case false when !rbStopAutocustom.Checked:
          btStartStopAutocustom.Text = "Start";
          rbStopAutocustom.Checked = true;
          break;
      }
      // check motor state
      switch (Mc.Var.MotorState)
      {
        // change off
        case false when !rbOff.Checked:
          rbOff.Checked = true;
          break;
        case true when !rbOn.Checked:
          rbOn.Checked = true;
          break;
      }
      // check Calibration Step state
      switch (Mc.Var.CalibStepState)
      {
        // none
        case 0 when !rbCalibNone.Checked:
          rbCalibNone.Checked = true;
          break;
        // hold
        case 1 when !rbCalibHold.Checked:
          rbCalibHold.Checked = true;
          break;
        // forward
        case 2 when !rbCalibForward.Checked:
          rbCalibForward.Checked = true;
          break;
        // backward
        case 3 when !rbCalibBackward.Checked:
          rbCalibBackward.Checked = true;
          break;
        // finish
        case 4 when !rbCalibFinish.Checked:
          rbCalibFinish.Checked = true;
          break;
      }
      // check Calibration Result state
      switch (Mc.Var.CalibResultState)
      {
        // success
        case 0 when !rbCalibSuccess.Checked:
          rbCalibSuccess.Checked = true;
          break;
        // fail
        case 1 when !rbCalibFail.Checked:
          rbCalibFail.Checked = true;
          break;
        // user stop
        case 2 when !rbCalibUserStop.Checked:
          rbCalibUserStop.Checked = true;
          break;
      }
      if (Mc.Flag.b1ControlFL != 0)
      {
        btFastenLoosen.Text = "Fasten";
        tbLoosenAngle.Enabled = true;
      }
      else
      {
        btFastenLoosen.Text = "Loosen";
        tbLoosenAngle.Enabled = false;
      }
      if (Mc.Flag.b1Run != 0)
      {
        btStartStopFL.Text = "StopFL";
      }
      else
      {
        btStartStopFL.Text = "StartFL";
      }
      if (Mc.Var.Mot_or_Nut)
      {
        rbMot.Checked = true;
        rbNut.Checked = false;
        gbFastenLoosen.Visible = false;
        gbServo.Visible = true;
      }
      else
      {
        rbMot.Checked = false;
        rbNut.Checked = true;
        gbServo.Visible = false;
        gbFastenLoosen.Visible = true;
      }
      if (Mc.Var.Mcinitialized != 0)
      {
        btMcInit.Text = @"Init MC - Yes";
      }
      else
      {
        btMcInit.Text = @"Init MC - No";
      }
      if (Mc.Flag.b1Run == 0)
      {
        btServoOnOff.Text = "Servo On";
      }
      else
      {
        btServoOnOff.Text = "Servo Off";
      }
      timer_working = false;
    }

    private static IEnumerable<byte> GetCrc(IEnumerable<byte> packet)
    {
      var crc = new byte[] { 0xFF, 0xFF };
      ushort crcFull = 0xFFFF;
      // check total packet
      foreach (var data in packet)
      {
        // XOR 1 byte
        crcFull = (ushort)(crcFull ^ data);
        // cyclic redundancy check
        for (var j = 0; j < 8; j++)
        {
          // get LSB
          var lsb = (ushort)(crcFull & 0x0001);
          // check AND
          crcFull = (ushort)((crcFull >> 1) & 0x7FFF);
          // check LSB
          if (lsb == 0x01)
            // XOR
            crcFull = (ushort)(crcFull ^ 0xA001);
        }
      }

      // set CRC
      crc[1] = (byte)((crcFull >> 8) & 0xFF);
      crc[0] = (byte)(crcFull & 0xFF);

      return crc;
    }

    private static IEnumerable<byte> GetPacket(ushort addr, int value)
    {
      var list = new List<byte>();
      // add data
      list.Add(0x01);
      list.Add(0x06);
      list.Add((byte)((addr >> 8) & 0xFF));
      list.Add((byte)(addr & 0xFF));
      list.Add((byte)((value >> 8) & 0xFF));
      list.Add((byte)(value & 0xFF));
      // get crc
      var crc = GetCrc(list);
      // add crc
      list.AddRange(crc);
      // return
      return list;
    }
    public struct RecvBuf_
    {
      public ushort head;
      public ushort tail;
      public byte[] data;
      public RecvBuf_(int num)
      {
        this.head = 0;
        this.tail = 0;
        this.data = new byte[num];
      }
    }
    RecvBuf_ RecvBuf = new RecvBuf_(SERIAL_BUF_SIZE);

    //[Obsolete]
    public void Refresh_graph()
    {
      // tbDataCount.Text = Data_ch1.Count.ToString();
      // tbGraphDataCount.Text = Packet.Graph_ch1.Count.ToString();
      List<double> Graph_time = new List<double>();
      // Graph_time.Clear();
      for (int i = 0; i < Packet.Graph_ch1.Count; i++)
        Graph_time.Add(5e-3d * (double)i);

      formsPlot1.Plot.Clear();
      if (cbGraph_ch1.Checked)
      {
        var sig1 = formsPlot1.Plot.Add.ScatterLine(Graph_time, Packet.Graph_ch1);
        sig1.LegendText = "Torque";
      }
      if (cbGraph_ch2.Checked)
      {
        var sig2 = formsPlot1.Plot.Add.ScatterLine(Graph_time, Packet.Graph_ch2);
        sig2.LegendText = "Current";
      }
      if (cbGraph_ch3.Checked)
      {
        var sig3 = formsPlot1.Plot.Add.ScatterLine(Graph_time, Packet.Graph_ch3);
        sig3.LegendText = "Speed";
      }
      if (cbGraph_ch4.Checked)
      {
        var sig4 = formsPlot1.Plot.Add.ScatterLine(Graph_time, Packet.Graph_ch4);
        sig4.LegendText = "Angle";
      }
      if (cbGraph_ch5.Checked)
      {
        var sig5 = formsPlot1.Plot.Add.ScatterLine(Graph_time, Packet.Graph_ch5);
        sig5.LegendText = "Speed Command";
      }
      if (cbGraph_ch6.Checked)
      {
        var sig6 = formsPlot1.Plot.Add.ScatterLine(Graph_time, Packet.Graph_ch6);
        sig6.LegendText = "Current Command";
      }
      if (cbGraph_ch7.Checked)
      {
        var sig7 = formsPlot1.Plot.Add.ScatterLine(Graph_time, Packet.Graph_ch7);
        sig7.LegendText = "SnugAngle";
      }

      formsPlot1.Plot.ShowLegend(Alignment.UpperRight);

      formsPlot1.Plot.Axes.AutoScale();

      var vl = formsPlot1.Plot.Add.VerticalLine(0);
      vl.IsDraggable = true;
      vl.Text = $"{vl.X:0.00}";//"VLine";

      var hl = formsPlot1.Plot.Add.HorizontalLine(0);
      hl.IsDraggable = true;
      hl.Text = $"{hl.Y:0.00}";//"HLine";

      formsPlot1.Refresh();

      // use events for custom mouse interactivity
      //formsPlot1.MouseDown += FormsPlot1_MouseDown;
      //formsPlot1.MouseUp += FormsPlot1_MouseUp;
      //formsPlot1.MouseMove += FormsPlot1_MouseMove;
    }
    private void btnSaveGraph_Click(object sender, EventArgs e)
    {
      // string FileName = "";
      SaveFileDialog saveFile = new SaveFileDialog();
      saveFile.Title = "Save an Text File";
      saveFile.FileName = "GraphData";
      saveFile.DefaultExt = "txt";
      saveFile.Filter = "txt file(*.txt)|*.txt";
      if (saveFile.ShowDialog() == DialogResult.OK)
      {
        if (saveFile.FileName != "")
        {
          StreamWriter sw = new StreamWriter(saveFile.FileName);
          sw.WriteLine(Packet.Graph_ch1.Count());
          for (int i = 0; i < Packet.Graph_ch1.Count; i++)
          {
            sw.WriteLine(Packet.Graph_ch1[i].ToString());
            sw.WriteLine(Packet.Graph_ch2[i].ToString());
            sw.WriteLine(Packet.Graph_ch3[i].ToString());
            sw.WriteLine(Packet.Graph_ch4[i].ToString());
            sw.WriteLine(Packet.Graph_ch5[i].ToString());
            sw.WriteLine(Packet.Graph_ch6[i].ToString());
            sw.WriteLine(Packet.Graph_ch7[i].ToString());
          }
          sw.Close();
        }
        else
        {
          StreamWriter sw = new StreamWriter("GraphData.txt");
          sw.WriteLine(Packet.Graph_ch1.Count());
          for (int i = 0; i < Packet.Graph_ch1.Count; i++)
          {
            sw.WriteLine(Packet.Graph_ch1[i].ToString());
            sw.WriteLine(Packet.Graph_ch2[i].ToString());
            sw.WriteLine(Packet.Graph_ch3[i].ToString());
            sw.WriteLine(Packet.Graph_ch4[i].ToString());
            sw.WriteLine(Packet.Graph_ch5[i].ToString());
            sw.WriteLine(Packet.Graph_ch6[i].ToString());
            sw.WriteLine(Packet.Graph_ch7[i].ToString());
          }
          sw.Close();
        }
      }
    }
    private void btnLoadGraph_Click(object sender, EventArgs e)
    {
      // string FileName = "";
      OpenFileDialog loadFile = new OpenFileDialog();
      loadFile.Title = "Load an Text File";
      loadFile.FileName = "GraphData";
      loadFile.DefaultExt = "txt";
      loadFile.Filter = "txt file(*.txt)|*.txt";

      if (loadFile.ShowDialog() == DialogResult.OK)
      {
        if (loadFile.FileName != "")
        {
          StreamReader sr = new StreamReader(loadFile.FileName);
          int Count = Convert.ToInt32(sr.ReadLine());
          Packet.clear_graph_data();
          for (int i = 0; i < Count; i++)
          {
            Packet.Graph_ch1.Add(Convert.ToDouble(sr.ReadLine()));
            Packet.Graph_ch2.Add(Convert.ToDouble(sr.ReadLine()));
            Packet.Graph_ch3.Add(Convert.ToDouble(sr.ReadLine()));
            Packet.Graph_ch4.Add(Convert.ToDouble(sr.ReadLine()));
            Packet.Graph_ch5.Add(Convert.ToDouble(sr.ReadLine()));
            Packet.Graph_ch6.Add(Convert.ToDouble(sr.ReadLine()));
            Packet.Graph_ch7.Add(Convert.ToDouble(sr.ReadLine()));
          }
          sr.Close();
        }
        else
        {
          StreamReader sr = new StreamReader("GraphData.txt");
          int Count = Convert.ToInt32(sr.ReadLine());
          Packet.clear_graph_data();
          for (int i = 0; i < Count; i++)
          {
            Packet.Graph_ch1.Add(Convert.ToDouble(sr.ReadLine()));
            Packet.Graph_ch2.Add(Convert.ToDouble(sr.ReadLine()));
            Packet.Graph_ch3.Add(Convert.ToDouble(sr.ReadLine()));
            Packet.Graph_ch4.Add(Convert.ToDouble(sr.ReadLine()));
            Packet.Graph_ch5.Add(Convert.ToDouble(sr.ReadLine()));
            Packet.Graph_ch6.Add(Convert.ToDouble(sr.ReadLine()));
            Packet.Graph_ch7.Add(Convert.ToDouble(sr.ReadLine()));
          }
          sr.Close();
        }
        // Mc.Var.refresh_graph_flag = true;
        Refresh_graph();
      }
    }
    AxisLine? PlottableBeingDragged_Line = null;
    // SignalXY? PlottableBeingDragged_XY = null;
    // DataPoint StartingDragPosition = DataPoint.None;
    // double StartingDragOffset = 0;
    // Marker HighlightedPointMarker;
    private void FormsPlot1_MouseDown(object? sender, MouseEventArgs e)
    {
      var lineUnderMouse = GetLineUnderMouse(e.X, e.Y);
      if (lineUnderMouse is not null)
      {
        PlottableBeingDragged_Line = lineUnderMouse;
        formsPlot1.Interaction.Disable(); // disable panning while dragging
      }
    }
    private void FormsPlot1_MouseUp(object? sender, MouseEventArgs e)
    {
      // PlottableBeingDragged_XY = null;
      // StartingDragPosition = DataPoint.None;

      PlottableBeingDragged_Line = null;
      formsPlot1.Interaction.Enable(); // enable panning again
      formsPlot1.Refresh();
    }
    private void FormsPlot1_MouseMove(object? sender, MouseEventArgs e)
    {
      // this rectangle is the area around the mouse in coordinate units
      CoordinateRect rect = formsPlot1.Plot.GetCoordinateRect(e.X, e.Y, radius: 10);

      if (PlottableBeingDragged_Line is null)
      {
        // set cursor based on what's beneath the plottable
        var lineUnderMouse = GetLineUnderMouse(e.X, e.Y);
        if (lineUnderMouse is null) Cursor = Cursors.Default;
        else if (lineUnderMouse.IsDraggable && lineUnderMouse is VerticalLine) Cursor = Cursors.SizeWE;
        else if (lineUnderMouse.IsDraggable && lineUnderMouse is HorizontalLine) Cursor = Cursors.SizeNS;
      }
      else
      {
        // update the position of the plottable being dragged
        if (PlottableBeingDragged_Line is HorizontalLine hl)
        {
          hl.Y = rect.VerticalCenter;
          hl.Text = $"{hl.Y:0.00}";
        }
        else if (PlottableBeingDragged_Line is VerticalLine vl)
        {
          vl.X = rect.HorizontalCenter;
          vl.Text = $"{vl.X:0.00}";
        }
        formsPlot1.Refresh();
      }
    }
    private AxisLine? GetLineUnderMouse(float x, float y)
    {
      CoordinateRect rect = formsPlot1.Plot.GetCoordinateRect(x, y, radius: 10);

      foreach (AxisLine axLine in formsPlot1.Plot.GetPlottables<AxisLine>().Reverse())
      {
        if (axLine.IsUnderMouse(rect))
          return axLine;
      }

      return null;
    }
    private static (SignalXY? signalXY, DataPoint point) GetSignalXYUnderMouse(Plot plot, double x, double y)
    {
      Pixel mousePixel = new(x, y);

      Coordinates mouseLocation = plot.GetCoordinates(mousePixel);

      foreach (SignalXY signal in plot.GetPlottables<SignalXY>().Reverse())
      {
        DataPoint nearest = signal.Data.GetNearest(mouseLocation, plot.LastRender);
        if (nearest.IsReal)
        {
          return (signal, nearest);
        }
      }

      return (null, DataPoint.None);
    }
    private void cbGraph_CheckedChanged(object sender, EventArgs e)
    {
      Refresh_graph();
    }

    private void ServoFormClosed(object sender, FormClosedEventArgs e)
    {
      myThread_flag = false;
    }

    private void SetDriverInfo(object sender, EventArgs e)
    {
      if (!Packet.Port.IsOpen)
        return;
      Mc.outDriverInfo.u16Type = (ushort)UInt16.Parse(nudDriverType.Text);
      Mc.outDriverInfo.u16Version = (ushort)UInt16.Parse(nudDriverVersion.Text);
      Mc.outDriverInfo.u8Factory_Gear_efficiency = (ushort)UInt16.Parse(nudDriverGearEfficiency.Text);
      Mc.outDriverInfo.u8User_Gear_efficiency = (ushort)UInt16.Parse(nudDriverUserEfficiency.Text);
      uint SerialNum = UInt32.Parse(nudDriverSerial.Text);
      Mc.outDriverInfo.u16Serial_low = (ushort)(SerialNum >> 0);
      Mc.outDriverInfo.u16Serial_high = (ushort)(SerialNum >> 16);
      Mc.outDriverInfo.u16DriverVendor = (ushort)UInt16.Parse(nudDriverVendor.Text);
      Packet.MakeAndSendData(7, 1, 0, ref Mc);
    }

    private void GetDriverInfo(object sender, EventArgs e)
    {
      Packet.MakeAndSendData(7, 5, 0, ref Mc);
    }
    private void ShowDriverInfo()
    {
      nudDriverType.Text = Mc.DriverInfo.u16Type.ToString();
      nudDriverVersion.Text = Mc.DriverInfo.u16Version.ToString();
      nudDriverGearEfficiency.Text = Mc.DriverInfo.u8Factory_Gear_efficiency.ToString();
      nudDriverUserEfficiency.Text = Mc.DriverInfo.u8User_Gear_efficiency.ToString();
      uint SerialNum = (uint)((Mc.DriverInfo.u16Serial_high << 16) + Mc.DriverInfo.u16Serial_low);
      nudDriverSerial.Text = SerialNum.ToString();
      nudDriverVendor.Text = Mc.DriverInfo.u16DriverVendor.ToString();
      Mc.Var.DriverInfoIsReady = false;
    }
    private void ShowDriverInfo_TorqueOffset()
    {
      tbTqOffsetValue.Text = Mc.DriverInfo.f32TorqueOffset.ToString();
      Mc.Var.DriverInfo_TorqueOffsetIsReady = false;
    }

    private void rbSoftStopOff_CheckedChanged(object sender, EventArgs e)
    {
      Mc.Var.SoftStop = 0;
    }

    private void rbSoftStopOn_CheckedChanged(object sender, EventArgs e)
    {
      Mc.Var.SoftStop = 1;
    }

    private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    private void btTqSensorOffset_Click(object sender, EventArgs e)
    {
      if (!Packet.Port.IsOpen)
        return;

      if (sender == btCheckTqSensorOffset)
      {
        Packet.MakeAndSendData(7, 10, 0, ref Mc);
        btSaveTqSensorOffset.Enabled = true;
      }
      else if (sender == btSaveTqSensorOffset)
      {
        Packet.MakeAndSendData(7, 11, 0, ref Mc);
      }
    }

    private void gbServo_Enter(object sender, EventArgs e)
    {

    }
  }
}
