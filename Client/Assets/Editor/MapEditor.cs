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
            Tilemap tilemapBase = Utility.FindChild<Tilemap>(map, "Tilemap_Base", true);
            Tilemap tilemapCollision = Utility.FindChild<Tilemap>(map, "Tilemap_Collision", true);

            using (var writer = File.CreateText($"Assets/Resources/Map/{map.name}.txt"))
            {
                writer.WriteLine(tilemapBase.cellBounds.xMin);
                writer.WriteLine(tilemapBase.cellBounds.xMax - 1);
                writer.WriteLine(tilemapBase.cellBounds.yMin);
                writer.WriteLine(tilemapBase.cellBounds.yMax - 1);

                for (int y = tilemapBase.cellBounds.yMax - 1; y >= tilemapBase.cellBounds.yMin; y--)
                {
                    for (int x = tilemapBase.cellBounds.xMin; x <= tilemapBase.cellBounds.xMax - 1; x++)
                    {
                        TileBase tile = tilemapCollision.GetTile(new Vector3Int(x, y, 0));
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
