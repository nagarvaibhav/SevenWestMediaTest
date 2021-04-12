# SevenWestMediaTest.

### What is this repository for? ###
This repo if for Seven West Media Technical Test.It is created in .netcore 2.2. It uses Nunit and NSubstitute for UnitTesting and Nlog for logging.

### How do I get set up? ###
This is a ASP.NET core application.The solution has multiple projects.
- SevenWestTest.App (Console App)
- SevenWestTest.Api (API)
- SevenWestTest.Dto (Common DTO)
- SevenWestTest.Api.Tests (Unit tests for API)

The solution is set to run the API and Console app together so just download and run the solution id VS.


### Considerations ###
1.	The data source may change.

    **I implemented generic Formatter for formatting the data as per source. And the data provider is returning string always.**
2.	The endpoint could go down.

    **Proper exception handling is at place and implemented cache. Based on business requirements, We can keep the cache lifetime. So that even the api is down we can read the data from cache.**
3.	The endpoint has known to be slow in the past.

    **Caching is implemented. And all the functions are async.**
4.	The way source is fetched may change.

    **I have implemented data providers currently it is reading from an API by making http request. We may at any time implement and replace this with filedataprovider for ex.**
5.	The number of records may change (performance).

    **All the functions  are async and we can fetch the data in parallel. Fetching records based on page number for ex.**
6.	The functionality may not always be consumed in a console app.

    **I have created a WEB API which can be consumed in any platform.**

### Assumptions and Improvements ###
1. Currently the api has only one end point and it can spilt up in multiple endpoints like getuserbyid/getusersbyage.
2. The supplied sampletest endpoint only has one endpoint which returns all the user and there is not paging support in that api.