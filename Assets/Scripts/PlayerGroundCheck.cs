using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Este script se usa para detectar si el jugador esta sobre el piso.
*/

public class PlayerGroundCheck : MonoBehaviour
{
    PlayerController playerController;

    void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
    }


    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == playerController.gameObject) return;
        playerController.setGroundedState(true);
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject == playerController.gameObject) return;
        playerController.setGroundedState(false);
    }

    void OnTriggerStay(Collider other)
    {
        if(other.gameObject == playerController.gameObject) return;
        playerController.setGroundedState(true);
    }

    void OnCollisionEnter(Collision collision)
    {
      if(collision.gameObject == playerController.gameObject) return;
        playerController.setGroundedState(true);  
    }

    void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject == playerController.gameObject) return;
        playerController.setGroundedState(false);
    }

    void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject == playerController.gameObject) return;
        playerController.setGroundedState(true);
    }




}
