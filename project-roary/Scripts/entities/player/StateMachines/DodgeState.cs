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
    public Timer dodgeTimer;

    [Export] public int staminaCost;

    public bool dodging = false;
    public override void _Ready()
    {
        eventbus = GetNode<Eventbus>("/root/Eventbus");
        stateMachine = GetParent<PlayerStateMachine>();
        walk = GetNode<WalkState>("../walk");
        idle = GetNode<IdleState>("../idle");
        dodgeTimer = new Timer();
        AddChild(dodgeTimer);
        dodgeTimer.WaitTime = dodgeDuration;
        dodgeTimer.OneShot = true;
        dodgeTimer.Timeout += Exit;
    }

// what happens when player enters their new state
    public override void Enter()
    {
        //play dodge animation
        if (player.data.Stamina >= staminaCost)
        {
            player.usingStamina = true;
            player.data.Stamina -= staminaCost;
            eventbus.EmitSignal("updateStamina",player.data.Stamina);
            dodging = true;
            dodgeTimer.Start();
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
            float time = Mathf.Clamp((float)dodgeTimer.TimeLeft / (float)dodgeTimer.WaitTime, 0.0f, 1.0f);
            float speed = Mathf.Lerp(dodgeSpeed, player.data.Speed, time);
            player.Velocity = player.lastDirection.Normalized() * speed * (float)(player.data.Accel * delta);
        }
        return null;
    }

    public override State HandleInput(InputEvent @event)
    {
        return null;
    }
}
