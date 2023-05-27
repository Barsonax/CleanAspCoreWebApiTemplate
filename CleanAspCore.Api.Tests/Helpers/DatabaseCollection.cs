namespace CleanAspCore.Api.Tests.Helpers;

[CollectionDefinition("Database collection")]
public class DatabaseCollection :  ICollectionFixture<PostgreSqlLifetime>
{

}