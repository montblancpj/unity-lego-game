using UnityEngine;
using System.Collections;

public class CoinBlock : MonoBehaviour {

	private Vector3 originalPosition_;
	private bool isHit_ = false;
	private bool isBounce_ = false;
	private int cnt_ = 0;
	
	public float BounceLength = 0.3f;
	public int Cycle = 30;
	public GameObject BlockCoin;
	public Material After;
	
	void Awake()
	{
		originalPosition_ =	transform.localPosition;
	}
	

	void Hit() 
	{
		if (!isBounce_) {
		//if (!isHit_) {
			GameObject.FindWithTag("SoundController").GetComponent<SoundController>().Play("Coin");
			Instantiate(BlockCoin, transform.position, transform.rotation);
			isBounce_ = true;
		}
		//isHit_ = true;
	}
	
	
	void Update()
	{
		if (isBounce_) {
			float x = originalPosition_.x;
			float y = originalPosition_.y + BounceLength * Mathf.Sin(Mathf.PI * cnt_ / Cycle);
			float z = originalPosition_.z;
			transform.localPosition = new Vector3(x, y, z);
			
			++cnt_;
			if (cnt_ == Cycle/2) {
				//transform.renderer.material = After;
			}
			if (cnt_ > Cycle) {
				isBounce_ = false;
				cnt_ = 0;
			}
		}
	}
	
}
