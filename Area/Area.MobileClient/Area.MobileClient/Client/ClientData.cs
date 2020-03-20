using Area.Shared.Protocol.Connection;
using Area.Shared.Protocol.Profile.Enums;
using Area.Shared.Protocol.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Area.MobileClient.Client
{
    public class ClientData
    {

        #region "Variables"

        public ServiceListMessage Services;

        public IdentificationResultMessage Account;

        public ProfileResultEnum ProfileUpdateResult;

        #endregion

        #region "Builder"

        public ClientData()
        {
            Services = null;
            Account = null;
            ProfileUpdateResult = ProfileResultEnum.None;
        }

        #endregion

        #region "Methods"

        public void Update(ServiceListMessage services)
        {
            Services = services;
            OnDataChanged(this, new DataChangedEventArgs(DataChangedEnum.Services));
        }

        public void Update(IdentificationResultMessage account)
        {
            Account = account;
            if (Services == null)
            {
                Services = new ServiceListMessage();
            }
            Services.Services = account.Services;
            OnDataChanged(this, new DataChangedEventArgs(DataChangedEnum.Identification));
        }

        public void Update(ProfileResultEnum profileUpdateResult)
        {
            ProfileUpdateResult = profileUpdateResult;
            OnDataChanged(this, new DataChangedEventArgs(DataChangedEnum.ProfileUpdateResult));
        }

        public void Dispose()
        {

        }

        #endregion

        #region "Events"

        public enum DataChangedEnum
        {
            Services,
            Identification,
            ProfileUpdateResult,
        }


        public delegate void MessageEventHandler(object sender, DataChangedEventArgs args);

        public event MessageEventHandler OnDataChanged;

        public class DataChangedEventArgs : EventArgs
        {
            public DataChangedEnum Data;

            public DataChangedEventArgs(DataChangedEnum data)
            { Data = data; }
        };
        public void DataChanged(Object obj, DataChangedEventArgs eventArgs)
        {
            if (OnDataChanged != null)
            {
                OnDataChanged(obj, eventArgs);
            }
        }

        #endregion

    }
}
