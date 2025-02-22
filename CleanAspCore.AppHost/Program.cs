var builder = DistributedApplication.CreateBuilder(args);

var sql = builder.AddSqlServer("sql")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume();

var db = sql.AddDatabase("database");

var api = builder.AddProject<Projects.CleanAspCore_Api>("api")
    .WithReference(db, "Default")
    .WaitFor(db);

var storage = builder.AddAzureStorage("storage").RunAsEmulator();

builder.AddNpmApp("web", "")
    .WithReference(api)
    .WithExternalHttpEndpoints();

builder.Build().Run();
