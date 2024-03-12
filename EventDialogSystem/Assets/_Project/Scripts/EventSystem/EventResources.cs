using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace EventDialogSystem.EventSystem
{
    public class EventResources
    {
        public GameObject DialogPrefab { get; private set; }
        public Dictionary<string, string> Texts { get; } = new Dictionary<string, string>();
        public Dictionary<string, Sprite> Pictures { get; } = new Dictionary<string, Sprite>();
        private LuaEnv _luaEnv;
        public EventResources(LuaEnv luaEnv)
        {
            _luaEnv = luaEnv;
        }

        public void OnDestroy()
        {
            _luaEnv = null;
        }

        public void SetDialogPrefab(GameObject dialogPrefab)
        {
            DialogPrefab = dialogPrefab;
        }

        public void SetPictures(IList<Sprite> pictures)
        {
            foreach (var picture in pictures)
            {
                Pictures.Add(picture.name, picture);
            }
        }
        public void SetTexts(IList<TextAsset> texts)
        {
            foreach (var text in texts)
            {
                var table = _luaEnv.DoString(text.text)[0] as LuaTable;
                foreach (var key in table.GetKeys<string>())
                {
                    table.Get(key, out string value);
                    Texts.Add(key, value);
                }
            }
        }
    }
}