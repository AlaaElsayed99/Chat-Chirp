﻿namespace API.Entities
{
    public class connection
    {
        public connection()
        {
            
        }
        public connection(string? connectionId, string? username)
        {
            ConnectionId = connectionId;
            Username = username;
        }

        public string? ConnectionId { get; set; }
        public string? Username { get; set; }
    }
}
