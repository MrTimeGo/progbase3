using System;
using Microsoft.Data.Sqlite;

namespace ConsoleApplication
{
    class PostsRepository
    {
        private SqliteConnection connection;
        public PostsRepository(string databasePath)
        {
            this.connection = new SqliteConnection($"Data Source = {databasePath}");
            this.connection.Open();
        }
        public int Insert(Post post)
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"
            INSERT INTO posts (author_id, title, text, publish_time) 
            VALUES ($author_id, $title, $text, $publish_time);
 
            SELECT last_insert_rowid();
            ";

            command.Parameters.AddWithValue("$author_id", post.authorId);
            command.Parameters.AddWithValue("$title", post.title);
            command.Parameters.AddWithValue("$text", post.text);
            command.Parameters.AddWithValue("$publish_time", post.publishTime.ToString("o"));

            long newId = (long)command.ExecuteScalar();
            return (int)newId;
        }
        public Post FindById(int id)
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM posts WHERE id = $id";
            command.Parameters.AddWithValue("$id", id);

            SqliteDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                Post post = new Post
                {
                    id = reader.GetInt32(0),
                    authorId = reader.GetInt32(1),
                    title = reader.GetString(2),
                    text = reader.GetString(3),
                    publishTime = reader.GetDateTime(4)
                };

                return post;
            }
            else
            {
                throw new Exception("Post not found");
            }
        }
        public int EditById(Post editedPost)
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText =
            @"
                UPDATE posts
                SET author_id = $author_id, title = $title, text = $text, publish_time = $publish_time
                WHERE id = $id
            ";
            command.Parameters.AddWithValue("$id", editedPost.id);
            command.Parameters.AddWithValue("$author_id", editedPost.authorId);
            command.Parameters.AddWithValue("$title", editedPost.title);
            command.Parameters.AddWithValue("$text", editedPost.text);
            command.Parameters.AddWithValue("$publish_time", editedPost.publishTime.ToString("o"));

            int nChanged = command.ExecuteNonQuery();
            return nChanged;
        }
        public int DeleteById(int id)
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"DELETE FROM posts WHERE id = $id";
            command.Parameters.AddWithValue("$id", id);

            int nChanged = command.ExecuteNonQuery();

            return nChanged;
        }
        public void CloseConnection()
        {
            this.connection.Close();
        }
    }
}
