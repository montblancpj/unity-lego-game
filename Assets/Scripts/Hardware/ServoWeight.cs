using UnityEngine;
using System.Collections;

public class ServoWeight : MonoBehaviour {
	private ServoController servo_ = null;
	private Vector3 originalPosition_;
	private float preY_;
	private float moveLengthY = 0f;

	void Awake() {
        // WeightEnemyの移動可能距離
		moveLengthY = transform.localPosition.y + 7.51997f;
		servo_ = ServoController.Instance;
	}

	void Start()
	{
		originalPosition_ = transform.localPosition;
	}

	void Update()
	{
		servo_.SetY((originalPosition_.y -  transform.localPosition.y) / moveLengthY);
	}
}
