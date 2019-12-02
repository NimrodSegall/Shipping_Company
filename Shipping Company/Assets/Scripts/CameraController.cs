using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 1f;
    [SerializeField]
    private float rotSpeed = 1f;

    [SerializeField]
    private float movementTime = 5f;
    [SerializeField]
    private float rotationTime = 5f;

    [SerializeField]
    private Transform camTransform = null;
    [SerializeField]
    private Vector3 zoomAmount = new Vector3(0, 10, -10);
    [SerializeField]
    private float zoomTime = 5f;

    private Vector3 newPos = Vector3.zero;
    private Quaternion newRot = new Quaternion();
    private Vector3 newZoom = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        newPos = transform.position;
        newRot = transform.rotation;
        newZoom = camTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Rotate();
        Zoom();
    }

    private void Move()
    {
        if(Inputs.get.up)
        {
            newPos += transform.forward * moveSpeed;
        }

        if (Inputs.get.down)
        {
            newPos += -transform.forward * moveSpeed;
        }

        if (Inputs.get.left)
        {
            newPos += -transform.right * moveSpeed;
        }

        if (Inputs.get.right)
        {
            newPos += transform.right * moveSpeed;
        }

        transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * movementTime);
    }

    private void Rotate()
    {
        if (Inputs.get.rotClock)
        {
            newRot *= Quaternion.Euler(Vector3.up * rotSpeed);
        }

        if(Inputs.get.rotCounterClock)
        {
            newRot *= Quaternion.Euler(-Vector3.up * rotSpeed);
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, newRot, Time.deltaTime * rotationTime);
    }

    private void Zoom()
    {
        if(Inputs.get.zoomIn)
        {
            newZoom += zoomAmount;
        }

        if (Inputs.get.zoomOut)
        {
            newZoom -= zoomAmount;
        }

        camTransform.localPosition = Vector3.Lerp(camTransform.localPosition, newZoom, Time.deltaTime * zoomTime);
    }
}
