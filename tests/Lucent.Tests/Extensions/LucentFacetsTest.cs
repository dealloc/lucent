using Lucene.Net.Facet;
using Lucene.Net.Facet.Taxonomy;
using Lucene.Net.Index;
using Lucene.Net.Store;
using Lucent.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lucent.Tests.Extensions;

[TestClass]
public class LucentFacetsTest
{
    [TestMethod]
    public void AddLucentIndex_WithFacets_ResolvesTaxonomyWriter()
    {
        var services = new ServiceCollection();
        services.AddLucentIndex();

        services.Configure<IndexConfiguration>(config =>
            config.FacetsDirectory = new RAMDirectory());

        var provider = services.BuildServiceProvider();

        Assert.IsNotNull(provider.GetRequiredService<ITaxonomyWriter>());
    }

    [TestMethod]
    public void AddLucentIndex_WithFacets_ResolvesTaxonomyReader()
    {
        var services = new ServiceCollection();
        services.AddLucentIndex();

        services.Configure<IndexConfiguration>(config =>
            config.FacetsDirectory = new RAMDirectory());

        var provider = services.BuildServiceProvider();

        // Ensure writer is created and committed first to initialize the directory
        using (var writer = provider.GetRequiredService<ITaxonomyWriter>())
        {
            writer.Commit();
        }

        Assert.IsNotNull(provider.GetRequiredService<TaxonomyReader>());
    }

    [TestMethod]
    public void AddNamedLucentIndex_WithFacets_ResolvesTaxonomyWriter()
    {
        var services = new ServiceCollection();
        services.AddNamedLucentIndex("test");

        services.Configure<IndexConfiguration>("test", config =>
            config.FacetsDirectory = new RAMDirectory());

        var provider = services.BuildServiceProvider();

        Assert.IsNotNull(provider.GetRequiredKeyedService<ITaxonomyWriter>("test"));
    }

    [TestMethod]
    public void AddNamedLucentIndex_WithFacets_ResolvesTaxonomyReader()
    {
        var services = new ServiceCollection();
        services.AddNamedLucentIndex("test");

        services.Configure<IndexConfiguration>("test", config =>
            config.FacetsDirectory = new RAMDirectory());

        var provider = services.BuildServiceProvider();

        // Ensure writer is created and committed first to initialize the directory
        using (var writer = provider.GetRequiredKeyedService<ITaxonomyWriter>("test"))
        {
            writer.Commit();
        }

        Assert.IsNotNull(provider.GetRequiredKeyedService<TaxonomyReader>("test"));
    }

    [TestMethod]
    public void FacetsConfig_CanBeConfigured()
    {
        var services = new ServiceCollection();
        services.AddLucentIndex();

        var customFacetsConfig = new FacetsConfig();
        customFacetsConfig.SetMultiValued("category", true);

        services.Configure<IndexConfiguration>(config =>
        {
            config.FacetsDirectory = new RAMDirectory();
            config.FacetsConfig = customFacetsConfig;
        });

        var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();
        var configuration = scope.ServiceProvider.GetRequiredService<Microsoft.Extensions.Options.IOptions<IndexConfiguration>>();

        Assert.IsNotNull(configuration.Value.FacetsConfig);
        Assert.IsTrue(configuration.Value.FacetsConfig.GetDimConfig("category").IsMultiValued);
    }
}