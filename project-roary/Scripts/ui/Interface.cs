using Godot;
using System;


public partial class Interface : CanvasLayer
{
    public Eventbus eventbus;
    public Label time;
    public Label day;
    public Label temp;

    public override void _Ready()
    {
        eventbus = GetNode<Eventbus>("/root/Eventbus");
        eventbus.timeTick += setTime;

        time = GetNode<Label>("HUD/day cycle/PanelContainer/cycleInfo/time");
        day = GetNode<Label>("HUD/day cycle/PanelContainer/cycleInfo/day");
        temp = GetNode<Label>("HUD/day cycle/PanelContainer/cycleInfo/temp");
    }
    

    public void setTime(int day, int hour, int min)
    {
        if (hour >= 12 && hour <= 23)
        {
            time.Text = hour.ToString() + ":" + min.ToString() + " PM";
        }
        else
        {
            time.Text = hour.ToString() + ":" + min.ToString() + " AM";
        }
    }

    //use this to receive events for player input related to pause/and ui

}
