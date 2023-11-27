using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public static class AStar {

    /// <summary>
    /// Calculates a path using AStar. See more at https://github.com/kgazcurra/ProLibraryWiki/wiki/AStar
    /// </summary>
    /// <param name="start">The node where it starts calculating the path</param>
    /// <param name="isGoal">A function that, given a node, tells us whether we reached or goal or not</param>
    /// <param name="explode">A function that returns all the near neighbours of a given node</param>
    /// <param name="getHeuristic">A function that returns the heuristic of the given node</param>
    /// <typeparam name="T">Node type</typeparam>
    /// <returns>Returns a path from start node to goal. Returns null if a path could not be found</returns>
    public static IEnumerator CalculatePath<T>(T start,
                                                  Func<T, bool> isGoal,
                                                  Func<T, IEnumerable<WeightedNode<T>>> explode,
                                                  Func<T, float> getHeuristic,
                                                  Action<IEnumerable<T>> onPathCompleted,
                                                  Action pathCantComplete) 
    {
        var queue = new PriorityQueue<T>();
        var distances = new Dictionary<T, float>();
        var parents = new Dictionary<T, T>();
        var visited = new HashSet<T>();

        distances[start] = 0;
        queue.Enqueue(new WeightedNode<T>(start, 0));
   
        bool pathCompleted = false;
        Stopwatch myStopwatch = new Stopwatch(); //IA2-P4
        myStopwatch.Start();

        while (!queue.IsEmpty)
        {
            var dequeued = queue.Dequeue();
            visited.Add(dequeued.Element);

            if (isGoal(dequeued.Element))
            {
                onPathCompleted?.Invoke(CommonUtils.CreatePath(parents, dequeued.Element));
                pathCompleted = true;
                break;
            }

            var toEnqueue = explode(dequeued.Element);

            foreach (var transition in toEnqueue)
            {
                var neighbour = transition.Element;
                var neighbourToDequeuedDistance = transition.Weight;

                var startToNeighbourDistance =
                    distances.ContainsKey(neighbour) ? distances[neighbour] : float.MaxValue;
                var startToDequeuedDistance = distances[dequeued.Element];

                var newDistance = startToDequeuedDistance + neighbourToDequeuedDistance;

                if (!visited.Contains(neighbour) && startToNeighbourDistance > newDistance)
                {
                    distances[neighbour] = newDistance;
                    parents[neighbour] = dequeued.Element;

                    queue.Enqueue(new WeightedNode<T>(neighbour, newDistance + getHeuristic(neighbour)));
                }


            }

            if (myStopwatch.ElapsedMilliseconds > TimeSlicing.elapsedFramesPerSecond)
            {
                yield return new WaitForEndOfFrame();
                myStopwatch.Restart();
            }
        }

        if (!pathCompleted) pathCantComplete?.Invoke();
    }

    }
