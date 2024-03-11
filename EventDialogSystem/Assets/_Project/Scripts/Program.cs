using Cysharp.Threading.Tasks;
using EventDialogSystem.UI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace EventDialogSystem
{
    public class Program : MonoBehaviour
    {
        private const string AddressablesHeader = "Assets/_Project";
        [SerializeField] private Button _testButton;
        private Transform _canvasTransform;
        private GameObject _dialogPrefab;
        private Sprite _image;
        private Dialog _currentDialog;
        private void Start()
        {
            _canvasTransform = FindObjectOfType<Canvas>().transform;
            LoadResourcesAsync().Forget();
        }

        private async UniTask LoadResourcesAsync()
        {
            var loadButtonTask = Addressables.LoadAssetAsync<GameObject>($"{AddressablesHeader}/Prefabs/UI/Button.prefab");
            var loadDialogTask = Addressables.LoadAssetAsync<GameObject>($"{AddressablesHeader}/Prefabs/UI/Dialog.prefab");
            var loadImageTask = Addressables.LoadAssetAsync<Sprite>($"{AddressablesHeader}/Sprites/EventPictures/news_event_001.png");
            await UniTask.WhenAll(loadButtonTask.ToUniTask(), loadDialogTask.ToUniTask(), loadImageTask.ToUniTask());
            var buttonPrefab = loadButtonTask.Result;
            _dialogPrefab = loadDialogTask.Result;
            _image = loadImageTask.Result;
            Dialog.SetButtonPrefab(buttonPrefab);

            _testButton.onClick.AddListener(CreateEvent);
        }

        // TODO: Event 数据结构与文件
        private void CreateEvent()
        {
            _currentDialog = Instantiate(_dialogPrefab, _canvasTransform).GetComponent<Dialog>();
            _currentDialog.SetImage(_image);
            _currentDialog.SetTitle("卢沟桥事变");
            _currentDialog.SetText("日军最近对北京南面具有战略意义的卢沟桥发动了袭击，不过我们英勇的战士们把他们击退了。毫无疑问，这是一起精心策划的事件，目的是把责任推给我们，迫使我们交出更多的领土——就像当年日本入侵东北制造的九一八事变那样。\n\n我们该如何应对？");
            _currentDialog.AddButton("我们的忍耐已到极限！", () => { Debug.Log("日本 对 中国 宣战"); });
            _currentDialog.AddButton("我们承担不起与日本的全面战争。", () => { Debug.Log("日本 触发国家事件 卢沟桥事变"); });
        }
    }
}