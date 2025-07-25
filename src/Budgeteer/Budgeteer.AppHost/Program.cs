IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<KeycloakResource> keycloak = builder
    .AddKeycloak("keycloak", 8080)
    .WithDataVolume();

IResourceBuilder<PostgresServerResource> postgres = builder.AddPostgres("postgres")
    .WithPgAdmin();
IResourceBuilder<PostgresDatabaseResource> postgresDb = postgres
    .WithDataVolume()
    .AddDatabase("budgeteer");

builder.AddProject<Projects.Budgeteer_ApiService>("apiservice")
    .WithHttpHealthCheck("/health")
    .WithReference(postgresDb)
    .WithReference(keycloak);

// builder.AddProject<Projects.Budgeteer_Web>("webfrontend")
//     .WithExternalHttpEndpoints()
//     .WithHttpsHealthCheck("/health")
//     .WithReference(apiService)
//     .WaitFor(apiService);

builder.Build().Run();
