using Flurl;
using UrlFiller.Resolver;

namespace UrlFiller;

public class URLParser
{
    private readonly Dictionary<string, IValueResolver> valueResolvers;
    private readonly char[] encapsulationMarkers = new char[] { '[', ']' };
    private bool IncludeTrailingSlash { get; set; }

    public URLParser(Dictionary<string, IValueResolver> valueResolvers, bool includeTrailingSlash = false, char[]? encapsulationMarkers = null)
    {
        if (encapsulationMarkers is not null)
            this.encapsulationMarkers = encapsulationMarkers;
        this.valueResolvers = valueResolvers;
        IncludeTrailingSlash = includeTrailingSlash;
    }

    public Url ParseUrl(string urlString) => ParseUrl(new Url(urlString));

    public Url ParseUrl(Url url) => new Url(url.Root)
       .AppendPathSegments(url.PathSegments.Select(ResolveParameterValue))
       .SetQueryParams(url.QueryParams.ToDictionary(p => p.Name, p => ResolveParameterValue(p.Value.ToString())))
       .AppendPathSegment(IncludeTrailingSlash ? "/" : string.Empty);

    private string ResolveParameterValue(string? paramName) =>
        string.IsNullOrEmpty(paramName) ? string.Empty : ExtractAndResolveParam(paramName!);

    private string ResolveEncapsulatedParam(string cleanParamName) =>
         valueResolvers.TryGetValue(cleanParamName, out var resolver)
            ? resolver.GetValue(cleanParamName)
            : throw new Exception($"No value resolver found for parameter '{cleanParamName}'");

    private string ExtractCleanValue(string encapsulatedValue) => encapsulatedValue.Trim(encapsulationMarkers[0], encapsulationMarkers[1]);

    private bool IsEncapsulated(string value) => value.StartsWith(encapsulationMarkers[0]) && value.EndsWith(encapsulationMarkers[1]);

    private string ExtractAndResolveParam(string paramName) => !IsEncapsulated(paramName)
            ? paramName
            : ResolveEncapsulatedParam(ExtractCleanValue(paramName));

}