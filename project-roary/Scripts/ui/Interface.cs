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
    private Label moneyAmt;
    public MetaData playerMetaData;

    public TextureProgressBar health;
    public TextureProgressBar stamina;

    public override void _EnterTree()
    {
        eventbus = GetNode<Eventbus>("/root/Eventbus");
        playerMetaData = ResourceLoader.Load<MetaData>("res://Resources/entities/player/playerMetaData.tres");

        eventbus.timeTick += setTime;
        eventbus.updateHealth += updateHealth;
        eventbus.updateStamina += updateStamina;
        eventbus.updateMoneyDisplay += updateMoneyDisplay;
    }

    public override void _Ready()
    {
        player = GetTree().GetFirstNodeInGroup("player");
        stamina = GetNode<TextureProgressBar>("%Stamina");
        health = GetNode<TextureProgressBar>("%Health");
        moneyAmt = GetNode<Label>("%MoneyAmount");

        time = GetNode<Label>("%time");
        curDay = GetNode<Label>("%day");
        temp = GetNode<Label>("%temp");
    }


    private void updateStamina(int value)
    {
        stamina.Value = value;
    }
    
    private void updateHealth(int value)
    {
        health.Value = value;
    }

    private void setTime(int day, int hour, int min, float temp)
    {
        //set temp in c
        this.temp.Text = ((int)temp).ToString() + "°F";
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

    private void updateMoneyDisplay()
    {
        if (moneyAmt != null && player != null)
        {
            moneyAmt.Text = playerMetaData.Money.ToString();
        }
    }

    public override void _ExitTree()
    {
        eventbus.timeTick -= setTime;

        eventbus.updateHealth -= updateHealth;
        eventbus.updateStamina -= updateStamina;
    }

    //use this to receive events for player input related to pause/and ui

}
