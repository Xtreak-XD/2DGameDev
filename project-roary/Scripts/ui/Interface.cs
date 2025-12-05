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
    public SaveManager saveManager;
    public Player player;
    public Label Money;
    public Label Ammo;
    public Label time;
    public Label curDay;
    public Label temp;
    private Label moneyAmt;
    public MetaData playerMetaData;

    public Sprite2D sunMoonSprite;

    public TextureProgressBar health;
    public TextureProgressBar stamina;


    public override void _EnterTree()
    {
        eventbus = GetNode<Eventbus>("/root/Eventbus");
        saveManager = GetNode<SaveManager>("/root/SaveManager");
        eventbus.timeTick += setTime;
        eventbus.updateHealth += updateHealth;
        eventbus.updateStamina += updateStamina;
        eventbus.updateMoney += updateMoney;
        eventbus.updateAmmo += updateAmmo;
    }

    public override void _Ready()
    {
        player = (Player)GetTree().GetFirstNodeInGroup("player");
        Money = GetNode<Label>("HUD/playerinfo/HBoxContainer/VBoxContainer/HBoxContainer2/MoneyAmount");
        Ammo = GetNode<Label>("HUD/playerinfo/HBoxContainer/VBoxContainer/HBoxContainer/AmmoAmount"); 
        stamina = GetNode<TextureProgressBar>("HUD/playerinfo/HBoxContainer/VBoxContainer/Stamina");
        health = GetNode<TextureProgressBar>("HUD/playerinfo/HBoxContainer/VBoxContainer/Health");

        sunMoonSprite = GetNode<Sprite2D>("HUD/day cycle/PanelContainer/SubViewportContainer/SubViewport/Sprite2D");

        time = GetNode<Label>("HUD/day cycle/PanelContainer/cycleInfo/time");
        curDay = GetNode<Label>("HUD/day cycle/PanelContainer/cycleInfo/day");
        temp = GetNode<Label>("HUD/day cycle/PanelContainer/cycleInfo/temp");

        //setting default
        MetaData metadata = saveManager.GetMetaData();
        updateMoney(metadata.Money);
        updateAmmo(metadata.Ammo);
    }

    public void updateMoney(int value)
    {
        Money.Text = ": " + value;
    }
    public void updateAmmo(int value)
    {
       int ammo = Mathf.Clamp(value, 0, 50);
       Ammo.Text = "Ammo: " + ammo + "/50";
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

        float timeOfDay = (hour + (min / 60f)) / 24f;
        sunMoonSprite.Rotation = timeOfDay * Mathf.Tau;


        //setting time
        if (hour >= 12 && hour <= 23)
        {
            time.Text = hour.ToString() + ":" + min.ToString("D2") + " PM";
        }
        else
        {
            time.Text = hour.ToString() + ":" + min.ToString("D2") + " AM";
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

    public override void _ExitTree()
    {
        eventbus.timeTick -= setTime;

        eventbus.updateHealth -= updateHealth;
        eventbus.updateStamina -= updateStamina;
        eventbus.updateMoney -= updateMoney;
        eventbus.updateAmmo -= updateAmmo;
    }

    //use this to receive events for player input related to pause/and ui

}
