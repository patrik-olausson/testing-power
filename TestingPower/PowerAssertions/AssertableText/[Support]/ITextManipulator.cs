namespace TestingPower.PowerMocking
{
    public interface ITextManipulator
    {
        string CallId { get; }
        
        string ManipulateText(string originalText);
    }
}