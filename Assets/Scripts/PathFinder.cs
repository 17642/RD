using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PathFinder
{
    public class Node
    {
        public Vector2Int Position { get; set; }
        public int GCost { get; set; }
        public int HCost { get; set; }
        public int Fcost => GCost + HCost;
        public Node Parent { get; set; }

        public Node(Vector2Int position)
        {
            Position = position;
        }

        public override bool Equals(object obj)
        {
            return obj is Node node && Position == node.Position;
        }

        public static bool operator ==(Node left, Node right)
        {
            if (ReferenceEquals(left, null) && ReferenceEquals(right, null))
            {
                return true;
            }

            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
            {
                return false;
            }

            return left.Equals(right);
        }

        public static bool operator !=(Node left, Node right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return Position.GetHashCode();
        }
    }

    private static readonly Vector2Int[] Directions = new Vector2Int[]
    {
        new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(0, -1), new Vector2Int(-1, 0),
        new Vector2Int(1, 1), new Vector2Int(1, -1), new Vector2Int(-1, 1), new Vector2Int(-1, -1)
    };

    public static Node FindNextStep(int[,] map, Node startNode, Node targetNode)
    {
        List<Node> openList = new List<Node>();
        HashSet<Node> closedList = new HashSet<Node>();

        openList.Add(startNode);

        while (openList.Count > 0)
        {
            Node CurrentNode = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].Fcost < CurrentNode.Fcost || (openList[i].Fcost == CurrentNode.Fcost && openList[i].HCost < CurrentNode.HCost))
                {
                    CurrentNode = openList[i];
                }
            }

            openList.Remove(CurrentNode);
            closedList.Add(CurrentNode);

            if (CurrentNode == targetNode)
            {
                Node NextStep = RetraceFirstStep(startNode, CurrentNode);
                return NextStep;
            }

            foreach (var direction in Directions)
            {
                Vector2Int newPosition = CurrentNode.Position + direction;

                if (!IsInBounds(map, newPosition) || map[newPosition.x, newPosition.y] == 1) continue;

                Node neighbor = new Node(newPosition);
                if (closedList.Contains(neighbor)) continue;

                int newGCost = CurrentNode.GCost + 1;
                if (newGCost < neighbor.GCost || !openList.Contains(neighbor))
                {
                    neighbor.GCost = newGCost;
                    neighbor.HCost = GetDistance(neighbor, targetNode);
                    neighbor.Parent = CurrentNode;

                    if (!openList.Contains(neighbor))
                    {
                        openList.Add(neighbor);
                    }
                }
            }
        }

        return null;
    }

    static int GetDistance(Node node1, Node node2)
    {
        Vector2Int delta = node1.Position - node2.Position;
        int diagonalSteps = Mathf.Min(Mathf.Abs(delta.x), Mathf.Abs(delta.y));
        int straightSteps = Mathf.Abs(delta.x - delta.y);
        return diagonalSteps + straightSteps;
    }

    static bool IsInBounds(int[,] map, Vector2Int position)
    {
        return position.x >= 0 && position.x < map.GetLength(0) && position.y >= 0 && position.y < map.GetLength(1);
    }

    static bool CanMoveDiagonally(int[,] map, Vector2Int currentPosition, Vector2Int newPosition)
    {
        Vector2Int delta = newPosition - currentPosition;

        if (map[currentPosition.x + delta.x, currentPosition.y] == 1 || map[currentPosition.x, currentPosition.y + delta.y] == 1)
            return false;

        if (map[newPosition.x, newPosition.y] == 1)
            return false;

        return true;
    }

    static Node RetraceFirstStep(Node startNode, Node endNode)
    {
        Node currentNode = endNode;
        while (currentNode.Parent != startNode)
        {
            currentNode = currentNode.Parent;
        }

        return currentNode;
    }
}