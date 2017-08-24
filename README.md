Integration library for NET Standard 1.5 and above
==========================

This client library is used to manually report exceptions to OneTrueError (`OneTrue.Report(exception)`).

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

# Getting started

1. Download and install the [OneTrueError server](https://github.com/onetrueerror/onetrueerror.server) or create an account at [OneTrueError.com](https://onetrueerror.com)
2. Install this client library (using nuget `onetrueerror.client.netstd`)
3. Configure the credentials from your OneTrueError account in your `Program.cs`.

Done.
