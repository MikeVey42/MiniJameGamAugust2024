using Godot;
using System;

public partial class Camera : Camera2D
{
	Player player;
	// Amount that the camera is allowed to drift from the player in the x and y directions
	float xMargin = 0;
	float yMargin = 0;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		player = GetParent().GetNode<Player>("Player");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		float xOffset = player.Position.X - this.Position.X;
		float yOffset = player.Position.Y - this.Position.Y;

		if (xOffset > xMargin) {
			this.Position += new Vector2(xOffset - xMargin, 0);
		}else if (xOffset < -xMargin) {
			this.Position += new Vector2(xOffset + xMargin, 0);
		}

		if (yOffset > yMargin) {
			this.Position += new Vector2(0, yOffset - yMargin);
		}else if (yOffset < -yMargin) {
			this.Position += new Vector2(0, yOffset + yMargin);
		}
	}
}
