# Minimal APIs in ASP.NET 9

<a href="https://www.packtpub.com/en-us/product/minimal-apis-in-aspnet-9-9781805129127"><img src="https://content.packt.com/_/image/original/B20968/cover_image.jpg" alt="Book Name" height="256px" align="right"></a>

This is the code repository for [Minimal APIs in ASP.NET 9](https://www.packtpub.com/en-us/product/minimal-apis-in-aspnet-9-9781805129127), published by Packt.

**Design, implement, and optimize robust APIs in C# with .NET 9**

## What is this book about?
This book offers developers a holistic understanding of minimal API development in .NET. You’ll start with concepts like data mapping and end your journey with performance optimization, all illustrated with practical examples.

This book covers the following exciting features:
* Become proficient in minimal APIs within the .NET Core 9 framework
* Find out how to ensure scalability, performance, and maintainability
* Work with databases and ORMs, such as Entity Framework and Dapper
* Optimize minimal APIs, including asynchronous programming, caching strategies, and profiling tools
* Implement advanced features like dependency injection, request validation, data mapping, and routing techniques
* Create and configure minimal API projects effectively

If you feel this book is for you, get your [copy](https://www.amazon.com/Minimal-APIs-ASP-NET-Core-applications/dp/1805129120) today!

<a href="https://www.packtpub.com/?utm_source=github&utm_medium=banner&utm_campaign=GitHubBanner"><img src="https://raw.githubusercontent.com/PacktPublishing/GitHub/master/GitHub.png" 
alt="https://www.packtpub.com/" border="5" /></a>


## Instructions and Navigations
All of the code is organized into folders. For example, Chapter02 - Creating your first Minimal API.

The code will look like the following:
```
app.MapPut("/employees", (Employee employee) =>
{
    EmployeeManager.Update(employee);
    return Results.Ok();
});
```

**Following is what you need for this book:**
If you’re a generalist developer looking for a fresh perspective on API development with an emphasis on minimalism, then this book is for you. Aimed at intermediate developers, this book strikes the right balance between accessibility and depth. The book assumes an intermediate level of C# and .NET knowledge, while providing sufficient guidance and explanations to help you progress confidently through the chapters.

With the following software and hardware list you can run all code files present in the book (Chapter 1-14).

### Software and Hardware List

| Chapter  | Topics covered in the book                   | Required Skill Level                        |
| -------- | ------------------------------------| -----------------------------------|
| 1-14        | C#                     | .NET 9 SDK (Soft ware Development Kit) |
| 1-14        | SQL            | Microsoft SQL Server |
| 1-14        | MongoDB Server            | Microsoft SQL Server Management Studio |
| 1-14        | MongoDB Compass            | None – setup and confi guration for Minimal APIs is covered in the book |
| 1-14        | Visual Studio 2022            | Basic |
| 61-14        | Visual Studio Code            | Basic (if used – Visual Studio can be used alternatively) |
| 1-14        | Object-Oriented Programming            | Basic |


### Related products
* Web API Development with ASP.NET Core 8 [[Packt]](https://www.packtpub.com/en-us/product/web-api-development-with-aspnet-core-8-9781804610954) [[Amazon]](https://www.amazon.com/Web-Development-ASP-NET-Core-high-performance/dp/180461095X)

* ASP.NET 8 Best Practices [[Packt]](https://www.packtpub.com/en-in/product/aspnet-8-best-practices-9781837632121) [[Amazon]](https://www.amazon.com/ASP-NET-Best-Practices-Jonathan-Danylko/dp/183763212X)

## Get to Know the Author
**Nick Proud** is a software engineer, technology leader, and Microsoft MVP for Developer Technologies, specializing in robotic process automation and .NET. He is currently the director of soft ware engineering at NexBotix an intelligent automation firm, as well as a technical content creator, producing educational video content about C# and Microsoft Azure.
