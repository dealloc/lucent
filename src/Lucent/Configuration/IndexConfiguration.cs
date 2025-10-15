using System.ComponentModel.DataAnnotations;
using Lucene.Net.Analysis;
using Lucene.Net.Util;

namespace Lucent.Configuration;

/// <summary>
/// The configuration for a specific Lucene.NET index.
/// </summary>
public sealed class IndexConfiguration
{
    /// <summary>
    /// The version of Lucene to use.
    /// This <b>MUST</b> match the version used for <see cref="Analyzer" />!
    /// </summary>
    [Required]
    public LuceneVersion Version { get; set; } = LuceneVersion.LUCENE_48;

    /// <summary>
    /// The <see cref="Lucene.Net.Store.Directory" /> where the index will be created.
    /// </summary>
    /// <remarks>
    /// If you're testing or don't want to use the disk, use the <see cref="Lucene.Net.Store.RAMDirectory" />.
    /// </remarks>
    [Required]
    public Lucene.Net.Store.Directory? Directory { get; set; }

    /// <summary>
    /// The <see cref="Lucene.Net.Analysis.Analyzer" /> to use when creating the index.
    /// </summary>
    [Required]
    public Analyzer? Analyzer { get; set; }
}