using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidBody;
    AudioSource audioSource;

    [SerializeField]float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip succes;

    [SerializeField] ParticleSystem mainEngineParticle;
    [SerializeField] ParticleSystem deathParticle;
    [SerializeField] ParticleSystem succesParticle;

    enum State { Alive, Dying, Transcending};
    State state = State.Alive;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Alive) //todo sound sound on death
        {
            RespondToThustInput();
            RespondToRotateInput();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive) { return; }
        switch (collision.gameObject.tag)
        {
            case "Friendly":

                break;

            case "Finish":
                StartSuccesSequence();
                break;

            default:
                StartDeathSequnce();
                break;
        }
    }

    private void StartDeathSequnce()
    {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(death);
        deathParticle.Play();
        Invoke("LoadFirstLevel", 1f);
    }

    private void StartSuccesSequence()
    {
        state = State.Transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(succes);
        succesParticle.Play();
        Invoke("LoadNextLevel", 1f); ///parametirize param
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(1);
    }

    private void RespondToThustInput()
    {
        if (Input.GetKey(KeyCode.Space)) //poate sa accelereze in timp ce se roteste
        {
            ApplyThust();
        }
        else
        {
            audioSource.Stop();
            mainEngineParticle.Stop();
        }
    }

    private void ApplyThust()
    {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        if (!audioSource.isPlaying)
            audioSource.PlayOneShot(mainEngine);
        mainEngineParticle.Play();
    }

    private void RespondToRotateInput()
    {
        rigidBody.freezeRotation = true; //take manual control

        float rotationThisSpeed = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward*rotationThisSpeed);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisSpeed);
        }
        rigidBody.freezeRotation = false; //physics control
    }
}
