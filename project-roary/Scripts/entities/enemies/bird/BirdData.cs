using Godot;

[GlobalClass]
public partial class BirdData : Resource
{
    [Export] public float Speed = 450f;
    [Export] public float MinDropTime = 0.5f;
    [Export] public float MaxDropTime = 3f;
    [Export] public PackedScene PoopScene;

    [Export] public string FlyRightAnimation = "FlyRight";
    [Export] public string FlyLeftAnimation = "FlyLeft";
}