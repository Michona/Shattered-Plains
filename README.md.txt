# Overview

**Turn-based tactical PVP game** played on a board (square grid). The player controls 3 characters with distinct move/attack patterns and abilities. You can move and attack once per character in your turn. There is a timer on the turn, so it is more fast paced. Each character class has one ability that is once per game. The game is 3D, although movement is restricted to one plane. (i.e. 2D logical & 3D graphical). The board has “obstacles” in some tiles, that pervert movement and projectiles. They are random generated but with some constraints (like maximum number and distance between them). 

# Technology 
It is written in C# and **Unity**. The code architecture follows clear object oriented pattern and best practices in regards to Unity. The networking solution is **Photon Unity Networking**. It uses Photon Cloud for hosting. Players can join/create rooms.


# Gameplay Analysis

Allowing player creativity is key. Choosing of character classes and even their starting position has a lot of variety that player can use to their advantage. The synergy between classes is a powerful tool as well. Fast paced is the aim. Being quick and reacting to the situation is the big strong point. 

The concept of invisible traps adds a seemingly random element, but it actually opens up a level of “mind” game. Since placing of traps from enemy is not random (and its chosen with some goal), the player can try and predict and then take a risk or play safe. The crowd control abilities and the obstacles feed into that synergy. 

The distinct attack patterns let players think about positioning on their characters. For example, long range attack character should be in the backline, characters with more durability should be on the frontline and so on. The game having a lot of crowd control creates high priority on positioning in the map, then the approach becomes “OK, I want to position there but I might be forced to an unfavourable position, so how can I outplay this?”.

Another important point is the element of “randomness” in the game, meaning things that are not in control of either player. The only thing that is random is generation of obstacles when the map starts. That adds just enough diversity throughout games so that players can start reacting to the map state the moment they start playing. 

Having simple rules and interactions makes everything behaves as the player expected. There is no “critical” hits or chances to block damage. Also that is why I omitted cooldowns for the abilities just because it adds unnecessary level of complexity. Ultimately this setup should lead to multiple playmaking options without adding crazy layers of complexity or randomness. 
