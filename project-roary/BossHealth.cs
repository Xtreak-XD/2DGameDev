using Godot;
using System;

public partial class BossHealth : CanvasLayer
{
	private TextureProgressBar health;
	private Enemy boss;
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
            boss = bossNodes[0] as Enemy;
        }

		if (boss == null)
        {
            GD.PrintErr("BossHealth: Parent boss not found!");
			return;
        }

		if (boss.data == null)
		{
			GD.PrintErr("BossHealth: Boss data not found!");
			return;
		}

		bossName.Text = boss.Name;
		health.MaxValue = boss.data.MaxHealth;
		health.Value = boss.data.Health;
		eventbus.updateBossHealth += updateHealth;
    }

	private void updateHealth(int value)
    {
        health.Value = value;
    }

	public override void _ExitTree()
	{
		if (eventbus != null)
		{
			eventbus.updateBossHealth -= updateHealth;
		}
	}
}
