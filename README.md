# Rolling Horizon Evolution on generating Personalized/Balance racing games
> AI algorithm implementation on Unity for acquiring computer engineering diploma 
 
 ## Table of contents
* [General Info](#general-information)
* [Features](#features)
* [Technologies](#technologies)
* [Prerequisites](#prerequisites)
* [Acknowledgements](#acknowledgements)

## General Information
An innovative algorithm to offer personalized experiences online in a simple racing video game. 
A stochastic planning algorithm (Rolling Horizon Evolution Algorithm (RHEA)), generates
parts of a race track based on two factors:

* Difficulty of the level [^1]
* Player’s in-game performance. 

The algorithm attempts to bring the level of difficulty to match player performance through its fitness function.


## Features
The original implementation used static paths aimed at learning the agents.

For information on how to use the training model using anaconda environment, see [doc/Train-Behavior Configuration] (https://github.com/Unity-Technologies/ml-agents/blob/main/docs/Training-ML-Agents.md#:~:text=Training%20in%20the%20ML-Agents%20Toolkit%20is%20powered%20by,learning%29.%20Its%20implementation%20can%20be%20found%20at%20ml-agents%2Fmlagents%2Ftrainers%2Flearn.py.)

The initial body of work comes from [Medium article](http://medium.com/p/60b0e7a10d9e), for details on the content, please check the article.

We modify the implementation and the goal is not to learn the agents but to construct content that is identified with the performance of the player. 

* Construct dynamic paths on runtime execution (taking into account the parameters needed for construction (distances, direction))
* Modify the brain model for the requirements of our work. 
    * Player can use brain in __heuristic__ mode. 
    * Agents for flow channel area and evolution  use __inference__ mode brain.



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

[^1]:  Additional AI racing agents define the bounds of the channel within which the players are assessed.
