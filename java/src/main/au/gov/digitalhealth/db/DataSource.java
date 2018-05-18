package au.gov.digitalhealth.db;

import java.io.File;
import java.io.FileReader;
import java.io.IOException;
import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.SQLException;
import java.util.Properties;

import au.gov.digitalhealth.db.DataSource;

/**
 * Maintains the single connection to the database.<p/>
 * 
 * The configuration to establish the connection may be declared in the properties file <i>application.properties</i>.
 * Alternatively a different file may be specified by setting the system property <i>config.file</i><p/> 
 * These properties will be loaded in as standard system properties.
 * If the specified properties file cannot be found the application will continue with out it (no errors reported).
 * 
 * The following system properties are required:<ul>
 * <li>db.server.name (eg. mysql.localhost)
 * <li>db.schema.name (eg. SCT-AU-RF2)
 * <li>db.user.name (eg. John)
 * <li>db.user.password (eg. password)
 * <li>db.driver.classname (eg. com.mysql.jdbc.Driver)
 * <li>db.connection.url (eg. jdbc:mysql://${db.server.name}/${db.schema.name})* 
 * <li>db.max.rows
 * </ul>
 * * Note in-line property substitution supported on the db.connection.url value.
 */
public class DataSource {

    private static Connection connection;
    
    private String dbServer;
    private String dbSchema;
    private String url;
    private String username;
    private String password;
    private String driverClassname;
    private int maxRows;
    
    public DataSource() {
        loadConfig();
        driverClassname = getSystemProperty("db.driver.classname");
        dbServer = getSystemProperty("db.server.name");
        dbSchema = getSystemProperty("db.schema.name");
        username = getSystemProperty("db.user.name");
        password = getSystemProperty("db.user.password");        
        url = getSystemProperty("db.connection.url")
                        .replace("${db.server.name}", dbServer)
                        .replace("${db.schema.name}", dbSchema);
        
        maxRows = Integer.valueOf(getSystemProperty("db.max.rows"));

        // Add hook to automatically close the connection (if it is open) when the application completes.
        Runtime.getRuntime().addShutdownHook(new Thread(){
            public void run() {
                DataSource.close();
            }
        });
    }

    protected void loadConfig() {
        File configFile = new File(System.getProperty("config.file", "application.properties"));
        if (configFile.exists()) {
            try {
                Properties props = new Properties();
                props.load(new FileReader(configFile));
                System.getProperties().putAll(props);
            } catch (IOException e) {
                throw new RuntimeException("Unable to load config file " + configFile.getAbsoluteFile(), e);
            }
        }
    }
    
    protected String getSystemProperty(String key) {
        String result = System.getProperty(key);
        if (result == null || result.length() == 0) {
            throw new RuntimeException("Missing required system property '" + key + "'");
        }
        return result;
    }
    
    /**
     * Obtain the single connection to the database. 
     * The first time this method is called the connection will be established. 
     * 
     * @return The connection to the database
     */
    public Connection getConnection() {
        
        if (connection == null) {
            System.out.println("Connecting to database " + url + " as user '" + username + "'");

            try {
                
                Class.forName(driverClassname);
                connection = DriverManager.getConnection(url, username, password);
                
            } catch (ClassNotFoundException e) {
                throw new RuntimeException("Unknown database driver class: " + driverClassname);
            } catch (SQLException e) {
                throw new RuntimeException("Unable to create database connection!", e);
            }
        }
        
        return connection;
        
    }
    
    /**
     * Close the connection to the database;
     */
    public static void close() {
        if (connection != null) {
            try {
                connection.close();
                connection = null;
            } catch (SQLException e) {
                System.err.println("Unable to close database connection!");
            }
        }
    }

    /**
     * Get the maximum number of rows to return for a query, as defined in the application properties. 
     */
    public int getMaxRows() {
        return maxRows;
    }
    
}
