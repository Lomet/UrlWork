using TestUrlFiller.PreMade;
using UrlFiller.Resolver;
using UrlFiller;
using Xunit;

namespace TestUrlFiller;

public class URLParserTests
{
    private const string InputUrlWithPlaceholders = "https://api.covalenthq.com/v1/[ChainId]/events/address/[ContractAddress]/?starting-block=[StartingBlock]&ending-block=[EndingBlock]&page-number=[PageNumber]&page-size=[MaxPageNumber]&key=[Key]";
    private const string ExpectedUrl = "https://api.covalenthq.com/v1/1/events/address/0x7Fc66500c84A76Ad7e9c93437bFc5Ac33E2DDaE9?starting-block=0&ending-block=99999999&page-number=0&page-size=99999999&key=ckey_1234567890";
    private const string ExpectedUrlWithTrailingSlash = "https://api.covalenthq.com/v1/1/events/address/0x7Fc66500c84A76Ad7e9c93437bFc5Ac33E2DDaE9/?starting-block=0&ending-block=99999999&page-number=0&page-size=99999999&key=ckey_1234567890";

    private DownloaderSettings DownloaderSettings { get; set; }
    private Dictionary<string, string> LastBlockDictionary { get; set; }
    private string ChainSettings { get; set; }
    private PropertyGetValueResolver DownloaderSettingsResolver { get; set; }
    private FunctionCallValueResolver EndingBlockResolver { get; set; }
    private Dictionary<string, IValueResolver> ValueResolvers { get; set; }

    public URLParserTests()
    {
        // Create a DownloaderSettings instance
        DownloaderSettings = new DownloaderSettings
        {
            ChainId = "1",
            ContractAddress = "0x7Fc66500c84A76Ad7e9c93437bFc5Ac33E2DDaE9",
            StartingBlock = "0",
            MaxPageNumber = "99999999",
            Key = "ckey_1234567890",
            PageNumber = "0"
        };
        LastBlockDictionary = new Dictionary<string, string>();
        ChainSettings = "";

        DownloaderSettingsResolver = new PropertyGetValueResolver(DownloaderSettings);
        EndingBlockResolver = new FunctionCallValueResolver(input => JustFunction.EndingBlock(DownloaderSettings, LastBlockDictionary, ChainSettings));

        ValueResolvers = new Dictionary<string, IValueResolver>
        {
            ["ChainId"] = DownloaderSettingsResolver,
            ["ContractAddress"] = DownloaderSettingsResolver,
            ["StartingBlock"] = DownloaderSettingsResolver,
            ["EndingBlock"] = EndingBlockResolver,
            ["PageNumber"] = DownloaderSettingsResolver,
            ["MaxPageNumber"] = DownloaderSettingsResolver,
            ["Key"] = DownloaderSettingsResolver
        };
    }

    [Fact]
    public void ParseUrl_ShouldReplacePlaceholders_WhenNoTrailingSlashNeeded()
    {
        var outputUrl = new URLParser(ValueResolvers).ParseUrl(InputUrlWithPlaceholders);

        Assert.Equal(ExpectedUrl, outputUrl);
    }

    [Fact]
    public void ParseUrl_ShouldReplacePlaceholdersAndAddTrailingSlash_WhenTrailingSlashNeeded()
    {
        var outputUrl = new URLParser(ValueResolvers, true).ParseUrl(InputUrlWithPlaceholders);

        Assert.Equal(ExpectedUrlWithTrailingSlash, outputUrl);
    }
    [Fact]
    public void TestValueResolverNotFound()
    {
        var parser = new URLParser(ValueResolvers);
        string urlWithInvalidPlaceholder = "https://example.com/[InvalidPlaceholder]";

        Exception ex = Assert.Throws<KeyNotFoundException>(() => parser.ParseUrl(urlWithInvalidPlaceholder));

        Assert.Equal($"InvalidPlaceholder Not Found in valueResolvers", ex.Message);
    }

    [Fact]
    public void TestEncapsulationMarkersParentheses()
    {
        var customEncapsulationMarkers = new char[] { '(', ')' };
        var customUrl = "https://api.covalenthq.com/v1/(ChainId)/events/address/(ContractAddress)?starting-block=(StartingBlock)&ending-block=(EndingBlock)&page-number=(PageNumber)&page-size=(MaxPageNumber)&key=(Key)";

        var outputUrl = new URLParser(ValueResolvers, false, customEncapsulationMarkers).ParseUrl(customUrl);

        Assert.Equal(ExpectedUrl, outputUrl);
    }


}
