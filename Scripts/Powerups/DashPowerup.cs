using Godot;
using System;
using System.Diagnostics;

public partial class DashPowerup : Powerup
{
    Timer dashTimer, coolDownTimer;
    bool dashing, onCooldown;

    public override void OnGain()
    {
        dashTimer = new Timer{
            OneShot = true,
            Autostart = false,
            WaitTime = 1
        };
        coolDownTimer = new Timer{
            OneShot = true,
            Autostart = false,
            WaitTime = 10
        };

        player.AddChild(dashTimer);
        player.AddChild(coolDownTimer);

        dashing = false;
        onCooldown = false;

        dashTimer.Timeout += EndDash;
        coolDownTimer.Timeout += () => onCooldown = false;
    }
    public override void Periodic()
    {
        // If the player presses space and isn't dashing or on cool down, multiply their speed
        if (Input.IsActionJustPressed("dash")) {
            if (!dashing && !onCooldown) {
                StartDash();
            }
        }
    }

    public void StartDash() {
        dashing = true;
        player.moveSpeed *= 3;
        player.turnSpeed *= 3;
        dashTimer.Start();
    }
    public void EndDash() {
        dashing = false;
        player.moveSpeed /= 3;
        player.turnSpeed /= 3;
        onCooldown = true;
        coolDownTimer.Start();
    }
}
