FROM mcr.microsoft.com/dotnet/core/runtime:3.1

CMD ["dotnet publish"]

COPY bin/Debug app/

ENTRYPOINT ["dotnet", "app/Debug/Area.Server.dll"]