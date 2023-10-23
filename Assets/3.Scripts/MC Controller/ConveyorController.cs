using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorController : MonoBehaviour
{
    [Range(0.1f, 2.5f)]
    [SerializeField] float moveSpeed = 1f;

    [SerializeField] Transform[] conveyor_pos = new Transform[3]; //���� ���� ���� ����

    [HideInInspector] public bool isOn = false; //���� on/off ���� Ȯ��

    Coroutine conveyor = null; //���� �������� �ڷ�ƾ ��� ����
    GameObject plate = null;

    public int MC_state = 2; //�ӽ� Ÿ�� ��ġ ���� (sensor �ݶ��̴����� �������� ����)

    private void OnTriggerEnter(Collider other) //plate �ڽ����� ���
    {
        if (other.CompareTag("Plate"))
        {
            other.transform.parent = transform;

            plate = other.gameObject;
        }
    }

    public void ConveyorBtn()
    {
        if (transform.Find("Plate") != null) //plate �� �ڽ����� ������ �� ����
        {
            isOn = !isOn;

            if (isOn) //on ������ ��
            {
                if (conveyor != null) StopCoroutine(conveyor);

                conveyor = StartCoroutine(Conveyor_co(MC_state));
            }
            else //off ������ �� 
            {
                if (conveyor != null) StopCoroutine(conveyor);
            }
        }
    }

    IEnumerator Conveyor_co(int state)
    {
        while (Vector3.Distance(plate.transform.position, conveyor_pos[state].position) > 0.01f)
        {
            plate.transform.position = Vector3.MoveTowards
                (plate.transform.position, conveyor_pos[state].position, moveSpeed * Time.deltaTime);

            yield return null;
        }
        plate.transform.position = conveyor_pos[state].position;

        if(MC_state == 2)
        {
            plate.SetActive(false);
        }
    }

    public void ConveyorReset()
    {
        MC_state = 2;
        
        if (conveyor != null) StopCoroutine(conveyor);

        conveyor = null;

        isOn = false;
    }
}