using Godot;
using System;

public partial class BasicEnemy : CharacterBody2D
{
	Player player;

	private float speed = 200;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Find the player in the scene
		player = GetParent().GetNode<Player>("Player");
	}

    public override void _PhysicsProcess(double delta)
    {
        LookAt(player.Position);
		Vector2 directionToPlayer = player.Position - this.Position;
		Velocity = directionToPlayer.Normalized() * speed;
		MoveAndSlide();
    }
}
