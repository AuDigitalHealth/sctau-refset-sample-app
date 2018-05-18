# SNOMED CT-AU Refset Sample Application

## Prerequisites
1. Visual Studio 2015 
2. Install mysql-database-server

## Running the application for the first time
1. Load Project into Visual Studio 2015 
2. Modify the app.config settings for the LoadDB/CTDemo eg.. database name, connection string and Snomed-CT folder location (path to RF2Release directory).
   (This can be done through Visual Stuido by right clicking on the project properties and selecting settings)
3. Run the LoadDB project. (This initialises the database and imports a release)
4. Then right click on project CTDemo and set as start up project. 
5. Unit tests can be run by clicking ```'Test' -> 'Run' -> 'All tests'```

## Documentation
Detailed documentation of the application design and purpose can be found in the [OVERVIEW](../docs/OVERVIEW.md) file.
This application includes the MySql.Data ADO.Net driver (v8.0.11.0)
