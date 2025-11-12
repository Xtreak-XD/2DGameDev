using Godot;
using System;

public partial class EnemyManager : Node
{
    [Export] public float EnemySpawnTimerMin = 0.1f;
    [Export] public float EnemySpawnTimerMax = 4.0f;
    [Export] public Marker2D[] EnemySpawnPoints;
    [Export] public PackedScene EnemyScene;
    [Export] public int MaxEnemiesAlive = 4;
    private Timer EnemySpawnTimer;
    private int EnemyCount = 0;


    public override void _Ready()
    {
        Eventbus.OnEnemyDeath += OnEnemyDeath;
        
        EnemySpawnTimer = GetNode<Timer>("EnemySpawnTimer");
        if (EnemySpawnTimer != null)
            EnemySpawnTimer.Timeout += OnEnemySpawn;
            EnemySpawnTimer.Start(EnemySpawnTimerMin);
    }

    private void OnEnemySpawn()
    {
        if (EnemyCount >= MaxEnemiesAlive || EnemyScene == null || EnemySpawnPoints.Length == 0) return;

        GD.Print("Spawning an enemy!");
        Node2D newEnemy = (Node2D)EnemyScene.Instantiate();

        Node2D entityContainer = GetNodeOrNull<Node2D>("EntityContainer");
        if (entityContainer == null) return;

        var random = new RandomNumberGenerator();
        random.Randomize();
        int index = random.RandiRange(0, EnemySpawnPoints.Length - 1);
        Marker2D chosenSpawnPoint = EnemySpawnPoints[index];

        entityContainer.AddChild(newEnemy);
        newEnemy.Position = chosenSpawnPoint.Position;

        EnemyCount = Math.Min(EnemyCount + 1, MaxEnemiesAlive);

        if (EnemyCount < MaxEnemiesAlive)
        {
            float randomTime = random.RandfRange(EnemySpawnTimerMin, EnemySpawnTimerMax);
            EnemySpawnTimer.Start(randomTime);
        }
    }
    private void OnEnemyDeath(Enemy enemy) 
    {
        EnemyCount--;
    }
}