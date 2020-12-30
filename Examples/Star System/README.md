# Star System
    A simulation of our Solar system using the Physics.dll
	Set geocentric = true in Display.cs to place Earth at the center of the Solar system
	Variable number of asteroids

# What It Demostrates
	How to build a PhysicalSystem from Particles 
	How to Iterate() a PhysicalSystem to run a simulation

# What It Tests
    Pushes the performance of PhyscialSystem.Iterate() method
	
# Notes
    Mercury maintains a stable orbit with as large a timeInterval of 1 day between steps now that the modified RK2 method is used for PhysicalSystem.Iterate().