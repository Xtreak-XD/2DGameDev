using Godot;

[GlobalClass]
public partial class GenericData : Resource
{
	[Export] private int _health;
	public int Health
	{
		get
		{
			return _health;
		}
		set
		{
			_health = Mathf.Clamp(value, 0, MaxHealth);
		}
	}
	[Export]
	public int MaxHealth { get; set; }
	[Export]
	public int Damage { get; set; }
	[Export]
	public int Speed { get; set; }
	[Export]
	public double Accel { get; set; }
	[Export] private int _stamina;
	public int Stamina
	{
		get
		{
			return _stamina;
		}
		set
		{
			_stamina = Mathf.Clamp(value, 0, MaxStamina);
		}
	}
	[Export]
	public int MaxStamina { get; set; }

	[Export] public bool dealKnockback;
	[Export(PropertyHint.Range, "0.0, 100.0, 15.0")] public float knockBackAmount;


}
