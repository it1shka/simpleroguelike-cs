using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DungeonGenerator : MonoBehaviour
{
    public GameObject player;
    public GameObject levelEnd;
    private Vector2Int lastPlayerPos;
    private Health health;
    public RawImage mapImage;

    public enum CellType {Space, Wall};
    public Cell[,] Map;

    public GameObject space, wall;
    private List<Vector2Int> roomCenters;
    public Vector2 cellSize = new Vector2(.1f, .1f);
    public int mapWidth = 100, mapHeight = 100;
    public int roomsAmount = 5;
    public int minRoomWidth = 3, minRoomHeight = 3;
    public int maxRoomWidth = 7, maxRoomHeight = 7;

    [Range(0, 1)] public float enemyChance = .05f;
    [Range(0, 1)] public float boostChance = .01f;

    public GameObject[] enemies;
    public GameObject[] boosts;
    public float enemyChanceIncrease = .01f;
    private int dungeonNumber;
    private void Awake()
    {
        health = player.GetComponent<Health>();
        dungeonNumber = 0;
        GenerateMap();
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
            //GenerateMap();
        UpdateMapTexture(mapImage.texture as Texture2D);
    }

    public void GenerateMap(){

        dungeonNumber++;
        PlayerPrefs.SetInt("record", Mathf.Max(PlayerPrefs.GetInt("record"), dungeonNumber));
        PlayerPrefs.Save();

        ClearMap();
        Map = new Cell[mapWidth, mapHeight];
        for(int i=0; i<mapWidth; i++)
        {
            for(int j=0; j<mapHeight; j++)
            {
                Map[i, j] = new Cell();
                Map[i, j].cellType = CellType.Wall;
            }
        }

        GenerateRooms();
        GenerateTunnels();
        Visualize();

        if(mapImage)
        mapImage.texture = GetMapTexture();
        enemyChance += enemyChanceIncrease;
        health.safeTime = 2f;
    }

    void GenerateRooms(){
        roomCenters = new List<Vector2Int>();
        for (int i = 0; i < roomsAmount; i++) {
            int currentWidth = Random.Range(minRoomWidth, maxRoomWidth + 1);
            int currentHeight = Random.Range(minRoomHeight, maxRoomHeight + 1);
            int positionX = Random.Range(1, mapWidth - currentWidth - 1);
            int positionY = Random.Range(1, mapHeight - currentHeight - 1);
            var center = new Vector2Int(positionX + currentWidth / 2, positionY + currentHeight / 2);
            roomCenters.Add(center);

            //fill room

            for (int w = positionX; w <= positionX + currentWidth; w++)
            {
                for(int h = positionY; h <= positionY + currentHeight; h++)
                {
                    Map[w, h].cellType = CellType.Space;
                }
            }
        }

    }

    void GenerateTunnels(){
        for(int i=0; i<roomCenters.Count; i++)
        {
            for(int j=i+1; j<roomCenters.Count; j++)
            {
                MakeTunnel(roomCenters[i], roomCenters[j]);
            }
        }
    }

    void MakeTunnel(Vector2Int center1, Vector2Int center2)
    {
        Queue<Vector2Int> q = new Queue<Vector2Int>();
        q.Enqueue(center1);
        bool[,] used = new bool[mapWidth, mapHeight];
        Vector2Int[,] previousCells = new Vector2Int[mapWidth, mapHeight];

        while(q.Count > 0)
        {
            var cur = q.Dequeue();
            if (used[cur.x, cur.y]) continue;
            used[cur.x, cur.y] = true;
            //step
            if(cur.x + 1 < mapWidth && !used[cur.x + 1, cur.y]) {
                var step = new Vector2Int(cur.x + 1, cur.y);
                q.Enqueue(step);
                previousCells[step.x, step.y] = cur;
                if (step == center2) break;
            }
            if (cur.x - 1 >= 0 && !used[cur.x - 1, cur.y])
            {
                var step = new Vector2Int(cur.x - 1, cur.y);
                q.Enqueue(step);
                previousCells[step.x, step.y] = cur;
                if (step == center2) break;
            }
            if (cur.y + 1 < mapHeight && !used[cur.x, cur.y + 1])
            {
                var step = new Vector2Int(cur.x, cur.y + 1);
                q.Enqueue(step);
                previousCells[step.x, step.y] = cur;
                if (step == center2) break;
            }
            if (cur.y - 1 >= 0 && !used[cur.x, cur.y - 1])
            {
                var step = new Vector2Int(cur.x, cur.y - 1);
                q.Enqueue(step);
                previousCells[step.x, step.y] = cur;
                if (step == center2) break;
            }
        }

        var curPos = center2;
        while(curPos != center1)
        {
            Map[curPos.x, curPos.y].cellType = CellType.Space;
            curPos = previousCells[curPos.x, curPos.y];
        }
    }

    void Visualize()
    {

        player.transform.position = new Vector2(transform.position.x + cellSize.x * roomCenters[0].x, transform.position.y + cellSize.y * roomCenters[0].y);
        Instantiate(levelEnd, new Vector2(transform.position.x + cellSize.x * roomCenters[roomCenters.Count - 1].x, 
            transform.position.y + cellSize.y * roomCenters[roomCenters.Count - 1].y), Quaternion.identity);
        lastPlayerPos = roomCenters[0];
        for(int i=0; i<mapWidth; i++)
        {
            for(int j=0; j<mapHeight; j++)
            {
                Map[i, j].cellGameObject = Instantiate( ( Map[i, j].cellType == CellType.Space ) ? space : wall, 
                    new Vector2(transform.position.x + cellSize.x * i, transform.position.y + cellSize.y * j), 
                    Quaternion.identity);

                if ((i == roomCenters[0].x && j == roomCenters[0].y) ||
                    (i == roomCenters[roomCenters.Count - 1].x && j == roomCenters[roomCenters.Count - 1].y) ||
                    Map[i, j].cellType == CellType.Wall)
                    continue;

                var rnd = Random.value;
                GameObject additional = null;
                if(rnd <= enemyChance)
                {
                    //spawn enemy
                    if (enemies.Length != 0)
                    additional = Instantiate(enemies[Random.Range(0, Mathf.Min(dungeonNumber, enemies.Length))],
                    new Vector2(transform.position.x + cellSize.x * i, transform.position.y + cellSize.y * j),
                    Quaternion.identity);
                }
                else if(rnd <= enemyChance + boostChance)
                {
                    //spawn boost
                    if(boosts.Length != 0)
                    additional = Instantiate(boosts[Random.Range(0, boosts.Length)],
                    new Vector2(transform.position.x + cellSize.x * i, transform.position.y + cellSize.y * j),
                    Quaternion.identity);
                }

                Map[i, j].cellAdditionalGameObject = additional;
            }
        }
    }

    void ClearMap()
    {
        if (Map == null) return;
        for (int i = 0; i < mapWidth; i++)
        {
            for (int j = 0; j < mapHeight; j++)
            {
                Destroy(Map[i, j].cellGameObject);
                if (Map[i, j].cellAdditionalGameObject)
                    Destroy(Map[i, j].cellAdditionalGameObject);
            }
        }
    }

    public Texture2D GetMapTexture()
    {
        var texture = new Texture2D(mapWidth, mapHeight, TextureFormat.ARGB32, false);

        for(int i=0; i<mapWidth; i++)
        {
            for(int j=0; j<mapHeight; j++)
            {
                var info = Map[i, j].cellType;
                texture.SetPixel(i, j, info == CellType.Space ? Color.grey : Color.black);
            }
        }
        texture.Apply();
        return texture;
    }

    public void UpdateMapTexture(Texture2D texture)
    {
        if (!player) return;
        texture.SetPixel(lastPlayerPos.x, lastPlayerPos.y, 
            Map[lastPlayerPos.x, lastPlayerPos.y].cellType == CellType.Space ? Color.grey : Color.black);
        Vector2Int curPos = new Vector2Int(
            (int)((player.transform.position.x - transform.position.x) / cellSize.x),
            (int)((player.transform.position.y - transform.position.y) / cellSize.y)
        );
        texture.SetPixel(curPos.x, curPos.y, Color.green);
        lastPlayerPos = curPos;

        texture.SetPixel(roomCenters[roomCenters.Count-1].x, roomCenters[roomCenters.Count - 1].y, Color.white);
        texture.Apply();
    }
}
public class Cell {
    public DungeonGenerator.CellType cellType;
    public GameObject cellGameObject;
    public GameObject cellAdditionalGameObject;
}
