# Xenko Terrain

A demo for creating simple terrain using a height map strategy and the Xenko game engine.

## ToDo List

The following are items I wish to finish to consider this demo "done":

1. Some sort of UI for showing hotkeys.
2. Work on the modify feature efficiency. Currently updating the entire geometry for changes to small set.
3. Bring in and integrate the flowing water technique used in my [water sample](https://github.com/TomGroner/XenkoFlowingWater).
4. Better documentation of what the sample can do.
5. Better comments in-code to explain what is being done and why. I might not be doing the right thing for the right reason, but hopefully getting it partially right might point someone else in the right direction :)

## Will Do Time-Allowing

1. Take the time to really refine and abstract a version of this demo that could be a re-usable NuGet package.
2. Implement a paintbrush feature that can define a custom texture that is dynamically generated to paint the surface.

// Special thank you to profan for his sample on updating geometry on the fly: https://github.com/profan/XenkoByteSized
// His code was the basis for this class, with modifications (different geometry structure for example) but otherwise
// his technique entirely :)