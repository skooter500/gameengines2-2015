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

# Assignment
- [Assignment](assignment.md)

# Week 1

##Lectures
- Check out the first set of slides
- Revision on trigonometry, vectors and quaternions 

## Lab

You will recognise this question as being a variation on Question 1 from your Christmas exam. Instead of using the spaceship model from the exam, start with a blank unity project. 

- Create a new scene
- Add a Sphere and set its initial position to be (0, 0, 5)
- Add a point light and set its position to be (0, 0, 0) and its distance to be 20
- Change the camera position so that its initial position is (0, 20, 0) and rotate it so that it looks down on the scene. To do this, set its Rotation to be (90, 0, 0)
- Now attach a script to the sphere and call it CircleFollowing. Open this in Visual Studio
- What you need to do in this script is 
	- Add fields for radius and the waypointCount
	- Add a field to hold a list of wayponts
	- Set appropriate default values for these  
	- In the Start method, use trigonometry to calculate the waypoints
	- Calculate waypoints positioned around the outside of a circle centred at (000) and with a radius of 10. The points should be on the X-Z plane. The sphere's start position should be the zeroth waypoint. 
	- Int Update, the sphere should seek the next waypoint. When its’ distance is < 1, it should advance to the subsequent waypoint. When it reaches the last waypoint, it should seek the zeroth waypoint again and so on… 
- Draw appropriate gizmos