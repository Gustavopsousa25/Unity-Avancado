using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeatPulse : MonoBehaviour
{
    [SerializeField] private Vector3 minScale = new Vector3(.5f, .5f, .5f);
    [SerializeField] private float pulseSpeed = 5f;
    private Image target;
    private RythmManager manager;
    private Vector3 originalScale;

    private void Awake()
    {
        manager = FindObjectOfType<RythmManager>(); 
    }
    // Start is called before the first frame update
    void Start()
    {
        originalScale = transform.localScale;    
    }

    // Update is called once per frame
    void Update()
    {

        if (manager.IsOnBeat())
        {
            transform.localScale = minScale;
        }
        else
        {
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, Time.deltaTime * pulseSpeed);
        }
    }
}
