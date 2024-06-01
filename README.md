# ARRO-DODGE!
Unlike my other projects, this game isn't something I did for any of my classes at ACU. It's actually a passion project, and one that I've been working on for the past year and a half!

# C# File Explanations/Uses
## ArroController.cs
This script is responsible for the Arro (player)'s movement. It detects when the player inputs the arrow keys in all 8 cardinal directions, and brings the Arro to one of the dots on the board. It also handles the speed at which the Arro does so as well.
## CamPositionTrigger.cs
This is a script for one of the triggers that you can use in the game's level editor. What this trigger does, upon the level editor's timeline's line hitting it, is that it moves the camera a vertain amount of units for a duration of time. The final location and duration of the location trigger can be set via the properties panel, and saved to and loaded from a .JSON file.
## EditorControls.cs
The EditorControls script handles just about everything regarding the level editor, from the timeline line's speed, to the different modes you can pick (Drag, Properties, Delete), and even the default values of a level! This script is one of the major foundations of the game, and without it, ARRO-DODGE! probably wouldn't be a game. It's just that crucial.
## FileHandler.cs
Yet another crucial script to the game, and the script that I'm the most proud of! FileHandler, well, handles files. It's the way the game is able to save and load .JSON files in a way that makes sharing ARRO-DODGE! levels easy! Whatever triggers are in the timeline, along with their properties, locations, and values, are saved to a .JSON file. The level's default properties are set too. All of these can be flawlessly loaded back in since this script reads the .JSON's data and instantiates a trigger to a specific location on the timeline. It repeats this process for every trigger.
## ObjectSpawnerManager.cs
This script handles the way that the player puts triggers onto the timeline. There is a list of buttons in the upper right quadrant of the screen, and if the player presses one of them, they will be able to place a trigger onto the timeline depending on which button was clicked. This was another crucial script for the level editor, since it's the main way that levels are made!
