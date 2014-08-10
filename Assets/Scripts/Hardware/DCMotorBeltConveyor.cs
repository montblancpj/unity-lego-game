using UnityEngine;
using System.Collections;

public class DCMotorBeltConveyor : MonoBehaviour {
	private DCMotorController dc_;
	private int cnt_ = 0;
	
	void Start () 
	{
		dc_ = DCMotorController.Instance;
	}
	
	void Update()
	{
		// not work at starting
		if (cnt_++ == 60) {
			dc_.SetSpeed(-110);
		}
	}
	
	void OnApplicationQuit()
	{
		dc_.SetSpeed(0);
		dc_.Quit();
	}
}
