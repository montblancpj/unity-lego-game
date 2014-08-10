using UnityEngine;
using System.Collections.Generic;

public class SerialTestClient : MonoBehaviour {
	private DCMotorController m_dcController;
	private ServoController   m_servoController;
	private LEDController	  m_ledController;
	
	public int m_servoValue = 0;
	public int m_motorValue = 0;
	public List<int> m_ledValueList = new List<int>();
	
	// Use this for initialization
	void Start () {
		m_dcController = DCMotorController.Instance;
		m_servoController = ServoController.Instance;
		m_ledController = LEDController.Instance;
		
		for(int i = 0; i < 16; i++) {
			m_ledValueList.Add(0);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(m_dcController != null) 
		{
			if(Input.GetKeyDown(KeyCode.D)) 
			{
				int val = (int)Input.mousePosition.x;
				Debug.Log("DC motor speed: " + val.ToString());
				m_dcController.SetSpeed(val);	
			}
		}
			
		if(m_servoController != null) 
		{
			if(Input.GetKeyDown(KeyCode.S))
			{
				int px = (int)Input.mousePosition.x;
				Debug.Log("Servo motor " + px.ToString());
				m_servoController.SetYPixel(px);
			}
		}
	}
				
    void OnGUI() {
		// Servo Update
		var val = 0;
        val = (int)GUI.HorizontalSlider(new Rect(10, 10, 200, 40), m_servoValue, 0, 180);
		if(val != m_servoValue) {
			m_servoValue = val;
			if(m_servoController != null) {
				m_servoController.SetDegree(m_servoValue);	
			}
		}
        GUI.Label(new Rect(220, 10, 100, 40), "Servo: " + m_servoValue.ToString());
		
		// DCMotor Update
        val = (int)GUI.HorizontalSlider(new Rect(10, 50, 200, 40), m_motorValue, -255, 255);
		if(val != m_motorValue) {
			m_motorValue = val;
			if(m_dcController != null) {
				m_dcController.SetSpeed(m_motorValue);	
			}
		}
        GUI.Label(new Rect(220, 50, 100, 40), "DCMotor: " + m_motorValue.ToString());
		
		for(int i = 0; i < m_ledValueList.Count; i++) {
			var rect = new Rect(10 + i * 35, 90, 40, 150);
			val = (int)GUI.VerticalSlider(rect, m_ledValueList[i], 0, 4095);
			if(val != m_ledValueList[i]) {
				m_ledValueList[i] = val;
				if(m_ledController != null) {
					m_ledController.SetLed(i, m_ledValueList[i]);	
				}
			}
        	GUI.Label(rect, "LED" + i.ToString() + ": " + m_ledValueList[i].ToString());
		}
    }
	
	void OnApplicationQuit() 
    {
		if(m_dcController != null) {
			m_dcController.Quit();
		}
		if(m_servoController != null) {
			m_servoController.Quit();
		}
		if(m_ledController != null) {
			m_ledController.Quit();
		}
    }
}
