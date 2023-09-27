using UnityEngine;

public class FrustumService : MonoBehaviour
{
    Camera _camera;
    Plane[] _cameraFrustum;

    public static FrustumService Instance;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        _camera = Camera.main;
    }


    public Vector3 GetClosestFrustumPoint(Vector3 point)
    {
        float distance = float.PositiveInfinity;
        Vector3 closestPoint = Vector3.zero;

        foreach (Plane plane in _cameraFrustum)
        {
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
    /// <param name="threshold"></param>
    /// <param name="intersectPoint"></param>
    /// <returns></returns>
    public bool IsIntersecting(Vector3 point, float threshold, out Vector3 intersectionPoint)
    {
        intersectionPoint = Vector3.zero;
        var closestFrustumPoint = GetClosestFrustumPoint(point);

        if (Vector3.Distance(point, closestFrustumPoint) <= threshold)
        {
            intersectionPoint = closestFrustumPoint;
            return true;
        }

        return false;

    }


    private void Update()
    {
        _cameraFrustum = GeometryUtility.CalculateFrustumPlanes(_camera);
    }
}
