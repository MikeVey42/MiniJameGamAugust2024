using Godot;
using System;
using System.Diagnostics;

public partial class Player : DamageableEntity
{

    int speed = 400;  // move speed in pixels/sec

    public override void _Ready()
    {
        maxHealth = 5;
        base._Ready();
        this.Death += DeathMessage;
    }

    public override void _PhysicsProcess(double delta)
    {
        LookAt(GetGlobalMousePosition());
        float yVelocity = Input.GetAxis("up", "down");
        float xVelocity = Input.GetAxis("left", "right");
        Velocity = new Vector2(xVelocity, yVelocity) * speed;
        MoveAndSlide();
    }

    public void DeathMessage() {
        Debug.Print("You have died");
    }
}
