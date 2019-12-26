using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.IO;
using System;
public class Present : MonoBehaviour
{
    public static List<Slide> slides;

    private int currentSlide;
    // Start is called before the first frame update

    private Text slideText;
    private Image slideImage;
    private AudioSource slideAudio;

    private AudioClip speechClip;

    void Start()
    {

        slideText = GameObject.Find("slideText").GetComponent<Text>();
        slideImage = GameObject.Find("slideImage").GetComponent<Image>();
        slideAudio = GameObject.Find("slideSpeech").GetComponent<AudioSource>();

        currentSlide = -1;
        Debug.Log(App.slides.Count);
        if (App.slides.Count > 0)
        {
            NextSlide();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            NextSlide();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PrevSlide();
        }
    }

    void NextSlide()
    {
        if (App.slides.Count > currentSlide + 1)
        {
            currentSlide++;
            updateSlide();
        }
    }

    void PrevSlide()
    {
        if (currentSlide > 0)
        {
            currentSlide--;
            updateSlide();
        } else {
            SceneManager.LoadScene("Inicio");
        }
    }

    void updateSlide()
    {
        Debug.Log("actualizar slide: " + currentSlide);

        playAudio();
        slideText.text = App.slides[currentSlide].content;
        // slideImage.image = App.slides[currentSlide].imageUrl;
        slideImage.sprite = null;
        StartCoroutine(LoadImageCoroutine(App.slides[currentSlide].imageUrl));
    }

    private IEnumerator LoadImageCoroutine(string url)
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url))
        {
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError || uwr.isHttpError)
            {
                Debug.Log(uwr.error);
            }
            else
            {
                // Get downloaded asset bundle
                // var texture = DownloadHandlerTexture.GetContent(uwr);
                Texture2D webTexture = ((DownloadHandlerTexture)uwr.downloadHandler).texture as Texture2D;
                Sprite webSprite = SpriteFromTexture2D(webTexture);
                slideImage.sprite = webSprite;
            }
        }
    }

    Sprite SpriteFromTexture2D(Texture2D texture)
    {

        return Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
    }

    void playAudio()
    {
        File.WriteAllBytes(Application.dataPath + "/../speech.wav", Convert.FromBase64String(App.slides[currentSlide].speech));

        StartCoroutine(GetAudioClip());

    }
    IEnumerator GetAudioClip()
    {
        Debug.Log("getadioclip");
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip( Application.dataPath.Replace("/Assets","")  + "/speech.wav", AudioType.WAV))
        {
            ((DownloadHandlerAudioClip)www.downloadHandler).streamAudio = true;
            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {
                AudioClip speechClip = DownloadHandlerAudioClip.GetContent(www);
                slideAudio.clip = speechClip;
                slideAudio.Play();
            }
        }
    }


}
