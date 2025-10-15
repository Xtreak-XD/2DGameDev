using Godot;
using System;

public interface ITakeDamage
{
    void TakeDamage(float damage, Vector2? attackFromVector); //Attack from vector could be changed to the correct variable name created from the hitbox
    
}
