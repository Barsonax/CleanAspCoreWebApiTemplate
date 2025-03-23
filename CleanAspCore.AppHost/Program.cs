var builder = DistributedApplication.CreateBuilder(args);

var sql = builder.AddSqlServer("sql")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume();

var db = sql.AddDatabase("database");

var api = builder.AddProject<Projects.CleanAspCore_Api>("api")
    .WithReference(db, "Database")
    .WaitFor(db);

builder.Build().Run();
