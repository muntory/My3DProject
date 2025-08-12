using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPlatform : MonoBehaviour
{
    public float jumpForce = 10f;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Debug.Log("onTrigger");

        if (other.TryGetComponent<PlayerController>(out PlayerController playerController))
        {
            playerController.JumpByOther(jumpForce);
        }
    }
}
