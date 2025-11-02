using Godot;
using System;
using System.Collections.Generic;

public partial class Hitbox : Area2D
{
    public bool ContinuousDamage = false;
    public float DamageInterval = 1.0f;

    public float _contactTimer = 0f; // seconds between damage applications
    public GenericData data;
    public Eventbus eventbus;
    private List<Area2D> targetsInRange = new List<Area2D>();

    public override void _Ready()
    {
        
        AddToGroup("hitbox");
        eventbus = GetNode<Eventbus>("/root/Eventbus");
        Node parent = GetParent();

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
        if (area.IsInGroup("hurtbox") && (area.GetParent() == GetParent()))
        {
            targetsInRange.Add(area);
        }
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
                eventbus.EmitSignal("applyDamage", area.GetParent().Name, GetParent().Name, damageAmount);
            }
        }
    }

}
