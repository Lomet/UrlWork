namespace UrlFiller.Resolver;

public class PropertyGetValueResolver : IValueResolver
{
    private object targetObject;

    public PropertyGetValueResolver(object targetObject)
    {
        this.targetObject = targetObject;
    }

    public string GetValue(string input)
    {
        var propertyInfo = targetObject.GetType().GetProperty(input);

        if (propertyInfo == null)
            throw new ArgumentException($"No property '{input}' found in object of type '{targetObject.GetType().Name}'");
        return propertyInfo.GetValue(targetObject)?.ToString() ?? string.Empty;
    }
}
