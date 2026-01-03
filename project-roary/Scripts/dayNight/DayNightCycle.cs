using Godot;
using System;

public partial class DayNightCycle : CanvasModulate
{
    public double time = 0.0;
    public double pastMin = -1.0;
    const int MINUTES_PER_DAY = 1440;
    const int MIUNTES_PER_HOUR = 60;
    const float INGAME_TO_REAL_MINUTE_DURATION = 2 * Mathf.Pi / MINUTES_PER_DAY;
    public Eventbus eventbus;
    
    [Export]
    public GradientTexture1D gradient;
    
    [Export]
    public double INGAME_SPEED = 1.0;
    
    private int initialHour = 12;
    
    [Export]
    public int INITIAL_HOUR
    {
        get { return initialHour; }
        set
        {
            if (value > 24 || value < 0) return;
            initialHour = value;
        }
    }
    
    private bool isNight = false;
    
    public override void _Ready()
    {
        time = INGAME_TO_REAL_MINUTE_DURATION * INITIAL_HOUR * MIUNTES_PER_HOUR;
        eventbus = GetNode<Eventbus>("/root/Eventbus");
    }
    
    public override void _Process(double delta)
    {
        time += delta * INGAME_SPEED * INGAME_TO_REAL_MINUTE_DURATION;
        
        int total_mins = (int)(time / INGAME_TO_REAL_MINUTE_DURATION);
        int currentDayMins = total_mins % MINUTES_PER_DAY;
        int hour = currentDayMins / MIUNTES_PER_HOUR;
        int mins = currentDayMins % MIUNTES_PER_HOUR;
        
        double value = currentDayMins / (double)MINUTES_PER_DAY;
        
        if (gradient != null && gradient.Gradient != null)
        {
            Color newColor = gradient.Gradient.Sample((float)value);
            
            float darkenFactor = CalculateDarkenFactor(hour, mins);
            
            Color darkenedColor = newColor.Darkened(0.5f);
            Color = newColor.Lerp(darkenedColor, darkenFactor);
        }
        
        _recalculateTime();
        _checkDayNightTransition(hour);
    }

    private float CalculateDarkenFactor(int hour, int mins)
    {
        float totalMinutes = hour * 60 + mins;
        
        if (totalMinutes >= 300 && totalMinutes < 420)
        {
            return 0.5f - ((totalMinutes - 300) / 120.0f) * 0.5f;
        }
        else if (totalMinutes >= 420 && totalMinutes < 1080)
        {
            return 0.0f;
        }
        else if (totalMinutes >= 1080 && totalMinutes < 1200)
        {
            return ((totalMinutes - 1080) / 120.0f) * 0.5f;
        }
        else
        {
            return 0.5f; 
        }
    }
    
    private void _checkDayNightTransition(int hour)
    {
        bool nowNight = hour >= 20 || hour < 6;
        
        if (nowNight != isNight)
        {
            isNight = nowNight;
            if (isNight)
            {
                eventbus.EmitSignal("nightStarted");
            }
            else
            {
                eventbus.EmitSignal("dayStarted");
            }
        }
    }
    
    public void _recalculateTime()
    {
        int total_mins = (int)(time / INGAME_TO_REAL_MINUTE_DURATION);
        int day = (int)(total_mins / MINUTES_PER_DAY);
        int currentDayMins = total_mins % MINUTES_PER_DAY;
        int hour = currentDayMins / MIUNTES_PER_HOUR;
        int mins = currentDayMins % MIUNTES_PER_HOUR;
        
        if(day >= 7){ day = day % 7; }
        
        if (pastMin != mins)
        {
            pastMin = mins;
            eventbus.EmitSignal("timeTick", day, hour, mins, CalculateTemperature(hour, day));
        }
    }
    
    public float CalculateTemperature(int hour, int day)
    {
        float baseTemp = 20f;
        float dailyVariation = Mathf.Cos((float)((hour - 14) * Mathf.Pi / 12.0f)) * 10f;
        float weeklyVariation = Mathf.Sin((float)(day * Mathf.Pi / 3.5f)) * 3f;
        float randomNoise = (float)(GD.Randf() * 2 - 1);
        float tempC = baseTemp + dailyVariation + weeklyVariation + randomNoise;
        float tempF = (tempC * 9f / 5f) + 32f;
        return tempF;
    }
}