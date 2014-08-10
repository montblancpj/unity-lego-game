using UnityEngine;
using System.Collections;

public class LEDController {
//	private const string LEDHeader = "l";  // Arduinoの制御用ヘッダ
	static private LEDController s_instance = null;
	
	private SerialHandler m_handler = null;
	
	private LEDController()
	{
		m_handler = SerialHandler.GetSerialHandler(ArduinoType.DUEMILANOVE);
	}
	
	static public LEDController Instance 
	{
		get 
		{
			if(s_instance == null)
			{
				s_instance = new LEDController();
			}
			return s_instance;
		}
	}
	
	/// <summary>
	/// Sets the led.
	/// </summary>
	/// <param name='ledNum'>
	/// Led number, 0 - 15.
	/// </param>
	/// <param name='brightness'>
	/// Brightness, 0 - 255.
	/// </param>
	public void SetLed(int ledNum, int brightness)
	{
		if(m_handler != null) {
			var data = ledNum + "," + brightness.ToString() + "\0";
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
