using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System.Collections.Generic;

public enum ArduinoType {
	DUEMILANOVE,
	UNO,
}

public class SerialHandler {
	const string DuemilanovePort = "/dev/tty.usbserial-A800ey7d";
	const string UnoPort 		 = "/dev/tty.usbmodem1d1141";
	const int BaudRate 			 = 9600;
	
//	static private SerialHandler 	s_instance;
	static private Dictionary<ArduinoType, SerialHandler> m_handlerDict = new Dictionary<ArduinoType, SerialHandler>();
//	static private Dictionary<ArduinoType, SerialPort>    m_serialDict;
	
	private SerialPort 		m_serial;
	
	private SerialHandler(ArduinoType type)
	{
		switch(type)
		{
		case ArduinoType.DUEMILANOVE:
			m_serial = new SerialPort(DuemilanovePort, BaudRate);
//			m_serialDict.Add(ArduinoType.DUEMILANOVE, m_serial);
			break;
		case ArduinoType.UNO:
			m_serial = new SerialPort(UnoPort, BaudRate);
//			m_serialDict.Add(ArduinoType.UNO, m_serial);
			break;
		}
//		Debug.Log("Open Serial port: " + PortName);
//		m_serial = new SerialPort(PortName, BaudRate);
		Start();
	}
	
	~SerialHandler()
	{
		Debug.Log("Serial Handler Destructor");
//		Debug.Log("Close " + PortName);
//		Stop ();
	}
	
	static public SerialHandler GetSerialHandler(ArduinoType type)
	{
		if(type != ArduinoType.DUEMILANOVE && type != ArduinoType.UNO) return null;
		
		SerialHandler handler = null;
		if(!m_handlerDict.ContainsKey(type)) {
			handler = new SerialHandler(type);
			m_handlerDict.Add(type, handler);
		} else {
			handler = m_handlerDict[type];	
		}
	
		return handler;
	}
	
//	static public SerialHandler[int a]
//	{
//		get { 
//			if (s_instance == null)
//			{
//				s_instance = new SerialHandler();
//			}
//			return s_instance;
//		}
//	}
		
	private void Start() {
		if(m_serial != null) {
			if(m_serial.IsOpen) {
				Debug.LogError("Failed to open Serial Port, already open!");
				m_serial.Close();
			} else {
				try
				{
					m_serial.Open();
					m_serial.ReadTimeout = 50;
					Debug.Log("Open Serial port");
				}
				catch(System.IO.IOException)
				{
					IOErrorHandler();
				}
			}
		}
	}
	
	public void Update()
	{
		string value = m_serial.ReadLine(); //Read the information
		Debug.Log(value);
	}
	
	public void Stop() 
    {
		Debug.Log("STOP!!!");
		if(m_serial != null) {
			Debug.Log("CLose!!!");
			m_serial.Close();
		}
    }
		
	public string CreateSendData<T>(string header, T data)
	{
		return header + data.ToString() + "\0";
	}
	
	public void SendData(string data)
	{
		try
		{
			m_serial.Write(data);
		}
		catch(System.IO.IOException)
		{
			IOErrorHandler();
		}
	}
	
	private void IOErrorHandler()
	{
		Debug.LogError("IOException!!!!");
		Stop();
	}
}
