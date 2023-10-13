using System;
namespace CarRental.API.Models
{
	public class AuthentificationRequestBody
	{
		public string UserName { get; set; }
		public string Password { get; set; }

        public AuthentificationRequestBody(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }
    }
}

