using Godot;
using System;

public partial class player_controller : CharacterBody3D
{
	public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;

	public float Gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

	public float CONTROL = 15f;
	public float DEACCEL = 0.86f;
	public float MAX_SPEED = 10f;
	public float MAX_ACCEL = 150.0f;
	
	private Vector3 _direction = new Vector3(0,0,0);
	private Vector3 _hvel = new Vector3(0, 0, 0);
	
	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y -= Gravity * (float)delta;

		// Handle Jump.
		if (Input.IsActionJustPressed("move_jump") && IsOnFloor())
			velocity.Y = JumpVelocity;
		
		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 inputDir = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		_direction = _direction.Lerp((Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized(),
			(float)delta * CONTROL);
		_hvel = Velocity;
		_hvel.Y = 0;
		_hvel *= DEACCEL;

		if (_hvel.Length() < MAX_SPEED * 0.01)
		{
			_hvel = Vector3.Zero;
		}

		float speed = _hvel.Dot(_direction);

		double accel = Mathf.Clamp(MAX_SPEED - speed, 0.0, MAX_ACCEL * delta);
		_hvel += _direction * (float)accel;

		velocity.X = _hvel.X;
		velocity.Z = _hvel.Z;
		Velocity = velocity;

		MoveAndSlide();

		//Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		// if (direction != Vector3.Zero)
		// {
		// 	velocity.X = direction.X * Speed;
		// 	velocity.Z = direction.Z * Speed;
		// }
		// else
		// {
		// 	velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		// 	velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		// }
		//
		// Velocity = velocity;
		// MoveAndSlide();
	}
}
