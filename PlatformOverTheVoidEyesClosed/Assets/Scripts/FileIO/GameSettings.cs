using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class GameSettings
{
    public float sfxVolumeScale;
    public float bgmVolumeScale;

    public GameSettings() {
        sfxVolumeScale = 1f;
        bgmVolumeScale = 1f;
    }

    public GameSettings(float sfx, float bgm) {
        sfxVolumeScale = sfx;
        bgmVolumeScale = bgm;
    }
}
