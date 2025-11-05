using Godot;
using System;
using System.Collections.Generic;

public partial class Hitbox : Area2D
{
   [Export] public bool ContinuousDamage = false;
   [Export]  public float DamageInterval = 1.0f;

   [Export] public float _contactTimer = 0f; // seconds between damage applications

    //enemy or player hitbox reference 
    [Export] public string Attackergroup = "enemy";
    [Export] public string Targetgroup = "player";

    public GenericData data;
    public Eventbus eventbus;
    private List<Area2D> targetsInRange = new List<Area2D>();

    private Node parent;
    public override void _Ready()
    {

        AddToGroup("hitbox");
        eventbus = GetNode<Eventbus>("/root/Eventbus");
        parent = GetParent();

        if (parent is Player playerParent)
        {
            if (playerParent.data != null)
            {
                data = playerParent.data;
            }
            else
            {
                GD.PushWarning("Parent's 'data' property is null");
            }
        }
        else if (parent is Enemy enemyParent)
        {
            if (enemyParent.data != null)
            {
                data = enemyParent.data;
            }
            else
            {
                GD.PushWarning("Parent's 'data' property is null");
            }
        }
        else
        {
            GD.PushWarning("Hitbox parent is not a 'Character' type!");
        }

        AreaEntered += onAreaEntered;
        AreaExited += OnAreaExited;

        // Initialize contact timer
        _contactTimer = DamageInterval;
    }

    public void onAreaEntered(Area2D area)
    {
        if (!area.IsInGroup("hurtbox")) return;
        Node targetParent = area.GetParent();
        if (targetParent == parent) return;
        if (!parent.IsInGroup(Attackergroup)) return;
        if (!targetParent.IsInGroup(Targetgroup)) return;

        targetsInRange.Add(area);
        GD.Print($"ENTER: {parent.Name} saw {area.GetParent().Name}");
    }

    public void OnAreaExited(Area2D area)
    {
        if (area.IsInGroup("hurtbox") && targetsInRange.Contains(area))
        {
            targetsInRange.Remove(area);
        }
    }

    public override void _Process(double delta)
    {
        if (!ContinuousDamage)
            return;
        _contactTimer -= (float)delta;
        if(_contactTimer <= 0f)
        {
            ApplyDamageToAllTargets();
            _contactTimer = DamageInterval;
        }
    }
    public override void _Input(InputEvent @event)
    {
        if (ContinuousDamage)
            return;
        if (@event.IsActionPressed("attack"))
        {
            foreach (var area in targetsInRange)
            {
                if (area != null && GodotObject.IsInstanceValid(area) && area.IsInsideTree())
                {
                    eventbus.EmitSignal("applyDamage", area.GetParent().Name, GetParent().Name, data.Damage);
                }
            }
        }
    }
    public void ApplyDamageToAllTargets()
    {
        if (data == null)
        {
            GD.PushWarning("Hitbox data is null, cannot apply damage.");
        }
        int damageAmount = data != null ? data.Damage : 1;
        foreach(var area in targetsInRange)
        {
            if (area != null && GodotObject.IsInstanceValid(area) && area.IsInsideTree())
            {
                Node targetparent = area.GetParent();
                if (targetparent == parent) continue;
                if (!parent.IsInGroup(Attackergroup)) continue;
                if (!targetparent.IsInGroup(Targetgroup)) continue;
                
                eventbus.EmitSignal("applyDamage", area.GetParent().Name, GetParent().Name, damageAmount);
            }
        }
    }

}
