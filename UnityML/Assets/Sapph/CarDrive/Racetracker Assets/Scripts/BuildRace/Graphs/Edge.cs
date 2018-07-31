using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Edge<T>
{
    private Node<T> from;
    private Node<T> to;

    public Edge(Node<T> from, Node<T> to)
    {
        this.from = from;
        this.to = to;
    }

    public Node<T> From
    {
        get
        {
            return from;
        }
    }

    public Node<T> To
    {
        get
        {
            return to;
        }
    }

    public override bool Equals(object obj)
    {
        if (obj is Edge<T>)
        {
            Edge<T> other = (Edge<T>)obj;
            return other.from.Equals(from) && other.to.Equals(to);
        }
        else
        {
            return base.Equals(obj);
        }
    }

    public static bool operator ==(Edge<T> a, Edge<T> b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(Edge<T> a, Edge<T> b)
    {
        return !a.Equals(b);
    }
}
