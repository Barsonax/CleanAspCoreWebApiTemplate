var builder = DistributedApplication.CreateBuilder(args);

var sql = builder.AddSqlServer("sql")
    .WithLifetime(ContainerLifetime.Persistent);

var db = sql.AddDatabase("database");

builder.AddProject<Projects.CleanAspCore_Api>("api")
    .WithReference(db, "Default")
    .WaitFor(db);

builder.Build().Run();
