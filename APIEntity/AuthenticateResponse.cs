using System;
using System.Collections.Generic;
using System.Text;

namespace APIEntity
{
    public class AuthenticateResponse
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }

        public AuthenticateResponse(User user, string token)
        {
            Id = user.Id;
            Firstname = user.Firstname;
            Lastname = user.Lastname;
            Username = user.Username;
            Token = token;
        }
    }
}
