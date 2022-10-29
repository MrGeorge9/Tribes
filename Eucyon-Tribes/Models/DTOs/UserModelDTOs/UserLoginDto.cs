﻿namespace Eucyon_Tribes.Models.UserModels
{
    public class UserLoginDto
    {
        public string Name { get; }
        public string Password { get; }

        public UserLoginDto(string name, string password)
        {
            Name = name;
            Password = password;
        }
    }
}
