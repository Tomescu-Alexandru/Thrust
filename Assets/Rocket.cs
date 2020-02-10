using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidBody;
    AudioSource audioSource;

    [SerializeField]float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Thrust();
        Rotate();
    }

    void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
        
                break;

            case "Finish":
                print("Hit Finish");
                if (SceneManager.GetActiveScene() == SceneManager.GetSceneAt(0))
                SceneManager.LoadScene(1);
                else SceneManager.LoadScene(0);

                break;

            default:
                print("Dead");
                SceneManager.LoadScene(0);
                break;
        }
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space)) //poate sa accelereze in timp ce se roteste
        {
            rigidBody.AddRelativeForce(Vector3.up* mainThrust*Time.deltaTime);
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
        else
            audioSource.Stop();
    }

    private void Rotate()
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
