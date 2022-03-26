# Rolling Horizon Evolution on generating Personalized/Balance racing games
> AI algorithm implementation on Unity for acquiring computer engineering diploma 
 
 ## Table of contents
* [General Info](#general-information)
* [Features](#features)
* [Experiments](#experiments)
* [Technologies](#technologies)
* [Prerequisites](#prerequisites)
* [Acknowledgements](#acknowledgements)

## General Information
An innovative algorithm to offer personalized experiences online in a simple racing video game. 
A stochastic planning algorithm [(Rolling Horizon Evolution Algorithm (RHEA))](#rolling-horizon), generates
parts of a race track based on two factors:

* Difficulty of the level. [^1]
* Player’s in-game performance. 

The algorithm attempts to match players skills upon track measurement. In other words, bring the level of difficulty, up or down, to match player performance through its fitness function (time completion). The degree of difficulty is determined by a function that is rooted in theory of [Flow Channel](#flow-channel-theory).
We have conducted a series of experiments in our game environment with agents using __human strategies__ and __non-human player__ behavior.

## Features
The original implementation used static paths aimed at learning the agents. For information on how to use the training model using anaconda environment, see [doc/Train-Behavior Configuration](https://github.com/Unity-Technologies/ml-agents/blob/main/docs/Training-ML-Agents.md#:~:text=Training%20in%20the%20ML-Agents%20Toolkit%20is%20powered%20by,learning%29.%20Its%20implementation%20can%20be%20found%20at%20ml-agents%2Fmlagents%2Ftrainers%2Flearn.py.).
The initial body of work comes from [Medium article](http://medium.com/p/60b0e7a10d9e). For details on the content, please check the article.

<br><br>

![player2](https://user-images.githubusercontent.com/22920222/159187776-2b59c258-6a42-499d-90af-04ace17e0f9f.PNG)

<br>

We modify the implementation and the goal is not to learn the agents but to construct content that is matched to the performance of the player. 
* Construct dynamic paths on runtime execution (taking into account the parameters needed for construction (distances, direction, collisions) for main and RH tracks).
* Adequate spatial layout between all tracks of the RH algorithm.
* Modify the brain model for the requirements of our work. 
    * Player can use brain in __heuristic__ mode. 
    * Agents for flow channel area and evolution  use __inference__ mode brain.

### Rolling Horizon
In baseline form, *RHEA* utilises Evolutionary Algorithms (EA) to evolve an in-game sequence of actions at every game tick using a *Forward Model (FM)*, with restricted computation time per execution. 

* In our implementation, we evolve sequence of race tiles. External driving agents perform the evaluation

One evolution stage/iteration is up to:
* Parent chromosomes (main tracks)
* Offsprings (tracks built from evolution)
Parent chromosomes are divided into two equal sectors (genotype can be represented as a checkpoint in the middle of the tracks that marks the player's time). Offsprings do not use checkpoints[^2].

<br>

*Offsprings after player crosses checkpoint:*
<br>
![phase1](https://user-images.githubusercontent.com/22920222/160229906-66f0bd81-a156-49cf-b540-e6328f500502.png)

*Offsprings after player crosses finish line:*
<br>
![phase2](https://user-images.githubusercontent.com/22920222/160229911-b29754d2-96aa-4871-a46f-ce90baba040d.png)

We do not want an additional evolutionary phase in these type of tracks. So, there is no checkpoint.

<br>

### Flow Channel Theory
[*Flow activities*](https://dansilvestre.com/flow-channel/)  are guilty of maintaining challenge between the edges
of boredom and depression in gaming. There the user can become either bored or frustrated. Flow
can serve as a function of the search space between skills and challenges.

The concept focuses on how the agents behave on the benchmark tracks and contribute to the channel formulation based on their execution.
* Straight-line [^3]
* Snake-line [^4]

![ss](https://user-images.githubusercontent.com/22920222/160231006-1c68f858-93c2-4295-b214-dee7e356ea2f.png)

## Experiments
The experimental analysis contains three parts:
* Display configurations for the flow channel.
* Show sequence of generated tracks in response to player’s behavior.
* Exhibition the profile of the player.

| Parameter  | Value |
| ------------- | ------------- |
| Population Size P  | 25  |
| Elitism Factor | 20%  |
| Individual Length  | 20  |
| Mutation Propability  | 30%  |
| Crossover Propability  | 30%  |
| Generation  Iterations  | 20  |

We tested our system and the simulations lasted for 20 levels.

## Technologies
* Unity Real-Time Development Platform
* Unity Machine Learning Agents Toolkit (ML-Agents v-0.15) 
* Anaconda Navigator
* Visual Studio Community

Useful material for ml packages installation/execution/description/etc can also be found in the [docs](https://github.com/miyamotok0105/unity-ml-agents/tree/master/docs) of the creators.


## Prerequisites 
At best I'd suggest to follow the [official installation guide](https://github.com/Unity-Technologies/ml-agents/blob/master/docs/Installation.md). However the following steps should suffice:

1. Get Python 3.6 or above 64-bit (the 64 bit seems necessary, at least on Windows)
2. Get Unity 2020.1+
3. Download the ML-Agents 1.0.0+ package (see https://github.com/Unity-Technologies/ml-agents/blob/release_12_docs/docs/Installation.md)
4. Install dependencies: `pip3 install "mlagents"`
5. Clone this repository
6. Open project in Unity

Detailed information about the installation and execution can also be found at [doc](
https://github.com/miyamotok0105/unity-ml-agents/blob/master/docs/Installation-Windows.md)

## Acknowledgements
* This project was created for the requirements of my Diploma Work in TUC
* You can view more details about the work in [pdf](https://github.com/z1skgr/Thesis/issues/4#issue-1181625504)
and in [video](https://drive.google.com/file/d/1uL9BW7yPc2OaRKHYjAQV0_Kmkh-5ti09/view?usp=sharing)

[^1]: Additional AI racing agents define the bounds of the channel within which the players are assessed.
[^2]: We want to evolve main population, not offsprings. We could make some adjustments for modifications in evolving stages but not implemented.
[^3]: Maximum performance (or a low finish time).
[^4]: Minimum performance (or a high finish time).
