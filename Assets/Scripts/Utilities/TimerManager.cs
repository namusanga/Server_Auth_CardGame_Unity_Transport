using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    public static TimerManager Active
    {
        get
        {
            if(m_active == null)
            {
                GameObject _timerManager = new GameObject("TimerManager");
                m_active = _timerManager.AddComponent<TimerManager>();
            }
            return m_active;
        }
    }
    private static TimerManager m_active;

    private List<Timer> timers = new List<Timer>();


    public Timer AddTimer(float _delay, System.Action _action)
    {
        Timer _timer = new Timer(_delay, _action);
        timers.Add(_timer);
        return _timer;
    }

    /// <summary>
    /// remove the timer at this guid
    /// </summary>
    /// <param name="_guid"></param>
    /// <returns></returns>
    public bool RemoveTimer(System.Guid _guid)
    {
        for (int i = 0; i < timers.Count; i++)
        {
            if (timers[i].guid == _guid)
            {
                timers.RemoveAt(i);
                return true;
            }
        }
        return false;
    }

    private void Update()
    {
        for (int i = 0; i < timers.Count; i++)
        {
            //add the delta time if the timer is paused
            if (timers[i].paused)
                timers[i].executionTime += Time.deltaTime;

            //if the time for the timer has come 
            //execute it

            if(timers[i].executionTime <= Time.time)
            {
                timers[i].action?.Invoke();

                if(timers.Count > i)
                timers.RemoveAt(i);
            }
        }
    }

}

public class Timer
{
    public float executionTime;
    public System.Action action;
    public bool paused;
    public System.Guid guid;
    
    public Timer (float _delay, System.Action _action)
    {
        executionTime = UnityEngine.Time.time + _delay;
        action = _action;
        guid = System.Guid.NewGuid();
    }
}
