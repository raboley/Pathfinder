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

# Publishing to Github Packages

to publish to github packages:

```shell script

nuget restore Pathfinder.sln
msbuild Pathfinder.sln /p:Configuration=Release

export GITHUB_PASSWORD=<password>
nuget sources Add -Name "github" -Source https://nuget.pkg.github.com/raboley/index.json -UserName raboley -Password $GITHUB_PASSWORD

nuget setapikey $GITHUB_PASSWORD -Source "github"


nuget push Pathfinder/*.nupkg -source "github" -SkipDuplicate
```