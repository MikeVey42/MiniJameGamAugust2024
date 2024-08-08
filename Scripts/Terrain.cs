using Godot;
using System;

public partial class Terrain : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Sprite2D sprite = GetChild<Sprite2D>(0);
		sprite.Frame = GD.RandRange(0, 7);
		if (GD.RandRange(0, 2) < 2) {
			sprite.Frame = GD.RandRange(4,5);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
