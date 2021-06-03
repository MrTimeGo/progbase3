using System;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;

namespace Storage
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
        public int Edit(Post editedPost)
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
        private long GetCount()
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT COUNT(*) FROM posts";

            long count = (long)command.ExecuteScalar();
            connection.Close();
            return count;
        }
        private long GetCount(string searchKeyword, long userId)
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            if (userId == -1)
            {
                command.CommandText = @"SELECT COUNT(*) FROM posts WHERE title LIKE $keyword || '%'";
            }
            else
            {
                command.CommandText = @"SELECT COUNT(*) FROM posts WHERE author_id = $author_id AND title LIKE $keyword || '%'";
                command.Parameters.AddWithValue("$author_id", userId);
            }
            command.Parameters.AddWithValue("$keyword", searchKeyword);

            long count = (long)command.ExecuteScalar();
            connection.Close();
            return count;
        }
        public int GetTotalPages(string searchKeyword, long userId)
        {
            const int pageSize = 10;
            return (int)Math.Ceiling(GetCount(searchKeyword, userId) / (double)pageSize);
        }
        public List<Post> GetPage(int pageNumber, string searchKeyword, long authorId)
        {
            const int pageSize = 10;
            int numberOfPages = GetTotalPages(searchKeyword, authorId);
            if (pageNumber > numberOfPages || pageNumber <= 0)
            {
                throw new Exception("Page is not valid");
            }
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            if (authorId == -1)
            {
                command.CommandText = @"SELECT * FROM posts WHERE (title LIKE $keyword || '%') LIMIT $limit OFFSET $offset";
            }
            else
            {
                command.CommandText = @"SELECT * FROM posts WHERE (author_id = $author_id AND title LIKE $keyword || '%') LIMIT $limit OFFSET $offset";
                command.Parameters.AddWithValue("$author_id", authorId);
            }
            command.Parameters.AddWithValue("$keyword", searchKeyword);
            command.Parameters.AddWithValue("$limit", pageSize);
            command.Parameters.AddWithValue("$offset", (pageNumber - 1) * pageSize);

            SqliteDataReader reader = command.ExecuteReader();
            List<Post> list = new List<Post>();

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

                list.Add(post);
            }
            reader.Close();
            connection.Close();
            return list;
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
