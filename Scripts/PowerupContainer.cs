using Godot;
using System;

public partial class PowerupContainer : DamageableEntity
{
	[Export] public Powerup storedPowerup;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		storedPowerup = new DamageUpPowerup();

		maxHealth = 30;
		base._Ready();

		OnDeath += GrantPowerup;
	}

	public void GrantPowerup() {
		GetParent().GetNode<Player>("Player").ApplyPowerup(storedPowerup);
		QueueFree();
	}
}
