using Godot;
using System;

public partial class StadiumUnavailable : Area2D
{
    public override void _Ready()
    {
        BodyEntered += onAreaEntered;
        BodyExited += onAreaExit;
    }

    public override void _ExitTree()
    {
        BodyEntered -= onAreaEntered;
        BodyExited -= onAreaExit;
    }

    void onAreaEntered(Node2D area)
    {
        GetNode<Label>("UnavailableLabel").Visible = true;
    }

    void onAreaExit(Node2D area)
    {
        GetNode<Label>("UnavailableLabel").Visible = false;
    }

}
