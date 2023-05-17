# URLWork
This project provides a simple way to parse URLs with placeholders and replace them with actual values. The replacements are done with the help of a dictionary of value resolvers.

## Description

The purpose of this project is to provide a flexible and extensible way to construct URLs for various purposes, such as API calls. This project utilizes the Flurl HTTP client library for .NET, which provides a fluent, chainable URL builder and testable HTTP.

## Installation

The code in this repository is a .NET library. To use it, you can either include it directly in your project or package it as a NuGet package.

### Usage
1. Define a Dictionary<string, IValueResolver>. Each key is a placeholder name (without brackets) and each value is an instance of IValueResolver that can provide the real value for this placeholder.

2. Create an instance of URLParser and pass the dictionary to its constructor.

3. Use URLParser.GetOutputUrl() method to get a URL where all placeholders are replaced with actual values.

### Example

```csharp
var downloaderSettingsResolver = new PropertyGetValueResolver(downloaderSettings);
var endingBlockResolver = new FunctionCallValueResolver(input => EndingBlock(downloaderSettings, lastBlockDictionary, chainSettings));

var valueResolvers = new Dictionary<string, IValueResolver>
{
    ["ChainId"] = downloaderSettingsResolver,
    ["ContractAddress"] = downloaderSettingsResolver,
    ["StartingBlock"] = downloaderSettingsResolver,
    ["EndingBlock"] = endingBlockResolver,
    ["PageNumber"] = downloaderSettingsResolver,
    ["MaxPageNumber"] = downloaderSettingsResolver,
    ["Key"] = downloaderSettingsResolver
};

var parser = new URLParser(valueResolvers);
var outputUrl = parser.GetOutputUrl("https://api.covalenthq.com/v1/[ChainId]/events/address/[ContractAddress]?starting-block=[StartingBlock]&ending-block=[EndingBlock]&page-number=[PageNumber]&page-size=[MaxPageNumber]&key=[Key]");
```

will be:

```csharp
 const string expexted = "https://api.covalenthq.com/v1/1/events/address/0x7Fc66500c84A76Ad7e9c93437bFc5Ac33E2DDaE9?starting-block=0&ending-block=99999999&page-number=0&page-size=99999999&key=ckey_1234567890";
```

### Note

The URLParser class uses the Flurl library for URL parsing and manipulation.

## Contributing

Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## License
MIT

