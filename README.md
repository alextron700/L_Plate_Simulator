# L_Plate_Simulator
This is my L-plate simulator, written in unity, as a school project. Most of this has been made without access to assets from the asset store, due to network restrictions at school. This repository contains everything one needs to run the project.
Features:
- 3D Environment
- working indicators
- working speedometer
- Custom car control system
## A brief overview of some of the main scripts
- Testscript controls all the player logic, and car movements.
- CarAI controls the behaviors of NPC Cars, and it can even follow a preset route.
- CarAINavigationCheckpoint defines the wapoints that make up a route
- WaypointRouteManager turns a series of waypoints into a route, which CarAI Can then be assigned to follow.
- ScenarioManager is a script that in later updates is hoped to have the ability to turn JSON files into playable levels in game.
- DayNightSystem manipulates a directional light to control when it's day or night
- TrafficLightManager makes traffic lights work.
The Project will now contain a Scene asset, Which chould be able to be uploaded directly to unity to view my work
https://miro.com/app/board/uXjVPko_8Po=/ <- The Miro Board is here. 
