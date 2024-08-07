using Godot;
using System;

public partial class DamageUpPowerup : Powerup
{
    public override void OnGain() {
        player.attackDamage+= 100;
    }
}
