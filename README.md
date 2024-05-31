# Pickup 'N' Packup!
This is a game that I made for a VR development class using Steam's SteamVR Unity plugin, and I think it turned out quite well for my first proper VR game! It was one of my first ever experiences with VR development, and the first ever game that I have published to the Steam platform.

# C# File Explanations/Uses
## BinScript.cs
BinScript is the script that I use to handle all of the bins in the game that require you to place specific objects inside of them. It handles whether or not a bin is filled properly, how many items should go inside of the bin, and which kinds of objects count as correct objects to be placed into it.
## CompletionManager.cs
CompletionManager is how the game determines whether or not the player has won, whether or not the game has started or ended, and keeps track of time. It allows me to set the time limit, how many bins should be completed until the game registers the level as completed, and when to load the player into the next level, or reload the current level if the player loses.
## CheckScore.cs
CheckScore is a relatively simple script, but it's also a very useful one too. This script is responsible for allowing the player to check the status of the level; it checks how many bins they've completed, as well as how much time they have left to complete the level. All of this information is given at the hold of a button, and I believe that the addition of this script greatly increased the quality-of-life for the game.
