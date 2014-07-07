RetryLib
========

RetryLib is a .Net Library for C# Retry Operations. It allows developer to write retry code easily for Web/IO Trasient or custom error. It also supports async method.

Installing via NuGet
=
    Install-Package SK.RetryLib
    
Sample code
=
###Retry Action or Function###
Retry action or function if there's some exceptions.
```csharp
// Retry at most 3 times
Retry.Action(() =>
             {
                // Do something here.
             }, 3);
                
// Retry at most 3 times & wait 2 sec
var result = Retry.Func(() =>
             {
                // Do something here.
                return XXX;
             }, 3, 2 * 1000);
``` 
###Retry async method###
```csharp
var task = Retry.FuncAsync(
                async () =>
                {
                    WebRequest request = WebRequest.Create(@"http://www.bing.com/");
                    return await request.GetResponseAsync();
                }, 3);
task.Wait();
```
###Use Retry Policy###
Retry Policy is used for only retry specific Exception. <i>WebRetryPolicy</i> and <i>IORetryPolicy</i> are provided by RetryLib, and developer can implement IRetryPolicy for custom use.
```csharp
var task = Retry.FuncAsync(
                async () =>
                {
                    WebRequest request = WebRequest.Create(@"http://www.bing.com/");
                    return await request.GetResponseAsync();
                }, 3, retryPolicy: new WebRetryPolicy()); // Pass Retry Policy to task.
task.Wait();
```

###Retry Wait Type###
* <b>Linear</b>:   wait Xs, Xs, Xs....
* <b>Double</b>:   wait Xs, 2Xs, 4Xs....
* <b>Random</b>:   wait (0, Xs)
* <b>Zero</b>:   never wait

```csharp
// Wait 2 sec, 4 sec, 8 sec...
Retry.Func(() =>
            {
                // Do something here.
                return XXX;
            }, 4, 2 * 1000, waitType: RetryWaitType.Double);
``` 

###Exception Handler###
RetryLib provides event <i>OnExceptionCatch</i> to handle Exceptions in function.
```csharp
Retry.Func(() =>
            {
                // Do something here.
                return XXX;
            }, 4, 2 * 1000, 
            exceptionHandler: 
            (sender, exceptionArgs) => 
            {
                // Do something in exception handler
                record(exceptionArgs.Ex);
            });
```
