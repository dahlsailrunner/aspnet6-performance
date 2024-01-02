# CarvedRock.Aot.Api

This is an example of the API for the Carved Rock catalog that uses
[Ahead of Time compilation (Native AOT)](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/native-aot?view=aspnetcore-8.0).

Here are a few notes:

- Could not use the Hellang.Middleware.ProblemDetails package because it causes a runtime
error (could not resolve ObjectResponseFormatter).
- Needed to add a [compiled model](https://learn.microsoft.com/en-us/ef/core/performance/advanced-performance-topics?tabs=with-di%2Cexpression-api-with-constant#compiled-models
to the EF core context. I did this by running the following command in the `CarvedRock.Data` project folder:
`dotnet ef dbcontext optimize`.
- Needed to add a constructor to the `LocalContext that takes a `DbContextOptions` parameter.
- Pivoted to Dapper.AOT (see notes below) for the query.

You cannot to `RemoveRange` on a `DbSet` -- that gives a "model building is not supported" error.
This means that the `MigrateAndCreateData` should have already been done by some other means.

If you just wanted to `Migrate` that's fine.  But the `RemoveRange` doesn't work.

## Running the Project

Make sure you run the other API project first -- that will seed the database with some data.  It should not be running
at the same time as the AOT project.  Once the API has started up, you can stop that project.

Then to run the AOT project, any method should work (run from Visual Studio or other IDE, plus `dotnet run`).

To publish the project, use `dotnet publish`.  That will create an `exe` in the `bin` folder.  You can run that
`exe` from the command line.

## Dapper.AOT

I pivoted to [Dapper.AOT](https://aot.dapperlib.dev/gettingstarted) because EF Core still has more features to enable in order to
work with Native AOT. Dynamic queries (like `ctx.Products.FindAsync(id)`) are not supported at this point.

By switching to Dapper.AOT, I was able to get the API to work with Native AOT (from `dotnet publish`).
  
The API still works within Visual Studio with EF Core, just not at runtime from `dotnet publish`.

## "Desktop Development with C++" Workload is Required

```
C:\Users\dahls\.nuget\packages\microsoft.dotnet.ilcompiler\8.0.0\build\Microsoft.NETCore.Native.Windows.targets(123,5):
 error : Platform linker not found. Ensure you have all the required prerequisites documented at https://aka.ms/nativea
ot-prerequisites, in particular the Desktop Development for C++ workload in Visual Studio. For ARM64 development also i
nstall C++ ARM64 build tools. [C:\Users\dahls\source\repos\aspnetcore6-performance\CarvedRock.Aot.Api\CarvedRock.Aot.Ap
i.csproj]
```