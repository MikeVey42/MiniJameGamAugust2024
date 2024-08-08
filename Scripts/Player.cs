using Godot;
using Godot.Collections;
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

public partial class Player : DamageableEntity
{

    [Export]
    public float moveSpeed = 400;  // move speed in pixels/sec
    [Export]
    public float turnSpeed = 0.01f;

    private PackedScene chompPrefab;

    private ProgressBar healthBar;

    public int attackDamage = 6;
    public float attackSize = 1;

    Array<Powerup> powerups;

    [Export] PowerupPopup powerupPopup;

    public int glassShards = 0;

    [Export] private AudioStreamPlayer2D biteSound;
    [Export] public AudioStreamPlayer2D powerupGainedSound;
    [Export] public AudioStreamPlayer2D dashSound;
    // Animation
    [Export] private AnimatedSprite2D sprite;

    private Timer attackCooldownTimer;
    private Timer attackDelayTimer;

    bool laserEnabled;

    public override void _Ready()
    {
        maxHealth = 10;
        base._Ready();
        OnDeath += DeathMessage;

        chompPrefab = GD.Load<PackedScene>("res://Scenes/Projectiles/Chomp.tscn");

        healthBar = GetParent().GetNode("UI").GetNode<ProgressBar>("HealthBar");
        OnDamage += () => healthBar.Value = (float) health * 100 / maxHealth;

        powerups = new Array<Powerup>();

        glassShards = 0;

        attackCooldownTimer = new Timer {
            OneShot = true,
            Autostart = false,
            WaitTime = 0.35
        };

        attackDelayTimer = new Timer {
            OneShot = true,
            Autostart = false,
            WaitTime = 0.1
        };

        AddChild(attackCooldownTimer);
        AddChild(attackDelayTimer);

        attackDelayTimer.Timeout += Bite;

        laserEnabled = false;
    }

    public override void _PhysicsProcess(double delta)
    {
        // Run each powerup
        foreach (Powerup powerup in powerups) {powerup.Periodic();}

        // Move the player linearly according to inputs
        float yVelocity = Input.GetAxis("up", "down");
        float xVelocity = Input.GetAxis("left", "right");
        // If the player is attacking, don't move
        if (!IsAttacking()) {
            Velocity = new Vector2(xVelocity, yVelocity) * moveSpeed;
            WalkingAnimation();
        }else if (attackDelayTimer.TimeLeft != 0) {
            Velocity = Vector2.FromAngle(Rotation) * moveSpeed * 2;
        } else {
            Velocity = Vector2.Zero;
        }

        MoveAndSlide();

        FaceTowardsPlayer(turnSpeed);

        // Make the player lunge and attack
        if (Input.IsActionJustPressed("click") && !IsAttacking() && !laserEnabled) {
            attackDelayTimer.Start();
            // Play the bite animation
            sprite.Play("bite");
        }
    }

    public void DeathMessage() {
        GetTree().CallDeferred(SceneTree.MethodName.ChangeSceneToFile, "res://Scenes/DeathScreen.tscn");
    }

    public void FaceTowardsPlayer(float speed) {
        // If the player is attacking, don't move
        if (IsAttacking()) {
            return;
        }

        // If the player is moving, turn slower
        if (Velocity.Length() >= 1) {
            speed /= 2;
        }

        // If the player has the laser, turn faster
        if (laserEnabled) {
            speed *= 1.5f;
        }

        // Turn the player to face towards the mouse
        float desiredAngle = (GetGlobalMousePosition() - this.Position).Angle();
        float error = desiredAngle - Rotation;
        if (Math.Abs(error) < speed) {
            // If the player is almost facing the right way, snap them to it
            Rotation = desiredAngle;
            if (Math.Abs(error) < 0.003 && Velocity.Length() < 1) {
                sprite.Play("idle");
            }
        }else if (error > 0 && error < Math.PI || error < -Math.PI) { // Check which direction to turn to
            Rotation += speed;
            if (Velocity.Length() < 1) {
                sprite.Play("turnRight");
            }
        }else {
            Rotation -= speed;
            if (Velocity.Length() < 1) {
                sprite.Play("turnLeft");
            }
        }
    }

    public void ApplyPowerup(Powerup powerup) {
        if (powerup is LaserPowerup) {
            laserEnabled = true;
        }

        powerup.Connect(this);
        powerup.OnGain();
        powerups.Add(powerup);
        powerupPopup.ShowHint(powerup.GetHint());
        powerupGainedSound.Play();
    }

    // Handle the animation for walking linearly
    public void WalkingAnimation() {
        // If idle, we aren't moving, don't do anything
        if (Velocity.Length() < 1) {
            return;
        }

        float strafeAngle = (Velocity.Angle() * 180 / (float) Math.PI) - RotationDegrees;
        bool forwards = (strafeAngle < 45 && strafeAngle > -45) || strafeAngle > 315 || strafeAngle < -315;
        bool backwards = (strafeAngle > 135 && strafeAngle < 225) || (strafeAngle < -135 && strafeAngle > -225);
        if (!backwards) {
            sprite.Play("walk");
        }else if (backwards) {
            sprite.PlayBackwards("walk");
        }else {
            sprite.Play("idle");
        }
    }

    private bool IsAttacking() {
        return !(attackCooldownTimer.TimeLeft == 0 && attackDelayTimer.TimeLeft == 0);
    }

    // Make the player bite
    private void Bite() {
        AnimatedAreaAttack chomp = (AnimatedAreaAttack) chompPrefab.Instantiate();
        chomp.Position = new Vector2(170, 0);
        chomp.damageAmount = attackDamage;
        chomp.Scale *= attackSize;
        AddChild(chomp);
        biteSound.PitchScale = (float) GD.RandRange(0.9, 1.1);
        biteSound.Play();

        attackCooldownTimer.Start();
    }
}
