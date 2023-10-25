using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSpliter : MonoBehaviour
{
    string data = null;

    char[] Cdata_array = new char[8];
    public int[] Idata_array = new int[8];

    //데이터 분리하기
    public int[] SplitData()
    {
        data = DBManager.Instance.gpio;

        Cdata_array = data.ToCharArray(0, 7); //string 데이터를 char 형태로 쪼개기

        for (int i = 0; i < Cdata_array.Length; i++)
        {
            Idata_array[i] = (int)char.GetNumericValue(Cdata_array[i]); //char 데이터를 int로 형변환 하기
        }

        return Idata_array;
    }
}
