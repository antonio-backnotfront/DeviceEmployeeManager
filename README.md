# DeviceEmployeeManager

## appsettings.json Structure

1. Logging Settings

Controls how much info gets logged
Normal logs: shows basic info
Framework logs: only shows warnings (to avoid too much noise)
2. Security Settings

AllowedHosts: "*" means the app can run anywhere (you might want to change this later)
3. Database Connection

Where your database info goes
Look for DefaultDatabase to add your own database details
Never put real passwords or connection strings in this file!

## Why I chose not to split the solution into multiple projects
I went for this approach because on my local machine it's not very convenient to work on multiple projects at the same time. Additionally, it's much more comfortable in terms of importing external libraries as I don't need to manually import a single library into multiple projects, I only need to do it once. Also, referencing my classes and methods from other files is faster during the development process. 