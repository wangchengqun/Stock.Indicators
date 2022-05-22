using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skender.Stock.Indicators;

namespace Internal.Tests;

[TestClass]
public class EmaTests : TestBase
{
    [TestMethod]
    public void Standard()
    {
        List<EmaResult> results = quotes.GetEma(20)
            .ToList();

        // assertions

        // proper quantities
        // should always be the same number of results as there is quotes
        Assert.AreEqual(502, results.Count);
        Assert.AreEqual(483, results.Where(x => x.Ema != null).Count());

        // sample values
        EmaResult r29 = results[29];
        Assert.AreEqual(216.6228, NullMath.Round(r29.Ema, 4));

        EmaResult r249 = results[249];
        Assert.AreEqual(255.3873, NullMath.Round(r249.Ema, 4));

        EmaResult r501 = results[501];
        Assert.AreEqual(249.3519, NullMath.Round(r501.Ema, 4));
    }

    [TestMethod]
    public void Stream()
    {
        List<Quote> quotesList = quotes
            .OrderBy(x => x.Date)
            .ToList();

        // time-series
        List<EmaResult> series = quotesList.GetEma(20).ToList();

        // stream simulation
        Ema emaBase = quotesList.Take(25).InitEma(20);

        for (int i = 25; i < series.Count; i++)
        {
            Quote q = quotesList[i];
            emaBase.Add(q);
            emaBase.Add(q); // redundant
        }

        List<EmaResult> stream = emaBase.Results.ToList();

        // assertions
        for (int i = 0; i < series.Count; i++)
        {
            EmaResult t = series[i];
            EmaResult s = stream[i];

            Assert.AreEqual(t.Date, s.Date);
            Assert.AreEqual(t.Ema, s.Ema);
        }
    }

    [TestMethod]
    public void Chaining()
    {
        List<EmaResult> results = quotes
            .GetRsi(14)
            .GetEma(20)
            .ToList();

        // assertions
        Assert.AreEqual(488, results.Count);
        Assert.AreEqual(469, results.Where(x => x.Ema != null).Count());

        // sample values
        EmaResult r18 = results[18];
        Assert.IsNull(r18.Ema);

        EmaResult r19 = results[19];
        Assert.AreEqual(67.4565, NullMath.Round(r19.Ema, 4));

        EmaResult r235 = results[235];
        Assert.AreEqual(70.4659, NullMath.Round(r235.Ema, 4));

        EmaResult r487 = results[487];
        Assert.AreEqual(37.0728, NullMath.Round(r487.Ema, 4));
    }

    [TestMethod]
    public void Custom()
    {
        List<EmaResult> results = quotes
            .Use(CandlePart.Open)
            .GetEma(20)
            .ToList();

        // assertions

        // proper quantities
        // should always be the same number of results as there is quotes
        Assert.AreEqual(502, results.Count);
        Assert.AreEqual(483, results.Where(x => x.Ema != null).Count());

        // sample values
        EmaResult r29 = results[29];
        Assert.AreEqual(216.2643, NullMath.Round(r29.Ema, 4));

        EmaResult r249 = results[249];
        Assert.AreEqual(255.4875, NullMath.Round(r249.Ema, 4));

        EmaResult r501 = results[501];
        Assert.AreEqual(249.9157, NullMath.Round(r501.Ema, 4));
    }

    [TestMethod]
    public void BadData()
    {
        IEnumerable<EmaResult> r = Indicator.GetEma(badQuotes, 15);
        Assert.AreEqual(502, r.Count());
    }

    [TestMethod]
    public void NoQuotes()
    {
        IEnumerable<EmaResult> r0 = noquotes.GetEma(10);
        Assert.AreEqual(0, r0.Count());

        IEnumerable<EmaResult> r1 = onequote.GetEma(10);
        Assert.AreEqual(1, r1.Count());
    }

    [TestMethod]
    public void Removed()
    {
        List<EmaResult> results = quotes.GetEma(20)
            .RemoveWarmupPeriods()
            .ToList();

        // assertions
        Assert.AreEqual(502 - (20 + 100), results.Count);

        EmaResult last = results.LastOrDefault();
        Assert.AreEqual(249.3519, NullMath.Round(last.Ema, 4));
    }

    [TestMethod]
    public void Exceptions()
    {
        // bad lookback period
        Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            Indicator.GetEma(quotes, 0));
    }
}
