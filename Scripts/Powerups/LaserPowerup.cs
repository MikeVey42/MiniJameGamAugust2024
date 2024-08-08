using Godot;
using System;

public partial class LaserPowerup : Powerup
{
    PackedScene laserPrefab;
    BasicAreaAttack laser;
    Boolean attacking;
    public override string GetHint() {
        return "The end is now. Kill everyone";
    }
    public override void OnGain() {
        player.attackDamage+=100;
        laserPrefab = GD.Load<PackedScene>("res://Scenes/Projectiles/Laser.tscn");
        laser = null;
        attacking = false;
    }

    public override void Periodic()
    {
        if (Input.IsActionJustPressed("click")) {
            laser = laserPrefab.Instantiate<BasicAreaAttack>();
            player.AddChild(laser);
            attacking = true;
        }

        if (Input.IsActionJustReleased("click") && attacking) {
            laser.QueueFree();
            attacking = false;
        }
    }
}
