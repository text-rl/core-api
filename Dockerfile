FROM mcr.microsoft.com/dotnet/sdk:6.0 as build

WORKDIR /app
COPY . .
RUN dotnet restore
RUN dotnet publish -o /app/published-app --no-restore

#Debian GNU/Linux 11 (bullseye)
FROM mcr.microsoft.com/dotnet/aspnet:6.0 as runtime
WORKDIR /usr

RUN mkdir monitoring && mkdir src/app && \
    apt update && \
    apt install wget -y && \
    wget https://github.com/prometheus/node_exporter/releases/download/v1.3.1/node_exporter-1.3.1.linux-amd64.tar.gz -P ./monitoring && \
    tar xvfz ./monitoring/node_exporter-1.3.1.linux-amd64.tar.gz -C ./monitoring

COPY --from=build /app/published-app ./src/app
COPY --from=build /app/docker-run.sh ./src/app/docker-run.sh
RUN ls ./src/app

RUN chmod 777 -R ./src/app/docker-run.sh

CMD [ "./src/app/docker-run.sh"  ]
