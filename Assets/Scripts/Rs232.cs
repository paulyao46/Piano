using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

public enum parity_mode{
	none,
	even,
	odd,
};
public enum data_bits{
	db5,
	db6,
	db7,
	db8,
	db9,
};
public enum stop_bits{
	sb1,
	sb2,
};
public enum DeviceNumber{
	ttyS0,
	COM1 = ttyS0,
	MH_CON4_Lower = ttyS0,
	ttyS1,
	COM2 = ttyS1,
	MH_CON4_Upper = ttyS1,
	ttyS2,
	COM3 = ttyS2,
	ttyS3,
	COM4 = ttyS3,
	ttyS4,
	COM5 = ttyS4,
	ttyS5,
	COM6 = ttyS5,
	ttyS6,
	COM7 = ttyS6,
	ttyS7,
	COM8 = ttyS7,
	ttyS8,
	COM9 = ttyS8,
	ttyS9,
	COM10 = ttyS9,
	ttyS10,
	COM11 = ttyS10,
	ttyS11,
	COM12 = ttyS11,
	ttyS12,
	COM13 = ttyS12,
	ttyS13,
	COM14 = ttyS13,
	ttyS14,
	COM15 = ttyS14,
	ttyS15,
	COM16 = ttyS15,
	ttyUSB0,
	ttyUSB1,
	ttyUSB2,
	ttyUSB3,
	ttyUSB4,
	ttyUSB5,
	ttyUSB6,
	ttyUSB7,
	ttyUSB8,
	ttyUSB9,
	ttyUSB10,
	ttyUSB11,
	ttyUSB12,
	ttyUSB13,
	ttyUSB14,
	ttyUSB15,
	ttyUSB16,
	ttyUSB17,
	ttyUSB18,
	ttyUSB19,
	ttyUSB20,
	ttyAMA0,
	ttyAMA1,
	ttyACM0,
	ttyACM1,
	ttyGS0,
	rfcomm0,
	rfcomm1,
	ircomm0,
	ircomm1,
	ttyXRUSB0,
	MI_COM1_Lower = ttyXRUSB0,
	ttyXRUSB1,
	MI_COM1_Upper = ttyXRUSB1,
	ttyXRUSB2,
	MI_COM3_Lower = ttyXRUSB2,
	ttyXRUSB3,
	MI_COM3_Upper = ttyXRUSB3,
	MCU_Uart0,
	MCU_Uart1,
	Count,
};
public enum BaudRate{
	Rate300 = 300,
	Rate1200 = 1200,
	Rate2400 = 2400,
	Rate4800 = 4800,
	Rate9600 = 9600,
	Rate19200 = 19200,
	Rate38400 = 38400,
	Rate57600 = 57600,
	Rate115200 = 115200,
};

public class Rs232 : MonoBehaviour {
#region "Dll 定義"

	[DllImport("RS232")]
	public static extern int RS232_OpenComport(int comport_number, int baudrate);
	[DllImport("RS232")]
//	public static extern int RS232_PollComport(int comport_number, unsigned char *buf, int size);
	public static extern int RS232_PollComport(int comport_number, byte[] buf, int size);
	[DllImport("RS232")]
//	public static extern int RS232_SendByte(int comport_number, unsigned char byte);
	public static extern int RS232_SendByte(int comport_number, byte send_byte);
	[DllImport("RS232")]
//	public static extern int RS232_SendBuf(int comport_number, unsigned char *buf, int size);
	public static extern int RS232_SendBuf(int comport_number, byte[] buf, int size);
	[DllImport("RS232")]
	public static extern void RS232_CloseComport(int comport_number);
	[DllImport("RS232")]
//	public static extern void RS232_cputs(int comport_number, const char *text);
	public static extern void RS232_cputs(int comport_number, string text);
	[DllImport("RS232")]
	public static extern int RS232_IsDCDEnabled(int comport_number);
	[DllImport("RS232")]
	public static extern int RS232_IsCTSEnabled(int comport_number);
	[DllImport("RS232")]
	public static extern int RS232_IsDSREnabled(int comport_number);
	[DllImport("RS232")]
	public static extern void RS232_enableDTR(int comport_number);
	[DllImport("RS232")]
	public static extern void RS232_disableDTR(int comport_number);
	[DllImport("RS232")]
	public static extern void RS232_enableRTS(int comport_number);
	[DllImport("RS232")]
	public static extern void RS232_disableRTS(int comport_number);
	[DllImport("RS232")]
	public static extern void RS232_SetAttributes(parity_mode pm, data_bits db, stop_bits sb);
	[DllImport("RS232")]
	public static extern void RS232_FlushBuffer(int com);
	[DllImport("RS232")]
	//	public static extern int RS232_GetPortNum(unsigned char *path);
	public static extern int RS232_GetPortNum(string path);
	[DllImport("RS232")]
	public static extern void RS232_Runtime_SetAttributes(int dn_port, int br, parity_mode pm, data_bits db, stop_bits sb);
#endregion

#region "讀寫執行序"
//	#if UNITY_EDITOR
	public string[] runningMarks = new string[]{"|","||","|||","||||","|||||","||||","|||","||","|"};
	public int runningMarkIndex = 0;
//	#endif
	public int sleepTime = 16;

	bool running = false;
	Thread comportThread;
	public bool isRunning{
		get{
			return running;
		}
	}

	public string errorlogStr = "";
	public string sendlogStr = "";
	public string receivelogStr = "";

	void RunThread(){
		receiveBuffer = new byte[receiveBufferSize];
		while (running){
			#if UNITY_EDITOR
			runningMarkIndex = (runningMarkIndex + 1) % 9;
			#endif

			try {
				SendBuffer();
				ReceiveBuffer();
				Thread.Sleep(sleepTime);
			} catch (System.Exception ex) {
				errorlogStr += "[Error] : " + ex.ToString() + "\n";
				Debug.LogError (errorlogStr);
			}
		}
	}
#endregion

	public int receiveBufferSize = 25;
	DeviceNumber deviceNumber;
	Queue<byte[]>receiveBufferQueue = new Queue<byte[]>();
	Queue<byte[]>sendBufferQueue = new Queue<byte[]>();
	Queue<string>sendCmdNameQueue = new Queue<string>();
	byte[] receiveBuffer = null;
	byte[] sendBuffer = null;
	string sendCmdName = "";

	public int sendCount = 0;
	public int receiveCount = 0;

	void SendBuffer(){
		if (sendBufferQueue.Count <= 0) {	return;	}
		sendBuffer = sendBufferQueue.Dequeue();
		sendCmdName = sendCmdNameQueue.Dequeue();

		int len = sendBuffer.Length;

		byte _checkSum = 0x00;
		for (int f=0; f<(len-1); f++){
			_checkSum += sendBuffer[f];
		}
		sendBuffer[len - 1] = _checkSum;

		sendlogStr = "SendBuf [" + sendCmdName + "], " + len + " Byte : ";

		int _errorCode = RS232_SendBuf((int)deviceNumber, sendBuffer, len);

		if (_errorCode < 0) {
			throw new Exception ("SendBuf Failed, Error Code : " + _errorCode);
		}
		sendCount++;
		sendBuffer = null;
	}
	void ReceiveBuffer(){
		int _getLength = RS232_PollComport ((int)deviceNumber, receiveBuffer, receiveBufferSize);
		ReceiveBuffer (receiveBuffer, _getLength);
	}
	void ReceiveBuffer(byte[] p_receiveBuffer, int p_length){
		if (p_length > 0) {
			receivelogStr = "ReceiveBuf, " + p_length + " Byte : ";
			receiveBufferQueue.Enqueue ((byte[])p_receiveBuffer.Clone());
			receiveCount++;
		} else {
			receivelogStr = "ReceiveBuf, " + p_length + " Byte";
		}
	}
	
	public void OpenComport(DeviceNumber p_deviceNumber, BaudRate p_baudrate, parity_mode p_parity, data_bits p_dataBits, stop_bits p_stopBits){
		try {
			deviceNumber = p_deviceNumber;
			receiveBuffer = new byte[receiveBufferSize];

			RS232_SetAttributes(p_parity, p_dataBits, p_stopBits);
			int _errorCode = RS232_OpenComport((int)p_deviceNumber, (int)p_baudrate);
			if (_errorCode == 0) {
				comportThread = new Thread (RunThread);
				comportThread.IsBackground = true;
				running = true;
				comportThread.Start ();
			} else {
				throw new Exception ("OpenComport Failed, Error Code : " + _errorCode);
			}
		} catch (System.Exception ex) {
			errorlogStr = "[Error] : " + ex.ToString() + "\n";
			Debug.LogError (errorlogStr);
		}

	}
	public void CloseComport(){
		RS232_CloseComport ((int)deviceNumber);
		running = false;
	}

	public void Send(byte[] p_buffer, string p_cmdName = ""){
		sendBufferQueue.Enqueue ((byte[])p_buffer.Clone());
		sendCmdNameQueue.Enqueue (p_cmdName);
	}

	public delegate void ReceiveEventHandler(byte[] buffer);
	public event ReceiveEventHandler ReceiveEvent;

	public void ReceiveTest(byte[] p_receiveBuffer){
		ReceiveBuffer(p_receiveBuffer, p_receiveBuffer.Length);
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		while (receiveBufferQueue.Count > 0) {
			byte[] _receiveBuffer = receiveBufferQueue.Dequeue ();
			if (ReceiveEvent != null) {
				ReceiveEvent.Invoke (_receiveBuffer);
			}
		}
	}

	void OnApplicationQuit() {
		if (running) {
			Application.CancelQuit();
			StartCoroutine (IeStopRs232AndQuit());
		}
	}
	IEnumerator IeStopRs232AndQuit(){
		CloseComport ();
		yield return new WaitForSeconds (0.5f);
		#if UNITY_EDITOR
		EditorApplication.isPlaying = false;
		#else
		Application.Quit ();
		#endif
	}
}
