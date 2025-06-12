using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEvent : MonoBehaviour
{
    public UnityEvent onDamageStart, onDamageEnd, animFinishedEvent;
    public void StartDamagePeriod() => onDamageStart?.Invoke();
    public void EndDamagePeriod() => onDamageEnd?.Invoke();
    public void animationFinished() => animFinishedEvent?.Invoke();


}
