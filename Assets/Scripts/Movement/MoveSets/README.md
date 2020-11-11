Any enemy movement AI should be implemented using the `Mover` class which registers
animations for each enemy type when a translation occurs via `SetMovement()`.

Objects MUST have Animator components and parameters with the following set:

- `Horizontal` type float
- `Vertical` type float

Any objects that should be affected by `SlipTime` should inherit from `SlipTimeMover` instead.
