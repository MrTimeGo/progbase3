using System;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;

namespace Progbase3ClassLib
{
    public class PostsRepository
    {
        private SqliteConnection connection;
        public PostsRepository(SqliteConnection connection)
        {
            this.connection = connection;
        }
        public long Insert(Post post)
        {
            connection.Open();
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
            connection.Close();

            return newId;
        }
        public Post GetById(long id)
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM posts WHERE id = $id";
            command.Parameters.AddWithValue("$id", id);

            SqliteDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                Post post = new Post
                {
                    id = reader.GetInt64(0),
                    authorId = reader.GetInt64(1),
                    title = reader.GetString(2),
                    text = reader.GetString(3),
                    publishTime = reader.GetDateTime(4)
                };
                reader.Close();
                connection.Close();
                return post;
            }
            reader.Close();
            connection.Close();
            return null;
        }
        public int EditById(Post editedPost)
        {
            connection.Open();
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
            connection.Close();
            return nChanged;
        }
        public int DeleteById(long id)
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"DELETE FROM posts WHERE id = $id";
            command.Parameters.AddWithValue("$id", id);

            int nChanged = command.ExecuteNonQuery();

            connection.Close();
            return nChanged;
        }
        public List<Post> GetByUserId(long userId)
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM posts WHERE author_id = $author_id";
            command.Parameters.AddWithValue("$author_id", userId);

            SqliteDataReader reader = command.ExecuteReader();
            List<Post> list = new List<Post>();
            while(reader.Read())
            {
                Post post = new Post
                {
                    id = reader.GetInt64(0),
                    authorId = userId,
                    title = reader.GetString(2),
                    text = reader.GetString(3),
                    publishTime = reader.GetDateTime(4)
                };

                list.Add(post);
            }
            reader.Close();
            connection.Close();
            return list;
        }
        public bool PostExists(string title)
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM posts WHERE title = $title";
            command.Parameters.AddWithValue("$title", title);

            SqliteDataReader reader = command.ExecuteReader();
            bool result = reader.Read();

            reader.Close();
            connection.Close();
            return result;
        }
        public long GetCount()
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT COUNT(*) FROM posts";

            long count = (long)command.ExecuteScalar();
            connection.Close();
            return count;
        }
        public Post[] GetAll()
        {
            long length = GetCount();
            Post[] posts = new Post[length];

            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM posts";
            SqliteDataReader reader = command.ExecuteReader();

            int i = 0;
            while (reader.Read())
            {
                Post post = new Post
                {
                    id = reader.GetInt64(0),
                    authorId = reader.GetInt64(1),
                    title = reader.GetString(2),
                    text = reader.GetString(3),
                    publishTime = reader.GetDateTime(4)
                };
                posts[i] = post;
                i++;
            }
            reader.Close();
            connection.Close();

            return posts;
        }
    }
}
