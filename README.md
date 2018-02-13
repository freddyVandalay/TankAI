
This demo demostrates implementations of behaviour trees using NPBehave for the tank AI in the Tanks demo tutorial. 
Code from Coursework 1 on Behaviour Trees has been re-used some with modifications. Any re-used code have
have been marked out in the source code script 'Behaviour.cs'. 

The introduction of a sophisticated sensor system for the NPC allowed for a more sensitive navigation within the trivial world compared
to the previous implementation for coursework 1. 

The NPC can addapt two types of behvaiours; "Lost Behaviour" and "Attack on sight behaviour". 


Behaviour 1: "Lost Behaviour" (The number represents NPC behaviour in the Unity Game Manager settings)
	This behaviour can best be described as tank that aimlessly travel within the world
	while avoiding obsticles, almost like a wandering state. The behaviour is determind only by one tree which is seen as the navigation
	tree and only have one behaviour if you like. The NPC is non-lethal in the sense that it can not aknowledge the enemy and will never attack.

Behaviour 2: "Attack on sight" 	
	Unlike behaviour one, " attack on sight" has what can be considered as two states. One wander state and one attack state. The NPC is allowed to wander
	until an enemy is in sigtht, which will promptthe NPC to attack its enemy. 
	
Methodology

Navigation: To be a to create some form of awareness of it the world around the NPC, the implementation of "sensors" or more specificly 6 vectors are being used.
One straight ahead and two vectors 45 degress one each side of the middle vector. And the same concept is applied back point in the opposite direction.
(The sensors pointing towards the back are not used in this implementation). These vectors ar used to send out ray casts to detect colossions with objects
within the world. Each vectors collision status is stored with a boolean value on the blackboard. Depending on the status pattern of the vectors combined 
new blackboard values can be created. 
	Ex 1. If vector pointing straight head and the vector pointing forward 45 degrees to the right are both detecting collision, 
	a sharp turn to the left will be executed. 
	Ex 2. If only the vector pointing forward 45 degrees to the right is detecting, only a slight turn the left will be executed to avoid the obsticle.


Unpredictability: To give the NPC a unpredictble element, a  combination of Selectors and the NPBehave.Random have been the choosen method. Using a 
random selector is a popular method to use to create this kind of behaviour but were no found in the NPBehave library and therefore this alternative
method were used instead to recreate a similar behaviour. 

To give the NPC a slight realistic feel, travelling speed is on some occations slowed down before avoidig an obsticle. Or sometimes if the NPC reaches
a dead end it sometimes reverse and turn while sometimes it stops and turns. These are a implementation design to make the NPC look clever and more realistic. 
This behaviour is acheived by assigning a probability value for a node to be executed using NPBehave.Random. 


Video tech demo:
https://youtu.be/V2X4bdxGEeM


