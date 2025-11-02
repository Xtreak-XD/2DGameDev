using Godot;
using System;
public partial class Petitioner : Enemy
{
    // how often this enemy hurts you while you're touching it
    public float ContactDamageInterval = 0.5f;

    // can be turned off in the editor if you ever want a non-contact version
    public bool UseContactDamage = true;

    private Hitbox _hitbox;

    public override void _Ready()
    {
        // IMPORTANT: this calls Enemy._Ready() so your FSM + despawn still work
        base._Ready();

        // get the Hitbox child we added in the scene
        _hitbox = GetNodeOrNull<Hitbox>("Hitbox");
        if (_hitbox == null)
        {
            GD.PushWarning($"{Name}: Hitbox node not found — contact damage will not work.");
            return;
        }

        // configure the shared Hitbox script to behave like a “touch = hurt” enemy
        _hitbox.ContinuousDamage = UseContactDamage;
        _hitbox.DamageInterval = ContactDamageInterval;
    }
}
