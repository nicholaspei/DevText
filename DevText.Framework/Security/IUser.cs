using System;

namespace DevText.Framework.Security
{
    public interface IUser
    {
        int Id { get; set; }
        string UserName { get; set; }
        string Email { get; set; }
        string Password { get; set; }
    }
}
