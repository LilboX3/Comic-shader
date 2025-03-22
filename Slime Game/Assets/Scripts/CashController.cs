using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashController : MonoBehaviour
{
    [SerializeField]
    private int scoreAmount;
    [SerializeField]
    private AudioClip collectionSound;

    private AudioSource audioSource;
    private ParticleSystem particles;
    private SlimeController slimeController;

    // Start is called before the first frame update
    void Start()
    {
        particles = GetComponentInChildren<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetPoints()
    {
        return scoreAmount;
    }

    void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            slimeController = other.gameObject.GetComponentInParent<SlimeController>();
            slimeController.AddPoints(scoreAmount);
            particles.Play();
            audioSource.PlayOneShot(collectionSound);
            StartCoroutine(DestroyAfterSeconds(0.5f));
        }
    }

    IEnumerator DestroyAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
}
