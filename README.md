Integration library for NET Standard 1.5 and above
==========================

[![VSTS](https://coderr.visualstudio.com/_apis/public/build/definitions/75570083-b1ef-4e78-88e2-5db4982f756c/3/badge)]() [![NuGet](https://img.shields.io/nuget/dt/codeRR.Client.NetStd.svg?style=flat-square)]()

This client library is used to manually report exceptions to codeRR (`OneTrue.Report(exception)`).

For more information about codeRR, check the [homepage](https://coderrapp.com).

# Installation

1. Download and install the [codeRR server](https://github.com/coderrapp/coderr.server) or create an account at [codeRR.com](https://coderrapp.com)
2. Install this client library (using nuget `coderr.client.netstd`)
3. Configure the credentials from your codeRR account in your `Program.cs`.

# Getting started

Simply catch an exception and report it:

```csharp
public void UpdatePost(int uid, ForumPost post)
{
	try
	{
		_service.Update(uid, post);
	}
	catch (Exception ex)
	{
		Err.Report(ex, new{ UserId = uid, ForumPost = post });
	}
}
```

The context information will be attached as:

![](https://coderrapp.com/images/features/custom-context.png)

[Read more...](https://coderrapp.com/features/)

#  Features in this library

* HTTP proxy detection and usage when error reports are uploaded.
* Queued uploads (to allow the application to still be responsive, even if uploading are done over a slow connection)
* Compressed upload to minimize bandwidth usage.
* Context data collection
* Custom context data
 * Anonymous object
 * View models etc
* Adding tags to errors
* Allow user to leave feedback
* Automated information collection from windows, the process and the current thread.


# Requirements

You need to either install [codeRR Community Server](https://github.com/coderrapp/coderr.server) or use [codeRR Live](https://coderrapp.com/live).
