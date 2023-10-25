using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransferController : MonoBehaviour
{
    [Range(0.1f, 2.5f)]
    [SerializeField] float moveSpeed = 1f;

    [SerializeField] Transform[] transfer_pos = new Transform[2]; //시작 도착 지점 설정

    private bool isOn = false; //현재 on/off 상태 확인

    Coroutine transfer = null; //현재 실행중인 transfer 코루틴 담는 변수

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Plate"))
        {
            other.transform.parent = transform;
        }
    }

    public void TransferBtn(int state)
    {
        if(state == 0)
        {
            isOn = false;
        }
        else if(state == 1)
        {
            isOn = true;
        }
        else if(state == 2)
        {
            isOn = !isOn;
        }

        if (isOn) //on 상태일 때
        {
            if(transfer != null) StopCoroutine(transfer);
            transfer = StartCoroutine(Transfer_co(1));
        }
        else //off 상태일 때 
        {
            //plate 자식 개체에서 분리
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

        AutoMC.Instance.isReady = true;
    }

    public void TransferReset()
    {
        transform.position = transfer_pos[0].position;

        isOn = false;

        transfer = null;
    }
}
