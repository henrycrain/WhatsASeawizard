using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform target;
    public Transform pivot;
    
    public float rotateSpeed;
    public float maxViewAngle;
    public float minViewAngle;

    public bool invertY;

    private Vector3 offset;

    void Start()
    {
        offset = target.position - transform.position;

        pivot.transform.position = target.transform.position;
        pivot.transform.parent = target.transform;

        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        //HandleInput();
        HandleCameraMove();
    }
    
    /// <summary>
    /// Temporary method to unlock cursor until menus are implemented
    /// </summary>
    void HandleInput()
    {
        if (Cursor.lockState == CursorLockMode.Locked && Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else if (Cursor.lockState != CursorLockMode.Locked && Input.GetMouseButton(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    void HandleCameraMove()
    {
        float horizontal = 0f;
        float vertical = 0f;
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            horizontal = Input.GetAxis("Mouse X") * rotateSpeed;
            vertical = Input.GetAxis("Mouse Y") * rotateSpeed;
            vertical *= invertY ? 1 : -1;
        }

        target.Rotate(0.0f, horizontal, 0.0f);

        pivot.Rotate(vertical, 0.0f, 0.0f);

        float xAngle = pivot.rotation.eulerAngles.x;
        if (xAngle > maxViewAngle && xAngle < 180)
        {
            pivot.rotation = Quaternion.Euler(maxViewAngle, 0.0f, 0.0f);
        }
        else if (xAngle > 180 && xAngle < (360 + minViewAngle))
        {
            pivot.rotation = Quaternion.Euler(360 + minViewAngle, 0.0f, 0.0f);
        }

        float targetYAngle = target.eulerAngles.y;
        float targetXAngle = pivot.eulerAngles.x;

        var rotation = Quaternion.Euler(targetXAngle, targetYAngle, 0.0f);
        Transform trans = transform;
        trans.position = target.position - (rotation * offset);
        Vector3 position = trans.position;

        if (position.y < target.position.y)
        {
            trans.position = new Vector3(position.x,
                                         target.position.y,
                                         position.z);
        }

        trans.LookAt(target);
    }
}
