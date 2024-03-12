using System.Collections.Generic;
using EventDialogSystem.UI;
using UnityEngine;
using XLua;

namespace EventDialogSystem.EventSystem
{
    public class EventController
    {
        private Dictionary<string, Event> _events = new Dictionary<string, Event>();
        private LuaEnv _luaEnv;
        private EventViewer _eventViewer;

        public EventController(EventViewer eventViewer, LuaEnv luaEnv)
        {
            _eventViewer = eventViewer;
            _luaEnv = luaEnv;
        }

        public void Update()
        {
            foreach (var @event in _events.Values)
            {
                if (@event.Trigger())
                {
                    _eventViewer.ShowEvent(@event);
                }
            }
        }

        public void OnDestroy()
        {
            _eventViewer.OnDestroy();
            _luaEnv = null;
        }

        public void SetEvents(IList<TextAsset> events)
        {
            foreach (var eventText in events)
            {
                var table = _luaEnv.DoString(eventText.text)[0] as LuaTable;
                foreach (var key in table.GetKeys<int>())
                {
                    table.Get(key, out LuaTable eventTable);
                    var newEvent = new Event(eventTable);
                    _events.Add(newEvent.Id, newEvent);
                }
            }
        }


    }
}