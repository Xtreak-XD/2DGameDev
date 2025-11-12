using Godot;
using System;
using System.Collections.Generic;


public partial class Interface : CanvasLayer
{
    Dictionary<int, string> days = new Dictionary<int, string>()
    {
        {0, "Monday"},
        {1, "Tuesday"},
        {2, "Wednesday"},
        {3, "Thursday"},
        {4, "Friday"},
        {5,"Saturday"},
        {6, "Sunday"}
    };
    public Eventbus eventbus;
    public Node player;
    public Label time;
    public Label curDay;
    public Label temp;

    public TextureProgressBar health;
    public TextureProgressBar stamina;

    public override void _Ready()
    {
        player = GetTree().GetFirstNodeInGroup("player");

        eventbus = GetNode<Eventbus>("/root/Eventbus");
        eventbus.timeTick += setTime;

        eventbus.updateHealth += updateHealth;
        eventbus.updateStamina += updateStamina;
        stamina = GetNode<TextureProgressBar>("HUD/playerinfo/HBoxContainer/VBoxContainer/Stamina");
        health = GetNode<TextureProgressBar>("HUD/playerinfo/HBoxContainer/VBoxContainer/Health");

        time = GetNode<Label>("HUD/day cycle/PanelContainer/cycleInfo/time");
        curDay = GetNode<Label>("HUD/day cycle/PanelContainer/cycleInfo/day");
        temp = GetNode<Label>("HUD/day cycle/PanelContainer/cycleInfo/temp");
    }


    public void updateStamina(int value)
    {
        stamina.Value = value;
    }
    
    public void updateHealth(int value)
    {
        health.Value = value;
    }

    public void setTime(int day, int hour, int min)
    {
        //setting time
        if (hour >= 12 && hour <= 23)
        {
            time.Text = hour.ToString() + ":" + min.ToString() + " PM";
        }
        else
        {
            time.Text = hour.ToString() + ":" + min.ToString() + " AM";
        }

        //setting day
        switch (day)
        {
            case 0:
                curDay.Text = days[0];
                break;
            case 1:
                curDay.Text = days[1];
                break;
            case 2:
                curDay.Text = days[2];
                break;
            case 3:
                curDay.Text = days[3];
                break;
            case 4:
                curDay.Text = days[4];
                break;
            case 5:
                curDay.Text = days[5];
                break;
            case 6:
                curDay.Text = days[6];
                break;
        }
    }

    //use this to receive events for player input related to pause/and ui

}
