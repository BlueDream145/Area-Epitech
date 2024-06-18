using Area.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Area.Server
{
    public abstract class ITrigger
    {
        public abstract ReactionEnum Reaction { get; set; }

        public abstract string Params { get; set; }

        public abstract void Start();
        public abstract void Stop();


    }
}
