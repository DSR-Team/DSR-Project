using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableObject : MonoBehaviour
{
    [SerializeField] float SelectedSize = 1.0f;

    private Vector3 originalPosition;
    private Vector3 originalScale;
    private Quaternion originalRotation;


    public void Selected()
    {
        originalPosition = transform.position;
        originalScale = transform.localScale;
        originalRotation = transform.rotation;
        ChangeSize();
    }

    public void SelectExited()
    {        
        transform.localScale = originalScale;
        transform.position = originalPosition;
        transform.rotation = originalRotation;
    }

    private void ChangeSize()
    {
        transform.localScale = Vector3.one * SelectedSize;
    }
}
