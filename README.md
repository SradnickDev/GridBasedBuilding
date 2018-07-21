# GridBasedBuilding
Fortnite Building Remake


This  should be a remake of the Fortnite Building System.
It is still Work In Progress but Im not sure if i ever have the Time to finish it.

I'll try to add/change things over Time.



Short Introduction : 

- it is still a Prototype
- alot of the Code isnt commented(tryin to do this)

- ok, like the name said based on a  3D Grid
- each Grid Node stores some Information
- like Nighbours about wich i split in static and dynamic Nighbours
- static nighbour is the nighbour in world Position so it is always the same
- dynamic nighbour is based on the rotation/direction of the custom Gizmo

- simple Terrain/Ground Collision Check


I didnt find a good way to solve the tons of conditions wich are needed for the Wall,Ramp,Floor Placement.
Also for the chain destruction i thought of saving all required Nighbours wich was the wrong way of doing it.

Take a look in the GridNode.cs for more Information :D have fun to see some horror code

About the GridNode Recognition , it uses cardinal points mixing Y and Z Axis Rotation.

Have Fun =D

https://youtu.be/KEdmQQ63Uto

[![Fortnite Building Remake in Unity](https://img.youtube.com/vi/KEdmQQ63Uto/0.jpg)](https://www.youtube.com/watch?v=KEdmQQ63Uto "Fortnite Building Remake in Unity"

