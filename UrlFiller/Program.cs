namespace UrlFiller;
//the goal here is to take a url and fill it with data from other sources
//the url will be a template with placeholders for the data
// for example : "https://api.covalenthq.com/v1/[ChainId]/events/address/[ContractAddress]/?starting-block=[StartingBlock]&ending-block=[EndingBlock]&page-number=[PageNumber]&page-size=[MaxPageNumber]&key=[Key]"
// will be made into : "https://api.covalenthq.com/v1/1/events/address/0x7Fc66500c84A76Ad7e9c93437bFc5Ac33E2DDaE9/?starting-block=0&ending-block=99999999&page-number=0&page-size=99999999&key=ckey_1234567890"
// using flurl.http url builder
internal class Program
{
    

    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
    }
}