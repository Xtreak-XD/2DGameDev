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
    }

    public override void _Process(double delta)
    {
        eventbus = GetNode<Eventbus>("/root/Eventbus");
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
            eventbus.EmitSignal("timeTick", day, hour, mins);
        }
        
    }

}
