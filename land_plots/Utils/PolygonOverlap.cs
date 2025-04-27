using System;
using System.Collections.Generic;
using System.Windows;

namespace LandManagementApp.Utils
{
    // для перевірки перетину двох полігонів
    public static class PolygonOverlap
    {
        // метод перевіря чи перетинаються полігони polygonA і polygonB
        public static bool Check(List<Point> polygonA, List<Point> polygonB)
        {
            // проходимо по обох полігонах
            foreach (var polygon in new[] { polygonA, polygonB })
            {
                //для кожного ребра полігона
                for (int i = 0; i < polygon.Count; i++)
                {
                    Point edgeStart = polygon[i];
                    Point edgeEnd = polygon[(i + 1) % polygon.Count]; //наступна точка (+циклічний перехід до початку)

                    //вектор ребра
                    Vector edge = edgeEnd - edgeStart;

                    // нормальний вектор до ребра (перпендикуляр)
                    Vector axis = new Vector(-edge.Y, edge.X);
                    axis.Normalize(); // Робимо вісь одиничної довжини

                    // якщо полігони не перетинаються на цій осі — точно немає перетину
                    if (!OverlapOnAxis(polygonA, polygonB, axis))
                        return false;
                }
            }

            // якщо перетини є на всіх осях — полігони перетинаються
            return true;
        }
        //OverlapOnAxis
    }
}
