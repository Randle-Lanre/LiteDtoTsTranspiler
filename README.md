LiteDtoTsTranspiler
LiteDtoTsTranspiler is a simple and lightweight tool that converts C# data transfer objects (DTOs) to TypeScript interfaces. It can help you to keep your front-end and back-end models in sync, and avoid manual errors and inconsistencies.

Installation and usage
To install LiteDtoTsTranspiler, you need to have .NET 7 or higher installed on your machine. You can download the latest release of LiteDtoTsTranspiler from the releases page or clone this repository and build it yourself.

To use LiteDtoTsTranspiler, you need to provide the path to the folder that contains your C# DTOs and the path to the output folder where you want to save the generated TypeScript interfaces. For example, you can run the following command:





https://github.com/Randle-Lanre/LiteDtoTsTranspiler/assets/19264390/0feff782-53dc-4154-b9de-b2d5fc69587d



## installation 


TODO

___

## Description

This will scan the input folder for any C# files that contain DTOs and generate corresponding TypeScript interfaces in the output folder. The generated interfaces will have the same name as the DTOs, but with a .ts extension. For example, if you have a C# file called UserDto.cs that contains the following DTO:

```C#


public class UserDto
{
public int Id { get; set; }
public string Name { get; set; }
public string Email { get; set; }
public DateTime Birthday {get; set;}
}
```
LiteDtoTsTranspiler will generate a TypeScript file called UserDto.ts that contains the following interface:

```TypeScript


export interface UserDto {
id: number,
name: string,
email: string,
birthday: Date,
}
```
## LiteDtoTsTranspiler is currently in active development and has the following features:

LiteDtoTsTranspiler supports the following C# types and their TypeScript equivalents:



    C# type	TypeScript type
    bool	boolean
    byte	number
    sbyte	number
    decimal	number
    double	number
    float	number
    int	number
    uint	number
    long	number
    string	string
    DateTime	Date
    char	string
    ulong	number
    short	number
    ushort	number
    object	any
    dynamic	any
    Guid	string
    Enum	string
## Roadmap


    DateTimeOffset	Date
    Nullable<T>	T | null
    List<T>	T[]
    Array<T>	T[]
    Dictionary<K, V>	Record<K, V>

- Supports nested DTOs and circular references
-  Supports nullable types and optional properties
- Supports custom attributes to control the generation process
-  Supports configuration file to specify global options

### The following features are planned for future releases:

- Support more C# types and collections
- Support generics and inheritance
- Support custom naming conventions and formatting
- Support comments and documentation
- Support command-line arguments and interactive mode
- Support GUI and web interface
