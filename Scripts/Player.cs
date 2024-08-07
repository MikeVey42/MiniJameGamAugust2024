using Godot;
using Godot.Collections;
using System;
using System.Diagnostics;
using System.Linq;

public partial class Player : DamageableEntity
{

    [Export]
    public float moveSpeed = 400;  // move speed in pixels/sec
    [Export]
    public float turnSpeed = 0.01f;

    private PackedScene chompPrefab;

    private ProgressBar healthBar;

    public int attackDamage = 1;

    Array<Powerup> powerups;

    public override void _Ready()
    {
        maxHealth = 5;
        base._Ready();
        OnDeath += DeathMessage;

        chompPrefab = GD.Load<PackedScene>("res://Scenes/Projectiles/Chomp.tscn");

        healthBar = GetParent().GetNode("UI").GetNode<ProgressBar>("HealthBar");
        OnDamage += () => healthBar.Value = (float) health * 100 / maxHealth;

        powerups = new Array<Powerup>();
    }

    public override void _PhysicsProcess(double delta)
    {
        // Run each powerup
        foreach (Powerup powerup in powerups) {powerup.Periodic();}

        // Move the player linearly according to inputs
        float yVelocity = Input.GetAxis("up", "down");
        float xVelocity = Input.GetAxis("left", "right");
        Velocity = new Vector2(xVelocity, yVelocity) * moveSpeed;
        MoveAndSlide();

        FaceTowardsPlayer(turnSpeed);

        // Make the player attack
        if (Input.IsActionJustPressed("click")) {
            AnimatedAreaAttack chomp = (AnimatedAreaAttack) chompPrefab.Instantiate();
            chomp.Position = new Vector2(120, 0);
            chomp.damageAmount = attackDamage;
            AddChild(chomp);
        }
    }

    public void DeathMessage() {
        GetTree().ChangeSceneToFile("res://Scenes/DeathScreen.tscn");
    }

    public void FaceTowardsPlayer(float speed) {
        // Turn the player to face towards the mouse
        float desiredAngle = (GetGlobalMousePosition() - this.Position).Angle();
        float error = desiredAngle - Rotation;
        if (Math.Abs(error) < speed) {
            // If the player is almost facing the right way, snap them to it
            Rotation = desiredAngle;
        }else if (error > 0 && error < Math.PI || error < -Math.PI) { // Check which direction to turn to
            Rotation += speed;
        }else {
            Rotation -= speed;
        }
    }

    public void ApplyPowerup(Powerup powerup) {
        powerup.Connect(this);
        powerup.OnGain();
        powerups.Add(powerup);
    }
}
