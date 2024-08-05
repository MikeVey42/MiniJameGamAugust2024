using Godot;
using System;

public partial class SwordSlash : CharacterBody2D
{
	int damageAmount = 1;
	public float speed = 30;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
    {
		// Move the slash and check for a collision
        KinematicCollision2D collisionInfo = MoveAndCollide(Vector2.FromAngle(this.Rotation) * speed);

		// If we hit nothing, do nothing
		if (collisionInfo == null) {
			return;
		}
		// If the projectile didn't hit a collision shape, it's not the player's weakpoint, and delete this object
		if (!(collisionInfo.GetColliderShape() is CollisionShape2D)) {
			QueueFree();
			return;
		}

		// Convert to a collision shape to get it's name
		CollisionShape2D collidedShape = (CollisionShape2D) collisionInfo.GetColliderShape();

		// If it's not the player's weakpoint, just delete the projectile
		if (collidedShape.Name != "WeakPoint") {
			QueueFree();
			return;
		}

		// Otherwise, damage the player then delete this object
		Player player = (Player) collisionInfo.GetCollider();
		player.Damage(damageAmount);
		QueueFree();
		
    }
}
