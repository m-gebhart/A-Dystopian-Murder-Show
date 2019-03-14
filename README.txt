# A-Dystopian-Murder-Show
This is a publish-only repository to see the .cs-files of 'A-Dystopian Murder Show', a student's project of the Cologne Game Lab. BA 1 2018/2019, Ludic Collaborative Project

1. Overview:
Engine: Unity, Language: C#
2D Platformer
	-> use of Orthographic Camera, 2D Sprites / Animations
Two playable characters:
- Laurel (Marketing: Stan)
- Hardy (Marketing: Oliver)

five different enemies: 
Banana, Bat, Lobster, Pie Cannons, Bombs
- each with their own behaviour


2. Combat System (both players and enemies can attack and get killed)

Conceptual Approach:
class: InputManager refers to keys and buttons that are called for certain actions by other classes' functions
other manager classes are responsible for User Interface, Audio, Scoring and more

class: Player containing all abilities both characters share: Spawn(), Jump(), Move(), Die(), GroundContact(), etc.
Laurel and Hardy each contain abilitites only one character can execute:
	class: Laurel: DoubleJump(), RangedAttack() / TrowHat(), CatchHat()
	class: Hardy: MeleeAttack(), JumpKill()
	attributes such as jumpHeight or movementSpeed can be adjusted for every character individually
	class: Playermanager is responsible for Shapeshifting(), including the transfer of information between Laurel and Hardy (e.g. rotation, velocity, ...)

class: Enemy contains all abilites every type of enemy shares: Spawn(), Die(), Grow() etc.
class: MovingEnemy contains ability to Move() back and forth - Lobsters and Enemies inherit from this class
every enemy type's class (Banana, Lobster, Pie Cannons, Bombs, Bats) contain information how they react to the player's action (e.g. being jumped on, chase him, kill him, ...)


3. General Concept of Combat:
- if the player attacks an enemy (instatiating of Attack Trigger) and hits the opponent's right trigger / the right part of body (Polygon Collider), the enemie dies and the player checks in an internally table how big the score / reward is
- if the player touches an enemy but his head or the enemy's Attack Trigger (without the player Attacking()), he dies and game is lead to an Game Over Screen
- if the player jumps / falls down on an enemy's head, the player executes a jump (in case of Hardy: he kills the enemy with a JumpKill() at the same time) 
exceptions for different enemies: e.g. Bananas cannot get jumped on, touching Cannons don't lead to a Game Over immediately, etc.
