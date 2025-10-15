using Lucene.Net.Documents;
using Lucene.Net.Documents.Extensions;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucent.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddLucentIndex();

// Tell Lucent to create the index in a folder called "index" in the working directory.
builder.Services.Configure<IndexConfiguration>(options =>
    options.Directory = new MMapDirectory(new DirectoryInfo("index")));

var app = builder.Build();

using var scope = app.Services.CreateScope();
using var writer = scope.ServiceProvider.GetRequiredService<IndexWriter>();

var document = new Document();
document.AddStringField("name", "brown fox",  Field.Store.YES);
writer.AddDocument(document);
writer.Commit();

var searcher = scope.ServiceProvider.GetRequiredService<IndexSearcher>();
var query = new PhraseQuery
{
    new Term("name", "brown fox")
};

var hits = searcher.Search(query, 10);

Console.Write("Hits: ");
Console.WriteLine(hits.TotalHits);