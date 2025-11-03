using System;
using Godot;

public partial class Iguana : Enemy
{
	[Export]
	public new GenericData data;
	public new IguanaStateMachine stateMachine;

	[Export]
	public Player target;
	public Area2D aggroArea;
	public Area2D hurtBox;
	public Area2D hitbox;

	public const int ROAM_RANGE = 150;

	public override void _Ready()
	{
		AddToGroup("enemy");
		stateMachine = GetNode<IguanaStateMachine>("IguanaStateMachine");
		stateMachine.Initialize(this);
		aggroArea = GetNode<Area2D>("TargetDetector");
		hurtBox = GetNode<Area2D>("HurtBox");
		hitbox = GetNode<Area2D>("Hitbox");
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
	
	public void AttackPlayer()
	{
		if(hitbox.GetOverlappingBodies().Contains(target))
		{
            // Deal damage when player is in hitbox range.
			GD.Print("The iguana has attacked.");
        }
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
    {
    }
}
