namespace CleanAspCore.Api.Tests.TestSetup;

internal sealed record MigrationScript(string FromMigration, string ToMigration, string UpScript, string DownScript)
{
    public override string ToString() => ToMigration;
}
