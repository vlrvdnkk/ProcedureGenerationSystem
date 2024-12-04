using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



    public enum CouplingType
    {
        FourTiles,
        EightTiles,
        HexagonalTiles
    }

    public abstract class CouplingData
    {
        public abstract CouplingType Type { get; }

        public abstract int GetDirectionCount();
        public abstract int GetOppositeDirection(int direction);

        public abstract bool[] GetCouplingArray();

        public abstract Vector2Int GetCouplingOffset(int direction, Vector2Int pos, int startY);

        public abstract bool GetCoupling(int indexA, int indexB, int direction);
        public abstract void SetCoupling(int indexA, int indexB, int direction, bool value);

        public int GetLCVHeuristic(Vector2Int pos, int startY, SparseSet[,] domains, BoundsInt bounds, int index)
        {
            int size = 0;

            for (int d = 0; d < GetDirectionCount(); d++)
            {
                Vector2Int adjacentPosition = pos + GetCouplingOffset(d, pos, startY);

                if (adjacentPosition.x >= 0 && adjacentPosition.y >= 0 && adjacentPosition.x < bounds.size.x && adjacentPosition.y < bounds.size.y)
                {
                    SparseSet domain = domains[adjacentPosition.x, adjacentPosition.y];
                    for (int i = 0; i < domain.Count; i++)
                    {
                        if (!GetCoupling(index, domain.GetDense(i), d))
                        {
                            size++;
                        }
                    }
                }
            }

            return size;
        }

        public static string GetCouplingTypeString(CouplingType type)
        {
            switch (type)
            {
                case CouplingType.FourTiles:
                    return "Four way";
                case CouplingType.EightTiles:
                    return "Eight way";
                case CouplingType.HexagonalTiles:
                    return "Hexagonal";
                default:
                    return "Invalid connectivity type";
            }
        }
    }
