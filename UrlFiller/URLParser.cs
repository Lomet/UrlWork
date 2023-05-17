using Flurl;
using UrlFiller.Resolver;
using System.Linq;

namespace UrlFiller
{
    public class URLParser
    {
        private readonly Dictionary<string, IValueResolver> valueResolvers;

        public URLParser(Dictionary<string, IValueResolver> valueResolvers)
        {
            this.valueResolvers = valueResolvers;
        }

        public Url GetOutputUrl(string Url)
        {
            var inputUrl = new Url(Url);
            var outputUrl = new Url(inputUrl.Root);
            outputUrl.AppendPathSegments(inputUrl.PathSegments.Select(GetRealValue));
            outputUrl.SetQueryParams(inputUrl.QueryParams.Select(p => new KeyValuePair<string, string>(p.Name, GetRealValue(p.Value.ToString()))));
            return outputUrl;
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

                throw new Exception($"No value resolver found for parameter '{paramName}'");
            }

            return paramName;
        }
    }
}
