using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyerController : MonoBehaviour
{
    [SerializeField] TransferController transfer;
    [SerializeField] ConveyorController conveyor;
    [SerializeField] StopperController stopper;
    [SerializeField] GripperController gripper;
    [SerializeField] GripController grip;
    [SerializeField] StoreController store;

    [SerializeField] GameObject plate;
    [SerializeField] Transform supplyPoint;

    public void SupplyBtn()
    {
        Reset_MC(); //공급과 동시에 모든 장치 리셋

        plate.transform.position = supplyPoint.position;

        plate.SetActive(true);
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
