namespace TestUrlFiller.PreMade;

public class DownloaderSettings
{
#pragma warning disable CS8618 // this is just object for testing, so I don't need to initialize it
    public string ChainId { get; set; }
    public string ContractAddress { get; set; }
    public string StartingBlock { get; set; }
    public string MaxPageNumber { get; set; }
    public string Key { get; set; }
    public string PageNumber { get; set; }
    // Other properties...
#pragma warning restore CS8618 // this is just object for testing, so I don't need to initialize it
}
