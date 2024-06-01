# By A Thread
This game was supposed to be my final project for one of my classes, but due to various circumstances on my end, I couldn't turn it in on time. I used this opportunity to finish the game up the best I could to add it to my portfolio.

# C# File Explanations/Uses
## DialogueManager.cs
The DialogueManager script handles everything regarding dialogue in the game, from cutscenes to actual gameplay. It utilizes Inkle's "Ink Unity Integration" unitypackage, which I highly recommend for anyone trying to implement dialogue into their Unity game, due to the high amount of customization that Ink allows you to have when developing a dialogue system, along with the ease of use once everything is set up. This script handles how dialogue is shown on the screen, which character is talking, and which character's portrait should be shown.
## DialogueTrigger.cs
This script ties directly into the DialogueManager script, as it's the script that triggers the dialogue to happen when the player enters the trigger. This script is one of my favorites because of how customizable I made it. I could make the player stop or keep moving upon entering the trigger, I could make it activate and deactivate GameObjects, and I could make it start automatically after a few seconds if I wanted to, without the need for the player to enter the trigger!
## GrappleGun.cs
This is the script that was the result of my first ideas of By A Thread back in the concept phase. The Grapple Gun is the main mechanic of the game, and without it, the game would be yet another regular 3D platformer. This script handles which surfaces you can grapple onto, how far or near you have to be to grapple, and the rope that you see when grappling to an object. It also makes you able to swing from it.
## PlayerMovement.cs
The PlayerMovement script was probably one of the most complicated movement scripts that I have ever created. While it has all the things you'd expect a movement script to have (WASD movement, jumping, sprinting), I had to account for physics and velocity bugs while using the Grapple Gun, which is why I made it so that at a higher speed, your velocity decelerates faster so you can move in mid-air after swinging. It sounds simple, but then again, everything about a game is simple until you actually try developing it yourself.
