using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    public Grid CurrentGrid { get; private set; }

    public void LoadMap(int id)
    {
        DestroyMap();

        string name = "Map_" + id.ToString("000");
        GameObject map = Manager.Resource.Instantiate($"Map/{name}");
        map.name = "Map";

        Tilemap tilemap = Utility.FindChild<Tilemap>(map, "Tilemap_Collision", true);
        if (tilemap != null)
            tilemap.gameObject.SetActive(false);

        CurrentGrid = map.GetComponent<Grid>();
    }

    public void DestroyMap()
    {
        GameObject map = GameObject.Find("Map");
        if (map != null)
        {
            GameObject.Destroy(map);
            CurrentGrid = null;
        }

    }
}
