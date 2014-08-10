using UnityEngine;
using System.Collections;

public class LedBlock : MonoBehaviour {
	private LEDController led_ = null;
	private bool isActive_ = true;

	void Awake() {
		led_ = LEDController.Instance;
	}

	void Hit() 
	{
		if (isActive_) {
			led_.SetLed(true);
			isActive_ = false;
			StartCoroutine(SetLedOffAfter(0.2f));
		}
	}
	
	private IEnumerator SetLedOffAfter(float time) 
	{
		yield return new WaitForSeconds(time);
		led_.SetLed(false);
		isActive_ = true;
	}
}
