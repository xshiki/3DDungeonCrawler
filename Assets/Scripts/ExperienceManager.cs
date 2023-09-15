using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceManager : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Experience")]
    [SerializeField] AnimationCurve experienceCurve;

    int currentLevel = 1, totalExperience;
    int previousLevelExperience, nextLevelExperience;

    [Header("Interface")]
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI experienceText;
    [SerializeField] Image experienceFill;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip levelUpSound;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Start()
    {
        UpdateLevel();
    }

    public void AddExperience(int amount)
    {
        totalExperience += amount;
        CheckForLevelUp();
        UpdateInterface();
    }

    void CheckForLevelUp()
    {
        if(totalExperience >= nextLevelExperience) { currentLevel++; UpdateLevel();
            //Start level up sequence, VFX or sound
            audioSource.PlayOneShot(levelUpSound);

        }
    }


    void UpdateLevel()
    {
        previousLevelExperience= (int) experienceCurve.Evaluate(currentLevel);
        nextLevelExperience = (int) experienceCurve.Evaluate (currentLevel+1);
        UpdateInterface();
    }

    void UpdateInterface()
    {
        int start = totalExperience - previousLevelExperience;
        int end = nextLevelExperience - previousLevelExperience;

        levelText.text = currentLevel.ToString();
        experienceText.text = start + " exp / " + end + " exp ";
        experienceFill.fillAmount = (float)start / (float)end;
    }
}
