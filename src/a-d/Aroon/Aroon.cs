namespace Skender.Stock.Indicators;

public static partial class Indicator
{
    // AROON OSCILLATOR
    /// <include file='./info.xml' path='indicator/*' />
    ///
    public static IEnumerable<AroonResult> GetAroon<TQuote>(
        this IEnumerable<TQuote> quotes,
        int lookbackPeriods = 25)
        where TQuote : IQuote
    {
        // convert quotes
        List<QuoteD> quotesList = quotes.ToQuoteD();

        // check parameter arguments
        ValidateAroon(lookbackPeriods);

        // initialize
        List<AroonResult> results = new(quotesList.Count);

        // roll through quotes
        for (int i = 0; i < quotesList.Count; i++)
        {
            QuoteD q = quotesList[i];

            AroonResult result = new()
            {
                Date = q.Date
            };

            // add aroons
            if (i + 1 > lookbackPeriods)
            {
                double? lastHighPrice = 0;
                double? lastLowPrice = double.MaxValue;
                int lastHighIndex = 0;
                int lastLowIndex = 0;

                for (int p = i + 1 - lookbackPeriods - 1; p <= i; p++)
                {
                    QuoteD d = quotesList[p];

                    if (d.High > lastHighPrice)
                    {
                        lastHighPrice = d.High;
                        lastHighIndex = p + 1;
                    }

                    if (d.Low < lastLowPrice)
                    {
                        lastLowPrice = d.Low;
                        lastLowIndex = p + 1;
                    }
                }

                result.AroonUp = 100d * (lookbackPeriods - (i + 1 - lastHighIndex)) / lookbackPeriods;
                result.AroonDown = 100d * (lookbackPeriods - (i + 1 - lastLowIndex)) / lookbackPeriods;
                result.Oscillator = result.AroonUp - result.AroonDown;
            }

            results.Add(result);
        }

        return results;
    }

    // parameter validation
    private static void ValidateAroon(
        int lookbackPeriods)
    {
        // check parameter arguments
        if (lookbackPeriods <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(lookbackPeriods), lookbackPeriods,
                "Lookback periods must be greater than 0 for Aroon.");
        }
    }
}
