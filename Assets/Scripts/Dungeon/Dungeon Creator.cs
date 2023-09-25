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
        [Range(0.0f,0.3f)]
        public float roomBottomCornerModifier;
        [Range(0.7f,1.0f)]
        public float roomTopCornerModifier;
        [Range(0,2)]
        public int roomOffset;
    
        public Material material;

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
            var listOfRooms = dungeonGenerator.CalculateRooms(maxIterations, minRoomWidth, maxRoomWidth,roomBottomCornerModifier,roomTopCornerModifier,roomOffset,corridorWidth);
            for (int i = 0; i < listOfRooms.Count; i++)
            {
                CreateMesh(listOfRooms[i].BottomLeftAreaCorner,listOfRooms[i].TopRightAreaCorner);
            }
        }

        private void CreateMesh(Vector2 bottomLeftCorner, Vector2 topRightCorner)
        {
            Vector3 bottomLeftV = new Vector3(bottomLeftCorner.x, 0, bottomLeftCorner.y);
            Vector3 bottomRightV = new Vector3(topRightCorner.x, 0, bottomLeftCorner.y);
            Vector3 topLeftV = new Vector3(bottomLeftCorner.x, 0, topRightCorner.y);
            Vector3 topRightV = new Vector3(topRightCorner.x, 0, topRightCorner.y);

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

            GameObject dungeonFloor = new GameObject("Mesh"+bottomLeftCorner, typeof(MeshFilter), typeof(MeshRenderer));
        
            dungeonFloor.transform.position=Vector3.zero;
            dungeonFloor.transform.localScale=Vector3.one;
            dungeonFloor.GetComponent<MeshFilter>().mesh = mesh;
            dungeonFloor.GetComponent<MeshRenderer>().material = material;
        }

        #endregion
    }
}
