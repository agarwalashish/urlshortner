# URL shortening service
This is the code for a url shortening service (similar to bit.ly). The service also ensures that users are not trying to mask phishing websites by checking if each submitted url exists in the phish tank (phishtank.com) database.

**NOTE:** At the moment, the service only checks to see if the url is a known phish or not.

## Technical Design
![Is Phish Technical Design](https://github.com/agarwalashish/urlshortner/blob/master/TechnicalDesign.png?raw=true)

## API Contract
There is only one supported endpoint -

1. POST /shorten

Request Body
```javascript
{
   "LongUrl" : ""
}
```

## Security 

There are some basic security measures put in place to protect the API endpoints

### Is a phish ?

The service has an Azure function that runs on the top of every hour and refreshes the list of known phish websites from phishtank.com. This list of websites will be stored in a CosmosDB NoSQL database. The index of each document is set to the host of each known phish.

When a request is made to shorten a URL, the service will check to see if the host of the request exists in the phish database. If it does exist, then the service will return a 400 exception to the user. Otherwise the request will proceed as expected.

### Rate throttling 

IP based rate throttling has been implemented to ensure that individual users do not bring down the service. The rules are set in the configuration files and can be easily modified based on requirements. The rate throttling is based on the `AspNetCoreRateLimit` nuget package. 

The default rate limits are set as follows -

1. 2 requests per second
2. 100 requests per 15 mins
3. 1000 requests per 12 hrs

## System dependencies
1. .NET core 
2. Visual studio
3. Cosmos DB Emulator (2.0 or above)
4. Windows Storage emulator (5.7 or above)


