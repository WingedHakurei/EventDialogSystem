using System;

namespace EventDialogSystem.GameTimeSystem
{
    public class GameTime
    {
        public bool IsRunning { get; private set; } = false;
        public int SpeedLevel { get; private set; } = DefaultSpeedLevel;
        public int Year => BaseYear + _gameTimeStamp / DaysPerYear;
        public int Month => BaseMonth + _gameTimeStamp % DaysPerYear / DaysPerMonth + 1;
        public int Day => BaseDay + _gameTimeStamp % DaysPerYear % DaysPerMonth + 1;
        public event Action<GameTime> OnUpdated;
        private int _speed = Speeds[DefaultSpeedLevel - 1];
        private int _gameTimeStamp;
        private int _dayTimer = 0;
        public void TryFixedUpdate()
        {
            if (!IsRunning)
            {
                return;
            }
            _dayTimer += _speed;
            if (_dayTimer >= TimeScalePerDay)
            {
                _dayTimer = 0;
                _gameTimeStamp++;
                OnUpdated?.Invoke(this);
            }
        }
        public void Pause()
        {
            IsRunning = false;
        }
        public void Resume()
        {
            IsRunning = true;
        }
        public bool TrySpeedUp()
        {
            if (SpeedLevel < MaxSpeedLevel)
            {
                SpeedLevel++;
                _speed = Speeds[SpeedLevel - 1];
                return true;
            }
            return false;
        }
        public bool TrySlowDown()
        {
            if (SpeedLevel > MinSpeedLevel)
            {
                SpeedLevel--;
                _speed = Speeds[SpeedLevel - 1];
                return true;
            }
            return false;
        }

        public void OnDestroy()
        {
            OnUpdated = null;
        }

        private const int BaseYear = 1936;
        private const int BaseMonth = 0;
        private const int BaseDay = 0;
        private const int DaysPerMonth = 30;
        private const int MonthsPerYear = 12;
        private const int DaysPerYear = DaysPerMonth * MonthsPerYear;
        private const int MinSpeedLevel = 1;
        private const int DefaultSpeedLevel = 3;
        private const int MaxSpeedLevel = 5;
        private static readonly int[] Speeds = new int[] { 1, 2, 4, 8, 16 };
        private const int TimeScalePerDay = 1 << 8;
    }
}