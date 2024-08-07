using Godot;
using System;

public partial class AnimatedAreaAttack : Area2D
{
	[Export] public int damageAmount {get; set;}

	private AnimatedSprite2D animation;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Hit things when they enter this area
		BodyEntered += hit;
		// Make the animation play and the area dissapear when it's animation ends
		foreach (Node child in GetChildren()) {
			if (child is AnimatedSprite2D) {
				animation = (AnimatedSprite2D) child;
				break;
			}
		}
		animation.Play();
		animation.AnimationLooped += () => QueueFree();
	}

	public void hit(Node2D bodyHit) {
		if (!(bodyHit is DamageableEntity)) {
			return;
		}

		DamageableEntity target = (DamageableEntity) bodyHit;

		target.Damage(damageAmount);
	}
}
