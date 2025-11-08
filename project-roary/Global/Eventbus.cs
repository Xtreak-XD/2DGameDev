using Godot;
using System;

/// <summary>
/// Eventbus class for all events in the game.
/// Please document events here with a description of what they do and where they are used!
/// how to make an event-> 
/// [Signal]
/// public delegate void eventNameEventHandler();
///
/// </summary>
public partial class Eventbus : Node
{
    public static event Action<Enemy> OnEnemyDeath; //this is emitted by the enemy
    public static void EnemyDied(Enemy enemy)
    {
        OnEnemyDeath?.Invoke(enemy);
    }

    [Signal]
    public delegate void applyDamageEventHandler(string dmgReceiverName, string dmgDealerName, int dmg); //this is emitted by hitbox and used by hurtbox to pass dmg and information to deal dmg and apply effects.

    [Signal]
    public delegate void timeTickEventHandler(int day, int hour, int min); //this signal is used for ticking time, emitted by dayNightcycle script

    [Signal]
    public delegate void finishedDisplayingEventHandler(); //this is emitted by text boxes when a set of text is done displaying.

    [Signal]
    public delegate void inventoryUpdatedEventHandler(); // this is emitted when the inventory is updated (item added/removed)
}  
