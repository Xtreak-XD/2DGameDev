using System;
using Godot;

public partial class Iguana : CharacterBody2D
{
	[Export]
	public GenericData data;
	public IguanaStateMachine stateMachine;

	[Export]
	public CharacterBody2D target;
	public Area2D aggroArea;
	public Area2D hurtBox;
	public const int ROAM_RANGE = 150;

	public override void _Ready()
	{
		AddToGroup("enemy");
		stateMachine = GetNode<IguanaStateMachine>("IguanaStateMachine");
		stateMachine.Initialize(this);
		aggroArea = GetNode<Area2D>("TargetDetector");
		hurtBox = GetNode<Area2D>("HurtBox");
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

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
    {
    }
}
