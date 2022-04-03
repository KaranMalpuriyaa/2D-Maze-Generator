using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAP : MonoBehaviour
{
    public Transform tile;
    public Vector3 mapSize;

    private void Start () {
        GenerateMap ();
    }
    void GenerateMap() {
        for(int x = 0; x < mapSize.x; x++) {
            for(int y = 0; y < mapSize.y; y++) {
                Vector3 tilePos = new Vector3 (-mapSize.x + 0.5f + x, -mapSize.y +0.5f + y, 0 );
                Transform newTile = Instantiate (tile, tilePos, Quaternion.identity);
            }
        }
    }
   
}
