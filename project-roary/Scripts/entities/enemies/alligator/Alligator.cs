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

	public AnimationPlayer anim;

	public const int ROAM_RANGE = 3000;
	public const int DRAG_MAX_DIST = 2500;
	public const int LUNGE_RANGE = 200;
	public const int ATTACK_RANGE = 200;
	public const int CHOMP_RANGE = 100;

	public override void _Ready()
	{
		if (data != null)
		{
			data = (GenericData)data.Duplicate();
		}

		target = (Player)GetTree().GetFirstNodeInGroup("player");

		anim = GetNode<AnimationPlayer>("AnimationPlayer");

		aggroArea = GetNode<Area2D>("TargetDetector");
		hurtBox = GetNode<Area2D>("HurtBox");
		hitbox = GetNode<Area2D>("Hitbox");

		homePositionTimer.Timeout += SetHomePos;
		stateMachine = GetNode<AlligatorStateMachine>("AlligatorStateMachine");
		stateMachine.Initialize(this);
	}

	public override void _EnterTree()
	{
		AddToGroup("enemy");
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
		return GlobalPosition.DistanceTo(target.GlobalPosition)
		 <= CHOMP_RANGE;
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

	public void animation(Vector2 direction)
    {
        if (Mathf.Abs(direction.X) > Mathf.Abs(direction.Y))
        {
            if (direction.X > 0)
            {
                if (!anim.IsPlaying() || anim.CurrentAnimation != "walk_right")
                {
                    anim.Play("walk_right");
                }
            }
            else if (direction.X < 0)
            {
                if (!anim.IsPlaying() || anim.CurrentAnimation != "walk_left")
                {
                    anim.Play("walk_left");
                }
            }
        }
        else if (Mathf.Abs(direction.Y) > 0)
        {
            if (direction.Y > 0)
            {
                if (!anim.IsPlaying() || anim.CurrentAnimation != "walk_down")
                {
                    anim.Play("walk_down");
                }
            }
            else if (direction.Y < 0)
            {
                if (!anim.IsPlaying() || anim.CurrentAnimation != "walk_up")
                {
                    anim.Play("walk_up");
                }
            }
        }
        else
        {
            // Not moving, stop animation
            if (anim.IsPlaying())
                anim.Stop();
        }
    }
}
