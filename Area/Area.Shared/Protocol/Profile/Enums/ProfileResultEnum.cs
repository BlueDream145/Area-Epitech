using System;
using System.Collections.Generic;
using System.Text;

namespace Area.Shared.Protocol.Profile.Enums
{
    public enum ProfileResultEnum
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
