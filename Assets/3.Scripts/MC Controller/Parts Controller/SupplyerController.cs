using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyerController : MonoBehaviour
{
    //reset 스크립트 목록
    TransferController transfer;
    ConveyorController conveyor;
    StopperController stopper;
    GripperController gripper;
    GripController grip;
    StoreController store;

    [SerializeField] GameObject plate;
    [SerializeField] Transform supplyPoint;

    private void Awake()
    {
        transfer = FindObjectOfType<TransferController>();
        conveyor = FindObjectOfType<ConveyorController>();
        stopper = FindObjectOfType<StopperController>();
        gripper = FindObjectOfType<GripperController>();
        grip = FindObjectOfType<GripController>();
        store = FindObjectOfType<StoreController>();
    }

    public void SupplyBtn(int state)
    {
        if (state == 1)
        {
            Reset_MC(); //공급과 동시에 모든 장치 리셋

            plate.transform.position = supplyPoint.position;

            plate.SetActive(true);
        }
    }

    public void Reset_MC()
    {
        StopAllCoroutines();

        transfer.TransferReset();
        conveyor.ConveyorReset();
        stopper.StopperReset();
        gripper.GripperReset();
        grip.GripReset();
        store.StoreReset();

        plate.transform.parent = null;
        plate.SetActive(false);
    }
}
