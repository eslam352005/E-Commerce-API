namespace Shared.Model_Errors
{
    public class ValidationError
    {
        public string Field { get; set; } = null!;
        public IEnumerable<string> Errors { get; set; } = [];
    }
}