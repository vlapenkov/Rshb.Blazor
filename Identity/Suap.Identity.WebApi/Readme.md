
MigrationHistory по умолчанию в схеме public, поэтому нужен ConfigDatabase из Extensions.cs

C:\DotNetProjects\Suap.App\Identity\Suap.Identity.Persistence>dotnet ef migrations add Init -s ..\Suap.Identity.WebApi\Suap.Identity.WebApi.csproj

C:\DotNetProjects\Suap.App\Identity\Suap.Identity.Persistence>dotnet ef database update -s ..\Suap.Identity.WebApi\Suap.Identity.WebApi.csproj