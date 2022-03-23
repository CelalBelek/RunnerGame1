using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingObstacle : MonoBehaviour
{
    private Transform localTrans;

    private Vector3 localRotate;

    public float rotateSpeed = 15f;

    void Start()
    {
        localTrans = GetComponent<Transform>();
    }

    void Update()
    {
        localRotate = transform.eulerAngles;
        localTrans.transform.rotation = Quaternion.Euler(localRotate.x, localRotate.y, localRotate.z + rotateSpeed * Time.deltaTime);
    }
}
