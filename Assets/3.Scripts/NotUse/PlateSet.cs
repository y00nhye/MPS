using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateSet : MonoBehaviour
{
    public int metal_Check = 0;

    MPSController mpsController;

    /*private void Awake()
    {
        mpsController = FindObjectOfType<MPSController>();
    }*/

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Transfer"))
        {
            transform.parent = other.transform;
        }

        if (other.CompareTag("Sensor"))
        {
            metal_Check = Random.Range(0, 2);

            if (metal_Check == 1)
            {
                mpsController.Stopper();
            }
        }
    }*/

    /*private void OnTriggerStay(Collider other)
    {
        if (!mpsController.isGrip)
        {
            if (other.CompareTag("Store"))
            {
                transform.parent = other.transform;
            }
        }
    }*/
}
