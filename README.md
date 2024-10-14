# Graduation-Project
 
## Mystery Dungeon Roguelike

### 1. Introduction
The Mystery Dungeon genre of games is loved by me and many others but rarely seen in any new games today. Why is not clear but because of this it was decided that the aim for this project was to develop a Mystery Dungeon like game inspired by modern roguelikes such as The Binding of Isaac and Hades in 10 weeks. Mystery Dungeon is a series of games developed by Spike Chunsoft consisting mostly of spin offs where the most popular and most peoples entry into the series being the Pokémon Mystery Dungeon games. The games are turn based where you walk around on randomly generated floors of dungeons killing enemies and collecting items resulting in your character getting stronger. The goal of each floor is to find the exit to the next floor. If the player is defeated they would have to start over on the first floor again, letting them try again to beat the dungeon. 

The goals of the project are:
- Create a fully playable game with a game loop. I often stop working on projects when i am finished with all the gameplay mechanics, never actually making them into actual games
- Making the game easily expandable if I want to keep working on it.
- Learn A*. I know the basics but have never implemented it in a game.
- Add all planned features and finish this report in 10 weeks.

### 2. Process
#### 2.1 Initial Setup
Before any actual programming Unity was chosen as the engine and a design document was created. Since there were no Game Designers involved in the project the document was a bit barebones but still contained a rough outline of what the game was supposed to contain.

#### 2.2 Map Generation
One of the most important parts of any roguelike is a procedurally generated map. This was implemented using a Simple Room Placement algorithm. The algorithm works by trying to place rooms randomly on a map. First create a randomly sized room and place it at a random position on the map. If the room overlaps with an already placed room, move the newly created room to a new position. This is repeated until the max number of rooms are placed. After all rooms are placed a random room is selected as the exit room and corridors are created between all the rooms. In order for the map to feel more like a labyrinth than a straight line to the finish, rooms will try to connect corridors to the closest room regardless of if it is connected to another already.  Iterate through all rooms and create a corridor to the closest room that is not already connected to the current room. To create a corridor, first choose randomly if the corridor will start horizontally or vertically from the room, then from the center of that room remove solid tiles in the chosen direction until the center Y or X position of the other room is reached. Then move on the opposite axis until the center of the other room is reached. This however caused a problem where sometimes three or more rooms could connect only to each other making them unreachable. To fix this some sort of pathfinding was needed to check if every room could reach both the start and exit room.

#### 2.3 A* Pathfinding
A* or A-Star is an algorithm used in many games for pathfinding and map generation. It keeps track of the distance moved from the start node for each node and uses a heuristic function to determine the distance to the end node. By moving in the direction of the unexplored node that has the lowest combined distance from the start and to the end you get a relatively fast and accurate pathfinder. The pathfinder gets more accurate the more accurate heuristic distance function you use but since for map generation the fastest path is not really needed the most simple distance function was used; the delta x and delta y of the start and end node combined.

With the map generation problem solved next up was creating the map in the game. By using the Unity grid component and tiles this was easily solved. An exit was placed in a random spot in the exit room. See figure below.

![image](https://github.com/user-attachments/assets/ab8e4539-3bb0-4caf-a863-6536fb7dbcce)

#### 2.4 Game Manager
The game manager keeps track of which character’s turn it is. When the game is started it will create a new map and start the player’s turn. It contains a Level object which contains everything related to the map. The Level keeps track of all units and other objects that are on the map. It also has several methods that give information about the current state of the Level.

#### 2.5 Character Controller and Unit class
The Character Controller handles input and AI and has a reference to a Unit that it controls. When the Unit has completed its turn the controller will invoke an event, telling the Game Manager that the turn is over and the next character's turn will start. The Unit is the actual character that moves around. The Unit class contains Health, Stats and Components. By using the Component design pattern the code becomes decoupled and more modular. This will allow different functionality for different units without the need for any changes to the Unit class. For example melee and ranged attack.

#### 2.6 Movement Component and Attack Component
Next up is the Movement Component for the units. The Movement Component should handle moving the character the fastest path to the desired position. We can do this by utilizing the pathfinder used for map generation. Since the game is turn based the characters should only be able to move one tile at a time. The movement is created by lerping between the original and target position. When the character has moved an event is invoked to signal that the movement has been completed. This however resulted in the game feeling slow and sluggish since every character has to wait for all other characters to finish their movement before it can move again. This was fixed by ending the turn the moment the character starts moving.

The pathfinder has no way of knowing if there are other characters in the way, this was fixed by creating a method in the Level class that copies the current map and adds walls where the other characters are and using that map for pathfinding instead.

Currently all the logic for the movement is in the Update method of the component. This was changed to a Coroutine to increase readability and allow easier changes to the code. 

The Attack Component handles the normal attack the characters can perform. There are two types of attack; Melee and Ranged. When the unit has a melee attack component and an attack is performed a Coroutine is started that moves the Unit sprite towards the target, deals damage then moves back to the original position. The ranged component creates a projectile that moves toward the target that deals damage when it reaches its destination. 

The player can store a move or attack command, automatically moving towards the target position or enemy as soon as the turn starts. This was added because most turn based games can be quite slow with a lot of downtime between actions. All animations in the game are also very fast to increase the pace of the game. The result was that the game felt more fluid and fast paced which was the goal. 

#### 2.7 IAction Interface
To easily be able to implement more actions that the unit can perform an interface was added that the components can implement. The interface has a StartAction method that will start the action.
It also contains an OnTurnOver event and an ActionStarted boolean. The event is invoked when the turn should end and the Boolean set to true when the action is finished. The Boolean is needed because the Movement Component works differently than all the other actions since it ends the turn instantly but shouldn't be able to be started again until the previous movement action is finished.

#### 2.8 Upgrades, Experience and leveling
When killing an enemy the player should be awarded experience and when enough experience has been collected they should level up and be rewarded an upgrade, buffing the character in some way. Two basic upgrade types were added: Multiplicative Stat Upgrade and Flat Stat Upgrade. These were created as Scriptable Objects at first making it easy to create new upgrades in the editor. This would however also add bloat to the menu in the editor since every type of upgrade would need its own menu item, so they were changed to regular classes. Since there are no designers working on this project creating upgrades purely in code was a viable option. The units have multiple events on them that are invoked when performing different actions such as moving, attacking, taking damage etc. This allows upgrades to both increase stats and add effects that apply when these actions are performed.

The enemies are supposed to keep on spawning so as to not let the player farm them for experience forever to get an infinite amount of upgrades, experience and leveling were removed and upgrades are awarded after a level has been cleared instead.

#### 2.9 Items and inventory
	
The items were Scriptable Objects but for the same reasons as the upgrades they were changed to be normal classes instead. The items implement the IAction interface and have an Icon, Name and Description. An inventory component was created that is attached to the units. When the player presses the inventory key the inventory ui will open. The player can see all items they currently have and can then pick an item and select a target for it. This will consume the item and perform its action, ending the player's turn. Enemies have space for one item in their inventory and will drop it on the ground on death. They have a small chance to spawn with a random item and can pick up dropped items from the floor.

#### 2.10 Asset Loader
Since all the items and upgrades need sprites and other assets assigned to them but are normal classes so they cant be assigned in the editor, an AssetLoader class was created. This class loads Asset Bundles and stores them in a static dictionary where the name is the key. The asset loader can then load any asset from a bundle. Now the object only needs the name of the bundle and the name of the asset and they can be loaded cheap from anywhere.

#### 2.11 Targeting
To better visualize how the player will move and interact with items a targeting component was created for the player controller.  Every action has a Targeting Type that dictates how the targeting component will work when selecting a target. This component basically works like a Finite State Machine. When the player starts targeting with an action, the targeting component will store the action’s targeting type. The targeting type contains Start, Update and Exit methods that the component will call when starting, during and when exiting targeting respectively. A reticle will show on the cursor that changes based on the targeting type and a line will show the path the player will take when trying to move towards the cursor. See figure to the below.

![image](https://github.com/user-attachments/assets/b6be9bb4-0c97-4d05-a043-204e64fe1c7c)

 For example:
- Unit Targeting; The reticle will only show up if hovering a unit.
- Area Targeting; The reticle will grow in size, indicating the area that the action will affect.

At fírst there was only a single reticle that could be used which limited what could be done with the targeting. The component kept a reference to a reticle game object that the targeting types could access through the component. Because of the addition of the asset loader this was changed so that by using the Object Pool design pattern the base targeting type keeps a static pool of reticles that the targeting types can use at any time, resulting in an increased amount of possible targeting types. For example the Line Targeting, which places reticles in a line from the player in the target direction. See figure below.

![image](https://github.com/user-attachments/assets/22a04144-901f-423d-9e9f-eee23fcadcf5)

#### 2.12 Special Attacks, Buffs and Debuffs
Special attacks are attacks that have more powerful effects than the normal attack. They implement the IAction interface and work a lot like the items  After using them they require a couple of turns to recharge before they can be used again instead of being consumed. The units have a Special Attack Component that can hold up to four special attacks. At the moment only the player can use special attacks. When a special attack is selected the targeting component will start targeting with the targeting type of the attack. Some special attacks can cause buffs and debuffs such as poison or damage increase. The events that the units invoke during different actions allow buffs and debuffs to trigger their effects at any time. To make the Special Attacks feel like they actually have a cooldown, their cooldowns will not tick down when moving. 

#### 2.13 Tooltips
On the UI you can see the Special Attack icons but not what the Special Attacks do. By adding tooltips that will show up when hovered their descriptions are shown without bloating the ui with text. This was achieved by implementing the IPointerEnterHandler and IPointerExitHandler on the special attack ui slots and giving them a child tooltip gameobject that will only be visible when the slot is hovered.

#### 2.14 Fog of War
Fog of war covers the level preventing the player from seeing unexplored areas. This was achieved by using a flood fill algorithm to remove the fog around the player. The fog has 3 different states: Full fog, half fog and no fog. Any area that has been seen by the player will be half fog, revealing only the layout of the map. In the area around the player the half fog will become no fog, showing everything. Any unexplored areas will be full of fog, hiding everything. If an action is performed when the unit is hidden, animations will be skipped.

#### 2.15 Interactables
Interactables are objects on the level that can be interacted with. So far the only interactables are the exit and dropped items. If the unit that moves over it can interact with it its effect will be triggered. This allows for easy expansion with for example stage hazards that apply negative effects on the unit that steps on them. The exit will open a menu that allows the player to choose a random item, special attack or upgrade to obtain. When a reward has been picked a new level will be loaded. The dropped items add the item to the unit's inventory. If the inventory is full the item will stay on the ground. 

#### 2.16. Character Select
When pressing play on the main menu a character selection screen will open. There the player will be given the choice of all the currently available characters. The characters are objects containing information about the characters. They contain items and Special Attacks that the player will start with if they pick the character. They also contain a prefab for the unit. These characters are stored in an array and when they are picked their index will be saved to the player prefs. When the game is loaded the player will be initialized from the character that was picked.

### 3. Conclusion
Most of the goals were reached. The game is fully playable with some features missing due to lack of motivation, bugs, performance issues and sickness. The bugs and performance issues were fixed in the end and the game is playable. Sounds, graphics and animations are mostly missing due to lack of both designers and artists on the project.

The major things that are missing are more Items, Upgrades and Special Attacks. The game is created so that adding more special effects for these is easily done and only requires some time and ideas. Due to the small amount of programming needed to implement more this was prioritized lower than other features. There are only two different playable characters for the same reason.

Another thing that is missing is enemy variety. Currently there is only a single enemy type and they will not use special attacks or items. The lack of enemy types were just like more items down-prioritized but the lack of AI for Item and Special Attack use were due to time constraints.

The game is also very unbalanced but it was to be expected since this is a programming project.
Most of these were skipped because of lack of time and that they seemed less important than the other features.

The biggest obstacle during the project was motivation and ideas. When most of the systems were in place so that only bug fixes and actual content for the game were left, I lost some motivation to keep working. The fact that I was working on the project alone is a big reason for this. When I have to work so as to not let someone else down except myself, I have a much easier time doing the grindy and boring tasks. One bug was especially demotivating, where the player could move and attack in the same turn if the commands were inputted in quick succession. It  took about 3 days to solve and left me with very little motivation to continue for the week. After resting a bit I forced myself to continue and regained some motivation. I also became sick twice during the project and lost about a week's worth of time.

I regret making a roguelike because of the sheer amount of items and upgrades etc needed for the game to be fun and have any replay value. If I were to redo the project I would have made it more Zelda inspired with puzzles and fewer but more interesting items and mechanics.

During the project I learned more about A* pathfinding and map generation and have improved my ability to keep code decoupled. Learning A* was one of my goals for the project, which I regret not having learnt earlier since it was not as hard to implement as I had thought. Almost everything in the project is a component and most of the interactions between objects are through events. For some reason I have always been against using Singletons, but in this project I have used a few and I realized that in moderation they are very useful. The level object is a singleton that can be accessed by everything. The level object contains a lot of helpful methods like checking if a tile is empty and getting the unit at a specific position. This reduced the amount of dependency injection needed by a lot. I also learnt to not always overengineer everything, sometimes you just have to be content with systems working.

My time estimate was kind of accurate. Pathfinding and map generation was thought to take at least a week but was almost fully implemented in just 2 days, while the targeting component was thought to only take a day or two but was refactored multiple times and took in total about a week. If I had 1 or 2 more weeks the game would have been even more polished, have more content and more replay value. I am still happy with the result and will probably continue working on the game in my spare time.


