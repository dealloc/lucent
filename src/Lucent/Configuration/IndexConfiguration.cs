using System.ComponentModel.DataAnnotations;
using Lucene.Net.Analysis;
using Lucene.Net.Facet;
using Lucene.Net.Index;
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
    /// Allows configuring the <see cref="Lucene.Net.Store.Directory" /> used for storing facet information.
    /// </summary>
    public Lucene.Net.Store.Directory? FacetsDirectory { get; set; }

    /// <summary>
    /// The <see cref="Lucene.Net.Analysis.Analyzer" /> to use when creating the index.
    /// </summary>
    [Required]
    public Analyzer? Analyzer { get; set; }

    /// <summary>
    /// The <see cref="Lucene.Net.Index.IndexWriterConfig" /> used when creating the <see cref="IndexWriter" />.
    /// If set to <c>null</c> (default), an instance will automatically be created from <see cref="Version" /> and
    /// <see cref="Analyzer" />.
    /// </summary>
    public IndexWriterConfig? IndexWriterConfig { get; set; }

    /// <summary>
    /// Allows configuring the <see cref="FacetsConfig" /> used for faceted search.
    /// </summary>
    public FacetsConfig? FacetsConfig { get; set; }
}