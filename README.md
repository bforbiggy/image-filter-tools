# image-filter-tools
### Image filter tools is a command line tool for applying simple filters/operations on images. 
<img src="https://www.techexplorist.com/wp-content/uploads/2018/02/Proboscis-monkey1-768x432.jpg"/> 
 
 
## How To Use
The only pre-requisite is a windows machine with a [.NET core runtime](https://dotnet.microsoft.com/en-us/download) of v6.0 and up.
The program has only two required inputs:
* Source image file path
* Filter/Operation to apply
The operation applied is always the first argument, where input path is specified via ```--input```.  
For a comprehensive list of available options, see program help via the -h option. 
 
 
## Examples
The bare minimum command specifies the operation and the file path  
```./image-filter-tools.exe ascii --input "C:\Users\biggy\Desktop\lol.png"```
<img src="https://i.imgur.com/JW9OXdZ.png"> 
 
 
 
However, you may specify more options as shown below  
```./image-filter-tools.exe grayscale --input "lol.jpg" --resize 0.3 --output "C:\Users\biggy\Desktop\zamn.jpg"```
<img src="https://i.imgur.com/TqFM97i.jpg"> 
 
## Optional Options
output: Lets you specify output location  
resize: Scales image by factor  
detailed: (ascii ONLY) whether or not to use detailed ASCII char set
  
## Filters
ascii: Converts input image into ascii art  
grayscale: Grayscales input image  
blur: blur images
