public class Slide
{
    public string content;
    public string imageUrl;
    public string speech;

}
[System.Serializable]
public class SpeechRequest
{
    public AudioConfig audioConfig = new AudioConfig();
    public SpeechInput input = new SpeechInput();

    public SpeechVoice voice = new SpeechVoice();
}

[System.Serializable]
public class AudioConfig
{
    public string audioEncoding = "LINEAR16";
}

[System.Serializable]
public class SpeechInput
{
    public string text = "";
}

[System.Serializable]
public class SpeechVoice
{
    public string ssmlGender = "FEMALE";
    public string languageCode = "es";
}

[System.Serializable]
public class SpeechResponse{
    public string audioContent;
}