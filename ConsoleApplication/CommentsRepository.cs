using System;
using Microsoft.Data.Sqlite;

namespace TemporaryUnnamedProject
{
    class CommentsRepository
    {
        private SqliteConnection connection;
        public CommentsRepository(string databasePath)
        {
            this.connection = new SqliteConnection($"Data Source = {databasePath}");
            this.connection.Open();
        }
        public int Insert(Comment comment)
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"
            INSERT INTO comments (author_id, post_id, text, publish_time, is_pinned) 
            VALUES ($author_id, $post_id, $text, $publish_time, $is_pinned);
 
            SELECT last_insert_rowid();
            ";

            command.Parameters.AddWithValue("$author_id", comment.authorId);
            command.Parameters.AddWithValue("post_id", comment.postId);
            command.Parameters.AddWithValue("$text", comment.text);
            command.Parameters.AddWithValue("$publish_time", comment.publishTime.ToString("o"));
            command.Parameters.AddWithValue("$is_pinned", comment.isPinned);

            long newId = (long)command.ExecuteScalar();
            return (int)newId;
        }
        public Comment FindById(int id)
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM comments WHERE id = $id";
            command.Parameters.AddWithValue("$id", id);

            SqliteDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                Comment comment = new Comment
                {
                    id = reader.GetInt32(0),
                    authorId = reader.GetInt32(1),
                    postId = reader.GetInt32(2),
                    text = reader.GetString(3),
                    publishTime = reader.GetDateTime(4),
                    isPinned = reader.GetBoolean(5)
                };

                return comment;
            }
            else
            {
                throw new Exception("Comment not found");
            }
        }
        public int EditById(Comment editedComment)
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText =
            @"
                UPDATE comments
                SET author = $author_id, post_id = $post_id, text = $text, publish_time = $publish_time, is_pinned = $is_pinned
                WHERE id = $id
            ";
            command.Parameters.AddWithValue("$id", editedComment.id);
            command.Parameters.AddWithValue("$author_id", editedComment.authorId);
            command.Parameters.AddWithValue("$post_id", editedComment.postId);
            command.Parameters.AddWithValue("$text", editedComment.text);
            command.Parameters.AddWithValue("$publish_time", editedComment.publishTime.ToString("o"));
            command.Parameters.AddWithValue("$is_pinned", editedComment.isPinned);

            int nChanged = command.ExecuteNonQuery();
            return nChanged;
        }
        public int DeleteById(int id)
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"DELETE FROM comments WHERE id = $id";
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
