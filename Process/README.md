# Process Journal

## (1/23/2025)<br> Tiny project: ***Dance Battle!*** 
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
