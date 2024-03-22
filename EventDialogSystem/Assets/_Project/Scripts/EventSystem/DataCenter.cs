using System;

namespace EventDialogSystem.EventSystem
{
    public class DataCenter
    {
        public Action<string> InvokeEvent;
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
    }
}