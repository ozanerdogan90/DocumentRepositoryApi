using DocumentRepositoryApi.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DocumentRepositoryApi.IntegrationTests.Helpers.Models
{
    public static class UserHelper
    {
        public static User User
        {
            get
            {
                return new User()
                {
                    Email = "test@user.com",
                    Password = "11223"
                };
            }

        }

        public static DocumentRepositoryApi.Models.Login Login
        {
            get
            {
                return new DocumentRepositoryApi.Models.Login()
                {
                    Email = "test@user.com",
                    Password = "11223"
                };
            }

        }
    }
}
