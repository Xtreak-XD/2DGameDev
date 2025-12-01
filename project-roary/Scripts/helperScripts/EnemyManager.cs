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
    private RandomNumberGenerator random;
    public Node2D entityContainer;
    public string carEnemy = "res://Scenes/entities/enemies/CarEnemy.tscn";

    public override void _Ready()
    {
        Eventbus.OnEnemyDeath += OnEnemyDeath;

        random = new RandomNumberGenerator();
        random.Randomize();

        entityContainer = GetNodeOrNull<Node2D>("EntityContainer");
        
        EnemySpawnTimer = GetNode<Timer>("EnemySpawnTimer");
        if (EnemySpawnTimer != null)
            EnemySpawnTimer.Timeout += OnEnemySpawn;
            EnemySpawnTimer.Start(EnemySpawnTimerMin);
    }

    private int GetCurrentEnemyCount()
    {
        if (entityContainer == null)
            return 0;
        
        return entityContainer.GetChildCount();
    }
    private void OnEnemySpawn()
    {
        int currentEnemies = GetCurrentEnemyCount();
        if (currentEnemies >= MaxEnemiesAlive || EnemyScene == null || EnemySpawnPoints.Length == 0){
            
            float randomTime = random.RandfRange(EnemySpawnTimerMin, EnemySpawnTimerMax);
            EnemySpawnTimer.Start(randomTime);
            return;
        }

        if (entityContainer == null)
        {
            float randomTime = random.RandfRange(EnemySpawnTimerMin, EnemySpawnTimerMax);
            EnemySpawnTimer.Start(randomTime);
            return;
        }

        int index = random.RandiRange(0, EnemySpawnPoints.Length - 1);
        Marker2D chosenSpawnPoint = EnemySpawnPoints[index];
        Node2D newEnemy = (Node2D)EnemyScene.Instantiate();

        if (EnemyScene.ResourcePath == carEnemy && newEnemy is CarEnemy carEnemyInstance)
        {
            carEnemyInstance.currentDirection = DetermineDirectionFromSpawnPoint(chosenSpawnPoint.GlobalPosition);
        }

        entityContainer.AddChild(newEnemy);
        newEnemy.Position = chosenSpawnPoint.GlobalPosition;

        float nextSpawnTime = random.RandfRange(EnemySpawnTimerMin, EnemySpawnTimerMax);
        EnemySpawnTimer.Start(nextSpawnTime);
    }

    private CarEnemy.directionChosen DetermineDirectionFromSpawnPoint(Vector2 spawnPos)
    {
        float sceneWidth = 10000f;
        float sceneHeight = 10000f;

        // Calculate relative position (0 to 1)
        float relativeX = spawnPos.X / sceneWidth;
        float relativeY = spawnPos.Y / sceneHeight;

        if (relativeX < 0.1f) // Left
            return CarEnemy.directionChosen.East;
        else if (relativeX > 0.9f) // Right
            return CarEnemy.directionChosen.West;
        else if (relativeY < 0.1f) // Top 
            return CarEnemy.directionChosen.South;
        else if (relativeY > 0.9f) // Bottom
            return CarEnemy.directionChosen.North;
        
        return CarEnemy.directionChosen.North;
    }
    private void OnEnemyDeath(Enemy enemy) 
    {
        if (!EnemySpawnTimer.IsStopped())
            return;

        float randomTime = random.RandfRange(EnemySpawnTimerMin, EnemySpawnTimerMax);
        EnemySpawnTimer.Start(randomTime);
    }

    public override void _ExitTree()
    {
        Eventbus.OnEnemyDeath -= OnEnemyDeath;
    }
}