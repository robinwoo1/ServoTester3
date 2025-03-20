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
    public const ushort COMMAND_LIST_NUM = 1000;
    public const byte ON = 1;
    public const byte OFF = 0;
    public const int _LengthLow = 2;
    public const int _LengthHigh = 3;
    private List<byte> _requestPacket;
    public byte[] SendDataPacket = new byte[SERIAL_BUF_SIZE];
    public byte[] FlagRun = new byte[10];
    public byte DriverRun = 0;
    public byte CommandRun = 0;
    public byte[] FlagFL = new byte[10];
    public byte DriverFL = 0;
    public byte CommandFL = 0;
    public bool closing_flag = false;
    public bool Mot_or_Nut = true;//false;
    Thread myThread;// = new Thread(myFunc);
    public bool myThread_flag = false;
    public int graph_count = 0;
    ushort TqSensorValue = 0;
    ushort TqSensorOffsetValue = 0;
    ushort Error = 0;
    uint MaintCnt = 0;
    ushort Enc = 0;
    ushort Mcinitialized = 0;
    public Form1()
    {
      InitializeComponent();

    }
    // List<double> Graph_time = new List<double>();
    List<double> Graph_ch1 = new List<double>();
    List<double> Graph_ch2 = new List<double>();
    List<double> Graph_ch3 = new List<double>();
    List<double> Graph_ch4 = new List<double>();
    List<double> Graph_ch5 = new List<double>();
    List<double> Graph_ch6 = new List<double>();
    List<double> Graph_ch7 = new List<double>();
    List<double> Graph_ch8 = new List<double>();
    List<double> Data_ch1 = new List<double>();
    List<double> Data_ch2 = new List<double>();
    List<double> Data_ch3 = new List<double>();
    List<double> Data_ch4 = new List<double>();
    List<double> Data_ch5 = new List<double>();
    List<double> Data_ch6 = new List<double>();
    List<double> Data_ch7 = new List<double>();
    List<double> Data_ch8 = new List<double>();



    private bool refresh_graph_flag = false;
    private bool clear_graph_flag = false;
    private SerialPort Port { get; } = new SerialPort();
    private bool MotorState { get; set; }
    // private int MotorState;
    private int CalibStepState;// { CALIB_SUCCESS, CALIB_FAIL, CALIB_USERSTOP }
    private int CalibResultState;// { get; set; }
    private int time_tick;
    private bool timer_working = false;
    private bool port_working = false;
    public ConcurrentQueue<byte> cq = new ConcurrentQueue<byte>();
    // public ConcurrentQueue<byte> graph_cq = new ConcurrentQueue<byte>();
    public byte[] ComReadBuffer = new byte[128 * 16 * 8];
    public byte[] graph_ComReadBuffer = new byte[1024];
    public int ComReadIndex = 0;
    public ushort Command_Index_Pc;
    public ushort[,] Command_List_Pc = new ushort[COMMAND_LIST_NUM, 3];
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
    
    // public byte IniStep = 0;
    // public bool DriverInfoIsReady = false;
    // public byte SoftStop = 0;
    public struct McVar_
    {
      public byte IniStep;// = 0;
      public bool DriverInfoIsReady;// = false;
      public byte SoftStop;// = 0;
      public float TqOffsetValue;
      public ushort TorquePgain;
      public ushort TorqueIgain;
      public ushort TorqueFFgain;
      public ushort SpeedPgain;
      public ushort SpeedIgain;
      public ushort SpeedFFgain;
      public McVar_()
      {
        this.IniStep = 0;
        this.DriverInfoIsReady = false;
        this.SoftStop = 0;
        this.TqOffsetValue = 0;
      }
    }
    Parameter Mc = new Parameter();
    Packet_ Packet = new Packet_();
    McVar_ McVar = new McVar_();

    public void SendPacket(byte[] Packet, ushort Cnt)
    {
      try
      {
        if (Port.IsOpen && Cnt > 0)
          Port.Write(Packet, 0, Cnt);
      }
      finally
      {

      }
    }
    public void MakeAndSendData(byte Command, ushort StartAddress, short Data)
    {
      ushort u16PtrCnt = 0;
      ushort calc_crc = 0;

      if (Command == 1)
      {
        if (StartAddress == 1 || StartAddress == 2 || StartAddress == 3)
        {
          //MakePacket(Command, StartAddress, Data);
          Packet.make(Command, StartAddress, Data, ref SendDataPacket, ref Mc, ref McVar);
          u16PtrCnt = Mc.CmdAck.u16PtrCnt;
          calc_crc = GetCRC(SendDataPacket, u16PtrCnt + 2);
          SendDataPacket[u16PtrCnt++] = (byte)(calc_crc >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(calc_crc >> 8);
          // Port.Write(SendDataPacket, 0, u16PtrCnt);
          SendPacket(SendDataPacket, u16PtrCnt);
        }
        // else if (StartAddress == 4)
      }
      else if (Command == 2)
      {
        if (StartAddress == 1 || StartAddress == 2 || StartAddress == 3 || StartAddress == 4 || StartAddress == 5 ||
            StartAddress == 6 || StartAddress == 7 || StartAddress == 8 || StartAddress == 9 || StartAddress == 10)
        {
          string input = String.Empty;
          if (tbLoosenAngle.Text == "" || tbLoosenAngle.Text == "0")
            Mc.McFlag.LoosenAngle = 0;
          else
          {
            try
            {
              Mc.McFlag.LoosenAngle = Int16.Parse(tbLoosenAngle.Text);
              // Console.WriteLine(result);
            }
            catch (FormatException)
            {
              // Console.WriteLine($"Unable to parse '{input}'");
              Mc.McFlag.LoosenAngle = 0;
            }
          }
          // MakePacket(Command, StartAddress, Data);
          Packet.make(Command, StartAddress, Data, ref SendDataPacket, ref Mc, ref McVar);
          u16PtrCnt = Mc.CmdAck.u16PtrCnt;
          calc_crc = GetCRC(SendDataPacket, u16PtrCnt + 2);
          SendDataPacket[u16PtrCnt++] = (byte)(calc_crc >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(calc_crc >> 8);
          // Port.Write(SendDataPacket, 0, u16PtrCnt);
          SendPacket(SendDataPacket, u16PtrCnt);
        }
        // else if (StartAddress == 11)
      }
      // else if (Command == 3) // cyclic PC<-MC
      // else if (Command == 4) // praph PC<-MC
      // else if (Command == 5) // event PC<-MC
      else if (Command == 6)
      {
        if (StartAddress == 1)// Sync setting
        {
          //MakePacket(Command, StartAddress, Data);
          Packet.make(Command, StartAddress, Data, ref SendDataPacket, ref Mc, ref McVar);
          u16PtrCnt = Mc.CmdAck.u16PtrCnt;
          calc_crc = GetCRC(SendDataPacket, u16PtrCnt + 2);
          SendDataPacket[u16PtrCnt++] = (byte)(calc_crc >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(calc_crc >> 8);
          SendPacket(SendDataPacket, u16PtrCnt);
        }
        // else if (StartAddress == 2)// Sync state out PC<-MC
        else if (StartAddress == 3)// Sync resume
        {
          //MakePacket(Command, StartAddress, Data);
          Packet.make(Command, StartAddress, Data, ref SendDataPacket, ref Mc, ref McVar);
          u16PtrCnt = Mc.CmdAck.u16PtrCnt;
          calc_crc = GetCRC(SendDataPacket, u16PtrCnt + 2);
          SendDataPacket[u16PtrCnt++] = (byte)(calc_crc >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(calc_crc >> 8);
          SendPacket(SendDataPacket, u16PtrCnt);
        }
        else if (StartAddress == 4)// Sync in event update
        {
          //MakePacket(Command, StartAddress, Data);
          Packet.make(Command, StartAddress, Data, ref SendDataPacket, ref Mc, ref McVar);
          u16PtrCnt = Mc.CmdAck.u16PtrCnt;
          calc_crc = GetCRC(SendDataPacket, u16PtrCnt + 2);
          SendDataPacket[u16PtrCnt++] = (byte)(calc_crc >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(calc_crc >> 8);
          SendPacket(SendDataPacket, u16PtrCnt);
        }
      }
      else if (Command == 7)
      {
        if (StartAddress == 1)//download driver info
        {
          McVar.TqOffsetValue = (float)Double.Parse(tbTqOffsetValue.Text);
          //MakePacket(Command, StartAddress, Data);
          Packet.make(Command, StartAddress, Data, ref SendDataPacket, ref Mc, ref McVar);
          u16PtrCnt = Mc.CmdAck.u16PtrCnt;
          calc_crc = GetCRC(SendDataPacket, u16PtrCnt + 2);
          SendDataPacket[u16PtrCnt++] = (byte)(calc_crc >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(calc_crc >> 8);
          SendPacket(SendDataPacket, u16PtrCnt);
        }
        // else if (StartAddress == 2)//upload driver info
        else if (StartAddress == 3)//Speaker & Output
        {
          //MakePacket(Command, StartAddress, Data);
          Packet.make(Command, StartAddress, Data, ref SendDataPacket, ref Mc, ref McVar);
          u16PtrCnt = Mc.CmdAck.u16PtrCnt;
          calc_crc = GetCRC(SendDataPacket, u16PtrCnt + 2);
          SendDataPacket[u16PtrCnt++] = (byte)(calc_crc >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(calc_crc >> 8);
          SendPacket(SendDataPacket, u16PtrCnt);
        }
        else if (StartAddress == 4)//LED band & output
        {
          MakePacket(Command, StartAddress, Data);
          //Packet.make(Command, StartAddress, Data, ref SendDataPacket, ref Mc, ref McVar);
          u16PtrCnt = Mc.CmdAck.u16PtrCnt;
          calc_crc = GetCRC(SendDataPacket, u16PtrCnt + 2);
          SendDataPacket[u16PtrCnt++] = (byte)(calc_crc >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(calc_crc >> 8);
          SendPacket(SendDataPacket, u16PtrCnt);
        }
        else if (StartAddress == 5)//request Driver Info
        {
          MakePacket(Command, StartAddress, Data);
          //Packet.make(Command, StartAddress, Data, ref SendDataPacket, ref Mc, ref McVar);
          u16PtrCnt = Mc.CmdAck.u16PtrCnt;
          calc_crc = GetCRC(SendDataPacket, u16PtrCnt + 2);
          SendDataPacket[u16PtrCnt++] = (byte)(calc_crc >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(calc_crc >> 8);
          SendPacket(SendDataPacket, u16PtrCnt);
        }
        // else if (StartAddress == 6)//
        // else if (StartAddress == 7)//
        else if (StartAddress == 8)//reset Maintenance count
        {
          MakePacket(Command, StartAddress, Data);
          //Packet.make(Command, StartAddress, Data, ref SendDataPacket, ref Mc, ref McVar);
          u16PtrCnt = Mc.CmdAck.u16PtrCnt;
          calc_crc = GetCRC(SendDataPacket, u16PtrCnt + 2);
          SendDataPacket[u16PtrCnt++] = (byte)(calc_crc >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(calc_crc >> 8);
          SendPacket(SendDataPacket, u16PtrCnt);
        }
        else if (StartAddress == 9)// Set torque Offset
        {
          MakePacket(Command, StartAddress, Data);
          //Packet.make(Command, StartAddress, Data, ref SendDataPacket, ref Mc, ref McVar);
          u16PtrCnt = Mc.CmdAck.u16PtrCnt;
          calc_crc = GetCRC(SendDataPacket, u16PtrCnt + 2);
          SendDataPacket[u16PtrCnt++] = (byte)(calc_crc >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(calc_crc >> 8);
          SendPacket(SendDataPacket, u16PtrCnt);
        }
        else if (StartAddress == 10)//Check torque Sensor Offset
        {
          MakePacket(Command, StartAddress, Data);
          //Packet.make(Command, StartAddress, Data, ref SendDataPacket, ref Mc, ref McVar);
          u16PtrCnt = Mc.CmdAck.u16PtrCnt;
          calc_crc = GetCRC(SendDataPacket, u16PtrCnt + 2);
          SendDataPacket[u16PtrCnt++] = (byte)(calc_crc >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(calc_crc >> 8);
          SendPacket(SendDataPacket, u16PtrCnt);
        }
        else if (StartAddress == 11)//Save torque Sensor Offset
        {
          MakePacket(Command, StartAddress, Data);
          //Packet.make(Command, StartAddress, Data, ref SendDataPacket, ref Mc, ref McVar);
          u16PtrCnt = Mc.CmdAck.u16PtrCnt;
          calc_crc = GetCRC(SendDataPacket, u16PtrCnt + 2);
          SendDataPacket[u16PtrCnt++] = (byte)(calc_crc >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(calc_crc >> 8);
          SendPacket(SendDataPacket, u16PtrCnt);
        }
        else if (StartAddress == 12)//Start Initail Angle
        {
          MakePacket(Command, StartAddress, Data);
          //Packet.make(Command, StartAddress, Data, ref SendDataPacket, ref Mc, ref McVar);
          u16PtrCnt = Mc.CmdAck.u16PtrCnt;
          calc_crc = GetCRC(SendDataPacket, u16PtrCnt + 2);
          SendDataPacket[u16PtrCnt++] = (byte)(calc_crc >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(calc_crc >> 8);
          SendPacket(SendDataPacket, u16PtrCnt);
        }
        // else if (StartAddress == 13)//receive Initial Angle result
      }
      else if (Command == 8)
      {
        //if (StartAddress == 1)
        {
          MakePacket(Command, StartAddress, Data);
          //Packet.make(Command, StartAddress, Data, ref SendDataPacket, ref Mc, ref McVar);
          u16PtrCnt = Mc.CmdAck.u16PtrCnt;
          calc_crc = GetCRC(SendDataPacket, u16PtrCnt + 2);
          SendDataPacket[u16PtrCnt++] = (byte)(calc_crc >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(calc_crc >> 8);
          // Port.Write(SendDataPacket, 0, u16PtrCnt);
          SendPacket(SendDataPacket, u16PtrCnt);
        }
      }
      else if (Command == 9)
      {
        //if (StartAddress == 1)
        {
          McVar.TorquePgain = (ushort)UInt16.Parse(tbTorquePgain.Text);
          McVar.TorqueIgain = (ushort)UInt16.Parse(tbTorqueIgain.Text);
          McVar.TorqueFFgain = (ushort)UInt16.Parse(tbTorqueFFgain.Text);
          McVar.SpeedPgain = (ushort)UInt16.Parse(tbSpeedPgain.Text);
          McVar.SpeedIgain = (ushort)UInt16.Parse(tbSpeedIgain.Text);
          McVar.SpeedFFgain = (ushort)UInt16.Parse(tbSpeedFFgain.Text);
          MakePacket(Command, StartAddress, Data);
          //Packet.make(Command, StartAddress, Data, ref SendDataPacket, ref Mc, ref McVar);
          u16PtrCnt = Mc.CmdAck.u16PtrCnt;
          calc_crc = GetCRC(SendDataPacket, u16PtrCnt + 2);
          SendDataPacket[u16PtrCnt++] = (byte)(calc_crc >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(calc_crc >> 8);
          // Port.Write(SendDataPacket, 0, u16PtrCnt);
          SendPacket(SendDataPacket, u16PtrCnt);
        }
      }
      else if (Command == 104)
      {
        // if (StartAddress == 1)
        {
          MakePacket(Command, StartAddress, Data);
          //Packet.make(Command, StartAddress, Data, ref SendDataPacket, ref Mc, ref McVar);
          u16PtrCnt = Mc.CmdAck.u16PtrCnt;
          calc_crc = GetCRC(SendDataPacket, u16PtrCnt + 2);
          SendDataPacket[u16PtrCnt++] = (byte)(calc_crc >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(calc_crc >> 8);
          // Port.Write(SendDataPacket, 0, u16PtrCnt);
          SendPacket(SendDataPacket, u16PtrCnt);
        }
      }
      else if (Command == 106)
      {
        // if (StartAddress == 1)
        {
          MakePacket(Command, StartAddress, Data);
          //Packet.make(Command, StartAddress, Data, ref SendDataPacket, ref Mc, ref McVar);
          u16PtrCnt = Mc.CmdAck.u16PtrCnt;
          calc_crc = GetCRC(SendDataPacket, u16PtrCnt + 2);
          SendDataPacket[u16PtrCnt++] = (byte)(calc_crc >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(calc_crc >> 8);
          // Port.Write(SendDataPacket, 0, u16PtrCnt);
          SendPacket(SendDataPacket, u16PtrCnt);
        }
      }
    }
    private void MakePacket(byte Command, ushort StartAddress, short Data)
    {
      // ushort data, A1, A2, A3;
      ushort u16PtrCnt = 0;
      ushort Revision = 0;
      byte TryNum = 0;
      ushort u16Value;

      TestUnion d = new TestUnion();

      SendDataPacket[u16PtrCnt++] = (byte)0x5A;              // Start low            0
      SendDataPacket[u16PtrCnt++] = (byte)0xA5;              // Start high           1
      SendDataPacket[u16PtrCnt++] = (byte)0;//(Length>>0);   // Length low           2
      SendDataPacket[u16PtrCnt++] = (byte)0;//(Length>>8);   // Length high          3
      SendDataPacket[u16PtrCnt++] = Command;                    // Function code        4
      SendDataPacket[u16PtrCnt++] = (byte)(Revision >> 0);     // revision low         5
      SendDataPacket[u16PtrCnt++] = (byte)(Revision >> 8);     // revision high        6
      SendDataPacket[u16PtrCnt++] = TryNum;                     // TryNum               7
      SendDataPacket[u16PtrCnt++] = (byte)(StartAddress >> 0); // Start Address low    8
      SendDataPacket[u16PtrCnt++] = (byte)(StartAddress >> 8); // Start Address high   9

      //if (Command == 1)
      //{
      //  if ((StartAddress == 1) || (StartAddress == 2))
      //  {
      //    SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_TCAM_ACTM >> 0);
      //    SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_TCAM_ACTM >> 8);
      //    d.f = Mc.Para.val.f32MC_FASTEN_TORQUE;
      //    SendDataPacket[u16PtrCnt++] = d.b0;
      //    SendDataPacket[u16PtrCnt++] = d.b1;
      //    SendDataPacket[u16PtrCnt++] = d.b2;
      //    SendDataPacket[u16PtrCnt++] = d.b3;
      //    d.f = Mc.Para.val.f32MC_TORQUE_MIN_MAX;
      //    SendDataPacket[u16PtrCnt++] = d.b0;
      //    SendDataPacket[u16PtrCnt++] = d.b1;
      //    SendDataPacket[u16PtrCnt++] = d.b2;
      //    SendDataPacket[u16PtrCnt++] = d.b3;
      //    SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_TARGET_ANGLE >> 0);
      //    SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_TARGET_ANGLE >> 8);
      //    SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_FASTEN_MIN_ANGLE >> 0);
      //    SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_FASTEN_MIN_ANGLE >> 8);
      //    SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_FASTEN_MAX_ANGLE >> 0);
      //    SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_FASTEN_MAX_ANGLE >> 8);
      //    d.f = Mc.Para.val.f32MC_SNUG_TORQUE;
      //    SendDataPacket[u16PtrCnt++] = d.b0;
      //    SendDataPacket[u16PtrCnt++] = d.b1;
      //    SendDataPacket[u16PtrCnt++] = d.b2;
      //    SendDataPacket[u16PtrCnt++] = d.b3;
      //    SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_FASTEN_SPEED >> 0);
      //    SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_FASTEN_SPEED >> 8);
      //    SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_FREE_FASTEN_ANGLE >> 0);
      //    SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_FREE_FASTEN_ANGLE >> 8);
      //    SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_FREE_FASTEN_SPEED >> 0);
      //    SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_FREE_FASTEN_SPEED >> 8);
      //    SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_SOFT_START >> 0);
      //    SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_SOFT_START >> 8);
      //    SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_FASTEN_SEATTING_POINT_RATE >> 0);
      //    SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_FASTEN_SEATTING_POINT_RATE >> 8);
      //    SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_FASTEN_TQ_RISING_TIME >> 0);
      //    SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_FASTEN_TQ_RISING_TIME >> 8);
      //    SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_RAMP_UP_SPEED >> 0);
      //    SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_RAMP_UP_SPEED >> 8);
      //    SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_TORQUE_COMPENSATION >> 0);
      //    SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_TORQUE_COMPENSATION >> 8);
      //    SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_TORQUE_OFFSET >> 0);
      //    SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_TORQUE_OFFSET >> 8);
      //    SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_MAX_PULSE_COUNT >> 0);
      //    SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_MAX_PULSE_COUNT >> 8);
      //    SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_SCREW_TYPE >> 0);
      //    SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_SCREW_TYPE >> 8);
      //    SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_SOFT_STOP >> 0);
      //    SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_SOFT_STOP >> 8);


      //    SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_ADVANCED_MODE >> 0);
      //    SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_ADVANCED_MODE >> 8);
      //    d.f = Mc.Para.val.f32MC_ADVANCED_PARA1;
      //    SendDataPacket[u16PtrCnt++] = d.b0;
      //    SendDataPacket[u16PtrCnt++] = d.b1;
      //    SendDataPacket[u16PtrCnt++] = d.b2;
      //    SendDataPacket[u16PtrCnt++] = d.b3;
      //    d.f = Mc.Para.val.f32MC_ADVANCED_PARA2;
      //    SendDataPacket[u16PtrCnt++] = d.b0;
      //    SendDataPacket[u16PtrCnt++] = d.b1;
      //    SendDataPacket[u16PtrCnt++] = d.b2;
      //    SendDataPacket[u16PtrCnt++] = d.b3;
      //    d.f = Mc.Para.val.f32MC_ADVANCED_PARA3;
      //    SendDataPacket[u16PtrCnt++] = d.b0;
      //    SendDataPacket[u16PtrCnt++] = d.b1;
      //    SendDataPacket[u16PtrCnt++] = d.b2;
      //    SendDataPacket[u16PtrCnt++] = d.b3;
      //    d.f = Mc.Para.val.f32MC_ADVANCED_PARA4;
      //    SendDataPacket[u16PtrCnt++] = d.b0;
      //    SendDataPacket[u16PtrCnt++] = d.b1;
      //    SendDataPacket[u16PtrCnt++] = d.b2;
      //    SendDataPacket[u16PtrCnt++] = d.b3;
      //    d.f = Mc.Para.val.f32MC_ADVANCED_PARA5;
      //    SendDataPacket[u16PtrCnt++] = d.b0;
      //    SendDataPacket[u16PtrCnt++] = d.b1;
      //    SendDataPacket[u16PtrCnt++] = d.b2;
      //    SendDataPacket[u16PtrCnt++] = d.b3;
      //    d.f = Mc.Para.val.f32MC_ADVANCED_PARA6;
      //    SendDataPacket[u16PtrCnt++] = d.b0;
      //    SendDataPacket[u16PtrCnt++] = d.b1;
      //    SendDataPacket[u16PtrCnt++] = d.b2;
      //    SendDataPacket[u16PtrCnt++] = d.b3;
      //    d.f = Mc.Para.val.f32MC_ADVANCED_PARA7;
      //    SendDataPacket[u16PtrCnt++] = d.b0;
      //    SendDataPacket[u16PtrCnt++] = d.b1;
      //    SendDataPacket[u16PtrCnt++] = d.b2;
      //    SendDataPacket[u16PtrCnt++] = d.b3;
      //    d.f = Mc.Para.val.f32MC_ADVANCED_PARA8;
      //    SendDataPacket[u16PtrCnt++] = d.b0;
      //    SendDataPacket[u16PtrCnt++] = d.b1;
      //    SendDataPacket[u16PtrCnt++] = d.b2;
      //    SendDataPacket[u16PtrCnt++] = d.b3;
      //    d.f = Mc.Para.val.f32MC_ADVANCED_PARA9;
      //    SendDataPacket[u16PtrCnt++] = d.b0;
      //    SendDataPacket[u16PtrCnt++] = d.b1;
      //    SendDataPacket[u16PtrCnt++] = d.b2;
      //    SendDataPacket[u16PtrCnt++] = d.b3;
      //    d.f = Mc.Para.val.f32MC_ADVANCED_PARA10;
      //    SendDataPacket[u16PtrCnt++] = d.b0;
      //    SendDataPacket[u16PtrCnt++] = d.b1;
      //    SendDataPacket[u16PtrCnt++] = d.b2;
      //    SendDataPacket[u16PtrCnt++] = d.b3;
      //    d.f = Mc.Para.val.f32MC_ADVANCED_PARA11;
      //    SendDataPacket[u16PtrCnt++] = d.b0;
      //    SendDataPacket[u16PtrCnt++] = d.b1;
      //    SendDataPacket[u16PtrCnt++] = d.b2;
      //    SendDataPacket[u16PtrCnt++] = d.b3;
      //    d.f = Mc.Para.val.f32MC_ADVANCED_PARA12;
      //    SendDataPacket[u16PtrCnt++] = d.b0;
      //    SendDataPacket[u16PtrCnt++] = d.b1;
      //    SendDataPacket[u16PtrCnt++] = d.b2;
      //    SendDataPacket[u16PtrCnt++] = d.b3;
      //    d.f = Mc.Para.val.f32MC_ADVANCED_PARA13;
      //    SendDataPacket[u16PtrCnt++] = d.b0;
      //    SendDataPacket[u16PtrCnt++] = d.b1;
      //    SendDataPacket[u16PtrCnt++] = d.b2;
      //    SendDataPacket[u16PtrCnt++] = d.b3;
      //    d.f = Mc.Para.val.f32MC_ADVANCED_PARA14;
      //    SendDataPacket[u16PtrCnt++] = d.b0;
      //    SendDataPacket[u16PtrCnt++] = d.b1;
      //    SendDataPacket[u16PtrCnt++] = d.b2;
      //    SendDataPacket[u16PtrCnt++] = d.b3;
      //    d.f = Mc.Para.val.f32MC_ADVANCED_PARA15;
      //    SendDataPacket[u16PtrCnt++] = d.b0;
      //    SendDataPacket[u16PtrCnt++] = d.b1;
      //    SendDataPacket[u16PtrCnt++] = d.b2;
      //    SendDataPacket[u16PtrCnt++] = d.b3;
      //    d.f = Mc.Para.val.f32MC_ADVANCED_PARA16;
      //    SendDataPacket[u16PtrCnt++] = d.b0;
      //    SendDataPacket[u16PtrCnt++] = d.b1;
      //    SendDataPacket[u16PtrCnt++] = d.b2;
      //    SendDataPacket[u16PtrCnt++] = d.b3;
      //    d.f = Mc.Para.val.f32MC_ADVANCED_PARA17;
      //    SendDataPacket[u16PtrCnt++] = d.b0;
      //    SendDataPacket[u16PtrCnt++] = d.b1;
      //    SendDataPacket[u16PtrCnt++] = d.b2;
      //    SendDataPacket[u16PtrCnt++] = d.b3;
      //    d.f = Mc.Para.val.f32MC_ADVANCED_PARA18;
      //    SendDataPacket[u16PtrCnt++] = d.b0;
      //    SendDataPacket[u16PtrCnt++] = d.b1;
      //    SendDataPacket[u16PtrCnt++] = d.b2;
      //    SendDataPacket[u16PtrCnt++] = d.b3;
      //    d.f = Mc.Para.val.f32MC_ADVANCED_PARA19;
      //    SendDataPacket[u16PtrCnt++] = d.b0;
      //    SendDataPacket[u16PtrCnt++] = d.b1;
      //    SendDataPacket[u16PtrCnt++] = d.b2;
      //    SendDataPacket[u16PtrCnt++] = d.b3;
      //    SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_FREE_REVERSE_ROTATION_SPEED >> 0);
      //    SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_FREE_REVERSE_ROTATION_SPEED >> 8);
      //    d.f = Mc.Para.val.f32MC_FREE_REVERSE_ROTATION_ANGLE;
      //    SendDataPacket[u16PtrCnt++] = d.b0;
      //    SendDataPacket[u16PtrCnt++] = d.b1;
      //    SendDataPacket[u16PtrCnt++] = d.b2;
      //    SendDataPacket[u16PtrCnt++] = d.b3;
      //    SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_REVERS_ANGLE_SETTING_SPEED >> 0);
      //    SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_REVERS_ANGLE_SETTING_SPEED >> 8);
      //    SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_REVERS_ANGLE_SETTING_ANGLE >> 0);
      //    SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_REVERS_ANGLE_SETTING_ANGLE >> 8);
      //    SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_REVERS_ANGLE_SETTING_FW_REV >> 0);
      //    SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_REVERS_ANGLE_SETTING_FW_REV >> 8);

      //    if (StartAddress == 1)
      //    {
      //      SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_UNIT >> 0);
      //      SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_UNIT >> 8);
      //      SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_ACC_DEC_TIME >> 0);
      //      SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_ACC_DEC_TIME >> 8);
      //      SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_FASTEN_TORQUE_MAINTAIN_TIME >> 0);
      //      SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_FASTEN_TORQUE_MAINTAIN_TIME >> 8);
      //      SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_USE_MAXTQ_FOR_LOOSENING >> 0);
      //      SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_USE_MAXTQ_FOR_LOOSENING >> 8);
      //      SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_LOOSENING_SPEED >> 0);
      //      SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_LOOSENING_SPEED >> 8);
      //      d.f = Mc.Para.val.f32MC_TOTAL_FASTENING_TIME;
      //      SendDataPacket[u16PtrCnt++] = d.b0;
      //      SendDataPacket[u16PtrCnt++] = d.b1;
      //      SendDataPacket[u16PtrCnt++] = d.b2;
      //      SendDataPacket[u16PtrCnt++] = d.b3;
      //      d.f = Mc.Para.val.f32MC_TOTAL_LOOSENING_TIME;
      //      SendDataPacket[u16PtrCnt++] = d.b0;
      //      SendDataPacket[u16PtrCnt++] = d.b1;
      //      SendDataPacket[u16PtrCnt++] = d.b2;
      //      SendDataPacket[u16PtrCnt++] = d.b3;
      //      d.f = Mc.Para.val.f32MC_STALL_LOOSENING_TIME_LIMIT;
      //      SendDataPacket[u16PtrCnt++] = d.b0;
      //      SendDataPacket[u16PtrCnt++] = d.b1;
      //      SendDataPacket[u16PtrCnt++] = d.b2;
      //      SendDataPacket[u16PtrCnt++] = d.b3;
      //      // SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_SCREW_TYPE >> 0);
      //      // SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_SCREW_TYPE >> 8);
      //      d.f = Mc.Para.val.f32MC_JUDGE_FASTEN_MIN_TURNS;
      //      SendDataPacket[u16PtrCnt++] = d.b0;
      //      SendDataPacket[u16PtrCnt++] = d.b1;
      //      SendDataPacket[u16PtrCnt++] = d.b2;
      //      SendDataPacket[u16PtrCnt++] = d.b3;
      //      SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_FASTENING_STOP_ALARM >> 0);
      //      SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_FASTENING_STOP_ALARM >> 8);
      //      SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_TORQUE_COMPENSATION_MAIN >> 0);
      //      SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_TORQUE_COMPENSATION_MAIN >> 8);
      //      SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_CROWFOOT_ENABLE >> 0);
      //      SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_CROWFOOT_ENABLE >> 8);
      //      d.f = Mc.Para.val.f32MC_CROWFOOT_RATIO;
      //      SendDataPacket[u16PtrCnt++] = d.b0;
      //      SendDataPacket[u16PtrCnt++] = d.b1;
      //      SendDataPacket[u16PtrCnt++] = d.b2;
      //      SendDataPacket[u16PtrCnt++] = d.b3;
      //      SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_CROWFOOT_EFFICIENCY >> 0);
      //      SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_CROWFOOT_EFFICIENCY >> 8);
      //      d.f = Mc.Para.val.f32MC_CROWFOOT_REVERSE_TORQUE;
      //      SendDataPacket[u16PtrCnt++] = d.b0;
      //      SendDataPacket[u16PtrCnt++] = d.b1;
      //      SendDataPacket[u16PtrCnt++] = d.b2;
      //      SendDataPacket[u16PtrCnt++] = d.b3;
      //      SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_CROWFOOT_REVERSE_SPEED >> 0);
      //      SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_CROWFOOT_REVERSE_SPEED >> 8);
      //      d.f = Mc.Para.val.f32MC_FREE_SPEED_MAX_TORQUE;
      //      SendDataPacket[u16PtrCnt++] = d.b0;
      //      SendDataPacket[u16PtrCnt++] = d.b1;
      //      SendDataPacket[u16PtrCnt++] = d.b2;
      //      SendDataPacket[u16PtrCnt++] = d.b3;
      //      // SendDataPacket[u16PtrCnt++] = (byte)(0);
      //      // SendDataPacket[u16PtrCnt++] = (byte)(0);
      //      // SendDataPacket[u16PtrCnt++] = (byte)(0);
      //      // SendDataPacket[u16PtrCnt++] = (byte)(0);
      //      SendDataPacket[u16PtrCnt++] = (byte)(0);
      //      SendDataPacket[u16PtrCnt++] = (byte)(0);
      //      SendDataPacket[u16PtrCnt++] = (byte)(0);
      //      SendDataPacket[u16PtrCnt++] = (byte)(0);
      //      SendDataPacket[u16PtrCnt++] = (byte)(0);
      //      SendDataPacket[u16PtrCnt++] = (byte)(0);
      //    }
      //    else
      //    {
      //      SendDataPacket[u16PtrCnt++] = (byte)(0);
      //      SendDataPacket[u16PtrCnt++] = (byte)(0);
      //      SendDataPacket[u16PtrCnt++] = (byte)(0);
      //      SendDataPacket[u16PtrCnt++] = (byte)(0);
      //    }
      //  }
      //  else if (StartAddress == 3) // Driver Model index & Info_DrvModel 1set
      //  {
      //    SendDataPacket[u16PtrCnt++] = (byte)(Mc.Info_DrvModel_para.u16Driver_id >> 0);
      //    SendDataPacket[u16PtrCnt++] = (byte)(Mc.Info_DrvModel_para.u16Driver_id >> 8);
      //    SendDataPacket[u16PtrCnt++] = (byte)(Mc.Info_DrvModel_para.u16Driver_vendor_id >> 0);
      //    SendDataPacket[u16PtrCnt++] = (byte)(Mc.Info_DrvModel_para.u16Driver_vendor_id >> 8);
      //    SendDataPacket[u16PtrCnt++] = (byte)(Mc.Info_DrvModel_para.u16Controller_id >> 0);
      //    SendDataPacket[u16PtrCnt++] = (byte)(Mc.Info_DrvModel_para.u16Controller_id >> 8);
      //    SendDataPacket[u16PtrCnt++] = (byte)(Mc.Info_DrvModel_para.u16Motor_id >> 0);
      //    SendDataPacket[u16PtrCnt++] = (byte)(Mc.Info_DrvModel_para.u16Motor_id >> 8);

      //    d.f = Mc.Info_DrvModel_para.f32Tq_min_Nm;
      //    SendDataPacket[u16PtrCnt++] = d.b0;
      //    SendDataPacket[u16PtrCnt++] = d.b1;
      //    SendDataPacket[u16PtrCnt++] = d.b2;
      //    SendDataPacket[u16PtrCnt++] = d.b3;
      //    d.f = Mc.Info_DrvModel_para.f32Tq_max_Nm;
      //    SendDataPacket[u16PtrCnt++] = d.b0;
      //    SendDataPacket[u16PtrCnt++] = d.b1;
      //    SendDataPacket[u16PtrCnt++] = d.b2;
      //    SendDataPacket[u16PtrCnt++] = d.b3;
      //    d.u = Mc.Info_DrvModel_para.u32Speed_min;
      //    SendDataPacket[u16PtrCnt++] = d.b0;
      //    SendDataPacket[u16PtrCnt++] = d.b1;
      //    SendDataPacket[u16PtrCnt++] = d.b2;
      //    SendDataPacket[u16PtrCnt++] = d.b3;
      //    d.u = Mc.Info_DrvModel_para.u32Speed_max;
      //    SendDataPacket[u16PtrCnt++] = d.b0;
      //    SendDataPacket[u16PtrCnt++] = d.b1;
      //    SendDataPacket[u16PtrCnt++] = d.b2;
      //    SendDataPacket[u16PtrCnt++] = d.b3;
      //    d.f = Mc.Info_DrvModel_para.f32Gear_ratio;
      //    SendDataPacket[u16PtrCnt++] = d.b0;
      //    SendDataPacket[u16PtrCnt++] = d.b1;
      //    SendDataPacket[u16PtrCnt++] = d.b2;
      //    SendDataPacket[u16PtrCnt++] = d.b3;
      //    d.f = Mc.Info_DrvModel_para.f32Angle_head_ratio;
      //    SendDataPacket[u16PtrCnt++] = d.b0;
      //    SendDataPacket[u16PtrCnt++] = d.b1;
      //    SendDataPacket[u16PtrCnt++] = d.b2;
      //    SendDataPacket[u16PtrCnt++] = d.b3;

      //    for (int ii = 0; ii < 32; ii++)
      //      SendDataPacket[u16PtrCnt++] = (byte)0;
      //  }
      //  // else if (StartAddress == 4) // MC model & version
      //}
      //if (Command == 2)
      //{
      //  switch (StartAddress)
      //  {
      //    case 1://fasten/loosen
      //      SendDataPacket[u16PtrCnt++] = (byte)(Data >> 0);
      //      SendDataPacket[u16PtrCnt++] = (byte)0;
      //      SendDataPacket[u16PtrCnt++] = (byte)0;
      //      SendDataPacket[u16PtrCnt++] = (byte)0;
      //      break;
      //    case 2://Start/Stop
      //      if (Data != 0)//start
      //      {
      //        string input = String.Empty;

      //        if (tbLoosenAngle.Text == "" || tbLoosenAngle.Text == "0")
      //          Mc.McFlag.LoosenAngle = 0;
      //        else
      //        {
      //          try
      //          {
      //            Mc.McFlag.LoosenAngle = Int16.Parse(tbLoosenAngle.Text);
      //            // Console.WriteLine(result);
      //          }
      //          catch (FormatException)
      //          {
      //            // Console.WriteLine($"Unable to parse '{input}'");
      //            Mc.McFlag.LoosenAngle = 0;
      //          }
      //        }
      //        SendDataPacket[u16PtrCnt++] = (byte)(Data >> 0);
      //        SendDataPacket[u16PtrCnt++] = (byte)(Mc.McFlag.LoosenAngle >> 0);
      //        SendDataPacket[u16PtrCnt++] = (byte)(Mc.McFlag.LoosenAngle >> 8);
      //        SendDataPacket[u16PtrCnt++] = (byte)McVar.SoftStop;
      //      }
      //      else//stop
      //      {
      //        SendDataPacket[u16PtrCnt++] = (byte)(Data >> 0);
      //        SendDataPacket[u16PtrCnt++] = (byte)0;
      //        SendDataPacket[u16PtrCnt++] = (byte)0;
      //        SendDataPacket[u16PtrCnt++] = (byte)0;
      //      }
      //      break;
      //    case 3://Save Origin Point
      //    case 4://Move origin
      //    case 5://Reset MC
      //    case 6://Reset Alarm/Error
      //    case 7://parameter initialization
      //      SendDataPacket[u16PtrCnt++] = (byte)0;
      //      SendDataPacket[u16PtrCnt++] = (byte)0;
      //      SendDataPacket[u16PtrCnt++] = (byte)0;
      //      SendDataPacket[u16PtrCnt++] = (byte)0;
      //      break;
      //    case 8://soft/hard joint customizing
      //    case 9://start/stop auto-customizing
      //    case 10://send start comm.
      //      SendDataPacket[u16PtrCnt++] = (byte)(Data >> 0);
      //      SendDataPacket[u16PtrCnt++] = (byte)0;
      //      SendDataPacket[u16PtrCnt++] = (byte)0;
      //      SendDataPacket[u16PtrCnt++] = (byte)0;
      //      break;
      //    // case 11://answer to start comm. PC<-MC
      //    default:
      //      break;
      //  }
      //}
      // else if (Command == 3) cyclic PC<-MC
      // else if (Command == 4) graph PC<-MC
      // else if (Command == 5) event PC<-MC
      //else
      //if (Command == 6)
      //{
      //  if (StartAddress == 1)// Sync setting
      //  {
      //    SendDataPacket[u16PtrCnt++] = Mc.SyncStruct.Bits_b1OnOff;
      //    SendDataPacket[u16PtrCnt++] = Mc.SyncStruct.Bits_b1Master;
      //    SendDataPacket[u16PtrCnt++] = (byte)(Mc.SyncStruct.u16WaitingBeforeSync >> 0);
      //    SendDataPacket[u16PtrCnt++] = (byte)(Mc.SyncStruct.u16WaitingBeforeSync >> 8);
      //    SendDataPacket[u16PtrCnt++] = (byte)(Mc.SyncStruct.u16WaitingBetweenSync >> 0);
      //    SendDataPacket[u16PtrCnt++] = (byte)(Mc.SyncStruct.u16WaitingBetweenSync >> 8);
      //    SendDataPacket[u16PtrCnt++] = (byte)0;
      //    SendDataPacket[u16PtrCnt++] = (byte)0;
      //  }
      //  // else if (StartAddress == 2)// Sync state out PC<-MC
      //  else if (StartAddress == 3)// Sync resume
      //  {
      //    SendDataPacket[u16PtrCnt++] = Mc.SyncStruct.Bits_b1ResumeOnOff;
      //    SendDataPacket[u16PtrCnt++] = (byte)0;
      //    SendDataPacket[u16PtrCnt++] = (byte)0;
      //    SendDataPacket[u16PtrCnt++] = (byte)0;
      //  }
      //  else if (StartAddress == 4)// Sync in event update
      //  {
      //    SendDataPacket[u16PtrCnt++] = Mc.SyncStruct.Bits_b1SyncIn;
      //    SendDataPacket[u16PtrCnt++] = (byte)0;
      //    SendDataPacket[u16PtrCnt++] = (byte)0;
      //    SendDataPacket[u16PtrCnt++] = (byte)0;
      //  }
      //}
      //else if (Command == 7) // parameter
      //{
      //  if (StartAddress == 1) // Download Driver info
      //  {
      //    SendDataPacket[u16PtrCnt++] = (byte)(Mc.DriverInfo.u16Type >> 0);
      //    SendDataPacket[u16PtrCnt++] = (byte)(Mc.DriverInfo.u16Type >> 8);
      //    SendDataPacket[u16PtrCnt++] = (byte)(Mc.DriverInfo.u16Version >> 0);
      //    SendDataPacket[u16PtrCnt++] = (byte)(Mc.DriverInfo.u16Version >> 8);
      //    SendDataPacket[u16PtrCnt++] = (byte)(Mc.DriverInfo.u16Serial_low >> 0);
      //    SendDataPacket[u16PtrCnt++] = (byte)(Mc.DriverInfo.u16Serial_low >> 8);
      //    SendDataPacket[u16PtrCnt++] = (byte)(Mc.DriverInfo.u16Serial_high >> 0);
      //    SendDataPacket[u16PtrCnt++] = (byte)(Mc.DriverInfo.u16Serial_high >> 8);
      //    SendDataPacket[u16PtrCnt++] = (byte)(Mc.DriverInfo.u8Factory_Gear_efficiency >> 0);
      //    SendDataPacket[u16PtrCnt++] = (byte)(0);
      //    SendDataPacket[u16PtrCnt++] = (byte)(Mc.DriverInfo.u8User_Gear_efficiency >> 0);
      //    SendDataPacket[u16PtrCnt++] = (byte)(0);
      //    SendDataPacket[u16PtrCnt++] = (byte)(Mc.DriverInfo.u16DriverVendor >> 0);
      //    SendDataPacket[u16PtrCnt++] = (byte)(Mc.DriverInfo.u16DriverVendor >> 8);
      //    SendDataPacket[u16PtrCnt++] = (byte)(0);
      //    SendDataPacket[u16PtrCnt++] = (byte)(0);
      //  }
      //  // if (StartAddress == 2) // Upload Driver info PC<-MC
      //  else if (StartAddress == 3)//Speaker & Output
      //  {
      //    SendDataPacket[u16PtrCnt++] = (byte)(Data >> 0);
      //    SendDataPacket[u16PtrCnt++] = (byte)(0);
      //    SendDataPacket[u16PtrCnt++] = (byte)(0);
      //    SendDataPacket[u16PtrCnt++] = (byte)(0);
      //  }
      //  else if (StartAddress == 4)//LED Band Set
      //  {
      //    SendDataPacket[u16PtrCnt++] = (byte)(0);
      //    SendDataPacket[u16PtrCnt++] = (byte)(0);
      //    SendDataPacket[u16PtrCnt++] = (byte)(0);
      //    SendDataPacket[u16PtrCnt++] = (byte)(0);
      //  }
      //  else if (StartAddress == 5)//request Driver Info
      //  {
      //    SendDataPacket[u16PtrCnt++] = (byte)(0);
      //    SendDataPacket[u16PtrCnt++] = (byte)(0);
      //    SendDataPacket[u16PtrCnt++] = (byte)(0);
      //    SendDataPacket[u16PtrCnt++] = (byte)(0);
      //  }
      //  // else if (StartAddress == 6)//reserved
      //  // else if (StartAddress == 7)//reserved
      //  else if (StartAddress == 8)//Reset maintenance count
      //  {
      //    SendDataPacket[u16PtrCnt++] = (byte)(1);
      //    SendDataPacket[u16PtrCnt++] = (byte)(0);
      //    SendDataPacket[u16PtrCnt++] = (byte)(0);
      //    SendDataPacket[u16PtrCnt++] = (byte)(0);
      //  }
      //  else if (StartAddress == 9)//Set Torque Offset
      //  {
      //    //d.f = (float)Double.Parse(tbTqOffsetValue.Text);
      //    d.f = McVar.TqOffsetValue;
      //    SendDataPacket[u16PtrCnt++] = d.b0;
      //    SendDataPacket[u16PtrCnt++] = d.b1;
      //    SendDataPacket[u16PtrCnt++] = d.b2;
      //    SendDataPacket[u16PtrCnt++] = d.b3;
      //    SendDataPacket[u16PtrCnt++] = (byte)(2);//unit low
      //    SendDataPacket[u16PtrCnt++] = (byte)(0);//unit high
      //    SendDataPacket[u16PtrCnt++] = (byte)(0);
      //    SendDataPacket[u16PtrCnt++] = (byte)(0);
      //  }
      //  else if (StartAddress == 10)//Check torque sensor OffsetADC
      //  {
      //    SendDataPacket[u16PtrCnt++] = (byte)(0);//(byte)(Mc.DriverInfo.u16TorqueOffset >> 0);
      //    SendDataPacket[u16PtrCnt++] = (byte)(0);//(byte)(Mc.DriverInfo.u16TorqueOffset >> 8);
      //    SendDataPacket[u16PtrCnt++] = (byte)(0);
      //    SendDataPacket[u16PtrCnt++] = (byte)(0);
      //  }
      //  else if (StartAddress == 11)//Save torque sensor OffsetADC
      //  {
      //    SendDataPacket[u16PtrCnt++] = (byte)(Mc.DriverInfo.u16TorqueOffset >> 0);
      //    SendDataPacket[u16PtrCnt++] = (byte)(Mc.DriverInfo.u16TorqueOffset >> 8);
      //    SendDataPacket[u16PtrCnt++] = (byte)(0);
      //    SendDataPacket[u16PtrCnt++] = (byte)(0);
      //  }
      //  else if (StartAddress == 12)
      //  {
      //    SendDataPacket[u16PtrCnt++] = (byte)(Data >> 0);
      //    SendDataPacket[u16PtrCnt++] = (byte)(Data >> 8);
      //    SendDataPacket[u16PtrCnt++] = (byte)(0);
      //    SendDataPacket[u16PtrCnt++] = (byte)(0);
      //  }
      //  // else if (StartAddress == 13)//reseive Initial angle result PC<-MC
      //  else
      //  {
      //    SendDataPacket[u16PtrCnt++] = (byte)(Data >> 0);
      //    SendDataPacket[u16PtrCnt++] = (byte)(Data >> 8);
      //  }
      //}
      //else if (Command == 8) // MotTest or NutRunner
      //{
      //  //if (StartAddress == 1)
      //  {
      //    SendDataPacket[u16PtrCnt++] = (byte)(Data >> 0);
      //    SendDataPacket[u16PtrCnt++] = (byte)(Data >> 8);
      //  }
      //}
      //else 
      if (Command == 9) // MotTest or NutRunner
      {
        //if (StartAddress == 1)
        // {
        //   SendDataPacket[u16PtrCnt++] = (byte)(Data >> 0);
        //   SendDataPacket[u16PtrCnt++] = (byte)(Data >> 8);
        // }
        switch (StartAddress)
        {
          case 1:
          case 2:
          case 3:
          case 4:
          case 5:
          case 6:
          case 7:
          case 8:
            SendDataPacket[u16PtrCnt++] = (byte)(Data >> 0);
            SendDataPacket[u16PtrCnt++] = (byte)(Data >> 8);
            break;
          case 9:
            u16Value = (ushort)UInt16.Parse(tbTorquePgain.Text);
            SendDataPacket[u16PtrCnt++] = (byte)(u16Value >> 0);//10
            SendDataPacket[u16PtrCnt++] = (byte)(u16Value >> 8);
            u16Value = (ushort)UInt16.Parse(tbTorqueIgain.Text);
            SendDataPacket[u16PtrCnt++] = (byte)(u16Value >> 0);//12
            SendDataPacket[u16PtrCnt++] = (byte)(u16Value >> 8);
            u16Value = (ushort)UInt16.Parse(tbTorqueFFgain.Text);
            SendDataPacket[u16PtrCnt++] = (byte)(u16Value >> 0);//14
            SendDataPacket[u16PtrCnt++] = (byte)(u16Value >> 8);
            u16Value = (ushort)UInt16.Parse(tbSpeedPgain.Text);
            SendDataPacket[u16PtrCnt++] = (byte)(u16Value >> 0);//16
            SendDataPacket[u16PtrCnt++] = (byte)(u16Value >> 8);
            u16Value = (ushort)UInt16.Parse(tbSpeedIgain.Text);
            SendDataPacket[u16PtrCnt++] = (byte)(u16Value >> 0);//18
            SendDataPacket[u16PtrCnt++] = (byte)(u16Value >> 8);
            u16Value = (ushort)UInt16.Parse(tbSpeedFFgain.Text);
            SendDataPacket[u16PtrCnt++] = (byte)(u16Value >> 0);//20
            SendDataPacket[u16PtrCnt++] = (byte)(u16Value >> 8);
            break;
          default:
            break;
        }
        // MakeAndSendData(9, addr, Convert.ToInt16(((NumericUpDown)control).Value));
      }
      else if (Command == 104) // parameter
      {
        // if (StartAddress == 1)
        {
          SendDataPacket[u16PtrCnt++] = (byte)(Data >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(Data >> 8);
        }
      }
      else if (Command == 106) // parameter
      {
        // if (StartAddress == 1)
        {
          SendDataPacket[u16PtrCnt++] = (byte)(Data >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(Data >> 8);
        }
      }

      ushort Length = (ushort)(u16PtrCnt - 4);
      SendDataPacket[_LengthLow] = (byte)(Length >> 0);     // Length low
      SendDataPacket[_LengthHigh] = (byte)(Length >> 8);    // Length high

      if (((Command == 2) && (StartAddress == 10)) // Start comm.
                                                   // ||((Command == 2)&&( StartAddress == 11))
                                                   // || ((LcdMcCmdAck.u8Command == 3)&&(LcdMcCmdAck.u16StartAddress == 1))) // cyclic no ack processing
        || ((Command == 3) && (StartAddress == 1))) // cyclic no ack processing
      {
        Packet.ResetAckState(ref Mc);
        Mc.CmdAck.u16PtrCnt = u16PtrCnt;
      }
      else
      {
        Mc.CmdAck.u8Command = Command;
        Mc.CmdAck.u16PtrCnt = u16PtrCnt;
        Mc.CmdAck.u16StartAddress = StartAddress;
        Mc.CmdAck.u8AckWait = ON;
      }
    }
    ushort GetCRC(byte[] data, int Length)
    {
      int i, j;
      ushort CRCFull = 0xFFFF;
      byte CRCLSB;
      for (i = 0; i < Length - 2; i++)
      {
        CRCFull = (ushort)(CRCFull ^ data[i]);

        for (j = 0; j < 8; j++)
        {
          CRCLSB = (byte)(CRCFull & 0x0001);
          CRCFull = (ushort)((CRCFull >> 1) & 0x7FFF);

          if (CRCLSB == 1)
            CRCFull = (ushort)(CRCFull ^ 0xA001);
        }
      }

      return CRCFull;
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
        MakeAndSendData(8, 2, 0);
      }
      else// btTorque
      {
        nudSpeed.Enabled = false;
        nudTorque.Enabled = true;
        MakeAndSendData(8, 2, 1);
      }
    }
    private void ServoOn_Click(object sender, EventArgs e)
    {
      if (!Port.IsOpen)
        return;
      if (btServoOnOff.Text == "Servo On")
      {
        MakeAndSendData(8, 3, 1);
        Mc.McFlag.b1Run = 1;
        btServoOnOff.Text = "Servo Off";
      }
      else
      {
        MakeAndSendData(8, 3, 0);
        Mc.McFlag.b1Run = 0;
        btServoOnOff.Text = "Servo On";
      }
    }
    private void TestModeSelect_Click(object sender, EventArgs e)
    {
      if (!Port.IsOpen)
        return;

      if (sender == btMotorTest)
      {
        MakeAndSendData(8, 1, 1);
        btServoOnOff.Enabled = true;
        // gbServo.Visible = true;
        // gbFastenLoosen.Visible = false;
        rbMot.Checked = true;
      }
      else// (sender == btNutRunner)
      {
        MakeAndSendData(8, 1, 0);
        btServoOnOff.Enabled = false;
        // gbFastenLoosen.Visible = true;
        // gbServo.Visible = false;
        rbNut.Checked = true;
      }
    }
    private void btSaveOrigin_Click(object sender, EventArgs e)
    {
      MakeAndSendData(2, 3, 0);
    }
    private void btStartOrigin_Click(object sender, EventArgs e)
    {
      MakeAndSendData(2, 4, 0);
    }
    private void btResetMC_Click(object sender, EventArgs e)
    {
      MakeAndSendData(2, 5, 0);
    }
    private void btSoftHardAutocustom_Click(object sender, EventArgs e)
    {
      if (btSoftHardAutocustom.Text == "Soft")
      {
        MakeAndSendData(2, 8, 0);
        // btSoftHardAutocustom.Text = "Hard";
        // rbSoftAutocustom.Checked = true;
      }
      else//Hard
      {
        MakeAndSendData(2, 8, 1);
        // btSoftHardAutocustom.Text = "Soft";
        // rbHardAutocustom.Checked = true;
      }
    }
    private void btStartStopAutocustom_Click(object sender, EventArgs e)
    {
      if (btStartStopAutocustom.Text == "Start")
      {
        MakeAndSendData(2, 9, 1);
        // btStartStopAutocustom.Text = "Stop";
        // rbStartAutocustom.Checked = true;
      }
      else//Stop
      {
        MakeAndSendData(2, 9, 0);
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
      switch (Port.IsOpen)
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

            ComReadIndex = 0;
            RecvBuf.tail = 0;
            RecvBuf.head = 0;
            // set port
            Port.PortName = port;
            Port.BaudRate = baudrate;
            Port.Encoding = Encoding.GetEncoding(28591);
            // open
            Port.Open();
            // InitAutoSetting();
            // InitMcFlag();
            // InitMcInfo();
            // InitSyncStruct();
            // InitInfo_DrvModel_para(4);//1);
            // InitDriverInfo(4);
            // InitParameter(4);
            // set event
            // Port.DataReceived += PortOnDataReceived;
            Port.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);

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
            Port.DiscardOutBuffer();
            Port.DiscardInBuffer();
            Port.Close();
            // stop timer
            while (timer_working) { }
            workTimer.Stop();
            // change button text
            btCommOpen.Text = @"Open";
            Port.DataReceived -= new SerialDataReceivedEventHandler(DataReceivedHandler);

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
        //   //               fresh_graph_data();
        //   //           }));
        //   // fresh_graph_data();
        //   // graph_count++;
        // }
        ProcessPcMcReceivedCommData();
        Thread.Sleep(50);
      }
    }
    // private void btMotor_Click(object sender, EventArgs e)
    // {
    //     // var list = new List<byte>();
    //     // check sender
    //     if (btRunStop.Text == @"Servo On")
    //     {
    //         MakeAndSendData(106, 1, 1);
    //         btRunStop.Text = @"Servo Off";
    //     }
    //     else
    //     {
    //         MakeAndSendData(106, 1, 0);
    //         btRunStop.Text = @"Servo On";
    //     }

    //     // // check port is open
    //     // if (Port.IsOpen && list.Count > 0)
    //     //   // write packet
    //     //   Port.Write(list.ToArray(), 0, list.Count);
    // }
    private void btStartStopFL_Click(object sender, EventArgs e)
    {
      if (btStartStopFL.Text == "StartFL")
      {
        MakeAndSendData(2, 2, 1);
      }
      else//Loosen
      {
        MakeAndSendData(2, 2, 0);
      }
    }
    private void btFastenLoosen_Click(object sender, EventArgs e)
    {
      clear_graph_flag = true;
      if (btFastenLoosen.Text == "Fasten")
      {
        MakeAndSendData(2, 1, 0);
      }
      else//Loosen
      {
        MakeAndSendData(2, 1, 1);
      }
    }
    private void btMcInit_Click(object sender, EventArgs e)
    {
      if (!Port.IsOpen)
        return;
      ushort DriverType = (ushort)Int16.Parse(tbDriverType.Text);
      Mc.InitInfo_DrvModel_para(DriverType);//1);
      Mc.InitDriverInfo(DriverType);
      Mc.InitParameter(DriverType);
      MakeAndSendData(2, 10, 0);
      McVar.IniStep = 0;
    }
    private void btTqOffset_Click(object sender, EventArgs e)
    {
      
    }
    private void btSetTqOffset_Click(object sender, EventArgs e)
    {
      if (!Port.IsOpen)
        return;

      if (sender == btSetTqOffset)
      {
        MakeAndSendData(7, 9, 0);
        btSetTqOffset.Enabled = true;
      }
    }
    private void btAlarmReset_Click(object sender, EventArgs e)
    {
      if (!Port.IsOpen)
        return;
      MakeAndSendData(2, 6, 0);
    }
    private void btCalibrationCommand_Click(object sender, EventArgs e)
    {
      if (!Port.IsOpen)
        return;
      if (sender == btCalibStart)
      {
        MakeAndSendData(7, 12, 1);
      }
      else
      {
        MakeAndSendData(7, 12, 0);
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
          MakeAndSendData(9, addr, (short)(Convert.ToInt32(((NumericUpDown)control).Value / 10)));
          break;
        case 2:
        case 3:
        case 4:
        case 5:
        case 6:
        case 7:
        case 8:
          // add range
          // packet.AddRange(GetPacket(addr, Convert.ToInt32(((ComboBox)control).SelectedIndex)));
          // MakeAndSendData(9, addr, Convert.ToInt16(((ComboBox)control).SelectedIndex));
          MakeAndSendData(9, addr, Convert.ToInt16(((NumericUpDown)control).Value));
          break;
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
          //     MakeAndSendData(106, addr, Convert.ToInt16(((NumericUpDown)control).Value));
          //     break;
      }
      // // check port is open
      // if (Port.IsOpen && packet.Count > 0)
      //     // write packet
      //     Port.Write(packet.ToArray(), 0, packet.Count);

      // // debug
      // foreach (var b in packet)
      // {
      //     Debug.Write($@"{b:X2} ");
      // }
      // Debug.WriteLine(string.Empty);
    }
    private void btnSetAllGain_Click(object sender, EventArgs e)
    {
      MakeAndSendData(9, 9, 0);
    }
    private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
    {
      try
      {
        if (Port.IsOpen)
        {
          port_working = true;
          // this.Invoke(new EventHandler(MySerialReceived));//
          byte[] data = Port.Encoding.GetBytes(Port.ReadExisting());
          // rbuf_put(data, (ushort)(data.Count()));
          // cq.CopyTo(data, data.Count());
          for (int i = 0; i < data.Count(); i++)
          {
            cq.Enqueue(data[i]);
          }
          port_working = false;
        }
      }
      finally
      {
        //Port.Close();
      }

    }

    // private void MySerialReceived(object s, EventArgs e)  //
    // {
    //   try
    //   {
    //     byte[] data = Port.Encoding.GetBytes(Port.ReadExisting());
    //     // rbuf_put(data, (ushort)(data.Count()));
    //     // cq.CopyTo(data, data.Count());
    //     for (int i=0;i<data.Count();i++)
    //     {
    //       cq.Enqueue(data[i]);
    //     }
    //     // ProcessPcMcReceivedCommData();
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

      tbTqSensorValue.Text = TqSensorValue.ToString();
      tbTqSensorOffsetValue.Text = TqSensorOffsetValue.ToString();
      tbError.Text = Error.ToString();
      tbMaintCnt.Text = MaintCnt.ToString();
      tbEnc.Text = Enc.ToString();

      // tbDataCount.Text = Data_ch1.Count.ToString();
      tbDataCount.Text = graph_count.ToString();
      tbGraphDataCount.Text = Graph_ch1.Count.ToString();
      // tbGraphDataCount.Text = graph_count.ToString();//Graph_ch1.Count.ToString();

      if (refresh_graph_flag)
      {
        refresh_graph_flag = false;
        Refresh_graph();
      }

      if (McVar.DriverInfoIsReady)
      {
        ShowDriverInfo();
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
      switch (MotorState)
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
      switch (CalibStepState)
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
      switch (CalibResultState)
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
      if (Mc.McFlag.b1ControlFL != 0)
      {
        btFastenLoosen.Text = "Fasten";
        tbLoosenAngle.Enabled = true;
      }
      else
      {
        btFastenLoosen.Text = "Loosen";
        tbLoosenAngle.Enabled = false;
      }
      if (Mc.McFlag.b1Run != 0)
      {
        btStartStopFL.Text = "StopFL";
      }
      else
      {
        btStartStopFL.Text = "StartFL";
      }
      if (Mot_or_Nut)
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
      if (Mcinitialized != 0)
      {
        btMcInit.Text = @"Init MC - Yes";
      }
      else
      {
        btMcInit.Text = @"Init MC - No";
      }
      if (Mc.McFlag.b1Run == 0)
      {
        btServoOnOff.Text = "Servo On";
      }
      else
      {
        btServoOnOff.Text = "Servo Off";
      }
      timer_working = false;
    }
    public void ProcessPcMcReceivedCommData()
    {
      byte data;
      while (cq.Count > 0)
      {
        cq.TryDequeue(out data);

        ComReadBuffer[ComReadIndex++] = data;
        // check header length
        if ((ComReadBuffer[0] == 0x5A) && (ComReadBuffer[1] == 0xA5) && (ComReadIndex >= 4))
        {
          // get length
          var data_length = (ComReadBuffer[3] << 8) | ComReadBuffer[2];
          if (data_length > 900 || ComReadIndex > 900)
          {
            ComReadIndex = 0;
            continue;
          }
          // check analyze count
          if (ComReadIndex == (data_length + 6))
          {
            byte check_Command = ComReadBuffer[4];
            byte Command = (byte)(check_Command & 0x7f);
            byte Try_num = ComReadBuffer[7];
            ushort StartAddress = (ushort)((ComReadBuffer[9] << 8) | (ushort)ComReadBuffer[8]);
            ushort received_crc = (ushort)(ComReadBuffer[ComReadIndex - 2] & 0xff);
            received_crc |= (ushort)(ComReadBuffer[ComReadIndex - 1] << 8);
            ushort calc_crc = GetCRC(ComReadBuffer, ComReadIndex);
            ComReadIndex = 0;
            if (calc_crc == received_crc)
            {
              if (Command != 3)
              {
                Command_List_Pc[Command_Index_Pc, 0] = Command;
                Command_List_Pc[Command_Index_Pc, 1] = StartAddress;
                Command_List_Pc[Command_Index_Pc, 2] = 0;
                Command_Index_Pc++;
                if (Command_Index_Pc >= COMMAND_LIST_NUM)
                  Command_Index_Pc = 0;
              }
              // check command
              switch (Command)
              {
                case 1:
                  if (StartAddress == 1 || StartAddress == 2)// || StartAddress == 3)
                  {
                    Packet.ResetAckState(ref Mc);
                  }
                  else if (StartAddress == 3)
                  {
                    Packet.ResetAckState(ref Mc);
                    MakeAndSendData(1, 1, 0);
                  }
                  else if (StartAddress == 4)
                  {
                    AckSend(Command, 0, StartAddress, 0);       // return Ack OK
                    Mc.McInfo.u16Con_Model_Type = (ushort)((ComReadBuffer[11] << 8) | ComReadBuffer[10]);
                    Mc.McInfo.u16Version = (ushort)((ComReadBuffer[13] << 8) | ComReadBuffer[12]);
                  }
                  break;
                case 2:
                  if (StartAddress == 1 || StartAddress == 2 || StartAddress == 3 || StartAddress == 4 ||
                      StartAddress == 6 || StartAddress == 7 || StartAddress == 8 || StartAddress == 9 || StartAddress == 10)
                  {
                    Packet.ResetAckState(ref Mc);
                  }
                  else if (StartAddress == 5)
                  {
                    // this.Invoke(new Action(delegate ()
                    // {
                    //   btMcInit.Text = @"Init MC - No";
                    // }));
                  }
                  else if (StartAddress == 11)
                  {
                    AckSend(Command, 0, StartAddress, 0);       // return Ack OK
                    Mcinitialized = ComReadBuffer[11];
                    // this.Invoke(new Action(delegate ()
                    // {
                    //   if (Mcinitialized != 0)
                    //   {
                    //     btMcInit.Text = @"Init MC - Yes";
                    //   }
                    //   else
                    //   {
                    //     btMcInit.Text = @"Init MC - No";
                    //   }
                    // }));
                  }
                  break;
                case 3:// Pc <- Mc, Cyclic
                  TqSensorValue = (ushort)((ComReadBuffer[13] << 8) | ComReadBuffer[12]);

                  TqSensorOffsetValue = (ushort)((ComReadBuffer[15] << 8) | ComReadBuffer[14]);
                  Mc.DriverInfo.u16TorqueOffset = TqSensorOffsetValue;

                  Error = (ushort)((ComReadBuffer[29] << 8) | ComReadBuffer[28]);
                  // tbError.Text = Error.ToString();//ui
                  McVar.IniStep = ComReadBuffer[39];
                  MaintCnt = (uint)((ComReadBuffer[51] << 24) | (ComReadBuffer[50] << 16) | (ComReadBuffer[49] << 8) | ComReadBuffer[48]);
                  // tbMaintCnt.Text = MaintCnt.ToString();//ui
                  Enc = (ushort)((ComReadBuffer[41] << 8) | ComReadBuffer[40]);
                  // tbEnc.Text = Enc.ToString();//ui

                  MotorState = ((ComReadBuffer[27] << 8) | ComReadBuffer[26]) != 0;
                  Mc.McFlag.b1Run = ComReadBuffer[26];
                  Mc.McFlag.b1ControlFL = ComReadBuffer[30];

                  if (ComReadBuffer[42] != 0)
                    Mc.AutoSetting.FlagSetting = true;
                  else
                    Mc.AutoSetting.FlagSetting = false;

                  if (ComReadBuffer[43] != 0)
                    Mc.AutoSetting.FlagStart = true;
                  else
                    Mc.AutoSetting.FlagStart = false;

                  byte b1Run = (byte)(ComReadBuffer[44] & 0x01);
                  if (FlagRun[0] != b1Run)
                  {
                    MakeAndSendData(2, 2, b1Run);
                  }
                  // FlagRun[4] = FlagRun[3];
                  // FlagRun[3] = FlagRun[2];
                  FlagRun[2] = FlagRun[1];
                  FlagRun[1] = FlagRun[0];
                  FlagRun[0] = (byte)(ComReadBuffer[44] & 0x01);

                  byte b1ControlFL = (byte)(ComReadBuffer[44] & 0x02);
                  if (FlagFL[0] != b1ControlFL)
                  {
                    if (b1ControlFL != 0)
                      MakeAndSendData(2, 1, 1);
                    else
                      MakeAndSendData(2, 1, 0);
                  }
                  // FlagFL[4] = FlagFL[3];
                  // FlagFL[3] = FlagFL[2];
                  FlagFL[2] = FlagFL[1];
                  FlagFL[1] = FlagFL[0];
                  FlagFL[0] = b1ControlFL;

                  if (ComReadBuffer[63] != 0)
                    Mot_or_Nut = true;
                  else
                    Mot_or_Nut = false;

                  break;
                case 4:
                  if (StartAddress == 1)
                  {
                    AckSend(Command, 0, StartAddress, 0);       // return Ack OK
                  }
                  graph_count++;
                  fresh_graph_data();
                  break;
                case 5:
                  if (StartAddress == 1)
                  {
                    AckSend(Command, 0, StartAddress, 0);       // return Ack OK
                  }
                  Mc.AutoSetting.CurrentSpeed = (ushort)((ComReadBuffer[119] << 8) | ComReadBuffer[118]);
                  Mc.AutoSetting.CurrentSeatingPoint = (ushort)((ComReadBuffer[121] << 8) | ComReadBuffer[120]);
                  Mc.AutoSetting.CurrentFSpeed = (ushort)((ComReadBuffer[123] << 8) | ComReadBuffer[122]);
                  Mc.AutoSetting.CurrentFAngle = (ushort)((ComReadBuffer[125] << 8) | ComReadBuffer[124]);
                  break;
                case 6:
                  break;
                case 7:
                  // if (StartAddress == 1)//download Driver info
                  if (StartAddress == 2)//upload Driver info
                  {
                    AckSend(Command, 0, StartAddress, 0);       // return Ack OK
                    Mc.inDriverInfo.u16Type = (ushort)((ComReadBuffer[11] << 8) | ComReadBuffer[10]);
                    Mc.inDriverInfo.u16Version = (ushort)((ComReadBuffer[13] << 8) | ComReadBuffer[12]);
                    Mc.inDriverInfo.u16Serial_low = (ushort)((ComReadBuffer[15] << 8) | ComReadBuffer[14]);
                    Mc.inDriverInfo.u16Serial_high = (ushort)((ComReadBuffer[17] << 8) | ComReadBuffer[16]);
                    Mc.inDriverInfo.u8Factory_Gear_efficiency = (ushort)((ComReadBuffer[19] << 8) | ComReadBuffer[18]);
                    Mc.inDriverInfo.u8User_Gear_efficiency = (ushort)((ComReadBuffer[21] << 8) | ComReadBuffer[20]);
                    Mc.inDriverInfo.u16DriverVendor = (ushort)((ComReadBuffer[23] << 8) | ComReadBuffer[22]);
                    McVar.DriverInfoIsReady = true;
                    if (McVar.IniStep != 11)
                      MakeAndSendData(1, 3, 0);
                  }
                  else if (StartAddress == 3)//Speaker On/Off
                  { }
                  else if (StartAddress == 4)//Led band
                  { }
                  // else if (StartAddress == 5)//Reserved
                  // else if (StartAddress == 6)//Reserved
                  // else if (StartAddress == 7)//Reserved
                  else if (StartAddress == 8)//reset maintenance
                  { }
                  // else if (StartAddress == 9)//Reserved
                  else if (StartAddress == 10)//Check Torque offset value
                  { }
                  else if (StartAddress == 11)//Save Torque offset value
                  { }
                  else if (StartAddress == 12)//Start/Stop Initail Angle
                  { }
                  else if (StartAddress == 13)// receive initial angle result Pc <- Mc
                  {
                    CalibResultState = (int)((ComReadBuffer[11] << 11) | ComReadBuffer[10]);
                    AckSend(Command, 0, StartAddress, 0);       // return Ack OK
                  }
                  // else if (StartAddress == 13)// Pc -> Mc
                  else if (StartAddress == 101)// Pc <- Mc
                  {
                    int CalibStepState1 = (int)((ComReadBuffer[11] << 8) | ComReadBuffer[10]);
                    if (CalibStepState1 == 0)
                      CalibStepState = 0;
                    else if (CalibStepState1 == 1)
                      CalibStepState = 1;
                    else if (CalibStepState1 == 2 || CalibStepState1 == 3)
                      CalibStepState = 2;
                    else if (CalibStepState1 == 4 || CalibStepState1 == 5)
                      CalibStepState = 3;
                    else
                      CalibStepState = 4;
                    AckSend(Command, Try_num, StartAddress, 0);       // return Ack OK
                  }
                  break;
                case 104:
                  // get value
                  // MotorState = ((ComReadBuffer[3] << 8) | ComReadBuffer[4]) != 0;
                  if (StartAddress == 1)// Pc -> Mc
                  {

                  }
                  else if (StartAddress == 2)// Pc <- Mc
                  {
                    MotorState = ((ComReadBuffer[11] << 8) | ComReadBuffer[10]) != 0;
                    // CalibStepState = ((ComReadBuffer[11] << 8) | ComReadBuffer[10]);
                    // CalibResultState = ((ComReadBuffer[11] << 8) | ComReadBuffer[10]);
                  }
                  break;
                case 106:
                  break;
                default:
                  break;
              }
            }
            else
            {
              // AckSend(Command, Try_num, StartAddress, 2);       // return check CRC error
            }
          }
        }
        else if (((ComReadIndex > 0) && (ComReadBuffer[0] != 0x5A))  // check packet error
            || ((ComReadIndex > 1) && (ComReadBuffer[1] != 0xA5)))  // check packet error
        {
          ComReadIndex = 0;// no return Ack
        }
      }
    }
    // void ResetAckState(ref Mc)
    // {
    //   Mc.CmdAck.u8Command = 0;
    //   Mc.CmdAck.u8AckWait = OFF;
    //   Mc.CmdAck.u16StartAddress = 0;
    //   Mc.CmdAck.u16PtrCnt = 0;
    // }
    // send ack code
    private void AckSend(byte command, byte Try_num, ushort StartAddress, byte code)
    {
      ushort u16PtrCnt = 0, calc_crc;
      byte[] DataPacket = new byte[20];

      DataPacket[u16PtrCnt++] = 0x5A;    // Start low
      DataPacket[u16PtrCnt++] = 0xA5;     // Start high
      DataPacket[u16PtrCnt++] = 0;              // Length low
      DataPacket[u16PtrCnt++] = 0;            // Length high
      if (code != 0)
        DataPacket[u16PtrCnt++] = (byte)(0x80 | command);         // Function code
      else
        DataPacket[u16PtrCnt++] = command;        // Function code
      DataPacket[u16PtrCnt++] = 0;            // revision low, 1byte
      DataPacket[u16PtrCnt++] = 0;            // revision high, 1byte
      DataPacket[u16PtrCnt++] = Try_num; // u8LcdMcComReadBuffer[7];		  // Try num.
      DataPacket[u16PtrCnt++] = (byte)(StartAddress);     // Start Address low
      DataPacket[u16PtrCnt++] = (byte)(StartAddress >> 8);      // Start Address high
      DataPacket[u16PtrCnt++] = code;     // return ack code
      DataPacket[u16PtrCnt++] = 0;            // 
      DataPacket[u16PtrCnt++] = 0;            // reserved
      DataPacket[u16PtrCnt++] = 0;            // reserved

      ushort Length = (ushort)(u16PtrCnt - 4);
      DataPacket[_LengthLow] = (byte)(Length);      // Length low
      DataPacket[_LengthHigh] = (byte)(Length >> 8);    // Length high

      calc_crc = GetCRC(DataPacket, u16PtrCnt + 2);
      DataPacket[u16PtrCnt++] = (byte)(calc_crc & 0xff);
      DataPacket[u16PtrCnt++] = (byte)((calc_crc >> 8) & 0xff);

      // SerialPuts_Pc((uint16_t)u16PtrCnt, (uint8_t*)DataPacket);
      SendPacket(DataPacket, u16PtrCnt);
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
    void clear_graph_data()
    {
      Graph_ch1.Clear();
      Graph_ch2.Clear();
      Graph_ch3.Clear();
      Graph_ch4.Clear();
      Graph_ch5.Clear();
      Graph_ch6.Clear();
      Graph_ch7.Clear();
      Graph_ch8.Clear();
    }
    void clear_data()
    {
      Data_ch1.Clear();
      Data_ch2.Clear();
      Data_ch3.Clear();
      Data_ch4.Clear();
      Data_ch5.Clear();
      Data_ch6.Clear();
      Data_ch7.Clear();
      Data_ch8.Clear();
    }

    //[Obsolete]
    public void Refresh_graph()
    {
      // tbDataCount.Text = Data_ch1.Count.ToString();
      // tbGraphDataCount.Text = Graph_ch1.Count.ToString();
      List<double> Graph_time = new List<double>();
      // Graph_time.Clear();
      for (int i = 0; i < Graph_ch1.Count; i++)
        Graph_time.Add(5e-3d * (double)i);

      formsPlot1.Plot.Clear();
      if (cbGraph_ch1.Checked)
      {
        var sig1 = formsPlot1.Plot.Add.ScatterLine(Graph_time, Graph_ch1);
        sig1.LegendText = "Torque";
      }
      if (cbGraph_ch2.Checked)
      {
        var sig2 = formsPlot1.Plot.Add.ScatterLine(Graph_time, Graph_ch2);
        sig2.LegendText = "Current";
      }
      if (cbGraph_ch3.Checked)
      {
        var sig3 = formsPlot1.Plot.Add.ScatterLine(Graph_time, Graph_ch3);
        sig3.LegendText = "Speed";
      }
      if (cbGraph_ch4.Checked)
      {
        var sig4 = formsPlot1.Plot.Add.ScatterLine(Graph_time, Graph_ch4);
        sig4.LegendText = "Angle";
      }
      if (cbGraph_ch5.Checked)
      {
        var sig5 = formsPlot1.Plot.Add.ScatterLine(Graph_time, Graph_ch5);
        sig5.LegendText = "Speed Command";
      }
      if (cbGraph_ch6.Checked)
      {
        var sig6 = formsPlot1.Plot.Add.ScatterLine(Graph_time, Graph_ch6);
        sig6.LegendText = "Current Command";
      }
      if (cbGraph_ch7.Checked)
      {
        var sig7 = formsPlot1.Plot.Add.ScatterLine(Graph_time, Graph_ch7);
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
    void fresh_graph_data()
    {

      TestUnion d = new TestUnion();
      d.b0 = ComReadBuffer[766 + 0];
      d.b1 = ComReadBuffer[766 + 1];
      d.b2 = ComReadBuffer[766 + 2];
      d.b3 = ComReadBuffer[766 + 3];
      float hss_gain = d.f;
      d.b0 = ComReadBuffer[770 + 0];
      d.b1 = ComReadBuffer[770 + 1];
      d.b2 = ComReadBuffer[770 + 2];
      d.b3 = ComReadBuffer[770 + 3];
      float tq_gain = d.f;
      clear_data();
      // Graph number
      d.b0 = ComReadBuffer[10];
      d.b1 = ComReadBuffer[11];
      if (d.us0 == 1)//start run
      {
        clear_graph_data();
      }
      d.b0 = ComReadBuffer[12];
      d.b1 = ComReadBuffer[13];
      ushort Graph_Data_Length = d.us0;
      for (ushort j = 0; j < Graph_Data_Length; j++)
      {
        d.b0 = ComReadBuffer[100 * 0 + 66 + j * 2 + 0];
        d.b1 = ComReadBuffer[100 * 0 + 66 + j * 2 + 1];
        Data_ch1.Add(d.s0 * tq_gain);//torque
        d.b0 = ComReadBuffer[100 * 1 + 66 + j * 2 + 0];
        d.b1 = ComReadBuffer[100 * 1 + 66 + j * 2 + 1];
        Data_ch2.Add(d.s0 * hss_gain);//current
        d.b0 = ComReadBuffer[100 * 2 + 66 + j * 2 + 0];
        d.b1 = ComReadBuffer[100 * 2 + 66 + j * 2 + 1];
        Data_ch3.Add(d.s0 * 2.0);//speed
        d.b0 = ComReadBuffer[100 * 3 + 66 + j * 2 + 0];
        d.b1 = ComReadBuffer[100 * 3 + 66 + j * 2 + 1];
        Data_ch4.Add(d.s0);//angle
        d.b0 = ComReadBuffer[100 * 4 + 66 + j * 2 + 0];
        d.b1 = ComReadBuffer[100 * 4 + 66 + j * 2 + 1];
        Data_ch5.Add(d.s0 * 2.0);//speed command
        d.b0 = ComReadBuffer[100 * 5 + 66 + j * 2 + 0];
        d.b1 = ComReadBuffer[100 * 5 + 66 + j * 2 + 1];
        Data_ch6.Add(d.s0 * hss_gain);//current command
        d.b0 = ComReadBuffer[100 * 6 + 66 + j * 2 + 0];
        d.b1 = ComReadBuffer[100 * 6 + 66 + j * 2 + 1];
        Data_ch7.Add(d.s0);
      }
      Graph_ch1.AddRange(Data_ch1);
      Graph_ch2.AddRange(Data_ch2);
      Graph_ch3.AddRange(Data_ch3);
      Graph_ch4.AddRange(Data_ch4);
      Graph_ch5.AddRange(Data_ch5);
      Graph_ch6.AddRange(Data_ch6);
      Graph_ch7.AddRange(Data_ch7);

      refresh_graph_flag = true;
      // this.Invoke(new Action(delegate () // this == Form 이다. Form이 아닌 컨트롤의 Invoke를 직접호출해도 무방하다.
      // {
      //   //Invoke를 통해 lbl_Result 컨트롤에 결과값을 업데이트한다.
      //   Refresh_graph();
      // }));
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
          sw.WriteLine(Graph_ch1.Count());
          for (int i = 0; i < Graph_ch1.Count; i++)
          {
            sw.WriteLine(Graph_ch1[i].ToString());
            sw.WriteLine(Graph_ch2[i].ToString());
            sw.WriteLine(Graph_ch3[i].ToString());
            sw.WriteLine(Graph_ch4[i].ToString());
            sw.WriteLine(Graph_ch5[i].ToString());
            sw.WriteLine(Graph_ch6[i].ToString());
            sw.WriteLine(Graph_ch7[i].ToString());
          }
          sw.Close();
        }
        else
        {
          StreamWriter sw = new StreamWriter("GraphData.txt");
          sw.WriteLine(Graph_ch1.Count());
          for (int i = 0; i < Graph_ch1.Count; i++)
          {
            sw.WriteLine(Graph_ch1[i].ToString());
            sw.WriteLine(Graph_ch2[i].ToString());
            sw.WriteLine(Graph_ch3[i].ToString());
            sw.WriteLine(Graph_ch4[i].ToString());
            sw.WriteLine(Graph_ch5[i].ToString());
            sw.WriteLine(Graph_ch6[i].ToString());
            sw.WriteLine(Graph_ch7[i].ToString());
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
          clear_graph_data();
          for (int i = 0; i < Count; i++)
          {
            Graph_ch1.Add(Convert.ToDouble(sr.ReadLine()));
            Graph_ch2.Add(Convert.ToDouble(sr.ReadLine()));
            Graph_ch3.Add(Convert.ToDouble(sr.ReadLine()));
            Graph_ch4.Add(Convert.ToDouble(sr.ReadLine()));
            Graph_ch5.Add(Convert.ToDouble(sr.ReadLine()));
            Graph_ch6.Add(Convert.ToDouble(sr.ReadLine()));
            Graph_ch7.Add(Convert.ToDouble(sr.ReadLine()));
          }
          sr.Close();
        }
        else
        {
          StreamReader sr = new StreamReader("GraphData.txt");
          int Count = Convert.ToInt32(sr.ReadLine());
          clear_graph_data();
          for (int i = 0; i < Count; i++)
          {
            Graph_ch1.Add(Convert.ToDouble(sr.ReadLine()));
            Graph_ch2.Add(Convert.ToDouble(sr.ReadLine()));
            Graph_ch3.Add(Convert.ToDouble(sr.ReadLine()));
            Graph_ch4.Add(Convert.ToDouble(sr.ReadLine()));
            Graph_ch5.Add(Convert.ToDouble(sr.ReadLine()));
            Graph_ch6.Add(Convert.ToDouble(sr.ReadLine()));
            Graph_ch7.Add(Convert.ToDouble(sr.ReadLine()));
          }
          sr.Close();
        }
        // refresh_graph_flag = true;
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
      if (!Port.IsOpen)
        return;
      Mc.DriverInfo.u16Type = (ushort)UInt16.Parse(nudDriverType.Text);
      Mc.DriverInfo.u16Version = (ushort)UInt16.Parse(nudDriverVersion.Text);
      Mc.DriverInfo.u8Factory_Gear_efficiency = (ushort)UInt16.Parse(nudDriverGearEfficiency.Text);
      Mc.DriverInfo.u8User_Gear_efficiency = (ushort)UInt16.Parse(nudDriverUserEfficiency.Text);
      uint SerialNum = UInt32.Parse(nudDriverSerial.Text);
      Mc.DriverInfo.u16Serial_low = (ushort)(SerialNum >> 0);
      Mc.DriverInfo.u16Serial_high = (ushort)(SerialNum >> 16);
      Mc.DriverInfo.u16DriverVendor = (ushort)UInt16.Parse(nudDriverVendor.Text);
      MakeAndSendData(7, 1, 0);
    }

    private void GetDriverInfo(object sender, EventArgs e)
    {
      MakeAndSendData(7, 5, 0);
    }
    private void ShowDriverInfo()
    {
      nudDriverType.Text = Mc.inDriverInfo.u16Type.ToString();
      nudDriverVersion.Text = Mc.inDriverInfo.u16Version.ToString();
      nudDriverGearEfficiency.Text = Mc.inDriverInfo.u8Factory_Gear_efficiency.ToString();
      nudDriverUserEfficiency.Text = Mc.inDriverInfo.u8User_Gear_efficiency.ToString();
      uint SerialNum = (uint)((Mc.inDriverInfo.u16Serial_high << 16) + Mc.inDriverInfo.u16Serial_low);
      nudDriverSerial.Text = SerialNum.ToString();
      nudDriverVendor.Text = Mc.inDriverInfo.u16DriverVendor.ToString();
      McVar.DriverInfoIsReady = false;
    }

    private void rbSoftStopOff_CheckedChanged(object sender, EventArgs e)
    {
      McVar.SoftStop = 0;
    }

    private void rbSoftStopOn_CheckedChanged(object sender, EventArgs e)
    {
      McVar.SoftStop = 1;
    }

    private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    private void btTqSensorOffset_Click(object sender, EventArgs e)
    {
      if (!Port.IsOpen)
        return;

      if (sender == btCheckTqSensorOffset)
      {
        MakeAndSendData(7, 10, 0);
        btSaveTqSensorOffset.Enabled = true;
      }
      else if (sender == btSaveTqSensorOffset)
      {
        MakeAndSendData(7, 11, 0);
      }
    }

  }
}
