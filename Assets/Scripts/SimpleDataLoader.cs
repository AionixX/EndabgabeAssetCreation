using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace SimpleLibrary
{
    public static class SimpleDataLoader
    {
        public static void SaveData<T>(T _data, string _path)
        {
            try
            {
                Serialize<T>(_data, _path);
            }
            catch (Exception e)
            {
                Debug.LogError("CAN'T SAVE DATA! - REASON: " + e.Message);
                DeleteData(_path);
            }
        }

        public static T LoadData<T>(string _path)
        {
            try
            {
                return Deserialize<T>(_path);
            }
            catch (Exception e)
            {
                Debug.LogError("CAN'T LOAD DATA! - REASON: " + e.Message);
            }
            T result = default(T);
            return result;
        }

        public static bool DeleteData(string _path)
        {
            if (File.Exists(_path))
            {
                File.Delete(_path);
                return true;
            }
            return false;
        }

        private static void Serialize<T>(T _data, string _path)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(_path, FileMode.Create);

            formatter.Serialize(stream, _data);
            stream.Close();
        }

        private static T Deserialize<T>(string _path)
        {
            if (File.Exists(_path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(_path, FileMode.Open);

                T data = (T)formatter.Deserialize(stream);
                stream.Close();
                return data;
            }
            else
                Debug.LogError("FILE DOESN'T EXISTS ON GIVEN PATH!");
                
            T result = default(T);
            return result;
        }

        //  public static bool CheckIfIsSerializable<T>(T _data) {
        //     Serialize(_data, serializePath);
        //     T deserialized = Deserialize<T>(serializePath);
        //     Debug.Log(_data.ToString());
        //     Debug.Log(deserialized.ToString());

        //     // DeleteData(serializePath);

        //     return _data.Equals(deserialized);
        // }


        // static string path = "/player.data";
        // public static PlayerData LoadPlayer()
        // {
        //     string path = Application.persistentDataPath + StateManager.path;

        //     if (File.Exists(path))
        //     {
        //         BinaryFormatter formatter = new BinaryFormatter();
        //         FileStream stream = new FileStream(path, FileMode.Open);

        //         PlayerData data = formatter.Deserialize(stream) as PlayerData;
        //         stream.Close();
        //         return data;
        //     }

        //     Debug.Log("Load - LOADED PLAYER COMPLETED");
        //     return CreateNewPlayerData();
        // }

        // public static void SavePlayer(PlayerData data)
        // {
        //     BinaryFormatter formatter = new BinaryFormatter();
        //     string path = Application.persistentDataPath + StateManager.path;
        //     FileStream stream = new FileStream(path, FileMode.Create);

        //     formatter.Serialize(stream, data);
        //     stream.Close();

        //     Player.RealodPlayerData();
        //     Debug.Log("Save - COMPLETED");
        // }

        // public static PlayerData CreateNewPlayerData()
        // {
        //     PlayerData newData = new PlayerData();
        //     newData.NAME = "Player";

        //     PlayerAureaData golem = new PlayerAureaData();
        //     golem.aureaName = "Golem";
        //     golem.aureaLevel = 1;
        //     newData.AddAurea(golem);
        //     newData.AddAureaToSquad("Golem");

        //     PlayerAureaData inkubus = new PlayerAureaData();
        //     inkubus.aureaName = "Inkubus";
        //     inkubus.aureaLevel = 1;
        //     newData.AddAurea(inkubus);
        //     newData.AddAureaToSquad("Inkubus");

        //     PlayerAureaData crystal = new PlayerAureaData();
        //     crystal.aureaName = "Crystal";
        //     crystal.aureaLevel = 1;
        //     newData.AddAurea(crystal);
        //     newData.AddAureaToSquad("Crystal");


        //     SavePlayer(newData);

        //     Debug.Log(Application.persistentDataPath + StateManager.path);
        //     return newData;
        // }

        // public static bool DeletePlayerData()
        // {
        //     //TODO
        //     Debug.Log("Delete - DELETION OF PLAYER DATA COMPLETED");
        //     return false;
        // }
    }
}
