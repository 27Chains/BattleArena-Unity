using FishNet;
using UnityEngine;
using UnityEngine.UI;

public class NetworkHUD : MonoBehaviour
{
    [SerializeField]
    Button hostButton;

    [SerializeField]
    Button serverButton;

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

        serverButton
            .onClick
            .AddListener(() =>
            {
                InstanceFinder.ServerManager.StartConnection();
            });

        connectButton
            .onClick
            .AddListener(() =>
            {
                if (InstanceFinder.ClientManager.Started)
                {
                    InstanceFinder.ClientManager.StopConnection();
                }
                else
                {
                    InstanceFinder.ClientManager.StartConnection();
                }
            });
    }
}
