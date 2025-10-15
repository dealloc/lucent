using Lucene.Net.Documents;
using Lucene.Net.Index;
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

writer.AddDocument(new Document());
writer.Commit();