using Lucene.Net.Documents;
using Lucene.Net.Documents.Extensions;
using Lucene.Net.Facet;
using Lucene.Net.Facet.Taxonomy;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucent.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddLucentIndex();

// Configure Lucent with facets support
builder.Services.Configure<IndexConfiguration>(options =>
{
    options.Directory = new RAMDirectory();
    options.FacetsDirectory = new RAMDirectory();
    options.FacetsConfig = new FacetsConfig();
});

var app = builder.Build();

using var scope = app.Services.CreateScope();

// Get the services
using var writer = scope.ServiceProvider.GetRequiredService<IndexWriter>();
using var taxonomyWriter = scope.ServiceProvider.GetRequiredService<ITaxonomyWriter>();
var facetsConfig = scope.ServiceProvider.GetRequiredService<Microsoft.Extensions.Options.IOptions<IndexConfiguration>>().Value.FacetsConfig!;

// Add documents with facet fields
var products = new[]
{
    new { Name = "MacBook Pro", Category = "Electronics", Brand = "Apple", Price = 2499 },
    new { Name = "iPhone 15", Category = "Electronics", Brand = "Apple", Price = 999 },
    new { Name = "Coffee Maker", Category = "Home & Kitchen", Brand = "Breville", Price = 299 },
    new { Name = "Desk Chair", Category = "Furniture", Brand = "Herman Miller", Price = 1299 },
    new { Name = "Running Shoes", Category = "Sports", Brand = "Nike", Price = 120 }
};

Console.WriteLine("Indexing products with facets...\n");

foreach (var product in products)
{
    var document = new Document();
    document.AddStringField("name", product.Name, Field.Store.YES);
    document.AddInt32Field("price", product.Price, Field.Store.YES);

    // Add facet fields
    document.AddFacetField("category", product.Category);
    document.AddFacetField("brand", product.Brand);

    // Build the document with facets
    var builtDoc = facetsConfig.Build(taxonomyWriter, document);
    writer.AddDocument(builtDoc);
}

writer.Commit();
taxonomyWriter.Commit();

// Search with facets
var searcher = scope.ServiceProvider.GetRequiredService<IndexSearcher>();
var taxonomyReader = scope.ServiceProvider.GetRequiredService<TaxonomyReader>();

// Search for all documents
var query = new MatchAllDocsQuery();
var facetsCollector = new FacetsCollector();

FacetsCollector.Search(searcher, query, 10, facetsCollector);

// Get facet results
var facets = new FastTaxonomyFacetCounts(taxonomyReader, facetsConfig, facetsCollector);

Console.WriteLine("Search Results with Facets:\n");
Console.WriteLine("Category Facets:");
var categoryFacets = facets.GetTopChildren(10, "category");
if (categoryFacets != null)
{
    foreach (var facet in categoryFacets.LabelValues)
    {
        Console.WriteLine($"  {facet.Label}: {facet.Value} items");
    }
}

Console.WriteLine("\nBrand Facets:");
var brandFacets = facets.GetTopChildren(10, "brand");
if (brandFacets != null)
{
    foreach (var facet in brandFacets.LabelValues)
    {
        Console.WriteLine($"  {facet.Label}: {facet.Value} items");
    }
}

// Drill down on a specific facet
Console.WriteLine("\n\nDrilling down on 'Electronics' category:");
var drillDownQuery = new DrillDownQuery(facetsConfig);
drillDownQuery.Add("category", "Electronics");

var electronicsCollector = new FacetsCollector();
var topDocs = FacetsCollector.Search(searcher, drillDownQuery, 10, electronicsCollector);

Console.WriteLine($"Found {topDocs.TotalHits} products in Electronics:");
foreach (var scoreDoc in topDocs.ScoreDocs)
{
    var doc = searcher.Doc(scoreDoc.Doc);
    Console.WriteLine($"  - {doc.Get("name")} (${doc.GetField("price").GetInt32Value()})");
}

// Show facets for the drill-down results
var electronicsFacets = new FastTaxonomyFacetCounts(taxonomyReader, facetsConfig, electronicsCollector);
Console.WriteLine("\nBrands in Electronics:");
var electronicsBrandFacets = electronicsFacets.GetTopChildren(10, "brand");
if (electronicsBrandFacets != null)
{
    foreach (var facet in electronicsBrandFacets.LabelValues)
    {
        Console.WriteLine($"  {facet.Label}: {facet.Value} items");
    }
}