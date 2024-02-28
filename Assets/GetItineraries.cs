using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine.UI;
public class GetItineraries : MonoBehaviour
{
    public GameObject maxPathPrefab; // Префаб объекта MaxPathPrefab
    public GameObject maxPathPrefabFarher;
    public GameObject maxPathPrefab2; // Префаб объекта MaxPathPrefab
    public GameObject maxPathPrefabFarher2;
    public GameObject midPathPrefab; // Префаб объекта MidPathPrefab
    public GameObject midPathPrefabFarher;
    public GameObject Caption;
    public GameObject ShotText;
    public GameObject textPrefabFarher;
    [System.Serializable]
    public class ImageData
    {
        public int id;
        public string image;
        public int order;
        public int itinerary;
    }

    [System.Serializable]
    public class Itinerary
    {
        public int id;
        public List<ImageData> images;
        public string name;
        public string name_eng;
        public string description;
        public string description_eng;
        public int spend_time;
        public List<int> showplaces;
    }

    void Start()
    {
        StartCoroutine(GetRequest("https://ivolga.endwork.today/api/itineraries/"));
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                Debug.Log("Response: " + webRequest.downloadHandler.text);

                // Parse JSON response
                ParseJSON(webRequest.downloadHandler.text);
            }
        }
    }

    void ParseJSON(string json)
    {
        // Deserialize JSON array into an array of JToken objects
        JArray jsonArray = JArray.Parse(json);

        // Iterate over each JSON object in the array
        foreach (JToken token in jsonArray)
        {
            // Deserialize the JSON object into an Itinerary object
            Itinerary itinerary = token.ToObject<Itinerary>();

            // Access the properties of the itinerary object
            Debug.Log("Itinerary ID: " + itinerary.id);
            Debug.Log("Itinerary Name: " + itinerary.name);
            Debug.Log("Itinerary Name (English): " + itinerary.name_eng);
            Debug.Log("Itinerary Description: " + itinerary.description);
            Debug.Log("Itinerary Description (English): " + itinerary.description_eng);
            Debug.Log("Itinerary Spend Time: " + itinerary.spend_time);

            // Access images data
            foreach (var image in itinerary.images)
            {
                Debug.Log("Image ID: " + image.id);
                Debug.Log("Image URL: " + image.image);
                Debug.Log("Image Order: " + image.order);
                Debug.Log("Image Itinerary ID: " + image.itinerary);
            }

            // Access showplaces data
            foreach (var showplace in itinerary.showplaces)
            {
                Debug.Log("Showplace ID: " + showplace);
            }
            GameObject maxPathObject = Instantiate(maxPathPrefab, Vector3.zero, Quaternion.identity);

            // Get the MaxPathComponent component of the created object
            MaxPathComponent maxPathComponent = maxPathObject.GetComponent<MaxPathComponent>();

            // Access the properties of the itinerary object and set them in MaxPathComponent
            maxPathComponent.pathNameText.text = itinerary.name; // Set Itinerary Name
            StartCoroutine(LoadSpriteFromUrl(itinerary.images[0].image, maxPathComponent.pathIco));
            maxPathObject.transform.parent=maxPathPrefabFarher.transform;
           /////////////////////////////////////////////////////////////////////////
             GameObject maxPathObject2 = Instantiate(maxPathPrefab2, Vector3.zero, Quaternion.identity);

            // Get the MaxPathComponent component of the created object
            MaxPathComponent maxPathComponent2 = maxPathObject2.GetComponent<MaxPathComponent>();

            // Access the properties of the itinerary object and set them in MaxPathComponent
            maxPathComponent2.pathNameText.text = itinerary.name; // Set Itinerary Name
            StartCoroutine(LoadSpriteFromUrl(itinerary.images[0].image, maxPathComponent.pathIco));
            maxPathObject2.transform.parent=maxPathPrefabFarher2.transform;
           


            //////////////////////////////////////////////////////
            GameObject midPathObject = Instantiate(midPathPrefab, Vector3.zero, Quaternion.identity);

            // Get the MidPathComponent component of the created object
            MidPathComponent midPathComponent = midPathObject.GetComponent<MidPathComponent>();
            midPathObject.transform.parent=midPathPrefabFarher.transform;
            // Access the properties of the itinerary object and set them in MidPathComponent
            midPathComponent.pathNameText.text = itinerary.name; // Set Itinerary Name

            // Load sprite from URL asynchronously
            StartCoroutine(LoadSpriteFromUrl(itinerary.images[0].image, midPathComponent.pathIco));
            ///////////////////////////////////////////////////////////////////////////////////////
            // Create a new Caption object
            GameObject captionObject = Instantiate(Caption, Vector3.zero, Quaternion.identity);

            // Get the TextMeshProUGUI component of the created object
            TextMeshProUGUI captionText = captionObject.GetComponent<TextMeshProUGUI>();
            captionObject.transform.parent=textPrefabFarher.transform;
            // Access the properties of the itinerary object and set them in Caption
            captionText.text=itinerary.name;// Set Itinerary Description

            GameObject TextObject = Instantiate(ShotText, Vector3.zero, Quaternion.identity);

            // Get the TextMeshProUGUI component of the created object
            TextMeshProUGUI ShotTextText = ShotText.GetComponent<TextMeshProUGUI>();
            TextObject.transform.parent=textPrefabFarher.transform;
            // Access the properties of the itinerary object and set them in Caption
             ShotTextText.text=itinerary.description;// Set Itinerary Description
  













        }
        // Create a new MaxPathPrefab object
            
    }


   IEnumerator LoadSpriteFromUrl(string url, Image image)
    {
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError ||
                www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error downloading image: " + www.error);
            }
            else
            {
                // Create texture from downloaded data
                Texture2D texture = DownloadHandlerTexture.GetContent(www);

                // Create sprite from texture
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);

                // Set sprite to the Image component
                image.sprite = sprite;
            }
        }
    }
}