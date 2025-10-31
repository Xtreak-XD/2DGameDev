using Godot;

public partial class IguanaRoam : IguanaState
{
	public Timer timer;
	public IguanaAttack IguanaAttack;
	public IguanaChase IguanaChase;
	bool Move;

	public override void _Ready()
	{
		timer = GetParent().GetNode<Timer>("AttackTimer");
		IguanaAttack = GetParent().GetNode<IguanaAttack>("IguanaAttack");
		IguanaChase = GetParent().GetNode<IguanaChase>("IguanaChase");
		Move = false;
    }

	public override void EnterState()
    {
		timer.Start();

		timer.Timeout += () =>
		{
			Move = true;
        };
    }

	// Called when the state is exited
	public override void ExitState()
    {
        timer.Stop();
		timer.WaitTime = 1;
    }

	public override IguanaState Process(double delta)
	{
		if(Move)
        {
            ActiveEnemy.Velocity = new Vector2(1, 0).Rotated(new RandomNumberGenerator().
		RandfRange(-Mathf.Pi / 2, Mathf.Pi / 2)) * ActiveEnemy.data.Speed *
		 (float) delta;
			ActiveEnemy.MoveAndSlide();

			Move = false;
			timer.Start();
        }

		if (ActiveEnemy.IsPlayerInChaseRange())
		{
			return IguanaChase;
		}
	
		return null;
    }
}
