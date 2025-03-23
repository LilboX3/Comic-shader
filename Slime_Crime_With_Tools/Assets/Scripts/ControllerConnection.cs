using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ControllerConnection : MonoBehaviour
{
    private bool connected = false;
    private int countdownTime = 5;
    private bool firstConnection = false;
    public GameObject connectionText;
    public GameObject slime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    bool IsControllerConnected()
    {
        return Input.GetKey(KeyCode.JoystickButton0) ||
               Input.GetKey(KeyCode.JoystickButton1) ||
               Input.GetKey(KeyCode.JoystickButton2) ||
               Input.GetKey(KeyCode.JoystickButton3) ||
               Input.GetKey(KeyCode.JoystickButton4) ||
               Input.GetKey(KeyCode.JoystickButton5) ||
               Input.GetKey(KeyCode.JoystickButton6) ||
               Input.GetKey(KeyCode.JoystickButton7);
    }

    void Update()
    {
        if (connected && !firstConnection)
        {
            slime.SetActive(true);
            connectionText.SetActive(true);
            StartCoroutine(UICountdown());
            StartCoroutine(WaitAndLoadScene(5f));
            firstConnection = true;
        }
        else if (!connected)
        {
            connected = IsControllerConnected();
            firstConnection = false;
        }
    }

    IEnumerator WaitAndLoadScene(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene(2);
    }

    IEnumerator UICountdown()
    {
        int secondsLeft = countdownTime;
        while (secondsLeft > 0)
        {
            connectionText.GetComponent<TextMeshProUGUI>().text = "Starting in " + secondsLeft + "..";
            yield return new WaitForSeconds(1f);
            secondsLeft--;
        }
        connectionText.GetComponent<TextMeshProUGUI>().text = "Starting in " + 0 + "..";
    }

}
