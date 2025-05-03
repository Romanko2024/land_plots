using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace LandManagementApp.Utils
{
    // для перевірки перетину двох полігонів
    //метод розділяючої осі (Separating Axis Theorem - SAT)
    public static class PolygonOverlap
    {
        // метод перевіря чи перетинаються полігони polygonA і polygonB
        public static bool Check(IEnumerable<Point> polygonA, IEnumerable<Point> polygonB)
        {
            var listA = polygonA.ToList();
            var listB = polygonB.ToList();
            //перевірка на повне включення одного полігону в інший
            if (IsPolygonInside(listA, listB) || IsPolygonInside(listB, listA))
                return true;

            //перевірка за допомогою методу розділяючої осі (SAT)
            return CheckWithSAT(listA, listB);
        }

        private static bool CheckWithSAT(List<Point> a, List<Point> b)
        {
            // проходимо по обох полігонах
            foreach (var polygon in new[] { a, b })
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

                    // нормальний вектор до ребра (перпендикуляр)
                    if (!OverlapOnAxis(a, b, axis))
                        return false;
                }
            }
            return true;
        }

        private static bool IsPolygonInside(List<Point> polyA, List<Point> polyB)
        {
            foreach (var point in polyA)
            {
                if (!IsPointInsidePolygon(point, polyB))
                    return false;
            }
            return true;
        }

        private static bool IsPointInsidePolygon(Point p, List<Point> polygon)
        {
            int intersections = 0;
            for (int i = 0; i < polygon.Count; i++)
            {
                Point a = polygon[i];
                Point b = polygon[(i + 1) % polygon.Count];

                if (RayCrossesSegment(p, a, b))
                    intersections++;
            }
            return (intersections % 2) == 1;
        }

        private static bool RayCrossesSegment(Point p, Point a, Point b)
        {
            double eps = 1e-8;

            if (a.Y > b.Y)
                (a, b) = (b, a);

            if (p.Y < a.Y - eps || p.Y > b.Y + eps)
                return false;

            if (Math.Abs(a.Y - b.Y) < eps)
                return false;

            double xIntersect = a.X + (p.Y - a.Y) * (b.X - a.X) / (b.Y - a.Y);

            return p.X <= xIntersect + eps &&
                   xIntersect - eps <= p.X &&
                   p.Y >= a.Y - eps &&
                   p.Y <= b.Y + eps;
        }

        private static bool OverlapOnAxis(List<Point> a, List<Point> b, Vector axis)
        {
            var (minA, maxA) = Project(a, axis);
            var (minB, maxB) = Project(b, axis);
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