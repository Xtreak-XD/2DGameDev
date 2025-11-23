using Godot;
using System;

public partial class SfxPitchModifier : AudioStreamPlayer2D
{
    [Export] public float MinPitchScale = 0.8f;
    [Export] public float MaxPitchScale = 1.2f;

    private RandomNumberGenerator rng = new RandomNumberGenerator();

    public override void _Ready()
    {
        rng.Randomize();
        changePitch();
    }

    public void changePitch()
    {
        float randomPitch = rng.RandfRange(MinPitchScale, MaxPitchScale);

        PitchScale = randomPitch;
    }
}
