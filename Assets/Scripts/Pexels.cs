using System;

[System.Serializable]
public class PexelsResponse
{
    public Pexels Pexels;
}
[System.Serializable]
public class Pexels
{
    public long total_results;
    public long page;
    public long per_page;
    public Photo[] photos;
    public string next_page;
    public string prev_page;
}

[System.Serializable]
public class Photo
{
    public long id;
    public long width;
    public long height;
    public string url;
    public string photographer;
    public string photographer_url;
    public long photographer_id;
    public Src src;
    public bool liked;
}

[Serializable]
public class Src
{
    public string original;
    public string large2x;
    public string large;
    public string medium;
    public string small;
    public string portrait;
    public string landscape;
    public string tiny;
}
