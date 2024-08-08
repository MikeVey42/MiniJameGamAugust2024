using Godot;
using System;

public partial class PowerupTracker : Sprite2D
{
	PowerupContainer powerupContainer;
	Player player;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		player = GetParent<Player>();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (!IsInstanceValid(powerupContainer)) {
			Visible = false;
			return;
		}else {
			Visible = true;
		}

		Vector2 directionToPowerup = powerupContainer.Position - player.Position;
		Position = Vector2.FromAngle(directionToPowerup.Angle() - player.Rotation) * 100;
	}

	public void Track(PowerupContainer target) {
		powerupContainer = target;
	}
}
