using System;
using Microsoft.Data.Sqlite;

namespace TemporaryUnnamedProject
{
    class UsersRepository
    {
        private SqliteConnection connection;
        public UsersRepository(string databasePath)
        {
            this.connection = new SqliteConnection($"Data Source = {databasePath}");
            this.connection.Open();
        }
        public int Insert(User user)
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"
            INSERT INTO users (username, password, is_moderator, gender) 
            VALUES ($username, $password, $is_moderator, $gender);
 
            SELECT last_insert_rowid();
            ";

            command.Parameters.AddWithValue("$username", user.username);
            command.Parameters.AddWithValue("$password", user.password);
            command.Parameters.AddWithValue("$is_moderator", user.isModerator);
            command.Parameters.AddWithValue("$gender", user.gender);

            long newId = (long)command.ExecuteScalar();
            return (int)newId;
        }
        public User FindById(int id)
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM users WHERE id = $id";
            command.Parameters.AddWithValue("$id", id);

            SqliteDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                User user = new User
                {
                    id = reader.GetInt32(0),
                    username = reader.GetString(1),
                    password = reader.GetString(2),
                    isModerator = reader.GetBoolean(3),
                    gender = reader.GetString(4)
                };

                return user;
            }
            else
            {
                throw new Exception("User not found");
            }
        }
        public int Edit(User editedUser)
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText =
            @"
                UPDATE users
                SET username = $new_username, password = $new_password, is_moderator = $new_is_moderator, gender = $gender
                WHERE id = $id
            ";
            command.Parameters.AddWithValue("$id", editedUser.id);
            command.Parameters.AddWithValue("$new_username", editedUser.username);
            command.Parameters.AddWithValue("$new_password", editedUser.password);
            command.Parameters.AddWithValue("$new_is_moderator", editedUser.isModerator);
            command.Parameters.AddWithValue("$gender", editedUser.gender);

            int nChanged = command.ExecuteNonQuery();
            return nChanged;
        }
        public int DeleteById(int id)
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"DELETE FROM users WHERE id = $id";
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
