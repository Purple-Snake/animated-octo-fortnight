using UnityEngine;
using UnityEngine.InputSystem;

public class RTSCameraMovement : MonoBehaviour
{
    Camera cam;
    GameObject cameraRig;

    InputAction movementInput;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movementInput = InputSystem.actions.FindAction("Move");
    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;

        MoveCamera(dt);
    }

    void MoveCamera(float deltaTime)
    {
        Vector2 movement = movementInput.ReadValue<Vector2>();

        if (movement != Vector2.zero)
        {

            Vector3 forward = transform.forward;
            forward.y = 0.0f;
            forward.Normalize();

            Vector3 right = transform.right;
            right.y = 0.0f;
            right.Normalize();

            Vector3 move = (forward * movement.y + right * movement.x).normalized;

            transform.position += move * 5 * deltaTime;
        }
    }
}
