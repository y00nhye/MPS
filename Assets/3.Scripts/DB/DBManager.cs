using System.Collections;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Data;
using System.IO;
using System.Threading;
using UnityEngine;

public class DBManager : Singleton<DBManager>
{
    string IP = "127.0.0.1";
    string ID = "root";
    string PW = "1234";
    string DB = "DB";
    string PORT = "3306";

    Thread th_DB_load;
    MySqlConnection conn;
    bool isDBRun;
    int dbConnectionCnt = 3; //db ���� �õ� Ƚ��

    MySqlDataReader reader;

    public string gpio;

    //public string gpio = "1000000";

    private void Start()
    {
        StartSearch();
    }
    private new void OnApplicationQuit()
    {
        if (isDBRun)
        {
            StopSearch();
        }
    }

    private ConnectionState DBConnect() //DB�� �����ϱ� ���� Json���� ����� DB���� ������ �о��, Json�� ���ٸ� DB������ �����ϰ� �о�� | ConnectionsState > ���� ���¸� ��ȯ����
    {
        try
        {
            string path = Application.dataPath + "/config.Json"; //json ���� ���

            if (File.Exists(path)) //���� ��ο� ������ �����Ѵٸ�
            {
                string fileData = File.ReadAllText(path); //json ������ �о����
                List<Dictionary<string, string>> datas = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(fileData); //json ���� ������ ������ȭ �ϱ�
                conn = new MySqlConnection($"server={datas[0]["IP"]}; user id={datas[0]["ID"]}; password={datas[0]["PW"]}; database={datas[0]["DB"]}; Port = {datas[0]["PORT"]}; charset=utf8;"); //���� ���� �ֱ� (json���� �о��)
            }
            else //���� ��ο� ������ �������� �ʴ´ٸ�
            {
                conn = new MySqlConnection($"server={IP}; user id={ID}; password={PW}; database={DB}; Port = {PORT}; charset=utf8;"); //���� ���� �ֱ� (Ŭ�������� �о��)

                List<Dictionary<string, string>> datas = new List<Dictionary<string, string>>();
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("IP", IP); data.Add("PORT", PORT); data.Add("DB", DB);
                data.Add("ID", ID); data.Add("PW", PW);
                datas.Add(data);
                string jsonData = JsonConvert.SerializeObject(datas); //Ŭ������ ������ ����ȭ �ϱ�

                File.WriteAllText(path, jsonData); //json ���Ϸ� �����ϱ�
            }

            conn.Open(); //���� json �۾��� ������ db ������ ����
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex);
        }

        return conn.State; //db ���� ���� ��ȯ
    }

    private void StartSearch() //db ������ Ȯ���ϰ� db���� ã�� ������ ã�� �Լ��� ��Ƽ������� ������
    {
        Debug.Log("DB �˻� ����");

        for (int i = 0; i < dbConnectionCnt; i++) //cnt ��ŭ ���� �õ�
        {
            if (DBConnect() == ConnectionState.Open) //db ���� �Ǿ��ٸ�
            {
                Debug.Log($"DB ���� : {conn.ConnectionString}");

                th_DB_load = new Thread(SQL_Search); //������ �޾ƿ� ��Ƽ������ ��ü ����
                th_DB_load.IsBackground = true; //��Ƽ������ ���� ��ġ ���� (��׶���� ������, ���ξ����� ���� �� ������ ����)
                th_DB_load.Start(); //��Ƽ������ ����
                isDBRun = true;

                return; //����Ǹ� ������
            }
        }

        Debug.Log("DB ���� ����");
    }

    public void SQL_Search()
    {
        try
        {
            while (isDBRun) //db�� ����Ǿ� �ִٸ�
            {
                SelectFromDB(); //db���� gpio ���� �ҷ����� �Լ� ����

                Thread.Sleep(300); //0.3�� ���� ����
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex);
        }
    }

    protected void StopSearch() //db ���� ���� �� ��Ƽ������ ����
    {
        isDBRun = false;
        conn.Close();
        th_DB_load.Join(); //��Ƽ������ ���� ��� (���ν����� ����� �� ���� ��ٸ�)
        th_DB_load.Abort(); //��Ƽ������ ����
    }

    public void SelectFromDB()
    {
        //db ������ �����ִٸ� ���� ��õ�
        if (conn.State != ConnectionState.Open)
        {
            Debug.Log("DB ���� ��õ�");
            if (DBConnect() != ConnectionState.Open)
            {
                Debug.Log("DB ���� ����");
                return;
            }
        }

        try
        {
            string select = "select * from mps";

            MySqlCommand cmd = new MySqlCommand(select, conn);
            reader = cmd.ExecuteReader();
            if (!reader.HasRows) return; //���� �����ϴ��� Ȯ��

            while (reader.Read())
            {
                if (!reader.IsDBNull(0)) //null �� üũ
                {
                    gpio = $"{reader["gpio"]}";
                }
            }

            reader.Close();
        }
        catch (System.Exception e)
        {
            Debug.Log(e.ToString());
        }
        finally
        {
            if (reader != null && !reader.IsClosed)
                reader.Close();
        }
    }

    public void UpdateToDB(string gpio)
    {
        string insert = $"update mps set gpio = @gpio";

        try
        {
            MySqlCommand cmd = new MySqlCommand(insert, conn);
            cmd.Parameters.AddWithValue("@gpio", gpio);

            cmd.ExecuteNonQuery();
        }
        catch (System.Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    private void DefectStatus()
    {

        if (conn.State != ConnectionState.Open)
        {
            Debug.Log("DB ���� ��õ�");
            if (DBConnect() != ConnectionState.Open)
            {
                Debug.Log("DB ���� ����");
                return;
            }
        }


        try
        {

            string sql = @"SELECT CMP_EQ_ID, QLTY_RSLT, BAD_CD, COUNT(BAD_CD)
                        FROM prc_qlty_tb
                        WHERE CMP_EQ_ID = 'EQ02' AND QLTY_RSLT = 'NG'  AND NOW()
                        UNION all

                        SELECT CMP_EQ_ID, QLTY_RSLT, BAD_CD, COUNT(BAD_CD)
                        FROM prc_qlty_tb
                        WHERE CMP_EQ_ID = 'EQ05'  AND QLTY_RSLT = 'NG'AND NOW()
                        UNION all

                        SELECT CMP_EQ_ID, QLTY_RSLT, BAD_CD, COUNT(BAD_CD)
                        FROM prc_qlty_tb
                        WHERE CMP_EQ_ID = 'EQ06' AND QLTY_RSLT = 'NG'  AND NOW(); ";

            MySqlCommand cmd = new MySqlCommand(sql, conn);
            reader = cmd.ExecuteReader();
            if (!reader.HasRows) return;
            while (reader.Read())
            {
                switch (reader["CMP_EQ_ID"].ToString())
                {
                    case "EQ02":
                        if (!reader.IsDBNull(3))
                        {
                            //dbData.Defect_Top = int.Parse(reader[3].ToString()); dbData.Defect_Bottom = int.Parse(reader[3].ToString());
                        }
                        break;
                }
            }
            reader.Close();
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }
        finally
        {
            if (reader != null && !reader.IsClosed)
                reader.Close();
        }
    }
}
