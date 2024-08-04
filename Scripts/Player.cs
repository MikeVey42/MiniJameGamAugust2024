using Godot;
using System;

public partial class Player : CharacterBody2D
{

	int speed = 400;  // move speed in pixels/sec

    public override void _Ready()
    {

    }

    public override void _PhysicsProcess(double delta)
    {
		LookAt(GetGlobalMousePosition());
		float yVelocity = Input.GetAxis("up", "down");
		float xVelocity = Input.GetAxis("left", "right");
		Velocity = new Vector2(xVelocity, yVelocity) * speed;
		MoveAndSlide();
    }
}
