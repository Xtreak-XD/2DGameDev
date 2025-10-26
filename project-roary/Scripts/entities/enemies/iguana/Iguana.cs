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
	
	public bool IsPLayerInAttackRange()
    {
        return hurtBox.GetOverlappingBodies().Contains(target);
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
    {
    }
}
