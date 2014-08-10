using UnityEngine;
using System.Collections;

public class LedBlock : MonoBehaviour {
	private LEDController led_ = LEDController.Instance;
	private bool isActive_ = true;
	
	void Hit() 
	{
		if (isActive_) {
			led_.SetLed(15, 4095);
			isActive_ = false;
			StartCoroutine(SetLedOffAfter(0.2f));
		}
	}
	
	private IEnumerator SetLedOffAfter(float time) 
	{
		yield return new WaitForSeconds(time);
		led_.SetLed(15, 0);
		isActive_ = true;
	}
	
	void OnApplicationQuit()
	{
		led_.Quit();
	}
}
