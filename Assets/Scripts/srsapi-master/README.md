# srsapi
This respository contains a set of examples on how to interact with the Sim Racing Studio (http://www.simracingstudio.com) API and lowlevel API (lla) using C# or Python 3.

Check out this step by step tutorial to get started with Unity3D and SRS:
https://gitlab.com/simracingstudio/srsapi/-/wikis/Getting-started-with-Unity3D-and-SRS

## What are the requirements to run the example
Sim Racing Studio App >=1.43.2 (Download it from https://www.simracingstudio.com/download)

## What is the difference between the API and Lowlevel API?
The API allows full control of motion, shakers, wind and tachometer, it's very useful for simulation of vehicles, .

The lowlevel api allows the developer control wind sim left and right fans individually from 0 to 100%.

## What the api example does?
It opens a socket and broadcasts the telemetry packets to the simracingstudio app
For example the speed goes from 0 to 200 and from 200 to 0.

## What the lowlevel api (lla) example does?
This example opens a socket and broadcasts the telemetry packets to the simracingstudio app
The fan power goes from 0 to 100 and from 100 to 0, and reverse in each side.

## How to run using C#?
* Using Visual Studio
* Create a new console application
* Replace the default Program.cs by the Program.cs from this repository
* Build and Execute

## How to run using Python:
* Download and install python 3
* Clone this repository
* Open the a command window and point to the location where the repository has been cloned
* Execute the follow command: python main.py

## API Definition
| Field | C Type | Description | Expected Value |
| --- | --- | --- | --- | 
| api_mode | char*3 | API Header packet | constant: api |
| version | uint8 | API version packet | 102 |
| game | char*50 | Game sending telemetry | Example: Project Cars 2 | 
| vehicle_name | char*50 | Car, Boat or aircraft name | Example: Lamborghini Huracan | 
| location | char*50 | Airport code or Track name | Circuit Gilles-Villeneuve |
| speed | float | Wind Speed | 0 to 400 does not matter if km/h, m/s or mph, app is smart to learn automatically | 
| rpm | float | Engine Current RPM | 0 to 32000 |
| max_rpm | float | Engine Max RPM for SLI | 0 to 32000 |
| gear | int8 | Vehicle/Plane Gear | -1 = revere 0=Neutral 1 to 9=gears |
| pitch | float | Pitch | in degrees -180 to +180 |
| roll | float | Roll | in degrees -180 to +180 |
| yaw | float | Yaw | in degrees -180 to +180 |
| lateral_velocity | float | used for traction loss | in float between -10 to 10 |
| lateral_acceleration | float | lateral gforce (sway)  | float values beteween -100 to 10 |
| vertical_acceleration | float | vertical gforce (heave) | float values beteween -10 to 10 |
| longitudinal_acceleration | float | longitudinal gforce (surge) | float values beteween -10 to 10 |
| suspension_travel_front_left | float | Suspension Travel FL | in float values -10 to 10 |
| suspension_travel_front_right | float | Suspension Travel FR | in float values -10 to 10 |
| suspension_travel_rear_left | float | Suspension Travel RL | in float values -10 to 10 |
| suspension_travel_rear_right | float | Suspension Travel RR | in float values -10 to 10 |
| wheel_terrain_front_left | uint8 | Wheel Terrain Contact Type FL |  0=all others, 1=rumble strip, 2=asphalt |
| wheel_terrain_front_right | uint8 | Wheel Terrain Contact Type FR |  0=all others, 1=rumble strip, 2=asphalt |
| wheel_terrain_rear_left | uint8 | Wheel Terrain Contact Type RL |  0=all others, 1=rumble strip, 2=asphalt |
| wheel_terrain_rear_right | uint8 | Wheel Terrain Contact Type RR |  0=all others, 1=rumble strip, 2=asphalt |



## Lowlevel API Definition
| Field | C Type | Description | Expected Value |
| --- | --- | --- | --- | 
| api_mode | char*3 | API Header packet | constant: lla | 
| version | uint8 | API version packet | 101 |
| left_fan_power | float | Power in % of left fan | 0 to 100 | 
| right_fan_power | float | Power in % of right fan | 0 to 100 |
| rpm | float | Engine Current RPM | 0 to 32000 |
| max_rpm | float | Engine Max RPM for SLI | 0 to 32000 |
| gear | int8 | Vehicle/Plane Gear | -1 = revere 0=Neutral 1 to 9=gears |

