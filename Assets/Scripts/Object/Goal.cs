using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour 
{
	// flag object
	private GameObject flag_;

	// animation state
	enum State {
		NORMAL,
		PAUSE,
		ANIMATING
	};
	private State state_;
	
	// falling speed
	public float Speed = 4.0f;
	
	// original object parameters
	private Vector3 originalPosition;
	private const float BASE_HEIGHT = 1.0f;
	
	void Start()
	{
		flag_ = transform.FindChild("Flag").gameObject;
		originalPosition = flag_.transform.localPosition;
	}
	

	void Update()
	{
		switch (state_) {
			case State.NORMAL:
				break;
			case State.PAUSE:
				break;
			case State.ANIMATING:
				Animate();
				break;
		}
	}
	
	
	void Animate()
	{
		if (flag_.transform.localPosition.y <= BASE_HEIGHT) {
			StopAnimation();
		} else {
			flag_.transform.localPosition -= Vector3.up * Speed * Time.deltaTime;
		}
	}
	
	
	void StartAnimation()
	{
		state_ = State.ANIMATING;
	}
	
	
	void StopAnimation()
	{
		state_ = State.PAUSE;
	}
	
	
	void Reset()
	{
		flag_.transform.localPosition = originalPosition;
		state_ = State.NORMAL;
	}
}
