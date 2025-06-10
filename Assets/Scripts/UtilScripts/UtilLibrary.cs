using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilLibrary : MonoBehaviour
{
    //[SerializeField] private float gravityMult = 1.5f;
    [SerializeField] private float turnSpeed = 360f;
    private float _verticalVelocity;

    public float VerticalVelocity { get => _verticalVelocity; set => _verticalVelocity = value; }

    public void FaceDirection(Vector3 direction)
    {
        if (direction.sqrMagnitude > 0.1f)
        {
            Quaternion targetRot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, turnSpeed * Time.deltaTime);
        }
            
    }
}
