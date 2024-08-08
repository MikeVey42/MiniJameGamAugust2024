using Godot;
using System;

public partial class DamageUpPowerup : Powerup
{

    public override string GetHint() {
        return "Bite power increased";
    }

    public override Texture2D GetIcon()
    {
        return LoadIcon("Explosion");
    }

    public override void OnGain() {
        player.attackDamage+=6;
    }
}
