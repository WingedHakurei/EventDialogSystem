using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace EventDialogSystem.UI
{
    public class MyButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _text;
        public Button.ButtonClickedEvent OnClick => _button.onClick;

        public void SetText(string text)
        {
            _text.text = text;
        }

        public void SetImage(Sprite sprite)
        {
            _button.image.sprite = sprite;
        }

    }
}