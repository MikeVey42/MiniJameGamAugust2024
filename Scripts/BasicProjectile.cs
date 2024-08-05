using Godot;
using System;

public partial class BasicProjectile : CharacterBody2D
{
	[Export]
	protected int damageAmount {get; set;}
	[Export]
	protected  float speed {get; set;}
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
		
		// If we hit an invincible object, destroy this projectile
		if (!(collisionInfo.GetCollider() is DamageableEntity)) {
			QueueFree();
			return;
		}

		// Find the specific hitbox that was struck
		Node hitBox = (Node) collisionInfo.GetColliderShape();

		// If the hitbox is invincible (like a shield or a turtle shell), destroy this projectile
		if (hitBox.IsInGroup("invincible")) {
			QueueFree();
			return;
		}

		// Otherwise, damage the target that was hit then delete this object
		DamageableEntity target = (DamageableEntity) collisionInfo.GetCollider();
		target.Damage(damageAmount);
		QueueFree();
		
    }
}
