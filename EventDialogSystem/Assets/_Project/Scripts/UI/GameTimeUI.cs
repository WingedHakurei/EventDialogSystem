using EventDialogSystem.GameTimeSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EventDialogSystem.UI
{
    public class GameTimeUI : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _gameTimeText;
        [SerializeField]
        private TMP_Text _speedLevelText;
        [SerializeField]
        private Button _pauseButton;
        [SerializeField]
        private Button _speedUpButton;
        [SerializeField]
        private Button _slowDownButton;

        public void Initialize(GameTime timer)
        {
            _pauseButton.onClick.AddListener(() =>
            {
                if (timer.IsRunning)
                {
                    timer.Pause();
                    _speedLevelText.text = "暂停";
                }
                else
                {
                    timer.Resume();
                    _speedLevelText.text = GetSpeedText(timer.SpeedLevel);
                }
            });
            _speedUpButton.onClick.AddListener(() =>
            {
                if (timer.TrySpeedUp() && timer.IsRunning)
                {
                    _speedLevelText.text = GetSpeedText(timer.SpeedLevel);
                }
            });
            _slowDownButton.onClick.AddListener(() =>
            {
                if (timer.TrySlowDown() && timer.IsRunning)
                {
                    _speedLevelText.text = GetSpeedText(timer.SpeedLevel);
                }
            });
            timer.OnUpdated += UpdateGameTimeText;
        }

        private void OnDestroy()
        {
            _pauseButton.onClick.RemoveAllListeners();
            _speedUpButton.onClick.RemoveAllListeners();
            _slowDownButton.onClick.RemoveAllListeners();
        }

        private void UpdateGameTimeText(GameTime timer)
        {
            _gameTimeText.text = $"{timer.Year}.{timer.Month}.{timer.Day}";
        }

        private static string GetSpeedText(int speed = 0)
        {
            return speed switch
            {
                1 => "最慢",
                2 => "慢速",
                3 => "正常",
                4 => "快速",
                5 => "最快",
                _ => "暂停"
            };
        }


    }
}