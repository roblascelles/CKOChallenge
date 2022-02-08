# Design & approach


![flow](flow.png)

Miro board: https://miro.com/app/board/uXjVOP8QxT8=/?invite_link_id=60414425877



### Assumptions

* Events are OK to be stored with full PCI data (PAN/CVV etc.) unencrypted!! 

### Productionise
* use actual database (for projection)
* use an real event-store
* use actual message bus
* merchants & API Keys retrieved from somewhere external
* structured logging
* metrics - e.g: stats for
  * request times & counts
  * response codes
  * errors
* tracing
* tokenisation

### What cloud technologies you’d use and why. 