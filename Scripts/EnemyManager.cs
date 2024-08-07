using Godot;
using System;

public partial class EnemyManager : Node2D
{
	private Player player;
	private Timer spawnTimer;

	private PackedScene swordEnemyPrefab, bowEnemyPrefab;

	private float spawnDistance = 1500;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Find the player in the scene
		player = GetParent().GetNode<Player>("Player");

		// Load the enemy prefabs
		swordEnemyPrefab = LoadEnemy("SwordEnemy");
		bowEnemyPrefab = LoadEnemy("BowEnemy");
		

		// Start the spawn timer
		spawnTimer = new Timer() {
			OneShot = false,
			Autostart = true,
			WaitTime = 3
		};
		AddChild(spawnTimer);
		spawnTimer.Timeout += () => SpawnEnemy(swordEnemyPrefab);
		spawnTimer.Timeout += () => SpawnEnemy(bowEnemyPrefab);

		SpawnEnemy(swordEnemyPrefab);
	}

	public void SpawnEnemy(PackedScene prefab) {
		float angle = (float) GD.RandRange(0, Math.PI * 2);
		Vector2 spawnPoint = player.Position + Vector2.FromAngle(angle) * spawnDistance;
		Node2D enemy = (Node2D) prefab.Instantiate();
		enemy.Position = spawnPoint;
		AddChild(enemy);
	}

	public PackedScene LoadEnemy(String name) {
		return GD.Load<PackedScene>("res://Scenes/Enemies/"+ name + ".tscn");
	}
}
