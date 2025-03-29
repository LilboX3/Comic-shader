using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlimeController : MonoBehaviour
{
    [SerializeField]
    private float minJumpForce = 2f;
    [SerializeField]
    private float maxJumpForce = 6f;
    [SerializeField]
    private float chargeSpeed = 4f;
    [SerializeField]
    private int health = 3;

    [SerializeField]
    private LineRenderer directionRenderer;
    [SerializeField]
    private float laserWidth = 0.1f;
    [SerializeField]
    private float laserStartLength = 0.5f;
    private float currentLaserLength;
    [SerializeField]
    private float directionModifierSpeed = 1.5f;
    [SerializeField]
    private float maxDirectionModifier = 3f;
    [SerializeField]
    private List<Transform> respawnPoints = new List<Transform>();
    [SerializeField]
    private GameObject gameManager;

    [SerializeField]
    private List<AudioClip> jumpSounds = new List<AudioClip>();
    [SerializeField]
    private AudioClip damagedSound;
    [SerializeField]
    private bool isInTutorial = false;
    private AudioSource audioSource;

    private int score = 0;

    private float currentJumpForce;
    private float currentDirectionModifier;
    private Rigidbody rb;
    private Animator animator;
    private Transform slimeObject;
    private Vector3 jumpDirection;
    private GameManager gameManagerScript;
    private bool isGrounded;
    private bool canTakeDamage = true;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        slimeObject = gameObject.transform.GetChild(0);
        currentJumpForce = minJumpForce;
        directionRenderer.enabled = false;
        directionRenderer.startWidth = laserWidth;
        currentDirectionModifier = 1;
        
        audioSource = gameObject.GetComponent<AudioSource>();
        if (!isInTutorial)
        {
            gameManagerScript = gameManager.GetComponent<GameManager>();
            transform.position = respawnPoints[Random.Range(0, respawnPoints.Count)].position;
        }
    }
        

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Jump") && isGrounded)
        {
            ChargeJump();
        }
        if (Input.GetButtonUp("Jump") && isGrounded)
        {
            ReleaseJump();
            audioSource.PlayOneShot(jumpSounds[Random.Range(0, jumpSounds.Count)]);
        }
    }

    private void ChargeJump()
    {
        currentJumpForce += chargeSpeed * Time.deltaTime;
        currentLaserLength += directionModifierSpeed * Time.deltaTime;
        currentDirectionModifier += directionModifierSpeed * Time.deltaTime;

        currentJumpForce = Mathf.Clamp(currentJumpForce, minJumpForce, maxJumpForce); //Wert begrenzen zwischen min und maxForce
        currentLaserLength = Mathf.Clamp(currentLaserLength, laserStartLength, maxDirectionModifier);
        currentDirectionModifier = Mathf.Clamp(currentDirectionModifier, 1, maxDirectionModifier);

        float horizontalInput = Input.GetAxis("Joystick horizontal"); //todo: WHAT THE HELL
        float verticalInput = -Input.GetAxis("Joystick vertical");
        jumpDirection = new Vector3(-horizontalInput, 0, -verticalInput).normalized; 

        Vector3 lineDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;
        directionRenderer.SetPosition(1, lineDirection * (currentLaserLength / 2)); //tweak to match distance jumped?
        directionRenderer.enabled = true;
    }

    private void ReleaseJump()
    {
        rb.AddForce(Vector3.up * currentJumpForce, ForceMode.Impulse);
        rb.AddForce(jumpDirection * currentDirectionModifier, ForceMode.Impulse);

        if (jumpDirection != Vector3.zero) //Rotate slime in jumpdirection
        {
            Quaternion toRotation = Quaternion.LookRotation(jumpDirection, Vector3.up);
            transform.GetChild(0).rotation = toRotation;
        }

        directionRenderer.enabled = false;
        isGrounded = false;
        //reset fórce
        currentJumpForce = minJumpForce;
        currentLaserLength = laserStartLength;
        currentDirectionModifier = 1;
    }

    public void AddPoints(int amount)
    {
        score += amount;
        gameManagerScript.UpdateScore(score);
    }

    public void TakeDamage(int damage)
    {
        if (canTakeDamage)
        {
            audioSource.PlayOneShot(damagedSound);
            gameManagerScript.DisplayHealth(health);
            health -= damage;
            if (health <= 0)
            {
                Die();
            }
        }
    }

    public void Die()
    {
        gameManagerScript.EndGame();
    }
      
    IEnumerator InvincibilityFrame(float seconds)
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(seconds);
        canTakeDamage = true;

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 6) //check if ground was hit
        {
            isGrounded = true;
        }
        if (collision.gameObject.CompareTag("Trap"))
        {
            TakeDamage(1);
            StartCoroutine(InvincibilityFrame(2.0f));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ocean"))
        {
            TakeDamage(1);
            if(!(health <= 0))
            {
                transform.position = respawnPoints[Random.Range(0, respawnPoints.Count)].position;
            }
        }
    }

}
