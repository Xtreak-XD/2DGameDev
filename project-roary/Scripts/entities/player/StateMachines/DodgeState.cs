using State = PlayerState;
using Godot;
using System;

public partial class DodgeState : State
{
    public Eventbus eventbus;
    public PlayerStateMachine stateMachine;
    public State idle;
    public State walk;
    [Export] public float dodgeSpeed;
    [Export] public double dodgeDuration;

    [Export] public int staminaCost;

    public bool dodging = false;
    bool isSignalConnected = false;
    public override void _Ready()
    {
        eventbus = GetNode<Eventbus>("/root/Eventbus");
        stateMachine = GetParent<PlayerStateMachine>();
        walk = GetNode<WalkState>("../walk");
        idle = GetNode<IdleState>("../idle");
    }

    public override void _ExitTree()
    {
        if (player.animationPlayer != null && isSignalConnected)
        {
            player.animationPlayer.AnimationFinished -= OnAnimFinished;
            isSignalConnected = false;
        }
    }


// what happens when player enters their new state
    public override void Enter()
    {
        if (player.data.Stamina >= staminaCost)
        {
            player.usingStamina = true;
            player.data.Stamina -= staminaCost;
            eventbus.EmitSignal("updateStamina",player.data.Stamina);
            dodging = true;

            player.UpdateAnimation("dodge");

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

// what happens when player exits their current state
    public override void Exit()
    {
        player.usingStamina = false;
        dodging = false;
    }

    private void OnAnimFinished(StringName animName)
    {
        if (animName == "dodge_down" || animName == "dodge_up" || animName == "dodge_left" || animName == "dodge_right")
        {
            Exit();
        }
    }

    public override State Process(double delta)
    {
        if (!dodging)
        {
            return stateMachine.prevState;
        }
        return null;
    }

    public override State Physics(double delta)
    {
        if (dodging)
        {
            player.Velocity = player.mousePosition * dodgeSpeed * 65 * (float)(player.data.Accel * delta);
        }
        return null;
    }

    public override State HandleInput(InputEvent @event)
    {
        return null;
    }
}
