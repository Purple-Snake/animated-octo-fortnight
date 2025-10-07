using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class UnitMovement : MonoBehaviour
{
    Camera cam;
    NavMeshAgent agent;
    public LayerMask ground;

    InputAction rightClick;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
        agent = GetComponent<NavMeshAgent>();

        rightClick = InputSystem.actions.FindAction("RightClick");
    }

    // Update is called once per frame
    void Update()
    {
        if (rightClick.IsPressed())
        {
            RaycastHit raycastHit;
            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out raycastHit, Mathf.Infinity, ground))
            {
                agent.SetDestination(raycastHit.point);
            }
        }
    }
}
