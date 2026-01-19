using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Datas;

/// <summary>
/// Provides access to MutualFundSchemes table and Supabase Postgres schema mappings.
/// </summary>
public partial class MFDbContext : DbContext
{

    /// <summary>
    /// Default parameterless constructor for design-time tools and migrations.
    /// </summary>
    public MFDbContext()
    {
    }

    /// <summary>
    /// Primary constructor with DbContextOptions for dependency injection.
    /// </summary>
    /// <param name="options">EF Core configuration options including connection string</param>
    public MFDbContext(DbContextOptions<MFDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// DbSet providing access to MutualFundSchemes table.
    /// </summary>
    public virtual DbSet<MutualFundScheme> MutualFundSchemes { get; set; }

    /// <summary>
    /// Configures EF Core model mappings for database schema.
    /// </summary>
    /// <param name="modelBuilder">Model builder for API configuration</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresEnum("auth", "aal_level", ["aal1", "aal2", "aal3"])
            .HasPostgresEnum("auth", "code_challenge_method", ["s256", "plain"])
            .HasPostgresEnum("auth", "factor_status", ["unverified", "verified"])
            .HasPostgresEnum("auth", "factor_type", ["totp", "webauthn", "phone"])
            .HasPostgresEnum("auth", "oauth_authorization_status", ["pending", "approved", "denied", "expired"])
            .HasPostgresEnum("auth", "oauth_client_type", ["public", "confidential"])
            .HasPostgresEnum("auth", "oauth_registration_type", ["dynamic", "manual"])
            .HasPostgresEnum("auth", "oauth_response_type", ["code"])
            .HasPostgresEnum("auth", "one_time_token_type", ["confirmation_token", "reauthentication_token", "recovery_token", "email_change_token_new", "email_change_token_current", "phone_change_token"])
            .HasPostgresEnum("realtime", "action", ["INSERT", "UPDATE", "DELETE", "TRUNCATE", "ERROR"])
            .HasPostgresEnum("realtime", "equality_op", ["eq", "neq", "lt", "lte", "gt", "gte", "in"])
            .HasPostgresEnum("storage", "buckettype", ["STANDARD", "ANALYTICS", "VECTOR"])
            .HasPostgresExtension("extensions", "pg_stat_statements")
            .HasPostgresExtension("extensions", "pgcrypto")
            .HasPostgresExtension("extensions", "uuid-ossp")
            .HasPostgresExtension("graphql", "pg_graphql")
            .HasPostgresExtension("vault", "supabase_vault");

        modelBuilder.Entity<MutualFundScheme>(entity =>
        {
            entity.HasKey(e => e.SchemeCode).HasName("mfapi_meta_data_pkey");
            entity.ToTable("mutual_fund_schemes", tb => tb.HasComment("The table contains metadata related to mutual fund schemes. It includes details such as the fund house, scheme type, category, and unique identifiers for each scheme. This data can be used for analyzing mutual fund offerings, comparing different schemes, and understanding the structure of the mutual fund market."));
            entity.Property(e => e.SchemeCode).ValueGeneratedNever().HasColumnName("scheme_code");
            entity.Property(e => e.FundHouse).HasColumnType("character varying").HasColumnName("fund_house");
            entity.Property(e => e.IsinDivReinvestment).HasColumnType("character varying")
                .HasColumnName("isin_div_reinvestment");
            entity.Property(e => e.IsinGrowth).HasColumnType("character varying").HasColumnName("isin_growth");
            entity.Property(e => e.SchemeCategory).HasColumnType("character varying").HasColumnName("scheme_category");
            entity.Property(e => e.SchemeName).HasColumnType("character varying").HasColumnName("scheme_name");
            entity.Property(e => e.SchemeType).HasColumnType("character varying").HasColumnName("scheme_type");
        });

        modelBuilder.HasSequence<int>("seq_schema_version", "graphql").IsCyclic();

        OnModelCreatingPartial(modelBuilder);
    }

    /// <summary>
    /// Method for additional model configuration from generated code.
    /// </summary>
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
