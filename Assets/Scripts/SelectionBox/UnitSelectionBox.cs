using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitSelectionBox : MonoBehaviour
{
    Camera myCam;

    [SerializeField]
    RectTransform boxVisual;

    Rect selectionBox;

    Vector2 startPosition;
    Vector2 endPosition;

    InputAction leftClick;

    private void Start()
    {
        myCam = Camera.main;
        startPosition = Vector2.zero;
        endPosition = Vector2.zero;
        leftClick = InputSystem.actions.FindAction("Click");

        DrawVisual();
    }

    private bool wasPressedLastFrame;
    private void Update()
    {
        bool isPressed = leftClick.IsPressed();
        // When Clicked
        if (isPressed && !wasPressedLastFrame)
        {
            startPosition = Mouse.current.position.ReadValue();

            // For selection the Units
            selectionBox = new Rect();
        }

        // When Dragging
        if (isPressed && wasPressedLastFrame)
        {
            if (boxVisual.rect.width > 0 ||  boxVisual.rect.height > 0) 
            { 
                SelectUnits();
            }
            endPosition = Mouse.current.position.ReadValue();
            DrawVisual();
            DrawSelection();
        }

        // When Releasing
        if (!isPressed && wasPressedLastFrame)
        {
            SelectUnits();

            startPosition = Vector2.zero;
            endPosition = Vector2.zero;
            DrawVisual();
        }
        wasPressedLastFrame = isPressed;
    }

    void DrawVisual()
    {
        // Calculate the starting and ending positions of the selection box.
        Vector2 boxStart = startPosition;
        Vector2 boxEnd = endPosition;

        // Calculate the center of the selection box.
        Vector2 boxCenter = (boxStart + boxEnd) / 2;

        // Set the position of the visual selection box based on its center.
        boxVisual.position = boxCenter;

        // Calculate the size of the selection box in both width and height.
        Vector2 boxSize = new Vector2(Mathf.Abs(boxStart.x - boxEnd.x), Mathf.Abs(boxStart.y - boxEnd.y));

        // Set the size of the visual selection box based on its calculated size.
        boxVisual.sizeDelta = boxSize;
    }

    void DrawSelection()
    {
        if (Mouse.current.position.x.ReadValue() < startPosition.x)
        {
            selectionBox.xMin = Mouse.current.position.x.ReadValue();
            selectionBox.xMax = startPosition.x;
        }
        else
        {
            selectionBox.xMin = startPosition.x;
            selectionBox.xMax = Mouse.current.position.x.ReadValue();
        }


        if (Mouse.current.position.y.ReadValue() < startPosition.y)
        {
            selectionBox.yMin = Mouse.current.position.y.ReadValue();
            selectionBox.yMax = startPosition.y;
        }
        else
        {
            selectionBox.yMin = startPosition.y;
            selectionBox.yMax = Mouse.current.position.y.ReadValue();
        }
    }

    void SelectUnits()
    {
        foreach (var unit in UnitSelectionManager.Instance.allUnitsList)
        {
            if (selectionBox.Contains(myCam.WorldToScreenPoint(unit.transform.position)))
            {
                UnitSelectionManager.Instance.DragSelect(unit);
            }
        }
    }
}
