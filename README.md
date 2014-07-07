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
Retry Policy used for only retry specific Exception, RetryLib provide <i>WebRetryPolicy</i> and <i>IORetryPolicy</i>. Developer can implement IRetryPolicy for custom use.
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

###Handler Exception catched###
RetryLib provide event <i>OnExceptionCatch</i> to handle Exceptions occur in function.
```csharp
// Wait 2 sec, 4 sec, 8 sec...
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
