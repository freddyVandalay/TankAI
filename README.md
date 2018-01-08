studentID: fsund001 	moduleID: IS53049A		date: 08/01/2018

Final Project - Behaviour Trees

Introduction

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





Conclusion
The two main goals for this project was to improve navigation and add another level of unpredictibility to the AI. 