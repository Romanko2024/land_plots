using System;
using System.Collections.Generic;
using System.Windows;

namespace LandManagementApp.Utils
{
    // для перевірки перетину двох полігонів
    //метод розділяючої осі (Separating Axis Theorem - SAT)
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
        // перевіряє чи накладаються проєкції обох полігонів на вісь
        private static bool OverlapOnAxis(List<Point> a, List<Point> b, Vector axis)
        {
            var (minA, maxA) = Project(a, axis); // проєкція полігону A
            var (minB, maxB) = Project(b, axis); // проєкція полігону B

            // проєкції перекриваються якщо їхні інтервали мають спільну частину
            return maxA >= minB && maxB >= minA;
        }

        // проектує всі точки полігону на вісь і знаходить мінімальну та максимальну координату
        private static (double min, double max) Project(List<Point> points, Vector axis)
        {
            double min = double.PositiveInfinity;
            double max = double.NegativeInfinity;

            foreach (var point in points)
            {
                // скалярний добуток точки на вісь — це координата проєкції
                double proj = point.X * axis.X + point.Y * axis.Y;

                //оновлюємо мінімальне та максимальне значення проєкції
                min = Math.Min(min, proj);
                max = Math.Max(max, proj);
            }

            return (min, max); // повертаємо межі проєкції
        }
    }
}
