using TestUrlFiller.PreMade;
using UrlFiller.Resolver;
using UrlFiller;
using Xunit; 

namespace TestUrlFiller;

public class UnitTest1
{
    const string FullUrl = "https://api.covalenthq.com/v1/[ChainId]/events/address/[ContractAddress]/?starting-block=[StartingBlock]&ending-block=[EndingBlock]&page-number=[PageNumber]&page-size=[MaxPageNumber]&key=[Key]";
    const string expexted = "https://api.covalenthq.com/v1/1/events/address/0x7Fc66500c84A76Ad7e9c93437bFc5Ac33E2DDaE9?starting-block=0&ending-block=99999999&page-number=0&page-size=99999999&key=ckey_1234567890";
    const string expexted2 = "https://api.covalenthq.com/v1/1/events/address/0x7Fc66500c84A76Ad7e9c93437bFc5Ac33E2DDaE9/?starting-block=0&ending-block=99999999&page-number=0&page-size=99999999&key=ckey_1234567890";

    internal DownloaderSettings downloaderSettings { get; set; }
    internal Dictionary<string, string> lastBlockDictionary { get; set; }
    internal string chainSettings { get; set; }
    internal PropertyGetValueResolver downloaderSettingsResolver { get; set; }
    internal FunctionCallValueResolver endingBlockResolver { get; set; }
    internal Dictionary<string, IValueResolver> valueResolvers { get; set; }

    public UnitTest1()
    {
        #region PreMadeCode
        // Create a DownloaderSettings instance
        downloaderSettings = new DownloaderSettings
        {
            ChainId = "1",
            ContractAddress = "0x7Fc66500c84A76Ad7e9c93437bFc5Ac33E2DDaE9",
            StartingBlock = "0",
            MaxPageNumber = "99999999",
            Key = "ckey_1234567890",
            PageNumber = "0"
            // Set other properties...
        };
        lastBlockDictionary = new Dictionary<string, string>();
        chainSettings = "";
        #endregion PreMadeCode

        downloaderSettingsResolver = new PropertyGetValueResolver(downloaderSettings);
        endingBlockResolver = new FunctionCallValueResolver(input => JustFunction.EndingBlock(downloaderSettings, lastBlockDictionary, chainSettings));

        valueResolvers = new Dictionary<string, IValueResolver>
        {
            ["ChainId"] = downloaderSettingsResolver,
            ["ContractAddress"] = downloaderSettingsResolver,
            ["StartingBlock"] = downloaderSettingsResolver,
            ["EndingBlock"] = endingBlockResolver,
            ["PageNumber"] = downloaderSettingsResolver,
            ["MaxPageNumber"] = downloaderSettingsResolver,  // Assuming MaxPageNumber is the same as PageNumber
            ["Key"] = downloaderSettingsResolver
        };
    }

    [Fact]
    public void Test1()
    {
        // Create a URLParser and pass the dictionary to its constructor
        var parser = new URLParser(valueResolvers);

        // Use the URLParser to create the output URL
        var outputUrl = parser.GetOutputUrl(FullUrl);
        Assert.Equal(expexted, outputUrl);
    }
    [Fact] public void Test2()
    {
        // Create a URLParser and pass the dictionary to its constructor
        var parser = new URLParser(valueResolvers,true);

        // Use the URLParser to create the output URL
        var outputUrl = parser.GetOutputUrl(FullUrl);
        Assert.Equal(expexted2, outputUrl);
    }
}