using UnityEngine;

namespace Toolbox
{
    public class CurrentTimeTest : MonoBehaviour
    {
        void Start()
        {
            CurrentTime.Instance.OnTick += OnTick;
        }

        private void OnTick(float _timeOfDay)
        {
            Debug.Log("Time of day: " + _timeOfDay);
        }
    }

}
