# Simple Status Icon

Windows Taskbar Notify Icon application designed to change icons based on resolved conditions.

## Build &amp; Install

The project requires DotNet Core 3 Preview 9 in order to build download that from Microsoft's website.

The best way to install the app is to build the app and copy all resulting files to a safe location.  Use windows `Task Scheduler` to schedule the task to run when your computer starts up.  Set to run the program when the user logins in.

1) Action: Start a Program
2) Program/script: {location of built exe}
3) Start in: {directory of built exe)

## Icon Files

Icon files are located in the Icon folder, if you want to force the icon file to copy to the output on build it does require an update to `SimpleStatusIcons.csproj`.  The only allowable icon types are `ico` files.  There are several tools to convert any image to an `ico`, all current icons are configured for 128 x 128 pixel resolution.

## Setting up a Config File

`AppSettings.json` file is requried for the application to run, please define one to suite your needs.
Status Icons resolve based on conditions that evaluate at certain times, the times that conditions are evaluated depends on the Modes that are set in the config.  After a paticular event is triggered it waits 2.5 seconds just in case other or duplicate events come in, after the first event is processed and a 2.5 second debounce time is waited, the conditions get analyzed in the order that the Nodes are defined.

I.E.
For the below config, it will first ping 10.0.0.1, if this resolves successfully it will set the icon to `./Icons/minus-circle-red.ico` if this does not resolve successfully it will continue to try to resolve the condition for the next icon.  Because the third icon does not contain any conditions this always resolves to true, meaning it will be the fallback icon.
```
{
  "StatusIcon": {
    "Icons": [
      {
        "Path": "./Icons/minus-circle-red.ico",
        "HoverText": "Connected to External Network",
        "Condition": {
          "Test": "Ping",
          "Uri": "10.10.1.120",
          // retry if this condition fails, useful when adapters are really slow to connect and the debounce time is too short,
          // the debouce time in the BaseIconHandler can also be adjusted to help eliviate having to force a retry.
          "ShouldRetry": true
        }
      },
      {
        "Path": "./Icons/check-circle-green.ico",
        "HoverText": "Connected to Private Network",
        "Condition": {
          "Test": "Ping",
          "Uri": "172.24.64.114"
        }
      },
      {
        "Path": "./Icons/minus-circle-gray.ico",
        "HoverText": "Not Connected to any VPN"
      }
    ],
    "Modes": [
      {
        "Event": "NetworkAdapterChanged"
      },
      {
        "Event": "Periodic",
        "Frequency": "1800000" // milliseconds == 30 minutes
      }
    ]
  }
}
```

Another example can be found below
```
{
  "StatusIcon": {
    "Icons": [
      {
        "Path": "./Icons/check-circle-green.ico",
        "HoverText": "Connected to External Network",
        "Condition": {
          "Test": "WebRequest",
          "Uri": "http://test"
        }
      },
      {
        "Path": "./Icons/minus-circle-gray.ico",
        "HoverText": "Connected to Private Network",
        "Condition": {
          "Test": "WebRequest",
          "Uri": "http://test2"
        }
      },
      {
        "Path": "./Icons/minus-circle-red.ico",
        "HoverText": "Not Connected to the internet"
      }
    ],
    "Modes": [
      {
        "Event": "NetworkAdapterChanged"
      },
      {
        "Event": "Periodic",
        "Frequency": "1800000" // milliseconds == 30 minutes
      }
    ]
  }
}
```