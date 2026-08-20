namespace Roton.Emulation.Data;

public static class TimerExtensions
{
    public static int Clock(this ITimer timer, int amount, int interval)
    {
        var value = timer.Ticks + amount;
        var result = 0;

        while (value >= interval)
        {
            value -= interval;
            result++;
        }

        timer.Ticks = value;
        return result;
    }
}