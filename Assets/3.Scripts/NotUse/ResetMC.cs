using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetMC : MonoBehaviour
{
    [SerializeField] TransferController transfer;
    [SerializeField] ConveyorController conveyor;
    [SerializeField] StopperController stopper;
    [SerializeField] GripperController gripper;
    [SerializeField] GripController grip;
    [SerializeField] StoreController store;

    [SerializeField] GameObject plate;
    [SerializeField] Transform binPoint;

    private void Start()
    {
        Reset_MC();
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
        plate.transform.position = binPoint.position;
        plate.SetActive(false);
    }
}
