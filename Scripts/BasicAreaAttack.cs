using Godot;
using System;

public partial class BasicAreaAttack : Area2D
{
	[Export] protected int damageAmount {get; set;}
	[Export] protected float lifespan {get; set;}

	private Timer lifeTimer;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Hit things when they enter this area
		BodyShapeEntered += hit;
		// Make the area dissapear when it's lifetime is up
		lifeTimer = new Timer() {
			Autostart = false,
			OneShot = true
		};
		AddChild(lifeTimer);
		lifeTimer.Start(lifespan);
		lifeTimer.Timeout += () => QueueFree();
	}

    private void hit(Rid bodyRid, Node2D body, long bodyShapeIndex, long localShapeIndex)
    {
		// Find the shape that we hit
		PhysicsBody2D physicsBody = (PhysicsBody2D) body;
		uint bodyShapeOwner = physicsBody.ShapeFindOwner((int)bodyShapeIndex);
		Node2D shapeHit = (Node2D) physicsBody.ShapeOwnerGetOwner(bodyShapeOwner);

		if (!(physicsBody is DamageableEntity)) {
			return;
		}

		if (shapeHit.IsInGroup("invincible")) {
			return;
		}

		DamageableEntity target = (DamageableEntity) physicsBody;

		target.Damage(damageAmount);
    }
}
