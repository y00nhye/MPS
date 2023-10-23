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
            metal_Check = Random.Range(0, 2); //�������� metal ���� �ο�

            if (metal_Check == 1) //metal �� ���
            {
                FindObjectOfType<StopperController>().Stopper(); //stopper �۵�

                conveyorController.MC_state = 1; //conveyor�� ��Ż ���� ����
                conveyorController.isOn = false; //�����̾� ���� ���� (��������)
                conveyorController.ConveyorBtn(); //�����̾� ����
            }
        }
    }
}
