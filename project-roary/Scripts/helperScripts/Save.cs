using Godot;
using System;

public partial class Save : StaticBody2D
{
    public interactionArea interactionArea;
    public Eventbus eventbus;
    [Export]
    public PointLight2D lampLight;
    
    [Export]
    public AnimatedSprite2D animatedSprite;

    public string dayAnimation = "off";
    
    public string nightAnimation = "on";

    public override void _Ready()
    {
        eventbus = GetNode<Eventbus>("/root/Eventbus");
        interactionArea = GetNode<interactionArea>("InteractableArea");
        interactionArea.interact = Callable.From(onInteract);

        eventbus.dayStarted += OnDayStarted;
        eventbus.nightStarted += OnNightStarted;
    }

    private void OnDayStarted()
    {
        if (lampLight != null)
            lampLight.Visible = false;
            
        if (animatedSprite != null)
            animatedSprite.Play(dayAnimation);
    }

    private void OnNightStarted()
    {
        if (lampLight != null)
            lampLight.Visible = true;
            
        if (animatedSprite != null)
            animatedSprite.Play(nightAnimation);
    }

    void onInteract()
    {
        eventbus.EmitSignal("save", false);
        eventbus.EmitSignal("interactionComplete");
    }
    
    public override void _ExitTree()
    {
        if (eventbus != null)
        {
            eventbus.dayStarted -= OnDayStarted;
            eventbus.nightStarted -= OnNightStarted;
        }
    }
}
