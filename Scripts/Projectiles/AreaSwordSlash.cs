using Godot;
using System;

public partial class AreaSwordSlash : BasicAreaAttack
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		damageAmount = 1;
		lifespan = 0.3f;
		base._Ready();
		Rotation += (1f/3f) * (float) Math.PI;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		Rotation -= (float) delta * (float) Math.PI * (2f/3f) * (1/lifespan);
	}
}
