


# AdventOfCode20xx
Solutions to the Advent Of Code 20xx with C# (.NET 7). The goal is to make it (somewhat) readable

[![GitHub issues](https://img.shields.io/github/issues/p-kostic/AdventOfCode2020)](https://github.com/p-kostic/AdventOfCode2020/issues)
[![GitHub forks](https://img.shields.io/github/forks/p-kostic/AdventOfCode2020)](https://github.com/p-kostic/AdventOfCode2020/network)
[![GitHub stars](https://img.shields.io/github/stars/p-kostic/AdventOfCode2020)](https://github.com/p-kostic/AdventOfCode2020/stargazers)
![visitors](https://visitor-badge.glitch.me/badge?page_id=p-kostic.adventofcode2020)
[![GitHub license](https://img.shields.io/github/license/p-kostic/AdventOfCode2020)](https://github.com/p-kostic/AdventOfCode2020/blob/master/LICENSE.md)
[![Twitter](https://img.shields.io/twitter/url?style=social&url=https%3A%2F%2Fgithub.com%2Fp-kostic%2FAdventOfCode2020)](https://twitter.com/intent/tweet?text=Wow:&url=https%3A%2F%2Fgithub.com%2Fp-kostic%2FAdventOfCode2020)

## Table of Contents
* [Solution Completion Table](https://github.com/p-kostic/AdventOfCode20xx#solution-completion-table)
* [Temporary note for running older years when cloning](https://github.com/p-kostic/AdventOfCode20xx#temporary-note-for-running-older-years-when-cloning)
* [Running locally for when you clone/fork this project](https://github.com/p-kostic/AdventOfCode20xx#running-locally-for-when-you-clonefork-this-project)
    * [Running the project](https://github.com/p-kostic/AdventOfCode20xx#running-the-project)
* [Notes](https://github.com/p-kostic/AdventOfCode20xx#notes)
    * [Generating previous years solution files](https://github.com/p-kostic/AdventOfCode20xx#generating-previous-years-solution-files)
* [License](https://github.com/p-kostic/AdventOfCode20xx#license)

## Solution Completion Table
Note that there's a seperate repository for 2021 in **Haskell**: [p-kostic/AdventOfCode2021](https://github.com/p-kostic/AdventOfCode2021) for Days [1-11, 13p1] before I ragequit when I encountered "stateful" puzzles that require the extensive use of e.g. [Records](https://en.wikibooks.org/wiki/Haskell/More_on_datatypes) and/or [Lenses](https://www.haskellforall.com/2013/05/program-imperatively-using-haskell.html)).

| Day/Year | 2019 | 2020 | 2021        | 2022 |
|:--------:|:----:|:----:|:-----------:|:----:|
|     1    |  ❌ |  ✔️ |  ✔️/Haskell |   ✔️  |
|     2    |  ❌ |  ✔️ |  ✔️/Haskell |   ✔️  |
|     3    |  ❌ |  ✔️ |  Haskell     |   ✔️  |
|     4    |  ❌ |  ✔️ |  Haskell     |   ✔️  |
|     5    |  ❌ |  ✔️ |  Haskell     |   ❌  |
|     6    |  ❌ |  ✔️ |  Haskell     |   ❌  |
|     7    |  ❌ |  ✔️ |  Haskell     |   ❌  |
|     8    |  ❌ |  ✔️ |  Haskell     |   ❌  |
|     9    |  ❌ |  ✔️ |  Haskell     |   ❌  |
|    10    |  ❌ |  ✔️ |  Haskell     |   ❌  |
|    11    |  ❌ |  ✔️ |  Haskell     |   ❌  |
|    12    |  ❌ |  ✔️ |  ❌         |   ❌  |
|    13    |  ❌ |  ✔️ |  Haskell(p1) |   ❌  |
|    14    |  ❌ |  ✔️ |  ❌         |   ❌  |
|    15    |  ❌ |  ✔️ |  ❌         |   ❌  |
|    16    |  ❌ |  ✔️ |  ❌         |   ❌  |
|    17    |  ❌ |  ✔️ |  ❌         |   ❌  |
|    18    |  ❌ |  ✔️ |  ❌         |   ❌  |
|    19    |  ❌ |  ✔️ |  ❌         |   ❌  |
|    20    |  ❌ |  ✔️(p1) |  ❌         |   ❌  |
|    21    |  ❌ |  ✔️ |  ❌         |   ❌  |
|    22    |  ❌ |  ✔️ |  ❌         |   ❌  |
|    23    |  ❌ |  ✔️ |  ❌         |   ❌  |
|    24    |  ❌ |  ✔️ |  ❌         |   ❌  |
|    25    |  ❌ |  ✔️(p1) |  ❌         |   ❌  |

Template from [sindrekjr/AdventOfCodeBase](https://github.com/sindrekjr/AdventOfCodeBase). Below is some of the template's original documentation to get you started for when you want to run it locally. 

## Temporary note for running older years when cloning
Note: A [recent commit](https://github.com/p-kostic/AdventOfCode20xx/commit/ac0d6781f956d767602d385a22cb33c158e1769a) that pulls all recent changes made to the base template since 2021 - 2022 requires a mundane refactor task for the namespaces of of the solution files for years 2019-2021

## Running locally for when you clone/fork this project 
Create `config.json` with the following key/value pairs. If you run the program without adding a `config.json` file, one will be created for you without a cookie field. The program will not be able to fetch puzzle inputs from the web before a valid cookie is added to the configuration. 
```json
{
  "cookie": "c0nt3nt",
  "year": 2022,
  "days": [1] 
}
```

*  `cookie` - Note that `c0nt3nt` must be replaced with a valid cookie value that your browser stores when logging in at adventofcode.com. Instructions on locating your session cookie can be found here: https://github.com/wimglenn/advent-of-code-wim/issues/1. NOTE: the "session=" prefix must be excluded. 
*  `year` - Specifies which year you wish to output solutions for when running the project. Defaults to the current year if left unspecified.
*  `days` - Specifies which days you wish to output solutions for when running the project. Defaults to current day if left unspecified and an event is actively running, otherwise defaults to `0`.

The `days` field supports list comprehension syntax and strings, meaning the following notations are valid.
* `"1..4, 10"` - runs day 1, 2, 3, 4, and 10.
* `[1, 3, "5..9", 15]` - runs day 1, 3, 5, 6, 7, 8, 9, and 15.
* `0` - runs all days

### Running the project
Write your code solutions to advent of code within the appropriate day classes in the Solutions folder, and run the project. From the command line you may do as follows.
```
> cd AdventOfCode
> dotnet build
> dotnet run
```
Using `dotnet run` from the root of the repository will also work as long as you specify which project to run by adding `-p AdventOfCode`. Note that your `config.json` must be stored in the location from where you run your project.

## Notes
### Generating Previous Year's Solution Files
Use the included PowerShell script `AdventOfCode/UserScripts/GenerateSolutionFiles.ps1` to generate a year's solution files following the same layout as those already included.

Usage: `GenerateSolutionFiles.ps1 [-Year <Int>]`

If no value is provided it will generate files for the current year. The script will avoid overwriting existing files.  

Requires PowerShell v3 or later due to the way `$PSScriptRoot` behaves. If you have Windows 8+ you should be set. Upgrades for previous versions, and installs for macOS and Linux can be found in [Microsoft's Powershell Documentation](https://docs.microsoft.com/en-us/powershell/scripting/install/installing-powershell?view=powershell-7.1)

## License
[MIT](https://github.com/p-kostic/AdventOfCode20xx/blob/master/LICENSE.md)
