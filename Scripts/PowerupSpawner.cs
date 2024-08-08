using Godot;
using Godot.Collections;
using System;
using System.Diagnostics;
using System.Linq;

public partial class PowerupSpawner : Node
{
	PackedScene powerupContainerPrefab, glassShardPrefab;
	Player player;

	Timer glassShardSpawnTimer;

	private static Func<Powerup>[] multipleUsePowerups = {
		() => new DamageUpPowerup()
	};

	private Array<Powerup> singleUsePowerups = new Array<Powerup> {
		new DashPowerup()
	};

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		powerupContainerPrefab = GD.Load<PackedScene>("res://Scenes/PowerupContainer.tscn");
		glassShardPrefab = GD.Load<PackedScene>("res://Scenes/GlassShard.tscn");
		player = GetParent().GetNode<Player>("Player");
		SpawnPowerupContainer();

		glassShardSpawnTimer = new Timer {
			OneShot = false,
			Autostart = true,
			WaitTime = 0.5
		};
		AddChild(glassShardSpawnTimer);
		glassShardSpawnTimer.Timeout += SpawnGlassShard;
	}

	public void SpawnPowerupContainer() {
		// Generate a random angle
		float angle = (float) GD.RandRange(0, Math.PI * 2);
		// Generate a random distance
		float distance = (float) GD.RandRange(3000f, 5000f);
		// Use the above two variables to choose a spawn position around the player
		Vector2 spawnPoint = player.Position + Vector2.FromAngle(angle) * distance;

		PowerupContainer powerupContainer = powerupContainerPrefab.Instantiate<PowerupContainer>();
		powerupContainer.Position = spawnPoint;
		powerupContainer.storedPowerup = PickRandomPowerup();
		CallDeferred(Node.MethodName.AddChild, powerupContainer);

		// Set the powerup tracker to point towards the new powerup container
		player.GetNode<PowerupTracker>("PowerupTracker").Track(powerupContainer);
	}

	public Powerup PickRandomPowerup() {
		int index = GD.RandRange(0, multipleUsePowerups.Length + singleUsePowerups.Count - 1);
		if (index < multipleUsePowerups.Length) {
			Debug.Print("Multi use powerup granted");
			return multipleUsePowerups[index]();
		}else {
			Powerup powerup = singleUsePowerups[index - multipleUsePowerups.Length];
			singleUsePowerups.RemoveAt(index - multipleUsePowerups.Length);
			return powerup;
		}
	}

	public void SpawnGlassShard() {
		// Generate a random angle
		float angle = (float) GD.RandRange(0, Math.PI * 2);
		// Generate a random distance
		float distance = (float) GD.RandRange(2000, 4000);
		// Use the above two variables to choose a spawn position around the player
		Vector2 spawnPoint = player.Position + Vector2.FromAngle(angle) * distance;

		// Spawn a glass shard
		GlassShard glassShard = glassShardPrefab.Instantiate<GlassShard>();
		glassShard.Position = spawnPoint;
		CallDeferred(Node.MethodName.AddChild, glassShard);

	}
}