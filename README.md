Integration library for NET Standard 1.5 and above
==========================

This client library is used to manually report exceptions to OneTrueError (`OneTrue.Report(exception)`).

For more information about OneTrueError, check the [homepage](https://onetrueerror.com).

# Installation

1. Download and install the [OneTrueError server](https://github.com/onetrueerror/onetrueerror.server) or create an account at [OneTrueError.com](https://onetrueerror.com)
2. Install this client library (using nuget `onetrueerror.client.netstd`)
3. Configure the credentials from your OneTrueError account in your `Program.cs`.

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
		OneTrue.Report(ex, new{ UserId = uid, ForumPost = post });
	}
}
```

The context information will be attached as:

![](https://onetrueerror.com/images/features/custom-context.png)

[Read more...](https://onetrueerror.com/features/)

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

You need to either install [https://github.com/onetrueerror/onetrueerror.server](OneTrueError Community Server) or use [OneTrueError Live](https://onetrueerror.com/live).
