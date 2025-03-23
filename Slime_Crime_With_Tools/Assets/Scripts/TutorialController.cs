using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour
{
    [SerializeField]
    private GameObject controlsVideo;
    [SerializeField]
    private GameObject collectionVideo;
    [SerializeField]
    private GameObject trapsVideo;
    [SerializeField]
    private GameObject worldViewVideo;
    [SerializeField]
    private TextMeshProUGUI tutorialText;
    private int timesClicked = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ContinueButtonClicked()
    {
        switch (timesClicked)
        {
            case 0:
                controlsVideo.SetActive(false);
                collectionVideo.SetActive(true);
                tutorialText.text = "Collect as much money as you can in 90 seconds!\nMoney will not respawn if you stay too close.";
                break;
            case 1:
                collectionVideo.SetActive(false);
                trapsVideo.SetActive(true);
                tutorialText.text = "Avoid traps, you only have 3 lives!";
                break;
            case 2:
                trapsVideo.SetActive(false);
                worldViewVideo.SetActive(true);
                tutorialText.text = "~Score calculation~\nDiamonds: 500 points\n Gold: 200 points\nCash: 50 points";
                break;
            case 3:
                tutorialText.text = "Good luck becoming the richest slime in the world!";
                break;
            case 4:
                SceneManager.LoadScene(3);
                break;
            default:
                Debug.Log("ERROR: clicked " + timesClicked + " times?");
                break;
        }
        timesClicked++;
    }
}
