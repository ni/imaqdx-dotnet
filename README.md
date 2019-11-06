# NI IMAQdx for .NET

#

NI IMAQdx for .NET is a collection of .NET wrapper functions that call the [IMAQdx](http://www.ni.com/en-us/shop/select/vision-acquisition-software) driver APIs.

IMAQdx support for 32-bit .NET was deprecated after version 2013 SP1.  New features added to IMAQdx after that version are not available for .Net users.

Now, IMAQdx for .NET is available as an open source and intends to help users build the 32-bit .NET binary NationalInstruments.Vision.Acquisition.Imaqdx.dll

This project intends to provide users the ability to add new .Net APIs and build custom binaries, for all the features available in IMAQdx.

# Dependencies

#

Users need to install the following dependencies to get started:

- Visual Studio – 2015 or later
- .NET - 4.5
- Vision Acquisition Software – 2019 or later

# Getting Started

#

The following steps should be executed in order to build and use an existing .NET API:

1. Clone from the master branch.

2. Open the niimaqdx_net.2015.sln from the src folder.

3. Make sure the target framework in the project-\>Properties-\>Application is set to ".NET Framework 4.5" for the imaqdx_net.2015 project in imaqdx_net.2015.sln.
![alt text](https://github.com/ni/imaqdx-dotnet/blob/master/image.png)

4. Make sure that the following references in the project are linked correctly: "NationalInstruments.Common.dll", "NationalInstruments.MStudioCLM.dll" and "NationalInstruments.NiLmClientDLL.dll", from \<Measurement Studio Installation directory\>\DotNET\Assemblies\Current.
Also link "NationalInstruments.Vision.Common.dll" from https://github.com/ni/vdm-dotnet/tree/master/src/dotNet/Exports/VS2008/DotNET/Assemblies/Current to niimaqdx_net.2015.csproj located in the src folder.

5. Build niimaqdx_net.2015.sln. This will create NationalInstruments.Vision.Acquisition.Imaqdx.dll in src\bin\x86\Release or src\bin\x86\Debug.

6. Link the dlls created in step 5 to your application.

# Examples

#

Run any of the IMAQdx examples in the Examples folder to get started. For example:

1. Open examples\Grab\Grab.2015.sln in Visual Studio 2015.
2. Navigate to Solution Explorer -\> References.
3. Modify the following references in order to pick them from the right path in your system:
    1. NationalInstruments.Common.dll
    2. NationalInstruments.MStudioCLM.dll
    3. NationalInstruments.NiLmClientDLL.dll
    4. NationalInstruments.Vision.Acquisition.Imaqdx.dll
    5. NationalInstruments.Vision.Common.dll

For the three first assemblies, make sure they point to the following folder: \<Measurement Studio Installation directory\>\DotNET\Assemblies\Current.
For "NationalInstruments.Vision.Acquisition.Imaqdx.dll", point it to where you have build the imaqdx_net.2015 assembly.
For "NationalInstruments.Vision.Common.dll", point it to https://github.com/ni/vdm-dotnet/tree/master/src/dotNet/Exports/VS2008/DotNET/Assemblies/Current.

4. Build the example project for x86 Release.
5. Navigate to the location where the example executable gets created (look for the path in output window while building the executable).
6. Launch the example executable from the path in step 5 and run.

# Bug Reports

#

To report a bug specific to vdm-dotnet, please use the [Github Issues page](https://github.com/ni/imaqdx-dotnet/blob/master/.github/ISSUE_TEMPLATE.md).

Fill in the issue template as completely as possible.

# Contributing

#

We welcome contributions! Please refer to [CONTRIBUTING.md](https://github.com/ni/imaqdx-dotnet/blob/master/CONTRIBUTING.md) page for information on how to contribute as well as what tests to add.

# License

#


**imaqdx-dotnet** is licensed under the [MIT License](https://github.com/ni/imaqdx-dotnet/blob/master/LICENSE)
