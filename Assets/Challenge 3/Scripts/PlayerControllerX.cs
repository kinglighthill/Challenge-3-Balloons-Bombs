using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public bool gameOver;

    private float floatForce = 10;
    private float bounceForce = 5;
    private float gravityModifier = 1.5f;
    private Rigidbody playerRb;

    public ParticleSystem explosionParticle;
    public ParticleSystem fireworksParticle;

    private AudioSource playerAudio;
    public AudioClip moneySound;
    public AudioClip explodeSound;
    public AudioClip bounceSound;

    private float upperBound = 15.0f;

    private bool isOnGround = false;
    private bool isLowEnough = true;


    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();

        Physics.gravity *= gravityModifier;
        playerAudio = GetComponent<AudioSource>();

        // Apply a small upward force at the start of the game
        playerRb.AddForce(Vector3.up * 5);

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y > upperBound)
        {
            transform.position = new Vector3(transform.position.x, upperBound, transform.position.z);
            isLowEnough = false;
        }

        if (transform.position.y < upperBound)
        {
            isLowEnough = true;
        }

        // While space is pressed and player is low enough, float up
        if (Input.GetKey(KeyCode.Space) && isLowEnough  && !gameOver)
        {
            playerRb.AddForce(Vector3.up * floatForce);
            isOnGround = false;
        }
    }

    private void OnCollisionEnter(Collision other)
    {

        Debug.Log("Wait");
        // if player collides with bomb, explode and set gameOver to true
        if (other.gameObject.CompareTag("Bomb"))
        {
            explosionParticle.Play();
            playerAudio.PlayOneShot(explodeSound, 1.0f);
            gameOver = true;
            Debug.Log("Game Over!");
            Destroy(other.gameObject);
        } 

        // if player collides with money, fireworks
        else if (other.gameObject.CompareTag("Money"))
        {
            fireworksParticle.Play();
            playerAudio.PlayOneShot(moneySound, 1.0f);
            Destroy(other.gameObject);
            Debug.Log("Hi");
        }

        else if(other.gameObject.CompareTag("Ground") && !gameOver)
        {
            isOnGround = true;
            playerAudio.PlayOneShot(bounceSound, 5.0f);
            playerRb.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);
        }

    }

}
