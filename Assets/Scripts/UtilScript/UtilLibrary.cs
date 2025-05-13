using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilLibrary : MonoBehaviour
{
    [SerializeField] private float gravityMult = 1.5f;
    private float _verticalVelocity;

    public float VerticalVelocity { get => _verticalVelocity; set => _verticalVelocity = value; }

    public void FaceDirection(Vector3 direction)
    {
        if (direction.sqrMagnitude > 0.1f)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.1f);
    }
    public void ApplyGravity(CharacterController cc)
    {
        if (cc.isGrounded && VerticalVelocity < 0)
        {
            VerticalVelocity = -2f;
        }

        VerticalVelocity += Physics.gravity.y * gravityMult * Time.deltaTime;
    }
}
