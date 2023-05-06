using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveDataManager : MonoBehaviour
{
    public Toggle audioToggle;
    public ToggleSpriteSwap audioSpriteToggle;
    public Toggle voiceToggle;
    public ToggleSpriteSwap voiceSpriteToggle;
    void Start(){
        PlayerPrefs.SetInt("God of Beginnings", 1);
        //When the toggles are "Off" the player sees "On" because their default state is on.
        AudioListener.pause = PlayerPrefs.GetInt("Audio Off") == 1;
        Debug.Log(PlayerPrefs.GetInt("Audio Off"));
        Debug.Log(PlayerPrefs.GetInt("Voice Off"));

        if(audioToggle != null){
            audioToggle.isOn = PlayerPrefs.GetInt("Audio Off") == 1;
            audioSpriteToggle.OnTargetToggleValueChanged(audioToggle.isOn);
            PlayerPrefs.SetInt("Audio Off", audioToggle.isOn ? 1 : 0);
        }

        if(voiceToggle != null){
            voiceToggle.isOn = PlayerPrefs.GetInt("Voice Off") == 1;
            voiceSpriteToggle.OnTargetToggleValueChanged(voiceToggle.isOn);
            PlayerPrefs.SetInt("Voice Off", voiceToggle.isOn ? 1 : 0);
        }
    }

    public void ToggleAudio(){
        Debug.Log("Toggled Audio");
        PlayerPrefs.SetInt("Audio Off", PlayerPrefs.GetInt("Audio Off") == 0 ? 1 : 0);
    }

    public void ToggleVoice(){
        PlayerPrefs.SetInt("Voice Off", PlayerPrefs.GetInt("Voice Off") == 0 ? 1 : 0);
    }
}
