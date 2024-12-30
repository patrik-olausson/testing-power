namespace TestingPower.PowerMocking
{
    public enum AssertableTextDetailLevel { Verbose, Minimal, None }
    
    public static class AssertableTextDetailLevelExtensions
    {
        public static bool IsVerbose(this AssertableTextDetailLevel level) =>  level == AssertableTextDetailLevel.Verbose;
        public static bool IsMinimal(this AssertableTextDetailLevel level) =>  level == AssertableTextDetailLevel.Minimal;
        public static bool IsNone(this AssertableTextDetailLevel level) =>  level == AssertableTextDetailLevel.None;
    }
}