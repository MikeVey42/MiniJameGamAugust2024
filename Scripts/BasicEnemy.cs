using Godot;
using System;
using System.Diagnostics;

public partial class BasicEnemy : CharacterBody2D
{
	Player player;
	PackedScene swordSlashPrefab;

	private float speed = 200;

	Timer attackTimer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Find the player in the scene
		player = GetParent().GetNode<Player>("Player");
		// Load the sword slash prefab
		swordSlashPrefab = GD.Load<PackedScene>("res://Scenes/SwordSlash.tscn");
		attackTimer = new Timer();
		attackTimer.Start(1);
		attackTimer.Timeout += fire;
	}

    public override void _PhysicsProcess(double delta)
    {
        LookAt(player.Position);
		Vector2 directionToPlayer = player.Position - this.Position;
		Velocity = directionToPlayer.Normalized() * speed;
		MoveAndSlide();
    }

	public void fire() {
		if ((player.Position - this.Position).Length() < 150) {
			SwordSlash swordSlash = (SwordSlash) swordSlashPrefab.Instantiate();
			Debug.Print("fired!");
			GetParent().AddChild(swordSlash);
			swordSlash.Position = this.Position;
			swordSlash.Rotation = this.Rotation;
		}
	}
}
