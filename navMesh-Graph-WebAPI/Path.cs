using System.Collections.Generic;
using NavMesh_Graph;

namespace navMesh_Graph_WebAPI
{
    public class NavMeshPath: WorldVector3
    {
        public NavMeshPath(float X, float Y, float Z): base(X,Y,Z)
        {
            
        }
        
        public List<WorldVector3> path = new List<WorldVector3>();
    }
}