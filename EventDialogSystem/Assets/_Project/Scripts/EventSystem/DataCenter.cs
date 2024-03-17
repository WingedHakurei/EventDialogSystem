using System;

namespace EventDialogSystem.EventSystem
{
    public class DataCenter
    {
        public Action<string> InvokeEvent;
        public bool TestData { get; set; }
    }
}