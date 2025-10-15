using Lucene.Net.Index;
using Microsoft.Extensions.DependencyInjection;

namespace Lucent.Tests.Extensions;

[TestClass]
public class LucentIndexTest
{
    [TestMethod]
    public void AddLucentIndex_Resolves()
    {
        var services = new ServiceCollection();
        services.AddLucentIndex();

        var provider = services.BuildServiceProvider();

        Assert.IsNotNull(provider.GetRequiredService<IndexWriter>());
    }
}