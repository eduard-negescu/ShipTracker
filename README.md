# ShipTracker

For installing project:
* For installing postgres db with docker run the following command from inside the ShipTrackerServer:
```docker compose up -d```
* Either run from VS 2022 or if not:
    ```dotnet restore```
    * Run for migrations:
    ```dotnet ef database update```
    * Then:
    ```dotnet run ```
