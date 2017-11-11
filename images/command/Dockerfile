FROM microsoft/dotnet:1.1.4-sdk
WORKDIR /src
COPY . .
ENV ASPNETCORE_URLS http://+:80
EXPOSE 80
RUN dotnet restore
RUN dotnet publish -c Release -o /app
WORKDIR /app
RUN rm -rf /src
ENTRYPOINT ["dotnet", "KubTest.WebApi.dll"]