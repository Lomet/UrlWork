namespace UrlFiller.Resolver;

public class PropertyGetValueResolver : IValueResolver
{
    private readonly object targetObject;

    public PropertyGetValueResolver(object targetObject)
    {
        this.targetObject = targetObject;
    }

    public string GetValue(string input)
    {
        var propertyInfo = targetObject.GetType().GetProperty(input);

        return propertyInfo == null
            ? throw new ArgumentException($"No property '{input}' found in object of type '{targetObject.GetType().Name}'")
            : propertyInfo.GetValue(targetObject)?.ToString() ?? string.Empty;
    }
}
