FROM microsoft/dotnet:1.1.4-sdk
WORKDIR /src
COPY . .
ENV ASPNETCORE_URLS http://+:80
EXPOSE 80
RUN dotnet restore
RUN dotnet publish -o /app
WORKDIR /app
ENTRYPOINT ["dotnet", "KubTest.WebApi.dll"]