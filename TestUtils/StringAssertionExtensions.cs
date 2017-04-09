using System.Linq;

namespace TestUtils
{
  public static class StringAssertionExtensions
  {
    public static bool ContainsTerms(this string test, params string[] terms)
    {
      var normalized = test.ToLower();
      return terms.All(term => normalized.Contains(term.ToLower()));
    }
  }
}