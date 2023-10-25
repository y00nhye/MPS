using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopperController : MonoBehaviour
{
    [Range(0.1f, 2.5f)]
    [SerializeField] float moveSpeed = 1f;

    [SerializeField] Transform[] Stopper_pos = new Transform[2]; //시작 도착 지점 설정

    public void Stopper()
    {
        StartCoroutine(Stopper_co(1));
    }

    IEnumerator Stopper_co(int state)
    {
        while (Vector3.Distance(transform.position, Stopper_pos[state].position) > 0.01f)
        {
            transform.position = Vector3.MoveTowards
                (transform.position, Stopper_pos[state].position, moveSpeed * Time.deltaTime);

            yield return null;
        }
        transform.position = Stopper_pos[state].position;
    }

    public void StopperReset()
    {
        transform.position = Stopper_pos[0].position;
    }
}
