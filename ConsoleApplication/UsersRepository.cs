using System;
using Microsoft.Data.Sqlite;

namespace ConsoleApplication
{
    class UsersRepository
    {
        private SqliteConnection connection;
        public UsersRepository(SqliteConnection connection)
        {
            this.connection = connection;
        }
        public long Insert(User user)
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"
            INSERT INTO users (username, password, is_moderator, gender, created_at) 
            VALUES ($username, $password, $is_moderator, $gender, $created_at);
 
            SELECT last_insert_rowid();
            ";

            command.Parameters.AddWithValue("$username", user.username);
            command.Parameters.AddWithValue("$password", user.password);
            command.Parameters.AddWithValue("$is_moderator", user.isModerator);
            command.Parameters.AddWithValue("$gender", user.gender);
            command.Parameters.AddWithValue("$created_at", user.createdAt);

            long newId = (long)command.ExecuteScalar();
            connection.Close();
            return newId;
        }
        public User GetById(long id)
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM users WHERE id = $id";
            command.Parameters.AddWithValue("$id", id);

            SqliteDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                User user = new User
                {
                    id = reader.GetInt64(0),
                    username = reader.GetString(1),
                    password = reader.GetString(2),
                    isModerator = reader.GetBoolean(3),
                    gender = reader.GetString(4),
                    createdAt = reader.GetDateTime(5)
                };

                reader.Close();
                connection.Close();
                return user;
            }
            reader.Close();
            connection.Close();
            return null;
        }
        public int Edit(User editedUser)
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText =
            @"
                UPDATE users
                SET username = $new_username, password = $new_password, is_moderator = $new_is_moderator, gender = $gender, created_at = $created_at
                WHERE id = $id
            ";
            command.Parameters.AddWithValue("$id", editedUser.id);
            command.Parameters.AddWithValue("$new_username", editedUser.username);
            command.Parameters.AddWithValue("$new_password", editedUser.password);
            command.Parameters.AddWithValue("$new_is_moderator", editedUser.isModerator);
            command.Parameters.AddWithValue("$gender", editedUser.gender);
            command.Parameters.AddWithValue("$created_at", editedUser.createdAt.ToString("o"));

            int nChanged = command.ExecuteNonQuery();
            connection.Close();
            return nChanged;
        }
        public int DeleteById(long id)
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"DELETE FROM users WHERE id = $id";
            command.Parameters.AddWithValue("$id", id);

            int nChanged = command.ExecuteNonQuery();

            connection.Close();
            return nChanged;
        }
        public bool UserExists(string username)
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM users WHERE username = $username";
            command.Parameters.AddWithValue("$username", username);

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
            command.CommandText = @"SELECT COUNT(*) FROM users";

            long count = (long)command.ExecuteScalar();
            connection.Close();
            return count;
        }
        public User[] GetAll()
        {
            long length = GetCount();
            User[] users = new User[length];

            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM users";
            SqliteDataReader reader = command.ExecuteReader();

            int i = 0;
            while (reader.Read())
            {
                User user = new User
                {
                    id = reader.GetInt64(0),
                    username = reader.GetString(1),
                    password = reader.GetString(2),
                    isModerator = reader.GetBoolean(3),
                    gender = reader.GetString(4),
                    createdAt = reader.GetDateTime(5)
                };

                users[i] = user;
                i++;
            }
            reader.Close();
            connection.Close();

            return users;
        }
    }
}
