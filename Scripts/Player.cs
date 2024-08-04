using Godot;
using System;

public partial class Player : CharacterBody2D
{

	int speed = 400;  // move speed in pixels/sec


	PackedScene boomerangPrefab = GD.Load<PackedScene>("res://Scenes/boomerang.tscn");

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

		if (Input.IsActionJustPressed("click")) {
			RigidBody2D boomerang = (RigidBody2D)boomerangPrefab.Instantiate();
			GetParent().AddChild(boomerang);
			Transform2D spawnPoint =  this.Transform.Translated(Vector2.FromAngle(Transform.Rotation) * 200);
			boomerang.Position = this.Position + Vector2.FromAngle(this.Rotation) * 95;
			boomerang.Rotation = this.Rotation + (float) Math.PI / 2;
			boomerang.ApplyImpulse(Vector2.FromAngle(Rotation) * 600);
		}


    }
}
