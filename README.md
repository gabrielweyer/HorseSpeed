# HorseSpeed

`HorseSpeed` will compute the `mean`, `median`, `standard deviation`, and `percentile(s)` of the `time-taken` field in an `IIS` log.

As the name indicates it, don't take this project too seriously. I wrote it because I didn't figure out a quick way to compute percentiles from an `IIS` log.

## Prerequisite

[Log Parser 2.2][log-parser] needs to be installed on your machine. It is free and you're most likely already using it anyway to query `IIS` logs.

## Usage

By default `HorseSpeed` will compute the `mean`, `median`, `standard deviation`, `50th percentile`, `70th percentile` and `95th percentile`.

```posh
HorseSpeed.exe "log-path" "where-clause"
```

- `log-path` could be `D:\u_ex160510.log`
- `where-clause` can be anything that is a valid `WHERE` clause. A basic example is `cs-uri-stem = '/legacy/SlowPage.aspx'`

## Custom percentiles

You can replace the default percentiles by your own:

```posh
HorseSpeed.exe "log-path" "where-clause" "5,50,99.99"
```

- This will compute the `5th`, `50th` and `99.99th percentile` (on top of the `mean`, `median` and `standard deviation`).

## Reading multiple files

The `log-path` argument supports two different modes:

- **File mode**: `D:\u_ex160510.log` or `D:\u_ex1605*.log` - will process all the files matching the filter and aggregate the result.
- **Directory mode**: `D:\` will read all the files with the `.log` extension in the `D:\` directory (excluding sub directories) and process them separately.

## Writing to file

The ordered (`ASC`) `time-taken` resulting from the query can be written to `.csv` files(s) so that you can generate all kind of crazy charts via `Excel`.

To enable this feature add a `WriteTimingFile` key with the value `True` to the `app.config`. It will then create `CSV` file(s) in the `.\out\` directory (relative to where the `exe` is located).

## What I dislike

I don't want to use any `NuGet` package except if I absolutely have to.

### Easy to fix

  Statistics class: orderedList. Should I order it again just in case?
- Some metrics are double, other are decimal. Does it make sense?

### Will require some thinking

Testability issues could be fixed by introducing a level of indirection on the file system. But I would be modifying the design to allow testing.

- 2 kinds of distinct options: from command line (from clause, where clause, percentiles) and from config file (should write timing file). Option from config file is read via function in `Program.cs`.
- `FileWriter` is not testable as it depends from `FileSystem`. It's an issue as `GenerateOutFilePath` has logic.
- `FileWriter` requires 2 calls to write output (as we want to know the generated filepath in order to inform the caller). I could return the filepath from `WriteTimingFile` but this would break the `Command Query` separation.
- `GetFromClauses` is hanging from `Program.cs` right now.
- `GetFromClauses` is not testable as it has a dependency on file system.

[log-parser]: https://www.microsoft.com/en-us/download/details.aspx?id=24659
