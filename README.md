Generic Windows Service
=======================

To get the windows service installed:
-------------------------------------

`cd c:\Users\mike.baranski\Documents\SharpDevelop Projects\RunProcess\GenericWindows Service\bin\Debug`

To install the service

`c:\windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe GenericWindowsService.exe`

To uninstall:

`c:\windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe /uninstall GenericWindowsService.exe`

Registry
--------

Registry key is in `HKLM\SOFTWARE\Wow6432Node\mikeski.net\GenericWindowsService`.  Under that key, create a key (or keys) for each process that the service will start.
Create the key (with an arbitrary name) and create a String value underneath is called `ConfigurationFile`.  The value of `ConfigurationFile` should be the path to an
xml configuration file

![Example Image](/registry.png)

Configuration File
------------------
Confugration files support the following values:

<table>
<tr><th>Field</th><th>Description</th></tr>
<tr>

  <td>
    proc
  </td>
  <td>
    The executable to run, generally the full path
  </td>

</tr>
<tr>

  <td>
    args
  </td>
  <td>
    Any arguments, all arguments as one string
  </td>

</tr>
<tr>

  <td>
    pwd
  </td>
  <td>
    The working directory to change into before executing the script
  </td>

</tr>
<tr>

  <td>
    user
  </td>
  <td>
    The user to run the process as
  </td>

</tr>
<tr>

  <td>
    pass
  </td>
  <td>
    The password
  </td>

</tr>
<tr>

  <td>
    dom
  </td>
  <td>
    The domain for the user account
  </td>

</tr>
</table>

Configuration files should have the following format, and for any *null* values just delete the XML element.  If you want a null user, pass and dom simply remove the entries for them.:

    <?xml version="1.0" encoding="utf-16"?>
    <HeadlessProgramConfiguration xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
      <proc>d:\LightTPD\LightTPD.exe</proc>
      <args>-D  -f conf\lighttpd.conf</args>
      <pwd>d:\LightTPD</pwd>
      <user>user</user>
      <pass>secret</pass>
      <dom>hiya.com</dom>
    </HeadlessProgramConfiguration>

For example, to run a process as the same user that the service is running as, you want the user, pass and dom to be null.  Your configuration file would look like this:

    <?xml version="1.0" encoding="utf-16"?>
    <HeadlessProgramConfiguration xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
      <proc>d:\LightTPD\LightTPD.exe</proc>
      <args>-D  -f conf\lighttpd.conf</args>
      <pwd>d:\LightTPD</pwd>
    </HeadlessProgramConfiguration>
