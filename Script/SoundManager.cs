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
    private readonly Dictionary<SoundBUS, int> activeVoiceCounts = new();
    private readonly Dictionary<SoundBUS, int> busVoiceLimit = new()
    {
        {SoundBUS.Ambient, 3 },
        {SoundBUS.Environment, 1},
        {SoundBUS.SFX, 10},
        {SoundBUS.UI, 5},
        {SoundBUS.Voice, 2}
    };
    private readonly Dictionary<string, SoundGroup> soundGroups = new();

    private void Start()
    {    
        foreach (SoundBUS bus in busVoiceLimit.Keys)
        {
            activeVoiceCounts[bus] = 0;
        }

        SoundGroup[] _soundGroups = GetComponentsInChildren<SoundGroup>();

        foreach (SoundGroup soundGroup in _soundGroups)
        {
            soundGroups[soundGroup.gameObject.name] = soundGroup;
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
        if (soundGroups.TryGetValue(soundGroupName, out SoundGroup soundGroup))
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

    public void SetBusVolume(SoundBUS bus, float volume)
    {
        foreach (var pair in soundGroups)
        {
            SoundGroup soundGroup = pair.Value;
            if (soundGroup.SoundBUS == bus)
            {
                foreach (AudioSource source in soundGroup.AudioSources)
                {
                    source.volume = volume;
                }
            }
        }
    }

}

