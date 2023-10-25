using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMC : MonoBehaviour
{
    [Header("Auto generate Parts")]
    [SerializeField] SupplyerController supplyer;
    [SerializeField] TransferController transfer;
    [SerializeField] ConveyorController conveyor;
    [SerializeField] GripperController gripper;
    [SerializeField] GripController grip;
    [SerializeField] StoreController store;

    [Space(20)]
    [SerializeField] int[] dbData = new int[7];

    DataSpliter spliter;

    private void Awake()
    {
        spliter = FindObjectOfType<DataSpliter>();
    }

    private void Update()
    {
        AutoStart();
    }

    public void AutoStart()
    {
        dbData = spliter.SplitData();
        
        for (int i = 0; i < dbData.Length; i++)
        {
            switch (i)
            {
                case 0:
                    supplyer.SupplyBtn(dbData[i]);
                    break;
                case 1:
                    transfer.TransferBtn(dbData[i]);
                    break;
                case 2:
                    conveyor.ConveyorBtn(dbData[i]);
                    break;
                case 3:
                    gripper.GripperVBtn(dbData[i]);
                    break;
                case 4:
                    gripper.GripperHBtn(dbData[i]);
                    break;
                case 5:
                    grip.GripBtn(dbData[i]);
                    break;
                case 6:
                    store.StoreBtn(dbData[i]);
                    break;

            }
        }
    }
}
