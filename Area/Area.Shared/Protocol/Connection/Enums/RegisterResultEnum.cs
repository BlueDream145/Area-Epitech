using System;
using System.Collections.Generic;
using System.Text;

namespace Area.Shared.Protocol.Connection.Enums
{
    public enum RegisterResultEnum
    {
        None,
        Success,
        UsernameAlreadyRegistered,
        MailAlreadyRegistered,
        InvalidUsername,
        InvalidMail,
        InvalidPassword,
        InternalError
    }
}
