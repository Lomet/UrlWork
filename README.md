# URLWork

This project provides a straightforward way to parse URLs containing placeholders and replace them with actual values using a dictionary of value resolvers.

## Description

URLWork offers a flexible and extensible approach to construct URLs for various purposes, like API calls. It leverages the Flurl HTTP client library for .NET, providing a fluent, chainable URL builder and testable HTTP capabilities.

## Installation

The code in this repository is a .NET library. To use it, you can either include it directly in your project or package it as a NuGet package.

### Usage
1. Define a Dictionary<string, IValueResolver>. Each key represents a placeholder name (excluding brackets), and each value is an instance of IValueResolver that provides the real value for this placeholder.
2. Instantiate URLParser and pass the dictionary to its constructor.
3. Use URLParser.ParseUrl() method to get a URL where all placeholders have been replaced with actual values.

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
var outputUrl = parser.ParseUrl("https://api.covalenthq.com/v1/[ChainId]/events/address/[ContractAddress]?starting-block=[StartingBlock]&ending-block=[EndingBlock]&page-number=[PageNumber]&page-size=[MaxPageNumber]&key=[Key]");
```

The expected output will be:


```csharp
 const string expected = "https://api.covalenthq.com/v1/1/events/address/0x7Fc66500c84A76Ad7e9c93437bFc5Ac33E2DDaE9?starting-block=0&ending-block=99999999&page-number=0&page-size=99999999&key=ckey_1234567890";
```

### Trailing Slash Support

The URLParser class supports adding a trailing slash to the parsed URL. This feature is controlled by a boolean argument (includeTrailingSlash) in the constructor. If true, a trailing slash will be added to the parsed URL. This option is useful for APIs that require URLs to end with a slash.

Example:

```csharp
var parser = new URLParser(valueResolvers, true); // Pass 'true' to add a trailing slash
var outputUrl = parser.ParseUrl("https://api.covalenthq.com/v1/[ChainId]/events/address/[ContractAddress]?starting-block=[StartingBlock]&ending-block=[EndingBlock]&page-number=[PageNumber]&page-size=[MaxPageNumber]&key=[Key]");
```

Please note that the requirement of a trailing slash depends on the API you're interfacing with. Some APIs require a trailing slash, while others don't. Always consult the API documentation to be sure.

### Note

The URLParser class uses the Flurl library for URL parsing and manipulation.

## Contributing

Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## License
MIT

