using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ServoTester3.Form1;

namespace ServoTester3
{
  internal class _Parameter
  {
    public _Parameter()
    {
      this.Var = new _Var();
      this.Gain = new _Gain();
      this.AutoSetting = new _auto_setting();
      this.Info = new _InfoStruct();
      this.SyncStruct = new _SyncStruct();
      this.Info_DrvModel_para = new _dr_model(0);
      this.Flag = new _Flag();
      this.outDriverInfo = new _DriverInfoStruct(0);
      this.DriverInfo = new _DriverInfoStruct(0);
      this.Para = new _para();
    }
    public struct _Var
    {
      public int CalibStepState;// { CALIB_SUCCESS, CALIB_FAIL, CALIB_USERSTOP }
      public int CalibResultState;// { get; set; }
      public int graph_count;
      public bool Mot_or_Nut;//false;
      public byte[] FlagFL;// = new byte[10];
      public byte[] FlagRun;// = new byte[10];
      public bool MotorState { get; set; }
      public ushort Enc;
      public uint MaintCnt;
      public ushort Error;
      public ushort TqSensorOffsetValue;
      public ushort TqSensorValue;
      public ushort Mcinitialized;
      public bool refresh_graph_flag;
      public byte IniStep;
      public bool DriverInfoIsReady;
      public bool DriverInfo_TorqueOffsetIsReady;
      public byte SoftStop;
      public _Var()
      {
        this.CalibStepState = 0;
        this.CalibResultState = 0;
        this.graph_count = 0;
        this.Mot_or_Nut = true;
        this.FlagFL = new byte[10];
        this.FlagRun = new byte[10];
        this.MotorState = false;
        this.Enc = 0;
        this.MaintCnt = 0;
        this.Error = 0;
        this.TqSensorOffsetValue = 0;
        this.TqSensorValue = 0;
        this.Mcinitialized = 0;
        this.refresh_graph_flag = false;
        this.IniStep = 0;
        this.DriverInfoIsReady = false;
        this.DriverInfo_TorqueOffsetIsReady = false;
        this.SoftStop = 0;
      }
    }
    public _Var Var;
    public struct _Gain
    {
      public short Speed;
      public short Torque;
      public ushort Tq_Kp;
      public ushort Tq_Ki;
      public ushort Tq_Kf;
      public ushort Sp_Kp;
      public ushort Sp_Ki;
      public ushort Sp_Kf;
      public _Gain()
      {
        this.Speed = 0;
        this.Torque = 0;
        this.Tq_Kp = 100;
        this.Tq_Ki = 100;
        this.Tq_Kf = 100;
        this.Sp_Kp = 100;
        this.Sp_Ki = 100;
        this.Sp_Kf = 100;
      }
    }
    public _Gain Gain;// = new _Gain();
    public struct _auto_setting
    {
      public bool FlagSetting;
      public bool FlagStart;
      public ushort CurrentSpeed;
      public ushort CurrentSeatingPoint;
      public ushort CurrentFSpeed;
      public ushort CurrentFAngle;
    }
    public _auto_setting AutoSetting;
    public void InitAutoSetting()
    {
      AutoSetting.FlagSetting = false;
      AutoSetting.FlagStart = false;
      AutoSetting.CurrentSpeed = 0;
      AutoSetting.CurrentSeatingPoint = 0;
      AutoSetting.CurrentFSpeed = 0;
      AutoSetting.CurrentFAngle = 0;
    }
    public struct _InfoStruct
    {
      public ushort u16Con_Model_Type;
      public ushort u16Version;
      public ushort u16Serial_low;
      public ushort u16Serial_high;
    }
    public _InfoStruct Info;
    public void InitMcInfo()
    {
      Info.u16Con_Model_Type = 0;
      Info.u16Version = 0;
      Info.u16Serial_low = 0;
      Info.u16Serial_high = 0;
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
    public _SyncStruct SyncStruct;
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
    public _dr_model Info_DrvModel_para;// = new _dr_model(0);
    
    public void InitInfo_DrvModel_para(ushort u16Driver_id_)
    {
      Info_DrvModel_para.u16Driver_id = u16Driver_id_;
      Info_DrvModel_para.u16Driver_vendor_id = 2;//1:hantas, 2:torero
      Info_DrvModel_para.u16Controller_id = 1;      // controller model no. 1:26, 2:32
      Info_DrvModel_para.u16Motor_id = 2;          // used motor no.       1:26, 2:32
                                                   // // TORQUE / SPEED
                                                   // Mc.Info_DrvModel_para.f32Tq_min_Nm = 15;         // default Nm
                                                   // Mc.Info_DrvModel_para.f32Tq_max_Nm = 80;         // default Nm
                                                   // Mc.Info_DrvModel_para.u32Speed_min = 50;
                                                   // Mc.Info_DrvModel_para.u32Speed_max = 475;
                                                   // SETING
      switch (u16Driver_id_)
      {
        case 1://30
               // TORQUE
          Info_DrvModel_para.f32Tq_min_Nm = 7.0f;           // default Nm
          Info_DrvModel_para.f32Tq_max_Nm = 35.0f;          // default Nm
                                                            // SPEED
          Info_DrvModel_para.u32Speed_min = 50;
          Info_DrvModel_para.u32Speed_max = 1090;
          // Gear
          Info_DrvModel_para.f32Gear_ratio = 21.1923055f;//48.8163261f;
          Info_DrvModel_para.f32Angle_head_ratio = 1.545455f;//1.54545498f;
          break;
        case 2://40
               // TORQUE
          Info_DrvModel_para.f32Tq_min_Nm = 8.0f;           // default Nm
          Info_DrvModel_para.f32Tq_max_Nm = 40.0f;          // default Nm
                                                            // SPEED
          Info_DrvModel_para.u32Speed_min = 50;
          Info_DrvModel_para.u32Speed_max = 1090;
          // Gear
          Info_DrvModel_para.f32Gear_ratio = 21.1923055f;//4.461538f * 4.75f;
          Info_DrvModel_para.f32Angle_head_ratio = 1.545455f;//1.54545498f;
          break;
        case 3://50
               // TORQUE
          Info_DrvModel_para.f32Tq_min_Nm = 10.0f;          // default Nm
          Info_DrvModel_para.f32Tq_max_Nm = 55.0f;          // default Nm
                                                            // SPEED
          Info_DrvModel_para.u32Speed_min = 50;
          Info_DrvModel_para.u32Speed_max = 655;
          // Gear
          Info_DrvModel_para.f32Gear_ratio = 35.2857123f;//7.428571f * 4.75f;
          Info_DrvModel_para.f32Angle_head_ratio = 1.545455f;//1.54545498f;
          break;
        case 4://70
               // TORQUE
          Info_DrvModel_para.f32Tq_min_Nm = 15.0f;          // default Nm
          Info_DrvModel_para.f32Tq_max_Nm = 80.0f;          // default Nm
                                                            // SPEED
          Info_DrvModel_para.u32Speed_min = 50;
          Info_DrvModel_para.u32Speed_max = 475;
          // Gear
          Info_DrvModel_para.f32Gear_ratio = 48.8163269f;//48.8163261f;
          Info_DrvModel_para.f32Angle_head_ratio = 1.545455f;//1.54545498f;
          break;
        case 5://100
               // TORQUE
          Info_DrvModel_para.f32Tq_min_Nm = 20.0f;          // default Nm
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
          Info_DrvModel_para.f32Tq_min_Nm = 30.0f;          // default Nm
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
          Info_DrvModel_para.f32Tq_min_Nm = 35.0f;          // default Nm
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
          Info_DrvModel_para.f32Tq_min_Nm = 40.0f;          // default Nm
          Info_DrvModel_para.f32Tq_max_Nm = 200.0f;         // default Nm
                                                            // SPEED
          Info_DrvModel_para.u32Speed_min = 50;
          Info_DrvModel_para.u32Speed_max = 185;
          // Gear
          Info_DrvModel_para.f32Gear_ratio = 103.999994f;//48.8163261f;
          Info_DrvModel_para.f32Angle_head_ratio = 1.8f;//1.54545498f;
          break;
        case 9://old version
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
        case 10:// coreless
          Info_DrvModel_para.u16Driver_id = u16Driver_id_;
          Info_DrvModel_para.u16Driver_vendor_id = 1;//1:hantas, 2:torero
          Info_DrvModel_para.u16Controller_id = 1;      // controller model no. 1:26, 2:32
          Info_DrvModel_para.u16Motor_id = 3;          // used motor no.       1:26, 2:32
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
      // Mc.Info_DrvModel_para.f32Gear_ratio = 48.8163261f;
      // Mc.Info_DrvModel_para.f32Angle_head_ratio = 1.54545498f;
    }
    public struct _Flag
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
    public _Flag Flag;
    public void InitMcFlag()
    {
      Flag.b1Run = 0;   // #00
      Flag.b1Reset = 0;   // #01
      Flag.b1ControlFL = 0;   // #02     Forward/reverse distinction.
      Flag.LoosenAngle = 0;
      Flag.b1Lock = 0;   // #03     Run driver lock.
      Flag.b1Stopping = 0;   // #04     stop process start
      Flag.b1Multi_Mode = 0;   // #05     select mult mode
      Flag.b1Multi_Start = 0;   // #06     start mult sequence by IO or start switch
      Flag.b1TorqueUpCompleteOut = 0;   // #07
      Flag.b1FasteningCompleteOut = 0;   // #08
      Flag.b2LockCommand = 0;   // #09 #10 driver lock type
      Flag.b1Buzzer = 0;   // #11     buzzer control
      Flag.b1ReceiveModBusData = 0;   // #12
      Flag.b1InternalRun = 0;   // #13     driver start switch
      Flag.b1ExternalRun = 0;   // #14     IO start
      Flag.b1RunByMult = 0;   // #15     Run inside Multisequence start..
      Flag.b1JabCompliteIoOut = 0;   // #16     Flag_JabCompliteIOOut io output..
      Flag.b1FirmwareUpdate = 0;   // #17 
      Flag.b1CountStartSensorSignalResult = 0;   // #18 A signal considering the delay time of the sensor input.
      Flag.b1ParaStartInitialize = 0;   // #19
      Flag.b1ParaInitialized = 0;   // #20
      Flag.b1SaveDrvModel = 0;   // #21
      Flag.b1OneTimeExecute = 0;   // #22 Executed only once during initial booting.
      Flag.b1ResetSystem = 0;   // #23 reset System.
      Flag.b1SendHostCTqNotComplete = 0;   // #24 Step definition that increases c tq value..
      Flag.b1FasteningStopAlarm = 0;   // #25 Stop before fastening after start..
      Flag.b1FoundEngagingTorque = 0;   // #26
      Flag.b1Reached_LITTLE_REWIND = 0;   // #27 if error appier display torque.
      Flag.b1DriverParaInit = 0;   // #28 driver parameter init request
      Flag.b1DriverSaveParaData = 0;   // #29
      Flag.b1EnableCyclic = 0;   // #30 enable cyclic
      Flag.b1Ready = 0;
    }
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
      public ushort u16TorqueSensorOffset;              // 10
      public ushort u16LED_Band;                  // 11
      public ushort u16Temperature;               // 12
      public ushort u16Initial_Angle;             // 13
      public ushort u16Error;                     // 14
      public ushort u16TorqueOffset_low;          // 15
      public ushort u16TorqueOffset_high;         // 16
      public float f32TorqueOffset;
      public ushort u16DriverVendor;              // 24
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
        this.u16TorqueSensorOffset = 32768;
        this.u16LED_Band = 0;
        this.u16Temperature = 0;
        this.u16Initial_Angle = 0;
        this.u16Error = 0;
        this.u16TorqueOffset_low = 0;
        this.u16TorqueOffset_high = 0;
        this.u16DriverVendor = 0;
      }
    }
    public _DriverInfoStruct outDriverInfo;// = new _DriverInfoStruct(0);
    public _DriverInfoStruct DriverInfo;// = new _DriverInfoStruct(0);
    
    public void InitDriverInfo(ushort u16Type_)
    {
      outDriverInfo.u16Type = u16Type_;//1;
      outDriverInfo.u16Version = 123;
      outDriverInfo.u8Factory_Gear_efficiency = 100;
      outDriverInfo.u8User_Gear_efficiency = 100;
      outDriverInfo.u16Serial_low = 1234;
      outDriverInfo.u16Serial_high = 5678;
      outDriverInfo.u16MaintenanceCount_low = 0;
      outDriverInfo.u16MaintenanceCount_high = 0;
      outDriverInfo.u16WarningMaintenanceCount = 0;
      outDriverInfo.u16TorqueSensorOffset = 32768;
      outDriverInfo.u16LED_Band = 0;
      outDriverInfo.u16Temperature = 0;
      outDriverInfo.u16Initial_Angle = 0;
      outDriverInfo.u16Error = 0;
    }
    public struct _para_member
    {
      public ushort u16MC_ZERO_DUMMY;
      public ushort u16MC_TCAM_ACTM;                  //1
      public float f32MC_FASTEN_TORQUE;               //2
      public float f32MC_TORQUE_MIN_MAX;              //3
      public ushort u16MC_TARGET_ANGLE;               //4
      public ushort u16MC_FASTEN_MIN_ANGLE;           //5
      public ushort u16MC_FASTEN_MAX_ANGLE;           //6
      public float f32MC_SNUG_TORQUE;                 //7
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
      public ushort u16MC_SCREW_TYPE;                 //18
      public ushort u16MC_SOFT_STOP;                  //19
      public ushort u16MC_ADVANCED_MODE;                //0
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
      public ushort u16MC_FREE_REVERSE_ROTATION_SPEED;  //1
      public float f32MC_FREE_REVERSE_ROTATION_ANGLE;   //2
      public ushort u16MC_REVERS_ANGLE_SETTING_SPEED;   //3
      public ushort u16MC_REVERS_ANGLE_SETTING_ANGLE;   //4
      public ushort u16MC_REVERS_ANGLE_SETTING_FW_REV;  //5
      public ushort u16MC_DRIVER_MODEL;                 //0
      public ushort u16MC_UNIT;                         //1
      public ushort u16MC_ACC_DEC_TIME;                 //2
      public ushort u16MC_FASTEN_TORQUE_MAINTAIN_TIME;  //3
      public ushort u16MC_USE_MAXTQ_FOR_LOOSENING;      //4
      public ushort u16MC_LOOSENING_SPEED;              //5
      public float f32MC_TOTAL_FASTENING_TIME;          //6
      public float f32MC_TOTAL_LOOSENING_TIME;          //7
      public float f32MC_STALL_LOOSENING_TIME_LIMIT;    //8
      public float f32MC_JUDGE_FASTEN_MIN_TURNS;        //9
      public ushort u16MC_FASTENING_STOP_ALARM;         //10
      public ushort u16MC_TORQUE_COMPENSATION_MAIN;     //11
      public ushort u16MC_CROWFOOT_ENABLE;              //12
      public float f32MC_CROWFOOT_RATIO;                //13
      public ushort u16MC_CROWFOOT_EFFICIENCY;          //14
      public float f32MC_CROWFOOT_REVERSE_TORQUE;       //15
      public ushort u16MC_CROWFOOT_REVERSE_SPEED;       //16
      public float f32MC_FREE_SPEED_MAX_TORQUE;         //17
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
    public _para Para;

    public void InitParameter(ushort u16MC_DRIVER_MODEL_)
    {
      Para.dft.u16MC_ZERO_DUMMY = 0;                 Para.min.u16MC_ZERO_DUMMY = 0;                 Para.max.u16MC_ZERO_DUMMY = 1;                 //dummy
      Para.dft.u16MC_TCAM_ACTM = 0;                  Para.min.u16MC_TCAM_ACTM = 0;                  Para.max.u16MC_TCAM_ACTM = 1;                  //SET[01] :i select torque/angle
      Para.dft.f32MC_FASTEN_TORQUE = 50;             Para.min.f32MC_FASTEN_TORQUE = 30;             Para.max.f32MC_FASTEN_TORQUE = 500;            //SET[02] :f toque [Nm*100]
      Para.dft.f32MC_TORQUE_MIN_MAX = 1000;          Para.min.f32MC_TORQUE_MIN_MAX = 0;             Para.max.f32MC_TORQUE_MIN_MAX = 10000;         //SET[03] : %  (Actually use value when initializing para)
      Para.dft.u16MC_TARGET_ANGLE = 0;               Para.min.u16MC_TARGET_ANGLE = 0;               Para.max.u16MC_TARGET_ANGLE = 20000;           //SET[04] : degree
      Para.dft.u16MC_FASTEN_MIN_ANGLE = 0;           Para.min.u16MC_FASTEN_MIN_ANGLE = 0;           Para.max.u16MC_FASTEN_MIN_ANGLE = 20000;       //SET[05] : 
      Para.dft.u16MC_FASTEN_MAX_ANGLE = 0;           Para.min.u16MC_FASTEN_MAX_ANGLE = 0;           Para.max.u16MC_FASTEN_MAX_ANGLE = 20000;       //SET[06] : 
      Para.dft.f32MC_SNUG_TORQUE = 0;                Para.min.f32MC_SNUG_TORQUE = 0;                Para.max.f32MC_SNUG_TORQUE = 100;              //SET[07] : %
      Para.dft.u16MC_FASTEN_SPEED = 300;             Para.min.u16MC_FASTEN_SPEED = 100;             Para.max.u16MC_FASTEN_SPEED = 2000;            //SET[08] : speed[RPM]
      Para.dft.u16MC_FREE_FASTEN_ANGLE = 0;          Para.min.u16MC_FREE_FASTEN_ANGLE = 0;          Para.max.u16MC_FREE_FASTEN_ANGLE = 20000;      //SET[09] : degree
      Para.dft.u16MC_FREE_FASTEN_SPEED = 0;          Para.min.u16MC_FREE_FASTEN_SPEED = 0;          Para.max.u16MC_FREE_FASTEN_SPEED = 1000;       //SET[10] : 
      Para.dft.u16MC_SOFT_START = 100;               Para.min.u16MC_SOFT_START = 0;                 Para.max.u16MC_SOFT_START = 300;               //SET[11] : 
      Para.dft.u16MC_FASTEN_SEATTING_POINT_RATE = 40;Para.min.u16MC_FASTEN_SEATTING_POINT_RATE = 10;Para.max.u16MC_FASTEN_SEATTING_POINT_RATE = 95;//SET[12] : %
      Para.dft.u16MC_FASTEN_TQ_RISING_TIME = 50;     Para.min.u16MC_FASTEN_TQ_RISING_TIME = 50;     Para.max.u16MC_FASTEN_TQ_RISING_TIME = 200;    //SET[13] : ms
      Para.dft.u16MC_RAMP_UP_SPEED = 0;              Para.min.u16MC_RAMP_UP_SPEED = 0;              Para.max.u16MC_RAMP_UP_SPEED = 1;              //SET[14] : speed[RPM]
      Para.dft.u16MC_TORQUE_COMPENSATION = 0;        Para.min.u16MC_TORQUE_COMPENSATION = 0;        Para.max.u16MC_TORQUE_COMPENSATION = 1;        //SET[15] : 
      Para.dft.u16MC_TORQUE_OFFSET = 0;              Para.min.u16MC_TORQUE_OFFSET = 0;              Para.max.u16MC_TORQUE_OFFSET = 20000;          //SET[16] : 
      Para.dft.u16MC_MAX_PULSE_COUNT = 0;            Para.min.u16MC_MAX_PULSE_COUNT = 0;            Para.max.u16MC_MAX_PULSE_COUNT = 20000;        //SET[17] : 
      Para.val.u16MC_SCREW_TYPE = 0;                 Para.min.u16MC_SCREW_TYPE = 0;                 Para.max.u16MC_SCREW_TYPE = 1;
      Para.val.u16MC_SOFT_STOP = 0;                  Para.min.u16MC_SOFT_STOP = 0;                  Para.max.u16MC_SOFT_STOP = 1;

      Para.dft.u16MC_ADVANCED_MODE = 0;              Para.min.u16MC_ADVANCED_MODE = 0;              Para.max.u16MC_ADVANCED_MODE = 10;                   //SET[0] :  
      Para.dft.f32MC_ADVANCED_PARA1 = 0;             Para.min.f32MC_ADVANCED_PARA1 = 0;             Para.max.f32MC_ADVANCED_PARA1 = 0xffff;              //SET[1] :  
      Para.dft.f32MC_ADVANCED_PARA2 = 0;             Para.min.f32MC_ADVANCED_PARA2 = 0;             Para.max.f32MC_ADVANCED_PARA2 = 0xffff;              //SET[2] :  
      Para.dft.f32MC_ADVANCED_PARA3 = 0;             Para.min.f32MC_ADVANCED_PARA3 = 0;             Para.max.f32MC_ADVANCED_PARA3 = 0xffff;              //SET[3] :  
      Para.dft.f32MC_ADVANCED_PARA4 = 0;             Para.min.f32MC_ADVANCED_PARA4 = 0;             Para.max.f32MC_ADVANCED_PARA4 = 0xffff;              //SET[4] :  
      Para.dft.f32MC_ADVANCED_PARA5 = 0;             Para.min.f32MC_ADVANCED_PARA5 = 0;             Para.max.f32MC_ADVANCED_PARA5 = 0xffff;              //SET[5] :  
      Para.dft.f32MC_ADVANCED_PARA6 = 0;             Para.min.f32MC_ADVANCED_PARA6 = 0;             Para.max.f32MC_ADVANCED_PARA6 = 0xffff;              //SET[6] :  
      Para.dft.f32MC_ADVANCED_PARA7 = 0;             Para.min.f32MC_ADVANCED_PARA7 = 0;             Para.max.f32MC_ADVANCED_PARA7 = 0xffff;              //SET[7] :  
      Para.dft.f32MC_ADVANCED_PARA8 = 0;             Para.min.f32MC_ADVANCED_PARA8 = 0;             Para.max.f32MC_ADVANCED_PARA8 = 0xffff;              //SET[8] :  
      Para.dft.f32MC_ADVANCED_PARA9 = 0;             Para.min.f32MC_ADVANCED_PARA9 = 0;             Para.max.f32MC_ADVANCED_PARA9 = 0xffff;              //SET[9] :  
      Para.dft.f32MC_ADVANCED_PARA10 = 0;            Para.min.f32MC_ADVANCED_PARA10 = 0;            Para.max.f32MC_ADVANCED_PARA10 = 0xffff;             //SET[10] : 
      Para.dft.f32MC_ADVANCED_PARA11 = 0;            Para.min.f32MC_ADVANCED_PARA11 = 0;            Para.max.f32MC_ADVANCED_PARA11 = 0xffff;             //SET[11] : 
      Para.dft.f32MC_ADVANCED_PARA12 = 0;            Para.min.f32MC_ADVANCED_PARA12 = 0;            Para.max.f32MC_ADVANCED_PARA12 = 0xffff;             //SET[12] : 
      Para.dft.f32MC_ADVANCED_PARA13 = 0;            Para.min.f32MC_ADVANCED_PARA13 = 0;            Para.max.f32MC_ADVANCED_PARA13 = 0xffff;             //SET[13] : 
      Para.dft.f32MC_ADVANCED_PARA14 = 0;            Para.min.f32MC_ADVANCED_PARA14 = 0;            Para.max.f32MC_ADVANCED_PARA14 = 0xffff;             //SET[14] : 
      Para.dft.f32MC_ADVANCED_PARA15 = 0;            Para.min.f32MC_ADVANCED_PARA15 = 0;            Para.max.f32MC_ADVANCED_PARA15 = 0xffff;             //SET[15] : 
      Para.dft.f32MC_ADVANCED_PARA16 = 0;            Para.min.f32MC_ADVANCED_PARA16 = 0;            Para.max.f32MC_ADVANCED_PARA16 = 0xffff;             //SET[16] : 
      Para.dft.f32MC_ADVANCED_PARA17 = 0;            Para.min.f32MC_ADVANCED_PARA17 = 0;            Para.max.f32MC_ADVANCED_PARA17 = 0xffff;             //SET[17] : 
      Para.dft.f32MC_ADVANCED_PARA18 = 0;            Para.min.f32MC_ADVANCED_PARA18 = 0;            Para.max.f32MC_ADVANCED_PARA18 = 0xffff;             //SET[18] : 
      Para.dft.f32MC_ADVANCED_PARA19 = 0;            Para.min.f32MC_ADVANCED_PARA19 = 0;            Para.max.f32MC_ADVANCED_PARA19 = 0xffff;             //SET[19] : 
      Para.dft.u16MC_FREE_REVERSE_ROTATION_SPEED = 0;Para.min.u16MC_FREE_REVERSE_ROTATION_SPEED = 0;Para.max.u16MC_FREE_REVERSE_ROTATION_SPEED = 1000;   //SET[1] :  
      Para.dft.f32MC_FREE_REVERSE_ROTATION_ANGLE = 0;Para.min.f32MC_FREE_REVERSE_ROTATION_ANGLE = 0;Para.max.f32MC_FREE_REVERSE_ROTATION_ANGLE = 200;    //SET[2] :  
      Para.dft.u16MC_REVERS_ANGLE_SETTING_SPEED = 0; Para.min.u16MC_REVERS_ANGLE_SETTING_SPEED = 0; Para.max.u16MC_REVERS_ANGLE_SETTING_SPEED = 1000;    //SET[3] :  
      Para.dft.u16MC_REVERS_ANGLE_SETTING_ANGLE = 0; Para.min.u16MC_REVERS_ANGLE_SETTING_ANGLE = 0; Para.max.u16MC_REVERS_ANGLE_SETTING_ANGLE = 30000;   //SET[4] :  
      Para.dft.u16MC_REVERS_ANGLE_SETTING_FW_REV = 0;Para.min.u16MC_REVERS_ANGLE_SETTING_FW_REV = 0;Para.max.u16MC_REVERS_ANGLE_SETTING_FW_REV = 1;      //SET[5] :  

      Para.dft.u16MC_DRIVER_MODEL = 2;               Para.min.u16MC_DRIVER_MODEL = 1;               Para.max.u16MC_DRIVER_MODEL = 99;                //SET[0] :  
      Para.dft.u16MC_UNIT = 2;                       Para.min.u16MC_UNIT = 0;                       Para.max.u16MC_UNIT = 6;                         //SET[1] :  
      Para.dft.u16MC_ACC_DEC_TIME = 100;             Para.min.u16MC_ACC_DEC_TIME = 10;              Para.max.u16MC_ACC_DEC_TIME = 1000;              //SET[2] :  
      Para.dft.u16MC_FASTEN_TORQUE_MAINTAIN_TIME = 2;Para.min.u16MC_FASTEN_TORQUE_MAINTAIN_TIME = 1;Para.max.u16MC_FASTEN_TORQUE_MAINTAIN_TIME = 20; //SET[3] :  
      Para.dft.u16MC_USE_MAXTQ_FOR_LOOSENING = 0;    Para.min.u16MC_USE_MAXTQ_FOR_LOOSENING = 0;    Para.max.u16MC_USE_MAXTQ_FOR_LOOSENING = 1;      //SET[4] :  
      Para.dft.u16MC_LOOSENING_SPEED = 500;          Para.min.u16MC_LOOSENING_SPEED = 100;          Para.max.u16MC_LOOSENING_SPEED = 1000;           //SET[5] :  
      Para.dft.f32MC_TOTAL_FASTENING_TIME = 100;     Para.min.f32MC_TOTAL_FASTENING_TIME = 0;       Para.max.f32MC_TOTAL_FASTENING_TIME = 600;       //SET[6] :  
      Para.dft.f32MC_TOTAL_LOOSENING_TIME = 100;     Para.min.f32MC_TOTAL_LOOSENING_TIME = 0;       Para.max.f32MC_TOTAL_LOOSENING_TIME = 600;       //SET[7] :  
      Para.dft.f32MC_STALL_LOOSENING_TIME_LIMIT = 2; Para.min.f32MC_STALL_LOOSENING_TIME_LIMIT = 1; Para.max.f32MC_STALL_LOOSENING_TIME_LIMIT = 5;    //SET[8] :  
      // Para.dft.u16MC_SCREW_TYPE = 0;                    Para.min.u16MC_SCREW_TYPE = 0;                    Para.max.u16MC_SCREW_TYPE = 0x7fff;              //SET[9] :  
      Para.dft.f32MC_JUDGE_FASTEN_MIN_TURNS = 0;     Para.min.f32MC_JUDGE_FASTEN_MIN_TURNS = 0;     Para.max.f32MC_JUDGE_FASTEN_MIN_TURNS = 50;      //SET[10] : 
      Para.dft.u16MC_FASTENING_STOP_ALARM = 0;       Para.min.u16MC_FASTENING_STOP_ALARM = 0;       Para.max.u16MC_FASTENING_STOP_ALARM = 1;         //SET[11] : 
      Para.dft.u16MC_TORQUE_COMPENSATION_MAIN = 100; Para.min.u16MC_TORQUE_COMPENSATION_MAIN = 90;  Para.max.u16MC_TORQUE_COMPENSATION_MAIN = 110;   //SET[12] : 
      Para.dft.u16MC_CROWFOOT_ENABLE = 0;            Para.min.u16MC_CROWFOOT_ENABLE = 0;            Para.max.u16MC_CROWFOOT_ENABLE = 1;              //SET[13] : 
      Para.dft.f32MC_CROWFOOT_RATIO = 1000;          Para.min.f32MC_CROWFOOT_RATIO = 0;             Para.max.f32MC_CROWFOOT_RATIO = 65000;           //SET[14] : 
      Para.dft.u16MC_CROWFOOT_EFFICIENCY = 100;      Para.min.u16MC_CROWFOOT_EFFICIENCY = 0;        Para.max.u16MC_CROWFOOT_EFFICIENCY = 300;        //SET[15] : 
      Para.dft.f32MC_CROWFOOT_REVERSE_TORQUE = 0;    Para.min.f32MC_CROWFOOT_REVERSE_TORQUE = 0;    Para.max.f32MC_CROWFOOT_REVERSE_TORQUE = 500;    //SET[16] : 
      Para.dft.u16MC_CROWFOOT_REVERSE_SPEED = 50;    Para.min.u16MC_CROWFOOT_REVERSE_SPEED = 0;     Para.max.u16MC_CROWFOOT_REVERSE_SPEED = 100;     //SET[17] : 

      Para.val.u16MC_ZERO_DUMMY = 0;     //0
      Para.val.u16MC_TCAM_ACTM = 0;      //1
      Para.val.f32MC_FASTEN_TORQUE = 20; //2
      Para.val.f32MC_TORQUE_MIN_MAX = 0; //3
      Para.val.u16MC_TARGET_ANGLE = 0;   //4
      Para.val.u16MC_FASTEN_MIN_ANGLE = 0; //5
      Para.val.u16MC_FASTEN_MAX_ANGLE = 0; //6
      Para.val.f32MC_SNUG_TORQUE = 0;      //7
      Para.val.u16MC_FASTEN_SPEED = 100;     //8
      Para.val.u16MC_FREE_FASTEN_ANGLE = 0;  //9
      Para.val.u16MC_FREE_FASTEN_SPEED = 0;  //10
      Para.val.u16MC_SOFT_START = 100;         //11
      Para.val.u16MC_FASTEN_SEATTING_POINT_RATE = 50; //12
      Para.val.u16MC_FASTEN_TQ_RISING_TIME = 50;      //13
      Para.val.u16MC_RAMP_UP_SPEED = 500;              //14
      Para.val.u16MC_TORQUE_COMPENSATION = 100;        //15
      Para.val.u16MC_TORQUE_OFFSET = 10;              //16
      Para.val.u16MC_MAX_PULSE_COUNT = 100;            //17
      Para.val.u16MC_SCREW_TYPE = 0;                   //18
      Para.val.u16MC_SOFT_STOP = 0;                   //19

      Para.val.u16MC_ADVANCED_MODE = 0;              //0
      Para.val.f32MC_ADVANCED_PARA1 = 0;             //1
      Para.val.f32MC_ADVANCED_PARA2 = 0;             //2
      Para.val.f32MC_ADVANCED_PARA3 = 0;             //3
      Para.val.f32MC_ADVANCED_PARA4 = 0;             //4
      Para.val.f32MC_ADVANCED_PARA5 = 0;             //5
      Para.val.f32MC_ADVANCED_PARA6 = 0;             //6
      Para.val.f32MC_ADVANCED_PARA7 = 0;             //7
      Para.val.f32MC_ADVANCED_PARA8 = 0;             //8
      Para.val.f32MC_ADVANCED_PARA9 = 0;             //9
      Para.val.f32MC_ADVANCED_PARA10 = 0;             //10
      Para.val.f32MC_ADVANCED_PARA11 = 0;             //11
      Para.val.f32MC_ADVANCED_PARA12 = 0;             //12
      Para.val.f32MC_ADVANCED_PARA13 = 0;             //13
      Para.val.f32MC_ADVANCED_PARA14 = 0;             //14
      Para.val.f32MC_ADVANCED_PARA15 = 0;             //15
      Para.val.f32MC_ADVANCED_PARA16 = 0;             //16
      Para.val.f32MC_ADVANCED_PARA17 = 0;             //17
      Para.val.f32MC_ADVANCED_PARA18 = 0;             //18
      Para.val.f32MC_ADVANCED_PARA19 = 0;             //19
      Para.val.u16MC_FREE_REVERSE_ROTATION_SPEED = 0;             //1
      Para.val.f32MC_FREE_REVERSE_ROTATION_ANGLE = 0;             //2
      Para.val.u16MC_REVERS_ANGLE_SETTING_SPEED = 0;             //3
      Para.val.u16MC_REVERS_ANGLE_SETTING_ANGLE = 0;             //4
      Para.val.u16MC_REVERS_ANGLE_SETTING_FW_REV = 0;             //5

      Para.val.u16MC_DRIVER_MODEL = u16MC_DRIVER_MODEL_;//4;//1;                 //0
      Para.val.u16MC_UNIT = 2;                         //1
      Para.val.u16MC_ACC_DEC_TIME = 200;                 //2
      Para.val.u16MC_FASTEN_TORQUE_MAINTAIN_TIME = 0;  //3
      Para.val.u16MC_USE_MAXTQ_FOR_LOOSENING = 0;      //4
      Para.val.u16MC_LOOSENING_SPEED = 100;              //5
      Para.val.f32MC_TOTAL_FASTENING_TIME = 10;         //6
      Para.val.f32MC_TOTAL_LOOSENING_TIME = 10;         //7
      Para.val.f32MC_STALL_LOOSENING_TIME_LIMIT = 0.2f;    //8
      Para.val.f32MC_JUDGE_FASTEN_MIN_TURNS = 0;       //9
      Para.val.u16MC_FASTENING_STOP_ALARM = 0;         //10
      Para.val.u16MC_TORQUE_COMPENSATION_MAIN = 100;     //11
      Para.val.u16MC_CROWFOOT_ENABLE = 0;              //12
      Para.val.f32MC_CROWFOOT_RATIO = 10;               //13
      Para.val.u16MC_CROWFOOT_EFFICIENCY = 100;          //14
      Para.val.f32MC_CROWFOOT_REVERSE_TORQUE = 50;      //15
      Para.val.u16MC_CROWFOOT_REVERSE_SPEED = 0;       //16
    }
  }
}
