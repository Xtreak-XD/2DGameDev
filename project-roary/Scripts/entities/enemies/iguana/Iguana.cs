using System;
using Godot;

public partial class Iguana : Enemy
{
	public new IguanaStateMachine stateMachine;

	[Export]
	public Player target;
	public Area2D aggroArea;
	public Area2D hurtBox;
	public Area2D hitbox;

	public AnimationPlayer anim;

	public const int ROAM_RANGE = 150;

	public override void _Ready()
	{
		AddToGroup("enemy");
		target = (Player)GetTree().GetFirstNodeInGroup("player");
		anim = GetNode<AnimationPlayer>("AnimationPlayer");
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
