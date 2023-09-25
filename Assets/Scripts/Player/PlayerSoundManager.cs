using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundManager : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Weapon")]
    public AudioClip[] swingSounds; //not used

    [Header("Foodsteps")]
    public List<AudioClip> grassFS;
    public List<AudioClip> rockFS;


    private AudioSource footStepSource;
    enum FSMaterial
    {
        Grass,Rock, Empty
    }
    void Start()
    {
        footStepSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AnimationEvent_FootStep()
    {
        AudioClip clip;

        clip = rockFS[Random.Range(0,rockFS.Count)];

        footStepSource.clip = clip;
        footStepSource.volume = Random.Range(0.02f, 0.05f);
        footStepSource.pitch = Random.Range(0.8f, 1.2f);
        footStepSource.Play();

    }
}
