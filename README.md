# A sample Camunda Client Class Library

This Class Library [Camunda Client](CamundaClient) is not meant to be a re-usable, stable component. It is meant to serve as example and can be a starting point for your own code. Feel free to copy and modify the code, as it is released under Apache License you can do what you want with the code.

It uses the [Camunda REST API](https://docs.camunda.org/manual/latest/reference/rest/) under the hood.

It is available as [NuGet Package: BerndRuecker.Sample.CamundaClient](https://www.nuget.org/packages/BerndRuecker.Sample.CamundaClient/0.0.2).

**This is just a sample developed for a showcase and not supported in any way.**

# Camunda C# examples using this library

* [C# Showcases: Insurance Application and others](https://github.com/berndruecker/camunda-csharp-showcase)
* [Flowing-retail resilience patterns](https://github.com/flowing/flowing-retail/tree/master/rest)


# Build NuGet Packages

As I always forget - here how I build NuGet Packages:

- Download nuget.exe from https://www.nuget.org/downloads
- Do a Release-Rebuild of the Visual Studio Project
- Run `nuget pack` in this directory
- Upload manually via https://www.nuget.org/packages/manage/upload