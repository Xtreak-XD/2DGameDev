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
    public delegate void applyDamageEventHandler(Node dmgReceiverName, Node dmgDealerName, int dmg); //this is emitted by hitbox and used by hurtbox to pass dmg and information to deal dmg and apply effects.

    [Signal]
    public delegate void timeTickEventHandler(int day, int hour, int min, float temp); //this signal is used for ticking time, emitted by dayNightcycle script

    [Signal]
    public delegate void finishedDisplayingEventHandler(); //this is emitted by text boxes when a set of text is done displaying.

    [Signal]
    public delegate void inventoryUpdatedEventHandler(); // this is emitted when the inventory is updated (item added/removed)

    [Signal]
    public delegate void itemDroppedEventHandler(InventoryItem item, int quantity);
    [Signal]
    public delegate void itemEquippedEventHandler(int slotIndex);

    [Signal] public delegate void updateStaminaEventHandler(int value);
    [Signal] public delegate void updateHealthEventHandler(int value);

    //effects
    [Signal] public delegate void hitStopEventHandler(float duration);
    [Signal] public delegate void screenShakeEventHandler(float intensity);
    [Signal] public delegate void knockBackEventHandler(CharacterBody2D target, float strength, Vector2 sourcePosition);
    [Signal] public delegate void triggerAttackEventHandler();
}  
