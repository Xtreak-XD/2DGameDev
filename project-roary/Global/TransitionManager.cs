using Godot;
using System;

public partial class TransitionManager : CanvasLayer
{
    public Eventbus eventbus;
    [Export] ColorRect fadeScreen;
    [Export] AnimationPlayer animationPlayer;
    [Export] RichTextLabel textLabel;

    public override void _Ready()
    {
        eventbus = GetNode<Eventbus>("/root/Eventbus");
        fadeScreen.Visible = false;
        textLabel.Visible = false;
        animationPlayer.AnimationFinished += OnAnimationFinished;
    }

    public override void _ExitTree()
    {
        animationPlayer.AnimationFinished -= OnAnimationFinished;
    }


    public void OnAnimationFinished(StringName animName)
    {
        switch (animName)
        {
            case "fade_to_blue":
                eventbus.EmitSignal(Eventbus.SignalName.onTransitionFinished);
                animationPlayer.Play("fade_to_normal");
                break;
            case "fade_to_normal":
                fadeScreen.Visible = false;
                break;
            case "Intro_Opening":
                eventbus.EmitSignal(Eventbus.SignalName.onTransitionFinished);
                textLabel.Visible = false;
                break;
            case "Opening_World":
                eventbus.EmitSignal(Eventbus.SignalName.onTransitionFinished);
                textLabel.Visible = false;
                break;
        }
    }

    public void transition(string titleTransitionName = "")
    {
        switch (titleTransitionName)
        {
            case "Intro_Opening":
                textLabel.Visible = true;
                animationPlayer.Play("Intro_Opening");
                break;
            case "Opening_World":
                textLabel.Visible = true;
                animationPlayer.Play("Opening_World");
                break;
            default:
                fadeScreen.Visible = true;
                animationPlayer.Play("fade_to_blue");
                break;
        }
    }

}
