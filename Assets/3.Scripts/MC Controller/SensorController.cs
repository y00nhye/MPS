using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorController : MonoBehaviour
{
    public int metal_Check = 0;

    ConveyorController conveyorController;

    private void Awake()
    {
        conveyorController = FindObjectOfType<ConveyorController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Plate"))
        {
            metal_Check = Random.Range(0, 2); //랜덤으로 metal 상태 부여

            if (metal_Check == 1) //metal 일 경우
            {
                FindObjectOfType<StopperController>().Stopper(); //stopper 작동

                conveyorController.MC_state = 1; //conveyor의 메탈 상태 변경
                conveyorController.isOn = false; //컨베이어 상태 변경 (정지상태)
                conveyorController.ConveyorBtn(); //컨베이어 실행
            }
        }
    }
}
