using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GripperController : MonoBehaviour
{
    [Range(0.1f, 2.5f)]
    [SerializeField] float moveSpeed = 1f;

    //움직일 실린더
    [SerializeField] GameObject gripperV_syl;
    [SerializeField] GameObject gripperH_syl;

    [SerializeField] Transform[] gripperV_pos = new Transform[2]; //수직 이동
    [SerializeField] Transform[] gripperH_pos = new Transform[2]; //수평 이동

    private bool isOn_V = false;
    private bool isOn_H = false;

    Coroutine gripperV = null;
    Coroutine gripperH = null;

    StoreController storeController;

    private void Awake()
    {
        storeController = FindObjectOfType<StoreController>();
    }

    public void GripperVBtn(int state)
    {
        if (state == 0)
        {
            isOn_V = false;
        }
        else if (state == 1)
        {
            isOn_V = true;
        }
        else if (state == 2)
        {
            isOn_V = !isOn_V;
        }

        if (isOn_V) //down 상태일 때
        {
            if (gripperV != null) StopCoroutine(gripperV);

            gripperV = StartCoroutine(Gripper_co(gripperV_syl, gripperV_pos[1].position));
        }
        else //up 상태일 때 
        {
            if (gripperV != null) StopCoroutine(gripperV);

            gripperV = StartCoroutine(Gripper_co(gripperV_syl, gripperV_pos[0].position));
        }
    }
    public void GripperHBtn(int state)
    {
        if (state == 0)
        {
            isOn_H = false;
        }
        else if (state == 1)
        {
            isOn_H = true;
        }
        else if (state == 2)
        {
            isOn_H = !isOn_H;
        }

        if (isOn_H)
        {
            if (gripperH != null) StopCoroutine(gripperH);

            gripperH = StartCoroutine(Gripper_co(gripperH_syl, gripperH_pos[1].position));
        }
        else
        {
            if (gripperH != null) StopCoroutine(gripperH);

            gripperH = StartCoroutine(Gripper_co(gripperH_syl, gripperH_pos[0].position));
        }
    }

    IEnumerator Gripper_co(GameObject Syl, Vector3 pos)
    {
        while (Vector3.Distance(Syl.transform.position, pos) > 0.01f)
        {
            Syl.transform.position = Vector3.MoveTowards
                (Syl.transform.position, pos, moveSpeed * Time.deltaTime);

            yield return null;
        }

        Syl.transform.position = pos;

        AutoMC.Instance.isReady = true;
    }

    public void GripperReset()
    {
        if (gripperV != null) StopCoroutine(gripperV);
        if (gripperH != null) StopCoroutine(gripperH);

        isOn_V = false;
        isOn_H = false;

        gripperV_syl.transform.position = gripperV_pos[0].position;
        gripperH_syl.transform.position = gripperH_pos[0].position;

        gripperV = null;
        gripperH = null;
    }
}
