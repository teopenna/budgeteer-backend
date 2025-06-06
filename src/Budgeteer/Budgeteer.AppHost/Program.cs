var builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<PostgresServerResource> postgres = builder.AddPostgres("postgres")
    .WithPgAdmin();
IResourceBuilder<PostgresDatabaseResource> postgresDb = postgres
    .WithDataVolume()
    .AddDatabase("budgeteer");

IResourceBuilder<ProjectResource> apiService = builder.AddProject<Projects.Budgeteer_ApiService>("apiservice")
    .WithHttpHealthCheck("/health")
    .WithReference(postgresDb);

// builder.AddProject<Projects.Budgeteer_Web>("webfrontend")
//     .WithExternalHttpEndpoints()
//     .WithHttpsHealthCheck("/health")
//     .WithReference(apiService)
//     .WaitFor(apiService);

builder.Build().Run();