namespace API.Models;

/// <summary>
/// Represents a scheme entity.
/// </summary>
public class Scheme
{
    /// <summary>
    /// Unique identifier for the scheme.
    /// </summary>
    public int Code { get; set; }

    /// <summary>
    /// Name of the scheme.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Name of the Asset Management Company (AMC) / Fund House.
    /// </summary>
    public string? House { get; set; }
    
    /// <summary>
    /// Plan variant.
    /// </summary>
    public string? Plan { get; set; }
    
    /// <summary>
    /// Category within scheme type.
    /// </summary>
    public string? Category { get; set; }

    /// <summary>
    /// Sub-category within scheme type.
    /// </summary>
    public string? SubCategory { get; set; }

    /// <summary>
    /// Type of scheme.
    /// </summary>
    public string? Type { get; set; }
}
