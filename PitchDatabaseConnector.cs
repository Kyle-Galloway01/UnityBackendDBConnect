using UnityEngine;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

public class DatabaseManager : MonoBehaviour
{
    // Connection string for the SQL database
    private string connectionString = "Server=yourServerAddress;Database=yourDatabaseName;User Id=yourUsername;Password=yourPassword;";

    // Player name, school, date, and pitch type to search for
    public string playerName;
    public string school;
    public string date;
    public string pitchType;

    // Object in Unity environment to add properties to
    public GameObject pitchObject;

    // Class to store pitch data
    private class PitchData
    {
        public string PlayerName;
        public string School;
        public string Date;
        public string PitchType;
        // Add additional properties as needed
    }

    // Start is called before the first frame update
    void Start()
    {
        // Connect to the SQL database
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            // SQL query to search for specific variables
            string query = "SELECT * FROM PitchData WHERE PlayerName = @PlayerName AND School = @School AND Date = @Date AND PitchType = @PitchType";

            // Create a command to execute the query
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                // Add parameters to the query
                command.Parameters.AddWithValue("@PlayerName", playerName);
                command.Parameters.AddWithValue("@School", school);
                command.Parameters.AddWithValue("@Date", date);
                command.Parameters.AddWithValue("@PitchType", pitchType);

                // Execute the query and retrieve the data
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    // Check if any rows were returned
                    if (reader.HasRows)
                    {
                        // Create a list to store pitch data
                        List<PitchData> pitchDataList = new List<PitchData>();

                        // Read each row of the result set
                        while (reader.Read())
                        {
                            // Create a new PitchData object for each row
                            PitchData pitchData = new PitchData();
                            pitchData.PlayerName = reader["PlayerName"].ToString();
                            pitchData.School = reader["School"].ToString();
                            pitchData.Date = reader["Date"].ToString();
                            pitchData.PitchType = reader["PitchType"].ToString();

                            // Add the PitchData object to the list
                            pitchDataList.Add(pitchData);
                        }

                        // Assign pitch data to the pitch object
                        if (pitchDataList.Count > 0)
                        {
                            // Get the first PitchData object from the list
                            PitchData firstPitchData = pitchDataList[0];

                            // Assign pitch data to the pitch object
                            pitchObject.GetComponent<Renderer>().material.color = Color.red; // Example: Change color of the pitch object
                            // You can add more properties to the pitch object as needed
                        }
                        else
                        {
                            Debug.Log("No pitch data found for the specified variables.");
                        }
                    }
                    else
                    {
                        Debug.Log("No rows returned from the query.");
                    }
                }
            }
        }
    }
}
