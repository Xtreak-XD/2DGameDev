using Godot;
using System;

public partial class Alligator : Enemy
{
	public new AlligatorStateMachine stateMachine;

	[Export]
	public ColorVariantData colorVariant;

	public Player target;
	public Area2D aggroArea;
	public Area2D hurtBox;
	public Area2D hitbox;
	public Vector2 homePosition;

	public const int ROAM_RANGE = 100;
	public const int DRAG_MAX_DIST = 750;
	public const int LUNGE_RANGE = 75;
	public const int ATTACK_RANGE = 200;

	public override void _Ready()
    {
		AddToGroup("enemy");

		stateMachine = GetNode<AlligatorStateMachine>("AlligatorStateMachine");
		stateMachine.Initialize(this);
		aggroArea = GetNode<Area2D>("TargetDetector");
		hurtBox = GetNode<Area2D>("HurtBox");
		hitbox = GetNode<Area2D>("Hitbox");
		target = GetParent().GetNode<Player>("Player");

		homePosition = GlobalPosition;

		GD.Print($"Alligator home position: ({homePosition.X}, {homePosition.Y})");
    }

	public override void _Process(double delta)
	{
	}

	public bool IsPlayerInChaseRange()
	{
		return aggroArea.GetOverlappingBodies().Contains(target);
	}

	public bool IsPlayerInAttackRange()
	{
		return GlobalPosition.DistanceTo(target.GlobalPosition)
		 <= ATTACK_RANGE;
	}

	public bool IsPlayerInChompkRange()
	{
		return hurtBox.GetOverlappingBodies().Contains(target);
	}

	public bool IsPlayerInDeathRollRange()
	{
		return GlobalPosition.DistanceTo(target.GlobalPosition)
		 <= DRAG_MAX_DIST;
	}

	public bool IsPlayerInLungeRange()
	{
		return GlobalPosition.DistanceTo(target.GlobalPosition)
		 <= LUNGE_RANGE;
	}

	public Vector2 GetRandomPositionInRoamRange()
	{
		Random random = new Random();

		float roamRange = ROAM_RANGE;

		float randomX = random.Next((int)-roamRange, (int)roamRange + 1);
		float randomY = random.Next((int)-roamRange, (int)roamRange + 1);

		return Position + new Vector2(randomX, randomY);
	}
}
