using Godot;
using System;
using System.Diagnostics;

public partial class Player : DamageableEntity
{

    [Export]
    private float moveSpeed = 400;  // move speed in pixels/sec
    [Export]
    private float turnSpeed = 0.01f;

    private PackedScene chompPrefab;

    private ProgressBar healthBar;

    public override void _Ready()
    {
        maxHealth = 5;
        base._Ready();
        OnDeath += DeathMessage;

        chompPrefab = GD.Load<PackedScene>("res://Scenes/Chomp.tscn");

        healthBar = GetParent().GetNode("UI").GetNode<ProgressBar>("HealthBar");
        OnDamage += () => healthBar.Value = (float) health * 100 / maxHealth;
    }

    public override void _PhysicsProcess(double delta)
    {
        // Move the player linearly according to inputs
        float yVelocity = Input.GetAxis("up", "down");
        float xVelocity = Input.GetAxis("left", "right");
        Velocity = new Vector2(xVelocity, yVelocity) * moveSpeed;
        MoveAndSlide();

        // Turn the player to face towards the mouse
        float desiredAngle = (GetGlobalMousePosition() - this.Position).Angle();
        float error = desiredAngle - Rotation;
        if (Math.Abs(error) < turnSpeed) {
            // If the player is almost facing the right way, snap them to it
            Rotation = desiredAngle;
        }else if (error > 0 && error < Math.PI || error < -Math.PI) { // Check which direction to turn to
            Rotation += turnSpeed;
        }else {
            Rotation -= turnSpeed;
        }

        // Make the player attack
        if (Input.IsActionJustPressed("click")) {
            Node2D chomp = (Node2D) chompPrefab.Instantiate();
            chomp.Position = new Vector2(120, 0);
            AddChild(chomp);
        }
    }

    public void DeathMessage() {
        GetTree().ChangeSceneToFile("res://Scenes/DeathScreen.tscn");
    }
}
