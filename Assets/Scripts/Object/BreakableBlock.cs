using UnityEngine;
using System.Collections;

public class BreakableBlock : MonoBehaviour 
{
	public GameObject effect1;
	public GameObject effect2;
	private bool isBroken_ = false;
	private int  cnt_      = 0;

	void Hit() 
	{
		SendMessage("Emit"); // SolenoidBlock.cs
		SoundController.Instance.Play("Explosion");
		if (effect1) {
			Instantiate(effect1, transform.position, effect1.transform.rotation);
		}
		if (effect2) {
			Instantiate(effect2, transform.position, effect2.transform.rotation);
		}
		// To enable collision
		isBroken_ = true;
	}

	void Update()
	{
		if (isBroken_) {
			if (cnt_ == 3) { // delay
				var destroyer = gameObject.AddComponent<DestroyAfter>();
				destroyer.destroyCnt = 300;
				transform.localScale = Vector3.zero;
			}
			++cnt_;
		}
	}
}