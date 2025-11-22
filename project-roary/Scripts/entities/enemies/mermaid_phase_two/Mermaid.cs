using Godot;
using System;

public partial class Mermaid : Enemy
{
	public new MermaidStateMachine stateMachine;
	public Hitbox hitbox;
	public HurtBox hurtBox;
	public Marker2D projectileSource;
	public Player target;
	public AnimationPlayer anim;
	public Timer targetTimer;
    public Timer recoverTridentTimer;
    public Timer recoverShieldTimer;

	public bool Shielded {get; set; } = true;
	public bool HasTrident {get; set; } = true;

	[Export]
	public Marker2D[] starfishSpawns;

	[Export]
	public PackedScene trident;

	[Export]
	public PackedScene shield;

    [Export]
    public PackedScene starfishEnemy;

	public override void _Ready()
	{
		stateMachine = GetNode<MermaidStateMachine>("MermaidStateMachine");
		stateMachine.Initialize(this);

		hitbox = GetNode<Hitbox>("Hitbox");
		hurtBox = GetNode<HurtBox>("HurtBox");
		projectileSource = GetNode<Marker2D>("ProjectileSource");
		anim = GetNode<AnimationPlayer>("AnimationPlayer");
        targetTimer = GetNode<Timer>("InitializePlayerTargetting");

		targetTimer.Timeout += SetTarget;
        targetTimer.Start();

        recoverShieldTimer = GetNode<Timer>("RecoverShieldTimer");
        recoverTridentTimer = GetNode<Timer>("RecoverTridentTimer");

        recoverTridentTimer.Timeout += SetHasTrident;
        recoverShieldTimer.Timeout += SetHasShield;
	}

    public override void _EnterTree()
    {
        AddToGroup("enemy");
    }

    public override void _ExitTree()
    {
        targetTimer.Timeout -= SetTarget;
        recoverTridentTimer.Timeout -= SetHasTrident;
        recoverShieldTimer.Timeout -= SetHasShield;
    }

	public void SetTarget()
    {
        target = (Player)GetTree().GetFirstNodeInGroup("player");

        if(target == null)
        {
            GD.Print("Player could not be found.");
        }
    }

    public void SetHasShield()
    {
        GD.Print("Mermaid has reclaimed its shield after a successful hit.");
        Shielded = true;
    }

    public void SetHasTrident()
    {
        GD.Print("Mermaid has reclaimed its trident after a successful hit.");
        HasTrident = true;
    }

	public Marker2D GetRandomStarfishSpawn()
    {
		Marker2D spawn = null;

        if(AllSpawnsOccupied())
        {
            return null;
        }

		while(SpawnOccupied(spawn))
        {
            spawn = starfishSpawns[new Random().Next(starfishSpawns.Length)];
        }

		if(spawn == null)
        {
            GD.Print("No unoccupied starfish spawns found");
        }

        return spawn;
    }

	public bool SpawnOccupied(Marker2D spawn)
    {
		if(spawn == null) // This should prevent infinite loops or issues
        {                 // when we get random spawn positions
            return true;
        }

        foreach(Node child in spawn.GetChildren())
        {
            if(child is Starfish)
            {
                return true;
            }
        }

		return false;
    }
	
    private bool AllSpawnsOccupied()
    {
        foreach(Marker2D spawn in starfishSpawns) 
        {
            if(!SpawnOccupied(spawn))
            {
                return false;
            }
        }

        return true;
    }
}
