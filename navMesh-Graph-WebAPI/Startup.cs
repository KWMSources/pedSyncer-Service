using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NavMesh_Graph;
using Newtonsoft.Json;

namespace navMesh_Graph_WebAPI
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                //Standard Endpoint
                endpoints.MapGet("/", async context => { await context.Response.WriteAsync("Hello World!"); });
                
                //Endpoint to get random spawns
                endpoints.MapGet("/getSpawns", async context =>
                {
                    List<NavigationMeshPolyFootpath>
                        spawns = NavigationMeshControl.getInstance().getRandomSpawnMeshes();
                    List<NavMeshPath> spawnsVektor = new List<NavMeshPath>();

                    foreach (NavigationMeshPolyFootpath spawn in spawns)
                    {
                        NavMeshPath path = new NavMeshPath(spawn.Position.X, spawn.Position.Y, spawn.Position.Z);

                        List<NavigationMeshPolyFootpath> pathNavMeshes =
                            NavigationMeshControl.getInstance().getRandomPathByMesh(spawn);

                        foreach (NavigationMeshPolyFootpath pathNavMesh in pathNavMeshes)
                        {
                            path.path.Add(pathNavMesh.Position);
                        }
                        
                        spawnsVektor.Add(path);
                    }

                    await context.Response.WriteAsync(JsonConvert.SerializeObject(spawnsVektor)); 
                    
                });
                
                //Endpoint to get a new path by given endCoords and the pre-end coords
                endpoints.MapGet("/getRoute/{endX:float}/{endY:float}/{endZ:float}/{preEndX:float}/{preEndY:float}/{preEndZ:float}", async context =>
                {
                    //Parse the given param coords
                    WorldVector3 endPosition = new WorldVector3(float.Parse(context.Request.RouteValues["endX"].ToString().Replace(".",".")), float.Parse(context.Request.RouteValues["endY"].ToString().Replace(".",".")), float.Parse(context.Request.RouteValues["endZ"].ToString().Replace(".",".")));
                    WorldVector3 preEndPosition = new WorldVector3(float.Parse(context.Request.RouteValues["preEndX"].ToString().Replace(".",".")), float.Parse(context.Request.RouteValues["preEndY"].ToString().Replace(".",".")), float.Parse(context.Request.RouteValues["preEndZ"].ToString().Replace(".",".")));

                    //Get endNavMesh by position
                    NavigationMeshPolyFootpath endMesh = NavigationMeshControl.getInstance().getMeshByPosition(endPosition);
                    
                    //Determine gon (direction) of the two coordinates
                    double gon = WorldVector3.directionalAngle(preEndPosition, endPosition);
                    
                    //Create a path for the ped
                    NavMeshPath path = new NavMeshPath(endPosition.X, endPosition.Y, endPosition.Z);
                    List<NavigationMeshPolyFootpath> pathNavMeshes = NavigationMeshControl.getInstance().getRandomPathByMeshAndGon(endMesh, gon);

                    foreach (NavigationMeshPolyFootpath pathNavMesh in pathNavMeshes)
                    {
                        path.path.Add(pathNavMesh.Position);
                    }

                    //Give the path back
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(path)); 
                });
            });
        }
    }
}