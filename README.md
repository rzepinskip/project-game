# The Project Game

Software Engineering project consisting of implementing system using given specification, using design patterns, planning systems' architecture, TCP communication, teamwork in Agile methodology.

## Description

The system organizes matches between teams of cooperating agents. The game is played in real time on a rectangular board, consisting of the tasks area and the goals area. Each team of agents needs to complete an identical project, consisting of placing a set of pieces, picked from the tasks area, in the project goals area. The pieces appear at random within the tasks area. The agent’s view of the tasks area is limited and the shape of the goals needs to be discovered. The game is won by the team of agents which is first to complete the project.

Initial technical specification designed during first semester of SE course(unfortunately, not implemented in this project) can be found at [Initial_specification.pdf](Initial_specifcation.pdf)

## Structure

The system consists of 3 main components - Game Master, Communication Server and Players. Matching console apps projects are suffixed with `.App`.

## Launch

All system's components can be launched using scripts located at `/Resources/GameScripts/`.

### Parameters

Apps' runtime parameters are described in theirs' usage, which is displayed when no parameters are passed.

## Authors

Paweł Rzepiński, Sebastian Sowik, Mateusz Stelmaszek, Ryszard Szymański, Wojciech Trześniowski
