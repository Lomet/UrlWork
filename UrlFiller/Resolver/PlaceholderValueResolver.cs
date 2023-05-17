namespace UrlFiller.Resolver;

public class PlaceholderValueResolver : IValueResolver
{
    public string GetValue(string input)
    {
        // For now, just return the input as is
        return input;
    }
}
