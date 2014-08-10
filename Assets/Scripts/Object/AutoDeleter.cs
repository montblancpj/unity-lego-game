using UnityEngine;
using System.Collections;

public class AutoDeleter : MonoBehaviour {
	void Update () {
		if (transform.childCount == 0) Destroy(gameObject);
	}
}
