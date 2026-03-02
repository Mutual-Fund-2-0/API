using System.ComponentModel.DataAnnotations;

namespace API.Models;

/// <summary>
/// The table contains metadata related to mutual fund schemes. It includes details such as the fund house, scheme type, category, and unique identifiers for each scheme. This data can be used for analyzing mutual fund offerings, comparing different schemes, and understanding the structure of the mutual fund market.
/// </summary>
public class MutualFundScheme
{

    /// <summary>
    /// Unique identifier for the mutual fund scheme.
    /// </summary>
    [Key]
    public int Code { get; set; }

    /// <summary>
    /// Name of the mutual fund scheme 
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Name of the Asset Management Company (AMC) / Fund House 
    /// </summary>
    public string House { get; set; } = null!;

    /// <summary>
    /// Category within scheme type 
    /// </summary>
    public string? Category { get; set; }

    /// <summary>
    /// Sub-category within scheme type 
    /// </summary>
    public string? SubCategory { get; set; }

    /// <summary>
    /// Plan variant.
    /// </summary>
    public string? Plan { get; set; }

    /// <summary>
    /// Type of mutual fund scheme.
    /// </summary>
    public string? Type { get; set; } = null!;

    /// <summary>
    /// Indicates if the scheme is currently active/available for investment.
    /// </summary>
    public bool? Active { get; set; }

    /// <summary>
    /// Timestamp when the scheme record was created.
    /// </summary>
    public DateTime? Created { get; }
}
