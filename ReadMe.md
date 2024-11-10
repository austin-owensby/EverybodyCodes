# Everybody Codes
This repository is setup to assist with solving challenges from the [Everybody Codes](https://everybody.codes/home/).

It allows you to easily run solutions in C#, submit answers and see the response, pull down your inputs, and run test inputs via an API described below.
This also includes some utilities to make some solutions easier. See the `Services/Utility.cs` file for more info.

The `main` branch contains a ready to use template to start your own solutions.
You may also reference the `aowensby-solutions` branch which contains my own solutions.

## Quick Start
1. Run the Program (See [Setup](#setup) below)
1. (Optional) Create a Cookie.txt file to enable puzzle input/submission (See [Puzzle Helper](#puzzle-helper) below)
1. (Optional) Make the `import-input-file` API call to get your input for the quest you're trying to solve (See [API](#post-apiimport-input-file) below)
1. Code your solution in one of the Service files
1. Make the `run-solution` API call and optionally submit the solution (See [API](#get-apirun-solution) below)

## Setup
1. If not already installed, install [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download)
1. Run the program
   - If using Visual Studio or VSCode, use the play button to build and run the code
   - If using a CLI, run `dotnet run` from the repo's base folder
1. Run API calls
   - You can use [Swagger](https://swagger.io/), an API documentation and execution library, to execute the API calls, or use your own tool to call to the API
   - Visual Studio
      - The browser should open Swagger by default, to change this behavior, update the `Properties/launchSettings.json`
   - VSCode
      - Use the `Launch (web)` or `Launch (web - no browser)` configuration to toggle if you want the browser to open automatically
      - If you'd prefer to remain within VSCode to make the API calls, I've used the [Thunder Client](https://marketplace.visualstudio.com/items?itemName=rangav.vscode-thunder-client) extension. You can inport the collection provided at `thunder_collection_EC.json`.
   - Other
      - Visit https://localhost:5001/swagger in your browser

## Puzzle Helper
This allows you to easily create the needed services as well as submitting answers.

In the `PuzzleHelper` folder, create a `Cookie.txt` file and add your own cookie that gets created when logging into the Everybody Codes website. While on the Everybody Codes website, if you open the Network tab in your browser's Dev Tools, you'll see the cookie in the header of API calls that are made while navigating the site. This typically expires after a month so you'll need to update it each year.

Ensure that the Cookie.txt is all 1 line.

If you would like to avoid this setup, you can always manually add you input and submit your solutions without having to create a Cookie.txt file.
The file is only required when interacting with the Everybody Codes website.

### Automation
Although not required by the Everybody Codes rules as of 2024-11-9, we'll follow the recommendations from the Advent of Code rules since Everybody Codes is inspired by it.
The Puzzle Helper does follow the automation guidelines on the [/r/adventofcode community wiki](https://www.reddit.com/r/adventofcode/wiki/faqs/automation).

Specifically:
* Outbound calls are throttled to every 3 minutes in the EverybodyCodesGateway's `ThrottleCall()` function
* Once inputs are downloaded, they are cached locally (PuzzleHelper's `WriteInputFile(int year, int quest)` function) through the `api/import-input-file` endpoint described below.
* If you suspect your input is corrupted, you can get a fresh copy by deleting the old file and re-running the `api/import-input-file` endpoint.
* The User-Agent header in the Program.cs's gateway configuration is set to me since I maintain this tool :)

## API

### GET `api/run-solution`
- Query parameters
   - year (Ex. 2024) (Defaults to 2024)
   - quest (Ex. 14) (Defaults to 1)
   - part (Ex. 3) (Defaults to 1)
   - send (Ex. true) (Defaults to false) Submit the result to Everybody Codes
   - example (Ex. true) (Defaults to false) Use an example file instead of the regular input, you must add the example at `Inputs/<YYYY>/<DD>/<P>_example.txt`
- Ex. `GET api/run-solution?year=2024&quest=14&part=3&send=true`

Runs a specific quest's solution, and optionally posts the answer to Everybody Codes and returns the result.

### POST `api/import-input-file`
- Query parameters
   - year (Ex. 2024) (Defaults to 2024)
   - quest (Ex. 14) (Defaults to 1)
   - part (Ex. 3) (Defaults to 1)
- Ex. `POST api/import-input-file?year=2024&quest=14&part=3`

Note, this has been left unimplemented because as of 11-10-2024 the moderator mentioned:
> My goal was to make it:

> - resistant to automation, or at least difficult to automate, to discourage AI enthusiasts from using bots to solve the quests - such as hitting the API unnecessarily frequently around the release of a new task, etc.

If this changes, I will check in my code that downloads the input files, for the time being you will need to add these manually.

Imports the input from Everybody Codes for a specific quest's part.

The program is idempotent (You can run this multiple times as it will only add a file if it is needed.)

### POST `api/generate-service-files`

Creates missing daily solution service files.
Useful when a new year has started to preemptively generate the service files for the calendar year before the advent starts.

You'll likely only need to use this once per year and only if either your source code has gotten out of sync from the `main` branch or I haven't kept it up to date.

The program is idempotent (You can run this multiple times as it will only add files if they are needed.)

## Extra Notes
- Extra restrictions are implemented here that are not explicitly required by Everybody Codes, but are by Advent of Code which is what Everybody Codes is inspired by
   - For example, the admin of Advent of Code have requested that puzzle inputs be cached (To reduce load on the system) and not be made publicly available (To make it harder to completely copy the site)
- This puzzle helper currently does not use the leader board api, but if you choose to copy this template and talk to the leader board, make sure to throttle (the recommendation is around 15 minutes) and cache the calls to not overload the server