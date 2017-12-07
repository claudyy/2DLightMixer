With the 2D Light Mixer system, Sprites can be used to mix with a Post Processing shader over the target camera. There are various ways to mix it.

How it works
The shader takes the render Texture from a second Camera, which only renders the light layer. 
The light layer can be defined by the user. The Light Camera Background is set to black. 
Setup sprites that are set to light layer to render them has light.

Add

The Render Texture is added to main Camera. Color the sprites to achieve different colored lights.

Multiply
The Render Texture is multiplied with the main Camera. 

Mix
The Render Texture is used as a mask to mix the main Camera with a color.

Mix with other Layer
The Render Texture is used as a mask to mix the main Camera with another Camera. What this camera renders can be defined by the user. 
This effect can be used to create a line of sight effect.

Make sure to tweet at me(@ClaudeFehlen) if you create something interesting. 