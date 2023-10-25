using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GripController : MonoBehaviour
{
    private bool isGrip = false; //grip 상태 확인 변수

    private bool canGrip = false; //grip 할 수 있는 상태 확인 변수

    [SerializeField] private GameObject plate = null;

    //plate와 grip의 trigger 체크로 canGrip 상태 변환
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Plate"))
        {
            canGrip = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Plate"))
        {
            canGrip = false;
        }
    }

    public void GripBtn(int state)
    {
        if (canGrip) //grip 할 수 있는 상황에서 isGrip 상태 변경
        {
            if (state == 0)
            {
                isGrip = false;
            }
            else if (state == 1)
            {
                isGrip = true;
            }
            else if (state == 2)
            {
                isGrip = !isGrip;
            }

            if (isGrip) //plate 잡은 상태
            {
                plate.GetComponent<Rigidbody>().useGravity = false;

                plate.transform.parent = transform;
            }
            else //plate 놓은 상태
            {
                plate.GetComponent<Rigidbody>().useGravity = true;

                plate.transform.parent = null;
            }
        }
    }

    public void GripReset()
    {
        plate.GetComponent<Rigidbody>().useGravity = false;

        isGrip = false;
        canGrip = false;
    }
}
