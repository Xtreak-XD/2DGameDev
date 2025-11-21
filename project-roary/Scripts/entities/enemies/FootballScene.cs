using Godot;
using System;
using System.Collections.Generic;

public partial class FootballScene : Enemy
{

    //Do the door close and change the logic for the middle based on even or odd
    private Viewport viewport;

    float numberToSpawn;

    private Vector2 spawnPosition;
    private enum GapLocation
    {
        SmallLeft,
        SmallRight,
        SmallMiddle,
        WideLeft,
        WideRight,
        WideMiddle,
        ZigZagMove,
    }

    private enum SpawnLocation
    {
        North,
        East,
        South,
        West,
    }

    private List<Node2D> footballPlayers = new List<Node2D>();

    private GapLocation gapChosen { get; set; } = GapLocation.SmallMiddle;
    private SpawnLocation spawnChosen { get; set; } = SpawnLocation.North;

    [Export]
    public float waitTimer;

    public Timer durationTimer;
    private Timer directionTimer;

    private Vector2 direction;

    public override void _Ready()
    {
        PackedScene footballPlayer = GD.Load<PackedScene>("Scenes/entities/enemies/FootballPlayer.tscn");
        FootballPlayer footballPlayerInstance = (FootballPlayer)footballPlayer.Instantiate();
        Sprite2D footballPlayerSprite = footballPlayerInstance.GetNode<Sprite2D>("Sprite2D");

        durationTimer = GetNode<Timer>("StampedeTimer");

        float footballPlayerWidth = footballPlayerSprite.Texture.GetWidth() + 50;
        float footballPlayerHeight = footballPlayerSprite.Texture.GetHeight() + 50;

        Random spawnRandomizer = new();

        int spawnLoc = spawnRandomizer.Next(0, 4);
        int gapLoc = spawnRandomizer.Next(0, 7);

        switch(spawnLoc)
        {
            case 0:
                spawnChosen = SpawnLocation.North;
                break;
            case 1:
                spawnChosen = SpawnLocation.South;
                break;
            case 2:
                spawnChosen = SpawnLocation.East;
                break;
            case 3:
                spawnChosen = SpawnLocation.West;
                break;
        }

        switch(gapLoc)
        {
            case 0:
                gapChosen = GapLocation.SmallLeft;
                break;
            case 1:
                gapChosen = GapLocation.SmallRight;
                break;
            case 2:
                gapChosen = GapLocation.SmallMiddle;
                break;
            case 3:
                gapChosen = GapLocation.WideLeft;
                break;
            case 4:
                gapChosen = GapLocation.WideRight;
                break;
            case 5:
                gapChosen = GapLocation.WideMiddle;
                break;
            case 6:
                gapChosen = GapLocation.ZigZagMove;
                break;
        }

        viewport = GetViewport();
        Rect2 visibleRect = viewport.GetVisibleRect();
        Vector2 viewportSize = visibleRect.Size;
        float viewportWidth = viewportSize.X;
        float viewportHeight = viewportSize.Y;
        footballPlayerWidth /= 2;
        footballPlayerHeight /= 2;

        if (spawnChosen == SpawnLocation.North || spawnChosen == SpawnLocation.South)
        {
            numberToSpawn = viewportWidth / footballPlayerWidth;
        }
        else
        {
            numberToSpawn = viewportHeight / footballPlayerHeight;
        }

        numberToSpawn = (int)numberToSpawn;

        GD.Print($"Football players to spawn: {numberToSpawn}");
        GD.Print($"Spawn Direction: {spawnChosen}");
        GD.Print($"Gap Position: {gapChosen}");

        switch (spawnChosen)
        {
            case SpawnLocation.North:
                for (int i = 0; i < numberToSpawn; i++)
                {
                    spawnPosition = new Vector2(footballPlayerWidth * i, 0);
                    FootballPlayer footballPlayerDup = (FootballPlayer)footballPlayerInstance.Duplicate(7);
                    footballPlayerDup.Name = $"footballPlayer{i + 1}";
                    footballPlayerDup.GlobalPosition = spawnPosition;
                    AddChild(footballPlayerDup);
                    footballPlayers.Add(footballPlayerDup);
                }
                break;

            case SpawnLocation.East:
                for (int i = 0; i < numberToSpawn; i++)
                {
                    spawnPosition = new Vector2(viewportWidth, 0 + (footballPlayerHeight * (i + 1)));
                    FootballPlayer footballPlayerDup = (FootballPlayer)footballPlayerInstance.Duplicate(7);
                    footballPlayerDup.Name = $"footballPlayer{i + 1}";
                    footballPlayerDup.GlobalPosition = spawnPosition;
                    AddChild(footballPlayerDup);
                    footballPlayers.Add(footballPlayerDup);
                }
                break;

            case SpawnLocation.South:
                for (int i = 0; i < numberToSpawn; i++)
                {
                    spawnPosition = new Vector2(footballPlayerWidth * i, viewportHeight);
                    FootballPlayer footballPlayerDup = (FootballPlayer)footballPlayerInstance.Duplicate(7);
                    footballPlayerDup.Name = $"footballPlayer{i + 1}";
                    footballPlayerDup.GlobalPosition = spawnPosition;
                    AddChild(footballPlayerDup);
                    footballPlayers.Add(footballPlayerDup);
                }
                break;

            case SpawnLocation.West:
                for (int i = 0; i < numberToSpawn; i++)
                {
                    spawnPosition = new Vector2(0, 0 + (footballPlayerHeight * (i + 1)));
                    FootballPlayer footballPlayerDup = (FootballPlayer)footballPlayerInstance.Duplicate(7);
                    footballPlayerDup.Name = $"footballPlayer{i + 1}";
                    footballPlayerDup.GlobalPosition = spawnPosition;
                    AddChild(footballPlayerDup);
                    footballPlayers.Add(footballPlayerDup);
                }
                break;
        }
        
        directionTimer = new Timer();
        directionTimer.WaitTime = waitTimer;
        directionTimer.OneShot = false; // repeat forever
        AddChild(directionTimer);

        directionTimer.Timeout += OnDirectionTimerTimeout;
        directionTimer.Start();

        if(spawnChosen == SpawnLocation.North || spawnChosen == SpawnLocation.South)
        {
            durationTimer.WaitTime = 4.3;
        } 
        else
        {
            durationTimer.WaitTime = 7;
        }

        durationTimer.Timeout += QueueFree;
        durationTimer.Start();
    }

    public override void _Process(double delta)
    {
        switch (spawnChosen)
        {
            case SpawnLocation.North:
                Velocity = Vector2.Down * data.Speed;
                break;
            case SpawnLocation.South:
                Velocity = Vector2.Up * data.Speed;
                break;
            case SpawnLocation.East:
                Velocity = Vector2.Left * data.Speed;
                break;
            case SpawnLocation.West:
                Velocity = Vector2.Right * data.Speed;
                break;
        }
        
        switch (gapChosen)
        {
            case GapLocation.SmallLeft:
                footballPlayers[0].Hide();
                break;
            case GapLocation.SmallRight:
                footballPlayers[footballPlayers.Count - 1].Hide();
                break;
            case GapLocation.SmallMiddle:
                int count = footballPlayers.Count;
                int mid = count / 2;
                if(footballPlayers.Count % 2 == 1)
                {
                    footballPlayers[mid].Hide();
                } 
                else
                {
                    footballPlayers[mid - 1]?.Hide();
                    footballPlayers[mid]?.Hide();
                }
                break;
            case GapLocation.WideLeft:
                footballPlayers[0].Hide();
                footballPlayers[1].Hide();
                break;
            case GapLocation.WideRight:
                footballPlayers[footballPlayers.Count - 1].Hide();
                footballPlayers[footballPlayers.Count - 2].Hide();
                break;
            case GapLocation.WideMiddle:
                int count2 = footballPlayers.Count;
                int mid2 = count2 / 2;
                if(count2 % 2 == 1)
                {
                    footballPlayers[mid2].Hide();
                    footballPlayers[mid2 - 1].Hide();
                }
                else
                {
                    footballPlayers[mid2].Hide();
                    footballPlayers[mid2 - 1].Hide();
                    footballPlayers[mid2 + 1].Hide();
                }
                break;
            case GapLocation.ZigZagMove:
                for (int i = 0; i < footballPlayers.Count; i++)
                {
                    if (i % 2 == 0)
                    {
                        footballPlayers[i].Hide();
                    }
                }
                if (spawnChosen == SpawnLocation.North)
                {
                    Vector2 velocity = Velocity;
                    velocity = direction * data.Speed * 2;
                    velocity.Y = data.Speed;
                    Velocity = velocity;
                } 
                else if(spawnChosen == SpawnLocation.South)
                {
                    Vector2 velocity = Velocity;
                    velocity = direction * data.Speed;
                    velocity.Y = -data.Speed;; 
                    Velocity = velocity;
                } 
                else if(spawnChosen == SpawnLocation.East)
                {
                    Vector2 velocity = Velocity;
                    velocity = direction * data.Speed * 2;
                    velocity.X = -data.Speed;
                    Velocity = velocity;
                }
                else
                {
                    Vector2 velocity = Velocity;
                    velocity = direction * data.Speed * 2;
                    velocity.X = data.Speed;
                    Velocity = velocity;
                }
                break;
        }
    }
    
    private void OnDirectionTimerTimeout()
    {
        if(spawnChosen == SpawnLocation.North || spawnChosen == SpawnLocation.South)
        {
            if (direction == Vector2.Left)
            {
                direction = Vector2.Right;
            }
            else
            {
                direction = Vector2.Left;
            }
        }
        else
        {
            if (direction == Vector2.Up)
            {
                direction = Vector2.Down;
            }
            else
            {
                direction = Vector2.Up;
            }
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        MoveAndSlide();
    }
}
