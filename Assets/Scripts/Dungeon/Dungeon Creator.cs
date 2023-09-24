using System;
using System.Collections;
using System.Collections.Generic;
using Dungeon;
using UnityEngine;

public class DungeonCreator : MonoBehaviour
{
    #region Variables

    public static DungeonCreator Instance;
    public int dungeonWidth,dungeonLength;
    public int minRoomWidth, maxRoomWidth;
    public int maxIterations;
    public int corridorWidth;

    #endregion

    #region Unity-Functions

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
        CreateDungeon();
    }

    #endregion

    #region Private-Functions

    private void CreateDungeon()
    {
        DungeonGenerator dungeonGenerator = new DungeonGenerator(dungeonWidth,dungeonLength);
        var listOfRooms = dungeonGenerator.CalculateRooms(maxIterations, minRoomWidth, maxRoomWidth);
    }

#endregion
}
