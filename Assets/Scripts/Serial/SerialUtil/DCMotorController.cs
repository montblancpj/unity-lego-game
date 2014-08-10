using UnityEngine;
using System.Collections;

public class DCMotorController {
	private const string DCMotorHeader = "d";  // Arduinoの制御用ヘッダ
	
	static private DCMotorController s_instance = null;
	
	private SerialHandler m_handler = null;
	
	private DCMotorController()
	{
		m_handler = SerialHandler.GetSerialHandler(ArduinoType.UNO);
	}
	
	static public DCMotorController Instance
	{
		get 
		{
			if(s_instance == null)
			{
				s_instance = new DCMotorController();	
			}
			return s_instance;
		}
	}
	
	/// <summary>
	/// Sets the DC motor speed.
	/// </summary>
	/// <param name='speed'>
	/// Speed. Reverse Rot: -255 to -11, Stop: -10 to 10, Forward Rot: 11 to 255
	/// </param>
	public void SetSpeed(int speed)
	{
		if(m_handler != null) {
			var data =  m_handler.CreateSendData<float>(DCMotorHeader, speed);
			m_handler.SendData(data);
		}
	}
	
	public void Quit()
	{
		if(m_handler != null) {
			m_handler.Stop();	
			m_handler = null;
		}
	}
}
