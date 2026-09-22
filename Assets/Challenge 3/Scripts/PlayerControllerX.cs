using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerX : MonoBehaviour
{
    public bool gameOver = false;
    public bool isOnGround = true;

    public float floatForce = 5f;
    private float gravityModifier = 1.5f;
    private Rigidbody playerRb;

    public InputAction floatAction;

    public ParticleSystem explosionParticle;
    public ParticleSystem fireworksParticle;

    private AudioSource playerAudio;
    public AudioClip moneySound;
    public AudioClip explodeSound;


    // Start is called before the first frame update
    void Start()
    {   
        // NEW: Initialized playerRb to avoid NullReferenceException
        playerRb = GetComponent<Rigidbody>();
        playerAudio = GetComponent<AudioSource>();
        Physics.gravity *= gravityModifier;
        floatAction.Enable();

        // Apply a small upward force at the start of the game
        playerRb.AddForce(Vector3.up * floatForce, ForceMode.Impulse);

    }

    // Update is called once per frame
    void Update()
    {
        // While space is pressed and player is low enough, float up
        // OLD: if (floatAction.IsPressed() && !gameOver)
        // NEW: Added check to prevent going above the ceiling
        if (floatAction.IsPressed() && !gameOver && transform.position.y < 14.5f)
        {
            playerRb.AddForce(Vector3.up * floatForce);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
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

        }

        // NEW: if player collides with ground, bounce up
         if (other.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            playerRb.AddForce(Vector3.up * 40, ForceMode.Impulse);
            playerAudio.PlayOneShot(moneySound, 1.0f); // Optional: add a bounce sound if available
            Debug.Log("Is on the ground!");
        }


    }

}
