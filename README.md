# CommBankCodeChallenge


This is the project corresponding to the code challenge for CommBank. 

This program performs the counting rules as given in the coding test pdf file.

How to run this program ? 
1. Either by running the exe CommBankCodeChallenge.exe. The program will execute the rules as described in the PDF and produce 
  the requested files in the bin folder (where the executable is). 

2. Launch this program from a command line / power shell with as "CommBankCodeChallenge.exe parameters.xml" and placing the file parameters.xml
  in the bin folder (where the executable is). This mode will read the xml file which is initiated with the default counting rules.
  If you want to change the counting rules, you can modify the file parameters.xml, save it and relaunch with the powershell.
  Note that the output file name might change depending on the parameters of the counting rules.

General notes:
* To simplify the program, the source of words is a static list but we can change it to read a file containing a source.
* To simplify the program, there is no try/catch block to prevent any exception to be raisen. Good input are therefore "requested" from the
  user.

If you have any questions regarding this program, please contact Arthur Bargy via email at bargy.arthur@gmail.com.
