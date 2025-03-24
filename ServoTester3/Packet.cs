using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Collections.Concurrent;
using System.Reflection.Metadata;

namespace ServoTester3
{
  internal class _Packet
  {
    public const ushort SERIAL_BUF_SIZE = 128 * 16;//128*8;
    public const ushort COMMAND_LIST_NUM = 1000;
    public const byte ON = 1;
    public const byte OFF = 0;
    public const int _LengthLow = 2;
    public const int _LengthHigh = 3;
    public SerialPort Port { get; } = new SerialPort();
    public byte[] ComReadBuffer = new byte[128 * 16 * 8];
    public byte[] SendDataPacket = new byte[SERIAL_BUF_SIZE];
    public int ComReadIndex = 0;
    public ushort[,] Command_List_Pc = new ushort[COMMAND_LIST_NUM, 3];
    public ushort Command_Index_Pc;
    public ConcurrentQueue<byte> cq = new ConcurrentQueue<byte>();
    // public _Packet(ref _Parameter _Mc)
    // {
    //   this.Port = new SerialPort();
    // }
    
    // List<double> Graph_time = new List<double>();
    public List<double> Graph_ch1 = new List<double>();
    public List<double> Graph_ch2 = new List<double>();
    public List<double> Graph_ch3 = new List<double>();
    public List<double> Graph_ch4 = new List<double>();
    public List<double> Graph_ch5 = new List<double>();
    public List<double> Graph_ch6 = new List<double>();
    public List<double> Graph_ch7 = new List<double>();
    public List<double> Graph_ch8 = new List<double>();
    public List<double> Data_ch1 = new List<double>();
    public List<double> Data_ch2 = new List<double>();
    public List<double> Data_ch3 = new List<double>();
    public List<double> Data_ch4 = new List<double>();
    public List<double> Data_ch5 = new List<double>();
    public List<double> Data_ch6 = new List<double>();
    public List<double> Data_ch7 = new List<double>();
    public List<double> Data_ch8 = new List<double>();

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
    TestUnion d = new TestUnion();
    
    public void clear_graph_data()
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
    public void clear_data()
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
    public void fresh_graph_data(ref _Parameter Mc)
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

      Mc.Var.refresh_graph_flag = true;
      // this.Invoke(new Action(delegate () // this == Form 이다. Form이 아닌 컨트롤의 Invoke를 직접호출해도 무방하다.
      // {
      //   //Invoke를 통해 lbl_Result 컨트롤에 결과값을 업데이트한다.
      //   Refresh_graph();
      // }));
    }
    public void ProcessPcMcReceivedCommData(ref _Parameter Mc)
    {
      byte data;
      TestUnion d = new TestUnion();
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
                    MakeAndSendData(1, 1, 0, ref Mc);
                  }
                  else if (StartAddress == 4)
                  {
                    AckSend(Command, 0, StartAddress, 0);       // return Ack OK
                    Mc.Info.u16Con_Model_Type = (ushort)((ComReadBuffer[11] << 8) | ComReadBuffer[10]);
                    Mc.Info.u16Version = (ushort)((ComReadBuffer[13] << 8) | ComReadBuffer[12]);
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
                    Mc.Var.Mcinitialized = ComReadBuffer[11];
                    // this.Invoke(new Action(delegate ()
                    // {
                    //   if (Mc.Var.Mcinitialized != 0)
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
                  Mc.Var.TqSensorValue = (ushort)((ComReadBuffer[13] << 8) | ComReadBuffer[12]);

                  Mc.Var.TqSensorOffsetValue = (ushort)((ComReadBuffer[15] << 8) | ComReadBuffer[14]);
                  Mc.DriverInfo.u16TorqueSensorOffset = Mc.Var.TqSensorOffsetValue;

                  Mc.Var.Error = (ushort)((ComReadBuffer[29] << 8) | ComReadBuffer[28]);
                  // tbError.Text = Error.ToString();//ui
                  Mc.Var.IniStep = ComReadBuffer[39];
                  Mc.Var.MaintCnt = (uint)((ComReadBuffer[51] << 24) | (ComReadBuffer[50] << 16) | (ComReadBuffer[49] << 8) | ComReadBuffer[48]);
                  // tbMaintCnt.Text = Mc.Var.MaintCnt.ToString();//ui
                  Mc.Var.Enc = (ushort)((ComReadBuffer[41] << 8) | ComReadBuffer[40]);
                  // tbEnc.Text = Mc.Var.Enc.ToString();//ui

                  Mc.Var.MotorState = ((ComReadBuffer[27] << 8) | ComReadBuffer[26]) != 0;
                  Mc.Flag.b1Run = ComReadBuffer[26];
                  Mc.Flag.b1ControlFL = ComReadBuffer[30];

                  if (ComReadBuffer[42] != 0)
                    Mc.AutoSetting.FlagSetting = true;
                  else
                    Mc.AutoSetting.FlagSetting = false;

                  if (ComReadBuffer[43] != 0)
                    Mc.AutoSetting.FlagStart = true;
                  else
                    Mc.AutoSetting.FlagStart = false;

                  byte b1Run = (byte)(ComReadBuffer[44] & 0x01);
                  if (Mc.Var.FlagRun[0] != b1Run)
                  {
                    MakeAndSendData(2, 2, b1Run, ref Mc);
                  }
                  // Mc.Var.FlagRun[4] = Mc.Var.FlagRun[3];
                  // Mc.Var.FlagRun[3] = Mc.Var.FlagRun[2];
                  Mc.Var.FlagRun[2] = Mc.Var.FlagRun[1];
                  Mc.Var.FlagRun[1] = Mc.Var.FlagRun[0];
                  Mc.Var.FlagRun[0] = (byte)(ComReadBuffer[44] & 0x01);

                  byte b1ControlFL = (byte)(ComReadBuffer[44] & 0x02);
                  if (Mc.Var.FlagFL[0] != b1ControlFL)
                  {
                    if (b1ControlFL != 0)
                      MakeAndSendData(2, 1, 1, ref Mc);
                    else
                      MakeAndSendData(2, 1, 0, ref Mc);
                  }
                  // Mc.Var.FlagFL[4] = Mc.Var.FlagFL[3];
                  // Mc.Var.FlagFL[3] = Mc.Var.FlagFL[2];
                  Mc.Var.FlagFL[2] = Mc.Var.FlagFL[1];
                  Mc.Var.FlagFL[1] = Mc.Var.FlagFL[0];
                  Mc.Var.FlagFL[0] = b1ControlFL;

                  if (ComReadBuffer[63] != 0)
                    Mc.Var.Mot_or_Nut = true;
                  else
                    Mc.Var.Mot_or_Nut = false;

                  break;
                case 4:
                  if (StartAddress == 1)
                  {
                    AckSend(Command, 0, StartAddress, 0);       // return Ack OK
                  }
                  Mc.Var.graph_count++;
                  fresh_graph_data(ref Mc);
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
                    Mc.DriverInfo.u16Type = (ushort)((ComReadBuffer[11] << 8) | ComReadBuffer[10]);
                    Mc.DriverInfo.u16Version = (ushort)((ComReadBuffer[13] << 8) | ComReadBuffer[12]);
                    Mc.DriverInfo.u16Serial_low = (ushort)((ComReadBuffer[15] << 8) | ComReadBuffer[14]);
                    Mc.DriverInfo.u16Serial_high = (ushort)((ComReadBuffer[17] << 8) | ComReadBuffer[16]);
                    Mc.DriverInfo.u8Factory_Gear_efficiency = (ushort)((ComReadBuffer[19] << 8) | ComReadBuffer[18]);
                    Mc.DriverInfo.u8User_Gear_efficiency = (ushort)((ComReadBuffer[21] << 8) | ComReadBuffer[20]);
                    Mc.DriverInfo.u16DriverVendor = (ushort)((ComReadBuffer[23] << 8) | ComReadBuffer[22]);
                    Mc.Var.DriverInfoIsReady = true;
                    if (Mc.Var.IniStep != 11)
                      MakeAndSendData(1, 3, 0, ref Mc);
                  }
                  else if (StartAddress == 3)//Speaker On/Off
                  { }
                  else if (StartAddress == 4)//Led band
                  { }
                  // else if (StartAddress == 5)//Reserved
                  // else if (StartAddress == 6)//Reserved
                  else if (StartAddress == 7)// Get Torque Offset
                  {
                    d.b0 = ComReadBuffer[12];
                    d.b0 = ComReadBuffer[13];
                    d.b0 = ComReadBuffer[14];
                    d.b0 = ComReadBuffer[15];
                    Mc.DriverInfo.f32TorqueOffset = d.f;
                    Mc.Var.DriverInfo_TorqueOffsetIsReady = true;
                  }
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
                    Mc.Var.CalibResultState = (int)((ComReadBuffer[11] << 11) | ComReadBuffer[10]);
                    AckSend(Command, 0, StartAddress, 0);       // return Ack OK
                  }
                  // else if (StartAddress == 13)// Pc -> Mc
                  else if (StartAddress == 101)// Pc <- Mc
                  {
                    int CalibStepState1 = (int)((ComReadBuffer[11] << 8) | ComReadBuffer[10]);
                    if (CalibStepState1 == 0)
                      Mc.Var.CalibStepState = 0;
                    else if (CalibStepState1 == 1)
                      Mc.Var.CalibStepState = 1;
                    else if (CalibStepState1 == 2 || CalibStepState1 == 3)
                      Mc.Var.CalibStepState = 2;
                    else if (CalibStepState1 == 4 || CalibStepState1 == 5)
                      Mc.Var.CalibStepState = 3;
                    else
                      Mc.Var.CalibStepState = 4;
                    AckSend(Command, Try_num, StartAddress, 0);       // return Ack OK
                  }
                  break;
                case 104:
                  // get value
                  // Mc.Var.MotorState = ((ComReadBuffer[3] << 8) | ComReadBuffer[4]) != 0;
                  if (StartAddress == 1)// Pc -> Mc
                  {

                  }
                  else if (StartAddress == 2)// Pc <- Mc
                  {
                    Mc.Var.MotorState = ((ComReadBuffer[11] << 8) | ComReadBuffer[10]) != 0;
                    // Mc.Var.CalibStepState = ((ComReadBuffer[11] << 8) | ComReadBuffer[10]);
                    // Mc.Var.CalibResultState = ((ComReadBuffer[11] << 8) | ComReadBuffer[10]);
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
    public void MakeAndSendData(byte Command, ushort StartAddress, short Data, ref _Parameter Mc)
    {
      ushort u16PtrCnt = 0;
      ushort calc_crc = 0;
      switch (Command)
      {
        case 1:
          if (StartAddress == 1 || StartAddress == 2 || StartAddress == 3)
          {
            MakePacket(Command, StartAddress, Data, ref SendDataPacket, ref Mc);
            u16PtrCnt = CmdAck.u16PtrCnt;
            calc_crc = GetCRC(SendDataPacket, u16PtrCnt + 2);
            SendDataPacket[u16PtrCnt++] = (byte)(calc_crc >> 0);
            SendDataPacket[u16PtrCnt++] = (byte)(calc_crc >> 8);
            SendPacket(SendDataPacket, u16PtrCnt);
          }
          // else if (StartAddress == 4)
          break;
        case 2:
          if (StartAddress == 1 || StartAddress == 2 || StartAddress == 3 || StartAddress == 4 || StartAddress == 5 ||
            StartAddress == 6 || StartAddress == 7 || StartAddress == 8 || StartAddress == 9 || StartAddress == 10)
          {
            MakePacket(Command, StartAddress, Data, ref SendDataPacket, ref Mc);
            u16PtrCnt = CmdAck.u16PtrCnt;
            calc_crc = GetCRC(SendDataPacket, u16PtrCnt + 2);
            SendDataPacket[u16PtrCnt++] = (byte)(calc_crc >> 0);
            SendDataPacket[u16PtrCnt++] = (byte)(calc_crc >> 8);
            SendPacket(SendDataPacket, u16PtrCnt);
          }
          // else if (StartAddress == 11)
          break;
        case 3: // cyclic PC<-MC
          break;
        case 4: // praph PC<-MC
          break;
        case 5: // event PC<-MC
          break;
        case 6:
          if (StartAddress == 1 ||// Sync setting
              StartAddress == 3 ||// Sync resume
              StartAddress == 4)// Sync in event update
          {
            MakePacket(Command, StartAddress, Data, ref SendDataPacket, ref Mc);
            u16PtrCnt = CmdAck.u16PtrCnt;
            calc_crc = GetCRC(SendDataPacket, u16PtrCnt + 2);
            SendDataPacket[u16PtrCnt++] = (byte)(calc_crc >> 0);
            SendDataPacket[u16PtrCnt++] = (byte)(calc_crc >> 8);
            SendPacket(SendDataPacket, u16PtrCnt);
          }
          // else if (StartAddress == 2)// Sync state out PC<-MC
          break;
        case 7:
          if (StartAddress == 1 ||//download driver info
              StartAddress == 3 ||//Speaker & Output
              StartAddress == 4 ||//LED band & output
              StartAddress == 5 ||//request Driver Info
              StartAddress == 6 ||// Set torque Offset
              StartAddress == 7 ||// Get torque Offset
              StartAddress == 8 ||//reset Maintenance count
              StartAddress == 10 ||//Check torque Sensor Offset
              StartAddress == 11 ||//Save torque Sensor Offset
              StartAddress == 12)//Start Initail Angle
          {
            MakePacket(Command, StartAddress, Data, ref SendDataPacket, ref Mc);
            u16PtrCnt = CmdAck.u16PtrCnt;
            calc_crc = GetCRC(SendDataPacket, u16PtrCnt + 2);
            SendDataPacket[u16PtrCnt++] = (byte)(calc_crc >> 0);
            SendDataPacket[u16PtrCnt++] = (byte)(calc_crc >> 8);
            SendPacket(SendDataPacket, u16PtrCnt);
          }
          // else if (StartAddress == 2)//upload driver info
          // else if (StartAddress == 9)//
          // else if (StartAddress == 13)//receive Initial Angle result
          break;
        case 8:
          {
            MakePacket(Command, StartAddress, Data, ref SendDataPacket, ref Mc);
            u16PtrCnt = CmdAck.u16PtrCnt;
            calc_crc = GetCRC(SendDataPacket, u16PtrCnt + 2);
            SendDataPacket[u16PtrCnt++] = (byte)(calc_crc >> 0);
            SendDataPacket[u16PtrCnt++] = (byte)(calc_crc >> 8);
            SendPacket(SendDataPacket, u16PtrCnt);
          }
          break;
        case 9:
          {
            MakePacket(Command, StartAddress, Data, ref SendDataPacket, ref Mc);
            u16PtrCnt = CmdAck.u16PtrCnt;
            calc_crc = GetCRC(SendDataPacket, u16PtrCnt + 2);
            SendDataPacket[u16PtrCnt++] = (byte)(calc_crc >> 0);
            SendDataPacket[u16PtrCnt++] = (byte)(calc_crc >> 8);
            SendPacket(SendDataPacket, u16PtrCnt);
          }
          break;
        case 104:
          {
            MakePacket(Command, StartAddress, Data, ref SendDataPacket, ref Mc);
            u16PtrCnt = CmdAck.u16PtrCnt;
            calc_crc = GetCRC(SendDataPacket, u16PtrCnt + 2);
            SendDataPacket[u16PtrCnt++] = (byte)(calc_crc >> 0);
            SendDataPacket[u16PtrCnt++] = (byte)(calc_crc >> 8);
            SendPacket(SendDataPacket, u16PtrCnt);
          }
          break;
        case 106:
          {
            MakePacket(Command, StartAddress, Data, ref SendDataPacket, ref Mc);
            u16PtrCnt = CmdAck.u16PtrCnt;
            calc_crc = GetCRC(SendDataPacket, u16PtrCnt + 2);
            SendDataPacket[u16PtrCnt++] = (byte)(calc_crc >> 0);
            SendDataPacket[u16PtrCnt++] = (byte)(calc_crc >> 8);
            SendPacket(SendDataPacket, u16PtrCnt);
          }
          break;
        default:
          break;
      }
    }
    public void MakePacket(byte Command, ushort StartAddress, short Data, ref byte[] SendDataPacket, ref _Parameter Mc)
    {
      // ushort data, A1, A2, A3;
      ushort u16PtrCnt = 0;
      ushort Revision = 0;
      byte TryNum = 0;
      ushort u16Value;
      
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
          SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_TCAM_ACTM >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_TCAM_ACTM >> 8);
          d.f = Mc.Para.val.f32MC_FASTEN_TORQUE;
          SendDataPacket[u16PtrCnt++] = d.b0;
          SendDataPacket[u16PtrCnt++] = d.b1;
          SendDataPacket[u16PtrCnt++] = d.b2;
          SendDataPacket[u16PtrCnt++] = d.b3;
          d.f = Mc.Para.val.f32MC_TORQUE_MIN_MAX;
          SendDataPacket[u16PtrCnt++] = d.b0;
          SendDataPacket[u16PtrCnt++] = d.b1;
          SendDataPacket[u16PtrCnt++] = d.b2;
          SendDataPacket[u16PtrCnt++] = d.b3;
          SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_TARGET_ANGLE >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_TARGET_ANGLE >> 8);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_FASTEN_MIN_ANGLE >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_FASTEN_MIN_ANGLE >> 8);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_FASTEN_MAX_ANGLE >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_FASTEN_MAX_ANGLE >> 8);
          d.f = Mc.Para.val.f32MC_SNUG_TORQUE;
          SendDataPacket[u16PtrCnt++] = d.b0;
          SendDataPacket[u16PtrCnt++] = d.b1;
          SendDataPacket[u16PtrCnt++] = d.b2;
          SendDataPacket[u16PtrCnt++] = d.b3;
          SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_FASTEN_SPEED >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_FASTEN_SPEED >> 8);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_FREE_FASTEN_ANGLE >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_FREE_FASTEN_ANGLE >> 8);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_FREE_FASTEN_SPEED >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_FREE_FASTEN_SPEED >> 8);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_SOFT_START >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_SOFT_START >> 8);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_FASTEN_SEATTING_POINT_RATE >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_FASTEN_SEATTING_POINT_RATE >> 8);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_FASTEN_TQ_RISING_TIME >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_FASTEN_TQ_RISING_TIME >> 8);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_RAMP_UP_SPEED >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_RAMP_UP_SPEED >> 8);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_TORQUE_COMPENSATION >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_TORQUE_COMPENSATION >> 8);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_TORQUE_OFFSET >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_TORQUE_OFFSET >> 8);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_MAX_PULSE_COUNT >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_MAX_PULSE_COUNT >> 8);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_SCREW_TYPE >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_SCREW_TYPE >> 8);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_SOFT_STOP >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_SOFT_STOP >> 8);

          SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_ADVANCED_MODE >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_ADVANCED_MODE >> 8);
          d.f = Mc.Para.val.f32MC_ADVANCED_PARA1;
          SendDataPacket[u16PtrCnt++] = d.b0;
          SendDataPacket[u16PtrCnt++] = d.b1;
          SendDataPacket[u16PtrCnt++] = d.b2;
          SendDataPacket[u16PtrCnt++] = d.b3;
          d.f = Mc.Para.val.f32MC_ADVANCED_PARA2;
          SendDataPacket[u16PtrCnt++] = d.b0;
          SendDataPacket[u16PtrCnt++] = d.b1;
          SendDataPacket[u16PtrCnt++] = d.b2;
          SendDataPacket[u16PtrCnt++] = d.b3;
          d.f = Mc.Para.val.f32MC_ADVANCED_PARA3;
          SendDataPacket[u16PtrCnt++] = d.b0;
          SendDataPacket[u16PtrCnt++] = d.b1;
          SendDataPacket[u16PtrCnt++] = d.b2;
          SendDataPacket[u16PtrCnt++] = d.b3;
          d.f = Mc.Para.val.f32MC_ADVANCED_PARA4;
          SendDataPacket[u16PtrCnt++] = d.b0;
          SendDataPacket[u16PtrCnt++] = d.b1;
          SendDataPacket[u16PtrCnt++] = d.b2;
          SendDataPacket[u16PtrCnt++] = d.b3;
          d.f = Mc.Para.val.f32MC_ADVANCED_PARA5;
          SendDataPacket[u16PtrCnt++] = d.b0;
          SendDataPacket[u16PtrCnt++] = d.b1;
          SendDataPacket[u16PtrCnt++] = d.b2;
          SendDataPacket[u16PtrCnt++] = d.b3;
          d.f = Mc.Para.val.f32MC_ADVANCED_PARA6;
          SendDataPacket[u16PtrCnt++] = d.b0;
          SendDataPacket[u16PtrCnt++] = d.b1;
          SendDataPacket[u16PtrCnt++] = d.b2;
          SendDataPacket[u16PtrCnt++] = d.b3;
          d.f = Mc.Para.val.f32MC_ADVANCED_PARA7;
          SendDataPacket[u16PtrCnt++] = d.b0;
          SendDataPacket[u16PtrCnt++] = d.b1;
          SendDataPacket[u16PtrCnt++] = d.b2;
          SendDataPacket[u16PtrCnt++] = d.b3;
          d.f = Mc.Para.val.f32MC_ADVANCED_PARA8;
          SendDataPacket[u16PtrCnt++] = d.b0;
          SendDataPacket[u16PtrCnt++] = d.b1;
          SendDataPacket[u16PtrCnt++] = d.b2;
          SendDataPacket[u16PtrCnt++] = d.b3;
          d.f = Mc.Para.val.f32MC_ADVANCED_PARA9;
          SendDataPacket[u16PtrCnt++] = d.b0;
          SendDataPacket[u16PtrCnt++] = d.b1;
          SendDataPacket[u16PtrCnt++] = d.b2;
          SendDataPacket[u16PtrCnt++] = d.b3;
          d.f = Mc.Para.val.f32MC_ADVANCED_PARA10;
          SendDataPacket[u16PtrCnt++] = d.b0;
          SendDataPacket[u16PtrCnt++] = d.b1;
          SendDataPacket[u16PtrCnt++] = d.b2;
          SendDataPacket[u16PtrCnt++] = d.b3;
          d.f = Mc.Para.val.f32MC_ADVANCED_PARA11;
          SendDataPacket[u16PtrCnt++] = d.b0;
          SendDataPacket[u16PtrCnt++] = d.b1;
          SendDataPacket[u16PtrCnt++] = d.b2;
          SendDataPacket[u16PtrCnt++] = d.b3;
          d.f = Mc.Para.val.f32MC_ADVANCED_PARA12;
          SendDataPacket[u16PtrCnt++] = d.b0;
          SendDataPacket[u16PtrCnt++] = d.b1;
          SendDataPacket[u16PtrCnt++] = d.b2;
          SendDataPacket[u16PtrCnt++] = d.b3;
          d.f = Mc.Para.val.f32MC_ADVANCED_PARA13;
          SendDataPacket[u16PtrCnt++] = d.b0;
          SendDataPacket[u16PtrCnt++] = d.b1;
          SendDataPacket[u16PtrCnt++] = d.b2;
          SendDataPacket[u16PtrCnt++] = d.b3;
          d.f = Mc.Para.val.f32MC_ADVANCED_PARA14;
          SendDataPacket[u16PtrCnt++] = d.b0;
          SendDataPacket[u16PtrCnt++] = d.b1;
          SendDataPacket[u16PtrCnt++] = d.b2;
          SendDataPacket[u16PtrCnt++] = d.b3;
          d.f = Mc.Para.val.f32MC_ADVANCED_PARA15;
          SendDataPacket[u16PtrCnt++] = d.b0;
          SendDataPacket[u16PtrCnt++] = d.b1;
          SendDataPacket[u16PtrCnt++] = d.b2;
          SendDataPacket[u16PtrCnt++] = d.b3;
          d.f = Mc.Para.val.f32MC_ADVANCED_PARA16;
          SendDataPacket[u16PtrCnt++] = d.b0;
          SendDataPacket[u16PtrCnt++] = d.b1;
          SendDataPacket[u16PtrCnt++] = d.b2;
          SendDataPacket[u16PtrCnt++] = d.b3;
          d.f = Mc.Para.val.f32MC_ADVANCED_PARA17;
          SendDataPacket[u16PtrCnt++] = d.b0;
          SendDataPacket[u16PtrCnt++] = d.b1;
          SendDataPacket[u16PtrCnt++] = d.b2;
          SendDataPacket[u16PtrCnt++] = d.b3;
          d.f = Mc.Para.val.f32MC_ADVANCED_PARA18;
          SendDataPacket[u16PtrCnt++] = d.b0;
          SendDataPacket[u16PtrCnt++] = d.b1;
          SendDataPacket[u16PtrCnt++] = d.b2;
          SendDataPacket[u16PtrCnt++] = d.b3;
          d.f = Mc.Para.val.f32MC_ADVANCED_PARA19;
          SendDataPacket[u16PtrCnt++] = d.b0;
          SendDataPacket[u16PtrCnt++] = d.b1;
          SendDataPacket[u16PtrCnt++] = d.b2;
          SendDataPacket[u16PtrCnt++] = d.b3;
          SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_FREE_REVERSE_ROTATION_SPEED >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_FREE_REVERSE_ROTATION_SPEED >> 8);
          d.f = Mc.Para.val.f32MC_FREE_REVERSE_ROTATION_ANGLE;
          SendDataPacket[u16PtrCnt++] = d.b0;
          SendDataPacket[u16PtrCnt++] = d.b1;
          SendDataPacket[u16PtrCnt++] = d.b2;
          SendDataPacket[u16PtrCnt++] = d.b3;
          SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_REVERS_ANGLE_SETTING_SPEED >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_REVERS_ANGLE_SETTING_SPEED >> 8);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_REVERS_ANGLE_SETTING_ANGLE >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_REVERS_ANGLE_SETTING_ANGLE >> 8);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_REVERS_ANGLE_SETTING_FW_REV >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_REVERS_ANGLE_SETTING_FW_REV >> 8);

          if (StartAddress == 1)
          {
            SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_UNIT >> 0);
            SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_UNIT >> 8);
            SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_ACC_DEC_TIME >> 0);
            SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_ACC_DEC_TIME >> 8);
            SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_FASTEN_TORQUE_MAINTAIN_TIME >> 0);
            SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_FASTEN_TORQUE_MAINTAIN_TIME >> 8);
            SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_USE_MAXTQ_FOR_LOOSENING >> 0);
            SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_USE_MAXTQ_FOR_LOOSENING >> 8);
            SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_LOOSENING_SPEED >> 0);
            SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_LOOSENING_SPEED >> 8);
            d.f = Mc.Para.val.f32MC_TOTAL_FASTENING_TIME;
            SendDataPacket[u16PtrCnt++] = d.b0;
            SendDataPacket[u16PtrCnt++] = d.b1;
            SendDataPacket[u16PtrCnt++] = d.b2;
            SendDataPacket[u16PtrCnt++] = d.b3;
            d.f = Mc.Para.val.f32MC_TOTAL_LOOSENING_TIME;
            SendDataPacket[u16PtrCnt++] = d.b0;
            SendDataPacket[u16PtrCnt++] = d.b1;
            SendDataPacket[u16PtrCnt++] = d.b2;
            SendDataPacket[u16PtrCnt++] = d.b3;
            d.f = Mc.Para.val.f32MC_STALL_LOOSENING_TIME_LIMIT;
            SendDataPacket[u16PtrCnt++] = d.b0;
            SendDataPacket[u16PtrCnt++] = d.b1;
            SendDataPacket[u16PtrCnt++] = d.b2;
            SendDataPacket[u16PtrCnt++] = d.b3;
            // SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_SCREW_TYPE >> 0);
            // SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_SCREW_TYPE >> 8);
            d.f = Mc.Para.val.f32MC_JUDGE_FASTEN_MIN_TURNS;
            SendDataPacket[u16PtrCnt++] = d.b0;
            SendDataPacket[u16PtrCnt++] = d.b1;
            SendDataPacket[u16PtrCnt++] = d.b2;
            SendDataPacket[u16PtrCnt++] = d.b3;
            SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_FASTENING_STOP_ALARM >> 0);
            SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_FASTENING_STOP_ALARM >> 8);
            SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_TORQUE_COMPENSATION_MAIN >> 0);
            SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_TORQUE_COMPENSATION_MAIN >> 8);
            SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_CROWFOOT_ENABLE >> 0);
            SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_CROWFOOT_ENABLE >> 8);
            d.f = Mc.Para.val.f32MC_CROWFOOT_RATIO;
            SendDataPacket[u16PtrCnt++] = d.b0;
            SendDataPacket[u16PtrCnt++] = d.b1;
            SendDataPacket[u16PtrCnt++] = d.b2;
            SendDataPacket[u16PtrCnt++] = d.b3;
            SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_CROWFOOT_EFFICIENCY >> 0);
            SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_CROWFOOT_EFFICIENCY >> 8);
            d.f = Mc.Para.val.f32MC_CROWFOOT_REVERSE_TORQUE;
            SendDataPacket[u16PtrCnt++] = d.b0;
            SendDataPacket[u16PtrCnt++] = d.b1;
            SendDataPacket[u16PtrCnt++] = d.b2;
            SendDataPacket[u16PtrCnt++] = d.b3;
            SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_CROWFOOT_REVERSE_SPEED >> 0);
            SendDataPacket[u16PtrCnt++] = (byte)(Mc.Para.val.u16MC_CROWFOOT_REVERSE_SPEED >> 8);
            d.f = Mc.Para.val.f32MC_FREE_SPEED_MAX_TORQUE;
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
          SendDataPacket[u16PtrCnt++] = (byte)(Mc.Info_DrvModel_para.u16Driver_id >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc.Info_DrvModel_para.u16Driver_id >> 8);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc.Info_DrvModel_para.u16Driver_vendor_id >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc.Info_DrvModel_para.u16Driver_vendor_id >> 8);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc.Info_DrvModel_para.u16Controller_id >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc.Info_DrvModel_para.u16Controller_id >> 8);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc.Info_DrvModel_para.u16Motor_id >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc.Info_DrvModel_para.u16Motor_id >> 8);

          d.f = Mc.Info_DrvModel_para.f32Tq_min_Nm;
          SendDataPacket[u16PtrCnt++] = d.b0;
          SendDataPacket[u16PtrCnt++] = d.b1;
          SendDataPacket[u16PtrCnt++] = d.b2;
          SendDataPacket[u16PtrCnt++] = d.b3;
          d.f = Mc.Info_DrvModel_para.f32Tq_max_Nm;
          SendDataPacket[u16PtrCnt++] = d.b0;
          SendDataPacket[u16PtrCnt++] = d.b1;
          SendDataPacket[u16PtrCnt++] = d.b2;
          SendDataPacket[u16PtrCnt++] = d.b3;
          d.u = Mc.Info_DrvModel_para.u32Speed_min;
          SendDataPacket[u16PtrCnt++] = d.b0;
          SendDataPacket[u16PtrCnt++] = d.b1;
          SendDataPacket[u16PtrCnt++] = d.b2;
          SendDataPacket[u16PtrCnt++] = d.b3;
          d.u = Mc.Info_DrvModel_para.u32Speed_max;
          SendDataPacket[u16PtrCnt++] = d.b0;
          SendDataPacket[u16PtrCnt++] = d.b1;
          SendDataPacket[u16PtrCnt++] = d.b2;
          SendDataPacket[u16PtrCnt++] = d.b3;
          d.f = Mc.Info_DrvModel_para.f32Gear_ratio;
          SendDataPacket[u16PtrCnt++] = d.b0;
          SendDataPacket[u16PtrCnt++] = d.b1;
          SendDataPacket[u16PtrCnt++] = d.b2;
          SendDataPacket[u16PtrCnt++] = d.b3;
          d.f = Mc.Info_DrvModel_para.f32Angle_head_ratio;
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
            SendDataPacket[u16PtrCnt++] = (byte)(Mc.Flag.b1ControlFL);//(Data >> 0);
            SendDataPacket[u16PtrCnt++] = (byte)0;
            SendDataPacket[u16PtrCnt++] = (byte)0;
            SendDataPacket[u16PtrCnt++] = (byte)0;
            break;
          case 2://Start/Stop
            if (Data != 0)//start
            {
              SendDataPacket[u16PtrCnt++] = (byte)(Data >> 0);
              SendDataPacket[u16PtrCnt++] = (byte)(Mc.Flag.LoosenAngle >> 0);
              SendDataPacket[u16PtrCnt++] = (byte)(Mc.Flag.LoosenAngle >> 8);
              SendDataPacket[u16PtrCnt++] = (byte)Mc.Var.SoftStop;
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
          SendDataPacket[u16PtrCnt++] = Mc.SyncStruct.Bits_b1OnOff;
          SendDataPacket[u16PtrCnt++] = Mc.SyncStruct.Bits_b1Master;
          SendDataPacket[u16PtrCnt++] = (byte)(Mc.SyncStruct.u16WaitingBeforeSync >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc.SyncStruct.u16WaitingBeforeSync >> 8);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc.SyncStruct.u16WaitingBetweenSync >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc.SyncStruct.u16WaitingBetweenSync >> 8);
          SendDataPacket[u16PtrCnt++] = (byte)0;
          SendDataPacket[u16PtrCnt++] = (byte)0;
        }
        // else if (StartAddress == 2)// Sync state out PC<-MC
        else if (StartAddress == 3)// Sync resume
        {
          SendDataPacket[u16PtrCnt++] = Mc.SyncStruct.Bits_b1ResumeOnOff;
          SendDataPacket[u16PtrCnt++] = (byte)0;
          SendDataPacket[u16PtrCnt++] = (byte)0;
          SendDataPacket[u16PtrCnt++] = (byte)0;
        }
        else if (StartAddress == 4)// Sync in event update
        {
          SendDataPacket[u16PtrCnt++] = Mc.SyncStruct.Bits_b1SyncIn;
          SendDataPacket[u16PtrCnt++] = (byte)0;
          SendDataPacket[u16PtrCnt++] = (byte)0;
          SendDataPacket[u16PtrCnt++] = (byte)0;
        }
      }
      else if (Command == 7) // parameter
      {
        if (StartAddress == 1) // Download Driver info
        {
          SendDataPacket[u16PtrCnt++] = (byte)(Mc.outDriverInfo.u16Type >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc.outDriverInfo.u16Type >> 8);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc.outDriverInfo.u16Version >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc.outDriverInfo.u16Version >> 8);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc.outDriverInfo.u16Serial_low >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc.outDriverInfo.u16Serial_low >> 8);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc.outDriverInfo.u16Serial_high >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc.outDriverInfo.u16Serial_high >> 8);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc.outDriverInfo.u8Factory_Gear_efficiency >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(0);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc.outDriverInfo.u8User_Gear_efficiency >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(0);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc.outDriverInfo.u16DriverVendor >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc.outDriverInfo.u16DriverVendor >> 8);
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
        else if (StartAddress == 5)//request Driver Info
        {
          SendDataPacket[u16PtrCnt++] = (byte)(0);
          SendDataPacket[u16PtrCnt++] = (byte)(0);
          SendDataPacket[u16PtrCnt++] = (byte)(0);
          SendDataPacket[u16PtrCnt++] = (byte)(0);
        }
        else if (StartAddress == 6)//Set Torque Offset
        {
          d.f = Mc.outDriverInfo.f32TorqueOffset;
          SendDataPacket[u16PtrCnt++] = (byte)(0);
          SendDataPacket[u16PtrCnt++] = (byte)(0);
          SendDataPacket[u16PtrCnt++] = d.b0;
          SendDataPacket[u16PtrCnt++] = d.b1;
          SendDataPacket[u16PtrCnt++] = d.b2;
          SendDataPacket[u16PtrCnt++] = d.b3;
          SendDataPacket[u16PtrCnt++] = (byte)(0);
          SendDataPacket[u16PtrCnt++] = (byte)(0);
        }
        else if (StartAddress == 7)//Get Torque Offset
        {
          SendDataPacket[u16PtrCnt++] = (byte)(0);
          SendDataPacket[u16PtrCnt++] = (byte)(0);
          SendDataPacket[u16PtrCnt++] = (byte)(0);
          SendDataPacket[u16PtrCnt++] = (byte)(0);
          SendDataPacket[u16PtrCnt++] = (byte)(0);
          SendDataPacket[u16PtrCnt++] = (byte)(0);
          SendDataPacket[u16PtrCnt++] = (byte)(0);
          SendDataPacket[u16PtrCnt++] = (byte)(0);
        }
        else if (StartAddress == 8)//Reset maintenance count
        {
          SendDataPacket[u16PtrCnt++] = (byte)(1);
          SendDataPacket[u16PtrCnt++] = (byte)(0);
          SendDataPacket[u16PtrCnt++] = (byte)(0);
          SendDataPacket[u16PtrCnt++] = (byte)(0);
        }
        // else if (StartAddress == 9)//
        else if (StartAddress == 10)//Check torque sensor OffsetADC
        {
          SendDataPacket[u16PtrCnt++] = (byte)(0);//(byte)(Mc.outDriverInfo.u16TorqueSensorOffset >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(0);//(byte)(Mc.outDriverInfo.u16TorqueSensorOffset >> 8);
          SendDataPacket[u16PtrCnt++] = (byte)(0);
          SendDataPacket[u16PtrCnt++] = (byte)(0);
        }
        else if (StartAddress == 11)//Save torque sensor OffsetADC
        {
          SendDataPacket[u16PtrCnt++] = (byte)(Mc.outDriverInfo.u16TorqueSensorOffset >> 0);
          SendDataPacket[u16PtrCnt++] = (byte)(Mc.outDriverInfo.u16TorqueSensorOffset >> 8);
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
            u16Value = Mc.Gain.Tq_Kp;//(ushort)UInt16.Parse(tbTorquePgain.Text);
            SendDataPacket[u16PtrCnt++] = (byte)(u16Value >> 0);//10
            SendDataPacket[u16PtrCnt++] = (byte)(u16Value >> 8);
            u16Value = Mc.Gain.Tq_Ki;//(ushort)UInt16.Parse(tbTorqueIgain.Text);
            SendDataPacket[u16PtrCnt++] = (byte)(u16Value >> 0);//12
            SendDataPacket[u16PtrCnt++] = (byte)(u16Value >> 8);
            u16Value = Mc.Gain.Tq_Kf;//(ushort)UInt16.Parse(tbTorqueFFgain.Text);
            SendDataPacket[u16PtrCnt++] = (byte)(u16Value >> 0);//14
            SendDataPacket[u16PtrCnt++] = (byte)(u16Value >> 8);
            u16Value = Mc.Gain.Sp_Kp;//(ushort)UInt16.Parse(tbSpeedPgain.Text);
            SendDataPacket[u16PtrCnt++] = (byte)(u16Value >> 0);//16
            SendDataPacket[u16PtrCnt++] = (byte)(u16Value >> 8);
            u16Value = Mc.Gain.Sp_Ki;//(ushort)UInt16.Parse(tbSpeedIgain.Text);
            SendDataPacket[u16PtrCnt++] = (byte)(u16Value >> 0);//18
            SendDataPacket[u16PtrCnt++] = (byte)(u16Value >> 8);
            u16Value = Mc.Gain.Sp_Kf;//(ushort)UInt16.Parse(tbSpeedFFgain.Text);
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
    public void ResetAckState()
    {
      CmdAck.u8Command = 0;
      CmdAck.u8AckWait = OFF;
      CmdAck.u16StartAddress = 0;
      CmdAck.u16PtrCnt = 0;
    }
    // send ack code
    public void AckSend(byte command, byte Try_num, ushort StartAddress, byte code)
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
    public CmdAck_ CmdAck = new CmdAck_(0, 0, 0);
    public ushort GetCRC(byte[] data, int Length)
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
  }
}
