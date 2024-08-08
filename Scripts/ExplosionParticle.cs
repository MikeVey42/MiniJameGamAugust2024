using Godot;
using System;

public partial class ExplosionParticle : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AnimatedSprite2D sprite = GetChild<AnimatedSprite2D>(0);
		sprite.Play();
		sprite.AnimationLooped += () => QueueFree();
	}
}