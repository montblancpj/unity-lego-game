using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour 
{
	public Texture[] Textures;
	private GameObject textureObject_;
	public string RendereObjectName = "Picture";
	
	private Vector3 forwardScale_, backwardScale_;

	protected int cnt_ = 0;
	public int AnimationFrameLength = 10;
	public float Speed = -1.0f;
	public float MaxFallSpeed = 20.0f;

	protected bool isHit_ = false;


	void Awake()
	{
		// child object that is for texture
		textureObject_ = transform.FindChild(RendereObjectName).gameObject;
		
		// direction scale vector
		forwardScale_  = transform.localScale;
		backwardScale_ = new Vector3(
			-forwardScale_.x, forwardScale_.y, forwardScale_.z);
	}


	void Update() {
		if (isHit_) {
			Disappear();
		} else {
			Move();
		}
		++cnt_;
	}


	protected virtual void Move()
	{
		FaceFront();

		// Turn
		RaycastHit hit;
		float rayLength = transform.collider.bounds.size.x / 2.0f;
		Vector3 direction = (Speed > 0.0f) ? Vector3.right : Vector3.left;
		if ( Physics.Raycast(transform.position, direction, out hit, rayLength) ) {
			if (hit.transform.tag != "Player") {
				Speed *= -1.0f;
			}
		}
	}
	
	
	protected void FaceFront()
	{
		int num = (cnt_ / AnimationFrameLength) % Textures.Length;
		textureObject_.renderer.material.mainTexture = Textures[num];
		transform.localPosition += Vector3.right * Speed * Time.deltaTime;
		
		if ( Speed < 0 ) {
			transform.localScale = forwardScale_;
		} else {
			transform.localScale = backwardScale_;
		}
	}


	protected virtual void Disappear()
	{
		const float shrinkingRate = 0.06f;
		transform.localScale    -= Vector3.up * shrinkingRate;
		transform.localPosition -= Vector3.up * shrinkingRate;
		if (transform.localScale.y < 0.01f) {
			Destroy(gameObject);
		}
	}


	protected virtual void Attacked()
	{
		if (isHit_) return;

		isHit_ = true;
		Destroy(gameObject.collider);
		Destroy(gameObject.rigidbody);
		GameObject.FindWithTag("SoundController").GetComponent<SoundController>().Play("Tramp");
	}


	void OnCollisionEnter(Collision collision) {
		if (isHit_) return;

		switch (collision.transform.tag) {
			case "Player":
				collision.transform.SendMessage("Dead");
				break;
			case "Outer":
				Destroy(gameObject);
				break;
			default:
				break;
		}
	}
}
