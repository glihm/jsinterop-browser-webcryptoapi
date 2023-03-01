namespace JSInterop.Browser.TestApp.Units;

/// <summary>
/// 
/// </summary>
public class UnitTestResult
{
    /// <summary>
    /// 
    /// </summary>
    public bool HasPassed { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string Reason { get; set; } = String.Empty;

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static UnitTestResult
    Passed()
    {
        return new UnitTestResult()
        {
            HasPassed = true
        };
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="reason"></param>
    /// <returns></returns>
    public static UnitTestResult
    Failed(String? reason = null)
    {
        return new UnitTestResult()
        {
            HasPassed = false,
            Reason = reason ?? String.Empty,
        };
    }

}
