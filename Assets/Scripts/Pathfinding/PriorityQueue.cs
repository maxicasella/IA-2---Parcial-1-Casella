using System.Collections.Generic;
using System.Linq;

public class PriorityQueue<T>
{

    private List<WeightedNode<T>> _queue = new List<WeightedNode<T>>();

    /// <summary>
    /// Adds an element to the Queue 
    /// </summary>
    /// <param name="element"></param>
    public void Enqueue(WeightedNode<T> element)
    {
        if (element == null || element.Element == null)
        {
            return;
        }
        _queue.Add(element);
    }

    /// <summary>
    /// Returns the element with minimum value in the Queue
    /// </summary>
    /// <returns></returns>
    public WeightedNode<T> Dequeue()
    {
        if (_queue.Count == 0)
        {
            return null;
        }

        WeightedNode<T> min = null;
        float minWeight = float.MaxValue;

        foreach (var element in _queue)
        {
            if (element == null)
            {
                continue;
            }

            if (element.Weight < minWeight)
            {
                min = element;
                minWeight = element.Weight;
            }
        }

        if (min == null)
        {
            return null;
        }

        _queue.Remove(min);
        return min;
    }

    /// <summary>
    /// Returns true if the Queue does not contain any elements
    /// </summary>
    public bool IsEmpty => _queue.Count == 0;
}

