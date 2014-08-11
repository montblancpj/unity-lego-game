using UnityEngine;
using System.Collections;

public class ServoDossun : MonoBehaviour {
	private ServoController servo_ = null;
	private Vector3 originalPosition_;
	private float preY_;

	void Awake() {
		servo_ = ServoController.Instance;
	}

	void Start()
	{
		originalPosition_ = transform.localPosition;
	}

	void Update()
	{
		float moveLength = 4.9f;
		float y = transform.localPosition.y;
		if (y - preY_ > 0.0f) {
			moveLength = 5.0f;
		}
		float len = Mathf.Sqrt(moveLength - (originalPosition_.y - y)) / moveLength;
		float dy = len * 8.9f * 200;
		if (dy > 0) {
			Debug.Log(dy);
			// servo_.SetYPixel(dy);
			preY_ = y;
		}
	}
}
