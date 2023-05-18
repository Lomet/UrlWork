using Flurl;
using UrlFiller.Resolver;

namespace UrlFiller
{
    public class URLParser
    {
        private readonly Dictionary<string, IValueResolver> valueResolvers;

        public URLParser(Dictionary<string, IValueResolver> valueResolvers)
        {
            this.valueResolvers = valueResolvers;
        }

        public Url GetOutputUrl(string Url) => GetParsedUrl(new Url(Url));
        
        private Url GetParsedUrl(Url url) => new Url(url.Root)
                .AppendPathSegments(url.PathSegments.Select(GetRealValue))
                .SetQueryParams(url.QueryParams.Select(p =>
                    new KeyValuePair<string, string>(p.Name, GetRealValue(p.Value.ToString()))));

        private string GetRealValue(string? paramName)
        {
            if (paramName is null)
                return string.Empty;

            if (paramName.StartsWith('[') && paramName.EndsWith(']'))
            {
                paramName = paramName.Trim('[', ']');
                if (valueResolvers.TryGetValue(paramName, out var resolver))
                    return resolver.GetValue(paramName);

                throw new Exception($"No value resolver found for parameter '{paramName}'");
            }

            return paramName;
        }
    }
}
