using TestUrlFiller.PreMade;
using UrlFiller.Resolver;
using UrlFiller;
using Xunit; 

namespace TestUrlFiller;

public class UnitTest1
{
    const string FullUrl = "https://api.covalenthq.com/v1/[ChainId]/events/address/[ContractAddress]/?starting-block=[StartingBlock]&ending-block=[EndingBlock]&page-number=[PageNumber]&page-size=[MaxPageNumber]&key=[Key]";
    const string expexted = "https://api.covalenthq.com/v1/1/events/address/0x7Fc66500c84A76Ad7e9c93437bFc5Ac33E2DDaE9?starting-block=0&ending-block=99999999&page-number=0&page-size=99999999&key=ckey_1234567890";

    [Fact]
    public void Test1()
    {
        #region PreMadeCode
        // Create a DownloaderSettings instance
        var downloaderSettings = new DownloaderSettings
        {
            ChainId = "1",
            ContractAddress = "0x7Fc66500c84A76Ad7e9c93437bFc5Ac33E2DDaE9",
            StartingBlock = "0",
            MaxPageNumber = "99999999",
            Key = "ckey_1234567890",
            PageNumber = "0"
            // Set other properties...
        };

        var lastBlockDictionary = new Dictionary<string, string>();
        string chainSettings = "";
        #endregion PreMadeCode
        // Until this point, all this code is in the main program, and the following code is in the test project
        // Create some instances of your resolvers
        var downloaderSettingsResolver = new PropertyGetValueResolver(downloaderSettings);
        var endingBlockResolver = new FunctionCallValueResolver(input => JustFunction.EndingBlock(downloaderSettings, lastBlockDictionary, chainSettings));
         
        // Create a dictionary that maps parameter names to resolvers
        var valueResolvers = new Dictionary<string, IValueResolver>
        {
            ["ChainId"] = downloaderSettingsResolver,
            ["ContractAddress"] = downloaderSettingsResolver,
            ["StartingBlock"] = downloaderSettingsResolver,
            ["EndingBlock"] = endingBlockResolver,
            ["PageNumber"] = downloaderSettingsResolver,
            ["MaxPageNumber"] = downloaderSettingsResolver,  // Assuming MaxPageNumber is the same as PageNumber
            ["Key"] = downloaderSettingsResolver
        };

        // Create a URLParser and pass the dictionary to its constructor
        var parser = new URLParser(valueResolvers);

        // Use the URLParser to create the output URL
        var outputUrl = parser.GetOutputUrl(FullUrl);
        Assert.Equal(expexted, outputUrl);
    }
}