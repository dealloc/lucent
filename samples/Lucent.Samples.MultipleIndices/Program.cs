using Lucene.Net.Documents;
using Lucene.Net.Documents.Extensions;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucent.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddNamedLucentIndex("index-1");
builder.Services.AddNamedLucentIndex("index-2");

var app = builder.Build();

using var scope = app.Services.CreateScope();
using var writer1 = scope.ServiceProvider.GetRequiredKeyedService<IndexWriter>("index-1");
using var writer2 = scope.ServiceProvider.GetRequiredKeyedService<IndexWriter>("index-2");

var document = new Document();
document.AddStringField("name", "brown fox",  Field.Store.YES);
writer1.AddDocument(document);
writer1.Commit();

var searcher = scope.ServiceProvider.GetRequiredKeyedService<IndexSearcher>("index-1");
var query = new PhraseQuery
{
    new Term("name", "brown fox")
};

var hits = searcher.Search(query, 10);

Console.Write("Hits: ");
Console.WriteLine(hits.TotalHits);