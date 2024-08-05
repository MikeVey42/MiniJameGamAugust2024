using Godot;
using System;

public partial class BasicAreaAttack : Area2D
{
	[Export] protected int damageAmount {get; set;}
	[Export] protected int lifespan {get; set;}

	private Timer lifeTimer;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Hit things when they enter this area
		BodyEntered += hit;
		// Make the area dissapear when it's lifetime is up
		lifeTimer = new Timer() {
			Autostart = false,
			OneShot = true
		};
		AddChild(lifeTimer);
		lifeTimer.Start(lifespan);
		lifeTimer.Timeout += () => QueueFree();
	}

	public void hit(Node2D bodyHit) {
		if (!(bodyHit is DamageableEntity)) {
			return;
		}

		DamageableEntity target = (DamageableEntity) bodyHit;

		target.Damage(damageAmount);
	}
}
