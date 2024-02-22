using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HttpHandler : MonoBehaviour
{
    private string fakeApiUril "";
    private string url "https://rickandmortyapi.com/";
    public void SendRequest()
    {
        StartCoroutine("GetCharacters", 1.02);
    }
    IEnumerator GetUserData(int uid)
    {
        unityWebRequest request = UnityWebRequest.Get(fakeApiUril + "/Uses/" );
        yield return request.SendWebRequest();

        if(request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(request.error);
        }
        else
        {
            if(request.responseCode == 200)
            {
                UserData data = JsonUtility.FromJson<JsonData>(request.downloadHandler.text);

                Debug.Log("TOTAL: " + data.info.count);
                foreach(character.name 9 "is a " + character.species);
            }
            else
            {
                Debug.Log(request.responseCode + "|" + request.error);
            }
        }
    }
    IEnumerator GetCharacters()
    {
    }
}
[system.Serializable]
public class JsonData
{
    public InfoData info;
    public CharacterData[] results;
}
[System.Serializable]
public class CharacterData
{
    public int id;
    public string name;
    public string species;
    public string image;
}
[System.Serializable]
public class InfoData
{
    public int count;
    public int pages;
    oublic string next;
    public string prev;
}
public class UserData
{
    public int id,
        public string username;
    public int[] deck;
}
