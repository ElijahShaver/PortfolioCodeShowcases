# Trucked Up!
This game is a project that I made for one of my classes at Abilene Christian University. If it isn't obvious enough already, this game was heavily inspired by the game Clustertruck.

# C# File Explanations/Uses
## PlayerMovement.cs
This PlayerMovement script is rather unique, at least to me, since this was the first time I ever added a stomp function along with the rest of the regular movement (moving horizontal/vertically, jumping). I've seen stomp mechanics in games like Sonic or Ultrakill, and I wanted to emulate that since I wanted the player to have a way to quickly get down from the air and onto a truck. I was originally going to add a dash too, but that was scrapped because it was troublesome to add. And for an additional look into the game development process, if you press down H, you will activate a mode that causes you to go insanely fast, and become invincible! This was just a dev tool that I didn't bother removing in all honesty.
## ScoreManager.cs
The ScoreManager script keeps track of the score and the time. You get bonus points for doing certain things like beating the level quickly or getting a lot of airtime. You even get a bonus just for winning the level! I made the game's UI to encourage strategy along with speed, since I'm sure that people are going to try for a fast time while also trying to max out their score.
## TitleTruckSpawnerScript.cs
I had a lot of fun with this script! It allowed me to make the title screen less bland by having a bunch of trucks spinning in the background. So the way that this works is a little complex, but seeing it in action makes things much easier to understand. There are 3 columns at different distances (close, midway, far) with various spawn points in each one. Every second, a random spawn point is chosen from each GameObject, making a truck go from left to right, all while spinning. The reason why it worked so well in the end was because offscreen, I had a large cube that acted as the despawner for the trucks. 
## TruckRotate.cs
This was a really cool script to implement, and I'm still proud of myself for figuring it out. The way that it works is whenever a truck is inside the trigger, it rotates at a specific speed, changing the direction of the truck without any use of animations or playables. Adding this made streamlining the level design process much easier, since I coudl just pop a rotation trigger down, and set it to whatever number it needed. It made for some very fun experiments!
