using UnityEngine;
using System.Collections;

public class LightBlinkController : MonoBehaviour 
{
	private Light light_;
	public KeyCode onOffKey = KeyCode.L;

	void Awake()
	{
		light_ = gameObject.GetComponent<Light>();
	}

	void Update() 
	{
		if ( Input.GetKeyDown(onOffKey) ) {
			light_.enabled = !light_.enabled;
		}
	}
}
