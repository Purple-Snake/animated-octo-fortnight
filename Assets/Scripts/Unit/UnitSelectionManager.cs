using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class UnitSelectionManager : MonoBehaviour
{
    public static UnitSelectionManager Instance { get; set; }

    Camera cam;
    public LayerMask clickable;
    public LayerMask ground;
    public GameObject groundMarker;

    InputAction leftClick;
    InputAction rightClick;
    InputAction leftShift;

    public List<GameObject> allUnitsList = new List<GameObject>();
    public List<GameObject> selectedUnits = new List<GameObject>();


    private void Awake()
    {
        if (Instance != null && Instance != this) 
        {
            Destroy(gameObject);
        } else
        { 
            Instance = this;
        }
    }

    private void Start()
    {
        cam = Camera.main;
        leftClick = InputSystem.actions.FindAction("Click");
        rightClick = InputSystem.actions.FindAction("RightClick");
        leftShift = InputSystem.actions.FindAction("Shift");
    }

    // Update is called once per frame
    void Update()
    {
        if (leftClick.IsPressed())
        {
            RaycastHit raycastHit;
            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out raycastHit, Mathf.Infinity, clickable))
            {
                if (leftShift.IsPressed())
                {
                    MultiSelect(raycastHit.collider.gameObject);
                } else
                {
                    selectByClicking(raycastHit.collider.gameObject);
                }
            }
            else 
            {
                if (!leftShift.IsPressed())
                {
                    DeselectAll(); 
                }
            }
        }

        if (rightClick.IsPressed() && selectedUnits.Count > 0)
        {
            RaycastHit raycastHit;
            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out raycastHit, Mathf.Infinity, ground))
            {
                groundMarker.transform.position = raycastHit.point;

                groundMarker.SetActive(false);
                groundMarker.SetActive(true);
            }
        }
    }

    private void MultiSelect(GameObject unit)
    {
        if (!selectedUnits.Contains(unit))
        {
            // Add unit to list 
            selectedUnits.Add(unit);
            SelectUnit(unit, true);
        }
        else
        {
            SelectUnit(unit, false);
            selectedUnits.Remove(unit);
        }
    }

    private void DeselectAll()
    {
        foreach (var unit in selectedUnits)
        {
            SelectUnit(unit, false);
        }
        selectedUnits.Clear();
        groundMarker.SetActive(false);
    }

    private void selectByClicking(GameObject unit)
    {
        DeselectAll();
        selectedUnits.Add(unit);
        SelectUnit(unit, true);
    }

    //SelectUnit just call TriggerSelectionIndicator and EnableUnitMovement soo you would need to write less
    void SelectUnit(GameObject unit, bool isSelected)
    {
        TriggerSelectionIndicator(unit, isSelected);
        EnableUnitMovement(unit, isSelected);
    }

    private void EnableUnitMovement(GameObject unit, bool canMove)
    {
        unit.GetComponent<UnitMovement>().enabled = canMove;
    }

    void TriggerSelectionIndicator (GameObject unit, bool isVisible)
    {
        unit.transform.GetChild(0).gameObject.SetActive(isVisible);
    }

    internal void DragSelect(GameObject unit)
    {
        if (!selectedUnits.Contains(unit))
        {
            selectedUnits.Add(unit);
            SelectUnit(unit, true);
        }
    }
}
