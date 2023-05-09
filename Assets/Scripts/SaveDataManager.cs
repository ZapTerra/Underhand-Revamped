using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SaveDataManager : MonoBehaviour
{
    public Toggle audioToggle;
    public ToggleSpriteSwap audioSpriteToggle;
    public Toggle voiceToggle;
    public ToggleSpriteSwap voiceSpriteToggle;
    public AudioSource music;
    public AudioSource fireplace;
    public AudioSource radio;
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
            SetMusic();
        }

        if(voiceToggle != null){
            voiceToggle.isOn = PlayerPrefs.GetInt("Voice Off") == 1;
            voiceSpriteToggle.OnTargetToggleValueChanged(voiceToggle.isOn);
            PlayerPrefs.SetInt("Voice Off", voiceToggle.isOn ? 1 : 0);
            SetRadio();
        }
    }

    public void ToggleAudio(){
        Debug.Log("Toggled Audio");
        PlayerPrefs.SetInt("Audio Off", PlayerPrefs.GetInt("Audio Off") == 0 ? 1 : 0);
        SetMusic();
    }

    public void SetMusic(){
        Debug.Log("Setting music on or off");
        bool stop = PlayerPrefs.GetInt("Audio Off") == 1 ? true : false;
        if(music != null){
            if(stop){
                music.Pause();
            }else{
                music.UnPause();
            }
        }
        if(fireplace != null){
            if(stop){
                fireplace.Pause();
            }else{
                fireplace.UnPause();
            }
        }
    }

    public void ToggleVoice(){
        PlayerPrefs.SetInt("Voice Off", PlayerPrefs.GetInt("Voice Off") == 0 ? 1 : 0);
        SetRadio();
    }

    public void SetRadio(){
        Debug.Log("Turning radio on or off");
        bool stop = PlayerPrefs.GetInt("Voice Off") == 1 ? true : false;
        if(radio != null){
            if(stop){
                radio.volume = 0;
            }else{
                radio.volume = 1;
            }
        }
    }

    public void ResetSaveData(){
        PlayerPrefs.SetInt("Rhybaax", 0);
        PlayerPrefs.SetInt("Wiindigoo", 0);
        PlayerPrefs.SetInt("Jhai'ti", 0);
        PlayerPrefs.SetInt("Kekujira", 0);
        PlayerPrefs.SetInt("Yacare", 0);
        PlayerPrefs.SetInt("Uhl'uht'c", 0);
        SceneManager.LoadScene("Menu");
    }

    public void ResetTutorial(){
        PlayerPrefs.SetInt("God of Beginnings", 0);
        PlayerPrefs.SetInt("desperatemeasures", 0);
        PlayerPrefs.SetInt("policeraid", 0);
        PlayerPrefs.SetInt("greed", 0);
        PlayerPrefs.SetInt("blessingsintroduction", 0);
        PlayerPrefs.SetInt("firststep", 0);
        PlayerPrefs.SetInt("resource", 0);
        PlayerPrefs.SetInt("initiation", 0);
        PlayerPrefs.SetInt("eventchain", 0);
        PlayerPrefs.SetInt("summongod", 0);
        PlayerPrefs.SetInt("foresight", 0);
        PlayerPrefs.SetInt("foresightdiscard", 0);
    }
}
