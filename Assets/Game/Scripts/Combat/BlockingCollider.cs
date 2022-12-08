using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

public class BlockingCollider : NetworkBehaviour
{
    BoxCollider blockingCollider;

    public bool IsBlocking => blockingCollider.enabled;

    private void Awake()
    {
        blockingCollider = GetComponent<BoxCollider>();
    }

    [Server]
    public void EnableCollider()
    {
        blockingCollider.enabled = true;
    }

    [Server]
    public void DisableCollider()
    {
        blockingCollider.enabled = false;
    }
}
