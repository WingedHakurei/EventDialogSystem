using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EventDialogSystem.UI
{
    public class Dialog : MonoBehaviour, IDragHandler
    {
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _title;
        [SerializeField] private TMP_Text _desc;
        [SerializeField] private Transform _buttonRoot;
        private static GameObject _buttonPrefab;
        private readonly List<MyButton> _buttons = new List<MyButton>();
        private RectTransform _rectTransform;

        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void OnDrag(PointerEventData eventData)
        {
            _rectTransform.anchoredPosition += eventData.delta;
        }

        public void SetPicture(Sprite picture)
        {
            _image.sprite = picture;
        }
        public void SetTitle(string title)
        {
            _title.text = title;
        }
        public void SetDesc(string desc)
        {
            _desc.text = desc;
        }
        public static void SetButtonPrefab(GameObject buttonPrefab)
        {
            _buttonPrefab = buttonPrefab;
        }
        public void AddButton(string text, Action action)
        {
            var button = Instantiate(_buttonPrefab, _buttonRoot).GetComponent<MyButton>();
            button.SetText(text);
            button.OnClick.AddListener(() =>
            {
                action?.Invoke();
                Destroy(gameObject);
            });

            _buttons.Add(button);
            ArrangeButtons();
        }

        private void ArrangeButtons()
        {
            var buttonCount = _buttons.Count;
            var buttonHeight = _buttonPrefab.GetComponent<RectTransform>().rect.height;
            var buttonRootHeight = buttonCount * buttonHeight;
            _buttonRoot.GetComponent<RectTransform>().sizeDelta = new Vector2(0, buttonRootHeight);
            for (var i = 0; i < buttonCount; i++)
            {
                var button = _buttons[i];
                var buttonTransform = button.GetComponent<RectTransform>();
                buttonTransform.anchoredPosition = new Vector2(0, -i * buttonHeight);
            }
        }

    }
}