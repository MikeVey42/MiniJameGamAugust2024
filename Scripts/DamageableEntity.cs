using Godot;
using System;
using System.Diagnostics;

public partial class DamageableEntity : CharacterBody2D
{

    [Export]
    public int maxHealth;
    protected int health;
    // Signal that is released when this object dies
    [Signal]
    public delegate void OnDeathEventHandler();
    [Signal]
    public delegate void OnDamageEventHandler();

    public override void _Ready()
    {
        health = maxHealth;
    }

    // Subtracts the appropriate amount of health from the entity, cuasing it to die if it's health reaches 0
    public void Damage(int amount) {
        if (health <= 0) {
            return;
        }
        health -= amount;
        // Broadcast that this entity has died
        if (health <= 0) {
            EmitSignal(SignalName.OnDeath);
        }else {
            EmitSignal(SignalName.OnDamage);
        }
    }

    // Adds the appropriate amount of health to the entity, but no more than it's max health  
    public void Heal(int amount) {
        health += amount;
        if (health > maxHealth) {
            health = maxHealth;
        }
    }

    // Returns the current health of the entity
    public int GetHealth() {
        return health;
    }
}
