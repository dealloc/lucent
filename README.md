# Lucent

A lean, minimalistic .NET library that provides idiomatic integration of Lucene.NET with dependency injection and modern .NET configuration patterns.

## Overview

Lucent is designed to bring Lucene.NET closer to idiomatic .NET best practices while maintaining direct access to the underlying Lucene.NET types and APIs. Unlike heavier abstraction layers, Lucent focuses on providing just what you need: service container integration, configuration management, and lifecycle handling.

## Key Features

- **Minimal Abstraction**: Direct access to Lucene.NET types and APIs
- **Service Container Integration**: Native support for .NET dependency injection
- **Idiomatic Configuration**: Leverage `IConfiguration` and options patterns
- **Lifecycle Management**: Proper disposal and resource management
- **Lean Design**: No unnecessary wrappers or query builders

## Philosophy

Lucent takes a different approach compared to libraries like Examine. While Examine provides extensive abstractions and query builders, Lucent maintains the full power and flexibility of Lucene.NET while simply making it easier to use in modern .NET applications.

If you require more high-level abstractions, fluent query builders, we recommend checking out the excellent [Examine](https://github.com/Shazwazza/Examine) library.

## Comparison with Examine

| Feature | Lucent | Examine |
|---------|---------|---------|
| Abstraction Level | Minimal | High |
| Query Building | Direct Lucene.NET | Fluent API |
| Learning Curve | Requires Lucene.NET knowledge | Examine-specific |
| Flexibility | Full Lucene.NET power | Limited to abstractions |
| Use Case | Lucene users | Quick implementation |

## Requirements

- .NET 9.0 or later
- Lucene.NET 4.8-beta00017 or later

## Installation

```bash
dotnet add package Lucent
```

## Quick Start

```csharp
using Lucene.Net.Documents;
using Lucene.Net.Documents.Extensions;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucent.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

// Register Lucent index with default configuration (uses RAMDirectory)
builder.Services.AddLucentIndex();

var app = builder.Build();

using var scope = app.Services.CreateScope();

// Get the IndexWriter and add a document
using var writer = scope.ServiceProvider.GetRequiredService<IndexWriter>();
var document = new Document();
document.AddStringField("name", "brown fox", Field.Store.YES);
writer.AddDocument(document);
writer.Commit();

// Search the index
var searcher = scope.ServiceProvider.GetRequiredService<IndexSearcher>();
var query = new PhraseQuery { new Term("name", "brown fox") };
var hits = searcher.Search(query, 10);

Console.WriteLine($"Hits: {hits.TotalHits}");
```

## Usage

### Basic Index Configuration

By default, Lucent uses an in-memory `RAMDirectory` and `StandardAnalyzer`. For production use, configure a persistent directory:

```csharp
using Lucene.Net.Store;
using Lucent.Configuration;

builder.Services.AddLucentIndex();

// Configure to use a file-based directory
builder.Services.Configure<IndexConfiguration>(options =>
    options.Directory = new MMapDirectory(new DirectoryInfo("index")));
```

### Custom Analyzer

Configure a custom analyzer for your index:

```csharp
using Lucene.Net.Analysis.En;
using Lucent.Configuration;

builder.Services.Configure<IndexConfiguration>(options =>
{
    options.Directory = new MMapDirectory(new DirectoryInfo("index"));
    options.Analyzer = new EnglishAnalyzer(options.Version);
});
```

### Multiple Indices

Lucent supports registering multiple named indices using keyed services:

```csharp
using Lucene.Net.Index;
using Lucene.Net.Search;
using Microsoft.Extensions.DependencyInjection;

// Register multiple indices
builder.Services.AddNamedLucentIndex("products");
builder.Services.AddNamedLucentIndex("customers");

// Configure each index separately
builder.Services.Configure<IndexConfiguration>("products", options =>
    options.Directory = new MMapDirectory(new DirectoryInfo("products-index")));

builder.Services.Configure<IndexConfiguration>("customers", options =>
    options.Directory = new MMapDirectory(new DirectoryInfo("customers-index")));

// Resolve using keyed services
using var scope = app.Services.CreateScope();
var productsWriter = scope.ServiceProvider.GetRequiredKeyedService<IndexWriter>("products");
var customersWriter = scope.ServiceProvider.GetRequiredKeyedService<IndexWriter>("customers");
var productsSearcher = scope.ServiceProvider.GetRequiredKeyedService<IndexSearcher>("products");
```

### Available Services

When you call `AddLucentIndex()` or `AddNamedLucentIndex()`, the following services are registered:

- **`IndexWriter`** (scoped) - For adding, updating, and deleting documents
- **`IndexReader`** (scoped) - For reading the index
- **`IndexSearcher`** (scoped) - For searching the index

### Configuration Options

The `IndexConfiguration` class supports the following options:

```csharp
public class IndexConfiguration
{
    // The Lucene version to use (default: LUCENE_48)
    public LuceneVersion Version { get; set; }

    // The directory where the index is stored (default: RAMDirectory)
    public Directory Directory { get; set; }

    // The analyzer to use for text processing (default: StandardAnalyzer)
    public Analyzer Analyzer { get; set; }

    // Optional: Custom IndexWriterConfig (auto-created if null)
    public IndexWriterConfig IndexWriterConfig { get; set; }
}
```

### Working with ASP.NET Core

Integrate Lucent in your ASP.NET Core application:

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLucentIndex();
builder.Services.Configure<IndexConfiguration>(
    builder.Configuration.GetSection("Lucent"));

// In your controllers or services, inject the required Lucene services
public class SearchController : ControllerBase
{
    private readonly IndexSearcher _searcher;

    public SearchController(IndexSearcher searcher)
    {
        _searcher = searcher;
    }

    [HttpGet]
    public IActionResult Search(string query)
    {
        var parsedQuery = new TermQuery(new Term("content", query));
        var results = _searcher.Search(parsedQuery, 10);
        return Ok(results);
    }
}
```

## Contributing

Contributions are welcome! Please read our [contributing guidelines](CONTRIBUTING.md) and submit pull requests to the `develop` branch.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE.md) file for details.

## Acknowledgments

- Built on top of the [Lucene.NET](https://lucenenet.apache.org/) library
- Inspired by the simplicity needs not met by existing abstraction layers