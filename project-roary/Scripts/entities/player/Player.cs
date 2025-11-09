using Godot;
using System;


public partial class Player : CharacterBody2D
{
	[Export]
	public GenericData data;
	public AnimationPlayer animationPlayer;
	public AnimatedSprite2D anim;
	public PlayerStateMachine stateMachine;

	public Vector2 cardinalDirection = Vector2.Down;
	public Vector2 direction = Vector2.Zero;

    public override void _Ready()
	{
		AddToGroup("player");
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		stateMachine = GetNode<PlayerStateMachine>("PlayerStateMachine");

		stateMachine.Initialize(this);
    }

	public override void _Process(double delta)
	{
		direction.X = Input.GetActionStrength("Right") - Input.GetActionStrength("Left");
		direction.Y = Input.GetActionStrength("Down") - Input.GetActionStrength("Up");
	}

	public override void _PhysicsProcess(double delta)
	{
		MoveAndSlide();
	}

	public bool SetDirection()
	{
		Vector2 sprite = Scale;

		Vector2 newDir = cardinalDirection;

		if (direction == Vector2.Zero)
		{
			return false;
		}

		if (direction.Y == 0)
		{
			newDir = direction.X < 0 ? Vector2.Left : Vector2.Right;
		}
		else if (direction.X == 0)
		{
			newDir = direction.Y < 0 ? Vector2.Up : Vector2.Down;
		}

		if (newDir == cardinalDirection)
		{
			return false;
		}

		cardinalDirection = newDir;
		sprite.X = cardinalDirection == Vector2.Left ? -1 : 1;
		return true;
    }

	public void UpdateAnimation(String state)
	{
		if (animationPlayer.HasAnimation(state) &&
			(animationPlayer.CurrentAnimation != state || !animationPlayer.IsPlaying()))
		{
			animationPlayer.Play(state);
		}
	}
	
	public string AnimDirection()
    {
		if (cardinalDirection == Vector2.Down) return "down";
		else if (cardinalDirection == Vector2.Up) return "up";
		else return "side";
    }

}
