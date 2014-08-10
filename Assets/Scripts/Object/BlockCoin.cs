using UnityEngine;
using System.Collections;

public class BlockCoin : MonoBehaviour {

	private Vector3 startPosition;
	private int cnt_ = 0;
	public int Cycle = 20;
	public float RotationPerFrame = 36;
	public float Bounce = 2.0f;
	public float Distance = 2.0f;

	void Start () {
		startPosition = transform.localPosition;
	}
	
	void Update () {
		float x = startPosition.x + Distance * ((float)cnt_ / Cycle);
		float y = startPosition.y + Bounce * Mathf.Sin(Mathf.PI * cnt_ / Cycle);
		float z = startPosition.z;
		transform.localPosition = new Vector3(x, y, z);
		
		transform.Rotate(Vector3.up, RotationPerFrame);
		
		++cnt_;
		if (cnt_ > Cycle * 1.3f) {
			Destroy(gameObject);
		}
	}
	
}
