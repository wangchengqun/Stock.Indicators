namespace Skender.Stock.Indicators;

public static partial class Indicator
{
    // SIMPLE MOVING AVERAGE
    /// <include file='./info.xml' path='indicator/type[@name="Main"]/*' />
    ///
    public static IEnumerable<SmaResult> GetSma<TQuote>(
        this IEnumerable<TQuote> quotes,
        int lookbackPeriods,
        CandlePart candlePart = CandlePart.Close)
        where TQuote : IQuote
    {
        // check parameter arguments
        ValidateSma(lookbackPeriods);

        // initialize
        List<BasicData> bdList = quotes.ToBasicClass(candlePart);

        // calculate
        return bdList.CalcSma(lookbackPeriods);
    }

    // internals
    private static IEnumerable<SmaResult> CalcSma(
        this List<BasicData> bdList,
        int lookbackPeriods)
    {
        // note: pre-validated
        // initialize
        List<SmaResult> results = new(bdList.Count);

        // roll through quotes
        for (int i = 0; i < bdList.Count; i++)
        {
            BasicData q = bdList[i];

            SmaResult result = new()
            {
                Date = q.Date
            };

            if (i + 1 >= lookbackPeriods)
            {
                double sumSma = 0;
                for (int p = i + 1 - lookbackPeriods; p <= i; p++)
                {
                    BasicData d = bdList[p];
                    sumSma += d.Value;
                }

                result.Sma = (decimal?)sumSma / lookbackPeriods;
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
