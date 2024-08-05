using Godot;
using System;
using System.Diagnostics;

public partial class BasicEnemy : DamageableEntity
{
	Player player;
	PackedScene swordSlashPrefab;

	private float speed = 200;

	Timer attackTimer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Find the player in the scene
		player = GetParent().GetParent().GetNode<Player>("Player");
		// Load the sword slash prefab
		swordSlashPrefab = GD.Load<PackedScene>("res://Scenes/SwordSlash.tscn");
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
		Death += () => QueueFree();
	}

    public override void _PhysicsProcess(double delta)
    {
        LookAt(player.Position);
		Vector2 directionToPlayer = player.Position - this.Position;
		Velocity = directionToPlayer.Normalized() * speed;
		MoveAndSlide();
    }

	public void fire() {
		if ((player.Position - this.Position).Length() < 150) {
			BasicProjectile swordSlash = (BasicProjectile) swordSlashPrefab.Instantiate();
			GetParent().AddChild(swordSlash);
			swordSlash.Position = this.Position;
			swordSlash.Rotation = this.Rotation;
		}
	}
}
