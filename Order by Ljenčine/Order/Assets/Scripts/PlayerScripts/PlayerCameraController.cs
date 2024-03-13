using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCameraController : MonoBehaviour
{
    public float mouseSensitivity = 2;
    public float smoothing = 2;
    public float lookUpMin = -60.0f;
    public float lookUpMax = 60.0f;

    private GameObject player;
    private Vector2 smoothedVelocity;
    private Vector2 currentLookingPosition;



    private void Start()
    {
        player = transform.parent.gameObject;
        currentLookingPosition = new Vector2(90.0f, 0.0f);
    }

    private void Update()
    {
        RotateCamera();
    }

    private void RotateCamera()
    {
        float rotateX = Input.GetAxisRaw("Mouse X");
        float rotateY = Input.GetAxisRaw("Mouse Y");

        Vector2 inputValue = new Vector2(rotateX, rotateY);

        inputValue = Vector2.Scale(inputValue, new Vector2(mouseSensitivity * smoothing, mouseSensitivity * smoothing));

        smoothedVelocity.x = Mathf.Lerp(smoothedVelocity.x, inputValue.x, 1f / smoothing);
        smoothedVelocity.y = Mathf.Lerp(smoothedVelocity.y, inputValue.y, 1f / smoothing);

        currentLookingPosition += smoothedVelocity;
        currentLookingPosition.y = Mathf.Clamp(currentLookingPosition.y, lookUpMin, lookUpMax);

        //Debug.Log(currentLookingPosition);

        transform.localRotation = Quaternion.AngleAxis(-currentLookingPosition.y, Vector3.right);

        player.transform.localRotation = Quaternion.AngleAxis(currentLookingPosition.x, player.transform.up);
    }

    public void RotateOnY(float startX, float startY, float diff)
    {
        currentLookingPosition = new Vector2((startX + diff)%360, startY);
    }

}
