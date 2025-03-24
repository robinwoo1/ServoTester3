using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Reflection.Metadata;

namespace ServoTester3
{
  internal class _Packet
  {
    public const byte ON = 1;
    public const byte OFF = 0;
    public const int _LengthLow = 2;
    public const int _LengthHigh = 3;
    public SerialPort Port { get; } = new SerialPort();

    // public _Packet()
    // {
    //   this.Port = new SerialPort();
    // }

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
