using Godot;
using System;

public partial class NaturePreserveController : Node2D
{
    SaveManager saveManager;

    public override void _Ready()
    {
        saveManager = GetNode<SaveManager>("/root/SaveManager");
        brotherSavedCheck(saveManager.metaData.SavedYoungerTurtleBrother);
    }

    void brotherSavedCheck(bool brotherSaved)
    {
        if (brotherSaved)
        {
            GetNode<CharacterBody2D>("WiseTurtleBrother").QueueFree();
        }
    }
}
