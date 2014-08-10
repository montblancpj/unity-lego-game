using UnityEngine;
using System.Collections;

public class Blbl : MonoBehaviour {
	public float Magnitude = 0.1f;
	void Update () {
		float rad = Random.value;
		transform.localPosition = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0.0f) * Magnitude;
	}
}
