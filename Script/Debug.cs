using TMPro;
using UnityEngine;

public class PerformanceCounter : MonoBehaviour
{
    public TMP_Text fpsText;
    public TMP_Text memoryText;
    public TMP_Text cpuTimeText;

    private int frameCount;
    private float nextUpdate;

    private void Start()
    {
        nextUpdate = Time.time + 1f;
    }

    private void Update()
    {
        frameCount++;
        float currentMemory = System.GC.GetTotalMemory(false) / 1024f / 1024f;
        float cpuTime = Time.deltaTime * 1000f;

        if (Time.time >= nextUpdate)
        {
            fpsText.text = $"{frameCount} FPS";
            memoryText.text = $"{currentMemory:0.##} MB";
            cpuTimeText.text = $"{cpuTime:0.##} ms CPU";

            frameCount = 0;
            nextUpdate = Time.time + 1f;
        }
    }
}
