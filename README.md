# codessentials.CGM

[![NuGet](https://img.shields.io/nuget/v/codessentials.CGM.svg)](https://nuget.org/packages/codessentials.CGM/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
[![Build Status](https://github.com/twenzel/CGM/workflows/Build/badge.svg?branch=master)](https://github.com/twenzel/CGM/actions)

[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=twenzel_CGM&metric=sqale_rating)](https://sonarcloud.io/dashboard?id=twenzel_CGM)
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=twenzel_CGM&metric=reliability_rating)](https://sonarcloud.io/dashboard?id=twenzel_CGM)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=twenzel_CGM&metric=security_rating)](https://sonarcloud.io/dashboard?id=twenzel_CGM)
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=twenzel_CGM&metric=bugs)](https://sonarcloud.io/dashboard?id=twenzel_CGM)
[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=twenzel_CGM&metric=vulnerabilities)](https://sonarcloud.io/dashboard?id=twenzel_CGM)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=twenzel_CGM&metric=coverage)](https://sonarcloud.io/dashboard?id=twenzel_CGM)

This library reads CGM (Computer Graphics Metafile) in binary and clear text format. Read graphics can be modified, analyzed and exported. Creating new graphics is also supported.
It implements the `ISO/IEC 8632-3:1999` and `ISO/IEC 8632-4:1999` specification.

Some additional functions for reading technical documentation items (Figures, Names) etc are also implemented.

## Install
Add the NuGet package [codessentials.CGM](https://nuget.org/packages/codessentials.CGM/) to any project supporting .NET Standard 2.0 or higher.

> &gt; dotnet add package codessentials.CGM

## Usage

### Write new CGM files
```CSharp
var writer = new CGMWriter(FileFormat.Binary);
writer.SetDescription("Created By UnitTest");
writer.SetElementList("DRAWINGPLUS");
writer.SetFontList(new[] { "Arial", "Arial Bold" });
writer.SetCharacterSetList(new[] { new KeyValuePair<CharacterSetList.Type, string>(CharacterSetList.Type._94_CHAR_G_SET, "B"), new KeyValuePair<CharacterSetList.Type, string>(CharacterSetList.Type._96_CHAR_G_SET, "A"), new KeyValuePair<CharacterSetList.Type, string>(CharacterSetList.Type.COMPLETE_CODE, "I"), new KeyValuePair<CharacterSetList.Type, string>(CharacterSetList.Type.COMPLETE_CODE, "L") });
writer.SetVDCType(VDCType.Type.Real);
// add several "drawing" commands
writer.AddCommand(...)

//
writer.Finish();

var data = writer.GetContent();
```

### Read & write binary CGM
```CSharp
var cgm = new BinaryCGMFile("corvette.cgm");

// modify graphic

cgm.WriteFile();
```

### Convert binary to clear text format
```CSharp
var binaryFile = new BinaryCGMFile("corvette.cgm");

var cleanTextFile = new ClearTextCGMFile(binaryFile);
var content = cleanTextFile.GetContent();
```

### `CGMFile` Helper functions
Name|Description
-|-
ContainsTextElement|Determines whether any text element equals the specified text.
GetMetaTitle|Gets the meta data title.
GetGraphicName|Gets the title of the illustration.
GetFigureItemTexts|Gets all texts of the figure items.
ContainsFigureItemText|Determines whether CGM contains a specific figure item text.
GetRectangles|Gets all found rectangles.

### Geometry Recognition Engine
The class `GeometryRecognitionEngine` provides several functions to find rectangles.

Name|Description
-|-
GetRectangles | Gets all rectangles of the given file.
IsNearBy | Determines whether point A is near point b.
