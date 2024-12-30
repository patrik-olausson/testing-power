namespace TestingPower.PowerMocking
{
    public interface ICapturedCallInfo
    {
        string Id { get; }
        
        string ToAssertableText(AssertableTextDetailLevel detailLevel);
    }
}