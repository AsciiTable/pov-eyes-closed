using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISFXHandler : MonoBehaviour
{
    private AudioSource audioSource;
    private static UISFXHandler instance = null;
    public static UISFXHandler Instance { get { return instance; } }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else {
            instance = this;
            audioSource = gameObject.GetComponent<AudioSource>();
        }
        DontDestroyOnLoad(gameObject);
    }

    public void PlayButtonClick() {
        audioSource.Play();
    }
}
