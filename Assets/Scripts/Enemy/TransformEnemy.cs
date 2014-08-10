using UnityEngine;
using System.Collections;

public class TransformEnemy : Enemy 
{
	public GameObject Carapace;
	
	protected override void Attacked()
	{
		Instantiate(Carapace, transform.position, transform.rotation);
		GameObject.FindWithTag("SoundController").GetComponent<SoundController>().Play("Tramp");
		Destroy(gameObject);
	}
}
