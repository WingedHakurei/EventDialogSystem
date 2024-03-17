using XLua;

namespace EventDialogSystem
{
    public static class LuaTableExtension
    {
        public static TValue? TryGetStruct<TKey, TValue>(this LuaTable luaTable, TKey key) where TValue : struct
        {
            if (!luaTable.ContainsKey(key))
            {
                return null;
            }
            return luaTable.Get<TKey, TValue>(key);
        }

        public static TValue? TryGetValueType<TValue>(this LuaTable luaTable, string key) where TValue : struct
        {
            if (!luaTable.ContainsKey(key))
            {
                return null;
            }
            return luaTable.Get<TValue>(key);
        }
    }
}