using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
public partial class InteractionManager : Node2D
{
    public Player player;
    public dialogueManager dialogueManager;
    public Label label;

    const string labelText = "[E] to ";

    public List<interactionArea> activeAreas = new List<interactionArea>(); //this will how  overlapping areas in case there are multiple interacts in 1 place.
    public bool canInteract = true;
    private Eventbus eventbus;

    public override void _Ready()
    {
        dialogueManager = GetNode<dialogueManager>("/root/DialogueManager");
        label = GetNode<Label>("Label");
        
		eventbus = GetNode<Eventbus>("/root/Eventbus");
        eventbus.interactionComplete += () => { canInteract = true; };
    }

    public override void _Process(double delta)
    {
        if (player == null || !IsInstanceValid(player))
        {
            player = (Player)GetTree().GetFirstNodeInGroup("player");
            
            if (player == null) return; 
        }

        activeAreas.RemoveAll(area => !IsInstanceValid(area)); // Clean up invalid areas

        if (activeAreas.Count() > 0 && canInteract)
        {
            activeAreas.Sort(SortByDistanceToPlayer);
            
            label.Text = labelText + activeAreas[0].actionName;
            label.GlobalPosition = activeAreas[0].GlobalPosition + new Vector2(-(label.Size.X / 2), -300);
            label.Show();
        }
        else
        {
            label.Hide();
        }
    }

    private int SortByDistanceToPlayer(interactionArea area1, interactionArea area2)
    {
        double d1 = player.GlobalPosition.DistanceTo(area1.GlobalPosition);
        double d2 = player.GlobalPosition.DistanceTo(area2.GlobalPosition);
        return d1.CompareTo(d2);
    }

    public void registerArea(interactionArea area)
    {
        activeAreas.Add(area);
    }

    public void unregisterArea(interactionArea area)
    {
        if(dialogueManager.isDialogActive)
        {
            dialogueManager.HandleDialogAdvance(true);
        }
        
        activeAreas.Remove(area);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("interact") && canInteract)
        {
            if (activeAreas.Count() > 0)
            {
                canInteract = false;
                label.Hide();
                activeAreas[0].interact.Call();
            }
        }
    }


}
