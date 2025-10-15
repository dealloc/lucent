using Lucene.Net.Index;
using Lucent.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Lucent.Tests;

[TestClass]
public class ConfigurationTest
{
    [TestMethod]
    public void IndexConfiguration_Directory_IsRequired()
    {
        var services = new ServiceCollection();
        services.AddLucentIndex();

        services.Configure<IndexConfiguration>(config => config.Directory = null);
        var provider = services.BuildServiceProvider();

        Assert.ThrowsExactly<OptionsValidationException>(() => provider.GetRequiredService<IndexWriter>());
    }

    
    [TestMethod]
    public void IndexConfiguration_Analyzer_IsRequired()
    {
        var services = new ServiceCollection();
        services.AddLucentIndex();

        services.Configure<IndexConfiguration>(config => config.Analyzer = null);
        var provider = services.BuildServiceProvider();

        Assert.ThrowsExactly<OptionsValidationException>(() => provider.GetRequiredService<IndexWriter>());
    }
}