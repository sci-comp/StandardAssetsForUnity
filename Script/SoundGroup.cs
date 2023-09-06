using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundGroup : MonoBehaviour
{
    public delegate void AudioSourceStoppedHandler(SoundGroup soundGroup);
    public event AudioSourceStoppedHandler OnAudioSourceStopped;

    [SerializeField] bool use3DSound = false;
    [SerializeField] Vector2 varyPitch = new(-0.05f, 0.05f);
    [SerializeField] Vector2 varyVolume = new(0.93f, 1.0f);
    [SerializeField] SoundBUS soundBUS = SoundBUS.SFX;
    [SerializeField] List<AudioSource> audioSources;

    public List<AudioSource> AudioSources => audioSources;
    public SoundBUS SoundBUS => soundBUS;
    public bool Use3DSound => use3DSound;

    private Queue<AudioSource> availableSources;
    private List<AudioSource> activeSources;

    private void Start()
    {
        availableSources = new(audioSources);
        activeSources = new();
    }

    public AudioSource GetAvailableSource()
    {
        AudioSource src;

        if (availableSources.Count > 0)
        {
            src = availableSources.Dequeue();
            src.enabled = true;
            activeSources.Add(src);
        }
        else if (activeSources.Count > 0)
        {
            src = activeSources[0];
            src.Stop();
            src.enabled = true;
            activeSources.RemoveAt(0);
            activeSources.Add(src);
        }
        else
        {
            Debug.LogError("No active or available audio sources. This is not logical.");
            return null;
        }

        src.pitch = Random.Range(varyPitch.x, varyPitch.y);
        src.volume = Random.Range(varyVolume.x, varyVolume.y);

        StartCoroutine(WaitForAudioToEnd(src));

        return src;
    }

    private IEnumerator WaitForAudioToEnd(AudioSource src)
    {
        src.Play();

        yield return new WaitUntil(() => !src.isPlaying);

        src.enabled = false;
        activeSources.Remove(src);
        availableSources.Enqueue(src);

        OnAudioSourceStopped?.Invoke(this);
    }

}

