# QSimPlanner
A tool for fuel planning and take-off/landing performance calculations.

![pic](https://dl.dropboxusercontent.com/content_link/sLFTArDyTjZIqdILf8dtWsGr3BF8vHomzPEbpG1XsaEVVcpijZzDVdKEACzPLaTf/file?dl=0)

### Features
##### Flight planner
- Automatic route finder, based on user-selected departure/arrival runways and SID/STAR. Also finds route to alternate airport.
- Find wind-optimized route.
- Possible to find a route which avoids certain country's airspace.
- Analyze existing routes, and use "AUTO" and "RAND" commands to complete a partial route.
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
To use QSimPlanner, an AIRAC cycle is required. You can purchase either [Aerosoft]'s NavDataPro or [Navigraph]'s FMS Data. Use the version of Aerosoft Airbus A318/A319/A320/A321.

Also .NET framework 4.5 is required.

### Projects
These are the projects contained in this repository:

**QSimPlanner**: The full flight planner with take-off and landing calculators.

**QspLite**: Only take-off/landing calculator. All features in this application is available in QSimPlanner as well.

**QSP**: The base class library for QSimPlanner and QspLite.

### Project Status
Currently the aircrafts available are
- 737-600
- 737-700
- 737-800
- 737-900
- 747-8F
- 777-200LR
- 777-300ER
- 777F
- 787-8

More aircrafts need to be added, especially the A320 and A330 families. It would be nice if more Boeing aircrafts can be added, but obtaining required performance data may be difficult.

### Roadmap
- Add more aircraft profiles for QspLite, including A320/A330 series.
- Add ETOPS planning and redispatch.

### Contributing
You are welcomed to submit pull requests to this repository. Feel free to leave any questions or suggestions.

### License
MIT License

   [Aerosoft]: <http://www.aerosoft.com/cgi-local/us/iboshop.cgi?showd,7411699320,D11688>
   [Navigraph]: <https://www.navigraph.com/FmsData.aspx>