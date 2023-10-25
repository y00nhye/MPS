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
    int dbConnectionCnt = 3; //db 연결 시도 횟수

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

    private ConnectionState DBConnect() //DB에 연결하기 위해 Json에서 저장된 DB정보 파일을 읽어옴, Json이 없다면 DB정보를 저장하고 읽어옴 | ConnectionsState => 연결 상태를 반환해줌
    {
        try
        {
            string path = Application.dataPath + "/config.Json"; //json 저장 경로

            if (File.Exists(path)) //저장 경로에 파일이 존재한다면
            {
                string fileData = File.ReadAllText(path); //json 파일을 읽어오기
                List<Dictionary<string, string>> datas = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(fileData); //json 파일 내용을 역직렬화 하기
                conn = new MySqlConnection($"server={datas[0]["IP"]}; user id={datas[0]["ID"]}; password={datas[0]["PW"]}; database={datas[0]["DB"]}; Port = {datas[0]["PORT"]}; charset=utf8;"); //연결 정보 넣기 (json에서 읽어옴)
            }
            else //저장 경로에 파일이 존재하지 않는다면
            {
                conn = new MySqlConnection($"server={IP}; user id={ID}; password={PW}; database={DB}; Port = {PORT}; charset=utf8;"); //연결 정보 넣기 (클래스에서 읽어옴)

                List<Dictionary<string, string>> datas = new List<Dictionary<string, string>>();
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("IP", IP); data.Add("PORT", PORT); data.Add("DB", DB);
                data.Add("ID", ID); data.Add("PW", PW);
                datas.Add(data);
                string jsonData = JsonConvert.SerializeObject(datas); //클래스의 내용을 직렬화 하기

                File.WriteAllText(path, jsonData); //json 파일로 생성하기
            }

            conn.Open(); //위의 json 작업이 끝나면 db 연결을 열기
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex);
        }

        return conn.State; //db 연결 상태 반환
    }

    private void StartSearch() //db 연결을 확인하고 db에서 찾을 데이터 찾는 함수를 멀티스레드로 돌리기
    {
        Debug.Log("DB 검색 시작");

        for (int i = 0; i < dbConnectionCnt; i++) //cnt 만큼 연결 시도
        {
            if (DBConnect() == ConnectionState.Open) //db 연결 되었다면
            {
                Debug.Log($"DB 연결 : {conn.ConnectionString}");

                th_DB_load = new Thread(SQL_Search); //데이터 받아올 멀티스레드 객체 생성
                th_DB_load.IsBackground = true; //멀티스레드 돌릴 위치 결정 (백그라운드로 돌리기, 메인쓰레드 종료 시 쓰레드 종료)
                th_DB_load.Start(); //멀티스레드 시작
                isDBRun = true;

                return; //연결되면 끝내기
            }
        }

        Debug.Log("DB 연결 실패");
    }

    public void SQL_Search()
    {
        try
        {
            while (isDBRun) //db가 연결되어 있다면
            {
                SelectFromDB(); //db에서 gpio 정보 불러오는 함수 실행

                Thread.Sleep(300); //0.3초 마다 돌기
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex);
        }
    }

    protected void StopSearch() //db 연결 종료 및 멀티쓰레드 종료
    {
        isDBRun = false;
        conn.Close();
        th_DB_load.Join(); //멀티스레드 실행 대기 (메인스레드 종료될 때 까지 기다림)
        th_DB_load.Abort(); //멀티스레드 종료
    }

    public void SelectFromDB()
    {
        string select = "select * from mps";

        try
        {
            MySqlCommand cmd = new MySqlCommand(select, conn);
            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                gpio = $"{reader["gpio"]}";
            }

            reader.Close();
        }
        catch (System.Exception e)
        {
            Debug.Log(e.ToString());
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
}
