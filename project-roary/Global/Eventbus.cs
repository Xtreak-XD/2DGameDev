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

    [Signal] public delegate void beatRoaryEventHandler();

    [Signal] public delegate void deletedEventHandler();

    [Signal]
    public delegate void applyDamageEventHandler(Node dmgReceiverName, Node dmgDealerName, int dmg); //this is emitted by hitbox and used by hurtbox to pass dmg and information to deal dmg and apply effects.

    [Signal]
    public delegate void timeTickEventHandler(int day, int hour, int min, float temp); //this signal is used for ticking time, emitted by dayNightcycle script

    [Signal]
    public delegate void finishedDisplayingEventHandler(); //this is emitted by text boxes when a set of text is done displaying.

    //day night warning
    [Signal] public delegate void dayStartedEventHandler();
    [Signal] public delegate void nightStartedEventHandler();

    //UI Signals
    [Signal] public delegate void updateStaminaEventHandler(int value);
    [Signal] public delegate void updateHealthEventHandler(int value);
    [Signal] public delegate void updateMoneyEventHandler(int value);
    [Signal] public delegate void updateAmmoEventHandler(int value);
    [Signal] public delegate void updateBossHealthEventHandler(int value);

    //Inventory Signals
    [Signal] public delegate void inventoryUpdatedEventHandler(); //emitted when inventory changes
    [Signal] public delegate void itemDroppedEventHandler(IndividualItem item, int quantity); //emitted when player drops an item
    [Signal] public delegate void itemEquippedEventHandler(int slotIndex); //emitted when player equips an item from hotbar

    //effects
    [Signal] public delegate void hitStopEventHandler(float duration);
    [Signal] public delegate void screenShakeEventHandler(float intensity);
    [Signal] public delegate void knockBackEventHandler(CharacterBody2D target, float strength, Vector2 sourcePosition);
    [Signal] public delegate void triggerAttackEventHandler();

    // Interaction
    [Signal] public delegate void interactionCompleteEventHandler();

    // Shop Signals
    [Signal] public delegate void openShopMenuEventHandler(bool isOpen, ShopResource config);
    [Signal] public delegate void shopItemSelectedEventHandler(IndividualItem item);
    //save and load
    [Signal] public delegate void saveEventHandler(bool firstLoad = false);
    [Signal] public delegate void deleteSaveEventHandler();
    [Signal] public delegate void loadEventHandler();
    [Signal] public delegate void loadSettingsEventHandler();

    [Signal] public delegate void showSettingsEventHandler();
    [Signal] public delegate void leftSettingsEventHandler();

    // Audio
    [Signal] public delegate void sceneChangedEventHandler(string newSceneName);

    //transitions
    [Signal] public delegate void onTransitionFinishedEventHandler();
    [Signal] public delegate void DefeatedMermaidEventHandler();
}
