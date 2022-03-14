# Rolling Horizon Evolution on generating Personalized/Balance racing games
> Implementation on Unity for acquiring diploma 
 
 ## Table of contents
* [General Info](#general-information)
* [Features](#features)
* [Technologies](#technologies)
* [Prerequisites](#prerequisites)
* [Setup](#setup)
* [How to run](#how-to-run)
* [Acknowledgements](#acknowledgements)

## General Information
An innovative algorithm to offer personalized experiences online in a simple racing video game. 
A stochastic planning algorithm (Rolling Horizon Evolution Algorithm (RHEA)), generates
parts of a race track based on two factors:

* Difficulty of the level [^1]
* Player’s in-game performance. 

The algorithm attempts to bring the level of difficulty to match player performance through its fitness function.


## Features

## Technologies
* Unity Real-Time Development Platform
* Unity Machine Learning Agents Toolkit (ML-Agents v-0.15) 
* Anaconda Navigator
* Visual Studio Community

## Prerequisites 
The initial body of work comes from [Medium article](http://medium.com/p/60b0e7a10d9e), for details on the content, please check the article.

At best I'd suggest to follow the [official installation guide](https://github.com/Unity-Technologies/ml-agents/blob/master/docs/Installation.md). However the following steps should suffice:

1. Get Python 3.6 or above 64-bit (the 64 bit seems necessary, at least on Windows)
2. Get Unity 2020.1+
3. Download the ML-Agents 1.0.0+ package (see https://github.com/Unity-Technologies/ml-agents/blob/release_12_docs/docs/Installation.md)
4. Install dependencies: `pip3 install "mlagents"`
5. Clone this repository
6. Open project in Unity


## Setup
## How to run
## Αcknowledgements
* This project was created for the requirements of my Diploma Work in TUC

[^1]:  Additional AI racing agents define the bounds of the channel within which the players are assessed.
