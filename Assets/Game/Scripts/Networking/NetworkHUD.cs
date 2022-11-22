using System.Collections;
using System.Collections.Generic;
using FishNet;
using UnityEngine;
using UnityEngine.UI;

public class NetworkHUD : MonoBehaviour
{
    [SerializeField]
    Button hostButton;

    [SerializeField]
    Button connectButton;

    void Start()
    {
        hostButton
            .onClick
            .AddListener(() =>
            {
                InstanceFinder.ServerManager.StartConnection();
                InstanceFinder.ClientManager.StartConnection();
            });

        connectButton
            .onClick
            .AddListener(() =>
            {
                InstanceFinder.ClientManager.StartConnection();
            });
    }
}
