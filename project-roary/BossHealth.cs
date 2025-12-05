using Godot;
using System;

public partial class BossHealth : CanvasLayer
{
	private TextureProgressBar health;
	private CharacterBody2D boss;
	private Label bossName; 
	public override void _Ready()
    {
        health = GetNode<TextureProgressBar>("Control/VBoxContainer/BossHealth");
		bossName = GetNode<Label>("Control/VBoxContainer/BossName");
		var bossNodes = GetTree().GetNodesInGroup("enemy");

		foreach (var bossNode in bossNodes)
        {
            GD.Print(bossNode.Name);
        }

		if (bossNodes.Count > 0)
        {
            boss = bossNodes[0] as CharacterBody2D;
        }

		if (boss == null)
        {
            GD.PrintErr("BossHealth: Parent boss not found!");
        }
		else
		{
			bossName.Text = boss.Name;
		}
    }

	private void updateHealth(int value)
    {
        health.Value = value;
    }
	
}
