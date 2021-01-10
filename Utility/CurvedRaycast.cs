using UnityEngine;
using System.Collections.Generic;

public static class Ext_Physics
{

    /// <summary>
    /// Performs a curved raycast.
    /// </summary>
    public static bool CurveCast(Vector3 origin, Vector3 direction, Vector3 gravityDirection, float smoothness, out RaycastHit hitInfo, float maxDistance, out List<Vector3> points, LayerMask layerMask)
    {
        if (maxDistance == Mathf.Infinity) maxDistance = 200;
        Vector3 currPos = origin, hypoPos = origin, hypoVel = direction / smoothness;
        List<Vector3> v = new List<Vector3>();
        RaycastHit hit;
        float curveCastLength = 0;

        do
        {
            v.Add(hypoPos);
            currPos = hypoPos;
            hypoPos = currPos + hypoVel + (gravityDirection * Time.fixedDeltaTime / (smoothness * smoothness));
            hypoVel = hypoPos - currPos;
            curveCastLength += hypoVel.magnitude;
        }
        while (UnityEngine.Physics.Raycast(currPos, hypoVel, out hit, hypoVel.magnitude, layerMask) == false && curveCastLength < maxDistance);
        hitInfo = hit;
        points = v;
        return UnityEngine.Physics.Raycast(currPos, hypoVel, hypoVel.magnitude);
    }

}