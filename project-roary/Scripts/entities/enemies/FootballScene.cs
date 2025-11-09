using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;

public partial class FootballScene : Enemy
{

    //Do the door close and change the logic for the middle based on even or odd
    private Viewport viewport;

    float numberToSpawn;

    private Vector2 spawnPosition;
    private enum gapLocation
    {
        SmallLeft,
        SmallRight,
        SmallMiddle,
        WideLeft,
        WideRight,
        WideMiddle,
        ZigZagMove,
    }
    private enum spawnLocation
    {
        North,
        East,
        South,
        West,
    }

    private List<Node2D> footballPlayers = new List<Node2D>();

    [Export] private gapLocation gapChosen { get; set; } = gapLocation.SmallMiddle;
    [Export] private spawnLocation spawnChosen { get; set; } = spawnLocation.North;
    private Timer directionTimer;

    [Export] float waitTimer;

    private Vector2 direction;

    public override void _Ready()
    {
        PackedScene footballPlayer = GD.Load<PackedScene>("Scenes/entities/enemies/FootballPlayer.tscn");
        Node2D footballPlayerInstance = (Node2D)footballPlayer.Instantiate();
        Sprite2D footballPlayerSprite = footballPlayerInstance.GetNode<Sprite2D>("Sprite2D");
        float footballPlayerWidth = footballPlayerSprite.Texture.GetWidth();
        float footballPlayerHeight = footballPlayerSprite.Texture.GetHeight();

        viewport = GetViewport();
        Rect2 visibleRect = viewport.GetVisibleRect();
        Vector2 viewportSize = visibleRect.Size;
        float viewportWidth = viewportSize.X;
        float viewportHeight = viewportSize.Y;
        footballPlayerWidth /= 2;
        footballPlayerHeight /= 2;

        if (spawnChosen == spawnLocation.North || spawnChosen == spawnLocation.South)
        {
            numberToSpawn = viewportWidth / footballPlayerWidth;
        }
        else
        {
            numberToSpawn = viewportHeight / footballPlayerHeight;
        }

        numberToSpawn = (int)numberToSpawn;

        switch (spawnChosen)
        {
            case spawnLocation.North:
                for (int i = 0; i < numberToSpawn; i++)
                {
                    spawnPosition = new Vector2(0 + (footballPlayerWidth * i), 0);
                    Node2D footballPlayerDup = (Node2D)footballPlayerInstance.Duplicate(7);
                    footballPlayerDup.Name = $"footballPlayer{i + 1}";
                    footballPlayerDup.Position = spawnPosition;
                    AddChild(footballPlayerDup);
                    footballPlayers.Add(footballPlayerDup);
                }
                break;

            case spawnLocation.East:
                for (int i = 0; i < numberToSpawn; i++)
                {
                    spawnPosition = new Vector2(0 + (footballPlayerWidth * i), 0);
                    Node2D footballPlayerDup = (Node2D)footballPlayerInstance.Duplicate(7);
                    footballPlayerDup.Name = $"footballPlayer{i + 1}";
                    footballPlayerDup.Position = spawnPosition;
                    AddChild(footballPlayerDup);
                    footballPlayers.Add(footballPlayerDup);
                }
                break;

            case spawnLocation.South:
                for (int i = 0; i < numberToSpawn; i++)
                {
                    spawnPosition = new Vector2(0 + (footballPlayerWidth * i), 0);
                    Node2D footballPlayerDup = (Node2D)footballPlayerInstance.Duplicate(7);
                    footballPlayerDup.Name = $"footballPlayer{i + 1}";
                    footballPlayerDup.Position = spawnPosition;
                    AddChild(footballPlayerDup);
                    footballPlayers.Add(footballPlayerDup);
                }
                break;

            case spawnLocation.West:
                for (int i = 0; i < numberToSpawn; i++)
                {
                    spawnPosition = new Vector2(0 + (footballPlayerWidth * i), 0);
                    Node2D footballPlayerDup = (Node2D)footballPlayerInstance.Duplicate(7);
                    footballPlayerDup.Name = $"footballPlayer{i + 1}";
                    footballPlayerDup.Position = spawnPosition;
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
    

    }

    public override void _Process(double delta)
    {
        switch (spawnChosen)
        {
            case spawnLocation.North:
                Velocity = Vector2.Down * data.Speed * (float)delta;
                break;
            case spawnLocation.South:
                Velocity = Vector2.Up * data.Speed * (float)delta;
                break;
            case spawnLocation.East:
                RotationDegrees = 90;
                Velocity = Vector2.Left * data.Speed * (float)delta;
                break;
            case spawnLocation.West:
                RotationDegrees = 270;
                Velocity = Vector2.Right * data.Speed * (float)delta;
                break;
        }
        switch (gapChosen)
        {
            case gapLocation.SmallLeft:
                footballPlayers[0].Hide();
                break;
            case gapLocation.SmallRight:
                footballPlayers[footballPlayers.Count - 1].Hide();
                break;
            case gapLocation.SmallMiddle:
                int count = footballPlayers.Count;
                int mid = count / 2;
                if(footballPlayers.Count % 2 == 1)
                {
                    footballPlayers[mid].Hide();
                } else
                {
                    footballPlayers[mid - 1]?.Hide();
                    footballPlayers[mid]?.Hide();
                }
                break;
            case gapLocation.WideLeft:
                footballPlayers[0].Hide();
                footballPlayers[1].Hide();
                break;
            case gapLocation.WideRight:
                footballPlayers[footballPlayers.Count - 1].Hide();
                footballPlayers[footballPlayers.Count - 2].Hide();
                break;
            case gapLocation.WideMiddle:
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
            case gapLocation.ZigZagMove:
                for (int i = 0; i < footballPlayers.Count; i++)
                {
                    if (i % 2 == 0)
                    {
                        footballPlayers[i].Hide();
                    }
                }
                if (spawnChosen == spawnLocation.North)
                {
                    Vector2 velocity = Velocity;
                    velocity = direction * data.Speed * 2 * (float)delta;
                    velocity.Y = data.Speed * (float)delta; 
                    Velocity = velocity; 
                } else if(spawnChosen == spawnLocation.South)
                {
                    Vector2 velocity = Velocity;
                    velocity = direction * data.Speed * 2 * (float)delta;
                    velocity.Y = -(data.Speed * (float)delta); 
                    Velocity = velocity; 
                } else if(spawnChosen == spawnLocation.East)
                {
                    Vector2 velocity = Velocity;
                    velocity = direction * data.Speed * 2 * (float)delta;
                    velocity.X = -(data.Speed * (float)delta); 
                    Velocity = velocity; 
                } else
                {
                    Vector2 velocity = Velocity;
                    velocity = direction * data.Speed * 2 * (float)delta;
                    velocity.X = data.Speed * (float)delta; 
                    Velocity = velocity; 
                }
                break;
        }
    }
    
    private void OnDirectionTimerTimeout()
    {
            if(spawnChosen == spawnLocation.North || spawnChosen == spawnLocation.South)
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
