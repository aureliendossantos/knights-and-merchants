using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Helpers
{
    /// <summary>
    /// Allows to write foreach loops with indexes.
    /// </summary>
    /// <returns>The same Enumerable with (item, index) values.</returns>
    public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> self)
    {
        return self.Select((item, index) => (item, index));
    }

    /// <summary>
    /// Repeats an action a given number of times.
    /// </summary>
    /// <param name="count">Number of times the action will be repeated.</param>
    /// <param name="action">The action to repeat</param>
    public static void Repeat(int count, System.Action<int> action)
    {
        for (int i = 0; i < count; i++)
        {
            action(i);
        }
    }

    /// <summary>
    /// Détruit tous les enfants d'un GameObject.
    /// </summary>
    public static void DestroyChildren(GameObject t)
    {
        t.transform.Cast<Transform>().ToList().ForEach(c => UnityEngine.Object.Destroy(c.gameObject));
    }

    /// <summary>
    /// Liste des coordonnées correspondant à la case et aux 8 cases autour.
    /// </summary>
    public static Vector2Int[] nearbyCells = new Vector2Int[] {
        new Vector2Int(0, 0),
        new Vector2Int(-1, -1),
        new Vector2Int(-1, 0),
        new Vector2Int(-1, 1),
        new Vector2Int(0, -1),
        new Vector2Int(0, 1),
        new Vector2Int(1, -1),
        new Vector2Int(1, 0),
        new Vector2Int(1, 1),
    };
}
