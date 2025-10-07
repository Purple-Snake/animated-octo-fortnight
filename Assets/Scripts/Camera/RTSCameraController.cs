using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class RTSCameraController : MonoBehaviour
{
    //Taken from a tutorial 
    //is here for reference

    [SerializeField] Transform CameraTarget;

    [SerializeField] float MoveSpeed = 20.0f;

    Vector2 moveInput;
    Vector2 scrollInput;
    Vector2 lookInput;

    private void Update()
    {
        float dt = Time.deltaTime;

        UpdateMovement(dt);
    }

    private void UpdateMovement(float deltaTime)
    {
        Vector3 forward = Camera.main.transform.forward;
        forward.y = 0.0f;
        forward.Normalize();

        Vector3 right = Camera.main.transform.right;
        right.y = 0.0f;
        right.Normalize();

        Vector3 targetVelocity = new Vector3(moveInput.x, 0, moveInput.y) * MoveSpeed;

        Vector3 motion = targetVelocity * deltaTime;

        CameraTarget.position += forward * motion.z + right * motion.x;
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnLook(InputValue value) 
    {
        lookInput = value.Get<Vector2>();
    }

    void OnScrollWheel(InputValue value)
    {
        scrollInput = value.Get<Vector2>();
    }
}
