using Godot;
using System;

public partial class PowerupContainer : DamageableEntity
{
	[Export] public Powerup storedPowerup;

	Player player;

	[Export] AudioStreamPlayer2D hitSound;

	private Sprite2D cracks;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		maxHealth = 60;
		base._Ready();

		OnDeath += GrantPowerup;

		player = GetParent().GetParent().GetNode<Player>("Player");

		OnDamage += () => hitSound.Play();

		GetNode<Sprite2D>("PowerupIcon").Texture = storedPowerup.GetIcon();

		cracks = GetNode<Sprite2D>("Cracks");
	}

    public override void _Process(double delta)
    {
        if (player.glassShards >= 10) {
			GetNode<Sprite2D>("PowerupIcon").Texture = Powerup.LoadIcon("Glasses");
		}

		// Show the health in the cracks on the orb
		if (health <= 6) {
			cracks.Frame = 4;
		}else if (health <= 18) {
			cracks.Frame = 3;
		}else if (health <= 30) {
			cracks.Frame = 2;
		}else if (health <= 42) {
			cracks.Frame = 1;
		}else if (health <= 54) {
			cracks.Frame = 0;
		}else {
			cracks.Frame = 5;
		}
    }

    public void GrantPowerup() {
		if (player.glassShards >= 10) {
			player.ApplyPowerup(new LaserPowerup());
			GetParent().GetParent().GetNode<EnemyManager>("Enemy Manager").Swarm();
			CallDeferred(Node.MethodName.QueueFree);
			return;
		}

		// Grant the player the powerup inside
		player.ApplyPowerup(storedPowerup);
		// Tell the powerup spawner to make a new powerup somewhere else
		((PowerupSpawner) GetParent()).SpawnPowerupContainer();
		// Destroy this container
		CallDeferred(Node.MethodName.QueueFree);
	}
}
