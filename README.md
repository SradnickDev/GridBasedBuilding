# GridBasedBuilding
### Fortnite Building Remake

This  should be a remake of the Fortnite Building System.
This Project is discontinued.


Short Introduction :

- it is still a Prototype
- alot of the Code is not commented (trying to do this)

- ok, like the name said, it is based on a 3D Grid
- each Grid Node stores some information
   - like Neighbors which I split into __Static__ and __Dynamic__ Neighbors
     - __Static__ Neighbors is the Neighbor in World Position so it is always the same
     - __Dynamic__ nighbour is based on the rotation/direction of the custom Gizmo
- simple Terrain/Ground Collision Check


I did not find a good way to solve the tons of conditions which are needed for the Wall, Ramp, Floor Placement.
Also for the chain destruction I considered saving all required Neighbors which was the wrong way of doing it.

Take a look in the `GridNode.cs` for more Information :D have fun to see some horror code

About the GridNode Recognition; it uses cardinal points mixing Y and Z Axis Rotation.

Have Fun =D

https://youtu.be/KEdmQQ63Uto
