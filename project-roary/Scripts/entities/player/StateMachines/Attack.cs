using Godot;
using System;
using State = PlayerState;

public partial class Attack : State
{
    public Eventbus eventbus;
    public PlayerStateMachine stateMachine;
    public State idle;
    public State walk;
    public bool attacking = false;
    private bool isSignalConnected = false;
    public override void _Ready()
    {
        eventbus = GetNode<Eventbus>("/root/Eventbus");
        stateMachine = GetParent<PlayerStateMachine>();
        walk = GetNode<WalkState>("../walk");
        idle = GetNode<IdleState>("../idle");
    }

    public override void _ExitTree()
    {
        player.animationPlayer.AnimationFinished -= OnAnimFinished;
    }

    private void OnAnimFinished(StringName animName)
    {
        if (animName == "attack_down" || animName == "attack_up" || animName == "attack_left" || animName == "attack_right")
        {
            Exit();
        }
    }

    public override void Enter()
    {
        if (player.equippedItem == null)
        {
            attacking = true;

            player.UpdateAnimation("attack");
            if(player.animationPlayer != null && !isSignalConnected)
            {
                player.animationPlayer.AnimationFinished += OnAnimFinished;
                isSignalConnected = true;
            }
        }
        else
        {
            Exit();
        }
    }

    public override void Exit()
    {
        attacking = false;
    }

    public override State Process(double delta)
    {
        if (!attacking)
        {
            return stateMachine.prevState;
        }
        return null;
    }

    public override State Physics(double delta)
    {
        if (attacking)
        {
            player.Velocity = Vector2.Zero;
        }
        return null;
    }

    public override State HandleInput(InputEvent @event)
    {
        if (@event.IsActionPressed("attack"))
        {
            return this;
        }
        return null;
    }
}
