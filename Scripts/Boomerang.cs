using Godot;
using System;
using System.Diagnostics;

public partial class Boomerang : RigidBody2D
{
	public float launchAngle;
	Stopwatch flightTImer;
	
	Sprite2D sprite;

	public float speed = 800;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		launchAngle = this.Rotation;
		flightTImer = new Stopwatch();
		flightTImer.Start();
		sprite = GetNode<Sprite2D>("Sprite2D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

    public override void _PhysicsProcess(double delta)
    {
		Vector2 flightVector;
        if (false && flightTImer.ElapsedMilliseconds < 500) {
			flightVector = Vector2.FromAngle(Rotation - (float) Math.PI / 2) * speed;
		}else {
			flightVector = Vector2.FromAngle(Rotation + (float) Math.PI / 2) * speed;
		}

		ApplyForce(flightVector);

		sprite.Rotate((float) Math.PI / 25);
    }
}
