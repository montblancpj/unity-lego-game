using UnityEngine;
using System.Collections;

public class LightBlink : MonoBehaviour {

	public float max = 1.0f;
	public int frequency = 30;
	private int cnt_ = 0;
	private Light light_;

	void Start()
	{
		light_ = GetComponent<Light>();
	}

	void Update() 
	{
		light_.intensity = 0.5f + 0.5f * Mathf.Sin(2.0f * Mathf.PI * cnt_ / frequency); 
		light_.intensity *= max;
		cnt_++;
	}
}
