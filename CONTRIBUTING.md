#
# Contributing to imaqdx-dotnet

Contributions to imaqdx-dotnet are welcome from all!

imaqdx-dotnet is managed via [git](https://git-scm.com/), with the canonical upstream repository hosted on [GitHub](https://github.com/ni/imaqdx-dotnet/).

imaqdx-dotnet follows a pull-request model for development. If you wish to contribute, you will need to create a GitHub account, fork this project, push a branch with your changes to your project, and then submit a pull request.

See [GitHub&#39;s official documentation](https://help.github.com/articles/using-pull-requests/) for more details.

# Getting Started

The following steps describe the process of adding new APIs:

1. New attributes needed for quick access can be added to the ImaqdxAttributeManager in src/Internal/ImaqdxAttributeManager.cs

2. New functions operating on a session can be added to the imaqdxSessionManager class in src/Internal/ImaqdxSessionManager.cs and src/ImaqdxSession.cs

3. New DLL entry points must be added to the NiImaqdxDll class in src/Internal/NiImaqdxDll.cs
Refer to C:\Program Files (x86)\National Instruments\NI-IMAQdx\include\NIIMAQdx.h for the latest C API. 
Follow step 2 to add the function created from the new entry point to the API.

# Testing

Add an xunit test in the test solution &quot;IMAQdxTestLibrary&quot; located in the tests/IMAQdxTestLibrary folder. Refer to the already existing test template in IMAQdxTests.cs to add new tests. For more information on getting started with xunit, please refer to:

 [https://xunit.net/](https://xunit.net/)

 [https://xunit.net/docs/getting-started/netfx/visual-studio](https://xunit.net/docs/getting-started/netfx/visual-studio)

# Developer Certificate of Origin (DCO)

Developer&#39;s Certificate of Origin 1.1

By making a contribution to this project, I certify that:

(a) The contribution was created in whole or in part by me and I have the right to submit it under the open source license indicated in the file; or

(b) The contribution is based upon previous work that, to the best of my knowledge, is covered under an appropriate open source license and I have the right under that license to submit that work with modifications, whether created in whole or in part by me, under the same open source license (unless I am permitted to submit under a different license), as indicated in the file; or

(c) The contribution was provided directly to me by some other person who certified (a), (b) or (c) and I have not modified it.

(d) I understand and agree that this project and the contribution are public and that a record of the contribution (including all personal information I submit with it, including my sign-off) is maintained indefinitely and may be redistributed consistent with this project or the open source license(s) involved.

(From [developercertificate.org](https://developercertificate.org/))

See [LICENSE](https://github.com/ni/imaqdx-dotnet/blob/master/LICENSE) for details about how imaqdx-dotnet is licensed.
