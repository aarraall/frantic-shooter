using System;
using UnityEngine;
using Zenject;

public class FrustumService : IFixedTickable, IDisposable
{
    [Inject] Camera _camera;
    Plane[] _cameraFrustum;

    public Vector3 GetClosestFrustumPoint(Vector3 point)
    {
        float distance = float.PositiveInfinity;
        Vector3 closestPoint = Vector3.zero;

        for (int i = 0; i < _cameraFrustum.Length; i++)
        {
            Plane plane = _cameraFrustum[i];

            var closestPointOnPlane = plane.ClosestPointOnPlane(point);
            var distanceToPlane = Vector3.Distance(point, closestPointOnPlane);

            if (distanceToPlane < distance)
            {
                distance = distanceToPlane;
                closestPoint = closestPointOnPlane;
            }
        }
        return closestPoint;
    }


    /// <summary>
    /// Returns if point intersects with any of the frustum planes
    /// If so, it returns intersectionPoint
    /// </summary>
    /// <param name="point"></param>
    /// <param name="radius"></param>
    /// <param name="intersectPoint"></param>
    /// <returns></returns>
    public bool IsIntersecting(Vector3 point, float radius, out Vector3 intersectionPoint)
    {
        intersectionPoint = Vector3.zero;
        var closestFrustumPoint = GetClosestFrustumPoint(point);

        if (Vector3.Distance(point, closestFrustumPoint) <= radius)
        {
            intersectionPoint = closestFrustumPoint;
            return true;
        }

        return false;

    }

    public void Dispose()
    {
        _camera = null;
        _cameraFrustum = null;
    }

    public void FixedTick()
    {
        if (_camera == null) return;
        _cameraFrustum = GeometryUtility.CalculateFrustumPlanes(_camera);
    }

}
