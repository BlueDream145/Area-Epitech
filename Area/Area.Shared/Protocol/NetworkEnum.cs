using System;
using System.Collections.Generic;
using System.Text;

namespace Area.Shared.Protocol
{
    public enum NetworkEnum
    {
        Empty = 0,
        // Handshake
        HelloConnectMessage = 1,
        // Connection
        IdentificationMessage = 2,
        IdentificationResultMessage = 3,
        RegisterMessage = 4,
        RegisterResultMessage = 5,
        // Services
        RegisterServiceMessage = 6,
        DeleteServiceMessage = 7,
        ServiceListMessage = 8,
        ServiceMessage = 9,
        // Entities
        ActionMessage = 10,
        ReactionMessage = 11,
        // Other
        UnknowBehaviourMessage = 12,
        // Profile
        ProfileUpdateRequestMessage = 13,
        ProfileUpdateResultMessage = 14,
        // Actions
        ActionRequestMessage = 15,
        ActionResultMessage = 16,
        // Reactions
        ReactionRequestMessage = 17,
        ReactionResultMessage = 18,
    }
}
