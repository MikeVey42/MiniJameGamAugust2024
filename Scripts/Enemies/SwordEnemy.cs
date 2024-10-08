using Godot;
using System;
using System.Diagnostics;

public partial class SwordEnemy : Enemy
{
	PackedScene swordSlashPrefab;

	[Export] AudioStreamPlayer2D swingSound;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Load the sword slash prefab
		swordSlashPrefab = GD.Load<PackedScene>("res://Scenes/Projectiles/AreaSwordSlash.tscn");

		desiredDistanceFromPlayer = 0;
		speed = 250;
		maxHealth = 12;
		timeBetweenAttacks = 1;

		base._Ready();
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
