﻿using Microsoft.Data.Sqlite;
using System.Collections.Generic;

namespace Progbase3ClassLib
{
    public class CommentsRepository
    {
        private SqliteConnection connection;
        public CommentsRepository(SqliteConnection connection)
        {
            this.connection = connection;
        }
        public long Insert(Comment comment)
        {
            connection.Open();
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

            connection.Close();
            return (int)newId;
        }
        public Comment GetById(long id)
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM comments WHERE id = $id";
            command.Parameters.AddWithValue("$id", id);

            SqliteDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                Comment comment = new Comment
                {
                    id = reader.GetInt64(0),
                    authorId = reader.GetInt64(1),
                    postId = reader.GetInt64(2),
                    text = reader.GetString(3),
                    publishTime = reader.GetDateTime(4),
                    isPinned = reader.GetBoolean(5)
                };
                reader.Close();
                connection.Close();
                return comment;
            }
            reader.Close();
            connection.Close();
            return null;
        }
        public int EditById(Comment editedComment)
        {
            connection.Open();
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
            connection.Close();
            return nChanged;
        }
        public int DeleteById(long id)
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"DELETE FROM comments WHERE id = $id";
            command.Parameters.AddWithValue("$id", id);

            int nChanged = command.ExecuteNonQuery();

            connection.Close();
            return nChanged;
        }
        public List<Comment> GetByUserId(long userId)
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM comments WHERE author_id = $author_id";
            command.Parameters.AddWithValue("$author_id", userId);

            SqliteDataReader reader = command.ExecuteReader();
            List<Comment> list = new List<Comment>();
            while (reader.Read())
            {
                Comment comment = new Comment()
                {
                    id = reader.GetInt64(0),
                    authorId = userId,
                    postId = reader.GetInt64(2),
                    text = reader.GetString(3),
                    publishTime = reader.GetDateTime(4),
                    isPinned = reader.GetBoolean(5)
                };

                list.Add(comment);
            }
            reader.Close();
            connection.Close();
            return list;
        }
        public List<Comment> GetByPostId(long postId)
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM comments WHERE post_id = $post_id";
            command.Parameters.AddWithValue("$post_id", postId);

            SqliteDataReader reader = command.ExecuteReader();
            List<Comment> list = new List<Comment>();
            while (reader.Read())
            {
                Comment comment = new Comment()
                {
                    id = reader.GetInt64(0),
                    authorId = reader.GetInt64(1),
                    postId = postId,
                    text = reader.GetString(3),
                    publishTime = reader.GetDateTime(4),
                    isPinned = reader.GetBoolean(5)
                };

                list.Add(comment);
            }
            reader.Close();
            connection.Close();
            return list;
        }

    }
}