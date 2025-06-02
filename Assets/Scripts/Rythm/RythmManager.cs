using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RythmManager : MonoBehaviour
{
    [Header("Rythm Settings")]
    [SerializeField] private float bpm = 120f, beatWindow = 0.15f;

    private float beatInterval, nextBeatTime;
    // Start is called before the first frame update
    void Start()
    {
        beatInterval = 60f / bpm;
        nextBeatTime = Time.time + beatInterval;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextBeatTime)
        {
            nextBeatTime += beatInterval;
        }
    }

    public bool IsOnBeat()
    {
        float timeForNext = MathF.Abs(nextBeatTime - Time.time);
        float timeFromLast = MathF.Abs((nextBeatTime - beatInterval) - Time.time);

        return timeForNext < beatWindow || timeFromLast < beatWindow;
    }

    public float TimeUntilNextBeat()
    {
        return nextBeatTime - Time.time;
    }
}
