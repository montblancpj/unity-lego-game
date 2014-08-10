using UnityEngine;
using System.Collections;

public class Pipe : MonoBehaviour
{
	public GameObject ConnectedPipe;
	public enum Direction {
		UP,
		LEFT,
		RIGHT,
		DOWN
	};
	public Direction PipeDirection;

	void Update () {

	}
}
