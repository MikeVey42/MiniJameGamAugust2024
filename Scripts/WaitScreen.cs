using Godot;
using System;

public partial class WaitScreen : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Timer waitTimer = new Timer {
			OneShot = true,
			Autostart = true,
			WaitTime = 7.5
		};
		waitTimer.Timeout += () => GetTree().ChangeSceneToFile("res://Scenes/MainScene.tscn");
		AddChild(waitTimer);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
