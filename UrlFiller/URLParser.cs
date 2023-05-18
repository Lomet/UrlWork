using Flurl;
using UrlFiller.Resolver;

namespace UrlFiller
{
    public class URLParser
    {
        private readonly Dictionary<string, IValueResolver> valueResolvers;
        private readonly char[] chars = new char[] { '[', ']' };
        private bool NeedTrailingSlash { get; set; }

        public URLParser(Dictionary<string, IValueResolver> valueResolvers, bool trailing = false)
        {
            this.valueResolvers = valueResolvers;
            NeedTrailingSlash = trailing;
        }

        public Url GetOutputUrl(string Url) => GetOutputUrl(new Url(Url));

        public Url GetOutputUrl(Url url)
        {
            var result = new Url(url.Root)
               .AppendPathSegments(url.PathSegments.Select(GetRealValue))
               .SetQueryParams(url.QueryParams.Select(p =>
                   new KeyValuePair<string, string>(p.Name, GetRealValue(p.Value.ToString()))));
            if (NeedTrailingSlash)
                result = result.AppendPathSegment("/");
            return result;
        }

        private string GetRealValue(string? paramName)
        {
            if (paramName is null)
                return string.Empty;

            if (paramName.StartsWith(chars[0]) && paramName.EndsWith(chars[1]))
            {
                paramName = paramName.Trim(chars[0], chars[1]);
                if (valueResolvers.TryGetValue(paramName, out var resolver))
                    return resolver.GetValue(paramName);

                throw new Exception($"No value resolver found for parameter '{paramName}'");
            }

            return paramName;
        }
    }
}
