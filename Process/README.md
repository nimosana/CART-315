# Process Journal

## Week 1 (1/23/2025)<br> Tiny project: ***Dance Battle!*** 
Before I began my project, I looked at the suggested engines and immediately discarded Twine and Ink, as I did not want my game to be text-focused. After checking out Bitsy and Pico-8, I opted for Pico-8 mainly because it allowed me to write my own code. I felt like Bitsy’s abstractions seemed limiting, and I was not interested in its tile map mechanic and movement.

Before starting the process of creating my game, I decided to watch Pico-8 YouTube tutorials to understand how the game engine works. While watching, I understood the engine’s programming language, learnt about the engine’s Sprite, Map and Sound features, but also about apparent limitations I would need to consider, I also found a “cheat sheet” grouping and explaining most functions I could need.

I began by following a tutorial for basic object(sprite) movement and display, I began with a static sprite that I could move around with accelerations, at first I was getting frustrated with the rudimental coding experience, unclear error codes, and lack of features compared to an IDE like Visual Studio Code.

As I was getting angrier at the coding experience, I decided to go to the sprite editor and draw moving variants for the little character I had created. As someone who isn’t comfortable at drawing, Pico-8’s low-pixel nature and ease of use for asset creation made it very relaxing and satisfactory for me!

My sprite animation made my character seem like it was dancing, giving me the idea to make a game where the player’s main goal is to do different dances. I believed this concept was funny and had potential, so now I was mainly focused on figuring out how to achieve my goal while being able to meet the deadline.

I started by making my five simple two-sprite dancing animations and programming them with movement depending on the key pressed. Because I really wanted to make it a 2-player game, I thought to make an array of dancing bots which would randomly switch dances, my idea being to make the players try to follow the rhythm and moves imposed. As working on the Pico-8 engine felt slow and rudimentary, I figured adding all the functionalities I wanted was going to be time consuming. Therefore, I decided to clean up/reorganize my code into functions to be able to work more efficiently as I continued the project.

I then focused on creating the bots and have the random dance mechanic, which I then complexified, and prepared variables to be able to track dance states and other info I would need to build my scoring system. I then coded basic scoring whenever the player’s movements matched the bots, and added a movement indicator and bar, as guessing the bots’ move changes in time was very difficult. 

I rapidly made a map background and added the camera to make the game more aesthetically pleasing, once I was done with the core functionalities, I implemented the second player, which made the game mostly complete. At this point, I felt I had invested more time than I should’ve, but I also wanted to complete the project and knew most of the job was done, so I did a final sprint to implement the game’s beginning and end states. I also added some instructions after I had a classmate try the game, and rapidly realized there were otherwise no instructions on how to play.

The playtest showed me that the instructions about holding the keys and how to play may still be unclear. For the inputs, I also wish I would've had time to make P1's inputs WASD instead of ESDF.

Link to the game: [Dance Battle!](https://nimosana.itch.io/dance-battle)

## Week 2 (1/30/2025)
This week, my exploration initially consisted of making the “dropper” continuously spawn falling circles which get deleted if the user does not catch them, or if they go out of bounds however this is unlikely to remain, as I rapidly changed paths. Towards the end, I focused on trying different ways of programming movement for last week’s square, I really wanted to cut the tie between framerate and movement speed. For this, I explored the Time.deltaTime value to create more predictable results independently of the machine, however I the confusing issue that Unity would override the acceleration values I set in the C# code.

I tried implementing the rigidbody2D Forces to do movement but reverted back to accel * deltaTime. I spent a lot of time thinking about game ideas and I believe I want to make a bird’s eye view shooter. For this reason, I started working on how to make controls which take into account combinations of keypresses and rotate differently depending on the combination, but ran out of time researching how to find the most optimal way to do this (without making a 10+) if/else chain.

## Week 3 (2/6/2025)
For my personal take on Pawng, I wanted to experiment with velocity and forces, I wanted the users to feel a sense of physics realism, such as the ball and paddles having weight. To achieve this, I began my experimentation by the paddles, creating speed variables and modifying the x positions based on the speed. I also wanted the paddles to have rotation, so I also implemented rotation speed. The result gave an illusion of physics-based speed, however I quickly realized that the forces would not transfer to the ball and was unsatisfied.

To successfully achieve my vision, I replaced the manual position and rotation updates by using the Rigidbody2D AddForce and AddTorque methods. While simultaneously achieving my goal of transferring the paddle’s energy to the ball, it allowed the paddle to be affected by the weight of the ball, I really liked this result as I felt it added learning and mastery curves to an otherwise simple game. I also used Time.deltaTime to have a consistent experience

To make the game more polished, I rapidly created a screen shake effect whenever the ball collides, photoshopped a pivot point to the paddles to highlight the relevancy of rotations, and added a red/blue theme for each player. To make the game more predictable, I added linear and rotational speed dampening, and small rotational forces if the paddle isn’t vertical, immobilizing and/or straightening the paddle whenever the player isn’t touching the relevant keys. I also changed the code so the ball goes to the loser, and resets itself if it manages to go out of bounds.

![pawng](https://github.com/user-attachments/assets/88b547ce-61df-4a66-af15-bbb28e9586d2)

![pawng2](https://github.com/user-attachments/assets/003a4885-e664-4e9c-9765-490121abb9d2)

## Week 4 (2/12/2025)

This week, I wanted to continue exploring the idea of integrating physics similarly to last week’s pong game. To achieve this, I began by freezing the paddle’s Y-axis movement and affecting it through forces instead of modifying xPos. I added rotational forces (locked to 45deg) tilting the paddle as the user moves to complexify gameplay. I also changed the Paddle’s code to be ran in FixedUpdate for more consistent calculations.

I observed that the addition of forces increased the difficulty, and allowed for a learning/mastery curve. Players can easily give the ball too much speed, to their disadvantage, while also being able to exploit the rotational nature of the paddle to dampen the speed of the ball and survive in the long game.

To further develop the flow of the game, clearing the bricks now refills them and increases the level, which progressively make the ball bouncier. To make the game “survival” based, the limited lives remain, with the objective of having the player fighting to keep the ball in control as the game becomes harder. To incentivize players to keep the ball going at a decent speed and discourage excessive speed dampening, players get more points in function of the ball’s speed magnitude when hitting the bricks.

To make the game seem more polished, I reused my paddle sprite to highlight the rotation mechanic, used the same color palette as my pong game, added a text to display the level, and made both labels (points and levels) different colors. I added the screen shake effect on collisions and also made the points variable static, so that I could call it in the Game Over scene and have it displayed when the player loses.

![breakout1](https://github.com/user-attachments/assets/822783c9-e39c-49f5-a55f-15cad8f6d2c7)

![breakouttt-ezgif com-optimize](https://github.com/user-attachments/assets/1a5f7aba-ec0e-4318-9a5a-a21ea1171030)

## Week 5 (2/20/23025)

This week, as I initially didn’t have much inspiration for what to add to my breakout game this week, I began my exploration in a technical manner by looking for ways to reorganize and optimize the existing code, moving general code and functions into my GameManagement script, such as the camera shake, level update, etc.
While reworking the Brick Layer code, I got inspired to add hitpoints to the bricks in function of the level, making the mechanic of levels feel more noticeable/meaningful. To make this hitpoint mechanic more obvious and satisfying, I explored how to reduce the opacity of each brick corresponding to its life percentage, this was done changing its RGB color to an HSV value, modifying its S & V values and converting it back to RGB to be passed to the Renderer. 

As levels are continuously longer and because the player lives mechanic was unforgiving, passing a level now restores an extra life to the player (if they have less than 3), Lives are now displayed at the bottom left of the screen to showcase this.

To reintegrate the blips while avoiding redundant code, it is now managed by a GameManagement function which receives a tone as a parameter, the tone of the blip is different according to what the ball is hitting. Finally, to add a new element to the learning/mastery curve, pressing the spacebar now cancels rotation, allowing the user to prepare or keep its paddle at desired angles, giving them a new action to use or consider.

![breakoutpt3](https://github.com/user-attachments/assets/e0664811-cc8d-4a4d-b227-9352e07a407b)

## Week 6 (3/6/2025)

My exploration during these past weeks was mostly introspective, thinking about which type of game I want to create, combined with alot of research on game design. I started reading "Elements of Game design" by Robert Zubek, and watched videos from Game Maker's toolkit. I've also been searching for teammates, which is why I haven't defined my game idea yet, although I personally steer towards a survival type game.


## Week 7 (3/13/2025)

This week, after long days of blockage finding the idea of the game I wanted to make, I started making simple character movement with wall collisions. The player is a 3d object that rotates towards the user's mouse position, and it currently moves through keypresses.

I wanted to make something multiplayer, so I followed a tutorial for multiplayer functionality. I ended up with 2 projects, my movement prototype, and the multiplayer one, which was a mistake. The multiplayer example allows to move around and shoot projectiles, but it runs in v2021, when I try to update it to the lastest version, I have a problem: Lobby and Relay are deprecated and must be replaced for a newer version. I’ve ran out of time debugging so I will have to figure out what to do next.

![image](https://github.com/user-attachments/assets/ebc41a27-0e4b-49dd-bd70-facf2682d4c1)
![image](https://github.com/user-attachments/assets/26fcd5a2-396f-48df-a5c0-6504734586ec)

## Week 8 (3/20/2025)

After finding the idea of the game I wanted to make, a survival game based around drone warfare where the user must defend their base and use drones to defend against the enemy ones, I got a drone model, programmed its movement with forces to simulate hovering, its rotation following the mouse. The drone tilts depending on its acceleration and speed to give a more realistic visual, the player can click to shoot bullets, dropping shell casings.

I also added an enemy variant of the drone which also moves and shoots but targeting the player, drones can friendly fire and fall into pieces when destroyed. I added an UI to display drone health, ammo, and information about waves such as level, and time/enemies remaining. The Game manager singleton instances the game objects and handles wave generation.

The idea for this iteration was to focus on the core elements of my game idea, creating the base of the game so that I can work and adapt it to continue working towards the larger idea of the game

![dronetime1](https://github.com/user-attachments/assets/0da9a995-519d-4e3e-88f8-965b98a586f8)
![Sequence01_1-ezgif com-optimize](https://github.com/user-attachments/assets/b7f98a56-c159-467b-ac7e-f399a760d3dd)

## Week 9 (3/26/2025)

This week, I began by transferring the game from Unity’s URP to HDRP for better visuals, this resulted in all assets, lighting and shadows looking much better, this was important to me as I wanted the player’s experience to be more immersive. I believe the transfer to HDRP paid off, as after adding trees and the player’s base, I am quite satisfied with how the game looks, it also pushed me to revise and optimize the code to ensure the game’s code is efficient and runs well. (the trees and textures are external assets found online)

After finding an asset pack of explosion VFX (found online) and adding them to the drone destruction scripts, I had to reorganize most of the code structure so the gameManager singleton instantiates the player, UI and enemies. The change was needed for the wave mechanic, and the option for the player to Kamikaze, which implied destroying the original player gameobject and reassigning a new one for the scripts of the camera, enemy AI and UI. This was something I should’ve but hadn’t foreseen but it also allows for more flexibility in making the game more complex.

Another unexpected problem that arose upon adding the player’s homebase was that it was really easy to lose track of its position, this pushed me to delay my original plans, opting to develop the now functioning minimap. This addition allows players to keep track of the target they’re defending, while simultaneously making it easier to find the enemy locations, I also made other small updates to the UI and added the base’s health bar, but it has yet to become functional. the UI is now set to screen space – camera to add the shaky effect, which I feel adds to the idea that it is a remote controlled drone,

but I ran out of time to implement the audio as I had planned to, for the next iteration I plan to reimplement the enemy’s targeting of the player, which is temporarily disabled as their main target is now the base.

https://youtu.be/XdEEljr3b-0
The game now has too much detail to make the 10 second gifs under 10MB
