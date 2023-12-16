namespace CleanAspCore.Api.Tests.TestSetup;

public record MigrationScript(string FromMigration, string ToMigration, string UpScript, string DownScript)
{
    public override string ToString() => ToMigration;
}
