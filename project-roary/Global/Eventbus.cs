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
    [Signal]
    public delegate void applyDamageEventHandler(string dmgReceiverName,string dmgDealerName, int dmg); //this is emitted by hitbox and used by hurtbox to pass dmg and information to deal dmg and apply effects.
}
