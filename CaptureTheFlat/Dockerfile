FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /code
COPY . .
RUN ["dotnet", "restore", "CaptureTheFlat"]
RUN dotnet publish CaptureTheFlat --output /output
FROM build
COPY --from=build /output /app
WORKDIR /app
EXPOSE 5000
ENTRYPOINT [ "dotnet", "CaptureTheFlat.dll"]