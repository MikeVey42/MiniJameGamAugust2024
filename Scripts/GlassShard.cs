using Godot;
using System;

public partial class GlassShard : Area2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		BodyEntered += Collect;
	}

	public void Collect(Node2D body) {
		if (body is Player) {
			Player player = (Player) body;
			player.glassShards++;
			player.powerupGainedSound.Play();
			QueueFree();
		}
	}
}
