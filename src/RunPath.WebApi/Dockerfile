FROM microsoft/dotnet:2.2-sdk AS build

ARG BUILDCONFIG=RELEASE
ARG VERSION=1.0.0

# Copy csproj and restore as distinct layers
COPY src/RunPath.WebApi/RunPath.WebApi.csproj ./RunPath.WebApi/
COPY src/RunPath.Domain/RunPath.Domain.csproj ./RunPath.Domain/
RUN dotnet restore RunPath.WebApi/RunPath.WebApi.csproj

# Copy everything else and build
COPY ./src .
WORKDIR /RunPath.WebApi/
RUN dotnet publish -c $BUILDCONFIG -o out /p:Version=$VERSION

# Build runtime image
FROM microsoft/dotnet:2.2-aspnetcore-runtime
WORKDIR /app
COPY --from=build RunPath.WebApi/out .

EXPOSE 5050
CMD ["dotnet", "RunPath.WebApi.dll"]