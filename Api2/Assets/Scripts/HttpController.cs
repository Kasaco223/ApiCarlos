using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class HttpController: MonoBehaviour
{


    public TextMeshProUGUI nombre;


    static int next = 0;
    [System.Serializable]
    public class ImageTextPair
    {
        public RawImage image;
        public TextMeshProUGUI text;
    }

    public ImageTextPair[] imageTextPairs;

    private string FakeApiUrl = "https://my-json-server.typicode.com/Kasaco223/ApiCarlos";
    private string RickYMortyApiUrl = "https://rickandmortyapi.com/api";
    private Coroutine sendRequest_GetCharacters;
    private int nextImageIndex = 0;
    public void Next()
    {
        next += 1;
        if (next > 2) { next = 0; }
    }
    public void SendRequest(int userId)
    {
        Debug.Log(userId);
        nextImageIndex = 0;
        userId += next;
        if (sendRequest_GetCharacters == null)
            sendRequest_GetCharacters = StartCoroutine(GetUserData(userId));
    }
    IEnumerator GetUserData(int uid)
    {
        UnityWebRequest request = UnityWebRequest.Get(FakeApiUrl + "/users/" + uid);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(request.error);
        }
        else
        {
            if (request.responseCode == 200)
            {
                UserData user = JsonUtility.FromJson<UserData>(request.downloadHandler.text);
                Debug.Log(user.username);
                nombre.text = (user.username);
                List<Coroutine> characterCoroutines = new List<Coroutine>();
                foreach (int cardid in user.deck)
                {
                    Coroutine coroutine = StartCoroutine(GetCharacter(cardid));
                    characterCoroutines.Add(coroutine);
                }

                // Esperar a que se completen todas las corrutinas de los personajes
                foreach (var coroutine in characterCoroutines)
                {
                    yield return coroutine;
                }

                // Todas las solicitudes de personajes se han completado
                Debug.Log("Todas las solicitudes de personajes se han completado");
            }
            else
            {
                Debug.Log(request.responseCode + "|" + request.error);
            }
        }
        sendRequest_GetCharacters = null;
    }


    IEnumerator GetCharacter(int id)
    {
        UnityWebRequest request = UnityWebRequest.Get(RickYMortyApiUrl + "/character/" + id);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(request.error);
        }
        else
        {
            if (request.responseCode == 200)
            {
                Character character = JsonUtility.FromJson<Character>(request.downloadHandler.text);
                Debug.Log(character.name + " is a " + character.species);
                Debug.Log(character.image);

                int index = nextImageIndex; // Usar el índice correcto
                nextImageIndex++;

                // Actualizar el texto asociado con la imagen
                imageTextPairs[index].text.text = character.name;

                // Descargar la imagen
                yield return StartCoroutine(DownloadImage(character.image, index));
            }
            else
            {
                Debug.Log(request.responseCode + "|" + request.error);
            }
        }
    }

    IEnumerator DownloadImage(string url, int index)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(request.error);
        }
        else
        {
            Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            imageTextPairs[index].image.texture = texture;
        }
    }
}


    [System.Serializable]
public class UserData
{
    public int id;
    public string username;
    public int[] deck;
}

[System.Serializable]
public class CharactersList
{
    public charactersInfo info;
    public Character[] results;
}

[System.Serializable]
public class Character
{
    public int id;
    public string name;
    public string species;
    public string image;
}

[System.Serializable]
public class charactersInfo
{
    public int count;
    public int pages;
    public string prev;
    public string next;
}
