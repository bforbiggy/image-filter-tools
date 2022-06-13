# image-filter-tools
SUPER CRAZY HIDDEN TECH TOOLS?!?! Program with somewhat decent modularity and expandability?!?!? Sheeeeeeeeeeeeeeeeeesh! 
Image filter tools is a cmd tool for applying simple filters/operations on images.

## How To Use
The program has only two required inputs:
* Image file path
* Filter/Operation to apply
The operation applied is always the first argument, where input path is specified via ```--input```.
For a comprehensive list of available options, see program help via the -h option.

## Examples
The bare minimum command specifies the operation and the file path  
```./image-filter-tools.exe ascii --input "C:\Users\biggy\Desktop\lol.png"```
  
  
However, you specify more options as shown below  
```./image-filter-tools.exe ascii --input "C:\Users\biggy\Desktop\lol.png"```

## Optional Options
output: Lets you specify output location  
resize: Scales image by factor  

## Filters
ascii: Converts input image into ascii art  
grayscale: Grayscales input image  
