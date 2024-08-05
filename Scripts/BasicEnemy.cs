using Godot;
using System;
using System.Diagnostics;

public partial class BasicEnemy : DamageableEntity
{
	Player player;
	PackedScene swordSlashPrefab;

	private float speed = 200;

	Timer attackTimer;
	// Timer for how long enemies should turn red when hit
	Timer hitFlashTImer;

	[Export] private Color hitFlashColor;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Find the player in the scene
		player = GetParent().GetParent().GetNode<Player>("Player");
		// Load the sword slash prefab
		swordSlashPrefab = GD.Load<PackedScene>("res://Scenes/AreaSwordSlash.tscn");
		// Create a timer to automatically attack
        attackTimer = new Timer
        {
            OneShot = false,
            Autostart = true,
			WaitTime = 1
        };
        AddChild(attackTimer);
		attackTimer.Timeout += fire;

		// Set the max health
		maxHealth = 3;
		base._Ready();
		// Delete this object when it dies
		OnDeath += () => QueueFree();

		// Start the walking animation
		AnimatedSprite2D animation = (AnimatedSprite2D) GetChild(0);
		animation.Play();

		// Set up hitflash timer
		hitFlashTImer = new Timer {
			OneShot = true,
			Autostart = false,
			WaitTime = 0.5
		};
		AddChild(hitFlashTImer);
		hitFlashTImer.Timeout += hitFlashOff;

		OnDamage += hitFlashOn;
	}

    public override void _PhysicsProcess(double delta)
    {
		Vector2 directionToPlayer = player.Position - this.Position;
		Velocity = directionToPlayer.Normalized() * speed;
		MoveAndSlide();
    }

	public void fire() {
		if ((player.Position - this.Position).Length() < 150) {
			AreaSwordSlash swordSlash = (AreaSwordSlash) swordSlashPrefab.Instantiate();
			Vector2 directionToPlayer = (player.Position - this.Position).Normalized();
			swordSlash.Position = directionToPlayer * 15;
			swordSlash.Rotation = directionToPlayer.Angle();
			AddChild(swordSlash);
		}
	}

	public void hitFlashOn() {
		Modulate = hitFlashColor;
		hitFlashTImer.Start();
	}

	public void hitFlashOff() {
		Modulate = new Color(1,1,1);
	}
}
