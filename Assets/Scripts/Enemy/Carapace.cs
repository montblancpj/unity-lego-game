using UnityEngine;
using System.Collections;

public class Carapace : Enemy
{
	public bool IsMoving = false;
	
	
	protected override void Move()
	{
		if (!IsMoving) return;
		
		FaceFront();

		// Turn
		RaycastHit hit;
		float rayLength = transform.collider.bounds.size.x / 2.0f;
		Vector3 direction = (Speed > 0.0f) ? Vector3.right : Vector3.left;
		if ( Physics.Raycast(transform.position, direction, out hit, rayLength) ) {
			if (hit.transform.tag != "Player" && hit.transform.tag != "Enemy") {
				Speed *= -1.0f;
			}
		}
	}
	
	
	void Kicked(CharacterController player)
	{
		Vector3 diff = player.transform.position - transform.position;
		
		if (IsMoving) {
			if (player.velocity.y > -0.01f && (Speed * player.velocity.x > 0) ) {
				player.SendMessage("Dead");
				return;
			}
		} else {
			if (diff.x < 0) {
				Speed = +Mathf.Abs(Speed);
			} else {
				Speed = -Mathf.Abs(Speed);
			}
		}
		GameObject.FindWithTag("SoundController").GetComponent<SoundController>().Play("Tramp");
		IsMoving = !IsMoving;
	}
	
	
	void OnCollisionEnter(Collision collision) {
		switch (collision.transform.tag) {
			case "Player":
				if (IsMoving) {
					collision.transform.SendMessage("Dead");
				}
				break;
			case "Enemy":
				if (IsMoving) {
					collision.transform.SendMessage("Attacked");
				}
				break;
			case "Outer":
				Destroy(gameObject);
				break;
			default:
				break;
		}
	}
}
