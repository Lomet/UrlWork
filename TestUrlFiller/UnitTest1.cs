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

    internal DownloaderSettings DownloaderSettings { get; set; }
    internal Dictionary<string, string> LastBlockDictionary { get; set; }
    internal string ChainSettings { get; set; }
    internal PropertyGetValueResolver DownloaderSettingsResolver { get; set; }
    internal FunctionCallValueResolver EndingBlockResolver { get; set; }
    internal Dictionary<string, IValueResolver> ValueResolvers { get; set; }

    public UnitTest1()
    {
        #region PreMadeCode
        // Create a DownloaderSettings instance
        DownloaderSettings = new DownloaderSettings
        {
            ChainId = "1",
            ContractAddress = "0x7Fc66500c84A76Ad7e9c93437bFc5Ac33E2DDaE9",
            StartingBlock = "0",
            MaxPageNumber = "99999999",
            Key = "ckey_1234567890",
            PageNumber = "0"
            // Set other properties...
        };
        LastBlockDictionary = new Dictionary<string, string>();
        ChainSettings = "";
        #endregion PreMadeCode

        DownloaderSettingsResolver = new PropertyGetValueResolver(DownloaderSettings);
        EndingBlockResolver = new FunctionCallValueResolver(input => JustFunction.EndingBlock(DownloaderSettings, LastBlockDictionary, ChainSettings));

        ValueResolvers = new Dictionary<string, IValueResolver>
        {
            ["ChainId"] = DownloaderSettingsResolver,
            ["ContractAddress"] = DownloaderSettingsResolver,
            ["StartingBlock"] = DownloaderSettingsResolver,
            ["EndingBlock"] = EndingBlockResolver,
            ["PageNumber"] = DownloaderSettingsResolver,
            ["MaxPageNumber"] = DownloaderSettingsResolver,  // Assuming MaxPageNumber is the same as PageNumber
            ["Key"] = DownloaderSettingsResolver
        };
    }

    [Fact]
    public void Test1()
    {
        // Create a URLParser and pass the dictionary to its constructor
        var parser = new URLParser(ValueResolvers);

        // Use the URLParser to create the output URL
        var outputUrl = parser.GetOutputUrl(FullUrl);
        Assert.Equal(expexted, outputUrl);
    }
    [Fact] public void Test2()
    {
        // Create a URLParser and pass the dictionary to its constructor
        var parser = new URLParser(ValueResolvers,true);

        // Use the URLParser to create the output URL
        var outputUrl = parser.GetOutputUrl(FullUrl);
        Assert.Equal(expexted2, outputUrl);
    }
}