using Godot;
using System;
using System.Diagnostics;

public partial class EliteSwordEnemy : Enemy
{
	PackedScene swordSlashPrefab;

	[Export] AudioStreamPlayer2D swingSound;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Load the sword slash prefab
		swordSlashPrefab = GD.Load<PackedScene>("res://Scenes/Projectiles/AreaSwordSlash.tscn");

		desiredDistanceFromPlayer = 0;
		speed = 200;
		maxHealth = 60;
		timeBetweenAttacks = 1;

		base._Ready();
	}

	public override void _PhysicsProcess(double delta)
    {
		Vector2 tailPosition = player.Position - Vector2.FromAngle(player.Rotation) * 140;
		Vector2 directionToPlayer = tailPosition - this.Position;
        if (directionToPlayer.Length() > desiredDistanceFromPlayer) {
		    Velocity = directionToPlayer.Normalized() * speed;
        }else if (directionToPlayer.Length() < desiredDistanceFromPlayer - 50) {
            Velocity = directionToPlayer.Normalized() * -speed;
        }else {
            Velocity = new Vector2(0, 0);
        }

		if (directionToPlayer.Length() > 1500) {
			Velocity *= 3;
		}

		MoveAndSlide();
		
    }

	public override void fire() {
		if ((player.Position - this.Position).Length() < 175) {
			AreaSwordSlash swordSlash = (AreaSwordSlash) swordSlashPrefab.Instantiate();
			Vector2 directionToPlayer = (player.Position - this.Position).Normalized();
			swordSlash.Position = directionToPlayer * 15;
			swordSlash.Rotation = directionToPlayer.Angle();
			AddChild(swordSlash);
			swingSound.Play();
		}
	}
}
