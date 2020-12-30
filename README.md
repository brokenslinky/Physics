# Physics
    Strongly typed physics API for .NET Standard
	Create PhysicalSystems of Particles and Interactions. 
	Simultaneously Iterate() the positions and momenta of all particles in a System.

# ToDo
	Define vector cross product
	Add field in DerivedUnits for unitRatio - the ratio of the appropriate SI units to the given units
	Add Dictionary with conversions from non-standard to SI units
	Add a Method() to figure out the combination of non-standard units making up a unitRatio
	Add a Method() to separate the BaseUnits from DerivedUnits by determining the prime factors
	Add implicit conversion between Scalar and String (requires unit Dictionaries first)
	PhysicalSystem.Iterate() could have reduced overhead by doing the arithmetic to doubles rather than Vectors
    Add implicit conversion of Vector and Scalar to double?
    Add Determinant() and Direction() methods to Tensor
    Define vector division (return a tensor)?
	PhysicalSystem.Iterate() calculates every interactionForce twice (once per particle). Is there a better way?
	    - Is it more efficient to loop through Interactions rather than Particles and use a Mutex on each netForce to prevent thread collisions?
