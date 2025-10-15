using Lucene.Net.Analysis.Standard;
using Lucene.Net.Store;
using Microsoft.Extensions.Options;

namespace Lucent.Configuration.Configurators;

/// <summary>
/// Configures the <see cref="IndexConfiguration" /> with default values aimed at local development.
/// </summary>
/// <remarks>
/// When deploying your application you should override the configuration (most likely with a specified directory).
/// </remarks>
internal class DefaultIndexConfigurator : IConfigureNamedOptions<IndexConfiguration>
{
    /// <inheritdoc />
    public void Configure(string? name, IndexConfiguration options)
    {
        Configure(options);
    }

    /// <inheritdoc />
    public void Configure(IndexConfiguration options)
    {
        options.Analyzer ??= new StandardAnalyzer(options.Version);
        options.Directory ??= new RAMDirectory();
    }
}