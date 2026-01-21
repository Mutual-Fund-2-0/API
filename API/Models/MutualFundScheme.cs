using System.ComponentModel.DataAnnotations;

namespace API.Models;

/// <summary>
/// The table contains metadata related to mutual fund schemes. It includes details such as the fund house, scheme type, category, and unique identifiers for each scheme. This data can be used for analyzing mutual fund offerings, comparing different schemes, and understanding the structure of the mutual fund market.
/// </summary>
public class MutualFundScheme
{

    /// <summary>
    /// Name of the Asset Management Company (AMC) / Fund House 
    /// </summary>
    public string FundHouse { get; set; } = null!;

    /// <summary>
    /// Type of mutual fund scheme.
    /// </summary>
    public string SchemeType { get; set; } = null!;

    /// <summary>
    /// Sub-category within scheme type 
    /// </summary>
    public string? SchemeCategory { get; set; }

    /// <summary>
    /// Unique identifier for the mutual fund scheme.
    /// </summary>
    [Key]
    public int SchemeCode { get; set; }

    /// <summary>
    /// Name of the mutual fund scheme 
    /// </summary>
    public string? SchemeName { get; set; }

    /// <summary>
    /// ISIN for Growth option of the scheme.
    /// </summary>
    public string? IsinGrowth { get; set; }

    /// <summary>
    /// ISIN for Dividend Reinvestment option of the scheme.
    /// </summary>
    public string? IsinDivReinvestment { get; set; }
}
