using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static int endScore = 0;
        
    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private GameObject pauseIcon;
    [SerializeField]
    private GameObject playIcon;
    [SerializeField]
    private TextMeshProUGUI timerText;
    [SerializeField]
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private List<Image> hearts;
    [SerializeField]
    private AudioSource audioSource;

    private float secondsLeft;
    private int totalScore;
    // Start is called before the first frame update
    void Start()
    {
        secondsLeft = 90;
        StartCoroutine(CountdownTime());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            OpenPauseMenu();
        }
    }

    IEnumerator CountdownTime()
    {
        while (secondsLeft >= 0)
        {
            timerText.text = "Time: " + secondsLeft;
            yield return new WaitForSeconds(1f);
            secondsLeft--;
        }
        EndGame();
    }

    public void ClosePauseMenu()
    {
        pauseMenu.SetActive(false);
        pauseIcon.SetActive(true);
        playIcon.SetActive(false);
        Time.timeScale = 1;
        audioSource.Play();
    }

    public void OpenPauseMenu()
    {
        pauseMenu.SetActive(true);
        pauseIcon.SetActive(false);
        playIcon.SetActive(true);
        Time.timeScale = 0;
        audioSource.Pause();
    }
    public void UpdateScore(int score)
    {
        scoreText.text = "Score: " + score;
        totalScore = score;
    }

    public void DisplayHealth(int heartsLeft)
    {
        hearts[heartsLeft-1].color = Color.black;
    }
    public void EndGame()
    {
        endScore = totalScore;
        Debug.Log("You had a total of: " + endScore);
        SceneManager.LoadScene(4); //Go to end score screen
    }
}
