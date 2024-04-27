## Identity - ролевая модель
В данном Api представлены endpoints для управления ролями, пользователями, claims, получением токенов.

## Migrations
MigrationHistory по умолчанию в схеме public, поэтому нужен ConfigDatabase из Extensions.cs

dotnet ef migrations add Init -s ..\Suap.Identity.WebApi\Suap.Identity.WebApi.csproj

dotnet ef database update -s ..\Suap.Identity.WebApi\Suap.Identity.WebApi.csproj