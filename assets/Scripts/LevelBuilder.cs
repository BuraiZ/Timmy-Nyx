using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelBuilder : MonoBehaviour {
    public Grid grid;
    public Sprite[] sprites;
    public GameObject[] tilePrefabs;
    private Dictionary<Sprite, GameObject> dict;

    private Tilemap timmyTilemap;
    private Tilemap nyxTilemap;

    // Use this for initialization
    void Start () {
        timmyTilemap = grid.transform.Find("TimmyTilemap").GetComponentInChildren<Tilemap>();
        nyxTilemap = grid.transform.Find("NyxTilemap").GetComponentInChildren<Tilemap>();
        dict = new Dictionary<Sprite, GameObject>();
        for (int i = 0; i < sprites.Length; i++) {
            dict.Add(sprites[i], tilePrefabs[i]);
        }

        BuildLevel(timmyTilemap, 0);
        BuildLevel(nyxTilemap, -1);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void BuildLevel(Tilemap tilemap, int depth) {
        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

        for (int y = bounds.position.y; y < (bounds.size.y + bounds.position.y); y++) {
            for (int x = bounds.position.x; x < (bounds.size.x + bounds.position.x); x++) {
                TileBase tile = allTiles[(x - bounds.position.x) + (y - bounds.position.y) * bounds.size.x];
                if (tile != null) {
                    Sprite sprite = tilemap.GetSprite(new Vector3Int(x, y, 0));
                    GameObject objToInstanciate = dict[sprite];
                    Instantiate(objToInstanciate, new Vector3(depth, y, x), Quaternion.identity);
                }
            }
        }
    }
}
