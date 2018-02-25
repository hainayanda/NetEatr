<p align="center">
  <img width="128" height="192" src="net.eatr.png"/>
</p>

# NetEatr
RESTful web service consumer for .Net

---
## Changelog
for changelog check [here](CHANGELOG.md)

---
## Features

- [x] Builder pattern
- [x] Auto encode param and form url encoded body
- [x] Auto parsed raw body from stream object to String
- [x] Auto parsed JSON using Newtonsoft library
- [x] Support synchronous or asynchronous operation
- [x] Support progress observer

---
## Requirements

- .Net Standar 2.0

---
## Installation
### Nuget
1. Go to Nuget package manager on visual studio
2. Search NetEatr
3. Install

### Package Manager
run this on package manager console

```
Install-Package NetEatr -Version 0.0.3
```
### .Net CLI
run this on .Net CLI

```
dotnet add package NetEatr --version 0.0.3
```
### Manually
1. Clone this repository.
2. Added to your project.
3. Congratulation!

## Usage Example
## Synchronous
Build the object using HttpRequestBuilder and then execute  
If your response contains some Json, you can pass your Model as Generic attribute

```cs
//HttpGet
Response response = HttpRequestBuilder.HttpGet.SetUrl("http://your.url.here")
    .AddHeaders("SOME-HEADER", "header_value").AddParam("param_key", "param_value").AwaitExecute();
    
boolean isSuccess = response.IsSuccess;
boolean hadException = response.hadException;
int statusCode = response.StatusCode;
string body = response.RawBody;

//HttpPost with Json response and Json body
RestResponse<Model> response = HttpRequestBuilder.HttpPost.SetUrl("http://your.url.here")
    .AddHeaders("SOME-HEADER", "header_value").AddParam("param_key", "param_value")
    .AddJsonBody(someObject).AwaitExecute<Model>();

Model responseObj = response.JsonBody;
```

### Simple Asynchronous
Everything is same like synchronous, but instead of call AwaitExecute, call AsyncExecute  
You can even pass onFinished delegate to run after request is finished

```cs
//Basic
HttpRequestBuilder.HttpGet.SetUrl("http://your.url.here")
    .AddHeaders("SOME-HEADER", "header_value").AddParam("param_key", "param_value")
    .AsyncExecute();
    
//With delegate
HttpRequestBuilder.HttpGet.SetUrl("http://your.url.here")
    .AddHeaders("SOME-HEADER", "header_value").AddParam("param_key", "param_value")
    .AsyncExecute((response) => {
        //YOUR CODE HERE
        // WILL BE EXECUTE AFTER REQUEST IS FINISHED
    });

//Using RestResponse
HttpRequestBuilder.HttpGet.SetUrl("http://your.url.here")
    .AddHeaders("SOME-HEADER", "header_value").AddParam("param_key", "param_value")
    .AsyncExecute<Model>((restResponse) => {
        //YOUR CODE HERE
        // WILL BE EXECUTE AFTER REQUEST IS FINISHED
        //restResponse is RestResponse<Model> object
    });
```

### Advanced
Same like any http request, but you can set your delegate consumer separately. you can set 5 consumer:
- SetOnProgress which will run for every progress, it will give the progress in float start from 0.0f to 1.0f  
Because this method will called periodically, its better if you're not put object creation inside this method
- SetOnBeforeSending which will run before sending
- SetOnResponded which will **ONLY** run when you get response
- SetOnTimeout which will **ONLY** run when you get no response after timeout
- SetOnException which will **ONLY** run when you get unhandled exception

You don't need to set all of the consumer. just the one you need.
Is up to you if you want to execute it using AsyncExecute or AwaitExecute

```cs
HttpRequestBuilder.HttpPost.SetUrl("http://your.url.here")
    .AddHeaders("SOME-HEADER", "header_value").AddParam("param_key", "param_value")
    .AddJsonBody(someObject)
    .SetOnTimeout(() => {
        //YOUR CODE HERE
    })
    .SetOnBeforeSending((httpWebRequest) => {
        //httpWebRequest is HttpWebRequest object which will executed before sending
        //You can manipulate this before sending
        //YOUR CODE HERE
    })
    .SetOnException((exception) => {
        //YOUR CODE HERE
    })
    .SetOnProgress((progress) => {
        //progress is float range from 0 - 1
        //YOUR CODE HERE
    })
    .SetOnResponded((response) => {
        //response is Response object
        //if you pass Generic parameter it will be RestResponse<Parameter> object
    })
    .AsyncExecute((response) => {
        //YOUR CODE HERE
        // WILL BE EXECUTE AFTER REQUEST IS FINISHED
    })
```
---
## Contribute
We would love you for the contribution to **NetEatr**, just contact me to nayanda1@outlook.com or just pull request
