# dwca-codegen Documentation

dwca-codegen is a dotnet command line tool that Darwin Core Archive meta-data to generate class files for mapping biodiversity datasets to strongly typed classes.

## Installation

### Local Install

> :bulb: **NOTE:**  Note: To install a tool locally you must first create a [tool manifest as described here](https://docs.microsoft.com/en-us/dotnet/core/tools/global-tools)

To install the dwca-codegen tool locally open a command prompt in your project folder and enter

```
dotnet tool install dwca-codegen
```

### Global Install

To install dwca-codegen globally open a command prompt and enter

```
dotnet tool install -g dwca-codegen
```    

## Configuration

The dwca-codegen tool accepts command line parameters and/or options from a [dotnet-config](https://dotnetconfig.org/) configuration file.  For information on dotnet-config and the command line tool for manipulating dotnet-config configuration files see https://dotnetconfig.org/.

### Initial Configuration File

To create a new default configuration file use the following command.

```
dwca-codegen config init
```
This will create a file named .dwca-codegen in one of the following locations depending on whether the tool was installed locally or globally.

|Installation Location|File Location|
|---|---|
|Local|Current working directory|
|Global|The dwca-codegen folder in the user's profile directory|

The layout of the file will appear similar to the following

```
# Sample default dwca-codegen configuration file

[dwca-codegen]
	pascalCase
	termAttribute = none
	namespace = DwC
	output = .
	mapMethod = false
[dwca-codegen "usings"]
	using = System
[properties "*"]
	typeName = string
	include
[properties "http://rs.tdwg.org/dwc/terms/coordinatePrecision"]
	typeName = double?
	include
[properties "http://rs.tdwg.org/dwc/terms/coordinateUncertaintyInMeters"]
	typeName = double?
	include
[properties "http://rs.tdwg.org/dwc/terms/dateIdentified"]
	typeName = DateTime?
	include
[properties "http://rs.tdwg.org/dwc/terms/day"]
	typeName = int?
	include
[properties "http://rs.tdwg.org/dwc/terms/decimalLatitude"]
	typeName = double?
	include
    propertyName = Latitude
[properties "http://rs.tdwg.org/dwc/terms/decimalLongitude"]
	typeName = double?
	include
    propertyName = Longitude
```
The following explains some of the options and what they do

|Section|Option|Type|Description|
|---|---|---|---|
|**dwca-codegen**|pascalCase|boolean|When true the name of the generated class and properties will be capitalized|
||termAttribute|none/name/index|When name or index this will add a Term attribute to each property along with the term or index that should be mapped to that property (e.g. [Term("http://rs.tdwg.org/dwc/terms/day")]).|
||namespace|string|Sets the namespace for the generated class|
||output|string|Output path to write generated class files to|
||mapMethod|boolean|Adds a static mapRow method to the class definition to assign values from an IRow to each property of the class|
|**dwca-codegen usings**|using|string|This is a list of usings that should be added to the generated source file. **Note:** If termAttribute is set to name or index then the DwC_A.Terms namespace will be added to the usings.
|**properties**||string|This is a list of property definitions for each term.  The name of the property should be the term IRI for that property. See [List of Darwin Core terms](https://dwc.tdwg.org/list/) for a list of term IRIs.  Use the __*__ wildcard to set a default type for all other terms.  Setting the include property to **false** for this property will exclude all undefined terms from the generated class definition|
||typeName|string|Type that should be used for this property.  If terms are not defined here they will default to type string|
||include|boolean|Should this property be included in the generated source or not|
||propertyName|string|By default properties are named after the label name of the associated term IRI but this field can be used to override that naming scheme.|

### Configuration Listing

To list the current configuration use the following command.

```
dwca-codegen config list
```

## Usage

To generate classes for an existing archive use the generate command as follows.

```
dwca-codegen -o code <path to archive file or folder>
```
This will generate source files for each file in the archive and write them to a subdirectory named `code`.  The output will list the current configuration, including any options that have been overriden on the command line, and list the files that have been created as so.

```
Generating files for archive .\Data\dwca-mvzobs_bird-v34.48 using configuration:
Config File: .dwca-codegen
Namespace:  DwC
PascalCase: True
Term Attribute: none
Output:     code
Usings:

Name      Type      Include Term
--------------------------------------------------------------------------------
          string    True    *
          double?   True    http://rs.tdwg.org/dwc/terms/coordinatePrecision
          double?   True    http://rs.tdwg.org/dwc/terms/coordinateUncertaintyInMeters
          DateTime? True    http://rs.tdwg.org/dwc/terms/dateIdentified
          int?      True    http://rs.tdwg.org/dwc/terms/day
          double?   True    http://rs.tdwg.org/dwc/terms/decimalLatitude
          double?   True    http://rs.tdwg.org/dwc/terms/decimalLongitude
          int?      True    http://rs.tdwg.org/dwc/terms/endDayOfYear
          double?   True    http://rs.tdwg.org/dwc/terms/footprintSpatialFit
          int?      True    http://rs.tdwg.org/dwc/terms/individualCount
          double?   True    http://rs.tdwg.org/dwc/terms/maximumDepthInMeters
          double?   True    http://rs.tdwg.org/dwc/terms/maximumDistanceAboveSurfaceInMeters
          DateTime? True    http://rs.tdwg.org/dwc/terms/measurementDeterminedDate
          double?   True    http://rs.tdwg.org/dwc/terms/minimumDistanceAboveSurfaceInMeters
          double?   True    http://rs.tdwg.org/dwc/terms/minimumElevationInMeters
          int?      True    http://rs.tdwg.org/dwc/terms/month
          int?      True    http://rs.tdwg.org/dwc/terms/namePublishedInYear
          double?   True    http://rs.tdwg.org/dwc/terms/pointRadiusSpatialFit
          DateTime? True    http://rs.tdwg.org/dwc/terms/relationshipEstablishedDate
          int?      True    http://rs.tdwg.org/dwc/terms/sampleSizeValue
          int?      True    http://rs.tdwg.org/dwc/terms/startDayOfYear
          int?      True    http://rs.tdwg.org/dwc/terms/year
Created code\Occurrence.cs
Created code\Multimedia.cs
```

The above example would create class files for Occurrence and Multimedia.  The Multimedia file would be a simple [POCO](https://en.wikipedia.org/wiki/Plain_old_CLR_object) file as fillows.

```csharp
namespace DwC
{
    public partial class Multimedia
    {
        public string Id { get; set; }
        public string Identifier { get; set; }
        public string Type { get; set; }
        public string Type1 { get; set; }
        public string Title { get; set; }
        public string Modified { get; set; }
        public string WebStatement { get; set; }
        public string Source { get; set; }
        public string Source1 { get; set; }
        public string Creator { get; set; }
        public string Creator1 { get; set; }
        public string Description { get; set; }
        public string CreateDate { get; set; }
        public string AccessURI { get; set; }
        public string Format { get; set; }
        public string Format1 { get; set; }
        public string FurtherInformationURL { get; set; }
    }
}
```
If the termAttribute option is set to name then the `[Term()]` attribute will be added to the properties as follows.

```
dwca-codegen -o code -t name ./data/dwca-mvzobs_bird-v34.48.zip
```

```csharp
namespace DwC
{
    using DwC_A.Attributes;

    public partial class Multimedia
    {
        [Term("id")]
        public string Id { get; set; }
        [Term("http://purl.org/dc/terms/identifier")]
        public string Identifier { get; set; }
        [Term("http://purl.org/dc/elements/1.1/type")]
        public string Type { get; set; }
        [Term("http://purl.org/dc/terms/type")]
        public string Type1 { get; set; }
        [Term("http://purl.org/dc/terms/title")]
        public string Title { get; set; }
        [Term("http://purl.org/dc/terms/modified")]
        public string Modified { get; set; }
        [Term("http://ns.adobe.com/xap/1.0/rights/WebStatement")]
        public string WebStatement { get; set; }
        [Term("http://purl.org/dc/elements/1.1/source")]
        public string Source { get; set; }
        [Term("http://purl.org/dc/terms/source")]
        public string Source1 { get; set; }
        [Term("http://purl.org/dc/elements/1.1/creator")]
        public string Creator { get; set; }
        [Term("http://purl.org/dc/terms/creator")]
        public string Creator1 { get; set; }
        [Term("http://purl.org/dc/terms/description")]
        public string Description { get; set; }
        [Term("http://ns.adobe.com/xap/1.0/CreateDate")]
        public string CreateDate { get; set; }
        [Term("http://rs.tdwg.org/ac/terms/accessURI")]
        public string AccessURI { get; set; }
        [Term("http://purl.org/dc/elements/1.1/format")]
        public string Format { get; set; }
        [Term("http://purl.org/dc/terms/format")]
        public string Format1 { get; set; }
        [Term("http://rs.tdwg.org/ac/terms/furtherInformationURL")]
        public string FurtherInformationURL { get; set; }
    }
}
```
