using Godot;
using System;

public partial class SurveyAnDbugs : LinkButton
{
    private Tween _tween;

    public override void _Ready()
    {
        StartPulse();
    }

    private void StartPulse()
    {
        if (_tween != null && _tween.IsValid())
        {
            _tween.Kill();
        }

        _tween = CreateTween();

        _tween.SetLoops();

        _tween.TweenProperty(this, "scale", new Vector2(1.1f, 1.1f), 0.5)
            .SetTrans(Tween.TransitionType.Sine)
            .SetEase(Tween.EaseType.InOut);

        _tween.TweenProperty(this, "scale", new Vector2(1.0f, 1.0f), 0.5)
            .SetTrans(Tween.TransitionType.Sine)
            .SetEase(Tween.EaseType.InOut);
    }

    public override void _ExitTree()
    {
        if (_tween != null && _tween.IsValid())
        {
            _tween.Kill();
        }
    }
}
