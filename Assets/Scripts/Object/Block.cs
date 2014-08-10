using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour {

	private Vector3 originalPosition_;
	private bool isBounce_ = false;
	private int cnt_ = 0;
	
	public float BounceLength = 0.3f;
	public int Cycle = 30;
	
	void Awake()
	{
		originalPosition_ =	transform.localPosition;
	}
	

	void Hit() 
	{
		if (!isBounce_) {
			GameObject.FindWithTag("SoundController").GetComponent<SoundController>().Play("Block");
			cnt_ = 0;
			isBounce_ = true;
		}
	}
	
	
	void Update()
	{
		if (isBounce_) {
			float x = originalPosition_.x;
			float y = originalPosition_.y + BounceLength * Mathf.Sin(Mathf.PI * cnt_ / Cycle);
			float z = originalPosition_.z;
			transform.localPosition = new Vector3(x, y, z);
			
			++cnt_;
			if (cnt_ > Cycle) {
				isBounce_ = false;
			}
		}
	}
	
}
