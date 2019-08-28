# Physics
    Strongly typed physics API for .NET Standard
	Create PhysicalSystems of Particles and Interactions. 
	Simultaneously Iterate() the positions and momenta of all particles in a System.

# ToDo
    Add summaries to classes and methods
	Define vector cross product
    Add implicit conversion of Vector and Scalar to double?
    Add Determinant() and Direction() methods to Tensor
    Define vector division (return a tensor)?
	PhysicalSystem.Iterate() calculates every interactionForce twice (once per particle). Is there a better way?
	    - Is it more efficient to loop through Interactions rather than Particles and use a Mutex on each netForce to prevent thread collisions?

