# A-Dystopian-Murder-Show

## Pls Note

⚠️ This is the first ever game project and the first project in Unity I have created. This project had been completed back in the very beginning of my study programme. Hence, the code is not representative of my current technical skills anymore. :warning:

## Description

In A Dystopian Murder Show, a 2D action game, players control two mutants sharing one body. Trapped in deadly arenas, they try to survive against brutal hordes of monsters and get the highest score as possible. 

As part of a collaborative team (size of 5), I was the main programmer being responsible for major game systems (e.g., controls, combat, UI, animation).

Project page on my <a href="https://michael-gebhart.com/projects/A_Dystopian_Murder_Show.html">portfolio website</a>.

Project page on <a href="https://colognegamelab.de/study-programs/student-projects/the-laurel-hardy-murder-case-2018-19/">Cologne Game Lab</a>.

Trailer and Build of A Dystopian Murder Show on <a href="https://gemoneoo.itch.io/a-dystopian-murder-show">itch.io</a>.

## Team

<ul>
	<li>Angelo Wedel (Arts & Animation)</li>
	<li>Ioana Sandu (Arts & Animation)</li>
	<li>Isaac Baltsch (Game Design)</li>
	<li>Michael Gebhart (Game Programming)</li>
	<li>Yannick Renz (Game Design, Sound Design)</li>
</ul>

## Media

### GIFs
![Murder-Show_1](https://user-images.githubusercontent.com/45672199/200560715-e68bc5e3-8129-4d03-a987-d432db93af5a.gif)
![Murder-Show_3](https://user-images.githubusercontent.com/45672199/200560737-4dcd6af4-1d4a-4bb4-9804-6eeb0f6368ff.gif)

#### Trailer on YouTube
[![A Dystopian Murder Show - Trailer](http://img.youtube.com/vi/6dNMNJt_RQI/0.jpg)](http://www.youtube.com/watch?v=6dNMNJt_RQI "YouTube: A Dystopian Murder Show - Trailer")

#### Screenshots
![grafik](https://user-images.githubusercontent.com/45672199/198715790-7fa06648-92ad-493f-b571-6027437f7b68.png)


## Project Concept

<ol>
	<li>Overview:</li>
		<ul>
			<li>Engine: Unity, Language: C#</li>
			<li>2D Platformer</li>
			<li>Two playable characters:</li>
				<ul>
				<li>Laurel</li>
				<li>Hardy</li>
				</ul>
			<li>enemies:</li>
				<ul>
				<li>Bananas, Bats, Lobsters, Pie Cannons, Bombs</li>
				</ul>
		</ul>
</ol>
<ol start="2">
	<li>Conceptual Approach:</li>
		<ul>
		<li>class: InputManager refers to the control system while other manager classes are responsible for User Interface, Audio, Scoring and more</li>
			<li>class: Player contains all abilities both characters share: Spawn(), Jump(), Move(), Die(), etc.</li>
			</li>classes Laurel and Hardy each contain abilitites only one character can execute:</li>
			<ul>
				<li>Laurel: DoubleJump(), RangedAttack() / TrowHat(), CatchHat()</li>
				<li>Hardy: MeleeAttack(), JumpKill()</li>	
			</ul>
			<li>class: Playermanager is responsible for Shapeshifting() and transfer of data between Laurel and Hardy</li>
			<li>class: Enemy contains all abilites every type of enemy shares: Spawn(), Die(), Grow() etc.</li>
			<ul>
			<li>class: MovingEnemy contains ability to Move() back and forth - Lobsters and Enemies inherit from this class</li>
			<li>every enemy type contains information on how it should react to the player's action (e.g. being jumped on, chase him, kill him, ...)</li>
			</ul>
		</ul>
		</ul>
</ol>

<ol start="3">
<li>General Concept of Combat:</li>
<ul>
<li>if the player attacks an enemy (instatiating of Attack Trigger) and hits the opponent's right part of body (trigger), the enemie dies and the player checks how big the score / reward is</li>
<li>if the player touches an enemy apart from his head or the enemy's Attack Trigger (without the player Attacking()), he dies and game leads to an Game Over Screen</li>
<li>if the player jumps / falls down on an enemy's head, the player auto-jumps (in case of Hardy: he kills the enemy with a JumpKill() at the same time)</li>
<li>game design exceptions for different enemies: e.g. Bananas cannot get jumped on, touching Cannons don't lead to a Game Over immediately, etc.</li>
</ul>
</ol>
