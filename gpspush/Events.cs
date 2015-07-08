using System;

namespace GPSPush
{
    public class ChangedEventArgs : EventArgs
    {
        public ChangedEventArgs(string name = "", string id = "")
        {
            ModuleName = name;
            Id = id;
        }

        public readonly string ModuleName;
        public readonly string Id;
    }

    public class ChangedEvent
    {
        public event ChangeHandler Change;

        public delegate void ChangeHandler(object s,ChangedEventArgs ea);

        protected void OnChange(object s, ChangedEventArgs e)
        {
            if (Change != null)
                Change(s, e);
        }

        public void BroadcastIt(string message, string id)
        {
            if (!string.IsNullOrEmpty(message) && !string.IsNullOrEmpty(id))
            {
                var info = new ChangedEventArgs(message, id);
                OnChange(this, info);
            }
        }
    }
}

