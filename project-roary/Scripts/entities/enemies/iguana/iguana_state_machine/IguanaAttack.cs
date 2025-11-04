using Godot;

public partial class IguanaAttack : IguanaState
{
	public Timer timer;
	public bool change;

	public IguanaChase IguanaChase;

	public IguanaRoam IguanaRoam;

	public override void _Ready()
    {
		timer = GetParent().GetNode<Timer>("AttackTimer");
		IguanaChase = GetParent().GetNode<IguanaChase>("IguanaChase");
		IguanaRoam = GetParent().GetNode<IguanaRoam>("IguanaRoam");
    }
	
	// Called when the state is entered
	public override void EnterState()
	{
		timer.Start();

		timer.Timeout += Attack;
    }

	// Called when the state is exited
	public override void ExitState()
	{
		timer.Stop();
		timer.WaitTime = 0.5;
    }

	public override IguanaState Process(double delta)
	{
        if (change)
		{
			change = false;
			return IguanaChase;
        }

		return null;
	}
	
	public void Attack()
	{
		if (ActiveEnemy.IsPlayerInAttackRange())
		{
			ActiveEnemy.AttackPlayer();
			change = true;
		}

		change = true;
    }
}
