RetryLib
========

RetryLib is a .Net Library for C# Retry Operations. It allows developer to easy write retry code for Web/IO Trasient or custom Error. It also support async method.

Installing via NuGet
=
    Install-Package SK.RetryLib
    
Sample code
=
###Retry Action or Function###
```csharp
// Retry 3 times & wait 2 sec
Retry.ExecuteAction(
                () =>
                {
                    // Do something here.
                }, 3, 2 * 1000);
                
// Retry 3 times & wait 2 sec
Retry.ExecuteFunc(
                () =>
                {
                    // Do something here.
                    return XXX;
                }, 3);
```
