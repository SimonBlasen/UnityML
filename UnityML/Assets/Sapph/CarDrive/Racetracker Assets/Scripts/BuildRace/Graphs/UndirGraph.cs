using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UndirGraph<T> : Graph<T>
{
    public UndirGraph()
    {
        nodes = new List<Node<T>>();
        edges = new List<Edge<T>>();
    }

    public override void AddEdge(Node<T> from, Node<T> to)
    {
        Edge<T> newEdge = new Edge<T>(from, to);
        Edge<T> backEdge = new Edge<T>(to, from);
        if (!edges.Contains(newEdge))
        {
            edges.Add(newEdge);
        }
        if (!edges.Contains(backEdge))
        {
            edges.Add(backEdge);
        }
    }

    public override bool AddNode(T node)
    {
        Node<T> newNode = new Node<T>(node);
        if (!nodes.Contains(newNode))
        {
            nodes.Add(newNode);
            return true;
        }

        return false;
    }
}
