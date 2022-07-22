#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

ENV RABBITMQ_HOST localhost
ENV RABBITMQ_PORT 5672
ENV RABBITMQ_USER guest
ENV RABBITMQ_PASSWORD guest
ENV RABBITMQ_VHOST /
ENV ASPNETCORE_URLS=http://+:5000


FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["neo-task.taskapi.solution.sln","neo-task.taskapi.solution/"]
COPY ["neo-task.taskapi/neo-task.taskapi.csproj", "neo-task.taskapi/"]
COPY ["neo-task.rabbitmq/neo-task.rabbitmq.csproj", "neo-task.rabbitmq/"]

RUN dotnet restore "neo-task.taskapi/neo-task.taskapi.csproj"
COPY . .
WORKDIR "/src/neo-task.taskapi"
RUN dotnet build "neo-task.taskapi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "neo-task.taskapi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "neo-task.taskapi.dll"]