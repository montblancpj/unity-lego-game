using UnityEngine;
using System.Collections;

public class Belt : MonoBehaviour {

	public float maxY  = 14;
	public float minY  = -14;
	public float speed = 1;

	void Update()
	{
		transform.position += speed * Time.deltaTime * Vector3.down;
		if (transform.position.y < minY || transform.position.y > maxY) {
			Destroy(gameObject);
		}
	}
	
}
