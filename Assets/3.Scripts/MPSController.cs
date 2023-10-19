using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MPSController : MonoBehaviour
{
    [Range(0.1f, 2.5f)]
    [SerializeField] float moveSpeed;

    [Header("Plate Pref")]
    [SerializeField] GameObject plate;

    [Space(10f)]

    [Header("Supplyer")]
    [SerializeField] Transform supplyPoint;

    [Space(10f)]

    [Header("TransFer")]
    [SerializeField] Transform transfer_Mov;
    [SerializeField] Transform transfer_sta;
    [SerializeField] Transform transfer_tar;

    [Space(10f)]

    [Header("Conveyor")]
    [SerializeField] Transform conveyor_sta;
    [SerializeField] Transform conveyor_stop;
    [SerializeField] Transform conveyor_tar;

    [Space(10f)]

    [Header("Stopper")]
    [SerializeField] Transform stopper_Mov;
    [SerializeField] Transform stopper_sta;
    [SerializeField] Transform stopper_tar;

    [Space(10f)]

    [Header("Grip")]
    [SerializeField] Transform grip_hor_Mov;
    [SerializeField] Transform grip_hor_sta;
    [SerializeField] Transform grip_hor_tar;

    [Space(7f)]

    [SerializeField] Transform grip_ver_Mov;
    [SerializeField] Transform grip_ver_sta;
    [SerializeField] Transform grip_ver_tar;

    [Space(7f)]

    [SerializeField] Transform gripPoint;

    [Space(10f)]

    [Header("Store")]
    [SerializeField] Transform store_Mov;
    [SerializeField] Transform store_sta;
    [SerializeField] Transform[] store_tar = new Transform[3];

    [Space(10f)]

    [Header("Bin")]
    [SerializeField] Transform binPoint;

    [Header("Buttons")]
    [SerializeField] Button supplyer;
    [SerializeField] Button transfer;
    [SerializeField] Button conveyor;
    [SerializeField] Button gripper_V;
    [SerializeField] Button gripper_H;
    [SerializeField] Button grip;
    [SerializeField] Button store;

    [HideInInspector]
    public bool isGrip = false;

    public void SupplyBtn()
    {
        store_Mov.position = store_sta.position;
        stopper_Mov.position = stopper_sta.position;
        grip.interactable = false;
        conveyor.interactable = false;

        plate.transform.position = supplyPoint.position;

        plate.SetActive(true);
    }

    public void TransferBtn()
    {
        StartCoroutine(Transfer_co());
    }

    public void ConveyorBtn()
    {
        StartCoroutine(Conveyor_co());
    }

    public void Stopper()
    {
        StartCoroutine(Stopper_co());
    }

    public void Grip_VBtn()
    {
        StartCoroutine(Gripper_Ver_co());
    }

    public void Grip_HBtn()
    {
        StartCoroutine(Gripper_Hor_co());
    }

    public void GripBtn()
    {
        if (!isGrip)
        {
            plate.transform.position = gripPoint.position;
            plate.transform.parent = gripPoint;

            isGrip = true;
        }
        else
        {
            isGrip = false;
        }
    }

    public void StoreBtn()
    {
        StartCoroutine(Store_co());
    }

    IEnumerator Transfer_co()
    {
        supplyer.interactable = false;
        transfer.interactable = false;
        
        while (Vector3.Distance(transfer_Mov.position, transfer_tar.position) > 0.01f)
        {
            transfer_Mov.position += new Vector3(1, 0, 0) * moveSpeed * Time.deltaTime;

            yield return null;
        }

        transfer_Mov.position = transfer_tar.position;
        conveyor.interactable = true;

        plate.transform.parent = null;

        yield return new WaitForSeconds(1f);

        while (Vector3.Distance(transfer_Mov.position, transfer_sta.position) > 0.01f)
        {
            transfer_Mov.position -= new Vector3(1, 0, 0) * moveSpeed * Time.deltaTime;
            yield return null;
        }

        transfer.interactable = true;
    }

    IEnumerator Conveyor_co()
    {
        supplyer.interactable = false;
        conveyor.interactable = false;
        gripper_V.interactable = false;
        
        while (Vector3.Distance(plate.transform.position, conveyor_stop.position) > 0.01f)
        {
            plate.transform.position += new Vector3(1, 0, 0) * moveSpeed * Time.deltaTime;

            yield return null;
        }

        plate.transform.position = conveyor_stop.position;

        if (plate.GetComponent<PlateSet>().metal_Check == 0)
        {
            while (Vector3.Distance(plate.transform.position, conveyor_tar.position) > 0.01f)
            {
                plate.transform.position += new Vector3(1, 0, 0) * moveSpeed * Time.deltaTime;

                yield return null;
            }

            plate.transform.position = conveyor_tar.position;

            while (Vector3.Distance(plate.transform.position, binPoint.position) > 0.01f)
            {
                plate.transform.position = Vector3.MoveTowards(plate.transform.position, binPoint.position, moveSpeed * Time.deltaTime);

                yield return null;
            }

            plate.transform.position = binPoint.position;
            plate.SetActive(false);
        }
        else
        {
            grip.interactable = true;
        }

        supplyer.interactable = true;
        gripper_V.interactable = true;
    }

    IEnumerator Stopper_co()
    {
        while (Vector3.Distance(stopper_Mov.position, stopper_tar.position) > 0.01f)
        {
            stopper_Mov.position -= new Vector3(0, 1, 0) * moveSpeed * Time.deltaTime;

            yield return null;
        }

        stopper_Mov.position = stopper_tar.position;
    }

    IEnumerator Gripper_Ver_co()
    {
        supplyer.interactable = false;
        gripper_V.interactable = false;
        gripper_H.interactable = false;
        
        if (grip_ver_Mov.position != grip_ver_tar.position)
        {
            while (Vector3.Distance(grip_ver_Mov.position, grip_ver_tar.position) > 0.01f)
            {
                grip_ver_Mov.position -= new Vector3(0, 1, 0) * moveSpeed * Time.deltaTime;
                yield return null;
            }

            grip_ver_Mov.position = grip_ver_tar.position;
        }
        else
        {
            while (Vector3.Distance(grip_ver_Mov.position, grip_ver_sta.position) > 0.01f)
            {
                grip_ver_Mov.position += new Vector3(0, 1, 0) * moveSpeed * Time.deltaTime;
                yield return null;
            }

            grip_ver_Mov.position = grip_ver_sta.position;
        }

        supplyer.interactable = true;
        gripper_V.interactable = true;
        gripper_H.interactable = true;

    }

    IEnumerator Gripper_Hor_co()
    {
        supplyer.interactable = false;
        gripper_V.interactable = false;
        gripper_H.interactable = false;

        if (grip_hor_Mov.position != grip_hor_tar.position)
        {
            while (Vector3.Distance(grip_hor_Mov.position, grip_hor_tar.position) > 0.01f)
            {
                grip_hor_Mov.position -= new Vector3(0, 0, 1) * moveSpeed * Time.deltaTime;

                yield return null;
            }

            grip_hor_Mov.position = grip_hor_tar.position;
        }
        else
        {
            while (Vector3.Distance(grip_hor_Mov.position, grip_hor_sta.position) > 0.01f)
            {
                grip_hor_Mov.position += new Vector3(0, 0, 1) * moveSpeed * Time.deltaTime;

                yield return null;
            }

            grip_hor_Mov.position = grip_hor_sta.position;
        }

        supplyer.interactable = true;
        gripper_V.interactable = true;
        gripper_H.interactable = true;
    }

    IEnumerator Store_co()
    {
        store.interactable = false;
        
        Transform tar = store_tar[Random.Range(0, 3)];

        while (Vector3.Distance(store_Mov.position, tar.position) > 0.01f)
        {
            store_Mov.position = Vector3.MoveTowards(store_Mov.position, tar.position, moveSpeed * Time.deltaTime);

            yield return null;
        }

        store_Mov.position = tar.position;

        store.interactable = true;
    }
}
