using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreController : MonoBehaviour
{
    [Range(0.1f, 2.5f)]
    [SerializeField] float moveSpeed = 1f;

    [SerializeField] Transform[] store_pos = new Transform[4]; //���� ���� ���� ����

    Coroutine store = null; //���� �������� �ڷ�ƾ ��� ����

    private int MC_state = 0; //�ӽ� Ÿ�� ��ġ ����

    public void StoreBtn()
    {
        if (store != null) StopCoroutine(store);

        store = StartCoroutine(Store_co(MC_state));
    }

    IEnumerator Store_co(int state)
    {
        while (Vector3.Distance(transform.position, store_pos[state].position) > 0.01f)
        {
            transform.position = Vector3.MoveTowards
                (transform.position, store_pos[state].position, moveSpeed * Time.deltaTime);

            yield return null;
        }
        transform.position = store_pos[state].position;
    }

    public void StoreReset()
    {
        MC_state = Random.Range(1, 4);

        store = null;

        transform.position = store_pos[0].position;
    }
}
