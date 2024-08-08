using Godot;
using System;
using System.Diagnostics;

public partial class EnemyManager : Node2D
{
	private Player player;
	private Timer spawnTimer;

	private PackedScene swordEnemyPrefab, bowEnemyPrefab;

	private float spawnDistance = 1500;

	bool endgame;
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
			WaitTime = 4
		};
		AddChild(spawnTimer);
		spawnTimer.Timeout += () => SpawnEnemy(swordEnemyPrefab);
		spawnTimer.Timeout += () => SpawnEnemy(bowEnemyPrefab);

		SpawnEnemy(swordEnemyPrefab);

		endgame = false;
	}

	public void SpawnEnemy(PackedScene prefab) {
		float angle = (float) GD.RandRange(0, Math.PI * 2);
		Vector2 spawnPoint = player.Position + Vector2.FromAngle(angle) * spawnDistance;
		Node2D enemy = (Node2D) prefab.Instantiate();
		enemy.Position = spawnPoint;
		CallDeferred(Node.MethodName.AddChild, enemy);
	}

	public PackedScene LoadEnemy(String name) {
		return GD.Load<PackedScene>("res://Scenes/Enemies/"+ name + ".tscn");
	}

	public void Swarm() {
		for (int i = 0; i < 30; i++) {
			SpawnEnemy(swordEnemyPrefab);
			SpawnEnemy(bowEnemyPrefab);
		}
		spawnTimer.Stop();

		endgame = true;
	}

    public override void _Process(double delta)
    {
        if (!endgame) {
			return;
		}

		Debug.Print(GetChildCount() +"");
		if (GetChildCount() == 1) {
			GetTree().ChangeSceneToFile("res://Scenes/WinScreen.tscn");
		}
    }
}
