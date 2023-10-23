using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GripController : MonoBehaviour
{
    private bool isGrip = false; //grip ���� Ȯ�� ����

    private bool canGrip = false; //grip �� �� �ִ� ���� Ȯ�� ����

    [SerializeField] private GameObject plate = null;

    //plate�� grip�� trigger üũ�� canGrip ���� ��ȯ
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

    public void GripBtn()
    {
        if (canGrip) //grip �� �� �ִ� ��Ȳ���� isGrip ���� ����
        {
            isGrip = !isGrip;

            if (isGrip) //plate ���� ����
            {
                plate.GetComponent<Rigidbody>().useGravity = false;

                plate.transform.parent = transform;
            }
            else //plate ���� ����
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
