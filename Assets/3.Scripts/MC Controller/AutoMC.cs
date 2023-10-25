using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMC : Singleton<AutoMC>
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

    public bool isSimulationOn = false;
    public bool isReady = true;

    int simulCnt;

    private void Awake()
    {
        spliter = FindObjectOfType<DataSpliter>();
    }

    private void Update()
    {
        /*if (!isSimulationOn)
        {
            DBDataUpdate();
        }*/

        if (isSimulationOn)
        {
            if (isReady)
            {
                isReady = false;

                switch (simulCnt)
                {
                    case 0:
                        supplyer.SupplyBtn(1);
                        break;
                    case 1:
                        transfer.TransferBtn(1);
                        break;
                    case 2:
                        conveyor.ConveyorBtn(1);
                        break;
                    case 3:
                        gripper.GripperVBtn(1);
                        break;
                    case 4:
                        grip.GripBtn(1);
                        break;
                    case 5:
                        gripper.GripperVBtn(0);
                        break;
                    case 6:
                        gripper.GripperHBtn(1);
                        break;
                    case 7:
                        store.StoreBtn(1);
                        break;
                    case 8:
                        gripper.GripperVBtn(1);
                        break;
                    case 9:
                        grip.GripBtn(0);
                        break;
                    case 10:
                        gripper.GripperVBtn(0);
                        break;
                    default:
                        Simulation();
                        simulCnt--;
                        break;
                }

                Debug.Log(simulCnt);
                simulCnt++;

            }
        }
    }

    public void DBDataUpdate()
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

    public void Simulation()
    {
        simulCnt = 0;
        isSimulationOn = true;
        isReady = true;
    }
    public void simulationStop()
    {
        simulCnt = 0;
        isSimulationOn = false;
        isReady = false;

        FindObjectOfType<SupplyerController>().Reset_MC();
    }
}
