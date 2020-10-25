# Pathfinder

![ci](https://github.com/raboley/Pathfinder/workflows/ci/badge.svg)

C# Library to find the most efficient path for a given grid based on X and Y coordinates.

#### Example Output of a Printed grid

```
Visualization of the path
s = start
e = end
w = waypoint
x = obstacle
-------------------------------
|     |     |     |     |     |
-------------------------------
|  e  |  w  |     |     |     |
-------------------------------
|  x  |  x  |  x  |  w  |     |
-------------------------------
|     |  w  |  w  |     |     |
-------------------------------
|  s  |     |     |     |     |
-------------------------------
```

The path to travel starts at `s` and travels along `w` avoiding `x`'s trying to reach `e`
