using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Hazard : MonoBehaviour
{
    Vector3 rotation;

    [SerializeField]
    private ParticleSystem breakingEffect;

    private CinemachineImpulseSource cinemachineImpulseSource;
    private Player player;
    private Coroutine destroyCoroutine;

    private void Start()
    {
        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
        player = FindObjectOfType<Player>();

        var xRotation = Random.Range(90f, 180f);
        rotation = new Vector3(-xRotation, 0);
        
    }

    private IEnumerator DelayDestroy()
    {
        rotation = Vector3.zero;
        yield return new WaitForSeconds(1f);
        
        Destroy(gameObject);
        Instantiate(breakingEffect, transform.position, Quaternion.identity);
        StopCoroutine(destroyCoroutine);

        yield return null;
    }

    private void Update()
    {
        transform.Rotate(rotation * Time.deltaTime);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(!collision.gameObject.CompareTag("Hazard"))
        {
            destroyCoroutine = StartCoroutine(DelayDestroy());
            
            if(player != null)
            {
                var distance = Vector3.Distance(transform.position, player.transform.position);
                var force = 1f / distance;

                cinemachineImpulseSource.GenerateImpulse();
            }
        }
    }

}
