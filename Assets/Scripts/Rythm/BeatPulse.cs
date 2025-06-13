using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeatPulse : MonoBehaviour
{
    [SerializeField] private Vector3 maxScale = new Vector3(.5f, .5f, .5f);
    [SerializeField] private float pulseSpeed = 5f;
    private Image target;
    private RythmManager manager;
    private Vector3 originalScale;

    void Start()
    {
        manager = RythmManager.Instance;
        originalScale = transform.localScale;    
    }
    void Update()
    {

        if (manager.IsOnBeat())
        {
            transform.localScale = maxScale;
        }
        else
        {
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, Time.deltaTime * pulseSpeed);
        }
    }
}
