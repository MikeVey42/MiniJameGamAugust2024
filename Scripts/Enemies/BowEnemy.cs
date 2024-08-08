using Godot;
using System;
using System.Diagnostics;

public partial class BowEnemy : Enemy
{
	PackedScene arrowPrefab;
	// Desired angle the enemy wants to attack the tail from
	float angleOffset;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Load the arrow prefab
		arrowPrefab = GD.Load<PackedScene>("res://Scenes/Projectiles/Arrow.tscn");

		desiredDistanceFromPlayer = GD.RandRange(350, 450);
		speed = 150;
		maxHealth = 2;
		timeBetweenAttacks = 2 + (float) GD.RandRange(-0.1, 0.1);

		angleOffset = (float) GD.RandRange(-Math.PI / 3, Math.PI / 3);

		
		base._Ready();
	}
	public override void fire() {
		if ((player.Position - this.Position).Length() < 600) {
			BasicProjectile arrow = (BasicProjectile) arrowPrefab.Instantiate();
			Vector2 tailPosition = player.Position - Vector2.FromAngle(player.Rotation) * 55;
			Vector2 directionToPlayer = (tailPosition - this.Position).Normalized();
			arrow.Position = this.Position + directionToPlayer * 30;
			arrow.Rotation = directionToPlayer.Angle();
			GetParent().GetParent().AddChild(arrow);
		}
	}

	public override void _PhysicsProcess(double delta)
    {
		Vector2 directionToPlayer = player.Position - this.Position;
		// If the enemy is about to or just attacked, don't move
		if (directionToPlayer.Length() < 600 && (attackTimer.TimeLeft < 0.25 || attackTimer.TimeLeft > timeBetweenAttacks - 0.25)) {
			Velocity = new Vector2(0, 0);
		} // If the player is far away, move in
        else if (directionToPlayer.Length() > desiredDistanceFromPlayer) {
		    Velocity = directionToPlayer.Normalized() * speed;
        } // If the player is too close, run away
		else if (directionToPlayer.Length() < desiredDistanceFromPlayer - 100) {
            Velocity = directionToPlayer.Normalized() * -speed;
        }else {
            float angleToTail = (player.Rotation + angleOffset) - directionToPlayer.Angle();
			if (angleToTail > 0.04 || angleToTail < (float) -Math.PI) {
				Velocity = directionToPlayer.Normalized().Rotated((float) Math.PI / 2) * -speed;
			}else if (angleToTail < -0.04 || angleToTail > (float) Math.PI) {
				Velocity = directionToPlayer.Normalized().Rotated((float) Math.PI / 2) * speed;
			}
        }

		MoveAndSlide();
    }
}
