using Flurl;
using UrlFiller.Resolver;

namespace UrlFiller;

public class URLParser
{
    private Url InputUrl { get; }
    private Dictionary<string, IValueResolver> valueResolvers;

    public URLParser(string inputUrl, Dictionary<string, IValueResolver> valueResolvers)
    {
        InputUrl = new(inputUrl);
        this.valueResolvers = valueResolvers;
    }
    public Url GetOutputUrl()
    {
        var outputUrl = new Url(InputUrl);
        outputUrl.ResetToRoot();
        outputUrl.AppendPathSegments(GetPathParams());
        outputUrl.SetQueryParams(GetQueryParams());
        return outputUrl;
    }

    private string[] GetPathParams()
    {
        var pathParams = InputUrl.PathSegments.ToList();
        var newPathParams = new List<string>();
        foreach (var item in pathParams)
        newPathParams.Add(GetRealValue(item));
        return newPathParams.ToArray();
    }

    private Dictionary<string, string> GetQueryParams()
    {
        var NewQueryParams = new Dictionary<string, string>();
        foreach (var item in InputUrl.QueryParams)
            NewQueryParams.Add(item.Name, GetRealValue(item.Value.ToString()));
        return NewQueryParams;
    }
    private string GetRealValue(string? paramName)
    {
        if (paramName is null)
            return string.Empty;
        if (paramName.StartsWith('[') && paramName.EndsWith(']'))
        {
            paramName = paramName.Trim('[', ']');
            if (valueResolvers.TryGetValue(paramName, out var resolver))
                return resolver.GetValue(paramName);
            else
                throw new Exception($"No value resolver found for parameter '{paramName}'");
        }
        return paramName;
    }
}
