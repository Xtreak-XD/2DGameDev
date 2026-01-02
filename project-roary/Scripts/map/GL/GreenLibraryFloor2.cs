using Godot;
using System;

public partial class GreenLibraryFloor2 : Node2D
{

    SaveManager saveManager;
    public override void _Ready()
    {
        saveManager = GetNode<SaveManager>("/root/SaveManager");
        unlockMermaidBoss(saveManager.metaData.CanEnterMermaidRoom, saveManager.metaData.DefeatedMermaid);
        var dayNight = GetNode<DayNightCycle>("/root/DayNightCycle");
        dayNight.Visible = false;
    }

    public override void _ExitTree()
    {
        var dayNight = GetNode<DayNightCycle>("/root/DayNightCycle");
        dayNight.Visible = true;
    }

    void unlockMermaidBoss(bool canFight, bool alreadyDefeated)
    {
        if (canFight && !alreadyDefeated)
        {
            GetNode<Area2D>("areaLocked").Monitoring = false;
            GetNode<Area2D>("sceneSwitchArea").Monitoring = true;
        }
        else
        {
            GetNode<Area2D>("areaLocked").Monitoring = true;
            GetNode<Area2D>("sceneSwitchArea").Monitoring = false;
        }
    }
}
