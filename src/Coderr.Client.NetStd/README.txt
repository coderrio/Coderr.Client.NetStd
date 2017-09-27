codeRR - .NET Standard Client
=====================================

Congratulations on taking the first step toward a more efficient exception handling.

Now you need to either download and install the open source server: https://github.com/coderrapp/codeRR.Server/
.. or create an account at https://coderrapp.com.

Once done, log into the server and find the configuration instructions.
(Or read the articles in our documentation: https://coderrapp.com/documentation)

Questions & Feedback:
http://discuss.coderrapp.com/

Source code:
https://github.com/coderrapp/codeRR.Client.NetStandard


Reporting exceptions
====================

This is one of many many examples ;)

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

Find more in our documentation.