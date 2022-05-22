namespace Skender.Stock.Indicators;

// EXPONENTIAL MOVING AVERAGE (API)
public static partial class Indicator
{
    // SERIES, from TQuote
    /// <include file='./info.xml' path='info/type[@name="standard"]/*' />
    ///
    public static IEnumerable<EmaResult> GetEma<TQuote>(
        this IEnumerable<TQuote> quotes,
        int lookbackPeriods)
        where TQuote : IQuote
    {
        // convert quotes
        List<(DateTime, double)> tpList
            = quotes.ToBasicTuple(CandlePart.Close);

        // calculate
        return tpList.CalcEma(lookbackPeriods);
    }

    // SERIES, from CHAIN
    public static IEnumerable<EmaResult> GetEma(
        this IEnumerable<IReusableResult> results,
        int lookbackPeriods)
    {
        // convert results
        List<(DateTime, double)> tpList
            = results.ToResultTuple();

        // calculate
        return tpList.CalcEma(lookbackPeriods);
    }

    // SERIES, from TUPLE
    public static IEnumerable<EmaResult> GetEma(
        this IEnumerable<(DateTime, double)> priceTuples,
        int lookbackPeriods)
    {
        // convert prices
        List<(DateTime, double)> tpList
            = priceTuples.ToTupleList();

        // calculate
        return tpList.CalcEma(lookbackPeriods);
    }

    // STREAM INITIALIZATION, from TQuote
    /// <include file='./info.xml' path='info/type[@name="stream"]/*' />
    ///
    public static Ema InitEma<TQuote>(
        this IEnumerable<TQuote> quotes,
        int lookbackPeriods)
        where TQuote : IQuote
    {
        // convert quotes
        List<(DateTime, double)> tpList
            = quotes.ToBasicTuple(CandlePart.Close);

        return new Ema(tpList, lookbackPeriods);
    }

    // STREAM INITIALIZATION, from CHAIN
    public static Ema InitEma(
        this IEnumerable<IReusableResult> results,
        int lookbackPeriods)
    {
        // convert results
        List<(DateTime, double)> tpList
            = results.ToResultTuple();

        return new Ema(tpList, lookbackPeriods);
    }
}
