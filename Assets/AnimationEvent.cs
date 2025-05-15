using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEvent : MonoBehaviour
{
    public UnityEvent damageEvent, animFinishedEvent;
    public void DamageEvent()
    {
        damageEvent?.Invoke();
    }
    public void animationFinished()
    {
        animFinishedEvent?.Invoke();
    }
}
