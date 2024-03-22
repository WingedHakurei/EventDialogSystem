// TODO: 引入 MTTH，每隔多长时间尝试触发一次事件，可视为检查间隔。这需要实现时间系统
// TODO: 引入 after，用于在事件触发后的一段时间后执行效果
// TODO: 添加 on_actions，用于在游戏中的特定时间点（如启动时、C#层内置事件触发时）触发事件
// TODO: 引入 scope

using Cysharp.Threading.Tasks;
using EventDialogSystem.EventSystem;
using EventDialogSystem.GameTimeSystem;
using EventDialogSystem.UI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using XLua;

namespace EventDialogSystem
{
    public class Program : MonoBehaviour
    {
        private const string AddressablesHeader = "Assets/_Project";
        private GameTime _gameTime;
        private DataCenter _dataCenter;
        private EventController _eventController;
        private LuaEnv _luaEnv;
        private void Start()
        {
            _luaEnv = new LuaEnv();
            StartAsync().Forget();
        }

        private void Update()
        {
            _eventController?.Update();
        }

        private void FixedUpdate()
        {
            if (_gameTime != null)
            {
                _gameTime.TryFixedUpdate();
                if (_gameTime.IsRunning && _dataCenter != null)
                {
                    _dataCenter.Year = _gameTime.Year;
                    _dataCenter.Month = _gameTime.Month;
                    _dataCenter.Day = _gameTime.Day;
                }
            }
        }

        private void OnDestroy()
        {
            _gameTime?.OnDestroy();
            _eventController?.OnDestroy();
            _luaEnv?.Dispose();
            _luaEnv = null;
        }

        private async UniTask StartAsync()
        {
            _gameTime = new GameTime();
            var canvasTransform = GameObject.Find("Canvas").transform;
            var gameTimeUI = await Addressables.InstantiateAsync($"{AddressablesHeader}/Prefabs/UI/GameTime.prefab").ToUniTask();
            gameTimeUI.name = "GameTime";
            var rectTransform = gameTimeUI.GetComponent<RectTransform>();
            rectTransform.SetParent(canvasTransform, false);
            rectTransform.anchorMin = new Vector2(1, 1);
            rectTransform.anchorMax = new Vector2(1, 1);
            rectTransform.anchoredPosition = new Vector2(-200, -100);

            gameTimeUI.GetComponent<GameTimeUI>().Initialize(_gameTime);
            var eventResources = await LoadEventResourcesAsync();
            var eventViewer = new EventViewer(canvasTransform, eventResources);
            _dataCenter = new DataCenter();
            _eventController = new EventController(eventViewer, _dataCenter, _luaEnv);
            _eventController.LoadEvents(eventResources.Events);
            _dataCenter.InvokeEvent = _eventController.InvokeEvent;
            Dialog.SetDataCenter(_dataCenter);
        }

        private async UniTask<EventResources> LoadEventResourcesAsync()
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

            var buttonPrefab = loadButtonTask.Result;
            Dialog.SetButtonPrefab(buttonPrefab);

            var eventResources = new EventResources(_luaEnv);
            eventResources.SetDialogPrefab(loadDialogTask.Result);
            eventResources.SetPictures(loadPicturesTask.Result);
            eventResources.SetTexts(loadTextsTask.Result);
            eventResources.SetEvents(loadEventsTask.Result);

            return eventResources;
        }



    }
}