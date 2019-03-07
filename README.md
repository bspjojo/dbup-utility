# dbup-utility

Simple utility to update a database using dbup

Currently supports SQL migrations.

## Uses

.net core and dbup

## How to run

- cd Migrator
- Change the configuration files
- dotnet run

## Docker

[Docker hub](https://hub.docker.com/r/bspjojo/dbup-utility)

A volume is needed for the sql migration files.

```
# docker-compose file
version: '3'
services:
  dbup:
    image: bspjojo/dbup-utility:1.0.0
    volumes:
      - ./migration-files:/app/migration-files
    environment: 
      ConnectionStrings:ConnectionString: Server=dev-sql,1433;Database=Database;User Id=sa;Password=MSSQLServer_Linux;
      Migration:FolderPath: ./migration-files/
      Migration:EnsureDatabaseExists: 'true'
    depends_on:
      - db
  db:
    image: "microsoft/mssql-server-linux"
    container_name: dev-sql
    environment:
      SA_PASSWORD: "MSSQLServer_Linux"
      ACCEPT_EULA: "Y"
    ports:
      - "9002:1433"
```
