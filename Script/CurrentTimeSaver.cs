using UnityEngine;
using PixelCrushers;
using System;

namespace Toolbox
{
    [AddComponentMenu("Toolbox/Saver")]
    [RequireComponent(typeof(CurrentTime))]
    public class GameTimeSaver : Saver
    {
        public override string RecordData()
        {
            return SaveSystem.Serialize(CurrentTime.Instance.CurrentDateTime);
        }

        public override void ApplyData(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return;
            }

            DateTime savedGameTime = SaveSystem.Deserialize<DateTime>(s);
            CurrentTime.Instance.SetGameTime(savedGameTime);
        }
    }
}