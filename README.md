# Xenko Terrain

A demo for creating simple terrain using a height map strategy and the Xenko game engine.

## ToDo List

The following are items I wish to finish to consider this demo "done":

1. Implement specular + additional types of lighting.
2. Implement a component to be used with a character component to allow walking on the surface of the terrain.
3. Better integration with Xenko Studio. Too often changes to component properties are not reflected in-editor until studio is re-started.
4. Too much was hard-coded to get this demo running. Will be moving anything hard-coded into component properties.
5. Add full, long-winded code comments to the entire project to help explain what and how each part of the demo is doing.

## Wishlist

This demo is being used to create a fully-featured terrain system that will be made available as a NuGet package eventually. However, some of the features of that system should probably be added to this demo in the mean time. The following are what I hope to improve on this demo as time allows:

1. The height and blend maps were assembled from random places online. The demo could look a lot better with higher quality maps.
2. Either along with #1, or as a minimum replacement for #1, the existing height maps do not blend together well. You can see a gap between the sample tiles in the demo right now because the edges of the height map have subtle differences. 
3. Right now I am using a single blend map for 3 of the 4 tiles in the demo (all 4 have their own height map). This causes those tiles to look pretty bad. I did this due to pure lack of time and the fact I am not a huge fan of working on images. Still, if #1 is not completed, I should at least make a very basic blend map for the 2 height maps in-use. 