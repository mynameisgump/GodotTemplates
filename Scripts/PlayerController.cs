namespace GodotTemplates.Scripts;
using Godot;
using System;

public partial class PlayerController : CharacterBody3D
{
	public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;

	public float Gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

	private float CONTROL = 15f;
	private float DEACCEL = 0.86f;
	private float MAX_SPEED = 10f;
	private float MAX_ACCEL = 150.0f;
	
	private Vector3 _direction = new Vector3(0,0,0);
	private Vector3 _hvel = new Vector3(0, 0, 0);
	
	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		if (!IsOnFloor())
			velocity.Y -= Gravity * (float)delta;

		if (Input.IsActionJustPressed("move_jump") && IsOnFloor())
			velocity.Y = JumpVelocity;
		
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
	}
}
