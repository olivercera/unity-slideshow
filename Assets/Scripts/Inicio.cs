using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;

using System;
using System.Net;
using System.IO;
using System.Text;

public class Inicio : MonoBehaviour
{
    public GameObject myGameObject;
    public static List<Slide> slides;
    private InputField content;
    private InputField keywords;

    private const string pexelsToken = "563492ad6f91700001000001769c9115db0149f5b6f679fd6d7cc89a";
    private const string speechKey = "AIzaSyAcmOzufTEogrusOS8F3Zt_5pJzfwf10_4";
    // Start is called before the first frame update
    void Start()
    {
        content = GameObject.Find("content").GetComponent<InputField>();
        keywords = GameObject.Find("keywords").GetComponent<InputField>();
        slides = new List<Slide>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartSlideshow()
    {
        CreateSlideData(content.text, keywords.text);
        Debug.Log(slides.Count);

        App.slides = slides;
        SceneManager.LoadScene("Slide");
    }

    private void CreateSlideData(string content, string keywords)
    {
        string[] contentList = content.Replace(System.Environment.NewLine, "").Trim().Split('.');
        string[] imageList = keywords.Trim().Split(',');

        slides = new List<Slide>();

        for (int i = 0; i < contentList.Length; i++)
        {
            Slide slide = new Slide();
            slide.content = contentList[i].Trim();
            slide.speech = getSpeech1(slide.content);
            //slide.speech = getSpeech2(slide.content);
            if (imageList.Length > i)
            {
                slide.imageUrl = getImage(imageList[i]);
            }

            slides.Add(slide);
        }

        App.slides = slides;
    }

    private string getImage(string keyword)
    {
        System.Random random = new System.Random();
        int randomNumber = random.Next(1, 1000);

        string url = "https://api.pexels.com/v1/search?query={0}&per_page=1&page=" + randomNumber;

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format(url, keyword));
        request.Headers.Add("Authorization", pexelsToken);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        Pexels info = JsonUtility.FromJson<Pexels>(jsonResponse);
        if (info.photos.Length > 0)
        {
            return info.photos[0].src.medium;
        }
        return null;
    }

    private string getSpeech1(string phrase)
    {

        string url = "https://texttospeech.googleapis.com/v1/text:synthesize?alt=json&key={0}";
        url = String.Format(url, speechKey);

        SpeechRequest data = new SpeechRequest();
        data.input.text = phrase;

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "POST";
        request.ContentType = "application/json";

        using (var streamWriter = new StreamWriter(request.GetRequestStream()))
        {
            string json = JsonUtility.ToJson(data);
            streamWriter.Write(json);
            streamWriter.Flush();
            streamWriter.Close();
        }

        try
        {
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string result;
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
                SpeechResponse info = JsonUtility.FromJson<SpeechResponse>(result);
                return info.audioContent;
            }
        }
        catch (WebException e)
        {
            string responseText;
            using (var reader = new StreamReader(e.Response.GetResponseStream()))
            {
                responseText = reader.ReadToEnd();
            }
            Debug.Log(responseText);
            return "";
        }

    }

}
