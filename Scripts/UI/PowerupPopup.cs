using Godot;
using System;

public partial class PowerupPopup : RichTextLabel
{
	Timer durationTimer;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		durationTimer = new Timer{
			OneShot = true,
			Autostart = false,
			WaitTime = 5
		};

		durationTimer.Timeout += () => this.Visible = false;

		AddChild(durationTimer);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void ShowHint(string text) {
		this.Visible = true;
		this.Text = "[center]" + text + "[/center]";
		durationTimer.Start();
	}
}
