using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ShootController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform spawnTransform;
    public XRGrabInteractable grabInteractable;
    public float firepower = 10;

    private void Start()
    {
        grabInteractable.activated.AddListener(OnTrigger);
    }

    void OnTrigger(ActivateEventArgs args)
    {
        var bullet = Instantiate(bulletPrefab, spawnTransform.position, spawnTransform.rotation);
        bullet.GetComponent<Rigidbody>().AddForce(transform.forward * firepower);
    }
}
