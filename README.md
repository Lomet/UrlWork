### URL Filler

URL Filler is a utility that allows you to dynamically build URLs based on provided data sources. It takes a URL template with placeholders for the data and fills it using the provided resolvers.

## Description

The purpose of this project is to provide a flexible and extensible way to construct URLs for various purposes, such as API calls. This project utilizes the Flurl HTTP client library for .NET, which provides a fluent, chainable URL builder and testable HTTP.

## Installation

The code in this repository is a .NET library. To use it, you can either include it directly in your project or package it as a NuGet package.

Usage
Here's an example of how to use the URL Filler:

```csharp
// Create a DownloaderSettings instance
var downloaderSettings = new DownloaderSettings
{
    // Set properties...
};

// Create some instances of your resolvers
var downloaderSettingsResolver = new PropertyGetValueResolver(downloaderSettings);
var endingBlockResolver = new FunctionCallValueResolver(input => EndingBlock(downloaderSettings));

// Create a dictionary that maps parameter names to resolvers
var valueResolvers = new Dictionary<string, IValueResolver>
{
    ["ChainId"] = downloaderSettingsResolver,
    // Add other parameter-resolver pairs...
};

// Create a URLParser and pass the dictionary to its constructor
var parser = new URLParser(FullUrl, valueResolvers);

// Use the URLParser to create the output URL
var outputUrl = parser.GetOutputUrl();
```

## Contributing

Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## License
MIT

