# Thawed In Frost - Part One
This is a video game I worked on for one of my classes at ACU, and it's a personal favorite as well. The entire development process was a very significant learning experience for me when it came to what goes on in game development, as well as a steep upwards curve in the quality of my products.

# C# File Explanations/Uses
## NPCController.cs
The NPCController script is responsible for how NPCs follow the player character on the screen. I originally wanted it to behave similar to Deltarune and its RPG parties, but that proved to be too difficult and time-consuming at the time, so I just made the NPC follow you from a set distance away from you. It's a little janky, but it gets the job done!
## NumberCode.cs, NumpadControls.cs
I decided to group these two together since they serve such a similar purpose and I believe that I could have easily made them into one script today. Regardless, this script was fun to implement! This was for a puzzle at the beginning of the game, where you have to input a code to proceed. It was a one-time thing, but I still wanted to show it off since it was my first time making a simple puzzle in Unity.
## ShowCutscene.cs
This script was only used once properly in the final launch, but a version of it is still in the game in the prologue, even though you can't see the photos changing since I didn't have time to add more illustrations. A lot of areas in this script could be made much better today, but I still added this script here because it was my first time creating code for cutscenes for a game. This script handles cutscenes in the form of images, similar to a slideshow. If you press Z or the Enter key, it advances to the next slide.
