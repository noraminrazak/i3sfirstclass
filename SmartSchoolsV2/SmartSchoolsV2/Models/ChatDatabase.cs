using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchoolsV2.Models
{
    public class ChatDatabase
    {
        readonly SQLiteAsyncConnection database;

        public ChatDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<Topic>().Wait();
            database.CreateTableAsync<PushMessage>().Wait();
            database.CreateTableAsync<ChatChannel>().Wait();
        }

        #region Channel
        public Task<List<ChatChannel>> GetAllChannelAsync()
        {
            //Get all channels.
            return database.QueryAsync<ChatChannel>("select * from ChatChannel order by time_message");
        }

        public Task<ChatChannel> GetChannelAsync(string channelId)
        {
            // Get a specific channel.
            return database.Table<ChatChannel>()
                            .Where(i => i.channel_id == channelId)
                            .FirstOrDefaultAsync();
        }

        public Task<int> SaveChannelAsync(ChatChannel channel)
        {
            // Save a new channel.
            return database.InsertAsync(channel);
        }

        public Task<int> UpdateChannelAsync(ChatChannel channel)
        {
            // Update an existing channel.
            //return database.UpdateAsync(note);
            return database.ExecuteAsync("update ChatChannel set last_message = ?, time_message = ? where channel_id = ?",
                    new object[] { channel.last_message, channel.time_message, channel.channel_id}
            );
        }

        public Task<int> DeleteChannelAsync(ChatChannel channel)
        {
            // Delete a channel.
            return database.DeleteAsync(channel);
        }

        public async Task<bool> CheckChannelExistAsync(string channelId)
        {
            bool result = false;
            var query = database.Table<ChatChannel>().Where(i => i.channel_id == channelId);
            var user = await query.ToListAsync();
            if (user.Count > 0)
            {
                result = true;
            }
            return result;
        }

        //public Task<ChatChannel> GetChannelIdAsync(string nodePath)
        //{
        //    return database.Table<ChatChannel>()
        //                    .Where(i => i.node_path == nodePath)
        //                    .FirstOrDefaultAsync();
        //}
        #endregion

        #region Topic
        public Task<int> SaveTopicAsync(Topic topic)
        {
            // Save a new channel.
            return database.InsertAsync(topic);
        }

        public Task<int> DeleteTopicAsync(Topic topic)
        {
            // Delete a channel.
            return database.DeleteAsync(topic);
        }

        public async Task<bool> CheckTopicExistAsync(string topic)
        {
            bool result = false;
            var query = database.Table<Topic>().Where(i => i.topic == topic);
            var user = await query.ToListAsync();
            if (user.Count > 0)
            {
                result = true;
            }
            return result;
        }
        #endregion

        #region PushMessage

        #endregion
    }
}
