using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    Pathfinding2D pathfinder;

    void Start()
    {
        pathfinder = GetComponent<Pathfinding2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(WalkTo(pathfinder.TargetPos));
        }
    }

    /// <summary>
    /// Finds the best path to go the target position then walks to it.
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    IEnumerator WalkTo(Vector3 target)
    {
        var path = pathfinder.GetPath(target);
        foreach (var position in path)
        {
            yield return WalkAnimation(position);
            transform.position = position;
        }
    }

    /// <summary>
    /// Walking animation from the current cell to the neighboring target cell.
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    IEnumerator WalkAnimation(Vector3 target)
    {
        var startPosition = transform.position;
        for (float i = 0.1f; i <= 1; i += 0.1f)
        {
            transform.position = startPosition * Mathf.Abs(i - 1) + target * i;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
