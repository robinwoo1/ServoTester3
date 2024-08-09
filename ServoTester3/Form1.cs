using ScottPlot;
using ScottPlot.Plottables;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Text;
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
    public bool Mot_or_Nut = false;
    Thread myThread;// = new Thread(myFunc);
    public bool myThread_flag = false;
    public int graph_count = 0;
    ushort TqSensorValue = 0;
    ushort TqOffsetValue = 0;
    ushort Error = 0;
    uint MaintCnt = 0;
    ushort Enc = 0;
    ushort Mcinitialized = 0;
    public Form1()
    {
      InitializeComponent();

    }
    List<double> Graph_time = new List<double>();
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
    public ConcurrentQueue<byte> graph_cq = new ConcurrentQueue<byte>();
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
    public int rbuf_put(byte[] rbuf, ushort rsize)
    {
      ushort nhead;
      ushort datatocopy;
      ushort curPos = RecvBuf.head; // Update the last position before copying new data

      if (curPos + rsize > SERIAL_BUF_SIZE)
      {
        datatocopy = (ushort)(SERIAL_BUF_SIZE - curPos); // find out how much space is left in the main buffer
        if (RecvBuf.tail > RecvBuf.head)
        {
          return 0;
        }
        else if ((RecvBuf.tail < RecvBuf.head) && (RecvBuf.tail <= (rsize - datatocopy)))
        {
          return 0;
        }
        // memcpy((uint8_t *)rb->data+curPos, rbuf, datatocopy); // copy data in that remaining space
        Array.Copy(rbuf, 0, RecvBuf.data, curPos, datatocopy); // copy data in that remaining space
        curPos = 0; // point to the start of the buffer
                    // memcpy((uint8_t *)rb->data, (uint8_t *)rbuf+datatocopy, (rsize-datatocopy)); // copy the remaining data
        Array.Copy(rbuf, datatocopy, RecvBuf.data, curPos, rsize - datatocopy); // copy the remaining data
        RecvBuf.head = (ushort)(rsize - datatocopy); // update the position
      }
      else
      {
        nhead = (ushort)(RecvBuf.head + rsize);
        if ((RecvBuf.tail > RecvBuf.head) && (RecvBuf.tail <= nhead))
        {
          return 0;
        }
        // rbuf.CopyTo(RecvBuf.data, curPos);
        Array.Copy(rbuf, 0, RecvBuf.data, curPos, rbuf.Count());
        RecvBuf.head = (ushort)((rsize + curPos) & (SERIAL_BUF_SIZE - 1));
      }

      return 1;
    }
    public byte rb_get(byte[] err)
    {
      byte d;
      ushort ntail = (ushort)((RecvBuf.tail + 1) & (SERIAL_BUF_SIZE - 1));
      if (RecvBuf.head == RecvBuf.tail)
      {
        err[0] = 1;
        return 0;
      }
      d = RecvBuf.data[RecvBuf.tail];
      RecvBuf.data[RecvBuf.tail] = 0;
      RecvBuf.tail = ntail;
      return d;
    }
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
          MakePacket(Command, StartAddress, Data);
          u16PtrCnt = CmdAck.u16PtrCnt;
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
          MakePacket(Command, StartAddress, Data);
          u16PtrCnt = CmdAck.u16PtrCnt;
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
          MakePacket(Command, StartAddress, Data);
          u16PtrCnt = CmdAck.u16PtrCnt;
          calc_crc = GetCRC(SendDataPacket, u16PtrCnt + 2);
          SendDataPacket[u16PtrCnt++] = (byte)(calc_crc >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(calc_crc >> 8);
          SendPacket(SendDataPacket, u16PtrCnt);
        }
        // else if (StartAddress == 2)// Sync state out PC<-MC
        else if (StartAddress == 3)// Sync resume
        {
          MakePacket(Command, StartAddress, Data);
          u16PtrCnt = CmdAck.u16PtrCnt;
          calc_crc = GetCRC(SendDataPacket, u16PtrCnt + 2);
          SendDataPacket[u16PtrCnt++] = (byte)(calc_crc >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(calc_crc >> 8);
          SendPacket(SendDataPacket, u16PtrCnt);
        }
        else if (StartAddress == 4)// Sync in event update
        {
          MakePacket(Command, StartAddress, Data);
          u16PtrCnt = CmdAck.u16PtrCnt;
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
          MakePacket(Command, StartAddress, Data);
          u16PtrCnt = CmdAck.u16PtrCnt;
          calc_crc = GetCRC(SendDataPacket, u16PtrCnt + 2);
          SendDataPacket[u16PtrCnt++] = (byte)(calc_crc >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(calc_crc >> 8);
          SendPacket(SendDataPacket, u16PtrCnt);
        }
        // else if (StartAddress == 2)//upload driver info
        else if (StartAddress == 3)//Speaker & Output
        {
          MakePacket(Command, StartAddress, Data);
          u16PtrCnt = CmdAck.u16PtrCnt;
          calc_crc = GetCRC(SendDataPacket, u16PtrCnt + 2);
          SendDataPacket[u16PtrCnt++] = (byte)(calc_crc >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(calc_crc >> 8);
          SendPacket(SendDataPacket, u16PtrCnt);
        }
        else if (StartAddress == 4)//LED band & output
        {
          MakePacket(Command, StartAddress, Data);
          u16PtrCnt = CmdAck.u16PtrCnt;
          calc_crc = GetCRC(SendDataPacket, u16PtrCnt + 2);
          SendDataPacket[u16PtrCnt++] = (byte)(calc_crc >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(calc_crc >> 8);
          SendPacket(SendDataPacket, u16PtrCnt);
        }
        // else if (StartAddress == 5)//
        // else if (StartAddress == 6)//
        // else if (StartAddress == 7)//
        else if (StartAddress == 8)//reset Maintenance count
        {
          MakePacket(Command, StartAddress, Data);
          u16PtrCnt = CmdAck.u16PtrCnt;
          calc_crc = GetCRC(SendDataPacket, u16PtrCnt + 2);
          SendDataPacket[u16PtrCnt++] = (byte)(calc_crc >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(calc_crc >> 8);
          SendPacket(SendDataPacket, u16PtrCnt);
        }
        // else if (StartAddress == 9)//
        else if (StartAddress == 10)//Check torque Offset
        {
          MakePacket(Command, StartAddress, Data);
          u16PtrCnt = CmdAck.u16PtrCnt;
          calc_crc = GetCRC(SendDataPacket, u16PtrCnt + 2);
          SendDataPacket[u16PtrCnt++] = (byte)(calc_crc >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(calc_crc >> 8);
          SendPacket(SendDataPacket, u16PtrCnt);
        }
        else if (StartAddress == 11)//Save torque Offset
        {
          MakePacket(Command, StartAddress, Data);
          u16PtrCnt = CmdAck.u16PtrCnt;
          calc_crc = GetCRC(SendDataPacket, u16PtrCnt + 2);
          SendDataPacket[u16PtrCnt++] = (byte)(calc_crc >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(calc_crc >> 8);
          SendPacket(SendDataPacket, u16PtrCnt);
        }
        else if (StartAddress == 12)//Start Initail Angle
        {
          MakePacket(Command, StartAddress, Data);
          u16PtrCnt = CmdAck.u16PtrCnt;
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
          u16PtrCnt = CmdAck.u16PtrCnt;
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
          MakePacket(Command, StartAddress, Data);
          u16PtrCnt = CmdAck.u16PtrCnt;
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
          u16PtrCnt = CmdAck.u16PtrCnt;
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
          u16PtrCnt = CmdAck.u16PtrCnt;
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

      if (Command == 1)
      {
        if ((StartAddress == 1) || (StartAddress == 2))
        {
          SendDataPacket[u16PtrCnt++] = (byte)(Mc_Para.val.u16MC_SCREW_TYPE >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc_Para.val.u16MC_SCREW_TYPE >> 8);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc_Para.val.u16MC_TCAM_ACTM >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc_Para.val.u16MC_TCAM_ACTM >> 8);
          d.f = Mc_Para.val.f32MC_FASTEN_TORQUE;
          SendDataPacket[u16PtrCnt++] = d.b0;
          SendDataPacket[u16PtrCnt++] = d.b1;
          SendDataPacket[u16PtrCnt++] = d.b2;
          SendDataPacket[u16PtrCnt++] = d.b3;
          d.f = Mc_Para.val.f32MC_TORQUE_MIN_MAX;
          SendDataPacket[u16PtrCnt++] = d.b0;
          SendDataPacket[u16PtrCnt++] = d.b1;
          SendDataPacket[u16PtrCnt++] = d.b2;
          SendDataPacket[u16PtrCnt++] = d.b3;
          SendDataPacket[u16PtrCnt++] = (byte)(Mc_Para.val.u16MC_TARGET_ANGLE >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc_Para.val.u16MC_TARGET_ANGLE >> 8);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc_Para.val.u16MC_FASTEN_MIN_ANGLE >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc_Para.val.u16MC_FASTEN_MIN_ANGLE >> 8);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc_Para.val.u16MC_FASTEN_MAX_ANGLE >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc_Para.val.u16MC_FASTEN_MAX_ANGLE >> 8);
          d.f = Mc_Para.val.f32MC_SNUG_TORQUE;
          SendDataPacket[u16PtrCnt++] = d.b0;
          SendDataPacket[u16PtrCnt++] = d.b1;
          SendDataPacket[u16PtrCnt++] = d.b2;
          SendDataPacket[u16PtrCnt++] = d.b3;
          SendDataPacket[u16PtrCnt++] = (byte)(Mc_Para.val.u16MC_FASTEN_SPEED >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc_Para.val.u16MC_FASTEN_SPEED >> 8);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc_Para.val.u16MC_FREE_FASTEN_ANGLE >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc_Para.val.u16MC_FREE_FASTEN_ANGLE >> 8);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc_Para.val.u16MC_FREE_FASTEN_SPEED >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc_Para.val.u16MC_FREE_FASTEN_SPEED >> 8);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc_Para.val.u16MC_SOFT_START >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc_Para.val.u16MC_SOFT_START >> 8);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc_Para.val.u16MC_FASTEN_SEATTING_POINT_RATE >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc_Para.val.u16MC_FASTEN_SEATTING_POINT_RATE >> 8);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc_Para.val.u16MC_FASTEN_TQ_RISING_TIME >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc_Para.val.u16MC_FASTEN_TQ_RISING_TIME >> 8);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc_Para.val.u16MC_RAMP_UP_SPEED >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc_Para.val.u16MC_RAMP_UP_SPEED >> 8);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc_Para.val.u16MC_TORQUE_COMPENSATION >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc_Para.val.u16MC_TORQUE_COMPENSATION >> 8);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc_Para.val.u16MC_TORQUE_OFFSET >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc_Para.val.u16MC_TORQUE_OFFSET >> 8);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc_Para.val.u16MC_MAX_PULSE_COUNT >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc_Para.val.u16MC_MAX_PULSE_COUNT >> 8);

          SendDataPacket[u16PtrCnt++] = (byte)(Mc_Para.val.u16MC_ADVANCED_MODE >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc_Para.val.u16MC_ADVANCED_MODE >> 8);
          d.f = Mc_Para.val.f32MC_ADVANCED_PARA1;
          SendDataPacket[u16PtrCnt++] = d.b0;
          SendDataPacket[u16PtrCnt++] = d.b1;
          SendDataPacket[u16PtrCnt++] = d.b2;
          SendDataPacket[u16PtrCnt++] = d.b3;
          d.f = Mc_Para.val.f32MC_ADVANCED_PARA2;
          SendDataPacket[u16PtrCnt++] = d.b0;
          SendDataPacket[u16PtrCnt++] = d.b1;
          SendDataPacket[u16PtrCnt++] = d.b2;
          SendDataPacket[u16PtrCnt++] = d.b3;
          d.f = Mc_Para.val.f32MC_ADVANCED_PARA3;
          SendDataPacket[u16PtrCnt++] = d.b0;
          SendDataPacket[u16PtrCnt++] = d.b1;
          SendDataPacket[u16PtrCnt++] = d.b2;
          SendDataPacket[u16PtrCnt++] = d.b3;
          d.f = Mc_Para.val.f32MC_ADVANCED_PARA4;
          SendDataPacket[u16PtrCnt++] = d.b0;
          SendDataPacket[u16PtrCnt++] = d.b1;
          SendDataPacket[u16PtrCnt++] = d.b2;
          SendDataPacket[u16PtrCnt++] = d.b3;
          d.f = Mc_Para.val.f32MC_ADVANCED_PARA5;
          SendDataPacket[u16PtrCnt++] = d.b0;
          SendDataPacket[u16PtrCnt++] = d.b1;
          SendDataPacket[u16PtrCnt++] = d.b2;
          SendDataPacket[u16PtrCnt++] = d.b3;
          d.f = Mc_Para.val.f32MC_ADVANCED_PARA6;
          SendDataPacket[u16PtrCnt++] = d.b0;
          SendDataPacket[u16PtrCnt++] = d.b1;
          SendDataPacket[u16PtrCnt++] = d.b2;
          SendDataPacket[u16PtrCnt++] = d.b3;
          d.f = Mc_Para.val.f32MC_ADVANCED_PARA7;
          SendDataPacket[u16PtrCnt++] = d.b0;
          SendDataPacket[u16PtrCnt++] = d.b1;
          SendDataPacket[u16PtrCnt++] = d.b2;
          SendDataPacket[u16PtrCnt++] = d.b3;
          d.f = Mc_Para.val.f32MC_ADVANCED_PARA8;
          SendDataPacket[u16PtrCnt++] = d.b0;
          SendDataPacket[u16PtrCnt++] = d.b1;
          SendDataPacket[u16PtrCnt++] = d.b2;
          SendDataPacket[u16PtrCnt++] = d.b3;
          d.f = Mc_Para.val.f32MC_ADVANCED_PARA9;
          SendDataPacket[u16PtrCnt++] = d.b0;
          SendDataPacket[u16PtrCnt++] = d.b1;
          SendDataPacket[u16PtrCnt++] = d.b2;
          SendDataPacket[u16PtrCnt++] = d.b3;
          d.f = Mc_Para.val.f32MC_ADVANCED_PARA10;
          SendDataPacket[u16PtrCnt++] = d.b0;
          SendDataPacket[u16PtrCnt++] = d.b1;
          SendDataPacket[u16PtrCnt++] = d.b2;
          SendDataPacket[u16PtrCnt++] = d.b3;
          d.f = Mc_Para.val.f32MC_ADVANCED_PARA11;
          SendDataPacket[u16PtrCnt++] = d.b0;
          SendDataPacket[u16PtrCnt++] = d.b1;
          SendDataPacket[u16PtrCnt++] = d.b2;
          SendDataPacket[u16PtrCnt++] = d.b3;
          d.f = Mc_Para.val.f32MC_ADVANCED_PARA12;
          SendDataPacket[u16PtrCnt++] = d.b0;
          SendDataPacket[u16PtrCnt++] = d.b1;
          SendDataPacket[u16PtrCnt++] = d.b2;
          SendDataPacket[u16PtrCnt++] = d.b3;
          d.f = Mc_Para.val.f32MC_ADVANCED_PARA13;
          SendDataPacket[u16PtrCnt++] = d.b0;
          SendDataPacket[u16PtrCnt++] = d.b1;
          SendDataPacket[u16PtrCnt++] = d.b2;
          SendDataPacket[u16PtrCnt++] = d.b3;
          d.f = Mc_Para.val.f32MC_ADVANCED_PARA14;
          SendDataPacket[u16PtrCnt++] = d.b0;
          SendDataPacket[u16PtrCnt++] = d.b1;
          SendDataPacket[u16PtrCnt++] = d.b2;
          SendDataPacket[u16PtrCnt++] = d.b3;
          d.f = Mc_Para.val.f32MC_ADVANCED_PARA15;
          SendDataPacket[u16PtrCnt++] = d.b0;
          SendDataPacket[u16PtrCnt++] = d.b1;
          SendDataPacket[u16PtrCnt++] = d.b2;
          SendDataPacket[u16PtrCnt++] = d.b3;
          d.f = Mc_Para.val.f32MC_ADVANCED_PARA16;
          SendDataPacket[u16PtrCnt++] = d.b0;
          SendDataPacket[u16PtrCnt++] = d.b1;
          SendDataPacket[u16PtrCnt++] = d.b2;
          SendDataPacket[u16PtrCnt++] = d.b3;
          d.f = Mc_Para.val.f32MC_ADVANCED_PARA17;
          SendDataPacket[u16PtrCnt++] = d.b0;
          SendDataPacket[u16PtrCnt++] = d.b1;
          SendDataPacket[u16PtrCnt++] = d.b2;
          SendDataPacket[u16PtrCnt++] = d.b3;
          d.f = Mc_Para.val.f32MC_ADVANCED_PARA18;
          SendDataPacket[u16PtrCnt++] = d.b0;
          SendDataPacket[u16PtrCnt++] = d.b1;
          SendDataPacket[u16PtrCnt++] = d.b2;
          SendDataPacket[u16PtrCnt++] = d.b3;
          d.f = Mc_Para.val.f32MC_ADVANCED_PARA19;
          SendDataPacket[u16PtrCnt++] = d.b0;
          SendDataPacket[u16PtrCnt++] = d.b1;
          SendDataPacket[u16PtrCnt++] = d.b2;
          SendDataPacket[u16PtrCnt++] = d.b3;
          SendDataPacket[u16PtrCnt++] = (byte)(Mc_Para.val.u16MC_FREE_REVERSE_ROTATION_SPEED >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc_Para.val.u16MC_FREE_REVERSE_ROTATION_SPEED >> 8);
          d.f = Mc_Para.val.f32MC_FREE_REVERSE_ROTATION_ANGLE;
          SendDataPacket[u16PtrCnt++] = d.b0;
          SendDataPacket[u16PtrCnt++] = d.b1;
          SendDataPacket[u16PtrCnt++] = d.b2;
          SendDataPacket[u16PtrCnt++] = d.b3;
          SendDataPacket[u16PtrCnt++] = (byte)(Mc_Para.val.u16MC_REVERS_ANGLE_SETTING_SPEED >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc_Para.val.u16MC_REVERS_ANGLE_SETTING_SPEED >> 8);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc_Para.val.u16MC_REVERS_ANGLE_SETTING_ANGLE >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc_Para.val.u16MC_REVERS_ANGLE_SETTING_ANGLE >> 8);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc_Para.val.u16MC_REVERS_ANGLE_SETTING_FW_REV >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc_Para.val.u16MC_REVERS_ANGLE_SETTING_FW_REV >> 8);

          if (StartAddress == 1)
          {
            SendDataPacket[u16PtrCnt++] = (byte)(Mc_Para.val.u16MC_UNIT >> 0);
            SendDataPacket[u16PtrCnt++] = (byte)(Mc_Para.val.u16MC_UNIT >> 8);
            SendDataPacket[u16PtrCnt++] = (byte)(Mc_Para.val.u16MC_ACC_DEC_TIME >> 0);
            SendDataPacket[u16PtrCnt++] = (byte)(Mc_Para.val.u16MC_ACC_DEC_TIME >> 8);
            SendDataPacket[u16PtrCnt++] = (byte)(Mc_Para.val.u16MC_FASTEN_TORQUE_MAINTAIN_TIME >> 0);
            SendDataPacket[u16PtrCnt++] = (byte)(Mc_Para.val.u16MC_FASTEN_TORQUE_MAINTAIN_TIME >> 8);
            SendDataPacket[u16PtrCnt++] = (byte)(Mc_Para.val.u16MC_USE_MAXTQ_FOR_LOOSENING >> 0);
            SendDataPacket[u16PtrCnt++] = (byte)(Mc_Para.val.u16MC_USE_MAXTQ_FOR_LOOSENING >> 8);
            SendDataPacket[u16PtrCnt++] = (byte)(Mc_Para.val.u16MC_LOOSENING_SPEED >> 0);
            SendDataPacket[u16PtrCnt++] = (byte)(Mc_Para.val.u16MC_LOOSENING_SPEED >> 8);
            d.f = Mc_Para.val.f32MC_TOTAL_FASTENING_TIME;
            SendDataPacket[u16PtrCnt++] = d.b0;
            SendDataPacket[u16PtrCnt++] = d.b1;
            SendDataPacket[u16PtrCnt++] = d.b2;
            SendDataPacket[u16PtrCnt++] = d.b3;
            d.f = Mc_Para.val.f32MC_TOTAL_LOOSENING_TIME;
            SendDataPacket[u16PtrCnt++] = d.b0;
            SendDataPacket[u16PtrCnt++] = d.b1;
            SendDataPacket[u16PtrCnt++] = d.b2;
            SendDataPacket[u16PtrCnt++] = d.b3;
            d.f = Mc_Para.val.f32MC_STALL_LOOSENING_TIME_LIMIT;
            SendDataPacket[u16PtrCnt++] = d.b0;
            SendDataPacket[u16PtrCnt++] = d.b1;
            SendDataPacket[u16PtrCnt++] = d.b2;
            SendDataPacket[u16PtrCnt++] = d.b3;
            SendDataPacket[u16PtrCnt++] = (byte)(Mc_Para.val.u16MC_SCREW_TYPE >> 0);
            SendDataPacket[u16PtrCnt++] = (byte)(Mc_Para.val.u16MC_SCREW_TYPE >> 8);
            d.f = Mc_Para.val.f32MC_JUDGE_FASTEN_MIN_TURNS;
            SendDataPacket[u16PtrCnt++] = d.b0;
            SendDataPacket[u16PtrCnt++] = d.b1;
            SendDataPacket[u16PtrCnt++] = d.b2;
            SendDataPacket[u16PtrCnt++] = d.b3;
            SendDataPacket[u16PtrCnt++] = (byte)(Mc_Para.val.u16MC_FASTENING_STOP_ALARM >> 0);
            SendDataPacket[u16PtrCnt++] = (byte)(Mc_Para.val.u16MC_FASTENING_STOP_ALARM >> 8);
            SendDataPacket[u16PtrCnt++] = (byte)(Mc_Para.val.u16MC_TORQUE_COMPENSATION_MAIN >> 0);
            SendDataPacket[u16PtrCnt++] = (byte)(Mc_Para.val.u16MC_TORQUE_COMPENSATION_MAIN >> 8);
            SendDataPacket[u16PtrCnt++] = (byte)(Mc_Para.val.u16MC_CROWFOOT_ENABLE >> 0);
            SendDataPacket[u16PtrCnt++] = (byte)(Mc_Para.val.u16MC_CROWFOOT_ENABLE >> 8);
            d.f = Mc_Para.val.f32MC_CROWFOOT_RATIO;
            SendDataPacket[u16PtrCnt++] = d.b0;
            SendDataPacket[u16PtrCnt++] = d.b1;
            SendDataPacket[u16PtrCnt++] = d.b2;
            SendDataPacket[u16PtrCnt++] = d.b3;
            SendDataPacket[u16PtrCnt++] = (byte)(Mc_Para.val.u16MC_CROWFOOT_EFFICIENCY >> 0);
            SendDataPacket[u16PtrCnt++] = (byte)(Mc_Para.val.u16MC_CROWFOOT_EFFICIENCY >> 8);
            d.f = Mc_Para.val.f32MC_CROWFOOT_REVERSE_TORQUE;
            SendDataPacket[u16PtrCnt++] = d.b0;
            SendDataPacket[u16PtrCnt++] = d.b1;
            SendDataPacket[u16PtrCnt++] = d.b2;
            SendDataPacket[u16PtrCnt++] = d.b3;
            SendDataPacket[u16PtrCnt++] = (byte)(Mc_Para.val.u16MC_CROWFOOT_REVERSE_SPEED >> 0);
            SendDataPacket[u16PtrCnt++] = (byte)(Mc_Para.val.u16MC_CROWFOOT_REVERSE_SPEED >> 8);
            d.f = Mc_Para.val.f32MC_FREE_SPEED_MAX_TORQUE;
            SendDataPacket[u16PtrCnt++] = d.b0;
            SendDataPacket[u16PtrCnt++] = d.b1;
            SendDataPacket[u16PtrCnt++] = d.b2;
            SendDataPacket[u16PtrCnt++] = d.b3;
            // SendDataPacket[u16PtrCnt++] = (byte)(0);
            // SendDataPacket[u16PtrCnt++] = (byte)(0);
            // SendDataPacket[u16PtrCnt++] = (byte)(0);
            // SendDataPacket[u16PtrCnt++] = (byte)(0);
            SendDataPacket[u16PtrCnt++] = (byte)(0);
            SendDataPacket[u16PtrCnt++] = (byte)(0);
            SendDataPacket[u16PtrCnt++] = (byte)(0);
            SendDataPacket[u16PtrCnt++] = (byte)(0);
            SendDataPacket[u16PtrCnt++] = (byte)(0);
            SendDataPacket[u16PtrCnt++] = (byte)(0);
          }
          else
          {
            SendDataPacket[u16PtrCnt++] = (byte)(0);
            SendDataPacket[u16PtrCnt++] = (byte)(0);
            SendDataPacket[u16PtrCnt++] = (byte)(0);
            SendDataPacket[u16PtrCnt++] = (byte)(0);
          }
        }
        else if (StartAddress == 3) // Driver Model index & Info_DrvModel 1set
        {
          SendDataPacket[u16PtrCnt++] = (byte)(Info_DrvModel_para.u16Driver_id >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(Info_DrvModel_para.u16Driver_id >> 8);
          SendDataPacket[u16PtrCnt++] = (byte)(Info_DrvModel_para.u16Driver_vendor_id >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(Info_DrvModel_para.u16Driver_vendor_id >> 8);
          SendDataPacket[u16PtrCnt++] = (byte)(Info_DrvModel_para.u16Controller_id >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(Info_DrvModel_para.u16Controller_id >> 8);
          SendDataPacket[u16PtrCnt++] = (byte)(Info_DrvModel_para.u16Motor_id >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(Info_DrvModel_para.u16Motor_id >> 8);

          d.f = Info_DrvModel_para.f32Tq_min_Nm;
          SendDataPacket[u16PtrCnt++] = d.b0;
          SendDataPacket[u16PtrCnt++] = d.b1;
          SendDataPacket[u16PtrCnt++] = d.b2;
          SendDataPacket[u16PtrCnt++] = d.b3;
          d.f = Info_DrvModel_para.f32Tq_max_Nm;
          SendDataPacket[u16PtrCnt++] = d.b0;
          SendDataPacket[u16PtrCnt++] = d.b1;
          SendDataPacket[u16PtrCnt++] = d.b2;
          SendDataPacket[u16PtrCnt++] = d.b3;
          d.u = Info_DrvModel_para.u32Speed_min;
          SendDataPacket[u16PtrCnt++] = d.b0;
          SendDataPacket[u16PtrCnt++] = d.b1;
          SendDataPacket[u16PtrCnt++] = d.b2;
          SendDataPacket[u16PtrCnt++] = d.b3;
          d.u = Info_DrvModel_para.u32Speed_max;
          SendDataPacket[u16PtrCnt++] = d.b0;
          SendDataPacket[u16PtrCnt++] = d.b1;
          SendDataPacket[u16PtrCnt++] = d.b2;
          SendDataPacket[u16PtrCnt++] = d.b3;
          d.f = Info_DrvModel_para.f32Gear_ratio;
          SendDataPacket[u16PtrCnt++] = d.b0;
          SendDataPacket[u16PtrCnt++] = d.b1;
          SendDataPacket[u16PtrCnt++] = d.b2;
          SendDataPacket[u16PtrCnt++] = d.b3;
          d.f = Info_DrvModel_para.f32Angle_head_ratio;
          SendDataPacket[u16PtrCnt++] = d.b0;
          SendDataPacket[u16PtrCnt++] = d.b1;
          SendDataPacket[u16PtrCnt++] = d.b2;
          SendDataPacket[u16PtrCnt++] = d.b3;

          for (int ii = 0; ii < 32; ii++)
            SendDataPacket[u16PtrCnt++] = (byte)0;
        }
        // else if (StartAddress == 4) // MC model & version
      }
      if (Command == 2)
      {
        switch (StartAddress)
        {
          case 1://fasten/loosen
            SendDataPacket[u16PtrCnt++] = (byte)(Data >> 0);
            SendDataPacket[u16PtrCnt++] = (byte)0;
            SendDataPacket[u16PtrCnt++] = (byte)0;
            SendDataPacket[u16PtrCnt++] = (byte)0;
            break;
          case 2://Start/Stop
            if (Data != 0)//start
            {
              string input = String.Empty;

              if (tbLoosenAngle.Text == "" || tbLoosenAngle.Text == "0")
                McFlag.LoosenAngle = 0;
              else
              {
                try
                {
                  McFlag.LoosenAngle = Int16.Parse(tbLoosenAngle.Text);
                  // Console.WriteLine(result);
                }
                catch (FormatException)
                {
                  // Console.WriteLine($"Unable to parse '{input}'");
                  McFlag.LoosenAngle = 0;
                }
              }
              SendDataPacket[u16PtrCnt++] = (byte)(Data >> 0);
              SendDataPacket[u16PtrCnt++] = (byte)(McFlag.LoosenAngle >> 0);
              SendDataPacket[u16PtrCnt++] = (byte)(McFlag.LoosenAngle >> 8);
              SendDataPacket[u16PtrCnt++] = (byte)0;
            }
            else//stop
            {
              SendDataPacket[u16PtrCnt++] = (byte)(Data >> 0);
              SendDataPacket[u16PtrCnt++] = (byte)0;
              SendDataPacket[u16PtrCnt++] = (byte)0;
              SendDataPacket[u16PtrCnt++] = (byte)0;
            }
            break;
          case 3://Save Origin Point
          case 4://Move origin
          case 5://Reset MC
          case 6://Reset Alarm/Error
          case 7://parameter initialization
            SendDataPacket[u16PtrCnt++] = (byte)0;
            SendDataPacket[u16PtrCnt++] = (byte)0;
            SendDataPacket[u16PtrCnt++] = (byte)0;
            SendDataPacket[u16PtrCnt++] = (byte)0;
            break;
          case 8://soft/hard joint customizing
          case 9://start/stop auto-customizing
          case 10://send start comm.
            SendDataPacket[u16PtrCnt++] = (byte)(Data >> 0);
            SendDataPacket[u16PtrCnt++] = (byte)0;
            SendDataPacket[u16PtrCnt++] = (byte)0;
            SendDataPacket[u16PtrCnt++] = (byte)0;
            break;
          // case 11://answer to start comm. PC<-MC
          default:
            break;
        }
      }
      // else if (Command == 3) cyclic PC<-MC
      // else if (Command == 4) graph PC<-MC
      // else if (Command == 5) event PC<-MC
      else if (Command == 6)
      {
        if (StartAddress == 1)// Sync setting
        {
          SendDataPacket[u16PtrCnt++] = SyncStruct.Bits_b1OnOff;
          SendDataPacket[u16PtrCnt++] = SyncStruct.Bits_b1Master;
          SendDataPacket[u16PtrCnt++] = (byte)(SyncStruct.u16WaitingBeforeSync >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(SyncStruct.u16WaitingBeforeSync >> 8);
          SendDataPacket[u16PtrCnt++] = (byte)(SyncStruct.u16WaitingBetweenSync >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(SyncStruct.u16WaitingBetweenSync >> 8);
          SendDataPacket[u16PtrCnt++] = (byte)0;
          SendDataPacket[u16PtrCnt++] = (byte)0;
        }
        // else if (StartAddress == 2)// Sync state out PC<-MC
        else if (StartAddress == 3)// Sync resume
        {
          SendDataPacket[u16PtrCnt++] = SyncStruct.Bits_b1ResumeOnOff;
          SendDataPacket[u16PtrCnt++] = (byte)0;
          SendDataPacket[u16PtrCnt++] = (byte)0;
          SendDataPacket[u16PtrCnt++] = (byte)0;
        }
        else if (StartAddress == 4)// Sync in event update
        {
          SendDataPacket[u16PtrCnt++] = SyncStruct.Bits_b1SyncIn;
          SendDataPacket[u16PtrCnt++] = (byte)0;
          SendDataPacket[u16PtrCnt++] = (byte)0;
          SendDataPacket[u16PtrCnt++] = (byte)0;
        }
      }
      else if (Command == 7) // parameter
      {
        if (StartAddress == 1) // Download Driver info
        {
          SendDataPacket[u16PtrCnt++] = (byte)(DriverInfo.u16Type >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(DriverInfo.u16Type >> 8);
          SendDataPacket[u16PtrCnt++] = (byte)(0);
          SendDataPacket[u16PtrCnt++] = (byte)(0);
          SendDataPacket[u16PtrCnt++] = (byte)(DriverInfo.u16Serial_low >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(DriverInfo.u16Serial_low >> 8);
          SendDataPacket[u16PtrCnt++] = (byte)(DriverInfo.u16Serial_high >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(DriverInfo.u16Serial_high >> 8);
          SendDataPacket[u16PtrCnt++] = (byte)(DriverInfo.u8Factory_Gear_efficiency >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(0);
          SendDataPacket[u16PtrCnt++] = (byte)(DriverInfo.u8User_Gear_efficiency >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(0);
          SendDataPacket[u16PtrCnt++] = (byte)(0);
          SendDataPacket[u16PtrCnt++] = (byte)(0);
          SendDataPacket[u16PtrCnt++] = (byte)(0);
          SendDataPacket[u16PtrCnt++] = (byte)(0);
        }
        // if (StartAddress == 2) // Upload Driver info PC<-MC
        else if (StartAddress == 3)//Speaker & Output
        {
          SendDataPacket[u16PtrCnt++] = (byte)(Data >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(0);
          SendDataPacket[u16PtrCnt++] = (byte)(0);
          SendDataPacket[u16PtrCnt++] = (byte)(0);
        }
        else if (StartAddress == 4)//LED Band Set
        {
          SendDataPacket[u16PtrCnt++] = (byte)(0);
          SendDataPacket[u16PtrCnt++] = (byte)(0);
          SendDataPacket[u16PtrCnt++] = (byte)(0);
          SendDataPacket[u16PtrCnt++] = (byte)(0);
        }
        // else if (StartAddress == 5)//reserved
        // else if (StartAddress == 6)//reserved
        // else if (StartAddress == 7)//reserved
        else if (StartAddress == 8)//Reset maintenance count
        {
          SendDataPacket[u16PtrCnt++] = (byte)(1);
          SendDataPacket[u16PtrCnt++] = (byte)(0);
          SendDataPacket[u16PtrCnt++] = (byte)(0);
          SendDataPacket[u16PtrCnt++] = (byte)(0);
        }
        // else if (StartAddress == 9)//reserved
        else if (StartAddress == 10)//Check OffsetADC
        {
          SendDataPacket[u16PtrCnt++] = (byte)(0);//(byte)(DriverInfo.u16TorqueOffset >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(0);//(byte)(DriverInfo.u16TorqueOffset >> 8);
          SendDataPacket[u16PtrCnt++] = (byte)(0);
          SendDataPacket[u16PtrCnt++] = (byte)(0);
        }
        else if (StartAddress == 11)//Save OffsetADC
        {
          SendDataPacket[u16PtrCnt++] = (byte)(DriverInfo.u16TorqueOffset >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(DriverInfo.u16TorqueOffset >> 8);
          SendDataPacket[u16PtrCnt++] = (byte)(0);
          SendDataPacket[u16PtrCnt++] = (byte)(0);
        }
        else if (StartAddress == 12)
        {
          SendDataPacket[u16PtrCnt++] = (byte)(Data >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(Data >> 8);
          SendDataPacket[u16PtrCnt++] = (byte)(0);
          SendDataPacket[u16PtrCnt++] = (byte)(0);
        }
        // else if (StartAddress == 13)//reseive Initial angle result PC<-MC
        else
        {
          SendDataPacket[u16PtrCnt++] = (byte)(Data >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(Data >> 8);
        }
      }
      else if (Command == 8) // MotTest or NutRunner
      {
        //if (StartAddress == 1)
        {
          SendDataPacket[u16PtrCnt++] = (byte)(Data >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(Data >> 8);
        }
      }
      else if (Command == 9) // MotTest or NutRunner
      {
        //if (StartAddress == 1)
        // {
        //   SendDataPacket[u16PtrCnt++] = (byte)(Data >> 0);
        //   SendDataPacket[u16PtrCnt++] = (byte)(Data >> 8);
        // }
        switch(StartAddress)
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
        ResetAckState();
        CmdAck.u16PtrCnt = u16PtrCnt;
      }
      else
      {
        CmdAck.u8Command = Command;
        CmdAck.u16PtrCnt = u16PtrCnt;
        CmdAck.u16StartAddress = StartAddress;
        CmdAck.u8AckWait = ON;
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
        btServoOnOff.Text = "Servo Off";
      }
      else
      {
        MakeAndSendData(8, 3, 0);
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
      InitInfo_DrvModel_para(DriverType);//1);
      InitDriverInfo(DriverType);
      InitParameter(DriverType);
      MakeAndSendData(2, 10, 0);
    }
    private void btTqOffset_Click(object sender, EventArgs e)
    {
      if (!Port.IsOpen)
        return;

      if (sender == btTqOffsetCheck)
      {
        MakeAndSendData(7, 10, 0);
        btTqOffsetSave.Enabled = true;
      }
      else if (sender == btTqOffsetSave)
      {
        MakeAndSendData(7, 11, 0);
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

      tbTargetSpeed.Text = AutoSetting.CurrentSpeed.ToString();
      tbSeatingPoint.Text = AutoSetting.CurrentSeatingPoint.ToString();
      tbFreeSpeed.Text = AutoSetting.CurrentFSpeed.ToString();
      tbFreeAngle.Text = AutoSetting.CurrentFAngle.ToString();

      tbTqSensorValue.Text = TqSensorValue.ToString();
      tbTqOffsetValue.Text = TqOffsetValue.ToString();
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


      switch (AutoSetting.FlagSetting)
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
      switch (AutoSetting.FlagStart)
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
      if (McFlag.b1ControlFL != 0)
      {
        btFastenLoosen.Text = "Fasten";
        tbLoosenAngle.Enabled = true;
      }
      else
      {
        btFastenLoosen.Text = "Loosen";
        tbLoosenAngle.Enabled = false;
      }
      if (McFlag.b1Run != 0)
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
      if (McFlag.b1Run == 0)
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
                    ResetAckState();
                  }
                  else if (StartAddress == 3)
                  {
                    ResetAckState();
                    MakeAndSendData(1, 1, 0);
                  }
                  else if (StartAddress == 4)
                  {
                    AckSend(Command, 0, StartAddress, 0);       // return Ack OK
                    McInfo.u16Con_Model_Type = (ushort)((ComReadBuffer[11] << 8) | ComReadBuffer[10]);
                    McInfo.u16Version = (ushort)((ComReadBuffer[13] << 8) | ComReadBuffer[12]);
                  }
                  break;
                case 2:
                  if (StartAddress == 1 || StartAddress == 2 || StartAddress == 3 || StartAddress == 4 ||
                      StartAddress == 6 || StartAddress == 7 || StartAddress == 8 || StartAddress == 9 || StartAddress == 10)
                  {
                    ResetAckState();
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
                  // tbTqSensorValue.Text = TqSensorValue.ToString();//ui

                  TqOffsetValue = (ushort)((ComReadBuffer[15] << 8) | ComReadBuffer[14]);
                  DriverInfo.u16TorqueOffset = TqOffsetValue;
                  // tbTqOffsetValue.Text = TqOffsetValue.ToString();//ui

                  Error = (ushort)((ComReadBuffer[29] << 8) | ComReadBuffer[28]);
                  // tbError.Text = Error.ToString();//ui
                  MaintCnt = (uint)((ComReadBuffer[51] << 24) | (ComReadBuffer[50] << 16) | (ComReadBuffer[49] << 8) | ComReadBuffer[48]);
                  // tbMaintCnt.Text = MaintCnt.ToString();//ui
                  Enc = (ushort)((ComReadBuffer[41] << 8) | ComReadBuffer[40]);
                  // tbEnc.Text = Enc.ToString();//ui

                  MotorState = ((ComReadBuffer[27] << 8) | ComReadBuffer[26]) != 0;
                  McFlag.b1Run = ComReadBuffer[26];
                  McFlag.b1ControlFL = ComReadBuffer[30];

                  if (ComReadBuffer[42] != 0)
                    AutoSetting.FlagSetting = true;
                  else
                    AutoSetting.FlagSetting = false;

                  if (ComReadBuffer[43] != 0)
                    AutoSetting.FlagStart = true;
                  else
                    AutoSetting.FlagStart = false;

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
                  AutoSetting.CurrentSpeed = (ushort)((ComReadBuffer[119] << 8) | ComReadBuffer[118]);
                  AutoSetting.CurrentSeatingPoint = (ushort)((ComReadBuffer[121] << 8) | ComReadBuffer[120]);
                  AutoSetting.CurrentFSpeed = (ushort)((ComReadBuffer[123] << 8) | ComReadBuffer[122]);
                  AutoSetting.CurrentFAngle = (ushort)((ComReadBuffer[125] << 8) | ComReadBuffer[124]);
                  break;
                case 6:
                  break;
                case 7:
                  // if (StartAddress == 1)//download Driver info
                  if (StartAddress == 2)//upload Driver info
                  {
                    AckSend(Command, 0, StartAddress, 0);       // return Ack OK
                    inDriverInfo.u16Type = (ushort)((ComReadBuffer[11] << 8) | ComReadBuffer[10]);
                    inDriverInfo.u16Version = (ushort)((ComReadBuffer[13] << 8) | ComReadBuffer[12]);
                    inDriverInfo.u16Serial_low = (ushort)((ComReadBuffer[15] << 8) | ComReadBuffer[14]);
                    inDriverInfo.u16Serial_high = (ushort)((ComReadBuffer[17] << 8) | ComReadBuffer[16]);
                    inDriverInfo.u8Factory_Gear_efficiency = (ushort)((ComReadBuffer[19] << 8) | ComReadBuffer[18]);
                    inDriverInfo.u8User_Gear_efficiency = (ushort)((ComReadBuffer[21] << 8) | ComReadBuffer[20]);
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
    void ResetAckState()
    {
      CmdAck.u8Command = 0;
      CmdAck.u8AckWait = OFF;
      CmdAck.u16StartAddress = 0;
      CmdAck.u16PtrCnt = 0;
    }
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
    private void InitInfo_DrvModel_para(ushort u16Driver_id_)
    {
      Info_DrvModel_para.u16Driver_id = u16Driver_id_;
      Info_DrvModel_para.u16Driver_vendor_id = 2;//1:hantas, 2:torero
      Info_DrvModel_para.u16Controller_id = 1;      // controller model no. 1:26, 2:32
      Info_DrvModel_para.u16Motor_id = 2;          // used motor no.       1:26, 2:32
                                                   // // TORQUE / SPEED
                                                   // Info_DrvModel_para.f32Tq_min_Nm = 15;         // default Nm
                                                   // Info_DrvModel_para.f32Tq_max_Nm = 80;         // default Nm
                                                   // Info_DrvModel_para.u32Speed_min = 50;
                                                   // Info_DrvModel_para.u32Speed_max = 475;
                                                   // SETING
      switch (u16Driver_id_)
      {
        case 1://30
               // TORQUE
          Info_DrvModel_para.f32Tq_min_Nm = 7.0f;         // default Nm
          Info_DrvModel_para.f32Tq_max_Nm = 35.0f;         // default Nm
                                                           // SPEED
          Info_DrvModel_para.u32Speed_min = 50;
          Info_DrvModel_para.u32Speed_max = 1090;
          // Gear
          Info_DrvModel_para.f32Gear_ratio = 21.1923055f;//48.8163261f;
          Info_DrvModel_para.f32Angle_head_ratio = 1.545455f;//1.54545498f;
          break;
        case 2://40
               // TORQUE
          Info_DrvModel_para.f32Tq_min_Nm = 8.0f;         // default Nm
          Info_DrvModel_para.f32Tq_max_Nm = 40.0f;         // default Nm
                                                           // SPEED
          Info_DrvModel_para.u32Speed_min = 50;
          Info_DrvModel_para.u32Speed_max = 1090;
          // Gear
          Info_DrvModel_para.f32Gear_ratio = 21.1923055f;//4.461538f * 4.75f;
          Info_DrvModel_para.f32Angle_head_ratio = 1.545455f;//1.54545498f;
          break;
        case 3://50
               // TORQUE
          Info_DrvModel_para.f32Tq_min_Nm = 10.0f;         // default Nm
          Info_DrvModel_para.f32Tq_max_Nm = 55.0f;         // default Nm
                                                           // SPEED
          Info_DrvModel_para.u32Speed_min = 50;
          Info_DrvModel_para.u32Speed_max = 655;
          // Gear
          Info_DrvModel_para.f32Gear_ratio = 35.2857123f;//7.428571f * 4.75f;
          Info_DrvModel_para.f32Angle_head_ratio = 1.545455f;//1.54545498f;
          break;
        case 4://70
               // TORQUE
          Info_DrvModel_para.f32Tq_min_Nm = 15.0f;         // default Nm
          Info_DrvModel_para.f32Tq_max_Nm = 80.0f;         // default Nm
                                                           // SPEED
          Info_DrvModel_para.u32Speed_min = 50;
          Info_DrvModel_para.u32Speed_max = 475;
          // Gear
          Info_DrvModel_para.f32Gear_ratio = 48.8163269f;//48.8163261f;
          Info_DrvModel_para.f32Angle_head_ratio = 1.545455f;//1.54545498f;
          break;
        case 5://100
               // TORQUE
          Info_DrvModel_para.f32Tq_min_Nm = 20.0f;         // default Nm
          Info_DrvModel_para.f32Tq_max_Nm = 100.0f;         // default Nm
                                                            // SPEED
          Info_DrvModel_para.u32Speed_min = 50;
          Info_DrvModel_para.u32Speed_max = 350;
          // Gear
          Info_DrvModel_para.f32Gear_ratio = 48.8163261f;
          Info_DrvModel_para.f32Angle_head_ratio = 1.8f;//1.54545498f;
          break;
        case 6://150
               // TORQUE
          Info_DrvModel_para.f32Tq_min_Nm = 30.0f;         // default Nm
          Info_DrvModel_para.f32Tq_max_Nm = 160.0f;         // default Nm
                                                            // SPEED
          Info_DrvModel_para.u32Speed_min = 50;
          Info_DrvModel_para.u32Speed_max = 227;
          // Gear
          Info_DrvModel_para.f32Gear_ratio = 103.999994f;//48.8163269f;//48.8163261f;
          Info_DrvModel_para.f32Angle_head_ratio = 1.8f;//1.54545498f;
          break;
        case 7://180
               // TORQUE
          Info_DrvModel_para.f32Tq_min_Nm = 35.0f;         // default Nm
          Info_DrvModel_para.f32Tq_max_Nm = 180.0f;         // default Nm
                                                            // SPEED
          Info_DrvModel_para.u32Speed_min = 50;
          Info_DrvModel_para.u32Speed_max = 190;
          // Gear
          Info_DrvModel_para.f32Gear_ratio = 103.999994f;//48.8163261f;
          Info_DrvModel_para.f32Angle_head_ratio = 1.8f;//1.54545498f;
          break;
        case 8://200
               // TORQUE
          Info_DrvModel_para.f32Tq_min_Nm = 40.0f;         // default Nm
          Info_DrvModel_para.f32Tq_max_Nm = 200.0f;         // default Nm
                                                            // SPEED
          Info_DrvModel_para.u32Speed_min = 50;
          Info_DrvModel_para.u32Speed_max = 185;
          // Gear
          Info_DrvModel_para.f32Gear_ratio = 103.999994f;//48.8163261f;
          Info_DrvModel_para.f32Angle_head_ratio = 1.8f;//1.54545498f;
          break;
        case 9:
          Info_DrvModel_para.u16Driver_id = u16Driver_id_;
          Info_DrvModel_para.u16Driver_vendor_id = 1;//1:hantas, 2:torero
          Info_DrvModel_para.u16Controller_id = 1;      // controller model no. 1:26, 2:32
          Info_DrvModel_para.u16Motor_id = 1;          // used motor no.       1:26, 2:32
          // TORQUE
          Info_DrvModel_para.f32Tq_min_Nm = 0.0f;         // default Nm
          Info_DrvModel_para.f32Tq_max_Nm = 1.5f;         // default Nm
          // SPEED
          Info_DrvModel_para.u32Speed_min = 0;
          Info_DrvModel_para.u32Speed_max = 40000;
          // Gear
          Info_DrvModel_para.f32Gear_ratio = 1.0f;//48.8163261f;
          Info_DrvModel_para.f32Angle_head_ratio = 1.0f;//1.54545498f;
          break;
        default:
          break;
      }
      // Info_DrvModel_para.f32Gear_ratio = 48.8163261f;
      // Info_DrvModel_para.f32Angle_head_ratio = 1.54545498f;
    }
    private void InitDriverInfo(ushort u16Type_)
    {
      DriverInfo.u16Type = u16Type_;//1;
      DriverInfo.u16Version = 123;
      DriverInfo.u8Factory_Gear_efficiency = 100;
      DriverInfo.u8User_Gear_efficiency = 100;
      DriverInfo.u16Serial_low = 1234;
      DriverInfo.u16Serial_high = 5678;
      DriverInfo.u16MaintenanceCount_low = 0;
      DriverInfo.u16MaintenanceCount_high = 0;
      DriverInfo.u16WarningMaintenanceCount = 0;
      DriverInfo.u16TorqueOffset = 32768;
      DriverInfo.u16LED_Band = 0;
      DriverInfo.u16Temperature = 0;
      DriverInfo.u16Initial_Angle = 0;
      DriverInfo.u16Error = 0;
    }
    private void InitParameter(ushort u16MC_DRIVER_MODEL_)
    {
      Mc_Para.dft.u16MC_ZERO_DUMMY = 0; Mc_Para.min.u16MC_ZERO_DUMMY = 0; Mc_Para.max.u16MC_ZERO_DUMMY = 1;                 //dummy
      Mc_Para.dft.u16MC_TCAM_ACTM = 0; Mc_Para.min.u16MC_TCAM_ACTM = 0; Mc_Para.max.u16MC_TCAM_ACTM = 1;                  //SET[01] :i select torque/angle
      Mc_Para.dft.f32MC_FASTEN_TORQUE = 50; Mc_Para.min.f32MC_FASTEN_TORQUE = 30; Mc_Para.max.f32MC_FASTEN_TORQUE = 500;            //SET[02] :f toque [Nm*100]
      Mc_Para.dft.f32MC_TORQUE_MIN_MAX = 1000; Mc_Para.min.f32MC_TORQUE_MIN_MAX = 0; Mc_Para.max.f32MC_TORQUE_MIN_MAX = 10000;         //SET[03] : %  (Actually use value when initializing para)
      Mc_Para.dft.u16MC_TARGET_ANGLE = 0; Mc_Para.min.u16MC_TARGET_ANGLE = 0; Mc_Para.max.u16MC_TARGET_ANGLE = 20000;           //SET[04] : degree
      Mc_Para.dft.u16MC_FASTEN_MIN_ANGLE = 0; Mc_Para.min.u16MC_FASTEN_MIN_ANGLE = 0; Mc_Para.max.u16MC_FASTEN_MIN_ANGLE = 20000;       //SET[05] : 
      Mc_Para.dft.u16MC_FASTEN_MAX_ANGLE = 0; Mc_Para.min.u16MC_FASTEN_MAX_ANGLE = 0; Mc_Para.max.u16MC_FASTEN_MAX_ANGLE = 20000;       //SET[06] : 
      Mc_Para.dft.f32MC_SNUG_TORQUE = 0; Mc_Para.min.f32MC_SNUG_TORQUE = 0; Mc_Para.max.f32MC_SNUG_TORQUE = 100;              //SET[07] : %
      Mc_Para.dft.u16MC_FASTEN_SPEED = 300; Mc_Para.min.u16MC_FASTEN_SPEED = 100; Mc_Para.max.u16MC_FASTEN_SPEED = 2000;            //SET[08] : speed[RPM]
      Mc_Para.dft.u16MC_FREE_FASTEN_ANGLE = 0; Mc_Para.min.u16MC_FREE_FASTEN_ANGLE = 0; Mc_Para.max.u16MC_FREE_FASTEN_ANGLE = 20000;      //SET[09] : degree
      Mc_Para.dft.u16MC_FREE_FASTEN_SPEED = 0; Mc_Para.min.u16MC_FREE_FASTEN_SPEED = 0; Mc_Para.max.u16MC_FREE_FASTEN_SPEED = 1000;       //SET[10] : 
      Mc_Para.dft.u16MC_SOFT_START = 100; Mc_Para.min.u16MC_SOFT_START = 0; Mc_Para.max.u16MC_SOFT_START = 300;               //SET[11] : 
      Mc_Para.dft.u16MC_FASTEN_SEATTING_POINT_RATE = 40; Mc_Para.min.u16MC_FASTEN_SEATTING_POINT_RATE = 10; Mc_Para.max.u16MC_FASTEN_SEATTING_POINT_RATE = 95;//SET[12] : %
      Mc_Para.dft.u16MC_FASTEN_TQ_RISING_TIME = 50; Mc_Para.min.u16MC_FASTEN_TQ_RISING_TIME = 50; Mc_Para.max.u16MC_FASTEN_TQ_RISING_TIME = 200;    //SET[13] : ms
      Mc_Para.dft.u16MC_RAMP_UP_SPEED = 0; Mc_Para.min.u16MC_RAMP_UP_SPEED = 0; Mc_Para.max.u16MC_RAMP_UP_SPEED = 1;              //SET[14] : speed[RPM]
      Mc_Para.dft.u16MC_TORQUE_COMPENSATION = 0; Mc_Para.min.u16MC_TORQUE_COMPENSATION = 0; Mc_Para.max.u16MC_TORQUE_COMPENSATION = 1;        //SET[15] : 
      Mc_Para.dft.u16MC_TORQUE_OFFSET = 0; Mc_Para.min.u16MC_TORQUE_OFFSET = 0; Mc_Para.max.u16MC_TORQUE_OFFSET = 20000;          //SET[16] : 
      Mc_Para.dft.u16MC_MAX_PULSE_COUNT = 0; Mc_Para.min.u16MC_MAX_PULSE_COUNT = 0; Mc_Para.max.u16MC_MAX_PULSE_COUNT = 20000;        //SET[17] : 

      Mc_Para.dft.u16MC_ADVANCED_MODE = 0; Mc_Para.min.u16MC_ADVANCED_MODE = 0; Mc_Para.max.u16MC_ADVANCED_MODE = 10;                   //SET[0] :  
      Mc_Para.dft.f32MC_ADVANCED_PARA1 = 0; Mc_Para.min.f32MC_ADVANCED_PARA1 = 0; Mc_Para.max.f32MC_ADVANCED_PARA1 = 0xffff;              //SET[1] :  
      Mc_Para.dft.f32MC_ADVANCED_PARA2 = 0; Mc_Para.min.f32MC_ADVANCED_PARA2 = 0; Mc_Para.max.f32MC_ADVANCED_PARA2 = 0xffff;              //SET[2] :  
      Mc_Para.dft.f32MC_ADVANCED_PARA3 = 0; Mc_Para.min.f32MC_ADVANCED_PARA3 = 0; Mc_Para.max.f32MC_ADVANCED_PARA3 = 0xffff;              //SET[3] :  
      Mc_Para.dft.f32MC_ADVANCED_PARA4 = 0; Mc_Para.min.f32MC_ADVANCED_PARA4 = 0; Mc_Para.max.f32MC_ADVANCED_PARA4 = 0xffff;              //SET[4] :  
      Mc_Para.dft.f32MC_ADVANCED_PARA5 = 0; Mc_Para.min.f32MC_ADVANCED_PARA5 = 0; Mc_Para.max.f32MC_ADVANCED_PARA5 = 0xffff;              //SET[5] :  
      Mc_Para.dft.f32MC_ADVANCED_PARA6 = 0; Mc_Para.min.f32MC_ADVANCED_PARA6 = 0; Mc_Para.max.f32MC_ADVANCED_PARA6 = 0xffff;              //SET[6] :  
      Mc_Para.dft.f32MC_ADVANCED_PARA7 = 0; Mc_Para.min.f32MC_ADVANCED_PARA7 = 0; Mc_Para.max.f32MC_ADVANCED_PARA7 = 0xffff;              //SET[7] :  
      Mc_Para.dft.f32MC_ADVANCED_PARA8 = 0; Mc_Para.min.f32MC_ADVANCED_PARA8 = 0; Mc_Para.max.f32MC_ADVANCED_PARA8 = 0xffff;              //SET[8] :  
      Mc_Para.dft.f32MC_ADVANCED_PARA9 = 0; Mc_Para.min.f32MC_ADVANCED_PARA9 = 0; Mc_Para.max.f32MC_ADVANCED_PARA9 = 0xffff;              //SET[9] :  
      Mc_Para.dft.f32MC_ADVANCED_PARA10 = 0; Mc_Para.min.f32MC_ADVANCED_PARA10 = 0; Mc_Para.max.f32MC_ADVANCED_PARA10 = 0xffff;             //SET[10] : 
      Mc_Para.dft.f32MC_ADVANCED_PARA11 = 0; Mc_Para.min.f32MC_ADVANCED_PARA11 = 0; Mc_Para.max.f32MC_ADVANCED_PARA11 = 0xffff;             //SET[11] : 
      Mc_Para.dft.f32MC_ADVANCED_PARA12 = 0; Mc_Para.min.f32MC_ADVANCED_PARA12 = 0; Mc_Para.max.f32MC_ADVANCED_PARA12 = 0xffff;             //SET[12] : 
      Mc_Para.dft.f32MC_ADVANCED_PARA13 = 0; Mc_Para.min.f32MC_ADVANCED_PARA13 = 0; Mc_Para.max.f32MC_ADVANCED_PARA13 = 0xffff;             //SET[13] : 
      Mc_Para.dft.f32MC_ADVANCED_PARA14 = 0; Mc_Para.min.f32MC_ADVANCED_PARA14 = 0; Mc_Para.max.f32MC_ADVANCED_PARA14 = 0xffff;             //SET[14] : 
      Mc_Para.dft.f32MC_ADVANCED_PARA15 = 0; Mc_Para.min.f32MC_ADVANCED_PARA15 = 0; Mc_Para.max.f32MC_ADVANCED_PARA15 = 0xffff;             //SET[15] : 
      Mc_Para.dft.f32MC_ADVANCED_PARA16 = 0; Mc_Para.min.f32MC_ADVANCED_PARA16 = 0; Mc_Para.max.f32MC_ADVANCED_PARA16 = 0xffff;             //SET[16] : 
      Mc_Para.dft.f32MC_ADVANCED_PARA17 = 0; Mc_Para.min.f32MC_ADVANCED_PARA17 = 0; Mc_Para.max.f32MC_ADVANCED_PARA17 = 0xffff;             //SET[17] : 
      Mc_Para.dft.f32MC_ADVANCED_PARA18 = 0; Mc_Para.min.f32MC_ADVANCED_PARA18 = 0; Mc_Para.max.f32MC_ADVANCED_PARA18 = 0xffff;             //SET[18] : 
      Mc_Para.dft.f32MC_ADVANCED_PARA19 = 0; Mc_Para.min.f32MC_ADVANCED_PARA19 = 0; Mc_Para.max.f32MC_ADVANCED_PARA19 = 0xffff;             //SET[19] : 
      Mc_Para.dft.u16MC_FREE_REVERSE_ROTATION_SPEED = 0; Mc_Para.min.u16MC_FREE_REVERSE_ROTATION_SPEED = 0; Mc_Para.max.u16MC_FREE_REVERSE_ROTATION_SPEED = 1000;   //SET[1] :  
      Mc_Para.dft.f32MC_FREE_REVERSE_ROTATION_ANGLE = 0; Mc_Para.min.f32MC_FREE_REVERSE_ROTATION_ANGLE = 0; Mc_Para.max.f32MC_FREE_REVERSE_ROTATION_ANGLE = 200;    //SET[2] :  
      Mc_Para.dft.u16MC_REVERS_ANGLE_SETTING_SPEED = 0; Mc_Para.min.u16MC_REVERS_ANGLE_SETTING_SPEED = 0; Mc_Para.max.u16MC_REVERS_ANGLE_SETTING_SPEED = 1000;    //SET[3] :  
      Mc_Para.dft.u16MC_REVERS_ANGLE_SETTING_ANGLE = 0; Mc_Para.min.u16MC_REVERS_ANGLE_SETTING_ANGLE = 0; Mc_Para.max.u16MC_REVERS_ANGLE_SETTING_ANGLE = 30000;   //SET[4] :  
      Mc_Para.dft.u16MC_REVERS_ANGLE_SETTING_FW_REV = 0; Mc_Para.min.u16MC_REVERS_ANGLE_SETTING_FW_REV = 0; Mc_Para.max.u16MC_REVERS_ANGLE_SETTING_FW_REV = 1;      //SET[5] :  

      Mc_Para.dft.u16MC_DRIVER_MODEL = 2; Mc_Para.min.u16MC_DRIVER_MODEL = 1; Mc_Para.max.u16MC_DRIVER_MODEL = 99;                //SET[0] :  
      Mc_Para.dft.u16MC_UNIT = 2; Mc_Para.min.u16MC_UNIT = 0; Mc_Para.max.u16MC_UNIT = 6;                         //SET[1] :  
      Mc_Para.dft.u16MC_ACC_DEC_TIME = 100; Mc_Para.min.u16MC_ACC_DEC_TIME = 10; Mc_Para.max.u16MC_ACC_DEC_TIME = 1000;              //SET[2] :  
      Mc_Para.dft.u16MC_FASTEN_TORQUE_MAINTAIN_TIME = 2; Mc_Para.min.u16MC_FASTEN_TORQUE_MAINTAIN_TIME = 1; Mc_Para.max.u16MC_FASTEN_TORQUE_MAINTAIN_TIME = 20; //SET[3] :  
      Mc_Para.dft.u16MC_USE_MAXTQ_FOR_LOOSENING = 0; Mc_Para.min.u16MC_USE_MAXTQ_FOR_LOOSENING = 0; Mc_Para.max.u16MC_USE_MAXTQ_FOR_LOOSENING = 1;      //SET[4] :  
      Mc_Para.dft.u16MC_LOOSENING_SPEED = 500; Mc_Para.min.u16MC_LOOSENING_SPEED = 100; Mc_Para.max.u16MC_LOOSENING_SPEED = 1000;           //SET[5] :  
      Mc_Para.dft.f32MC_TOTAL_FASTENING_TIME = 100; Mc_Para.min.f32MC_TOTAL_FASTENING_TIME = 0; Mc_Para.max.f32MC_TOTAL_FASTENING_TIME = 600;       //SET[6] :  
      Mc_Para.dft.f32MC_TOTAL_LOOSENING_TIME = 100; Mc_Para.min.f32MC_TOTAL_LOOSENING_TIME = 0; Mc_Para.max.f32MC_TOTAL_LOOSENING_TIME = 600;       //SET[7] :  
      Mc_Para.dft.f32MC_STALL_LOOSENING_TIME_LIMIT = 2; Mc_Para.min.f32MC_STALL_LOOSENING_TIME_LIMIT = 1; Mc_Para.max.f32MC_STALL_LOOSENING_TIME_LIMIT = 5;    //SET[8] :  
      Mc_Para.dft.u16MC_SCREW_TYPE = 0; Mc_Para.min.u16MC_SCREW_TYPE = 0; Mc_Para.max.u16MC_SCREW_TYPE = 0x7fff;              //SET[9] :  
      Mc_Para.dft.f32MC_JUDGE_FASTEN_MIN_TURNS = 0; Mc_Para.min.f32MC_JUDGE_FASTEN_MIN_TURNS = 0; Mc_Para.max.f32MC_JUDGE_FASTEN_MIN_TURNS = 50;      //SET[10] : 
      Mc_Para.dft.u16MC_FASTENING_STOP_ALARM = 0; Mc_Para.min.u16MC_FASTENING_STOP_ALARM = 0; Mc_Para.max.u16MC_FASTENING_STOP_ALARM = 1;         //SET[11] : 
      Mc_Para.dft.u16MC_TORQUE_COMPENSATION_MAIN = 100; Mc_Para.min.u16MC_TORQUE_COMPENSATION_MAIN = 90; Mc_Para.max.u16MC_TORQUE_COMPENSATION_MAIN = 110;   //SET[12] : 
      Mc_Para.dft.u16MC_CROWFOOT_ENABLE = 0; Mc_Para.min.u16MC_CROWFOOT_ENABLE = 0; Mc_Para.max.u16MC_CROWFOOT_ENABLE = 1;              //SET[13] : 
      Mc_Para.dft.f32MC_CROWFOOT_RATIO = 1000; Mc_Para.min.f32MC_CROWFOOT_RATIO = 0; Mc_Para.max.f32MC_CROWFOOT_RATIO = 65000;           //SET[14] : 
      Mc_Para.dft.u16MC_CROWFOOT_EFFICIENCY = 100; Mc_Para.min.u16MC_CROWFOOT_EFFICIENCY = 0; Mc_Para.max.u16MC_CROWFOOT_EFFICIENCY = 300;        //SET[15] : 
      Mc_Para.dft.f32MC_CROWFOOT_REVERSE_TORQUE = 0; Mc_Para.min.f32MC_CROWFOOT_REVERSE_TORQUE = 0; Mc_Para.max.f32MC_CROWFOOT_REVERSE_TORQUE = 500;    //SET[16] : 
      Mc_Para.dft.u16MC_CROWFOOT_REVERSE_SPEED = 50; Mc_Para.min.u16MC_CROWFOOT_REVERSE_SPEED = 0; Mc_Para.max.u16MC_CROWFOOT_REVERSE_SPEED = 100;     //SET[17] : 

      Mc_Para.val.u16MC_ZERO_DUMMY = 0;     //0
      Mc_Para.val.u16MC_TCAM_ACTM = 0;      //1
      Mc_Para.val.f32MC_FASTEN_TORQUE = 20; //2
      Mc_Para.val.f32MC_TORQUE_MIN_MAX = 0; //3
      Mc_Para.val.u16MC_TARGET_ANGLE = 0;   //4
      Mc_Para.val.u16MC_FASTEN_MIN_ANGLE = 0; //5
      Mc_Para.val.u16MC_FASTEN_MAX_ANGLE = 0; //6
      Mc_Para.val.f32MC_SNUG_TORQUE = 0;      //7
      Mc_Para.val.u16MC_FASTEN_SPEED = 100;     //8
      Mc_Para.val.u16MC_FREE_FASTEN_ANGLE = 0;  //9
      Mc_Para.val.u16MC_FREE_FASTEN_SPEED = 0;  //10
      Mc_Para.val.u16MC_SOFT_START = 100;         //11
      Mc_Para.val.u16MC_FASTEN_SEATTING_POINT_RATE = 50; //12
      Mc_Para.val.u16MC_FASTEN_TQ_RISING_TIME = 50;      //13
      Mc_Para.val.u16MC_RAMP_UP_SPEED = 150;              //14
      Mc_Para.val.u16MC_TORQUE_COMPENSATION = 100;        //15
      Mc_Para.val.u16MC_TORQUE_OFFSET = 10;              //16
      Mc_Para.val.u16MC_MAX_PULSE_COUNT = 100;            //17

      Mc_Para.val.u16MC_ADVANCED_MODE = 0;              //0
      Mc_Para.val.f32MC_ADVANCED_PARA1 = 0;             //1
      Mc_Para.val.f32MC_ADVANCED_PARA2 = 0;             //2
      Mc_Para.val.f32MC_ADVANCED_PARA3 = 0;             //3
      Mc_Para.val.f32MC_ADVANCED_PARA4 = 0;             //4
      Mc_Para.val.f32MC_ADVANCED_PARA5 = 0;             //5
      Mc_Para.val.f32MC_ADVANCED_PARA6 = 0;             //6
      Mc_Para.val.f32MC_ADVANCED_PARA7 = 0;             //7
      Mc_Para.val.f32MC_ADVANCED_PARA8 = 0;             //8
      Mc_Para.val.f32MC_ADVANCED_PARA9 = 0;             //9
      Mc_Para.val.f32MC_ADVANCED_PARA10 = 0;             //10
      Mc_Para.val.f32MC_ADVANCED_PARA11 = 0;             //11
      Mc_Para.val.f32MC_ADVANCED_PARA12 = 0;             //12
      Mc_Para.val.f32MC_ADVANCED_PARA13 = 0;             //13
      Mc_Para.val.f32MC_ADVANCED_PARA14 = 0;             //14
      Mc_Para.val.f32MC_ADVANCED_PARA15 = 0;             //15
      Mc_Para.val.f32MC_ADVANCED_PARA16 = 0;             //16
      Mc_Para.val.f32MC_ADVANCED_PARA17 = 0;             //17
      Mc_Para.val.f32MC_ADVANCED_PARA18 = 0;             //18
      Mc_Para.val.f32MC_ADVANCED_PARA19 = 0;             //19
      Mc_Para.val.u16MC_FREE_REVERSE_ROTATION_SPEED = 0;             //1
      Mc_Para.val.f32MC_FREE_REVERSE_ROTATION_ANGLE = 0;             //2
      Mc_Para.val.u16MC_REVERS_ANGLE_SETTING_SPEED = 0;             //3
      Mc_Para.val.u16MC_REVERS_ANGLE_SETTING_ANGLE = 0;             //4
      Mc_Para.val.u16MC_REVERS_ANGLE_SETTING_FW_REV = 0;             //5

      Mc_Para.val.u16MC_DRIVER_MODEL = u16MC_DRIVER_MODEL_;//4;//1;                 //0
      Mc_Para.val.u16MC_UNIT = 2;                         //1
      Mc_Para.val.u16MC_ACC_DEC_TIME = 200;                 //2
      Mc_Para.val.u16MC_FASTEN_TORQUE_MAINTAIN_TIME = 0;  //3
      Mc_Para.val.u16MC_USE_MAXTQ_FOR_LOOSENING = 0;      //4
      Mc_Para.val.u16MC_LOOSENING_SPEED = 100;              //5
      Mc_Para.val.f32MC_TOTAL_FASTENING_TIME = 10;         //6
      Mc_Para.val.f32MC_TOTAL_LOOSENING_TIME = 10;         //7
      Mc_Para.val.f32MC_STALL_LOOSENING_TIME_LIMIT = 0.2f;    //8
      Mc_Para.val.u16MC_SCREW_TYPE = 0;                   //9
      Mc_Para.val.f32MC_JUDGE_FASTEN_MIN_TURNS = 0;       //10
      Mc_Para.val.u16MC_FASTENING_STOP_ALARM = 0;         //11
      Mc_Para.val.u16MC_TORQUE_COMPENSATION_MAIN = 100;     //12
      Mc_Para.val.u16MC_CROWFOOT_ENABLE = 0;              //13
      Mc_Para.val.f32MC_CROWFOOT_RATIO = 10;               //14
      Mc_Para.val.u16MC_CROWFOOT_EFFICIENCY = 100;          //15
      Mc_Para.val.f32MC_CROWFOOT_REVERSE_TORQUE = 50;      //16
      Mc_Para.val.u16MC_CROWFOOT_REVERSE_SPEED = 0;       //17
    }
    public struct _auto_setting
    {
      public bool FlagSetting;
      public bool FlagStart;
      public ushort CurrentSpeed;
      public ushort CurrentSeatingPoint;
      public ushort CurrentFSpeed;
      public ushort CurrentFAngle;
    }
    _auto_setting AutoSetting;
    void InitAutoSetting()
    {
      AutoSetting.FlagSetting = false;
      AutoSetting.FlagStart = false;
      AutoSetting.CurrentSpeed = 0;
      AutoSetting.CurrentSeatingPoint = 0;
      AutoSetting.CurrentFSpeed = 0;
      AutoSetting.CurrentFAngle = 0;
    }
    public struct _McFlag
    {
      public byte b1Run;   // #00
      public byte b1Reset;   // #01
      public byte b1ControlFL;   // #02     Forward/reverse distinction.
      public short LoosenAngle;
      public byte b1Lock;   // #03     Run driver lock.
      public byte b1Stopping;   // #04     stop process start
      public byte b1Multi_Mode;   // #05     select mult mode
      public byte b1Multi_Start;   // #06     start mult sequence by IO or start switch
      public byte b1TorqueUpCompleteOut;   // #07
      public byte b1FasteningCompleteOut;   // #08
      public byte b2LockCommand;   // #09 #10 driver lock type
      public byte b1Buzzer;   // #11     buzzer control
      public byte b1ReceiveModBusData;   // #12
      public byte b1InternalRun;   // #13     driver start switch
      public byte b1ExternalRun;   // #14     IO start
      public byte b1RunByMult;   // #15     Run inside Multisequence start..
      public byte b1JabCompliteIoOut;   // #16     Flag_JabCompliteIOOut io output..
      public byte b1FirmwareUpdate;   // #17 
      public byte b1CountStartSensorSignalResult;   // #18 A signal considering the delay time of the sensor input.
      public byte b1ParaStartInitialize;   // #19
      public byte b1ParaInitialized;   // #20
      public byte b1SaveDrvModel;   // #21
      public byte b1OneTimeExecute;   // #22 Executed only once during initial booting.
      public byte b1ResetSystem;   // #23 reset System.
      public byte b1SendHostCTqNotComplete;   // #24 Step definition that increases c tq value..
      public byte b1FasteningStopAlarm;   // #25 Stop before fastening after start..
      public byte b1FoundEngagingTorque;   // #26
      public byte b1Reached_LITTLE_REWIND;   // #27 if error appier display torque.
      public byte b1DriverParaInit;   // #28 driver parameter init request
      public byte b1DriverSaveParaData;   // #29
      public byte b1EnableCyclic;   // #30 enable cyclic
      public byte b1Ready;
    }
    _McFlag McFlag;
    void InitMcFlag()
    {
      McFlag.b1Run = 0;   // #00
      McFlag.b1Reset = 0;   // #01
      McFlag.b1ControlFL = 0;   // #02     Forward/reverse distinction.
      McFlag.LoosenAngle = 0;
      McFlag.b1Lock = 0;   // #03     Run driver lock.
      McFlag.b1Stopping = 0;   // #04     stop process start
      McFlag.b1Multi_Mode = 0;   // #05     select mult mode
      McFlag.b1Multi_Start = 0;   // #06     start mult sequence by IO or start switch
      McFlag.b1TorqueUpCompleteOut = 0;   // #07
      McFlag.b1FasteningCompleteOut = 0;   // #08
      McFlag.b2LockCommand = 0;   // #09 #10 driver lock type
      McFlag.b1Buzzer = 0;   // #11     buzzer control
      McFlag.b1ReceiveModBusData = 0;   // #12
      McFlag.b1InternalRun = 0;   // #13     driver start switch
      McFlag.b1ExternalRun = 0;   // #14     IO start
      McFlag.b1RunByMult = 0;   // #15     Run inside Multisequence start..
      McFlag.b1JabCompliteIoOut = 0;   // #16     Flag_JabCompliteIOOut io output..
      McFlag.b1FirmwareUpdate = 0;   // #17 
      McFlag.b1CountStartSensorSignalResult = 0;   // #18 A signal considering the delay time of the sensor input.
      McFlag.b1ParaStartInitialize = 0;   // #19
      McFlag.b1ParaInitialized = 0;   // #20
      McFlag.b1SaveDrvModel = 0;   // #21
      McFlag.b1OneTimeExecute = 0;   // #22 Executed only once during initial booting.
      McFlag.b1ResetSystem = 0;   // #23 reset System.
      McFlag.b1SendHostCTqNotComplete = 0;   // #24 Step definition that increases c tq value..
      McFlag.b1FasteningStopAlarm = 0;   // #25 Stop before fastening after start..
      McFlag.b1FoundEngagingTorque = 0;   // #26
      McFlag.b1Reached_LITTLE_REWIND = 0;   // #27 if error appier display torque.
      McFlag.b1DriverParaInit = 0;   // #28 driver parameter init request
      McFlag.b1DriverSaveParaData = 0;   // #29
      McFlag.b1EnableCyclic = 0;   // #30 enable cyclic
      McFlag.b1Ready = 0;
    }
    public struct _SyncStruct
    {
      public ushort u16WaitingBeforeSync;
      public ushort u16WaitingBetweenSync;
      // _SyncBitsStruct Bits;
      public byte Bits_b1OnOff;
      public byte Bits_b1ResumeOnOff;
      public byte Bits_b1Master;
      public byte Bits_b1SyncIn;
      public byte Bits_b1SyncOut;
    };
    _SyncStruct SyncStruct;
    void InitSyncStruct()
    {
      SyncStruct.u16WaitingBeforeSync = 0;
      SyncStruct.u16WaitingBetweenSync = 0;
      // _SyncBitsStruct Bits;
      SyncStruct.Bits_b1OnOff = 0;
      SyncStruct.Bits_b1ResumeOnOff = 0;
      SyncStruct.Bits_b1Master = 0;
      SyncStruct.Bits_b1SyncIn = 0;
      SyncStruct.Bits_b1SyncOut = 0;
    }
    public struct _McInfoStruct
    {
      public ushort u16Con_Model_Type;
      public ushort u16Version;
      public ushort u16Serial_low;
      public ushort u16Serial_high;
    }
    _McInfoStruct McInfo;
    void InitMcInfo()
    {
      McInfo.u16Con_Model_Type = 0;
      McInfo.u16Version = 0;
      McInfo.u16Serial_low = 0;
      McInfo.u16Serial_high = 0;
    }
    public struct _para_member
    {
      public ushort u16MC_ZERO_DUMMY;
      public ushort u16MC_TCAM_ACTM;                  //1
      public float f32MC_FASTEN_TORQUE;                 //2
      public float f32MC_TORQUE_MIN_MAX;                //3
      public ushort u16MC_TARGET_ANGLE;               //4
      public ushort u16MC_FASTEN_MIN_ANGLE;           //5
      public ushort u16MC_FASTEN_MAX_ANGLE;           //6
      public float f32MC_SNUG_TORQUE;                   //7
      public ushort u16MC_FASTEN_SPEED;               //8
      public ushort u16MC_FREE_FASTEN_ANGLE;          //9
      public ushort u16MC_FREE_FASTEN_SPEED;          //10
      public ushort u16MC_SOFT_START;                 //11
      public ushort u16MC_FASTEN_SEATTING_POINT_RATE; //12
      public ushort u16MC_FASTEN_TQ_RISING_TIME;      //13
      public ushort u16MC_RAMP_UP_SPEED;              //14
      public ushort u16MC_TORQUE_COMPENSATION;        //15
      public ushort u16MC_TORQUE_OFFSET;              //16
      public ushort u16MC_MAX_PULSE_COUNT;            //17
      public ushort u16MC_ADVANCED_MODE;              //0
      public float f32MC_ADVANCED_PARA1;                //1
      public float f32MC_ADVANCED_PARA2;                //2
      public float f32MC_ADVANCED_PARA3;                //3
      public float f32MC_ADVANCED_PARA4;                //4
      public float f32MC_ADVANCED_PARA5;                //5
      public float f32MC_ADVANCED_PARA6;                //6
      public float f32MC_ADVANCED_PARA7;                //7
      public float f32MC_ADVANCED_PARA8;                //8
      public float f32MC_ADVANCED_PARA9;                //9
      public float f32MC_ADVANCED_PARA10;               //10
      public float f32MC_ADVANCED_PARA11;               //11
      public float f32MC_ADVANCED_PARA12;               //12
      public float f32MC_ADVANCED_PARA13;               //13
      public float f32MC_ADVANCED_PARA14;               //14
      public float f32MC_ADVANCED_PARA15;               //15
      public float f32MC_ADVANCED_PARA16;               //16
      public float f32MC_ADVANCED_PARA17;               //17
      public float f32MC_ADVANCED_PARA18;               //18
      public float f32MC_ADVANCED_PARA19;               //19
      public ushort u16MC_FREE_REVERSE_ROTATION_SPEED;//1
      public float f32MC_FREE_REVERSE_ROTATION_ANGLE;   //2
      public ushort u16MC_REVERS_ANGLE_SETTING_SPEED; //3
      public ushort u16MC_REVERS_ANGLE_SETTING_ANGLE; //4
      public ushort u16MC_REVERS_ANGLE_SETTING_FW_REV;//5
      public ushort u16MC_DRIVER_MODEL;               //0
      public ushort u16MC_UNIT;                       //1
      public ushort u16MC_ACC_DEC_TIME;               //2
      public ushort u16MC_FASTEN_TORQUE_MAINTAIN_TIME;//3
      public ushort u16MC_USE_MAXTQ_FOR_LOOSENING;    //4
      public ushort u16MC_LOOSENING_SPEED;            //5
      public float f32MC_TOTAL_FASTENING_TIME;          //6
      public float f32MC_TOTAL_LOOSENING_TIME;          //7
      public float f32MC_STALL_LOOSENING_TIME_LIMIT;     //8
      public ushort u16MC_SCREW_TYPE;                 //9
      public float f32MC_JUDGE_FASTEN_MIN_TURNS;        //10
      public ushort u16MC_FASTENING_STOP_ALARM;       //11
      public ushort u16MC_TORQUE_COMPENSATION_MAIN;   //12
      public ushort u16MC_CROWFOOT_ENABLE;            //13
      public float f32MC_CROWFOOT_RATIO;                //14
      public ushort u16MC_CROWFOOT_EFFICIENCY;        //15
      public float f32MC_CROWFOOT_REVERSE_TORQUE;       //16
      public ushort u16MC_CROWFOOT_REVERSE_SPEED;     //17
      public float f32MC_FREE_SPEED_MAX_TORQUE;       //18
                                                      // } para_Val_etc;
      public ushort u16MC_VERSION;
    }
    public struct _para
    {
      public _para_member val;
      public _para_member dft;
      public _para_member min;
      public _para_member max;
    }
    _para Mc_Para;
    public struct _dr_model
    {
      // MODEL
      public ushort u16Driver_id;
      public ushort u16Driver_vendor_id;
      public ushort u16Controller_id;      // controller model no. 1:26, 2:32
      public ushort u16Motor_id;          // used motor no.       1:26, 2:32
                                          // TORQUE / SPEED
      public float f32Tq_min_Nm;         // default Nm
      public float f32Tq_max_Nm;         // default Nm
      public uint u32Speed_min;
      public uint u32Speed_max;
      // SETING
      public float f32Gear_ratio;
      public float f32Angle_head_ratio;
      // RESERVED
      // public byte      reserved2[32];
      public _dr_model(ushort u16Driver_id_)
      {
        this.u16Driver_id = u16Driver_id_;
        this.u16Driver_vendor_id = 2;
        this.u16Controller_id = 1;      // controller model no. 1:26, 2:32
        this.u16Motor_id = 2;          // used motor no.       1:26, 2:32
                                       // TORQUE / SPEED
        this.f32Tq_min_Nm = 15;         // default Nm
        this.f32Tq_max_Nm = 80;         // default Nm
        this.u32Speed_min = 50;
        this.u32Speed_max = 475;
        // SETING
        this.f32Gear_ratio = 48.8163261f;
        this.f32Angle_head_ratio = 1.54545498f;
      }
    }
    _dr_model Info_DrvModel_para = new _dr_model(0);
    public struct _DriverInfoStruct
    {
      public ushort u16Type;                      // 1 
      public ushort u16Version;                   // 2
      public ushort u8Factory_Gear_efficiency;    // 3
      public ushort u8User_Gear_efficiency;       // 4
      public ushort u16Serial_low;                // 5
      public ushort u16Serial_high;               // 6
      public ushort u16MaintenanceCount_low;      // 7
      public ushort u16MaintenanceCount_high;     // 8
      public ushort u16WarningMaintenanceCount;   // 9
      public ushort u16TorqueOffset;              // 10
      public ushort u16LED_Band;                  // 11
      public ushort u16Temperature;               // 12
      public ushort u16Initial_Angle;             // 13
      public ushort u16Error;                     // 14
      public _DriverInfoStruct(ushort u16Type_)
      {
        this.u16Type = u16Type_;
        this.u16Version = 0;
        this.u8Factory_Gear_efficiency = 0;
        this.u8User_Gear_efficiency = 0;
        this.u16Serial_low = 0;
        this.u16Serial_high = 0;
        this.u16MaintenanceCount_low = 0;
        this.u16MaintenanceCount_high = 0;
        this.u16WarningMaintenanceCount = 0;
        this.u16TorqueOffset = 0;
        this.u16LED_Band = 0;
        this.u16Temperature = 0;
        this.u16Initial_Angle = 0;
        this.u16Error = 0;
      }
    }
    _DriverInfoStruct DriverInfo = new _DriverInfoStruct(0);
    _DriverInfoStruct inDriverInfo = new _DriverInfoStruct(0);
    public struct CmdAck_
    {
      public byte u8Command;
      public byte u8AckWait;
      public ushort u16PtrCnt;
      public ushort u16StartAddress;
      public CmdAck_(byte Command_, ushort PtrCnt_, ushort StartAddress_)
      {
        this.u8Command = Command_;
        this.u8AckWait = 0;
        this.u16PtrCnt = PtrCnt_;
        this.u16StartAddress = StartAddress_;
      }
    }
    CmdAck_ CmdAck = new CmdAck_(0, 0, 0);
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
      Graph_time.Clear();
      for (int i = 0; i < Graph_ch1.Count; i++)
        Graph_time.Add(5e-3d * (double)i);
      
      formsPlot1.Plot.Clear();
      if (cbGraph_ch1.Checked)
      {
        // var sig1 = formsPlot1.Plot.Add.Signal(Graph_ch1);
        var sig1 = formsPlot1.Plot.Add.ScatterLine(Graph_time, Graph_ch1);
        sig1.LegendText = "Torque";
        // sig1.Axes.XAxis = formsPlot1.Plot.Axes.Bottom;
        // sig1.Axes.YAxis = formsPlot1.Plot.Axes.Left;
        // formsPlot1.Plot.Axes.Left.Label.Text = "Torque";
      }
      if (cbGraph_ch2.Checked)
      {
        // var sig2 = formsPlot1.Plot.Add.Signal(Graph_ch2);
        var sig2 = formsPlot1.Plot.Add.ScatterLine(Graph_time, Graph_ch2);
        sig2.LegendText = "Current";
        // sig2.Axes.XAxis = formsPlot1.Plot.Axes.Bottom;
        // sig2.Axes.YAxis = formsPlot1.Plot.Axes.Left;
        // formsPlot1.Plot.Axes.Left.Label.Text = "Current";
      }
      if (cbGraph_ch3.Checked)
      {
        // var sig3 = formsPlot1.Plot.Add.Signal(Graph_ch3);
        var sig3 = formsPlot1.Plot.Add.ScatterLine(Graph_time, Graph_ch3);
        sig3.LegendText = "Speed";
        // sig3.Axes.XAxis = formsPlot1.Plot.Axes.Bottom;
        // sig3.Axes.YAxis = formsPlot1.Plot.Axes.Left;
        // formsPlot1.Plot.Axes.Left.Label.Text = "Speed";
      }
      if (cbGraph_ch4.Checked)
      {
        // var sig4 = formsPlot1.Plot.Add.Signal(Graph_ch4);
        var sig4 = formsPlot1.Plot.Add.ScatterLine(Graph_time, Graph_ch4);
        sig4.LegendText = "Angle";
        // sig4.Axes.XAxis = formsPlot1.Plot.Axes.Bottom;
        // sig4.Axes.YAxis = formsPlot1.Plot.Axes.Left;
        // formsPlot1.Plot.Axes.Left.Label.Text = "Angle";
      }
      if (cbGraph_ch5.Checked)
      {
        // var sig5 = formsPlot1.Plot.Add.Signal(Graph_ch5);
        var sig5 = formsPlot1.Plot.Add.ScatterLine(Graph_time, Graph_ch5);
        sig5.LegendText = "Speed Command";
        // sig5.Axes.XAxis = formsPlot1.Plot.Axes.Bottom;
        // sig5.Axes.YAxis = formsPlot1.Plot.Axes.Left;
        // formsPlot1.Plot.Axes.Left.Label.Text = "Speed Command";
      }
      if (cbGraph_ch6.Checked)
      {
        // var sig6 = formsPlot1.Plot.Add.Signal(Graph_ch6);
        var sig6 = formsPlot1.Plot.Add.ScatterLine(Graph_time, Graph_ch6);
        sig6.LegendText = "Current Command";
        // sig6.Axes.XAxis = formsPlot1.Plot.Axes.Bottom;
        // sig6.Axes.YAxis = formsPlot1.Plot.Axes.Left;
        // formsPlot1.Plot.Axes.Left.Label.Text = "Current Command";
      }
      if (cbGraph_ch7.Checked)
      {
        // var sig7 = formsPlot1.Plot.Add.Signal(Graph_ch7);
        var sig7 = formsPlot1.Plot.Add.ScatterLine(Graph_time, Graph_ch7);
        sig7.LegendText = "SnugAngle";
        // sig7.Axes.XAxis = formsPlot1.Plot.Axes.Bottom;
        // sig7.Axes.YAxis = formsPlot1.Plot.Axes.Left;
        // formsPlot1.Plot.Axes.Left.Label.Text = "SnugAngle";
      }

      // HighlightedPointMarker = formsPlot1.Plot.Add.Marker(0, 0);
      // HighlightedPointMarker.IsVisible = false;
      // HighlightedPointMarker.Size = 15;
      // HighlightedPointMarker.LineWidth = 2;
      // HighlightedPointMarker.Shape = MarkerShape.OpenCircle;

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
      formsPlot1.MouseDown += FormsPlot1_MouseDown;
      formsPlot1.MouseUp += FormsPlot1_MouseUp;
      formsPlot1.MouseMove += FormsPlot1_MouseMove;
    }
    void fresh_graph_data()
    {

      TestUnion d = new TestUnion();
      d.b0 = ComReadBuffer[764 + 0];
      d.b1 = ComReadBuffer[764 + 1];
      d.b2 = ComReadBuffer[764 + 2];
      d.b3 = ComReadBuffer[764 + 3];
      float hss_gain = d.f;
      d.b0 = ComReadBuffer[768 + 0];
      d.b1 = ComReadBuffer[768 + 1];
      d.b2 = ComReadBuffer[768 + 2];
      d.b3 = ComReadBuffer[768 + 3];
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
        d.b0 = ComReadBuffer[100 * 0 + 64 + j * 2 + 0];
        d.b1 = ComReadBuffer[100 * 0 + 64 + j * 2 + 1];
        Data_ch1.Add(d.s0 * tq_gain);//torque
        d.b0 = ComReadBuffer[100 * 1 + 64 + j * 2 + 0];
        d.b1 = ComReadBuffer[100 * 1 + 64 + j * 2 + 1];
        Data_ch2.Add(d.s0 * hss_gain);//current
        d.b0 = ComReadBuffer[100 * 2 + 64 + j * 2 + 0];
        d.b1 = ComReadBuffer[100 * 2 + 64 + j * 2 + 1];
        Data_ch3.Add(d.s0 * 2.0);//speed
        d.b0 = ComReadBuffer[100 * 3 + 64 + j * 2 + 0];
        d.b1 = ComReadBuffer[100 * 3 + 64 + j * 2 + 1];
        Data_ch4.Add(d.s0);//angle
        d.b0 = ComReadBuffer[100 * 4 + 64 + j * 2 + 0];
        d.b1 = ComReadBuffer[100 * 4 + 64 + j * 2 + 1];
        Data_ch5.Add(d.s0 * 2.0);//speed command
        d.b0 = ComReadBuffer[100 * 5 + 64 + j * 2 + 0];
        d.b1 = ComReadBuffer[100 * 5 + 64 + j * 2 + 1];
        Data_ch6.Add(d.s0 * hss_gain);//current command
        d.b0 = ComReadBuffer[100 * 6 + 64 + j * 2 + 0];
        d.b1 = ComReadBuffer[100 * 6 + 64 + j * 2 + 1];
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
      // (SignalXY? sigXY, DataPoint dataPoint) = GetSignalXYUnderMouse(formsPlot1.Plot, e.X, e.Y);
      //   // if (sigXY is null)
      //   //     return;
      // if (sigXY is not null)
      // {
      //   PlottableBeingDragged_XY = sigXY;
      //   StartingDragPosition = dataPoint;
      //   StartingDragOffset = sigXY.Data.XOffset;
      //   formsPlot1.Interaction.Disable(); // disable panning while dragging
      // }
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

      // // update the cursor to reflect what is beneath it
      // if (PlottableBeingDragged_XY is null)
      // {
      //     (var signalUnderMouse, DataPoint dp) = GetSignalXYUnderMouse(formsPlot1.Plot, e.X, e.Y);
      //     Cursor = signalUnderMouse is null ? Cursors.Arrow : Cursors.SizeWE;
      //     HighlightedPointMarker.IsVisible = signalUnderMouse is not null;

      //     if (signalUnderMouse is not null)
      //     {
      //         HighlightedPointMarker.Location = dp.Coordinates;
      //         HighlightedPointMarker.Color = signalUnderMouse.Color;
      //         Text = $"Index {dp.Index} at {dp.Coordinates}";
      //         formsPlot1.Refresh();
      //     }

      //     return;
      // }

      // // update the position of the plottable being dragged
      // if (PlottableBeingDragged_XY is SignalXY sigXY)
      // {
      //     HighlightedPointMarker.IsVisible = false;
      //     sigXY.Data.XOffset = rect.HorizontalCenter - StartingDragPosition.X + StartingDragOffset;
      //     formsPlot1.Refresh();
      // }
      
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

  }
}
