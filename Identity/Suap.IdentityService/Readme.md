
MigrationHistory по умолчанию в схеме public, поэтому нужен ConfigDatabase из Extensions.cs

dotnet ef migrations add Init --context LightIdentityDbContext

dotnet ef database update --context LightIdentityDbContext