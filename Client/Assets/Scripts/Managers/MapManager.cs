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

        return F < other.F ? 1 : -1;
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

    public int SizeX { get { return MaxX - MinX + 1; } }
    public int SizeY { get { return MaxY - MinY + 1; } }

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

    #region A* Algorithm
    // Direction : Up, Down, Left, Right
    int[] deltaY = new int[] { 1, -1, 0, 0 };
    int[] deltaX = new int[] { 0, 0, -1, 1 };
    int[] cost = new int[] { 10, 10, 10, 10 };

    public List<Vector3Int> FindPath(Vector3Int startCellPos, Vector3Int destCellPos, bool ignoreDestCellCollision = false)
    {
        // A* Algorithm
        // F = G + H
        // G : Cost from Start to Target
        // H : (Expected) Cost from Target to Destination
        bool[,] visited = new bool[SizeY, SizeX];

        // Found : F = G + H
        // Not Found : Max Value
        int[,] costs = new int[SizeY, SizeX];
        for (int y = 0; y < SizeY; y++)
        {
            for (int x = 0; x < SizeX; x++)
            {
                costs[y, x] = Int32.MaxValue;
            }
        }

        Pos[,] parent = new Pos[SizeY, SizeX];

        PriorityQueue<PQNode> pq = new PriorityQueue<PQNode>();

        Pos start = CellPosToPos(startCellPos);
        Pos dest = CellPosToPos(destCellPos);

        // 시작 좌표 : 발견 및 예약
        costs[start.Y, start.X] = 10 * (Math.Abs(dest.Y - start.Y) + Math.Abs(dest.X - start.X));
        pq.Push(new PQNode() { F = 10 * (Math.Abs(dest.Y - start.Y) + Math.Abs(dest.X - start.X)), G = 0, Y = start.Y, X = start.X });
        parent[start.Y, start.X] = new Pos(start.Y, start.X);

        while (pq.Count > 0)
        {
            PQNode node = pq.Pop();

            // 해당 좌표를 이미 방문했다면 스킵
            if (visited[node.Y, node.X])
                continue;

            // 방문
            visited[node.Y, node.X] = true;
            // 방문한 좌표가 도착 좌표라면 길찾기 종료
            if (node.Y == dest.Y && node.X == dest.X)
                break;

            // 방문한 좌표의 위, 아래, 양 옆 좌표를 확인
            for (int i = 0; i < deltaY.Length; i++)
            {
                Pos next = new Pos(node.Y + deltaY[i], node.X + deltaX[i]);

                if (next.Y != dest.Y || next.X != dest.X || !ignoreDestCellCollision)
                {
                    if (!CanGo(PosToCellPos(next)))
                        continue;
                }

                if (visited[next.Y, next.X])
                    continue;

                int g = 0;
                int h = 10 * ((dest.Y - next.Y) * (dest.Y - next.Y) + (dest.X - next.X) * (dest.X - next.X));
                if (costs[next.Y, next.X] < g + h)
                    continue;

                // 다음 좌표 예약
                costs[next.Y, next.X] = g + h;
                pq.Push(new PQNode() { F = g + h, G = g, Y = next.Y, X = next.X });
                parent[next.Y, next.X] = new Pos(node.Y, node.X);
            }
        }

        return CalcPathFromParent(parent, dest);
    }

    List<Vector3Int> CalcPathFromParent(Pos[,] parent, Pos dest)
    {
        List<Vector3Int> cells = new List<Vector3Int>();

        int y = dest.Y;
        int x = dest.X;

        while (parent[y, x].Y != y || parent[y, x].X != x)
        {
            cells.Add(PosToCellPos(new Pos(y, x)));
            Pos next = parent[y, x];
            y = next.Y;
            x = next.X;
        }

        cells.Add(PosToCellPos(new Pos(y, x)));
        cells.Reverse();

        return cells;
    }

    Pos CellPosToPos(Vector3Int cellPos)
    {
        return new Pos(MaxY - cellPos.y, cellPos.x - MinX);
    }

    Vector3Int PosToCellPos(Pos pos)
    {
        return new Vector3Int(pos.X + MinX, MaxY - pos.Y, 0);
    }
    #endregion
}
