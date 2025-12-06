using Godot;
using Godot.Collections;

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
		Array<Node> bossNodes = GetTree().GetNodesInGroup("enemy");

		foreach(Node node in bossNodes)
        {
            if(node is Logo || node is Mermaid || node is Roary)
			{
				boss = node as Enemy;
				break;
			}
        }

		foreach (var bossNode in bossNodes)
        {
            GD.Print(bossNode.Name);
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

	private bool IsBossInScene()
	{
    	return boss != null
			&& IsInstanceValid(boss)
			&& !boss.IsQueuedForDeletion()
			&& boss.IsInsideTree();
	}


	public override void _Process(double delta)
	{
		if (!IsBossInScene())
		{
			Array<Node> bossNodes = GetTree().GetNodesInGroup("enemy");

			foreach(Node node in bossNodes)
			{
				if(node is null) continue;

				if(node is Logo || node is Mermaid || node is Roary)
				{
					if(node == null || node.IsQueuedForDeletion()) continue;
					boss = node as Enemy;
					break;
				}
			}

			return;
		}

		bossName.Text = boss.Name;
		health.Value = boss.data.Health;
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
