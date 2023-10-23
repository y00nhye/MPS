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

    Coroutine gripper = null;

    StoreController storeController;

    private void Awake()
    {
        storeController = FindObjectOfType<StoreController>();
    }

    public void GripperVBtn()
    {   
        isOn_V = !isOn_V;

        if (isOn_V) //down 상태일 때
        {
            if (gripper != null) StopCoroutine(gripper);

            gripper = StartCoroutine(Gripper_co(gripperV_syl, gripperV_pos[1].position));
        }
        else //up 상태일 때 
        {
            if (gripper != null) StopCoroutine(gripper);

            gripper = StartCoroutine(Gripper_co(gripperV_syl, gripperV_pos[0].position));
        }
    }
    public void GripperHBtn()
    {
        if (!isOn_V) //수직 이동 실린더가 위로 올라가 있을 때만 동작하도록 제한
        {
            isOn_H = !isOn_H;

            if (isOn_H)
            {
                if (gripper != null) StopCoroutine(gripper);

                gripper = StartCoroutine(Gripper_co(gripperH_syl, gripperH_pos[1].position));
            }
            else
            {
                if (gripper != null) StopCoroutine(gripper);

                gripper = StartCoroutine(Gripper_co(gripperH_syl, gripperH_pos[0].position));
            }
        }
    }

    IEnumerator Gripper_co(GameObject Syl, Vector3 pos)
    {
        while (Vector3.Distance(transform.position, pos) > 0.01f)
        {
            Syl.transform.position = Vector3.MoveTowards
                (Syl.transform.position, pos, moveSpeed * Time.deltaTime);

            yield return null;
        }
        Syl.transform.position = pos;
    }

    public void GripperReset()
    {
        if (gripper != null) StopCoroutine(gripper);

        isOn_V = false;
        isOn_H = false;

        gripperV_syl.transform.position = gripperV_pos[0].position;
        gripperH_syl.transform.position = gripperH_pos[0].position;

        gripper = null;
    }
}
