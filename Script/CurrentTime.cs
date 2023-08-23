using System;
using UnityEngine;

namespace Toolbox
{
    public static class DayNightPhases
    {
        public static readonly int Day = 8;
        public static readonly int Night = 18;
        public static readonly int Sunrise = 6;
        public static readonly int Sunset = 16;
    }

    public class CurrentTime : Singleton<CurrentTime>
    {
        [SerializeField] float tickTimeStep = 1.0f;
        [SerializeField] float timeScale = 10.0f;

        public DateTime CurrentDateTime { get; private set; } = new DateTime(2022, 1, 1);
        public float ElapsedTime { get; private set; } = 0;
        public float TimeOfDay { get; private set; } = 0;

        public Action<float> OnTick;
        public Action OnNight;
        public Action OnDay;
        public Action OnSunrise;
        public Action OnSunset;

        private float nextTickTime;
        private bool isNight, isDay, isSunrise, isSunset;

        private void Start()
        {
            nextTickTime = ElapsedTime + tickTimeStep;
        }

        private void Update()
        {
            float timeStep = Time.deltaTime * timeScale;
            ElapsedTime += timeStep;
            DateTime previousDateTime = CurrentDateTime;
            CurrentDateTime = CurrentDateTime.AddSeconds(timeStep);
            TimeOfDay = CurrentDateTime.Hour + (CurrentDateTime.Minute / 60f) + (CurrentDateTime.Second / 3600f);

            if (ElapsedTime >= nextTickTime)
            {
                OnTick?.Invoke(TimeOfDay);
                nextTickTime += tickTimeStep;
            }

            if (CurrentDateTime.Day != previousDateTime.Day)
            {
                ResetPhaseFlags();
            }

            CheckPhases();
        }

        private void CheckPhases()
        {
            if (TimeOfDay >= DayNightPhases.Sunrise && TimeOfDay < DayNightPhases.Day && !isSunrise)
            {
                OnSunrise?.Invoke();
                isSunrise = true;
            }

            if (TimeOfDay >= DayNightPhases.Day && TimeOfDay < DayNightPhases.Sunset && !isDay)
            {
                OnDay?.Invoke();
                isDay = true;
            }

            if (TimeOfDay >= DayNightPhases.Sunset && TimeOfDay < DayNightPhases.Night && !isSunset)
            {
                OnSunset?.Invoke();
                isSunset = true;
            }

            if ((TimeOfDay >= DayNightPhases.Night || TimeOfDay < DayNightPhases.Sunrise) && !isNight)
            {
                OnNight?.Invoke();
                isNight = true;
            }
        }

        public void SetGameTime(DateTime dateTime)
        {
            CurrentDateTime = dateTime;
            ElapsedTime = dateTime.Hour * 3600 + dateTime.Minute * 60 + dateTime.Second;
            TimeOfDay = ElapsedTime / 3600 % 24;
            ResetPhaseFlags();
        }

        private void ResetPhaseFlags()
        {
            isNight = false;
            isDay = false;
            isSunrise = false;
            isSunset = false;
        }
    }
}

