using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace EventDialogSystem.EventSystem
{
    public class EventController
    {
        private readonly Dictionary<string, Event> _events = new Dictionary<string, Event>();
        private readonly List<Event> _mtthEvents = new List<Event>();
        private readonly EventViewer _eventViewer;
        private readonly DataCenter _dataCenter;
        private LuaEnv _luaEnv;
        public EventController(EventViewer eventViewer, DataCenter dataCenter, LuaEnv luaEnv)
        {
            _eventViewer = eventViewer;
            _dataCenter = dataCenter;
            _luaEnv = luaEnv;
        }

        public void Update()
        {
            int commonEventsCount = _mtthEvents.Count;
            for (int i = 0; i < commonEventsCount; i++)
            {
                var @event = _mtthEvents[i];
                var triggerRes = @event.Trigger?.Invoke(_dataCenter);
                if (triggerRes != false)
                {
                    @event.Immediate?.Invoke(_dataCenter);
                    _eventViewer.ShowEvent(@event);
                    if (@event.FireOnlyOnce == true)
                    {
                        _mtthEvents.RemoveAt(i);
                        commonEventsCount--;
                    }
                }
            }
        }

        public void OnDestroy()
        {
            foreach (var @event in _events.Values)
            {
                @event.OnDestroy();
            }
            _eventViewer.OnDestroy();
            _luaEnv = null;
        }

        public void InvokeEvent(string eventId)
        {
            if (_events.TryGetValue(eventId, out Event @event))
            {
                @event.Immediate?.Invoke(_dataCenter);
                _eventViewer.ShowEvent(@event);
            }
            else
            {
                Debug.LogError($"Event {eventId} not found");
            }
        }

        public void LoadEvents(IList<string> events)
        {
            foreach (var eventText in events)
            {
                var table = _luaEnv.DoString(eventText)[0] as LuaTable;
                foreach (var key in table.GetKeys<int>())
                {
                    table.Get(key, out LuaTable eventTable);
                    var newEvent = new Event(eventTable);
                    _events.Add(newEvent.Id, newEvent);
                    if (newEvent.IsTriggeredOnly != true)
                    {
                        _mtthEvents.Add(newEvent);
                    }
                }
            }
        }


    }
}