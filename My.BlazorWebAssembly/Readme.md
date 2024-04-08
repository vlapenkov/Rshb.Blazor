# Пример размещения Blazor Web assembly в docker
docker-compose up собирает и запускает . 
Файлы js собираются и копируются на nginx в папку /usr/share/nginx/html внутри контейнера, 
см. dockerfile

- http://localhost:8080/ - в докере на локальном
- http://test.averichev.tech/blazorwebassembly - на k8s

Для проброса окружения надо:
1.В nginx.conf добавить строчку: add_header Blazor-Environment Staging 
2.<base href="/" />  в index.html
3.Остальные настройки , пока лучше в appsettings.json
https://learn.microsoft.com/en-us/aspnet/core/blazor/fundamentals/configuration?view=aspnetcore-8.0