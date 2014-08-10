using UnityEngine;
using System.Collections;

public class Blink : MonoBehaviour {

	public int BlinkCycle = 120;
	private int cnt_ = 0;

	void Start () {
	}
	
	void Update () {
		float darkness = 0.20f + 0.20f * Mathf.Sin(2.0f * Mathf.PI * cnt_ / BlinkCycle);
		Color color = new Color(darkness, darkness, darkness, 1.0f);
		transform.renderer.material.SetColor("_SpecColor", color);
		
		++cnt_;
	}
}
