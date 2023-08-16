using UnityEngine;

/// <summary>
/// A simple next event timer. 
/// 
/// Events are spaced apart using an exponential distribution, defined by t_next_min, t_next_max,
/// and t_next_average. This timer can either loop or run once. Events can be triggered based on 
/// the elapsed time, and the class provides a reset function to set the timer back to the initial 
/// state.
/// </summary>
public class NextEventTimer : MonoBehaviour
{
    public event System.Action EventTriggered;

    [SerializeField] float t_next_min = 0.05f;
    [SerializeField] float t_next_max = 3.0f;
    [SerializeField] float t_next_average = 2.0f;
    [SerializeField] bool loop = true;

    [SerializeField] bool alreadyTriggeredEvent = false;

    private float t_current = 0.0f;
    private float t_next = 0.0f;
    private float u_min = 0.0f;
    private float u_max = 0.0f;

    private void Start()
    {
        u_min = Mathf.Exp(-t_next_min / t_next_average);
        u_max = Mathf.Exp(-t_next_max / t_next_average);

        t_current = 0.0f;
        t_next = -t_next_average * Mathf.Log(Random.Range(u_min, u_max));
        t_next = Mathf.Clamp(t_next, t_next_min, t_next_max);
    }

    private void Update()
    {
        if (loop || !alreadyTriggeredEvent)
        {
            if (t_current > t_next)
            {
                t_current = 0.0f;
                alreadyTriggeredEvent = true;
                t_next = -t_next_average * Mathf.Log(Random.Range(u_min, u_max));
                t_next = Mathf.Clamp(t_next, t_next_min, t_next_max);  // An unnecessary sanity check

                EventTriggered?.Invoke();
            }
            else
            {
                t_current += Time.deltaTime;
            }
        }
    }

    public void Reset()
    {
        alreadyTriggeredEvent = false;

        t_current = 0.0f;
        t_next = -t_next_average * Mathf.Log(Random.Range(u_min, u_max));
        t_next = Mathf.Clamp(t_next, t_next_min, t_next_max);
    }
}

