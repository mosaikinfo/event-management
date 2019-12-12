## Generate TypeScript API Client

```
dotnet msbuild -target:NSWag
```

## Run Angular Development Server seperately

This ASP.NET Core application will run the Angular Development Server internally
to serve the Angular application.

If you want to run the Angular Development Server with `ng serve` you can disable
this behavior by settings an environment variable.

Set `SPA__Proxy__DevServerBaseUri` to the URL of the Angular application,
usually 'http://localhost:4200'.