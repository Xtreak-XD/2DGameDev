using Godot;
using System;
using System.Security.Cryptography.X509Certificates;

public partial class DayNightCycle : CanvasModulate
{
    public double time = 0.0;
    public double pastMin = -1.0;
    const int MINUTES_PER_DAY = 1440;
    const int MIUNTES_PER_HOUR = 60;
    const float INGAME_TO_REAL_MINUTE_DURATION = (2 * Mathf.Pi) / MINUTES_PER_DAY;

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
            if (value > 24 || value < 0)
            {
                return;
            }
            initialHour = value;
        }
        
    }

    public override void _Ready()
    {
        time = INGAME_TO_REAL_MINUTE_DURATION * INITIAL_HOUR * MIUNTES_PER_HOUR;
        eventbus = GetNode<Eventbus>("/root/Eventbus");
    }

    public override void _Process(double delta)
    {
        time += delta * INGAME_SPEED * INGAME_TO_REAL_MINUTE_DURATION;
        double value = (Mathf.Sin(time - Mathf.Pi) + 1.0) / 2.0;
        Color = gradient.Gradient.Sample((float)value);

        _recalculateTime();
    }

    public void _recalculateTime()
    {
        int total_mins = (int)(time / INGAME_TO_REAL_MINUTE_DURATION);

        int day = (int)(total_mins / MINUTES_PER_DAY);
        int currentDayMins = total_mins % MINUTES_PER_DAY;
        var hour = (int)(currentDayMins / MIUNTES_PER_HOUR);
        var mins = (int)(currentDayMins % MIUNTES_PER_HOUR);

        if(day == 7){ day = 0; }
        if (pastMin != mins)
        {
            pastMin = mins;
            eventbus.EmitSignal("timeTick", day, hour, mins, CalculateTemperature(hour, day));
        }
        
    }
    public float CalculateTemperature(int hour, int day)
    {
        float baseTemp = 20f; //avr temp in Celcius

        // Warmest around 14:00, coldest around 5:00
        float dailyVariation = Mathf.Cos((float)((hour - 14) * Mathf.Pi / 12.0f)) * 10f;

        float weeklyVariation = Mathf.Sin((float)(day * Mathf.Pi / 3.5f)) * 3f;

        float randomNoise = (float)(GD.Randf() * 2 - 1);

        float tempC = baseTemp + dailyVariation + weeklyVariation + randomNoise;

        float tempF = (tempC * 9f / 5f) + 32f;

        return tempF;
    }


}
