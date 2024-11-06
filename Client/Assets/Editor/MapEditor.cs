using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;



#if UNITY_EDITOR
using UnityEditor;
#endif

public class MapEditor
{
#if UNITY_EDITOR

    // %: Ctrl, #: Shift, &: Alt
    [MenuItem("Tools/Generate Map %#m")]
    static void GenerateMap()
    {
        GameObject[] maps = Resources.LoadAll<GameObject>("Prefabs/Map");
        foreach (GameObject map in maps)
        {
            Tilemap tilemap = Utility.FindChild<Tilemap>(map, "Tilemap_Collision", true);

            using (var writer = File.CreateText($"Assets/Resources/Map/{map.name}.txt"))
            {
                writer.WriteLine(tilemap.cellBounds.xMin);
                writer.WriteLine(tilemap.cellBounds.xMax);
                writer.WriteLine(tilemap.cellBounds.yMin);
                writer.WriteLine(tilemap.cellBounds.yMax);

                for (int y = tilemap.cellBounds.yMax; y >= tilemap.cellBounds.yMin; y--)
                {
                    for (int x = tilemap.cellBounds.xMin; x <= tilemap.cellBounds.xMax; x++)
                    {
                        TileBase tile = tilemap.GetTile(new Vector3Int(x, y, 0));
                        if (tile != null)
                            writer.Write("1");
                        else
                            writer.Write("0");
                    }
                    writer.WriteLine();
                }
            }
        }
    }

#endif
}
