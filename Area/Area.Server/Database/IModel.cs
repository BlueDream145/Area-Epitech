using System;
using System.Collections.Generic;
using System.Text;

namespace Area.Server.Database
{
    public abstract class IModel
    {

        public abstract int Id { get; }

        public abstract void Update();

    }
}
