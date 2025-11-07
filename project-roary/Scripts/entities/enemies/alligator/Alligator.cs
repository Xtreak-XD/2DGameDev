using Godot;
using System;

public partial class Alligator : Enemy
{
	public new AlligatorStateMachine stateMachine;

	[Export]
	public ColorVariantData colorVariant;

	[Export]
	public Player target;
	public Area2D aggroArea;
	public Area2D hurtBox;
	public Area2D hitbox;

	public const int ROAM_RANGE = 100;

	public override void _Ready()
    {
		AddToGroup("enemy");

		stateMachine = GetNode<AlligatorStateMachine>("AlligatorStateMachine");
		stateMachine.Initialize(this);
		aggroArea = GetNode<Area2D>("TargetDetector");
		hurtBox = GetNode<Area2D>("HurtBox");
		hitbox = GetNode<Area2D>("Hitbox");
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
		return hurtBox.GetOverlappingBodies().Contains(target);
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
