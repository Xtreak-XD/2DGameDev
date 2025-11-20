using Godot;
using GodotPlugins.Game;
using System;
using System.Collections.Generic;

public partial class MenuScreen : Control
{
	private bool _isPaused = false;
	private Dictionary<Button, float> _originalPositions = new Dictionary<Button, float>();

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
			var currentButton = button;
			var originalX = currentButton.Position.X;
			
			_originalPositions[currentButton] = originalX;

			currentButton.MouseEntered += () => SlideButton(currentButton, originalX, true);;
			currentButton.MouseExited += () => SlideButton(currentButton, originalX, false);
			currentButton.Pressed += () => OnButtonPressed(currentButton.Name);
        }
    }

	private void SlideButton(Button button, float originalX, bool isHovering)
    {		
		var tween = CreateTween();
		tween.SetEase(Tween.EaseType.Out);
		tween.SetTrans(Tween.TransitionType.Cubic);
		
		if (isHovering)
		{
			tween.TweenProperty(button, "position:x", originalX + 50, 0.6);
		}
		else
		{
			tween.TweenProperty(button, "position:x", originalX, 0.6);
		}
    }

    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed("Pause"))
        {
			TogglePause();
        }
    }

	private void OnButtonPressed(string name)
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
        _isPaused = !_isPaused;
		Visible = _isPaused;
		GetTree().Paused = _isPaused;
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
		GetTree().Paused = false;
        GetTree().Quit();
    }
}