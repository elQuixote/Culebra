![IMAGE](images/Culebra_2.0_B.jpg)

#### CULEBRA.NET LIBRARY FOR RHINO/GH INTRODUCTION ####
Culebra.NET is a 2D|3D Multi Object Behavior library written in C# ([Wrapper around Culebra Java library](https://github.com/elQuixote/Culebra_Java)) focused on hybrid system interactions with custom Visualization, Data, and performance features. It contains a collection of objects and behaviors for creating dynamic multi agent interactions. For more information see the [Culebra Java Library](https://github.com/elQuixote/Culebra_Java).

#### WAYS TO USE CULEBRA ####
* Visual Studio – Use the Culebra Objects and Controller through the use of the CulebraData.dll (Recommended)
* Visual Studio – Create your own objects and simply implement behavior classes individually. 
* Visual Studio – Create your own objects and inherit from Culebra Objects. This will provide access to the controller and all classes, through the use of the CulebraData.dll
* Grasshopper Scripting Components in C#,Python – Unavailable in Beta Release, currently some issues with IKVM.JDK.Core.dll in GH scripting environment

#### CULEBRA 2.0 BETA GHA (GRASSHOPPER PLUGIN) ####
The Culebra grasshopper plugin was rewritten to implement a slew of new behaviors through the use CulebraData.dll which is a wrapper around the Culebra Java Library.
*Version 2.0 Beta updates include
*Wandering Behaviors
*Path Following Behaviors
*Multi.Objects Interactions
*Mesh Crawling Behaviors
*Stigmergy Behaviors
*Mesh Color Behavior Influence
*Graphics/Geometry Modes
*Visualization Features
*Single & Multi.Object Engines
*Zombie Engine
*Behavior Chaining with Controller

--------------------------------------------------------------

#### BEHAVIORS ####
##### There are 7 Major Types of Behaviors which can be hybridized anyway your heart desires. #####
* Flocking Behavior - Flockers Behavior Type Class Implements Flock Behavior Interface
* Wandering Behavior - Wanderer Behavior Type Class Implements Wander Behavior Interface, the base Wander behavior is from Craig Reynolds and Daniel Shiffman.
* Noise Behavior - Improved Perlin Noise Behavior Type Class Implements Noise Behavior Interface. 
* Tracking Behavior - Path, Shape and multiShape Tracker Behavior Type Class Implements Tracking Behavior Interface. 
* BabyMakers Behavior - Objects can spawn children under the right conditions. Objects can have type specific behaviors. For example, if you are a "Parent" object or object of type Creeper or Seeker or you derrive from these classes then you can create children, however if you are of type BabyX then you no longer have the capability to spawn children.
* Mesh Crawler - Mesh Crawler Behavior Class. 
* Forces – Attract, Repel and other force methods inside the controller class.
##### The Controller #####
* Controller Class - this is the class which acts as controller for all behaviors classes. This class also builds on behaviors, using image drivers and other features which are not in the stock behavior classes. See Java Class Diagram for more details.

#### RELEASES ####

##### Culebra 2.0 Beta – Requires Rhino 5.0 & Grasshopper Version 0.9.0076 #####
* View the [Installation Notes](http://culebra.technology/Culebra_2.0_InstallationNotes.pdf) and [Culebra 2.0 Beta GH User Guide](http://culebra.technology/Culebra_2.0_UserGuide.pdf)
* [Download Culebra.NET Library Documentation](http://www.food4rhino.com/app/culebra) 
* [Download Culebra 2.0 GH Demo Files](http://www.food4rhino.com/app/culebra) 
* [Download Culebra 2.0 BETA](http://www.food4rhino.com/app/culebra) 

##### For more specific details see the Class Diagram Below ####

#### CLASS DIAGRAMS ####
[![IMAGE](images/Culebra_ClassDiagram_Small.jpg)](http://www.culebra.technology/culebra-1/Culebra_ClassDiagram.jpg)

##### CREDITS #####

* Thanks to Craig Reynolds for all of his work on boid behavior
* Big thanks to Daniel Shiffman, his work has been very inspirational and referenced in this library.
