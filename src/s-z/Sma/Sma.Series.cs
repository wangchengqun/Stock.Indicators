namespace Skender.Stock.Indicators;

// SIMPLE MOVING AVERAGE (SERIES)
public static partial class Indicator
{
    // series calculation
    internal static IEnumerable<SmaResult> CalcSma(
        this List<(DateTime, double)> tpList,
        int lookbackPeriods)
    {
        // check parameter arguments
        ValidateSma(lookbackPeriods);

        // initialize
        List<SmaResult> results = new(tpList.Count);

        // roll through quotes
        for (int i = 0; i < tpList.Count; i++)
        {
            (DateTime date, double value) = tpList[i];

            SmaResult result = new()
            {
                Date = date
            };

            if (i + 1 >= lookbackPeriods)
            {
                double sumSma = 0;
                for (int p = i + 1 - lookbackPeriods; p <= i; p++)
                {
                    (DateTime pDate, double pValue) = tpList[p];
                    sumSma += pValue;
                }

                result.Sma = sumSma / lookbackPeriods;
            }

            results.Add(result);
        }

        return results;
    }

    // parameter validation
    private static void ValidateSma(
        int lookbackPeriods)
    {
        // check parameter arguments
        if (lookbackPeriods <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(lookbackPeriods), lookbackPeriods,
                "Lookback periods must be greater than 0 for SMA.");
        }
    }
}
