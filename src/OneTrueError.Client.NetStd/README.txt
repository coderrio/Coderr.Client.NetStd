OneTrueError - .NET Standard Client
=====================================

Congratulations on taking the first step toward a more efficient exception handling.

Now you need to either download and install the open source server: https://github.com/onetrueerror/OneTrueError.Server/
.. or create an account at https://onetrueerror.com.

Once done, log into the server and find the configuration instructions.
(Or read the articles in our documentation: https://onetrueerror.com/documentation)

Questions & Feedback:
http://discuss.onetrueerror.com/

Source code:
https://github.com/onetrueerror/OneTrueError.Client.NetStandard


Reporting exceptions
====================

public void SomeMethod(int userId, int postId)
{
  try
  {
     // [...some code...]
  }
  catch (Exception ex)
  {
    OneTrue.Report(ex, new { userId, postId });
  }
}
