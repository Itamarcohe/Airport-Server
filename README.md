# Airport Terminal Project

## Welcome to the Airport Terminal project!
This project is a web-based airport management system designed to facilitate the movement of flights through terminal legs.

## Overview
The Airport Terminal project utilizes ASP.NET Core Web API for the server-side logic and React for the client-side interface. It enables real-time tracking and management of flight movements, allowing users to monitor flights as they progress through different terminal legs within the airport.

The simulator is a separate console application that generates random flight objects and sends them to the web API via HTTP POST requests.

## Features
- **Multi-threaded processing** for efficient management of concurrent flights.
- **Real-time tracking and logging** of flight movements through terminal legs.
- Integration with **SQL database** for storing flight data, terminal configurations, and logs.
- Standalone console application simulates flight objects sent to the web API via **HTTP POST requests**.
- **Client-side interface** developed in React with **SignalR WebSocket** for real-time updates.

## Installation
1. **Clone the repository:**
   ```bash
   git clone https://github.com/Itamarcohe/Airport-Server.git
2. **Install dependencies** for the server-side (ASP.NET Core Web API): cd server dotnet restore
