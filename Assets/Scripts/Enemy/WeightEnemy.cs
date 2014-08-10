using UnityEngine;
using System;
using System.Collections;

public class WeightEnemy : Enemy 
{
	private Vector3 originalPosition_;
	
	public float ReactionDistance = 2.5f;
	public float RisingSpeed = 10.0f;
	public float WaitTime = 1.0f;
	public GameObject effect;
	
	enum State {
		WAITING,
		FALLING,
		RISING
	};
	private State state_ = State.WAITING;


	void Start() 
	{
		originalPosition_ = transform.localPosition;
	}
	
	
	void Update() 
	{
		++cnt_;
		switch (state_) {
			case State.WAITING:
				foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
					Vector3 diff = player.transform.position - transform.position;
					if (diff.y < 0.0f && Mathf.Abs(diff.x) < ReactionDistance) {
						rigidbody.isKinematic = false;
						rigidbody.useGravity  = true;
						state_ = State.FALLING;
						return;
					}
				}
				break;
			case State.FALLING:
				break;
			case State.RISING:
				transform.localPosition += transform.up * RisingSpeed * Time.deltaTime;
				if (transform.localPosition.y >= originalPosition_.y) {
					state_ = State.WAITING;
				}
				break;
		}
	}
	
	
	protected override void Attacked()
	{
		// does not become dead
	}
	
	
	void OnCollisionEnter(Collision collision)
	{
		if (collision.transform.tag == "Player") {
			collision.transform.SendMessage("Dead");
		} else if (state_ == State.FALLING && collision.contacts[0].normal.y > 0) {
			if (effect) {
				var obj = Instantiate(effect, transform.position + Vector3.down, Quaternion.identity);
				StartCoroutine(WaitThenCallback(2.0f, () => {
					Destroy(obj);
				}));
			}

			StartCoroutine(WaitThenCallback(WaitTime, () => {
				rigidbody.isKinematic = true;
				rigidbody.useGravity  = false;
				state_ = State.RISING;
			}));
		}
	}
	
	
	IEnumerator WaitThenCallback(float time, Action callback)
	{
		yield return new WaitForSeconds(time);
		callback();
	}
}
