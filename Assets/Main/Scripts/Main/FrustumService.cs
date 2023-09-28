using UnityEngine;
using Zenject;

public class FrustumService : MonoBehaviour
{
    [Inject] Camera _camera;
    Plane[] _cameraFrustum;

    public Vector3 GetClosestFrustumPoint(Vector3 point, bool checkPositiveSide = false)
    {
        float distance = float.PositiveInfinity;
        Vector3 closestPoint = Vector3.zero;

        foreach (Plane plane in _cameraFrustum)
        {
            var closestPointOnPlane = plane.ClosestPointOnPlane(point);
            var pointOnPositiveSide = true;

            if (checkPositiveSide)
            {
                pointOnPositiveSide = plane.GetSide(point);
            }
            var distanceToPlane = Vector3.Distance(point, closestPointOnPlane);

            if (distanceToPlane < distance && pointOnPositiveSide)
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
        var closestFrustumPoint = GetClosestFrustumPoint(point, true);

        if (Vector3.Distance(point, closestFrustumPoint) <= radius)
        {
            intersectionPoint = closestFrustumPoint;
            return true;
        }

        return false;

    }


    private void Update()
    {
        if (_camera == null) return;
        _cameraFrustum = GeometryUtility.CalculateFrustumPlanes(_camera);
    }
}
