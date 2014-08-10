using UnityEngine;
using System.Collections;

public class ServoController {
	private const string ServoHeader = "s";  // Arduinoの制御用ヘッダ
	private const float DegPerPixel  = 0.1f; // 1pxあたりのServoの回転角度　0度＝0px
	
	static private ServoController s_instance = null;
	
	private SerialHandler m_handler = null;
	
	private ServoController()
	{
		m_handler = SerialHandler.GetSerialHandler(ArduinoType.UNO);
	}
	
	static public ServoController Instance 
	{
		get 
		{
			if(s_instance == null)
			{
				s_instance = new ServoController();
			}
			return s_instance;
		}
	}
	
	public void SetDegree(int degree)
	{
		if(m_handler != null) {
			if(degree <= 180 && degree >= 0) {
				var data = m_handler.CreateSendData<int>(ServoHeader, degree);
				m_handler.SendData(data);
			}
		}
	}
	
	public void SetYPixel(float pixel)
	{
		if(m_handler != null) {
			var degree = (int)(pixel * DegPerPixel);
			// Debug.Log("pixel: " + pixel.ToString() + ", deg: " + deg.ToString());
			if(degree <= 180 && degree >= 0) {
				var data = m_handler.CreateSendData<int>(ServoHeader, degree);
				m_handler.SendData(data);
			}
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
