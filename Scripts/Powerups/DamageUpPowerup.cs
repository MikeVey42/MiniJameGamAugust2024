using Godot;
using System;

public partial class DamageUpPowerup : Powerup
{

    public override string GetHint() {
        return "Bite power increased";
    }
    public override void OnGain() {
        player.attackDamage+=100;
    }
}
