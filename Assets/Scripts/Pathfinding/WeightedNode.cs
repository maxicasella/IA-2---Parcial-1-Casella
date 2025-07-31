using System;
using UnityEngine;

[Serializable]
public class WeightedNode<T>
{

    [SerializeField] private T _element;
    [SerializeField] private float _weight;

    public T Element => _element;
    public float Weight => _weight;


    public WeightedNode(T element, float weight)
    {
        if (element == null)
            Debug.Log("Se creó un WeightedNode con elemento null.");

        _element = element;
        _weight = weight;
    }
}