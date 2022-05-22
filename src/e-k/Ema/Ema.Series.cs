namespace Skender.Stock.Indicators;

// EXPONENTIAL MOVING AVERAGE (SERIES)
public static partial class Indicator
{
    // series calculation
    internal static List<EmaResult> CalcEma(
        this List<(DateTime, double)> tpList,
        int lookbackPeriods)
    {
        // check parameter arguments
        ValidateEma(lookbackPeriods);

        // initialize
        int length = tpList.Count;
        List<EmaResult> results = new(length);

        double lastEma = 0;
        double k = 2d / (lookbackPeriods + 1);
        int initPeriods = Math.Min(lookbackPeriods, length);

        for (int i = 0; i < initPeriods; i++)
        {
            (DateTime date, double value) = tpList[i];
            lastEma += value;
        }

        lastEma /= lookbackPeriods;

        // roll through quotes
        for (int i = 0; i < length; i++)
        {
            (DateTime date, double value) = tpList[i];
            EmaResult r = new() { Date = date };

            if (i + 1 > lookbackPeriods)
            {
                double ema = Ema.Increment(value, lastEma, k);
                r.Ema = ema;
                lastEma = ema;
            }
            else if (i == lookbackPeriods - 1)
            {
                r.Ema = lastEma;
            }

            results.Add(r);
        }

        return results;
    }

    // parameter validation
    private static void ValidateEma(
        int lookbackPeriods)
    {
        // check parameter arguments
        if (lookbackPeriods <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(lookbackPeriods), lookbackPeriods,
                "Lookback periods must be greater than 0 for EMA.");
        }
    }
}
