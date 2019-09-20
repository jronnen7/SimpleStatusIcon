# Simple Status Icon - Build

I cannot gaurentee the prebuilt zip will run on everyones computer, but it should run on any windows 10 PC running dotnet core `v3.0.0-rc1`. 

[Microsoft Downloads](https://dotnet.microsoft.com/download/dotnet-core/3.0)
[x64](https://dotnet.microsoft.com/download/thank-you/dotnet-sdk-3.0.100-rc1-windows-x64-installer) 

The best way to install the app is to build the app and copy all resulting files to a safe location, (or the unzip file in this directory if you are choosing to use the prebuilt files) Then use windows `Task Scheduler` to schedule the task to run when your computer starts up.  Set to run the program when the user logins in.

1) Action: Start a Program
2) Program/script: {location of built exe}
3) Start in: {directory of built exe)

