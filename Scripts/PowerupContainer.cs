using Godot;
using System;

public partial class PowerupContainer : DamageableEntity
{
	[Export] public Powerup storedPowerup;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		maxHealth = 1;
		base._Ready();

		OnDeath += GrantPowerup;
	}

	public void GrantPowerup() {
		// Grant the player the powerup inside
		GetParent().GetParent().GetNode<Player>("Player").ApplyPowerup(storedPowerup);
		// Tell the powerup spawner to make a new powerup somewhere else
		((PowerupSpawner) GetParent()).SpawnPowerupContainer();
		// Destroy this container
		CallDeferred(Node.MethodName.QueueFree);
	}
}
