using Cysharp.Threading.Tasks;
using EventDialogSystem.EventSystem;
using EventDialogSystem.UI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using XLua;

namespace EventDialogSystem
{
    // TODO: 实现数据中心
    public class Program : MonoBehaviour
    {
        private const string AddressablesHeader = "Assets/_Project";
        private EventController _eventController;
        private LuaEnv _luaEnv;
        private void Start()
        {
            _luaEnv = new LuaEnv();
            LoadResourcesAsync().Forget();
        }

        // TODO: 完成事件系统与数据中心的交互后，启用 Update 方法
        // private void Update()
        // {
        //     _eventController?.Update();
        // }

        private void OnDestroy()
        {
            _eventController?.OnDestroy();
            _luaEnv = null;
        }

        // TODO: 整理此方法，使其更加清晰
        private async UniTask LoadResourcesAsync()
        {
            var loadButtonTask = Addressables.LoadAssetAsync<GameObject>($"{AddressablesHeader}/Prefabs/UI/Button.prefab");
            var loadDialogTask = Addressables.LoadAssetAsync<GameObject>($"{AddressablesHeader}/Prefabs/UI/Dialog.prefab");
            var picturesLabel = new AssetLabelReference()
            {
                labelString = "Picture"
            };
            var loadPicturesTask = Addressables.LoadAssetsAsync<Sprite>(picturesLabel, null);
            var textsLabel = new AssetLabelReference()
            {
                labelString = "Text"
            };
            var loadTextsTask = Addressables.LoadAssetsAsync<TextAsset>(textsLabel, null);
            var eventsLabel = new AssetLabelReference()
            {
                labelString = "Event"
            };
            var loadEventsTask = Addressables.LoadAssetsAsync<TextAsset>(eventsLabel, null);
            await UniTask.WhenAll(
                loadButtonTask.ToUniTask(),
                loadDialogTask.ToUniTask(),
                loadPicturesTask.ToUniTask(),
                loadTextsTask.ToUniTask(),
                loadEventsTask.ToUniTask());

            var canvasTransform = GameObject.Find("Canvas").transform;
            var buttonPrefab = loadButtonTask.Result;
            Dialog.SetButtonPrefab(buttonPrefab);

            var eventResources = new EventResources(_luaEnv);
            eventResources.SetDialogPrefab(loadDialogTask.Result);
            eventResources.SetPictures(loadPicturesTask.Result);
            eventResources.SetTexts(loadTextsTask.Result);
            var eventViewer = new EventViewer(canvasTransform, eventResources);

            _eventController = new EventController(eventViewer, _luaEnv);
            _eventController.SetEvents(loadEventsTask.Result);

            // TODO: 完成事件系统与数据中心的交互后，删除下一行代码
            _eventController.Update();
        }



    }
}