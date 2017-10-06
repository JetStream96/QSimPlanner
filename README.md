# QSimPlanner
A tool for fuel planning and take-off/landing performance calculations.

### [Screenshots](https://github.com/JetStream96/QSimPlanner/issues/4)

### Features
##### Flight planner
- Automatic route finder, based on user-selected departure/arrival runways and SID/STAR. Also finds route to alternate airport.
- Find wind-optimized route.
- Possible to find a route which avoids certain country's airspace.
- Analyze existing routes, and use "AUTO" and "RAND" commands to complete a partial route.
- Automatically download and parse tracks. Supports North Atlantic Tracks (NATs), Pacific Tracks (PACOTs) and Australian Organised Track Structure (AUSOTS).
- Export flight plans formats: FS9, FSX, P3D, and PMDG.
- Download real time wind aloft from NOAA.
- Compute required fuel for flight, using real-world upper wind and several user-specified parameters, such as taxi time, planned holding time and final reserve.
- NavDataPro/Navigraph data format support.

##### Take-off/Landing calculator
- Accurate take-off distance computation based on wind, anti-ice setting and packs, etc. 
- Assumed temperature takeoff is supported. For some aircrafts fixed derate (TO1, TO2) is available.
- Landing calculation with custom flaps, autobrakes setting, and runway surface conditions.

### Requirements
To use QSimPlanner, an AIRAC cycle is required. You can purchase either [Aerosoft](http://www.aerosoft.com/cgi-local/us/iboshop.cgi?showd,7411699320,D11688)'s NavDataPro or [Navigraph](https://www.navigraph.com/FmsData.aspx)'s FMS Data. Use the version of Aerosoft Airbus A318/A319/A320/A321.

### Project Status
Currently the aircrafts available are
- 737-600/700/800/900
- 747-400/400F/8/8F
- 777-200LR
- 777-300ER
- 777F
- 787-8

### Contributing
You are welcomed to submit pull requests to this repository. Feel free to leave any questions or suggestions.

### Coding style
When it is not important to distinguish different exception types thrown in a method, it is preferred to mark the method with `@Throws`. If a method does not throw an exception, mark it with `@NoThrow`.

### License
MIT License
