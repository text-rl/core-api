#!/bin/bash
cd /usr/monitoring/node_exporter-1.3.1.linux-amd64
./node_exporter &
cd /usr/src/app
dotnet CoreApi.Client.dll
