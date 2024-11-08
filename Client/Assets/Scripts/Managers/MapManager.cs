using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

public struct Pos
{
    public Pos(int y, int x) { Y = y; X = x; }
    public int Y;
    public int X;
}

public struct PQNode : IComparable<PQNode>
{
    public int F;
    public int G;

    public int Y;
    public int X;

    public int CompareTo(PQNode other)
    {
        if (F == other.F)
            return 0;

        return ((F < other.F) ? 1 : -1);
    }
}

public class MapManager
{
    public Grid CurrentGrid { get; private set; }

    bool[,] collision;

    public int MinX { get; set; }
    public int MaxX { get; set; }
    public int MinY { get; set; }
    public int MaxY { get; set; }

    public bool CanGo(Vector3Int cellPos)
    {
        if (cellPos.x < MinX || cellPos.x > MaxX)
            return false;

        if (cellPos.y < MinY || cellPos.y > MaxY)
            return false;

        int x = cellPos.x - MinX;
        int y = MaxY - cellPos.y;

        return !collision[y, x];
    }

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

        // Parse Map
        TextAsset mapText = Manager.Resource.Load<TextAsset>($"Map/{name}");
        StringReader reader = new StringReader(mapText.text);

        MinX = int.Parse(reader.ReadLine());
        MaxX = int.Parse(reader.ReadLine());
        MinY = int.Parse(reader.ReadLine());
        MaxY = int.Parse(reader.ReadLine());

        int xCount = MaxX - MinX + 1;
        int yCount = MaxY - MinY + 1;

        collision = new bool[yCount, xCount];
        for (int y = 0; y < yCount; y++)
        {
            string line = reader.ReadLine();
            for (int x = 0; x < xCount; x++)
            {
                collision[y, x] = line[x] == '1' ? true : false;
            }
        }
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
