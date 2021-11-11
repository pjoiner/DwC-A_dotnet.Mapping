# DwC-A_dotnet.Mapping Documentation

DwC-A_dotnet.Mapping is an extension library of DwC-A_dotnet for mapping Darwin Core Archive data to strongly typed classes.

## Install

To add DwC-A_dotnet.Mapping to your project run the following command in the Visual Studio Package Manager Console

    PM> Install-Package DwC-A_dotnet.Mapping

## Usage

There are two methods of mapping an IRow to a strongly typed class

1. Using a lambda or delegate.
2. Using the Term attribute.

### Lambda Mapping

This technique gives the most control over mapping but requires the mapper to be configured for your particular class as follows.

Assuming the following simple occurrence class type

```csharp
public class Occurrence
{
    public string Id {get; set;}
    public string ScientificName {get; set;}
    public double Longitude {get; set;}
    public double Latitude {get; set;}
}
```

A mapper should be set up as follows

```csharp
IMapper<Occurrence> mapper = new LambdaMapper<Occurrence>((o, row) =>
{
    o.Id = row["id"];
    o.DecimalLatitude = double.Parse(row[Terms.decimalLatitude]);
    o.DecimalLongitude = double.Parse(row[Terms.decimalLongitude]);
    o.ScientificName = row[Terms.scientificName];
});
```

The mapper can then be used to map rows from an occurrence file as follows.

```csharp
using DwC_A;
using DwC_A.Mapping;
using DwC_A.Terms;

using(ArchiveReader archive = new ArchiveReader("archivePath"))
{
    foreach(var row in archive.CoreFile.DataRows)
    {
        var occurrence = row.Map<Occurrence>(mapper);
        ...
    }
}
```

### Attribute Mapping

This method uses classes that have been annotated with the Term attribute to associate fields with properties.

First create a class and add Term attributes to each property that you wish to be mapped.

```csharp
using DwC_A.Attributes;

public class Occurrence
{
    [Term("id")]
    public string Id {get; set;}
    [Term("http://rs.tdwg.org/dwc/terms/scientificName")]
    public string ScientificName {get; set;}
    [Term("http://rs.tdwg.org/dwc/terms/decimalLongitude")]
    public double Longitude {get; set;}
    [Term("http://rs.tdwg.org/dwc/terms/decimalLatitude")]
    public double Latitude {get; set;}
}
```

The DwC-A_dotnet library then uses a Source Generator to add a map method to the annotated class which can be used as follows.

```csharp
using DwC_A;
using DwC_A.Mapping;
using DwC_A.Terms;

using(ArchiveReader archive = new ArchiveReader("archivePath"))
{
    foreach(var row in archive.CoreFile.DataRows)
    {
        var occurrence = new Occurrence();
        occurrence.Map(row);
        ...
    }
}
```



