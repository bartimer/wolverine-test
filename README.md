# Troubleshooting SQL transport for Wolverine

# The issue 
The Sender api should send a command to the background processor. 
The background processor will handle the command and schedule (bus.ScheduleAt) a new command but this command gets sometimes handled by the sender (which is supposed to be a sendonly endpoint)

## Run db from docker container

```
docker run -d -p 1433:1433 -v <pathTo>/Wolverine.Test:/C/temp/ -e ACCEPT_EULA=Y  -e "MSSQL_SA_PASSWORD=Test1234!" mcr.microsoft.com/mssql/server:2022-latest
```

Create a database called Test in the container.

## Steps to reproduce
Start up both the sender and the backgroundprocessor projects.
The sender spins up a swagger at http://localhost:5016/swagger/index.html and when doing a POST /test/{id}, you'll sends 5 messages to the backgroundprocessor.
Doing this request will process the MyCommand messages in the backgroundprocessor (in TheHandler class) and the handler for MyCommand schedules a new MyCommandToDelay to be processed 10 seconds later.
Most of the MyCommandToDelay messages are handled in the backgroundprocessor while sometimes some of them are handled in the sender process, which is unexpected as it does not have any routing or handlers defined.
