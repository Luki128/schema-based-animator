# schema-based-animator
Scritable engine to generate animations writen in c# netcore
To generate out source like mp4 or gif engie use FFmpeg binaries that you can find here:
https://github.com/BtbN/FFmpeg-Builds/releases

## Abstract script commands:
  To pass variable to any command is obligatory to use '$' sign before variable name
  
  Legend:
  * aVar - variable
  * bVar - variable or constant
  * name - name of variable (without '$' sign)
  * times - integer present how many repeats loop works
  ### Defnie variable
  * var name value
  ### int variable operations
  * add aVar bVar
  * sub aVar bVar
  * mul aVar bVar
  * div aVar bVar
  ### float variable operations
  * addf aVar Var
  * subf aVar bVar
  * mulf aVar bVar
  * divf aVar bVar
  ### Loop
  * repeat times
  * end - every loop must end with this command (end loop)

## Animation script commands:
This commands also accept constants and variables as argument
but has major impact on generated animation
Legend:
  * width,height,frames,origin_x,origin_y,x,y, - integer values
  * path - sttring valuse
  * angle,local_x,local_y,global_x,global_y - float
### Start of the animation
this command is required before any other command with this chapter
 * canvas width height frames
### Generate canvas
Load image on animation, any transformation commands will be applied after this command for given image until next load image command will appear in script
 * clip path origin_x origin_y
### Transformation commands
These commands, has argument frame which mean frame number when the transformation reach given values(using interpolation)
 * position frame x y
 * place frame x y
 * move frame x y
 * shift frame x y - shift has exption this x y args are floats
 * rotation frame angle
 * rotate frame angle
 * scale frame local_x local_y global_x global_y
 * rescale frame local_x local_y global_x global_y
### Generate final video/animation with applied commands
 * video name

# Examples
## Demo 1
### Script
```
var frames 40
var W 600
var H 450
var border 10
var boxSize 128
var boxMaxW $W
sub $boxMaxW $boxSize
sub $boxMaxW $border
sub $boxMaxW $border
var boxMaxH $H
sub $boxMaxH $boxSize
sub $boxMaxH $border
sub $boxMaxH $border
canvas $W $H $frames
clip sources\bg.png 0 0
scale 0 1 1 3 3
clip sources\box.png 0 0
position 0 $border $border
move 10 $boxMaxW 0
move 20 0 $boxMaxH
mul $boxMaxW -1
move 30 $boxMaxW 0
mul $boxMaxH -1
move 40 0 $boxMaxH
video demo1.gif
```
### Launch command
 ```
 schema-based-animator.exe demo1.txt
 ```
### Result
 ![](https://github.com/Luki128/schema-based-animator/blob/master/demos/demo1.gif)
