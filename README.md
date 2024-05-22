# adcash-interview-tasks

The .NET solution has been compiled against .NET Core 5 and tested to run on Ubuntu 22.04 with .NET Core 5 installed, although it hsould run on any system with .NET Core 5 installed.
Uploading the solution as is with debug symbols and deliverables for easier debugging.

For each task the Resources folder contains .in or .csv files with input data, one test case per file, and .out files, the expected output for the correspoinding input file.
The Resources folder is copied over to the bin/Debug folder on build, new test cases are to be added in the Resources folder of the project, not the build output folder.

Each of the 3 tasks accepts input through the standard console input. The console program can be executed by invoking the executable at TaskX/bin/Debug/net5.0/TaskX
For task 1 and 3, to invoke the program with inputs from the provided files in bash you can invoke ./TaskX $(cat ./Resources/X.in).
Task 2 expects a filename, not the contents of the file so it can be invoked as just ./Task2 ./Resources/X.csv

Let me know if a proper release build of the executables or a docker container already setup to execute them will be necessary.
