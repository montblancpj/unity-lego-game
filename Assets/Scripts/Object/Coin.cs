using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour {
	void OnTriggerEnter(Collider collider) 
	{
		if (collider.gameObject.tag == "Player") {
			Destroy(gameObject);
			GameObject.FindWithTag("SoundController").GetComponent<SoundController>().Play("Coin");
		}
	}
}
