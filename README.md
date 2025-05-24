This is the assets folder for the unity game I'm developing. I've called the repo CraftingDungeonCrawler because that is what the game is about. If you are seeing this, hi, hello. If you want to use anything from this repository for some reason, just put a credit in your code. I have no way of enforcing this and to be honest I don't really give a damn, but if you DO decide to use stuff in here and the stuff you use becomes part of something worth millions of dollars, hire me pls. Or just give me money. Either one works. I don't think I'll ever publish the game, and so this repository is mostly just for me or if someone wants to see what I'm working on. If, however, you are from the future and I have published this game and it's a major success and I'm making tons of money, wicked, that's awesome. Right now it's 2025 and I'm just a junior college student who wants to be a game developer but is unfortunately planning on doing something else, as I've heard that the work culture around game development sucks. If you are a hiring manager for a game dev studio and you are seeing this, forget I said that, please hire me. 

To give a better description of the game as I currently want it, it goes like this: Player spawns in a dungeon with a ton of monsters and fights until they die. Those monsters drop materials, xp, gold, etc which the player can then refine into crafting components which then turn into equipment. As the player levels up, they unlock stronger and stronger equipment recipes(current planned implementation is just more materials going into them which means more stat boosts, but I might add inherent modifiers to the recipes.) The player then goes back into the dungeon, fights to get more materials, dies, makes better equipment, repeat. Once the player kills all the enemies in the dungeon, they go down to the next floor, which has stronger enemies. Not sure yet how exactly I'm going to do the difficulty scaling. Maybe add modifiers to the enemy stats depending on floor number or something? Or just stick to making stronger enemies. Either way, the deeper the player goes, the better the materials they can get, which means stronger equipment, which means more floors, and so on. To be honest, I'm not making this game to publish it, I'm making it because 1. I want a game like this and 2. It'll help refine my skills and it's something I can put on my resume.

ROADMAP:
create boss scenes
ore scene
after each boss scene, new set of enemies is generated
environment drops like ore/gem outcrops - got it working, just needs to happen when player hits it instead of running into it
critical hit system
augment system
dynamic sprite generation - mostly working but is still rudimentary
refactor code to be more efficient. Right now a lot of stuff is in one file(for example RefiningUIManager). Put stuff in it’s own file and have the RefiningUIManager call it.
STORY
Fix sprite scaling, bits of it are cut off in inventory/equipment/augment
Alloying mechanic
Infinite combination of materials? Dynamic material creation? The possibilities are endless(supposedly). 
  Method of doing so: idk ask me later once I've figured it out
