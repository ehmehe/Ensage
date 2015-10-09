using System;
using Ensage;
using SharpDX;

namespace iZeus
{
    public static class iGeometry
    {
        /// <summary>
        ///     Calculates the 2D distance to the unit.
        /// </summary>
        public static float Distance(this Unit unit, Unit anotherUnit, bool squared = false)
        {
            return unit.NetworkPosition.To2D().Distance(anotherUnit.Position.To2D(), squared);
        }

        /// <summary>
        ///     Calculates the 2D distance to the point.
        /// </summary>
        public static float Distance(this Unit unit, Vector3 point, bool squared = false)
        {
            return unit.NetworkPosition.To2D().Distance(point.To2D(), squared);
        }

        /// <summary>
        ///     Calculates the 2D distance to the point.
        /// </summary>
        public static float Distance(this Unit unit, Vector2 point, bool squared = false)
        {
            return unit.NetworkPosition.To2D().Distance(point, squared);
        }

        /// <summary>
        ///     Calculates the 3D distance to the unit.
        /// </summary>
        public static float Distance3D(this Unit unit, Unit anotherUnit, bool squared = false)
        {
            return squared
                ? Vector3.DistanceSquared(unit.Position, anotherUnit.Position)
                : Vector3.Distance(unit.Position, anotherUnit.Position);
        }

        //Vector3 class extended methods:

        /// <summary>
        ///     Converts a Vector3 to Vector2
        /// </summary>
        public static Vector2 To2D(this Vector3 v)
        {
            return new Vector2(v.X, v.Y);
        }

        /// <summary>
        ///     Returns the 2D distance (XY plane) between two vector.
        /// </summary>
        public static float Distance(this Vector3 v, Vector3 other, bool squared = false)
        {
            return v.To2D().Distance(other, squared);
        }

        //Vector2 class extended methods:

        /// <summary>
        ///     Returns true if the vector is valid.
        /// </summary>
        public static bool IsValid(this Vector2 v)
        {
            return v != Vector2.Zero;
        }

        public static bool IsValid(this Vector3 v)
        {
            return v != Vector3.Zero;
        }

        /// <summary>
        ///     Converts the Vector2 to Vector3. (Z = Player.NetworkPosition.Z)
        /// </summary>
        public static Vector3 To3D(this Vector2 v)
        {
            return new Vector3(v.X, v.Y, ObjectMgr.LocalHero.NetworkPosition.Z);
        }

        /// <summary>
        ///     Calculates the distance to the Vector2.
        /// </summary>
        public static float Distance(this Vector2 v, Vector2 to, bool squared = false)
        {
            return squared ? Vector2.DistanceSquared(v, to) : Vector2.Distance(v, to);
        }

        /// <summary>
        ///     Calculates the distance to the Vector3.
        /// </summary>
        public static float Distance(this Vector2 v, Vector3 to, bool squared = false)
        {
            return v.Distance(to.To2D(), squared);
        }

        /// <summary>
        ///     Calculates the distance to the unit.
        /// </summary>
        public static float Distance(this Vector2 v, Unit to, bool squared = false)
        {
            return v.Distance(to.NetworkPosition.To2D(), squared);
        }

        /// <summary>
        ///     Retursn the distance to the line segment.
        /// </summary>
        public static float Distance(this Vector2 point,
            Vector2 segmentStart,
            Vector2 segmentEnd,
            bool onlyIfOnSegment = false,
            bool squared = false)
        {
            var objects = point.ProjectOn(segmentStart, segmentEnd);

            if (objects.IsOnSegment || onlyIfOnSegment == false)
            {
                return squared
                    ? Vector2.DistanceSquared(objects.SegmentPoint, point)
                    : Vector2.Distance(objects.SegmentPoint, point);
            }
            return float.MaxValue;
        }

        /// <summary>
        ///     Returns the vector normalized.
        /// </summary>
        public static Vector2 Normalized(this Vector2 v)
        {
            v.Normalize();
            return v;
        }

        public static Vector3 Normalized(this Vector3 v)
        {
            v.Normalize();
            return v;
        }

        public static Vector2 Extend(this Vector2 v, Vector2 to, float distance)
        {
            return v + distance * (to - v).Normalized();
        }

        public static Vector3 Extend(this Vector3 v, Vector3 to, float distance)
        {
            return v + distance * (to - v).Normalized();
        }

        public static Vector2 Shorten(this Vector2 v, Vector2 to, float distance)
        {
            return v - distance * (to - v).Normalized();
        }

        public static Vector3 Shorten(this Vector3 v, Vector3 to, float distance)
        {
            return v - distance * (to - v).Normalized();
        }

        public static Vector3 SwitchYZ(this Vector3 v)
        {
            return new Vector3(v.X, v.Z, v.Y);
        }

        /// <summary>
        ///     Returns the perpendicular vector.
        /// </summary>
        public static Vector2 Perpendicular(this Vector2 v)
        {
            return new Vector2(-v.Y, v.X);
        }

        /// <summary>
        ///     Returns the second perpendicular vector.
        /// </summary>
        public static Vector2 Perpendicular2(this Vector2 v)
        {
            return new Vector2(v.Y, -v.X);
        }

        /// <summary>
        ///     Rotates the vector a set angle (angle in radians).
        /// </summary>
        public static Vector2 Rotated(this Vector2 v, float angle)
        {
            var c = Math.Cos(angle);
            var s = Math.Sin(angle);

            return new Vector2((float)(v.X * c - v.Y * s), (float)(v.Y * c + v.X * s));
        }

        /// <summary>
        ///     Returns the cross product Z value.
        /// </summary>
        public static float CrossProduct(this Vector2 self, Vector2 other)
        {
            return other.Y * self.X - other.X * self.Y;
        }

        public static float RadianToDegree(double angle)
        {
            return (float)(angle * (180.0 / Math.PI));
        }

        public static float DegreeToRadian(double angle)
        {
            return (float)(Math.PI * angle / 180.0);
        }

        public struct ProjectionInfo
        {
            public bool IsOnSegment;
            public Vector2 LinePoint;
            public Vector2 SegmentPoint;

            public ProjectionInfo(bool isOnSegment, Vector2 segmentPoint, Vector2 linePoint)
            {
                IsOnSegment = isOnSegment;
                SegmentPoint = segmentPoint;
                LinePoint = linePoint;
            }
        }

        public static ProjectionInfo ProjectOn(this Vector2 point, Vector2 segmentStart, Vector2 segmentEnd)
        {
            var cx = point.X;
            var cy = point.Y;
            var ax = segmentStart.X;
            var ay = segmentStart.Y;
            var bx = segmentEnd.X;
            var by = segmentEnd.Y;
            var rL = ((cx - ax) * (bx - ax) + (cy - ay) * (by - ay)) /
                     ((float)Math.Pow(bx - ax, 2) + (float)Math.Pow(by - ay, 2));
            var pointLine = new Vector2(ax + rL * (bx - ax), ay + rL * (by - ay));
            float rS;
            if (rL < 0)
            {
                rS = 0;
            }
            else if (rL > 1)
            {
                rS = 1;
            }
            else
            {
                rS = rL;
            }

            var isOnSegment = rS.CompareTo(rL) == 0;
            var pointSegment = isOnSegment ? pointLine : new Vector2(ax + rS * (bx - ax), ay + rS * (@by - ay));
            return new ProjectionInfo(isOnSegment, pointSegment, pointLine);
        }
    }
}
