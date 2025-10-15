﻿using Lucene.Net.Index;
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
        AddKeyedLucentIndex(services, null);

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
        AddKeyedLucentIndex(services, indexName);

        return services;
    }

    /// <summary>
    /// Adds all services under the <paramref name="key" /> service key.
    /// If <paramref name="key" /> is <c>null</c>, services will be registered as if they weren't keyed.
    /// </summary>
    private static void AddKeyedLucentIndex(IServiceCollection services, string? key)
    {
        var name = key ?? Options.Options.DefaultName;
        services.AddOptions<IndexConfiguration>(name);
        services.AddSingleton<IValidateOptions<IndexConfiguration>, IndexConfigurationValidator>();
        services.TryAddTransient<IConfigureOptions<IndexConfiguration>, DefaultIndexConfigurator>();

        services.AddKeyedScoped<IndexWriter>(key, static (provider, name) =>
        {
            var snapshot = provider.GetRequiredService<IOptionsSnapshot<IndexConfiguration>>();
            var config = snapshot.Get(name as string);

            var writerConfig = config.IndexWriterConfig ??
                               new IndexWriterConfig(config.Version, config.Analyzer);
            return new IndexWriter(config.Directory, writerConfig);
        });

        // TODO: should we make these services transient to allow more fine grained control of re-creation?
        services.AddKeyedScoped<IndexReader>(key,
            static (provider, name) => provider.GetRequiredKeyedService<IndexWriter>(name).GetReader(false));
        services.AddKeyedScoped<IndexSearcher>(key, static (provider, name) =>
            new IndexSearcher(provider.GetRequiredKeyedService<IndexReader>(name)));
    }
}