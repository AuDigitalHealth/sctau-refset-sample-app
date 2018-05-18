# SNOMED CT-AU Refset Sample Application

## Prerequisites
1. Java JDK 1.6.0+ installed (http://www.oracle.com/technetwork/java/javase/downloads/)
2. Apache Ant 1.8+ installed (http://ant.apache.org/)
3. MySQL Server 5.0+ installed and configured (http://www.mysql.com/)
4. A downloaded and extracted copy of the SCT-AU Terminology Bundle (containing the RF2 files to be loaded)

## Running the application for the first time
1. Modify the application.properties file. You must specify the database server details (server name, user name, password, etc) and the location/path of the extracted SCT-AU Terminology Bundle.
2. Run the ant build. Open a command-line console, in the same directory as the build.xml file, run "ant". 

## Ant build targets
* ```"ant" (default)``` - Display this list of build targets. 
* ```"ant db"``` - Builds the database (creates the tables and loads in the SCT-AU RF2 files).
* ```"ant app"``` - Compiles the application and runs it. Use this target to avoid rebuilding the database every time.
* ```"ant test"``` - Run the provided JUnit test cases to ensure the application is functioning correctly.

## Documentation
Detailed documentation of the application design and purpose can be found in the [OVERVIEW](../docs/OVERVIEW.md) file.