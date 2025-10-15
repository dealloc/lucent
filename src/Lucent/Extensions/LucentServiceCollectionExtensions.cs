using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucent.Configuration;
using Lucent.Configuration.Configurators;
using Lucent.Configuration.Validation;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

// ReSharper disable once CheckNamespace - Common exception to the rule to make the registration more streamlined.
namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extensions to registers Lucent's services and configuration.
/// </summary>
public static class LucentServiceCollectionExtensions
{
    /// <summary>
    /// Registers <see cref="IndexWriter" /> as a scoped service and allows configuration through
    /// <see cref="IndexConfiguration" />.
    /// </summary>
    /// <remarks>
    /// To register multiple indexes, see <see cref="AddNamedLucentIndex" />.
    /// </remarks>
    public static IServiceCollection AddLucentIndex(this IServiceCollection services)
    {
        services.AddOptions<IndexConfiguration>();
        services.AddSingleton<IValidateOptions<IndexConfiguration>, IndexConfigurationValidator>();
        services.TryAddTransient<IConfigureOptions<IndexConfiguration>, DefaultIndexConfigurator>();

        services.AddScoped<IndexWriter>(static provider =>
        {
            var config = provider.GetRequiredService<IOptionsSnapshot<IndexConfiguration>>();

            var writerConfig = config.Value.IndexWriterConfig ??
                               new IndexWriterConfig(config.Value.Version, config.Value.Analyzer);
            return new IndexWriter(config.Value.Directory, writerConfig);
        });

        // TODO: should we make these services transient to allow more fine grained control of re-creation?
        services.AddScoped<IndexReader>(static provider => provider.GetRequiredService<IndexWriter>().GetReader(false));
        services.AddScoped<IndexSearcher>(static provider =>
            new IndexSearcher(provider.GetRequiredService<IndexReader>()));

        return services;
    }

    /// <summary>
    /// Registers <see cref="IndexWriter" /> as a scoped keyed service under <paramref name="indexName" /> and allows
    /// configuration through <see cref="IndexConfiguration" />.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to register the services under.</param>
    /// <param name="indexName">The name of the index to register, this will also serve as the key for services.</param>
    public static IServiceCollection AddNamedLucentIndex(this IServiceCollection services, string indexName)
    {
        services.AddOptions<IndexConfiguration>(indexName);
        services.AddSingleton<IValidateOptions<IndexConfiguration>, IndexConfigurationValidator>();
        services.TryAddKeyedTransient<IConfigureNamedOptions<IndexConfiguration>>(indexName);

        services.AddKeyedScoped<IndexWriter>(indexName, static (provider, name) =>
        {
            var config = provider.GetRequiredKeyedService<IOptionsSnapshot<IndexConfiguration>>(name);

            var writerConfig = config.Value.IndexWriterConfig ??
                               new IndexWriterConfig(config.Value.Version, config.Value.Analyzer);
            return new IndexWriter(config.Value.Directory, writerConfig);
        });

        // TODO: should we make these services transient to allow more fine grained control of re-creation?
        services.AddKeyedScoped<IndexReader>(indexName,
            static (provider, name) => provider.GetRequiredKeyedService<IndexWriter>(name).GetReader(false));
        services.AddKeyedScoped<IndexSearcher>(indexName, static (provider, name) =>
            new IndexSearcher(provider.GetRequiredKeyedService<IndexReader>(name)));

        return services;
    }
}