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
// Retry 3 times
Retry.Action(() =>
             {
                // Do something here.
             }, 3);
                
// Retry 3 times & wait 2 sec
Retry.Func(() =>
            {
                // Do something here.
                return XXX;
            }, 3, 2 * 1000);
``` 

###Retry Wait Type###
* RetryWaitType.Linear: wait Xs, Xs, Xs....
* * RetryWaitType.Linear: wait Xs, Xs, Xs....
```csharp
// Wait 2 sec, 4 sec, 8 sec...
Retry.Func(() =>
            {
                // Do something here.
                return XXX;
            }, 4, 2 * 1000, waitType = RetryWaitType.Double);
``` 
