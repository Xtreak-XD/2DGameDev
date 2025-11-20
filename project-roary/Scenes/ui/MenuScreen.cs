using Godot;
using GodotPlugins.Game;
using System;
using System.Collections.Generic;

public partial class MenuScreen : Control
{
	private bool isPaused = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
    {
     	Hide();
		ProcessMode = ProcessModeEnum.Always; 

		var buttons = new[] {
			GetNode<Button>("Continue"),
			GetNode<Button>("Settings"),
			GetNode<Button>("Quit")
		};

		foreach (var button in buttons)
        {
			var position = button.Position.X;

			button.MouseEntered += () => SlideButton(button, position, true);
			button.MouseExited += () => SlideButton(button, position, false);
			button.Pressed += () => OnButtonPressed(button.Name);
        }
	
    }

	private void SlideButton(Button button, float pos, bool isHovering)
    {    
		var tween = CreateTween();
		tween.SetEase(Tween.EaseType.Out);
		tween.SetTrans(Tween.TransitionType.Cubic);
		
		if (isHovering)
		{
			// Slide Right
			tween.TweenProperty(button, "position:x", pos + 50, 0.6);
		}
		else
		{
			// Return to original position
			tween.TweenProperty(button, "position:x", pos, 0.6);
		}
    }

    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed("Pause"))
        {
			TogglePause();
        }
    }

	private void OnButtonPressed(String name)
    {
        switch (name)
        {
            case "Continue":
				OnContinuePress();
				break;
			case "Settings":
				OnSettingsPress();
				break;
			case "Quit":
				OnQuitPress();
				break;
        }
    }

	private void TogglePause()
    {
        isPaused = !isPaused;
		Visible = isPaused;
		GetTree().Paused = isPaused;
    }
	private void OnContinuePress()
    {
        TogglePause();
    }

	private void OnSettingsPress()
    {
        GD.Print("Open Settings");
    }

	private void OnQuitPress()
    {
        GetTree().Quit();
    }
}
