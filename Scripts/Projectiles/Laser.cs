using Godot;
using System;

public partial class Laser : BasicAreaAttack
{
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		LookAt(GetGlobalMousePosition());
	}
}
