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

	[Export]
	public Timer homePositionTimer;

	public const int ROAM_RANGE = 150;
	public const int DRAG_MAX_DIST = 200;
	public const int LUNGE_RANGE = 90;
	public const int ATTACK_RANGE = 120;

	public override void _Ready()
	{
		stateMachine = GetNode<AlligatorStateMachine>("AlligatorStateMachine");
		stateMachine.Initialize(this);

		aggroArea = GetNode<Area2D>("TargetDetector");
		hurtBox = GetNode<Area2D>("HurtBox");
		hitbox = GetNode<Area2D>("Hitbox");

		homePositionTimer.Timeout += SetHomePos;
	}

	public override void _EnterTree()
	{
		AddToGroup("enemy");

		target = (Player)GetTree().GetFirstNodeInGroup("player");
		homePositionTimer.Autostart = true;
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

	public bool IsPlayerInChompRange()
	{
		return hurtBox.GetOverlappingBodies().Contains(target);
	}

	public bool IsPlayerInDeathRollRange()
	{
		return homePosition.DistanceTo(GlobalPosition)
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

		return GlobalPosition + new Vector2(randomX, randomY);
	}

	public void SetHomePos()
	{
		if(homePosition == Vector2.Zero)
        {
            homePosition = GlobalPosition;
			GD.Print($"Alligator home position: ({homePosition.X}, {homePosition.Y})");
        }
    }
}
