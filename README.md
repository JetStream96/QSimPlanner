# QSimPlanner
A tool for fuel planning and take-off/landing performance calculations.

### Features
##### Flight planner
- Automatic route finder, based on user-selected departure/arrival runways and SID/STAR. Also finds route to alternate airport.
- Analyze existing routes, and use "AUTO" and "RAND" commands to complete a partial route. "X AUTO Y" finds a route between X and Y. "X RAND Y" finds a random route between X and Y.
- Automatically download and parse tracks. Supports North Atlantic Tracks (NATs), Pacific Tracks (PACOTs) and Australian Organised Track Structure (AUSOTS).
- Export flight plans to PMDG .rte format.
- Download real time wind aloft from NOAA.
- Compute required fuel for flight, using real-world upper wind and several user-specified parameters, such as taxi time, planned holding time and final reserve.
- NavDataPro/Navigraph data format support.

##### Take-off/Landing calculator
- Accurate take-off distance computation based on wind, anti-ice setting and packs, etc. 
- Assumed temperature takeoff is supported. For some aircrafts fixed derate (TO1, TO2) is available.
- Landing calculation with custom flaps, autobrakes setting, and runway surface conditions.

### Requirements
To use QSimPlanner, an AIRAC cycle is required. You can use either Aerosoft's [NavDataPro] or Navigraph's [FMS Data].

Also .NET framework 4.5 is required.

### Projects
These are the projects contained in this repository:

**QSimPlanner**: The full flight planner with take-off and landing calculators.

**QspLite**: Only take-off/landing calculator. All features in this application is available in QSimPlanner as well.

**QSP**: The base class library for QSimPlanner and QspLite.

### Project Status
**QSimPlanner**: Some compenents for this project was written quite a while ago. Needs to be refactored and many unit tests have to be added. Also the UI needs complete rework. The application works at its current state but only if you are very familiar with this project.

**QspLite**: Currently the aircrafts available are
- 737-600
- 737-700
- 737-800
- 737-900
- 747-8F
- 777-200LR
- 777-300ER
- 777F
- 787-8

More aircrafts need to be added, especially the A320 and A330 family. It would be nice if more Boeing aircrafts can be added, but obtaining required performance data may be difficult.

### Roadmap
##### Short term
- Add unit tests for classes used in the flight planner. Possibly switch to another unit test framework (currently using MSTest).
- Add more aircraft profiles for QspLite, including A320/A330 series.
- More features for route finder. e.g. Finding shortest route based on wind,  avoiding certain country's airspace.
- Add ETOPS planning and redispatch.

##### Future
- Complete UI rework for QSimPlanner, with UI tests.
- Maybe some other cool things.

### Contributing
You are welcomed to submit pull requests to this repository. Feel free to leave any questions or suggestions.

### License
MIT License

   [NavDataPro]: <http://www.aerosoft.com/cgi-local/us/iboshop.cgi?showd,7411699320,D11688>
   [FMS Data]: <https://www.navigraph.com/FmsData.aspx>