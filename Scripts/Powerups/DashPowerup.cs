using Godot;
using System;
using System.Diagnostics;

public partial class DashPowerup : Powerup
{
    Timer dashTimer, coolDownTimer;
    bool dashing, onCooldown;

    public override string GetHint() {
        return "Press space to dash";
    }

    public override Texture2D GetIcon()
    {
        return LoadIcon("Wing");
    }

    public override void OnGain()
    {
        dashTimer = new Timer{
            OneShot = true,
            Autostart = false,
            WaitTime = 0.75
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

        if (dashing && !player.dashSound.Playing) {
            player.dashSound.Play();
        }
    }

    public void StartDash() {
        dashing = true;
        player.moveSpeed *= 3.5f;
        player.turnSpeed *= 3.5f;
        dashTimer.Start();
    }
    public void EndDash() {
        dashing = false;
        player.moveSpeed /= 3.5f;
        player.turnSpeed /= 3.5f;
        onCooldown = true;
        coolDownTimer.Start();
        player.dashSound.Stop();
    }
}
