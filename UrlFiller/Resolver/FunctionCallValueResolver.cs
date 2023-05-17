namespace UrlFiller.Resolver;

public class FunctionCallValueResolver : IValueResolver
{
    private Func<string, string> function;

    public FunctionCallValueResolver(Func<string, string> function)
    {
        this.function = function;
    }

    public string GetValue(string input)
    {
        return function(input);
    }
}
