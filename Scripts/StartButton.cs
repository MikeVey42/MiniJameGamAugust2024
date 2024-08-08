using Godot;
using System;

public partial class StartButton : TextureButton
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ButtonDown += () => GetTree().ChangeSceneToFile("res://Scenes/Objectives.tscn");
	}
}
