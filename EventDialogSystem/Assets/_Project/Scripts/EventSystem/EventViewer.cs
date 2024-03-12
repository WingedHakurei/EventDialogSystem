using EventDialogSystem.UI;
using UnityEngine;

namespace EventDialogSystem.EventSystem
{
    public class EventViewer
    {
        private readonly Transform _canvasTransform;
        private readonly EventResources _eventResources;
        public EventViewer(Transform canvasTransform, EventResources eventResources)
        {
            _canvasTransform = canvasTransform;
            _eventResources = eventResources;
        }
        public void ShowEvent(Event @event)
        {
            var dialog = Object.Instantiate(_eventResources.DialogPrefab, _canvasTransform)
                .GetComponent<Dialog>();
            dialog.SetTitle(_eventResources.Texts.ContainsKey(@event.Title) ? _eventResources.Texts[@event.Title] : @event.Title);
            dialog.SetDesc(_eventResources.Texts.ContainsKey(@event.Desc) ? _eventResources.Texts[@event.Desc] : @event.Desc);
            dialog.SetPicture(_eventResources.Pictures.ContainsKey(@event.Picture) ? _eventResources.Pictures[@event.Picture] : null);
            foreach (var option in @event.Options)
            {
                dialog.AddButton(
                    _eventResources.Texts.ContainsKey(option.Name) ? _eventResources.Texts[option.Name] : option.Name,
                    option.Immediate);
            }
        }
        public void OnDestroy()
        {
            _eventResources.OnDestroy();
        }
    }
}