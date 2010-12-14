using System;
using DevText.Framework.Data;

namespace DevText.Framework.Security
{
    public interface IUser:IEntity
    {
        int Id { get; set; }
        string UserName { get; set; }
        string Email { get; set; }
        string Password { get; set; }
    }
}
