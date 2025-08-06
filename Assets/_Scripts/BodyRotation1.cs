using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyRotation1 : MonoBehaviour
{
    public float rotationSpeed ;

    private Vector3 mouseStartPos;
    private bool isDragging = false;

    void OnMouseDown()
    {
        mouseStartPos = Input.mousePosition;
        isDragging = true;
    }

    void OnMouseDrag()
    {
        if (isDragging)
        {
            Vector3 mouseCurrentPos = Input.mousePosition;
            float dragDistance = mouseCurrentPos.x - mouseStartPos.x;

            if (Mathf.Abs(dragDistance) > 5)
            {
                bool rotateRight = dragDistance > 0;
                RotateCube(rotateRight);

                mouseStartPos = mouseCurrentPos;
            }
        }
    }

    void OnMouseUp()
    {
        isDragging = false;
    }

    void RotateCube(bool rotateRight)
    {
        float rotationDirection = rotateRight ? 1 : -1;
        Vector3 rotationVector = new Vector3(0, rotationSpeed * rotationDirection * Time.deltaTime, 0);

        transform.Rotate(rotationVector, Space.World);
    }
}
