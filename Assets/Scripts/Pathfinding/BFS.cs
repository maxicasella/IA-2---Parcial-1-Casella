using System;
using System.Collections.Generic;

public static class BFS {

    /// <summary>
    /// Calculates a path using Breadth First Search. See more at https://github.com/kgazcurra/ProLibraryWiki/wiki/BFS
    /// </summary>
    /// <param name="start">The node where it starts calculating the path</param>
    /// <param name="isGoal">A function that, given a node, tells us whether we reached or goal or not</param>
    /// <param name="explode">A function that returns all the near neighbours of a given node</param>
    /// <typeparam name="T">Node type</typeparam>
    /// <returns>Returns a path from start node to goal</returns>
    public static IEnumerable<T> CalculatePath<T>(T start, Func<T, bool> isGoal, Func<T, IEnumerable<T>> explode) {
        var path  = new List<T>(){start};
        var queue = new Queue<T>();

        queue.Enqueue(start);

        while (queue.Count > 0) {
            var dequeued = queue.Dequeue();

            if (isGoal(dequeued)) return path;

            var toEnqueue = explode(dequeued);
            foreach (var n in toEnqueue) {
                path.Add(n);
                queue.Enqueue(n);
            }

        }

        return path;
    }

}