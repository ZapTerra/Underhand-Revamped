using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : MonoBehaviour
{
    public static List<AudioClip> soundBytes = new List<AudioClip>();
    public float minDelay = 30f;
    public float maxDelay = 90f;
    public AudioSource radioPlayer;
    // Start is called before the first frame update
    void Start()
    {
        soundBytes.Clear();
        StartCoroutine(RadioRoutine());
    }
    public IEnumerator RadioRoutine(){
        while(true){
            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
            if(soundBytes.Count > 0){
                var clip = soundBytes[Random.Range(0, soundBytes.Count)];
                radioPlayer.clip = clip;
                radioPlayer.Play();
                soundBytes.Remove(clip);
            }
            if(maxDelay <= 0){
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
