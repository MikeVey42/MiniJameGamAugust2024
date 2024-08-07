using Godot;
using System;
using System.Diagnostics;

public partial class BowEnemy : Enemy
{
	PackedScene arrowPrefab;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Load the arrow prefab
		arrowPrefab = GD.Load<PackedScene>("res://Scenes/Projectiles/Arrow.tscn");

		desiredDistanceFromPlayer = 400;
		speed = 150;
		maxHealth = 2;
		timeBetweenAttacks = 2;

		
		base._Ready();
	}
	public override void fire() {
		if ((player.Position - this.Position).Length() < 600) {
			BasicProjectile arrow = (BasicProjectile) arrowPrefab.Instantiate();
			Vector2 directionToPlayer = (player.Position - this.Position).Normalized();
			arrow.Position = this.Position + directionToPlayer * 30;
			arrow.Rotation = directionToPlayer.Angle();
			GetParent().GetParent().AddChild(arrow);
		}
	}
}
