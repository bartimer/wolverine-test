# Troubleshooting SQL transport for Wolverine
The Sender api should send a command to the background processor. 
The background processor will schedule (bus.ScheduleAt) a new command but this command gets sometimes handled by the sender (which is supposed to be a sendonly endpoint)
## Run db from docker container

```
docker run -d -p 1433:1433 -v <pathTo>/Wolverine.Test:/C/temp/ -e ACCEPT_EULA=Y  -e "MSSQL_SA_PASSWORD=Test1234!" mcr.microsoft.com/mssql/server:2022-latest
```

Create a database called Test in the container.

