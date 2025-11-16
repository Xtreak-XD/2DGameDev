using Godot;


public partial class Player : CharacterBody2D
{

	public Eventbus eventbus;

	[Export] Resource metaData;
	[Export] public GenericData data;
	public AnimationPlayer animationPlayer;
	public PlayerStateMachine stateMachine;

	public Vector2 cardinalDirection = Vector2.Down;
	public Vector2 direction = Vector2.Zero;
	public Vector2 lastDirection = Vector2.Zero;
	public Vector2[] DIRECTIONS = {Vector2.Right, Vector2.Left, Vector2.Up, Vector2.Down};

	public bool usingStamina = false;
	public bool recoveringStamina = false;

	[Export] public float rateOfStaminaRecovery;
	[Export] public int amountOfStaminaRecovered;

	public Vector2 mousePosition;
	private Vector2 knockBackVelocity = Vector2.Zero;
	private const float KnockBackDecay = 750.0f;
	public override void _Ready()
	{
		stateMachine = GetNode<PlayerStateMachine>("PlayerStateMachine");
		AddToGroup("player");
		eventbus = GetNode<Eventbus>("/root/Eventbus");
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		stateMachine.Initialize(this);
	}

	public override void _Process(double delta)
	{
		direction.X = Input.GetActionStrength("Right") - Input.GetActionStrength("Left");
		direction.Y = Input.GetActionStrength("Down") - Input.GetActionStrength("Up");

		mousePosition = GetLocalMousePosition().Normalized();

		if (!usingStamina && data.Stamina < data.MaxStamina && !recoveringStamina)
		{
			recoveringStamina = true;
			RecoverStamina();
		}
	}

	private async void RecoverStamina()
	{
		await ToSignal(GetTree().CreateTimer(rateOfStaminaRecovery), Timer.SignalName.Timeout);
		data.Stamina += amountOfStaminaRecovered;
		eventbus.EmitSignal("updateStamina", data.Stamina);
		recoveringStamina = false;
	}
	
	public void ApplyKnockBack(Vector2 dir, float strength)
    {
		knockBackVelocity = dir * strength;
    }

	public override void _PhysicsProcess(double delta)
	{
		if (knockBackVelocity.LengthSquared() > 0.1f)
		{
			Velocity += knockBackVelocity;
			knockBackVelocity = knockBackVelocity.MoveToward(Vector2.Zero, KnockBackDecay * (float)delta);
		}
		MoveAndSlide();
	}
	
	public bool SetDirection()
	{
		if (direction == Vector2.Zero){ return false;}
		Vector2 new_dir;

		if (Mathf.Abs(direction.X) > Mathf.Abs(direction.Y))
		{
			new_dir = direction.X > 0 ? Vector2.Right : Vector2.Left;
		}
		else
		{
			new_dir = direction.Y > 0 ? Vector2.Down : Vector2.Up;
		}

		if (new_dir == cardinalDirection){ return false; }

		cardinalDirection = new_dir;

		return true;
    }

	public string SetAnimDir()
    {
		if (Mathf.Abs(cardinalDirection.X) > Mathf.Abs(cardinalDirection.Y))
		{
			return cardinalDirection.X > 0 ? "right" : "left";
		}
		else
		{
			return cardinalDirection.Y > 0 ? "down" : "up";
		}
    }
	public void UpdateAnimation(string state)
	{
		if( state != "idle")
		{
			animationPlayer.Play(state + "_" + SetAnimDir());
		}
        else
        {
            animationPlayer.Play(state + "_" + SetAnimDir());
        }
	}

	public void setSpawnPosition(Vector2 pos)
    {
        GlobalPosition = pos;
    }

}
