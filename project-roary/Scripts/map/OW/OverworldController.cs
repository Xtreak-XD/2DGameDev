using Godot;
using System;

public partial class OverworldController : Node2D
{
    SaveManager saveManager;

    public override void _Ready()
    {
        saveManager = GetNode<SaveManager>("/root/SaveManager");
        stadiumEntranceCheck(saveManager.metaData.CanEnterStadium);
    }

    void stadiumEntranceCheck(bool canEnterStadium)
    {
        if (canEnterStadium)
        {
            GetNode<Area2D>("StadiumEntrance").Monitoring = true;
            GetNode<Area2D>("stadiumUnavailable").Monitoring = false;
        }
        else
        {
            GetNode<Area2D>("stadiumUnavailable").Monitoring = true;
            GetNode<Area2D>("StadiumEntrance").Monitoring = false;
        }
    }
}
