using Lucene.Net.Index;
using Microsoft.Extensions.DependencyInjection;

namespace Lucent.Tests.Extensions;

[TestClass]
public class LucentNamedIndexTest
{
    [TestMethod]
    public void AddNamedLucentIndex_Resolves()
    {
        var services = new ServiceCollection();
        services.AddNamedLucentIndex("index-1");

        var provider = services.BuildServiceProvider();

        Assert.IsNotNull(provider.GetRequiredKeyedService<IndexWriter>("index-1"));
    }

    [TestMethod]
    public void AddNamedLucentIndex_NoDefault()
    {
        var services = new ServiceCollection();
        services.AddNamedLucentIndex("index-1");

        var provider = services.BuildServiceProvider();
        Assert.ThrowsExactly<InvalidOperationException>(() => provider.GetRequiredService<IndexWriter>(),
            "Default index writer should not have been registered");
    }
}