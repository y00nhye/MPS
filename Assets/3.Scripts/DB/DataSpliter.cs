using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSpliter : MonoBehaviour
{
    string data = null;

    char[] Cdata_array = new char[8];
    public int[] Idata_array = new int[8];

    //������ �и��ϱ�
    public int[] SplitData()
    {
        data = DBManager.Instance.gpio;

        Cdata_array = data.ToCharArray(0, 7); //string �����͸� char ���·� �ɰ���

        for (int i = 0; i < Cdata_array.Length; i++)
        {
            Idata_array[i] = (int)char.GetNumericValue(Cdata_array[i]); //char �����͸� int�� ����ȯ �ϱ�
        }

        return Idata_array;
    }
}
