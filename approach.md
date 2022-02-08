# Design & approach

## Assumptions

* It's OK to store full-on PCI data (full PAN/CVV/Expiry/CardHolderName) in our datastore for now - will need to come up an approach to secure that later (e.g. encryption, or tokenisation)
* The acquiring bank doesn't have to be out-of-process - so another API accessible over HTTP (it can just be a simple class for now).
* We can ignore any GDPR concerns for now.
* We’ll ignore any payment authentication concerns (i.e 3DS) - so the response from the acquiring bank will either be authorised, or declined (and maybe processing errors).
* We could support multiple acquiring banks, but it seems prudent to just have a single bank initially.
  

## Design considerations

* Our payment gateway will need to be accessed by multiple merchants, so putting a JSON API on the internet, seems like an obvious decision.
* Merchants need to know whether payments are successful or not. Possibly we could implement this asynchronously - e.g. by putting payment requests on a queue (returning 202 Accepted to the merchant); then notifying the merchant of the eventual result via webhooks - however, this will add complexity for our merchants, and seems against the purpose (which is to make processing payments easy for merchants).
* Merchants need to be individually authenticated, when both processing payments & retrieving details - an API key issued per merchant seems a suitable approach - probaly this will be sent in a header (over HTTPS).
* Use OpenAPI to auto generate document our API internally.
* Seems sensible to try and separate reads from writes - they will have differing requirements, so using CQRS & allowing for different datastores seems sensible - and will improve scalability.
* With critical financial data, we could investigate an event-sourcing approach - this is where we store domain events in an append-only log (event-store); the application’s state is determined by those stored events;  this also works well with the CQRS approach (I've also never had an excuse to use event-sourcing - so will be a good learning experience!).
* Follow clean architecture principles (equivalent to hexagonal/onion).
* Try to use TDD - this combined with clean architecture, will mean the tests target the application (mocking out dependencies); increasing development speed overall.
* .NET 6 is the latest .NET tech & has LTS - build the API in that & our application libraries in .NET Standard 2.1 to maximise potential re-use


## Implementation

Clean architecture: -- add this -- 

TDD:  -- add this -- 


### CQRS/event-sourcing flow 

Event-sourcing approach gleaned from:
* https://github.com/gregoryyoung/m-r  
* http://www.andreavallotti.tech/en/2018/01/event-sourcing-and-cqrs-in-c/ https://github.com/VenomAV/EventSourcingCQRS



CQRS/event-sourcing flow & responsibilities:
![flow](flow.png)

From [Miro board](https://miro.com/app/board/uXjVOP8QxT8=/?invite_link_id=60414425877)



### Areas for Improvement
* investigate tokenisation/encryption options for PCI data
* add another Web API for the fake-aquirer - so communicating over HTTP.
* use real datastores (for projection & events)
* use an actual message bus
* merchants & API Keys retrieved from somewhere external
* add structured logging
* add metrics - e.g: stats for
  * request times & counts
  * response codes
  * errors
* tracing (look into x-ray, if hosting on AWS)

Also, merchants will require API documentation - which will need to be much more detailed, and hand-holding, than OpenAPI/Swagger - maybe see if there's a [Hugo](https://gohugo.io/) template suitable.

### Hosting/Infrastructure options

The API could be hosted in a container - AWS has options for that (ECS/EKS).  The way the code is structured, it would also be easy to move the command + query handlers into AWS Lambda & use API gateway.

DynamoDB, or any other no-SQL seems a good fit for the projection datastore. 

For the event-store, we could look into Greg Young's [EventStore](https://www.eventstore.com/eventstoredb); possibly dynamoDB could be an option too.

Message bus could be implemented with AWS SNS+SQS.

I'm sure that other cloud providers have equally suitable offerings - just I'm more familiar with the AWS options.