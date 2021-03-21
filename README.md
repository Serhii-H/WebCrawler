# Web Crawler

Hi there. This is a repo of one test project the main purpose of which is to crawl web pages and count the occurrence of a particular expression on them.

The App itself is SPA which built based on .net core template (dotnet new angular -o <output_directory_name> -au Individual) in order to don't waste time on adding all the basic stuff including authentication, authorization and so on. There is Angular on FE, and .net core on BE.

In order to launch the App locally you need:
1) Provide a valid connections string in appsettings.json
2) Create DB based on existing migrations (open Package Manager Console and run Update-Database)
3) After the App is up and running - register an account and go ahead with crawling :)
