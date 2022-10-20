FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app
COPY . .
WORKDIR /app
RUN dotnet publish ./ESCPOS/ESCPOS.csproj -c Release 
VOLUME ["/output"]]
COPY ./ESCPOS/bin/Debug/netstandard2.0 /output
#RUN ["dotnet","test"]
