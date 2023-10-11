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
    [SerializeField] AnimationCurve healthCurve;
    [SerializeField] AnimationCurve manaCurve;
    [SerializeField] AnimationCurve strCurve;
    [SerializeField] AnimationCurve intCurve;
    [SerializeField] AnimationCurve spdCurve;
    [SerializeField] public int currentLevel = 1;
    public int totalExperience;
    int previousLevelExperience, nextLevelExperience;
    [SerializeField] private PlayerRessource _playerRessource;
    


    [Header("Interface")]
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI experienceText;
    [SerializeField] Image experienceFill;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip levelUpSound;
    private void Awake()
    {

        totalExperience = (int) experienceCurve.Evaluate(currentLevel);
        audioSource = GetComponent<AudioSource>();
        UpdateLevel();
    }
    void Start()
    {
      
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
            _playerRessource = GetComponent<PlayerRessource>();
            _playerRessource.maxHealth = (int) healthCurve.Evaluate(currentLevel);
            _playerRessource.maxMana = (int)manaCurve.Evaluate(currentLevel);
            _playerRessource.strength.SetValue((int) strCurve.Evaluate(currentLevel));
            _playerRessource.intelligence.SetValue((int) intCurve.Evaluate(currentLevel));
            _playerRessource.speed.SetValue((int) spdCurve.Evaluate(currentLevel));

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
        if(totalExperience >= nextLevelExperience) { CheckForLevelUp(); }
    }
}
