﻿﻿using System.Collections.Generic;
using MessagePack;

namespace NavMesh_Graph
{
    [MessagePackObject]
    public class NavigationMeshPolyFootpath
    {
        [Key(0)]
        public int Index { get; set; }

        [Key(1)]
        public int PartId { get; set; }

        [Key(2)]
        public WorldVector3 Position { get; set; }

        [Key(3)]
        public List<WorldVector3> Vertices { get; set; }

        [Key(4)]
        public int Id { get; set; }
        
        [Key(5)]
        public int AreaId { get; set; }

        [Key(6)]
        public List<int> Neighbours { get; set; }
        
        [IgnoreMember]
        public List<NavigationMeshPolyFootpath> NeighboursObjects { get; set; }

        [SerializationConstructor]
        public NavigationMeshPolyFootpath()
        {
        }

        public NavigationMeshPolyFootpath(NavigationMeshPoly obj)
        {
            this.Index = obj.Index;
            this.PartId = obj.PartId;
            this.Position = obj.Position;
            this.Vertices = obj.Vertices;
            this.Id = obj.Id;
            this.AreaId = obj.AreaId;
            this.Neighbours = new List<int>();
        }
        
        
        public static bool isNeighbour(NavigationMeshPolyFootpath poly1, NavigationMeshPolyFootpath poly2)
        {
            foreach (WorldVector3 vec1 in poly1.Vertices)
            {
                foreach (WorldVector3 vec2 in poly2.Vertices)
                {
                    if (WorldVector3.@equals(vec1, vec2)) return true;
                }
            }

            return false;
        }
    }
}