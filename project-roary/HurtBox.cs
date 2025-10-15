using Godot;
using System;

public partial class HurtBox : Area2D
{

    public override void _Ready()
    {
        var layersAndMasks = (LayersAndMasks)GetNode("/root/LayersAndMasks");
        CollisionLayer = 0;
        CollisionMask = layersAndMasks.GetCollisionLayerByName("HitBox"); //Change to the actual scene name of hitbox
        Connect(Area2D.SignalName.AreaEntered, new Callable(this, nameof(OnAreaEntered))); // Create a signal for area_entered
    }

        private void OnAreaEntered(Area2D area)
    {
        
        if (area is not Hitbox hitbox) // Replace "Hitbox" with the correct hitbox scene name
            return;

        if (Owner is ITakeDamage characterTakeDamage)
        {
            characterTakeDamage.TakeDamage(hitbox.Damage, hitbox.attackFromVector);
        }
    }

}
