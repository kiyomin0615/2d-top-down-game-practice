using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapCollision : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase tile;

    void Start()
    {
        tilemap.SetTile(new Vector3Int(0, 0, 0), tile);
    }

    void Update()
    {
        List<Vector3Int> blocked = new List<Vector3Int>();

        // cellBounds는 현재 Tilemap에 타일이 배치된 모든 셀들의 범위
        // 범위(Bounds)란, 타일이 배치된 셀들이 포함된 최소한의 사각형(또는 직육면체) 공간을 의미
        // allPositionsWithin는 cellBounds의 범위 내에 있는 모든 셀 위치들을 나열
        foreach (Vector3Int pos in tilemap.cellBounds.allPositionsWithin)
        {
            TileBase tile = tilemap.GetTile(pos);
            if (tile != null)
                blocked.Add(pos);
        }
    }
}
