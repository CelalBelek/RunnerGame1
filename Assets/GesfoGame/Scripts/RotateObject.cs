using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    private Transform localTrans;

    private Vector3 localRotate;

    public float rotateSpeed = 25f;

    void Start()
    {
        localTrans = GetComponent<Transform>();
    }

    void Update()
    {
        localRotate = transform.eulerAngles;
        localTrans.transform.rotation = Quaternion.Euler(localRotate.x, localRotate.y + rotateSpeed * Time.deltaTime, localRotate.z);
    }
}
