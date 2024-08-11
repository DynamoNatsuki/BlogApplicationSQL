using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class Database
    {
        // Declare a private readonly field to hold the SqlConnection object.
        // The 'readonly' keyword ensures that the connection field can only be assigned during initialization or in a constructor.
        private readonly SqlConnection connection;

        // Constructor for the Database class, which takes a connection string as a parameter.
        public Database(string connectionString)
        {
            // Initialize the SqlConnection object with the provided connection string.
            connection = new SqlConnection(connectionString);

            // Open the database connection immediately after creating the SqlConnection object.
            connection.Open();
        }


        public List<Blog> GetAllBlogs()
        {
            // Define the SQL query to select the Id, Title, and Summary columns from the Blog table.
            string query = "SELECT [Id],[Title],[Summary] FROM [BlogsDB].[dbo].[Blog]";

            // Create a new SqlCommand object using the query and the existing SQL connection.
            SqlCommand cmd = new SqlCommand(query, connection);

            // Execute the SQL query and get a SqlDataReader object to read the results.
            var reader = cmd.ExecuteReader();

            // Initialize an empty list to hold the Blog objects that will be created from the query results.
            var blogs = new List<Blog>();

            // Loop through each row returned by the query.
            while (reader.Read())
            {
                // Extract the Id from the current row, converting it from string to integer.
                int id = int.Parse(reader["Id"].ToString());

                // Extract the Title from the current row.
                string title = reader["Title"].ToString();

                // Extract the Summary from the current row.
                string summary = reader["Summary"].ToString();

                // Create a new Blog object and populate its properties with the data extracted from the current row.
                // Add the newly created Blog object to the list of blogs.
                blogs.Add(new Blog()
                {
                    Id = id,
                    Title = title,
                    Summary = summary
                });
            }

            // Return the list of Blog objects, which contains all the blog entries retrieved from the database.
            return blogs;
        }


        public void CreateBlog(string title, string summary, string blogText)
        {
            // Define the SQL query to insert a new row into the Blog table.
            // The query includes placeholders (@Title, @Summary, @BlogText) for the values to be inserted.
            string query = "INSERT INTO Blog (Title, Summary, BlogText) VALUES (@Title, @Summary, @BlogText)";

            // Use a 'using' statement to ensure that the SqlCommand is disposed of properly after use.
            // The SqlCommand object is created using the query and the existing SQL connection.
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                // Add the title parameter to the SqlCommand, replacing the @Title placeholder in the query with the actual title value.
                cmd.Parameters.AddWithValue("@Title", title);

                // Add the summary parameter to the SqlCommand, replacing the @Summary placeholder in the query with the actual summary value.
                cmd.Parameters.AddWithValue("@Summary", summary);

                // Add the blogText parameter to the SqlCommand, replacing the @BlogText placeholder in the query with the actual blogText value.
                cmd.Parameters.AddWithValue("@BlogText", blogText);

                // Execute the SQL command, which inserts the new blog post into the database.
                // The ExecuteNonQuery method is used for SQL statements that do not return any data, such as INSERT.
                cmd.ExecuteNonQuery();
            }
        }


        public Blog GetBlogById(int id)
        {
            // Define the SQL query to select the Id, Title, and BlogText columns using a parameterized query.
            string query = "SELECT [Id], [Title], [BlogText] FROM [BlogsDB].[dbo].[Blog] WHERE [Id] = @id";

            // Create a new SqlCommand object using the query and the existing SQL connection.
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                // Add the id parameter to the command to safely pass the value to the query.
                cmd.Parameters.AddWithValue("@id", id);

                // Execute the SQL query and get a SqlDataReader object to read the results.
                using (var reader = cmd.ExecuteReader())
                {
                    // Check if there is at least one row returned by the query.
                    if (reader.Read())
                    {
                        // Extract the Title and BlogText columns from the current row in the result set.
                        string title = reader["Title"].ToString();
                        string blogText = reader["BlogText"].ToString();

                        // Return a new Blog object, populating its properties with the data retrieved from the database.
                        return new Blog
                        {
                            Title = title,
                            BlogText = blogText
                        };
                    }
                    else
                    {
                        // Handle the case where no blog was found with the given id (e.g., return null or throw an exception).
                        return null;
                    }
                }
            }
        }

        public Blog GetBlogToDelete(int id)
        {
            // Define the SQL query to select the Id, Title, and BlogText columns using a parameterized query.
            string query = "SELECT [Id], [Title], [Summary], [BlogText] FROM [BlogsDB].[dbo].[Blog] WHERE [Id] = @id";

            // Create a new SqlCommand object using the query and the existing SQL connection.
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                // Add the id parameter to the command to safely pass the value to the query.
                cmd.Parameters.AddWithValue("@id", id);

                // Execute the SQL query and get a SqlDataReader object to read the results.
                using (var reader = cmd.ExecuteReader())
                {
                    // Check if there is at least one row returned by the query.
                    if (reader.Read())
                    {
                        // Extract the Title and BlogText columns from the current row in the result set.
                        int Id = int.Parse(reader["Id"].ToString());
                        string title = reader["Title"].ToString();
                        string summary = reader["Summary"].ToString();
                        string blogText = reader["BlogText"].ToString();

                        // Return a new Blog object, populating its properties with the data retrieved from the database.
                        return new Blog
                        {
                            Id = Id,
                            Title = title,
                            Summary = summary,
                            BlogText = blogText
                        };
                    }
                    else
                    {
                        // Handle the case where no blog was found with the given id (e.g., return null or throw an exception).
                        return null;
                    }
                }
            }
        }

        public void DeleteBlog(int id)
        {
            // Define the SQL query to delete a row from the Blog table where the Id matches the provided id.
            // The {id} placeholder in the query should be replaced by a parameter to prevent SQL injection.
            string query = "DELETE FROM [BlogsDB].[dbo].[Blog] WHERE [Id] = @id";

            // Use a 'using' statement to ensure that the SqlCommand is disposed of properly after use.
            // The SqlCommand object is created using the query and the existing SQL connection.
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                // Add the id parameter to the SqlCommand, replacing the @id placeholder in the query with the actual id value.
                cmd.Parameters.AddWithValue("@id", id);

                // Execute the SQL command, which deletes the row from the database.
                // The ExecuteNonQuery method is used for SQL statements that do not return any data, such as DELETE.
                cmd.ExecuteNonQuery();
            }
        }
    }
}
