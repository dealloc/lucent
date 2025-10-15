using Microsoft.Extensions.Options;

namespace Lucent.Configuration.Configurators;

/// <summary>
/// Configures the <see cref="IndexConfiguration" /> with default values aimed at local development.
/// </summary>
/// <remarks>
/// When deploying your application you should override the configuration (most likely with a specified directory).
/// </remarks>
internal class DefaultNamedIndexConfigurator : DefaultIndexConfigurator, IConfigureNamedOptions<IndexConfiguration>
{
    /// <inheritdoc />
    public void Configure(string? name, IndexConfiguration options)
    {
        Configure(options);
    }
}