using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalObstacleControll : MonoBehaviour
{
    private Transform localTrans;

    private Vector3 newPosForTrans;

    private float horizontalSpeed = 0.00001f;

    public float moveSpeed = 0.15f;

    public bool directionBool;

    void Start()
    {
        localTrans = GetComponent<Transform>();
    }

    void Update()
    {
        if (!directionBool)
        {
            newPosForTrans.x = localTrans.position.x - horizontalSpeed;
            newPosForTrans.y = localTrans.position.y;
            newPosForTrans.z = localTrans.position.z;

            localTrans.position = newPosForTrans + localTrans.right * moveSpeed * Time.deltaTime;
        }
        else
        {
            newPosForTrans.x = localTrans.position.x + horizontalSpeed;
            newPosForTrans.y = localTrans.position.y;
            newPosForTrans.z = localTrans.position.z;

            localTrans.position = newPosForTrans - localTrans.right * moveSpeed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
            if (other.name == "LeftTrigger")
                directionBool = false;
            else if (other.name == "RightTrigger")
                directionBool = true;
    }
}
