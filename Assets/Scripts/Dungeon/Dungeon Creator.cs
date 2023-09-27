using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeon
{
    public class DungeonCreator : MonoBehaviour
    {
        #region Variables

        public static DungeonCreator Instance;
        public int dungeonWidth,dungeonLength;
        public int minRoomWidth, maxRoomWidth;
        public int maxIterations;
        public int corridorWidth;
      //  [Range(0.0f,0.3f)]
        public float roomBottomCornerModifier=0.1f;
      //  [Range(0.7f,1.0f)]
        public float roomTopCornerModifier=0.9f;
        //[Range(0,2)]
        public int roomOffset=1;
        public Material material;
        public GameObject wallVertical, wallHorizontal;
        private List<Vector3Int> possibleDoorVerticalPosition;
        private List<Vector3Int> possibleDoorHorizontalPosition;
        private List<Vector3Int> possibleWallVerticalPosition;
        private List<Vector3Int> possibleWallHorizontalPosition;
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

        public void CreateDungeon()
        {
            DestroyAllChildren();
            DungeonGenerator dungeonGenerator = new DungeonGenerator(dungeonWidth,dungeonLength);
            var listOfRooms = dungeonGenerator.CalculateDungeon(maxIterations, minRoomWidth, maxRoomWidth,roomBottomCornerModifier,
                roomTopCornerModifier,roomOffset,corridorWidth); 
            
            GameObject wallParent = new GameObject("WallParent");
            wallParent.transform.parent = transform;
            
            possibleDoorVerticalPosition=new List<Vector3Int>();
            possibleDoorHorizontalPosition=new List<Vector3Int>();
            possibleWallVerticalPosition=new List<Vector3Int>();
            possibleWallHorizontalPosition=new List<Vector3Int>();

            for (int i = 0; i < listOfRooms.Count; i++)
            {
                CreateMesh(listOfRooms[i].BottomLeftAreaCorner,listOfRooms[i].TopRightAreaCorner);
            }

            CreateWalls(wallParent);
        }

        private void CreateWalls(GameObject wallParent)
        {
            //int i = 0;
            foreach (var wallPosition in possibleWallHorizontalPosition)
            {
               // Debug.Log(possibleWallHorizontalPosition[i]+" "+i);
                CreateWall(wallParent,wallPosition,wallHorizontal);
                //i++;
            }
            foreach (var wallPosition in possibleWallVerticalPosition)
            {
                CreateWall(wallParent, wallPosition, wallVertical);
            }
        }

        private void CreateWall(GameObject wallParent, Vector3Int wallPosition, GameObject wallPrefab)
        {
           var obj= Instantiate(wallPrefab, wallPosition, wallPrefab.transform.rotation, wallParent.transform);
           obj.name = wallPrefab.name + wallPosition;
        }

        private void CreateMesh(Vector2 bottomLeftCorner, Vector2 topRightCorner)
        {
            Vector3 bottomLeftV = new Vector3(bottomLeftCorner.x, 0, bottomLeftCorner.y);
            Vector3 bottomRightV = new Vector3(topRightCorner.x, 0, bottomLeftCorner.y);
            Vector3 topLeftV = new Vector3(bottomLeftCorner.x, 0, topRightCorner.y);
            Vector3 topRightV = new Vector3(topRightCorner.x, 0, topRightCorner.y);

            Debug.Log("bottom Left Vertix: " + bottomLeftV);
            Debug.Log("bottom right Vertix: " + bottomRightV);
            Debug.Log("top Left Vertix: " + topLeftV);
            Debug.Log("top right Vertix: " + topRightV);
            Vector3[] vertices = new[]
            {
                topLeftV,
                topRightV,
                bottomLeftV,
                bottomRightV
            };

            Vector2[] uvs = new Vector2[vertices.Length];
            for (int i = 0; i < uvs.Length; i++)
            {
                uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
            }

            int[] triangles = new[]
            {
                0,
                1,
                2,
                2,
                1,
                3
            };

            Mesh mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.uv = uvs;
            mesh.triangles = triangles;

            GameObject dungeonFloor = new GameObject("Mesh "+(topLeftV+topRightV)+" Upper "+(bottomLeftV+bottomRightV)+" Lower ", typeof(MeshFilter), typeof(MeshRenderer));

            dungeonFloor.transform.parent = transform;
            dungeonFloor.transform.position=Vector3.zero;
            dungeonFloor.transform.localScale=Vector3.one;
            dungeonFloor.GetComponent<MeshFilter>().mesh = mesh;
            dungeonFloor.GetComponent<MeshRenderer>().material = material;
            
            // walls
            for (int row = (int)bottomLeftV.x; row < (int)bottomRightV.x; row++)
            {
                var wallPosition = new Vector3(row, 0, bottomLeftV.z);
                AddWallPositionToList(wallPosition, possibleWallHorizontalPosition, possibleDoorHorizontalPosition);
            }
            /*for (int row = (int)topLeftV.x; row < (int)topRightCorner.x; row++)
            {
                var wallPosition = new Vector3(row, 0, topRightV.z);
                AddWallPositionToList(wallPosition, possibleWallHorizontalPosition, possibleDoorHorizontalPosition);
            }
            for (int col = (int)bottomLeftV.z; col < (int)topLeftV.z; col++)
            {
                var wallPosition = new Vector3(bottomLeftV.x, 0, col);
                AddWallPositionToList(wallPosition, possibleWallVerticalPosition, possibleDoorVerticalPosition);
            }
            for (int col = (int)bottomRightV.z; col < (int)topRightV.z; col++)
            {
                var wallPosition = new Vector3(bottomRightV.x, 0, col);
                AddWallPositionToList(wallPosition, possibleWallVerticalPosition, possibleDoorVerticalPosition);
            }*/
        }

        private void AddWallPositionToList(Vector3 wallPosition, List<Vector3Int> wallList, List<Vector3Int> doorList)
        {
            Vector3Int point = Vector3Int.CeilToInt(wallPosition);
            if (wallList.Contains(point))
            {
                doorList.Add(point);
                wallList.Remove(point);
            }
            else
            {
               // Debug.Log("Point : "+point);
                wallList.Add(point);
            }
        }

        private void DestroyAllChildren()
        {
            while (transform.childCount != 0)
            {
                foreach (Transform item in transform)
                {
                    DestroyImmediate(item.gameObject);
                }
            }
        }
        #endregion
    }
}
