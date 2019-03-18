using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Assets.Scripts.ExtensionMethods;
using Assets.Scripts.Utilities.PriorityQueue;

namespace Assets.Scripts.Utilities.WeightedGraph {
    public class WeightedGraph {
        class WeightedNode : IComparable<WeightedNode> {
            public Vector2Int mVertex;
            public List<WeightedEdge> mNeighbors;

            public WeightedNode(Vector2Int vertex) {
                mVertex = vertex;
                mNeighbors = new List<WeightedEdge>();
            }

            public int CompareTo(WeightedNode other) {
                return this.mVertex.CompareTo(other.mVertex);
            }

        }

        class WeightedEdge : IComparable<WeightedEdge> {
            public WeightedNode mFirst, mSecond;
            public double weight;

            public WeightedEdge(WeightedNode first, WeightedNode second, double weight) {
                this.mFirst = first;
                this.mSecond = second;
                this.weight = weight;
            }

            public int CompareTo(WeightedEdge other) {
                if (this.weight.CompareTo(other.weight) == 0) {
                    if (this.mFirst == other.mFirst) {
                        return this.mSecond.CompareTo(other.mSecond);
                    }
                    return this.mFirst.CompareTo(other.mFirst);
                }
                return weight.CompareTo(other.weight);
            }
        }

        private List<WeightedNode> mVertices;

        public WeightedGraph(Dictionary<Vector2Int, int> locations) {
            mVertices = new List<WeightedNode>();

            foreach (var loc in locations) {
                var vertex = loc.Key;
                mVertices.Add(new WeightedNode(vertex));
            }

            foreach (var loc in locations) {
                var vertex = loc.Key;
                foreach (var neighbor in vertex.GetNeighbors()) {
                    if (locations.Keys.Contains(neighbor)) {
                        WeightedNode mFirst = mVertices.SingleOrDefault(v => v.mVertex == vertex);
                        WeightedNode mSecond = mVertices.SingleOrDefault(v => v.mVertex == neighbor);

                        WeightedEdge edge = new WeightedEdge(mFirst, mSecond, locations[vertex]);
                        mFirst.mNeighbors.Add(edge);
                        mSecond.mNeighbors.Add(edge);
                    }
                }
            }
        }

        public List<DijkstraDistance> GetShortestDistancesFrom(Vector2Int source) {
            SimplePriorityQueue<DijkstraDistance> vertexQueue = new SimplePriorityQueue<DijkstraDistance>();
            DijkstraDistance start = new DijkstraDistance(source, 0);
            start.Path.Add(source);
            List<DijkstraDistance> distances = new List<DijkstraDistance>() {
                start
            };

            foreach (var vertex in mVertices) {
                DijkstraDistance dijkstraDistance = new DijkstraDistance(vertex.mVertex, Int32.MaxValue);
                if (vertex.mVertex != source) {
                    distances.Add(dijkstraDistance);
                }
                vertexQueue.Enqueue(dijkstraDistance, (float) dijkstraDistance.CurrentDistance);
            }

            while (vertexQueue.Count > 0) {
                var uDistance = vertexQueue.Dequeue();

                var uVertex = mVertices.SingleOrDefault(v => v.mVertex == uDistance.Vertex);

                foreach (var edge in uVertex.mNeighbors) {
                    WeightedNode vVertex;
                    if (edge.mFirst == uVertex) {
                        vVertex = edge.mSecond;
                    } else {
                        vVertex = edge.mFirst;
                    }

                    if (!vVertex.Equals(uVertex)) {
                        if (vertexQueue.Contains(distances.SingleOrDefault(d => d.Vertex == vVertex.mVertex ))) {
                            double length = distances.SingleOrDefault(d => d.Vertex == uVertex.mVertex).CurrentDistance + edge.weight;
                            if (length < distances.SingleOrDefault(d => d.Vertex == vVertex.mVertex).CurrentDistance) {
                                vertexQueue.Remove(distances.SingleOrDefault(d => d.Vertex == vVertex.mVertex));
                                distances.SingleOrDefault(d => d.Vertex == vVertex.mVertex).CurrentDistance = length;
                                distances.SingleOrDefault(d => d.Vertex == vVertex.mVertex).Path.Clear();
                                distances.SingleOrDefault(d => d.Vertex == vVertex.mVertex).Path.AddRange(uDistance.Path);
                                distances.SingleOrDefault(d => d.Vertex == vVertex.mVertex).Path.Add(vVertex.mVertex);
                                vertexQueue.Enqueue(distances.SingleOrDefault(d => d.Vertex == vVertex.mVertex), (float) distances.SingleOrDefault(d => d.Vertex == vVertex.mVertex).CurrentDistance);
                            }
                        }
                    }
                }
            }

            return distances;
        }

        public class DijkstraDistance : IComparable<DijkstraDistance> {
            public Vector2Int Vertex { get; private set; }
            public double CurrentDistance { get; set; }
            public List<Vector2Int> Path { get; set; }

            public DijkstraDistance(Vector2Int vertex, double distance) {
                Vertex = vertex;
                CurrentDistance = distance;
                Path = new List<Vector2Int>();
            }

            public int CompareTo(DijkstraDistance other) {
                return this.CurrentDistance.CompareTo(other.CurrentDistance);
            }
        }
    }
}
