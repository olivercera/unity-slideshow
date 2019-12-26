using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Splash : MonoBehaviour
{
    private string content;
    private string keywords;
    float secondsLeft = 0;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DelayLoadLevel(3));
    }

    IEnumerator DelayLoadLevel(float seconds)
    {
        secondsLeft = seconds;
        do
        {
            yield return new WaitForSeconds(1);
        } while (--secondsLeft > 0);

        SceneManager.LoadScene("Inicio");
    }
}
