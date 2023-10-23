using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransferController : MonoBehaviour
{
    [Range(0.1f, 2.5f)]
    [SerializeField] float moveSpeed = 1f;

    [SerializeField] Transform[] transfer_pos = new Transform[2]; //���� ���� ���� ����

    private bool isOn = false; //���� on/off ���� Ȯ��

    Coroutine transfer = null; //���� �������� transfer �ڷ�ƾ ��� ����

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Plate"))
        {
            other.transform.parent = transform;
        }
    }

    public void TransferBtn()
    {
        isOn = !isOn; //on/off ���� ����

        if (isOn) //on ������ ��
        {
            if(transfer != null) StopCoroutine(transfer);
            transfer = StartCoroutine(Transfer_co(1));
        }
        else //off ������ �� 
        {
            //plate �ڽ� ��ü���� �и�
            if(transform.Find("Plate") != null)
            {
                transform.Find("Plate").parent = null;
            }
            
            if (transfer != null) StopCoroutine(transfer);
            transfer = StartCoroutine(Transfer_co(0));
        }
    }

    IEnumerator Transfer_co(int state)
    {
        while (Vector3.Distance(transform.position, transfer_pos[state].position) > 0.01f)
        {
            transform.position = Vector3.MoveTowards
                (transform.position, transfer_pos[state].position, moveSpeed * Time.deltaTime);

            yield return null;
        }
        transform.position = transfer_pos[state].position;
    }

    public void TransferReset()
    {
        transform.position = transfer_pos[0].position;

        isOn = false;

        transfer = null;
    }
}
