using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSight : MonoBehaviour
{
    [SerializeField] private float rayLength = 8f;
    [SerializeField] private LayerMask rayMask;
    private RaycastHit hit;
    private Transform playerCam;

    private void Start()
    {
        playerCam = PlayerController.Instance().playerCam;
    }

    private void Update()
    {
        CheckIfMonsterInSight();
    }

    private void CheckIfMonsterInSight()
    {
        Ray ray = new Ray(playerCam.position, playerCam.forward);
        if (Physics.Raycast(ray, out hit, rayLength, rayMask)) 
        {
            if(hit.transform.CompareTag("Enemy"))
            {
                Monster.Instance().ScareMonster();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if(playerCam)
        {
            Gizmos.DrawLine(playerCam.position, playerCam.position + playerCam.forward * rayLength);
        }
    }
}
