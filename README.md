# Physics
    Strongly typed physics API for .NET Standard
	Create Systems of Particles and Interactions. 
	Simultaneously Iterate() the positions and momenta of all particles in a System.

# ToDo
    Add summaries to classes and methods
	Define vector cross product
    Add implicit conversion of Vector and Scalar to double?
    Add Determinant() and Direction() methods to Tensor
    Define vector division (return a tensor)?
	System is a namespace... think of a better name or just call as Physics.System?
	System.Iterate() calculates every interactionForce twice (once per particle). Is there a better way?
	    - Is it more efficient to use a Mutex on each netForce to prevent thread collisions?
    System.Iterate() could potentially take more advantage of Runge-Kutta method than Particle.Iterate() because it has access to all Particles.
