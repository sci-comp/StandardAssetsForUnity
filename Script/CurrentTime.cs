using System;
using System.Globalization;
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
        public Action OnMinutePassed;
        public Action OnHourPassed;
        public Action OnDayPassed;
        public Action OnWeekPassed;

        private bool isNight, isDay, isSunrise, isSunset;
        private int previousMinute, previousHour, previousDay, previousWeek;
        private float nextTickTime;
        
        private void Start()
        {
            nextTickTime = ElapsedTime + tickTimeStep;
            previousMinute = CurrentDateTime.Minute;
            previousHour = CurrentDateTime.Hour;
            previousDay = CurrentDateTime.Day;
            previousWeek = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(CurrentDateTime, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
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
            CheckTimeEvents();

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

        private void CheckTimeEvents()
        {
            if (CurrentDateTime.Minute != previousMinute)
            {
                OnMinutePassed?.Invoke();
                previousMinute = CurrentDateTime.Minute;
            }

            if (CurrentDateTime.Hour != previousHour)
            {
                OnHourPassed?.Invoke();
                previousHour = CurrentDateTime.Hour;
            }

            if (CurrentDateTime.Day != previousDay)
            {
                OnDayPassed?.Invoke();
                previousDay = CurrentDateTime.Day;
            }

            int currentWeek = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(CurrentDateTime, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            if (currentWeek != previousWeek)
            {
                OnWeekPassed?.Invoke();
                previousWeek = currentWeek;
            }
        }

        private void ResetPhaseFlags()
        {
            isNight = false;
            isDay = false;
            isSunrise = false;
            isSunset = false;
        }

        public void SetGameTime(DateTime dateTime)
        {
            CurrentDateTime = dateTime;
            ElapsedTime = dateTime.Hour * 3600 + dateTime.Minute * 60 + dateTime.Second;
            TimeOfDay = ElapsedTime / 3600 % 24;
            ResetPhaseFlags();
        }
    }
}

