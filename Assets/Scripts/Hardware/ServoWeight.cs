using UnityEngine;
using System.Collections;

public class ServoWeight : MonoBehaviour {
	private ServoController servo_ = null;
	private Vector3 originalPosition_;
	private Vector3 groundPosition_;
	private float preY_;
	private float moveLengthY = 0f;

	// Special Parameters for OOMF2014
	private const float EnemyGroundY = -7.5f;
	private const float EnemyFallingForecast = 0.03f;
	private const float EnemyRisingForecast = 0.09f;

	void Awake() {
		servo_ = ServoController.Instance;
	}

	void Start()
	{
		originalPosition_ = transform.localPosition;
		groundPosition_ = new Vector3(originalPosition_.x, EnemyGroundY);
		moveLengthY = originalPosition_.y - groundPosition_.y;
	}

	void Update()
	{

		var state = gameObject.GetComponent<WeightEnemy>().CurrentState;
		var y = (originalPosition_.y -  transform.localPosition.y) / moveLengthY;

		// Forecast an enemy's y-position because of servo delay.
		if(state == WeightEnemy.State.FALLING) {
			y += (transform.localPosition.y - groundPosition_.y) * EnemyFallingForecast;
		} else if(state == WeightEnemy.State.RISING) {
			y -= EnemyRisingForecast;
		}
		servo_.SetY(y);
	}
}
