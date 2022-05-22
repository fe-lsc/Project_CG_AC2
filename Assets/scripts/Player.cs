using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float forceMultiplier;
    [SerializeField]
    private ParticleSystem deathParticles;

    public GameObject mainVCam;
    public GameObject zoomVCam;

    private Rigidbody rb;
    private CinemachineImpulseSource cinemachineImpulseSource;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
    }


    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance == null){
            return;
        }
        var horizontalInput = Input.GetAxis("Horizontal");
        var verticalInput = Input.GetAxis("Vertical");

        if(GetComponent<Rigidbody>().velocity.magnitude <= 8f && horizontalInput <= 8f)
        {
            GetComponent<Rigidbody>().AddForce(new Vector3(horizontalInput * forceMultiplier * Time.deltaTime, 0, 0));
        }
        if(GetComponent<Rigidbody>().velocity.magnitude <= 8f && verticalInput <= 8f)
        {
            GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, verticalInput * forceMultiplier * Time.deltaTime));
        }
    }
    
    private void OnEnable(){
        this.transform.rotation = Quaternion.identity;
        this.transform.position = new Vector3(0,0.75f,0);
        rb.velocity = Vector3.zero;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Hazard"))
        {
            GameOver();

            Instantiate(deathParticles, transform.position, Quaternion.identity);
            cinemachineImpulseSource.GenerateImpulse();
        }
    }

    private void OnTriggerExit(Collider other){
        if(other.CompareTag("FallDown")){
            GameOver();
        }
    }

    private void GameOver(){
            GameManager.Instance.GameOver();
            gameObject.SetActive(false);
    }
}
