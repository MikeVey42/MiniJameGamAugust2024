using Godot;
using System;
using System.Diagnostics;

public abstract partial class Enemy : DamageableEntity
{
	protected Player player;

	protected Timer attackTimer;
	// Timer for how long enemies should turn red when hit
	private Timer hitFlashTImer;

	[Export] private Color hitFlashColor;


    // CUSTOMIZATION
    [Export] protected float timeBetweenAttacks = 1;
    [Export] protected float desiredDistanceFromPlayer = 0;
    [Export] protected Color baseColor;
    [Export] protected float speed = 200;

	[Export] AudioStreamPlayer2D hurtSound;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Find the player in the scene
		player = GetParent().GetParent().GetNode<Player>("Player");
		// Create a timer to automatically attack
        attackTimer = new Timer
        {
            OneShot = false,
            Autostart = true,
			WaitTime = timeBetweenAttacks
        };
        AddChild(attackTimer);
		attackTimer.Timeout += fire;

		// Set the health
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

        Modulate = baseColor;

		// Make being damaged play the hurt sound
		OnDamage += () => hurtSound.Play();
	}

    public override void _PhysicsProcess(double delta)
    {

		Vector2 directionToPlayer = player.Position - this.Position;
        if (directionToPlayer.Length() > desiredDistanceFromPlayer) {
		    Velocity = directionToPlayer.Normalized() * speed;
        }else if (directionToPlayer.Length() < desiredDistanceFromPlayer - 50) {
            Velocity = directionToPlayer.Normalized() * -speed;
        }else {
            Velocity = new Vector2(0, 0);
        }

		if (directionToPlayer.Length() > 1500) {
			Velocity *= 3;
		}

		MoveAndSlide();
    }

	public abstract void fire();

	public void hitFlashOn() {
		Modulate = hitFlashColor;
		hitFlashTImer.Start();
	}

	public void hitFlashOff() {
		Modulate = baseColor;
	}
}
