using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public Transform tilePrefab;
    public Transform obstaclePrefab;
    public Vector2 mapSize;

    List<Coord> allTileCood;
    Queue<Coord> shuffleTileCoord;
    Coord mapCenter;

    [Range (0, 1)]
    public float outlinePercent;
    [Range (0, 1)]
    public float obstaclePercent;
    public int Seed = 10; 

    private void Start () {
        GenerateMap ();
    }

    public void GenerateMap() {

        allTileCood = new List<Coord> ();
        for(int x = 0; x < mapSize.x; x++) {
            for(int y = 0; y < mapSize.y; y++) {
                allTileCood.Add (new Coord (x, y));
            }
        }

        shuffleTileCoord = new Queue<Coord> (Utility.ShuffletileArray (allTileCood.ToArray (), Seed));

        // Create Gameobject
        string holderName = "Generate Map";
        if(transform.Find(holderName)) {
            DestroyImmediate (transform.Find (holderName).gameObject);
        }
        Transform mapHolder = new GameObject (holderName).transform;
        mapHolder.parent = transform;


        // NEW TILE INSTANTIATE
        for(int x = 0; x < mapSize.x; x++) {
            for(int y = 0; y < mapSize.y; y++) {

                Vector3 tilePosition = CoordToPosition (x, y);
                Transform newTile = Instantiate (tilePrefab, tilePosition, Quaternion.identity);
                newTile.localScale = Vector3.one * ( 1 - outlinePercent );
                newTile.parent = mapHolder;
            }
        }

        // OBSTACLE INSTANTIATE
        bool[,] obstacleMap = new bool [(int) mapSize.x, (int) mapSize.y];
        mapCenter = new Coord ((int)mapSize.x / 2, (int)mapSize.y / 2);
        int currentObsCount = 0;

        int obstacleCount = (int) ( mapSize.x * mapSize.y * obstaclePercent );
        for(int i = 0; i < obstacleCount; i++) {
            Coord randomCoord = GetRandomCoord ();

            currentObsCount++;
            obstacleMap[randomCoord.x, randomCoord.y] = true;

            if(randomCoord != mapCenter && mapIsFullyAccessible (obstacleMap, currentObsCount)) {
                Vector3 obstaclePosition = CoordToPosition (randomCoord.x, randomCoord.y);
                Transform newObs = Instantiate (obstaclePrefab, obstaclePosition, Quaternion.identity);
                newObs.parent = mapHolder;
            }
            else {
                currentObsCount--;
                obstacleMap[randomCoord.x, randomCoord.y] = false;
            }
        }
        
    }

    // Method responceble for diffrent kind of map obstacle  generation -// 
    bool mapIsFullyAccessible(bool[,] obstaclemap, int currentObsCount) {
        bool[,] mapFlags = new bool [obstaclemap.GetLength (0), obstaclemap.GetLength (1)];
        Queue<Coord> queue = new Queue<Coord> ();
        queue.Enqueue (mapCenter);
        mapFlags[mapCenter.x, mapCenter.y] = true;
        int accessibleTileCount = 1;

        while(queue.Count > 0) {
            Coord tile = queue.Dequeue ();
            for(int x = -1; x <= 1; x++) {
                for(int y = -1; y <= 1; y++) {
                    int neabX = tile.x + x;
                    int neabY = tile.y + y;
                    if(x == 0 || y == 0) {
                        if(neabX >= 0 && neabX < obstaclemap.GetLength(0) && neabY >=0 && neabY < obstaclemap.GetLength(1)) {
                            if(!mapFlags[neabX, neabY] && !obstaclemap[neabX,neabY]) {
                                mapFlags[neabX, neabY] = true;
                                queue.Enqueue (new Coord (neabX, neabY));
                                accessibleTileCount++;
                            }
                        }
                    }
                }
            }
        }
        int targetAccCount = (int) (mapSize.x * mapSize.y - currentObsCount);
        return targetAccCount == accessibleTileCount;
    }

    Vector3 CoordToPosition(int x, int y ) {
        return new Vector3 (-mapSize.x / 2 + 0.5f + x,-mapSize.y / 2 + 0.5f + y,0);
    }

    public Coord GetRandomCoord() {
        Coord randomCoord = shuffleTileCoord.Dequeue ();
        shuffleTileCoord.Enqueue (randomCoord);
        return randomCoord;
    }

    public struct Coord {

        public int x;
        public int y;

        public Coord(int _x, int _y) {
            x = _x;
            y = _y;
        }

        public static bool operator ==(Coord c1, Coord c2) {
            return c1.x == c2.x && c1.y == c2.y;
        }
        public static bool operator != (Coord c1, Coord c2) {
            return !(c1 == c2);
        }



    }
}
