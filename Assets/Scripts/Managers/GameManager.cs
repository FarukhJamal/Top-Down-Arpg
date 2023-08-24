using System;
using System.Collections;
using System.Collections.Generic;
using Helpers;
using Player;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public PlayerController PlayerController;
    public GameObject playerCamera;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        var player=Instantiate(PlayerController,Vector3.zero, Quaternion.identity);
        var camera = Instantiate(playerCamera);
        if (camera != null)
        {
            SmoothCameraFollow cameraFollow = camera.GetComponent<SmoothCameraFollow>();
            if (cameraFollow != null)
            {
                player.IsometricCamera = cameraFollow.MainCamera;
                cameraFollow.SetCameraTarget(player.transform);
            }
        }
    }
}
