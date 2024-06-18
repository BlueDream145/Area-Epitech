FROM mcr.microsoft.com/dotnet/core/runtime:3.1

CMD ["dotnet publish"]

COPY bin/Debug/netcoreapp3.1/publish/ app/

EXPOSE 8080

ENTRYPOINT ["dotnet", "app/Area.Server.dll"]