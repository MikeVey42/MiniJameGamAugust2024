using Godot;
using System;
using System.Diagnostics;

public partial class SwordEnemy : Enemy
{
	PackedScene swordSlashPrefab;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Load the sword slash prefab
		swordSlashPrefab = GD.Load<PackedScene>("res://Scenes/Projectiles/AreaSwordSlash.tscn");

		desiredDistanceFromPlayer = 0;
		speed = 200;
		maxHealth = 3;
		timeBetweenAttacks = 1;

		base._Ready();
	}

	public override void fire() {
		if ((player.Position - this.Position).Length() < 150) {
			AreaSwordSlash swordSlash = (AreaSwordSlash) swordSlashPrefab.Instantiate();
			Vector2 directionToPlayer = (player.Position - this.Position).Normalized();
			swordSlash.Position = directionToPlayer * 15;
			swordSlash.Rotation = directionToPlayer.Angle();
			AddChild(swordSlash);
		}
	}
}
