# pedSyncer-Service

This is the Web-Service for the pedSyncer-ressource in altV. The mission of the pedSyncer-service is to calculate random spawns and paths for the wandering of the peds. The base of the calculation are the navMeshes from GTA V. The file `newNavigationMeshes.msgpack` includes all footpath navMeshes from GTA 5. There are ~ 98,000 navMeshes, which are managed by the service.

The pedSyncer needs an external service because the amount of navMeshes to manage is to big. The calculations of a path or of random spwans would be to much for NodeJS. The loading process of the `newNavigationMeshes.msgpack`-file would block the NodeJS event loop for a long time. Once loaded, the calculation of random paths and spawns is a task with a small load but the path calculation appears very, very often. For this reason, the load of the NodeJS increases with the number of peds. An external service which is contacted by NodeJS with REST-API as promises is the solution of this problem for now.

# Requirements

- A environment to run c#
- Packages: MessagePack/2.1.115, Newtonsoft.JSON/12.0.3

# Installation

- Download the repository
- go to bin/Debug/netcoreapp3.1/ and run navMesh-Graph-WebAPI.exe

# Endpoints

`https://localhost:5001/getSpawns` - gives back an JSON of random spawn positions and calculated routes

`https://localhost:5001/getRoute/{endPosition.x}/{endPosition.y}/{endPosition.z}/{preEndPosition.x}/{preEndPosition.y}/{preEndPosition.z}` - gives back an JSON of a path by the given two positions (two position is important because of the direction of the current wandering)
