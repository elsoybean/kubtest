FROM microsoft/dotnet:1.1.4-sdk
WORKDIR /app
COPY . .
ENV ASPNETCORE_URLS http://+:80
EXPOSE 80
RUN dotnet restore
ENTRYPOINT ["dotnet", "run"]