namespace CleanAspCore.TestUtils.DataBaseSetup;

public sealed record MigrationScript(string FromMigration, string ToMigration, string UpScript, string DownScript)
{
    public override string ToString() => ToMigration;
}
