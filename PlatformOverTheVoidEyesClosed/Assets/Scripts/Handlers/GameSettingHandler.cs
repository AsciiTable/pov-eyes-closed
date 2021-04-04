using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameSettingHandler : MonoBehaviour
{
    private GameSettings settings;
    public Slider sfxSlide;
    public Slider bgmSlide;
    public TextMeshProUGUI sfxDisplayValue;
    public TextMeshProUGUI bgmDisplayValue;

    static GameSettingHandler singleton = null;
    public static float SFXVolume { get => singleton.settings.sfxVolumeScale; }
    public static float BGMVolume { get => singleton.settings.bgmVolumeScale; }

    private void OnEnable()
    {
        if (singleton == null)
            singleton = this;
    }
    private void OnDisable()
    {
        singleton = null;
    }
    private void Start()
    {
        settings = new GameSettings();
        settings = SaveSystem.LoadGameSettings();
        if (settings == null)
        {
            settings = new GameSettings();
            sfxSlide.value = 100f;
            bgmSlide.value = 100f;
            sfxDisplayValue.text = "100%";
            bgmDisplayValue.text = "100%";
            SaveSystem.SaveGameSettings(settings);
        }
        else {
            sfxSlide.value = settings.sfxVolumeScale * 100;
            bgmSlide.value = settings.bgmVolumeScale * 100;
            sfxDisplayValue.text = sfxSlide.value.ToString("F2") + "%";
            bgmDisplayValue.text = bgmSlide.value.ToString("F2") + "%";
        }
    }
    public void AdjustSFX(System.Single value) {
        sfxSlide.value = value;
        settings.sfxVolumeScale = value / 100;
        sfxDisplayValue.text = value.ToString("F2") + "%";
        SaveSystem.SaveGameSettings(settings);
    }

    public void AdjustBGM(System.Single value) {
        bgmSlide.value = value;
        settings.bgmVolumeScale = value / 100;
        bgmDisplayValue.text = value.ToString("F2") + "%";
        SaveSystem.SaveGameSettings(settings);
    }
}
