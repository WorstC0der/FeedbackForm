# Используем официальный образ .NET 8 SDK для сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем файлы проекта
COPY *.csproj ./
RUN dotnet restore

# Копируем весь исходный код
COPY . ./

# Собираем приложение
RUN dotnet build -c Release -o /app/build

# Публикуем приложение
RUN dotnet publish -c Release -o /app/publish

# Используем runtime образ для финального контейнера
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Устанавливаем переменные окружения
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:80

# Копируем опубликованное приложение
COPY --from=build /app/publish .

# Открываем порт 80
EXPOSE 80

# Запускаем приложение
ENTRYPOINT ["dotnet", "FeedbackApi.dll"]
