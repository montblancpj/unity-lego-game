using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	// character controller
	private CharacterController controller_;
	private Transform player_;
	public string PlayerName;
	
	// textures for animations
	public Texture   IdleTexture;
	public Texture[] WalkTextures;
	public Texture[] JumpTextures;
	public Texture[] GoalTextures;
	public Texture[] PipeTextures;
	public Texture[] DeadTextures;
	
	// animation counter
	private int cnt_;
	public int WalkAnimationFrameLength = 10;
	public int DashAnimationFrameLength = 5;
	public int JumpAnimationFrameLength = 5;
	public int GoalAnimationFrameLength = 5;
	public int PipeAnimationFrameLength = 5;
	public int DeadAnimationFrameLength = 5;

	// speed
	public float WalkSpeed     = 4.0f;
	public float DashSpeed     = 8.0f;
	public float Gravity       = 20.0f;
	public float MaxFallSpeed  = 20.0f;
	public float GoalFallSpeed = 4.0f;

	// jump
	public float JumpForce  = 5.0f;
	private int jumpCnt_    = 0;
	private int jumpCntMax_ = 7;
	private const float JUMP_RATIO_WHEN_TRAMP_ENEMY = 5.0f;

	// direction
	bool isForward_ = true;
	private Vector3 forwardScale_, backwardScale_;

	// move state
	private enum MoveState {
		IDLE,
		WALK,
		DASH
	};
	private MoveState moveState_ = MoveState.IDLE;

	// state
	enum State {
		NORMAL,
		DEAD,
		ENTER_PIPE,
		EXIT_PIPE,
		GOAL
	};
	private State state_ = State.NORMAL;

	// move without collision
	private bool isMovingWithoutCollision = false;

	// Pipe
	private GameObject enterPipeObject_;
	private GameObject exitPipeObject_;


	void Start()
	{
		// player object
		player_ = transform.FindChild(PlayerName);

		// character controller
		controller_ = GetComponent<CharacterController>();

		// direction scale vector
		forwardScale_  = player_.localScale;
		backwardScale_ = new Vector3(
			-forwardScale_.x, forwardScale_.y, forwardScale_.z);
	}


	void Update()
	{
		// Set move state and direction
		if ( IsMoving() ) {
			if ( IsDashKeyPressed() ) {
				moveState_ = MoveState.DASH;
			} else {
				moveState_ = MoveState.WALK;
			}
		} else {
			moveState_ = MoveState.IDLE;
		}

		switch (state_) {
			case State.NORMAL:
				Move();
				Draw();
				break;
			case State.ENTER_PIPE:
				EnterPipeAnimation();
				break;
			case State.EXIT_PIPE:
				ExitPipeAnimation();
				break;
			case State.GOAL:
				GoalAnimation();
				break;
			case State.DEAD:
				DeadAnimation();
				break;
		}

		++cnt_;
	}


	void LateUpdate()
	{
		if (isMovingWithoutCollision) {
			isMovingWithoutCollision = false;
			controller_.enabled = true;
		}
	}


	void Move()
	{
		if (!controller_.enabled) return;

		float vx = controller_.velocity.x;
		float vy = controller_.velocity.y;

		// Move
		float direction = IsForward() ? 1.0f : -1.0f;
		switch (moveState_) {
			case MoveState.WALK:
				vx = direction * WalkSpeed;
				break;
			case MoveState.DASH:
				vx = direction * DashSpeed;
				break;
			default:
				vx = 0;
				break;
		}

		// Jump & Gravity
		if ( IsJumpKeyDown() && controller_.isGrounded ) {
			vy = JumpForce;
			GameObject.FindWithTag("SoundController").GetComponent<SoundController>().Play("Jump");
			jumpCnt_ = 0;
		} else if ( IsJumpKeyPressed() && jumpCnt_ < jumpCntMax_ ) {
			vy += JumpForce;
		} else {
			vy += Physics.gravity.y * Time.deltaTime;
			if (vy < -MaxFallSpeed) {
				vy = -MaxFallSpeed;
			}
		}
		++jumpCnt_;

		controller_.Move(new Vector3(vx, vy, 0.0f) * Time.deltaTime);
	}


	void Draw()
	{
		// set direction scale
		if ( IsForward() ) {
			player_.localScale = forwardScale_;
		} else {
			player_.localScale = backwardScale_;
		}

		int num = 0;
		if (controller_.isGrounded) {
			// start jump animation
			if ( IsJumpKeyDown() ) {
				cnt_ = 0;
			}

			// move animation (loop)
			switch (moveState_) {
				case MoveState.WALK:
					num = (cnt_ / WalkAnimationFrameLength) % WalkTextures.Length;
					player_.renderer.material.mainTexture = WalkTextures[num];
					break;
				case MoveState.DASH:
					num = (cnt_ / DashAnimationFrameLength) % WalkTextures.Length;
					player_.renderer.material.mainTexture = WalkTextures[num];
					break;
				case MoveState.IDLE:
					player_.renderer.material.mainTexture = IdleTexture;
					break;
			}
		// jump animation (once)
		} else {
			num = (cnt_ / JumpAnimationFrameLength) % JumpTextures.Length;
			/*if (cnt_  > JumpTextures.Length * JumpAnimationFrameLength) {
				num = JumpTextures.Length - 1;
			}*/
			player_.renderer.material.mainTexture = JumpTextures[num];
		}
	}


	void GoalAnimation()
	{
		int num = (cnt_ / GoalAnimationFrameLength) % GoalTextures.Length;
		player_.renderer.material.mainTexture = GoalTextures[num];

		float vy = -GoalFallSpeed;
		controller_.Move(new Vector3(0.0f, vy, 0.0f) * Time.deltaTime);
		if ((controller_.collisionFlags & CollisionFlags.Below) != 0) {
			MoveWithoutCollision(transform.localPosition + Vector3.right * 1.0f);
			state_ = State.NORMAL;
		}
	}


	void EnterPipeAnimation()
	{
		int num = (cnt_ / PipeAnimationFrameLength) % PipeTextures.Length;
		player_.renderer.material.mainTexture = PipeTextures[num];

		// disable controller
		controller_.enabled = false;

		// enter
		var pipe = enterPipeObject_.GetComponent<Pipe>();
		Vector3 direction = Vector3.zero;
		switch (pipe.PipeDirection) {
			case Pipe.Direction.UP    : direction = Vector3.down;  break;
			case Pipe.Direction.LEFT  : direction = Vector3.right; break;
			case Pipe.Direction.RIGHT : direction = Vector3.left;  break;
			case Pipe.Direction.DOWN  : direction = Vector3.up;    break;
		}
		const float enterSpeed = 3.0f;
		transform.localPosition += direction * enterSpeed * Time.deltaTime;

		// exit
		if (cnt_ >= 30) {
			var exitPipe = exitPipeObject_.GetComponent<Pipe>();
			Vector3 offsetPosition = Vector3.zero;
			switch (exitPipe.PipeDirection) {
				case Pipe.Direction.UP    : offsetPosition = -Vector3.up    * 1.0f; break;
				case Pipe.Direction.LEFT  : offsetPosition = -Vector3.left  * 1.0f; break;
				case Pipe.Direction.RIGHT : offsetPosition = -Vector3.right * 1.0f; break;
				case Pipe.Direction.DOWN  : offsetPosition = -Vector3.down  * 1.0f; break;
			}
			transform.position = exitPipeObject_.transform.position + offsetPosition;
			cnt_ = 0;
			state_ = State.EXIT_PIPE;
		}
	}


	void ExitPipeAnimation()
	{
		int num = (cnt_ / PipeAnimationFrameLength) % PipeTextures.Length;
		player_.renderer.material.mainTexture = PipeTextures[num];

		// exit
		var pipe = exitPipeObject_.GetComponent<Pipe>();
		Vector3 direction = Vector3.zero;
		switch (pipe.PipeDirection) {
			case Pipe.Direction.UP    : direction = Vector3.up;    break;
			case Pipe.Direction.LEFT  : direction = Vector3.left;  break;
			case Pipe.Direction.RIGHT : direction = Vector3.right; break;
			case Pipe.Direction.DOWN  : direction = Vector3.down;  break;
		}
		const float exitSpeed = 3.0f;
		transform.localPosition += direction * exitSpeed * Time.deltaTime;
		if (cnt_ >= 30) {
			cnt_ = 0;
			controller_.enabled = true;
			state_ = State.NORMAL;
			transform.localPosition -= Vector3.forward * 0.1f;
		}

	}
	
	
	void DeadAnimation()
	{
		int num = (cnt_ / DeadAnimationFrameLength) % DeadTextures.Length;
		player_.renderer.material.mainTexture = DeadTextures[num];
		
		transform.localPosition += Vector3.up * ( 10.0f -(0.3f * cnt_) ) * Time.deltaTime;
		
		const float zPositionOffset = 2.0f;
		if (cnt_ == 1) {
			transform.localPosition -= zPositionOffset * Vector3.forward;
		} else if (cnt_ >= 180) {
			state_ = State.NORMAL;
			transform.localPosition += zPositionOffset * Vector3.forward;
				
			player_.localScale = forwardScale_;
			controller_.enabled = true;
			controller_.Move(Vector2.zero);
			MoveWithoutCollision(GameObject.FindWithTag("Respawn").transform.position);
	
			// reset goal
			if (GameObject.FindWithTag("Goal")) {
				GameObject.FindWithTag("Goal").transform.SendMessage("Reset");
			}
		}
	}


	void OnControllerColliderHit(ControllerColliderHit hit)
	{
		switch (hit.transform.tag) {
			case "Outer":
				Dead();
				break;
			case "Hittable":
				if (hit.moveDirection.y > 0) {
					hit.transform.SendMessage("Hit");
				}
				break;
			case "Kickable":
				hit.transform.SendMessage("Kicked", hit.controller);
				if (controller_.velocity.y < -0.01f) {
					float vx = controller_.velocity.x;
					float vy = JumpForce * JUMP_RATIO_WHEN_TRAMP_ENEMY * 2;
					controller_.Move(Vector3.zero);
					controller_.Move(new Vector3(vx, vy, 0.0f) * Time.deltaTime);
					jumpCnt_ = jumpCntMax_ - 2;
				}
				break;
			case "Enemy":
				if (controller_.velocity.y < -0.01f) {
					hit.transform.SendMessage("Attacked");
					float vx = controller_.velocity.x;
					float vy = JumpForce * JUMP_RATIO_WHEN_TRAMP_ENEMY;
					controller_.Move(Vector3.zero);
					controller_.Move(new Vector3(vx, vy, 0.0f) * Time.deltaTime);
					jumpCnt_ = 0;
				} else {
					Dead();
				}
				break;
			case "Pipe":
				EnterPipe(hit.gameObject);
				break;
			case "Goal":
				if (state_ != State.GOAL) {
					hit.transform.SendMessage("StartAnimation");
					state_ = State.GOAL;
				}
				break;
			case "Warp Point":
				var warpPoint = hit.gameObject.GetComponent<WarpPoint>().WarpTo.transform.position;
				MoveWithoutCollision(warpPoint);
				break;
			case "Goal Point":
				Dead();
				break;
		}
	}


	void MoveWithoutCollision(Vector3 position)
	{
		// for avoiding interection with other objects, make controller disabled and move it,
		// then enable it at next LateUpdate().

		controller_.enabled = false;
		transform.position = position;
		isMovingWithoutCollision = true;
	}


	void Dead()
	{
		controller_.enabled = false;
		cnt_ = 0;
		state_ = State.DEAD;
	}


	void EnterPipe(GameObject pipeObject)
	{
		var pipe = pipeObject.GetComponent<Pipe>();
		if (
			(pipe.PipeDirection == Pipe.Direction.UP    && Input.GetAxis("Vertical")   < 0.0f) ||
			(pipe.PipeDirection == Pipe.Direction.LEFT  && Input.GetAxis("Horizontal") > 0.0f) ||
			(pipe.PipeDirection == Pipe.Direction.RIGHT && Input.GetAxis("Horizontal") < 0.0f) ||
			(pipe.PipeDirection == Pipe.Direction.DOWN  && Input.GetAxis("Vertical")   > 0.0f)
		) {
			enterPipeObject_ = pipe.gameObject;
			exitPipeObject_  = pipe.ConnectedPipe;
			cnt_ = 0;
			transform.localPosition += Vector3.forward * 0.1f;
			state_ = State.ENTER_PIPE;
		}
	}


	bool IsMoving()
	{
		return Mathf.Abs(Input.GetAxis("Horizontal")) > 0.01f;
	}


	bool IsForward()
	{
		if (Input.GetAxis("Horizontal") != 0) {
			isForward_ = Input.GetAxis("Horizontal") > 0;
		};
		return isForward_;
	}


	bool IsJumpKeyDown()
	{
		return Input.GetButtonDown("Jump") ||
			Input.GetKeyDown(KeyCode.JoystickButton0) ||
			Input.GetKeyDown(KeyCode.JoystickButton1) ||
			Input.GetKeyDown(KeyCode.JoystickButton3);
	}


	bool IsJumpKeyPressed()
	{
		return Input.GetButton("Jump") ||
			Input.GetKey(KeyCode.JoystickButton0) ||
			Input.GetKey(KeyCode.JoystickButton1) ||
			Input.GetKey(KeyCode.JoystickButton3);
	}


	bool IsDashKeyPressed()
	{
		return
			Input.GetKey(KeyCode.JoystickButton2) ||
			Input.GetKey(KeyCode.LeftShift) ||
			Input.GetKey(KeyCode.RightShift);
	}
}
