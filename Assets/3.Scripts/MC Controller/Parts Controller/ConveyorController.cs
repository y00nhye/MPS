using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorController : MonoBehaviour
{
    [Range(0.1f, 2.5f)]
    [SerializeField] float moveSpeed = 1f;

    [SerializeField] Transform[] conveyor_pos = new Transform[3]; //시작 도착 지점 설정

    [HideInInspector] public bool isOn = false; //현재 on/off 상태 확인

    Coroutine conveyor = null; //현재 실행중인 코루틴 담는 변수
    GameObject plate = null;

    public int MC_state = 2; //머신 타겟 위치 설정 (sensor 콜라이더에서 랜덤으로 결정)

    private void OnTriggerEnter(Collider other) //plate 자식으로 상속
    {
        if (other.CompareTag("Plate"))
        {
            other.transform.parent = transform;

            plate = other.gameObject;
        }
    }

    public void ConveyorBtn(int state)
    {
        if (transform.Find("Plate") != null) //plate 가 자식으로 들어왔을 때 동작
        {
            if (state == 0)
            {
                isOn = false;
            }
            else if (state == 1)
            {
                isOn = true;
            }
            else if (state == 2)
            {
                isOn = !isOn;
            }

            if (isOn) //on 상태일 때
            {
                if (conveyor != null) StopCoroutine(conveyor);

                conveyor = StartCoroutine(Conveyor_co(MC_state));
            }
            else //off 상태일 때 
            {
                if (conveyor != null) StopCoroutine(conveyor);
            }
        }
    }

    IEnumerator Conveyor_co(int state)
    {
        while (Vector3.Distance(plate.transform.position, conveyor_pos[state].position) > 0.01f)
        {
            plate.transform.position = Vector3.MoveTowards
                (plate.transform.position, conveyor_pos[state].position, moveSpeed * Time.deltaTime);

            yield return null;
        }
        plate.transform.position = conveyor_pos[state].position;

        if(MC_state == 2)
        {
            plate.SetActive(false);
            AutoMC.Instance.Simulation();
        }
        else
        {
            AutoMC.Instance.isReady = true;
        }
    }

    public void ConveyorReset()
    {
        MC_state = 2;
        
        if (conveyor != null) StopCoroutine(conveyor);

        conveyor = null;

        isOn = false;
    }
}
