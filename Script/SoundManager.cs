using System.Collections.Generic;
using Toolbox;
using UnityEngine;

public enum SoundBUS
{
    Ambient,
    Environment,
    SFX,
    UI,
    Voice
}

public class SoundManager : Singleton<SoundManager>
{
    private Dictionary<SoundBUS, int> activeVoiceCounts;
    private Dictionary<SoundBUS, int> busVoiceLimit;
    private Dictionary<string, SoundGroup> soundGroupMap;

    private void Start()
    {
        activeVoiceCounts = new Dictionary<SoundBUS, int>();
        soundGroupMap = new Dictionary<string, SoundGroup>();
        busVoiceLimit = new Dictionary<SoundBUS, int>
        {
            {SoundBUS.Ambient, 5},
            {SoundBUS.Environment, 3},
            {SoundBUS.SFX, 10},
            {SoundBUS.UI, 5},
            {SoundBUS.Voice, 2}
        };

        foreach (SoundBUS bus in busVoiceLimit.Keys)
        {
            activeVoiceCounts[bus] = 0;
        }

        SoundGroup[] soundGroups = GetComponentsInChildren<SoundGroup>();

        foreach (SoundGroup sg in soundGroups)
        {
            soundGroupMap[sg.gameObject.name] = sg;
        }
    }

    public void PlaySound(string soundGroupName)
    {
        PlaySoundInternal(soundGroupName, null);
    }

    public void PlaySound(string soundGroupName, Transform location)
    {
        PlaySoundInternal(soundGroupName, location);
    }

    private void PlaySoundInternal(string soundGroupName, Transform location)
    {
        if (soundGroupMap.TryGetValue(soundGroupName, out SoundGroup soundGroup))
        {
            SoundBUS bus = soundGroup.SoundBUS;
            if (activeVoiceCounts[bus] >= busVoiceLimit[bus])
            {
                return;
            }

            AudioSource source = soundGroup.GetAvailableSource();
            if (source != null)
            {
                activeVoiceCounts[bus]++;
                soundGroup.OnAudioSourceStopped += HandleAudioSourceStopped;

                if (location != null)
                {
                    source.transform.position = location.position;
                }

                source.Play();
            }
        }
    }

    private void HandleAudioSourceStopped(SoundGroup soundGroup)
    {
        SoundBUS bus = soundGroup.SoundBUS;
        activeVoiceCounts[bus]--;
        soundGroup.OnAudioSourceStopped -= HandleAudioSourceStopped;
    }
}
