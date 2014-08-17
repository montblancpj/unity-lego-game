using UnityEngine;
using System.Collections;

public class Bounce : MonoBehaviour 
{
	private Vector3 originalScale_;
	private Vector3 originalPosition_;
	public float bounce = 0.2f;
	public int period = 60;
	private int cnt_ = 0;

	void Start() 
	{
		originalScale_    = transform.localScale;
		originalPosition_ = transform.position;
	}
	
	void Update() 
	{
		var bounceScale = Vector3.up * bounce * ( 0.5f + Mathf.Sin(2 * Mathf.PI * cnt_ / period) ) / 2f;
		transform.localScale = originalScale_    - bounceScale; 
		transform.position   = originalPosition_ - bounceScale * 0.5f;
		++cnt_;
	}
}
