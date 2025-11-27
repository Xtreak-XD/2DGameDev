using Godot;
using GodotPlugins.Game;
using System;
using System.Collections.Generic;

public partial class MenuScreen : Control
{
	private const float HOVER_OFFSET = 50f;
	private const float ANIMATION_DURATION = 0.6f;

	private readonly Dictionary<Button, float> _originalPositions = new();
	private readonly Dictionary<Button, Tween> _buttonTweens = new();
	private SettingsMenu settingsMenu;

	public override void _Ready()
    {
     	Hide();
		ProcessMode = ProcessModeEnum.Always;

		settingsMenu = GetNode<SettingsMenu>("%Setting Menu");

		var buttons = new[] {
			GetNode<Button>("%Continue"),
			GetNode<Button>("%Settings"),
			GetNode<Button>("%Quit")
		};

		foreach (var button in buttons)
        {
			if (button == null)
			{
				GD.PushWarning("MenuScreen: Button node not found");
				continue;
			}

			var originalX = button.Position.X;
			_originalPositions[button] = originalX;

			button.MouseEntered += () => SlideButton(button, originalX, true);
			button.MouseExited += () => SlideButton(button, originalX, false);
			button.Pressed += () => OnButtonPressed(button.Name);
        }
    }

	public override void _ExitTree()
	{
		var buttons = new[] {
			GetNode<Button>("%Continue"),
			GetNode<Button>("%Settings"),
			GetNode<Button>("%Quit")
		};

		foreach (var button in buttons)
		{
			if (button == null) continue;

			button.MouseEntered -= () => SlideButton(button, _originalPositions[button], true);
			button.MouseExited -= () => SlideButton(button, _originalPositions[button], false);
			button.Pressed -= () => OnButtonPressed(button.Name);

			// Clean up any running tweens
			if (_buttonTweens.TryGetValue(button, out var tween) && tween != null)
			{
				tween.Kill();
			}
		}
	}

	private void SlideButton(Button button, float originalX, bool isHovering)
    {
		// Kill any existing tween for this button to prevent overlapping animations
		if (_buttonTweens.TryGetValue(button, out var existingTween) && existingTween != null)
		{
			existingTween.Kill();
		}

		var tween = CreateTween();
		tween.SetEase(Tween.EaseType.Out);
		tween.SetTrans(Tween.TransitionType.Cubic);

		_buttonTweens[button] = tween;

		if (isHovering)
		{
			tween.TweenProperty(button, "position:x", originalX + HOVER_OFFSET, ANIMATION_DURATION);
		}
		else
		{
			tween.TweenProperty(button, "position:x", originalX, ANIMATION_DURATION);
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
		var isPaused = !GetTree().Paused;
		Visible = isPaused;
		GetTree().Paused = isPaused;
    }

	private void OnContinuePress()
    {
        TogglePause();
    }

	private void OnSettingsPress()
    {
        Hide();
		settingsMenu.ShowSettings();
    }

	private void OnQuitPress()
    {
		GetTree().Paused = false;
        GetTree().Quit();
    }
}