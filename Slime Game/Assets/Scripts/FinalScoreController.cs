using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FinalScoreController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private TextMeshProUGUI levelText;
    [SerializeField]
    private Sprite[] scoreImages;
    [SerializeField]
    private Image finalImage;
    [SerializeField]
    private AudioClip[] soundEffects;

    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        scoreText.text = "Your final score is ";
        int finalScore = GameManager.endScore;
        StartCoroutine(ShowFinalScoreWithDelay(finalScore));
    }

    IEnumerator ShowFinalScoreWithDelay(int finalScore)
    {
        AddDotToText();
        yield return new WaitForSeconds(1.5f);
        AddDotToText();
        yield return new WaitForSeconds(1.5f);
        AddDotToText();
        yield return new WaitForSeconds(1.5f);
        scoreText.text += finalScore +" !";
        switch (finalScore)
        {
            case >= 10000:
                finalImage.sprite = scoreImages[5];
                audioSource.PlayOneShot(soundEffects[2]);
                levelText.text = "Level 10000 Mafia Boss";
                break;
            case >= 5000:
                finalImage.sprite = scoreImages[4];
                audioSource.PlayOneShot(soundEffects[2]);
                levelText.text = "Level 500 Millionaire";
                break;
            case >= 3500:
                finalImage.sprite = scoreImages[3];
                audioSource.PlayOneShot(soundEffects[1]);
                levelText.text = "Level 100 Gangster";
                break;
            case >= 2500:
                finalImage.sprite = scoreImages[2];
                audioSource.PlayOneShot(soundEffects[1]);
                levelText.text = "Level 10 Goon";
                break;
            case >= 1500:
                finalImage.sprite = scoreImages[1];
                audioSource.PlayOneShot(soundEffects[1]);
                levelText.text = "Level 5 Rookie Thief";
                break;
            default:
                finalImage.sprite = scoreImages[0];
                audioSource.PlayOneShot(soundEffects[0]);
                levelText.text = "Level 1 Peasant";
                break;
        }
        finalImage.color = Color.white;
    }

    private void AddDotToText()
    {
        scoreText.text += ". ";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
