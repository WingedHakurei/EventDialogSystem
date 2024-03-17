using System;
using System.Collections.Generic;
using XLua;

namespace EventDialogSystem.EventSystem
{
    public class Event
    {
        public string Id { get; private set; }
        public string Title { get; private set; }
        public string Desc { get; private set; }
        public string Picture { get; private set; }
        public bool? FireOnlyOnce { get; private set; }
        public bool? IsTriggeredOnly { get; private set; }
        public Action<DataCenter> Immediate { get; private set; }
        public Func<DataCenter, bool> Trigger { get; private set; }
        public List<Option> Options { get; private set; }
        public Event(LuaTable luaTable)
        {
            Id = luaTable.Get<string>("id");
            Title = luaTable.Get<string>("title");
            Desc = luaTable.Get<string>("desc");
            Picture = luaTable.Get<string>("picture");
            Trigger = luaTable.Get<Func<DataCenter, bool>>("trigger");
            FireOnlyOnce = luaTable.TryGetValueType<bool>("fire_only_once");
            IsTriggeredOnly = luaTable.TryGetValueType<bool>("is_triggered_only");
            Immediate = luaTable.Get<Action<DataCenter>>("immediate");
            Options = new List<Option>();
            var optionsTable = luaTable.Get<LuaTable>("options");
            foreach (var key in optionsTable.GetKeys<int>())
            {
                optionsTable.Get(key, out LuaTable optionTable);
                var option = new Option(optionTable);
                Options.Add(option);
            }
        }

        public void OnDestroy()
        {
            foreach (var option in Options)
            {
                option.OnDestroy();
            }
            Options.Clear();
            Options = null;
            Immediate = null;
            Trigger = null;
        }

        public class Option
        {
            public string Name { get; private set; }
            public Action<DataCenter> Effect { get; private set; }
            public Option(LuaTable luaTable)
            {
                Name = luaTable.Get<string>("name");
                Effect = luaTable.Get<Action<DataCenter>>("effect");
            }

            public void OnDestroy()
            {
                Effect = null;
            }
        }
    }
}