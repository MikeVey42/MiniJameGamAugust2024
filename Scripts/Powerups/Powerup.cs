using Godot;
using System;

public abstract partial class Powerup : Resource
{
	protected Player player;
	// Called when the node enters the scene tree for the first time.
	public void Connect(Player player) {
		this.player = player;
	}

	public virtual void OnGain() {}

	public virtual void Periodic() {}

	public abstract string GetHint();
}