using Godot;
using System;

public partial class BossHealth : CanvasLayer
{
	private TextureProgressBar health;
	private CharacterBody2D boss;
	private Label bossName; 
	private Eventbus eventbus;
	public override void _Ready()
    {
        health = GetNode<TextureProgressBar>("%BossHealth");
		bossName = GetNode<Label>("%BossName");
		eventbus = GetNode<Eventbus>("/root/Eventbus");
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
			return;
        }

		bossName.Text = boss.Name;

		eventbus.updateBossHealth += updateHealth;
    }

	private void updateHealth(int value)
    {
        health.Value = value;
    }
	
}
