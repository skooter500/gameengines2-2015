# Game Engines 2 2015/2016

## Resources
* [Slack](http://gameengines2015.slack.com)
* [Webcourses](http://dit.ie/webcourses)
* [Download Unity](http://processing.org)
* [Download Visual Studio 2015](http://processing.org/reference/)
* [Games Fleadh](http://www.gamesfleadh.ie/)
* [The Imagine Cup](https://www.imaginecup.com/)
* [The git manual - read the first three chapters](http://git-scm.com/documentation)
* [A video tutorial all about git/github](https://www.youtube.com/watch?v=p_PGUltnB6w)
* [Lecture notes](https://onedrive.live.com/redir?resid=AB603D769EDBF24E!210396&authkey=!AAb-R5vP9R9enWo&ithint=folder%2cpptx)

## Contact the lecturer
* Email: bryan.duggan@dit.ie
* Twitter: [@skooter500](http://twitter.com/skooter500)
* Slack: https://gameengines2015.slack.com

## Assignment marks

- C12487072 70
- C10720209 64
- C12388971 10
- C11426432 67
- C12369056 57
- C12428088 76
- C12474932 67
- C11483622 66
- C10321321 31
- C12449618 63
- C12331591 70
- C12339291 63
- C12391946 25
- C12722625 82

## Assignment
- [Assignment](assignment.md)
- [Assignment submission](https://docs.google.com/forms/d/1UCqdPEwhk3sFUedBJ12cTACrQpmvD3ws9Kg72uM2RL8/viewform)

# Week 5
- Finite state machines
- [A discussion about how best to implement FSM's in Unity3D](http://forum.unity3d.com/threads/implementing-finite-state-machine-ai-c.287401/)
- FSM Libraries for Unity3D
	- [http://www.hutonggames.com/]
	- [https://github.com/dotnet-state-machine/stateless]
	- [https://github.com/MhmmdAb/TinyStateMachine]

## Lab
- Have a read over the FSM lecture if you need to
- Clone the repo and check out scene6
- Have a look at the little fishies and see if you can figure out what their behaviours is. They all have the same behaviour. You might need to watch them for up to 5 minutes to see the full range.
- *Without* looking at the code or any of the objects in the scene, see if you can figure out what the behaviour is and how it's all implemented.
- Now go and check the code and see if you were correct!
- I am not happy with the idle behaviour. See if you can come up with a more fish like one. Have a look on YouTube if you need inspiration.
- See if you can make them fight and kill each other. Try and use states fof this.

# Week 4
- Pursue & offset pursue

## Lab
- Implement offset pursue and set up a fleet

# Week 3
- Crash course in all the steering behaviours
- Arrive, flee, pursue and evade

## Lab
- Clone the repo and study the solution to last week and make sure you know how it works!
- Create a new fish spawner that spawns red fish.
- The red fish should start at a random position in the tank and *arrive* at a random position in the fish tank.
- When a red fish arrives at its arrive target, it should pick a new target to arrive at.
- The green fish should *flee* from the red fish if they come in range.

Here is a video of my attempt to implement this behaviour. As usual, try and use tags, coliders and any other Unity magic you can:

[![YouTube](http://img.youtube.com/vi/bXZBaVNcWPA/0.jpg)](https://www.youtube.com/watch?v=bXZBaVNcWPA)


# Week 2
## Lectures
- Seek and arrive steering behaviours

## Lab

Create this virtual fish tank:

[![YouTube](http://img.youtube.com/vi/Yjm4cLNLNq0/0.jpg)](https://www.youtube.com/watch?v=Yjm4cLNLNq0)

You can start with yesterdays code. I only used ```Seek``` to make the fish move and I didn't modify the Boid class! You can use Colliders, tagging, rigid bodies, spawners, prefabs and co-routines and any other Unity magic you know. What's happening:

- Fish should spawn at random positions inside the cube.
- They should be coloured with a random shade of green.
- They should swim back and forth. There should be some variation in the speed the fish swim at.
- Every two seconds, food should drop into the tank.
- If a fish comes in range of food, it should chase it until the food goes out of range, or it eats it.
- When the food goes out of range or gets eaten, the fish should return to swimming back and forth.

- Solution to the lab is in Scene2

# Week 1

##Lectures
- Check out the first set of slides
- Revision on trigonometry, vectors and quaternions

## Lab

You will recognise this question as being a variation on Question 1 from your Christmas exam. Instead of using the spaceship model from the exam, start with a blank unity project.

- Create a new scene
- Add a cube and set its initial position to be (0, 0, 5). Set the cube colour using a material.
- Add a point light and set its position to be (0, 0, 0) and its distance to be 20
- Change the camera position so that its initial position is (0, 20, 0) and rotate it so that it looks down on the scene. To do this, set its Rotation to be (90, 0, 0)
- Now attach a script to the sphere and call it CircleFollowing. Open this in Visual Studio
- What you need to do in this script is
	- Add fields for radius and the waypointCount
	- Add a field to hold a list of wayponts
	- Set appropriate default values for these
	- In the Start method, use trigonometry to calculate the waypoints
	- Calculate waypoints positioned around the outside of a circle centred at (000) and with a radius of the radius field. The points should be on the X-Z plane. The cube's start position should be the zeroth waypoint.
	- Int update, the cube should seek the next waypoint. When its’ distance is < 1, it should advance to the subsequent waypoint. When it reaches the last waypoint, it should seek the zeroth waypoint again and so on…
- Draw appropriate gizmos

Can you generate the waypoints using quaternions?

Advanced! Read this tutorial on [using Co-routines in Unity](http://docs.unity3d.com/Manual/Coroutines.html). See if you can use a coroutine and a line renderer to have the the cube fire a lazer every 2 seconds

- [Solution to the lab](unity/SimplePathFollowing)